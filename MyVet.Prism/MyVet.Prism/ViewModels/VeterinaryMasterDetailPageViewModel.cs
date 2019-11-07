using MyVet.Common.Models;
using MyVet.Prism.Helpers;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
    public class VeterinaryMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public VeterinaryMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_pets_menu",
                    PageName = "PetsPage",
                    Title = Languages.MyPets
                },

                new Menu
                {
                    Icon = "ic_calendar_today",
                    PageName = "AgendaPage",
                    Title = Languages.MyAgenda
                },

                new Menu
                {
                    Icon = "ic_map",
                    PageName = "MapPage",
                    Title = Languages.Map
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ProfilePage",
                    Title = Languages.MyProfile
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = Languages.Logout
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

    }
}
