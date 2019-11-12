using MyVet.Common.Helpers;
using MyVet.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class AgendaItemViewModel : AgendaResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectAgendaCommand;

        public AgendaItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectAgendaCommand => _selectAgendaCommand ?? (_selectAgendaCommand = new DelegateCommand(SelectAgenda));

        private async void SelectAgenda()
        {
            var owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            if (!IsAvailable && Owner.Id != owner.Id)
            {
                return;
            }

            var parameters = new NavigationParameters
            {
                { "Agenda", this }
            };

            await _navigationService.NavigateAsync("AssignModifyAgenda", parameters);
        }
    }
}
