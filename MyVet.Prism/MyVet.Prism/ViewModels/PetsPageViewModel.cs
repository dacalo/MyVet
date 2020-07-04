using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private OwnerResponse _owner;
        private ObservableCollection<PetItemViewModel> _pets;
        private DelegateCommand _addPetCommand;
        private static PetsPageViewModel _instance;
        private DelegateCommand _refreshPetsCommand;
        private bool _isRefreshing;

        public PetsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Pets;
            LoadOwner();
        }

        public ObservableCollection<PetItemViewModel> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public DelegateCommand AddPetCommand => _addPetCommand ?? (_addPetCommand = new DelegateCommand(AddPetAsync));
        public DelegateCommand RefreshPetsCommand => _refreshPetsCommand ?? (_refreshPetsCommand = new DelegateCommand(RefreshPetsAsync));

        public static PetsPageViewModel GetInstance()
        {
            return _instance;
        }

        private void LoadOwner()
        {
            _owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            Title = $"{Languages.PetsOf} {_owner.FullName}";
            Pets = new ObservableCollection<PetItemViewModel>(_owner.Pets.Select(p => new PetItemViewModel(_navigationService)
            {
                Born = p.Born,
                Histories = p.Histories,
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                PetType = p.PetType,
                Race = p.Race,
                Remarks = p.Remarks
            }).ToList());
        }

        private async void AddPetAsync()
        {
            await _navigationService.NavigateAsync("EditPetPage");
        }

        public async Task UpdateOwnerAsync()
        {
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetOwnerByEmailAsync(
                Constants.URL_API,
                Constants.PREFIX,
                "Owners/GetOwnerByEmail",
                Constants.TokenType,
                token.Token,
                _owner.Email);

            if (response.IsSuccess)
            {
                var owner = (OwnerResponse)response.Result;
                Settings.Owner = JsonConvert.SerializeObject(owner);
                _owner = owner;
                LoadOwner();
            }
        }

        private async void RefreshPetsAsync()
        {
            IsRefreshing = true;
            await UpdateOwnerAsync();
            IsRefreshing = false;
        }

    }
}
