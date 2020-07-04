using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class AgendaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<AgendaItemViewModel> _agenda;
        private bool _isRefreshing;
        private DelegateCommand _refreshPetsCommand;

        public AgendaPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = Languages.Diary;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadAgenda();
        }

        public DelegateCommand RefreshPetsCommand => _refreshPetsCommand ?? (_refreshPetsCommand = new DelegateCommand(LoadAgenda));

        public ObservableCollection<AgendaItemViewModel> Agenda
        {
            get => _agenda;
            set => SetProperty(ref _agenda, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Refresh"))
            {
                LoadAgenda();
            }
        }

        private async void LoadAgenda()
        {
            IsRefreshing = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            var response = await _apiService.GetAgendaForOwner(Constants.URL_API, Constants.PREFIX, "/Agenda/GetAgendaForOwner", owner.Email, Constants.TokenType, token.Token);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var myAgenda = (List<AgendaResponse>)response.Result;
            Agenda = new ObservableCollection<AgendaItemViewModel>(myAgenda.Select(a => new AgendaItemViewModel(_navigationService)
            {
                Date = a.Date,
                Id = a.Id,
                IsAvailable = a.IsAvailable,
                Owner = a.Owner,
                Pet = a.Pet,
                Remarks = a.Remarks
            }).ToList());

            IsRefreshing = false;
        }

    }
}
