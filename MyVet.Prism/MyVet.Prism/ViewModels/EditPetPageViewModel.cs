﻿using Acr.UserDialogs;
using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyVet.Prism.ViewModels
{
    public class EditPetPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private PetResponse _pet;
        private ImageSource _imageSource;
        private bool _isEnabled;
        private bool _isEdit;
        private ObservableCollection<PetTypeResponse> _petTypes;
        private PetTypeResponse _petType;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _deleteCommand;

        public EditPetPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ObservableCollection<PetTypeResponse> PetTypes
        {
            get => _petTypes;
            set => SetProperty(ref _petTypes, value);
        }

        public PetTypeResponse PetType
        {
            get => _petType;
            set => SetProperty(ref _petType, value);
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(DeleteAsync));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("pet"))
            {
                Pet = parameters.GetValue<PetResponse>("pet");
                ImageSource = Pet.ImageUrl;
                IsEdit = true;
                Title = Languages.EditPet;
            }
            else
            {
                Pet = new PetResponse { Born = DateTime.Today };
                ImageSource = "noimage";
                IsEdit = false;
                Title = Languages.NewPet;
            }

            LoadPetTypesAsync();
        }

        private async void LoadPetTypesAsync()
        {
            IsEnabled = false;
            UserDialogs.Instance.ShowLoading(Languages.Loading);

            if (!_apiService.CheckConnection())
            {
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Connection, Languages.Accept);
                await _navigationService.GoBackAsync();
                return;
            }
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response<object> response = await _apiService.GetListAsync<PetTypeResponse>(Constants.URL_BASE, Constants.PREFIX, "PetTypes", Constants.TokenType, token.Token);

            IsEnabled = true;
            UserDialogs.Instance.HideLoading();

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                await _navigationService.GoBackAsync();
                return;
            }

            List<PetTypeResponse> petTypes = (List<PetTypeResponse>)response.Result;
            PetTypes = new ObservableCollection<PetTypeResponse>(petTypes);

            if (!string.IsNullOrEmpty(Pet.PetType))
            {
                PetType = PetTypes.FirstOrDefault(pt => pt.Name == Pet.PetType);
            }
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.PictureSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.FromCamera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            if (source == Languages.FromCamera)
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void SaveAsync()
        {
            if (!await ValidateData())
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Saving);
            IsEnabled = false;

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            OwnerResponse owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = FilesHelper.ReadFully(_file.GetStream());
            }

            PetRequest petRequest = new PetRequest
            {
                Born = Pet.Born,
                Id = Pet.Id,
                ImageArray = imageArray,
                Name = Pet.Name,
                OwnerId = owner.Id,
                PetTypeId = PetType.Id,
                Race = Pet.Race,
                Remarks = Pet.Remarks
            };

            Response<object> response;
            if (IsEdit)
            {
                response = await _apiService.PutAsync(Constants.URL_BASE, Constants.PREFIX, "Pets", petRequest.Id, petRequest, Constants.TokenType, token.Token);
            }
            else
            {
                response = await _apiService.PostAsync(Constants.URL_BASE, Constants.PREFIX, "Pets", petRequest, Constants.TokenType, token.Token);
            }

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                UserDialogs.Instance.HideLoading();
                IsEnabled = true;

                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await PetsPageViewModel.GetInstance().UpdateOwnerAsync();

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;

            await App.Current.MainPage.DisplayAlert(
                Languages.Ok,
                string.Format(Languages.CreateEditPetConfirm, IsEdit ? Languages.Edited : Languages.Created),
                Languages.Accept);

            await _navigationService.GoBackToRootAsync();

        }

        private async void DeleteAsync()
        {
            bool answer = await App.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.QuestionToDeletePet,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Deleting);
            IsEnabled = false;

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response<object> response = await _apiService.DeleteAsync(Constants.URL_BASE, Constants.PREFIX, "Pets", Pet.Id, Constants.TokenType, token.Token);

            if (!response.IsSuccess)
            {
                UserDialogs.Instance.HideLoading();
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await PetsPageViewModel.GetInstance().UpdateOwnerAsync();

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;
            await _navigationService.GoBackToRootAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Pet.Name))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Pet.Race))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.RaceError, Languages.Accept);
                return false;
            }

            if (PetType == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PetTypeError, Languages.Accept);
                return false;
            }

            return true;
        }

    }
}
