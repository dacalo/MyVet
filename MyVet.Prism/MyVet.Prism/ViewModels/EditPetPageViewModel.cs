using MyVet.Prism.Helpers;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class EditPetViewModel : ViewModelBase
    {
        public EditPetViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.NewPet;
        }
    }
}
