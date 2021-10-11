using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Maps;

namespace MyVet.Prism.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isEnabled;
        private Position _position;
        private OwnerResponse _owner;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public ProfilePageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IGeolocatorService geolocatorService) : base(navigationService)
        {
            Title = Languages.MyProfile;
            IsEnabled = true;
            Owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            _geolocatorService = geolocatorService;
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePassword));


        public OwnerResponse Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Save()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Loading);
            IsEnabled = false;

            var userRequest = new UserRequest
            {
                Address = Owner.Address,
                RFC = Owner.Document,
                Email = Owner.Email,
                FirstName = Owner.FirstName,
                LastName = Owner.LastName,
                Password = "123456", // It doesn't matter what is sent here. It is only for the model to be valid
                Phone = Owner.PhoneNumber,
                Latitude = _position.Latitude,
                Longitude = _position.Longitude
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.PutAsync(
                Constants.URL_BASE,
                Constants.PREFIX,
                "Account",
                userRequest,
                Constants.TokenType,
                token.Token);

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            Settings.Owner = JsonConvert.SerializeObject(Owner);

            await App.Current.MainPage.DisplayAlert(
                Languages.Ok,
                Languages.UserUpdated,
                Languages.Accept);
            await _navigationService.GoBackAsync();

        }

        private async void ChangePassword()
        {
            await _navigationService.NavigateAsync("ChangePasswordPage");
        }


        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Owner.Document))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DocumentError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Accept);
                return false;
            }

            var isValidAddress = await ValidateAddressAsync();
            if (!isValidAddress)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateAddressAsync()
        {
            var geoCoder = new Geocoder();
            var locations = await geoCoder.GetPositionsForAddressAsync(Owner.Address);
            var locationList = locations.ToList();
            if (locationList.Count == 0)
            {
                var response = await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NotAddressFound,
                    Languages.Yes,
                    Languages.No);
                if (response)
                {
                    await _geolocatorService.GetLocationAsync();
                    if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                    {
                        _position = new Position(
                            _geolocatorService.Latitude,
                            _geolocatorService.Longitude);

                        var list = await geoCoder.GetAddressesForPositionAsync(_position);
                        Owner.Address = list.FirstOrDefault();
                        return true;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            Languages.Error,
                            Languages.NotLocationAvailable,
                            Languages.Accept);
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            if (locationList.Count == 1)
            {
                _position = locationList.FirstOrDefault();
                return true;
            }

            if (locationList.Count > 1)
            {
                var addresses = new List<Address>();
                var names = new List<string>();
                foreach (var location in locationList)
                {
                    var list = await geoCoder.GetAddressesForPositionAsync(location);
                    names.AddRange(list);
                    foreach (var item in list)
                    {
                        addresses.Add(new Address
                        {
                            Name = item,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        });
                    }
                }

                var source = await App.Current.MainPage.DisplayActionSheet(
                    Languages.SelectAnAdrress,
                    Languages.Cancel,
                    null,
                    names.ToArray());
                if (source == Languages.Cancel)
                {
                    return false;
                }

                Owner.Address = source;
                var address = addresses.FirstOrDefault(a => a.Name == source);
                _position = new Position(address.Latitude, address.Longitude);
            }

            return true;
        }

    }
}
