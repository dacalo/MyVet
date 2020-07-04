using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class PetPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PetResponse _pet;
        private DelegateCommand _editPetCommand;

        public PetPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Details;
            _navigationService = navigationService;
        }

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public DelegateCommand EditPetCommand => _editPetCommand ?? (_editPetCommand = new DelegateCommand(EditPetAsync));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
                        
            Pet = JsonConvert.DeserializeObject<PetResponse>(Settings.Pet);
        }

        private async void EditPetAsync()
        {
            var parameters = new NavigationParameters
            {
                { "pet", Pet }
            };

            await _navigationService.NavigateAsync("EditPetPage", parameters);
        }

    }
}
