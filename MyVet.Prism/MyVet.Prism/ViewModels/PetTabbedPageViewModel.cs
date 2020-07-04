using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class PetTabbedPageViewModel : ViewModelBase
    {
        public PetTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //TODO Borrar var pet = JsonConvert.DeserializeObject<PetResponse>(Settings.Pet);
            Title = Languages.Pet;
        }
    }
}
