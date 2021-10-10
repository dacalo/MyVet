using Acr.UserDialogs;
using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Prism.ViewModels
{
    public class AssignModifyAgendaPageViewModel : ViewModelBase
    {
        private AgendaResponse _agenda;
        private PetResponse _pet;
        private ObservableCollection<PetResponse> _pets;
        private bool _isEnabled;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _assignCommand;
        private DelegateCommand _cancelCommand;


        public AssignModifyAgendaPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.AssignModifyAgenda;
            IsEnabled = true;
        }

        public AgendaResponse Agenda
        {
            get => _agenda;
            set => SetProperty(ref _agenda, value);
        }

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public ObservableCollection<PetResponse> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public DelegateCommand AssignCommand => _assignCommand ?? (_assignCommand = new DelegateCommand(Assign));

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Agenda"))
            {
                Agenda = parameters.GetValue<AgendaResponse>("Agenda");
                LoadPets();
            }
        }

        private void LoadPets()
        {
            OwnerResponse owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            Pets = new ObservableCollection<PetResponse>(owner.Pets);
            Pet = Pets.FirstOrDefault(p => p.Id == _agenda.Pet.Id);
        }

        private async void Assign()
        {
            bool isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Loading);
            IsEnabled = false;

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            OwnerResponse owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            AssignRequest request = new AssignRequest
            {
                AgendaId = Agenda.Id,
                OwnerId = owner.Id,
                PetId = Pet.Id,
                Remarks = Agenda.Remarks
            };

            Response<object> response = await _apiService.PostAsync(Constants.URL_BASE, Constants.PREFIX, "Agenda/AssignAgenda", request, Constants.TokenType, token.Token);

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "Refresh", true }
            };

            await _navigationService.GoBackAsync(parameters);
        }

        private async Task<bool> ValidateData()
        {
            if (Pet == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PetError, Languages.Accept);
                return false;
            }

            return true;
        }

        private async void Cancel()
        {
            bool answer = await App.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.CancelAgendaMessage,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Loading);
            IsEnabled = false;

            UnAssignRequest request = new UnAssignRequest { AgendaId = Agenda.Id };
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response<object> response = await _apiService.PostAsync(Constants.URL_BASE, Constants.PREFIX, "Agenda/UnAssignAgenda", request, Constants.TokenType, token.Token);

            UserDialogs.Instance.HideLoading();
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "Refresh", true }
            };

            await _navigationService.GoBackAsync(parameters);
        }

    }
}
