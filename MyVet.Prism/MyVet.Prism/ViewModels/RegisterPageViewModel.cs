using DryIoc;
using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace MyVet.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;
        private readonly IGeolocatorService _geolocatorService;
        private Position _position;
        private string _address;


        public RegisterPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IGeolocatorService geolocatorService) : base(navigationService)
        {
            Title = Languages.RegisterNewUser;
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
            _geolocatorService = geolocatorService;
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));

        public string RFC { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Register()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            var request = new UserRequest
            {
                Address = Address,
                RFC = RFC,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Phone = Phone,
                Latitude = _position.Latitude,
                Longitude = _position.Longitude
            };

            var response = await _apiService.RegisterUserAsync(
                Constants.URL_API,
                Constants.PREFIX,
                "/Account",
                request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(
                Languages.Ok,
                response.Message,
                Languages.Accept);
            await _navigationService.GoBackAsync();

        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(RFC))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorRFC, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorFirstName, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorLastName, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorLastName, Languages.Accept);
                return false;
            }

            //var isValidAddress = await ValidateAddressAsync();
            //if (!isValidAddress)
            //{
            //    return false;
            //}


            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorEmail, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorPhone, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorPassword, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorPasswordConfirm, Languages.Accept);
                return false;
            }

            if (!Password.Equals(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorPasswordConfirm2, Languages.Accept);
                return false;
            }

            return true;
        }

        //TODO Borrar
        //private async Task<bool> ValidateAddressAsync()
        //{
        //    var geoCoder = new Geocoder();
        //    var locations = await geoCoder.GetPositionsForAddressAsync(Address);
        //    var locationList = locations.ToList();
        //    if (locationList.Count == 0)
        //    {
        //        var response = await App.Current.MainPage.DisplayAlert(
        //            Languages.Error,
        //            Languages.NotAddressFound,
        //            Languages.Yes,
        //            Languages.No);
        //        if (response)
        //        {
        //            await _geolocatorService.GetLocationAsync();
        //            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
        //            {
        //                _position = new Position(
        //                    _geolocatorService.Latitude,
        //                    _geolocatorService.Longitude);

        //                var list = await geoCoder.GetAddressesForPositionAsync(_position);
        //                Address = list.FirstOrDefault();
        //                return true;
        //            }
        //            else
        //            {
        //                await App.Current.MainPage.DisplayAlert(
        //                    Languages.Error,
        //                    Languages.NotLocationAvailable,
        //                    Languages.Accept);
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }

        //    if (locationList.Count == 1)
        //    {
        //        _position = locationList.FirstOrDefault();
        //        return true;
        //    }

        //    if (locationList.Count > 1)
        //    {
        //        var addresses = new List<Address>();
        //        var names = new List<string>();
        //        foreach (var location in locationList)
        //        {
        //            var list = await geoCoder.GetAddressesForPositionAsync(location);
        //            names.AddRange(list);
        //            foreach (var item in list)
        //            {
        //                addresses.Add(new Address
        //                {
        //                    Name = item,
        //                    Latitude = location.Latitude,
        //                    Longitude = location.Longitude
        //                });
        //            }
        //        }

        //        var source = await App.Current.MainPage.DisplayActionSheet(
        //            Languages.SelectAnAdrress,
        //            Languages.Cancel,
        //            null,
        //            names.ToArray());
        //        if (source == Languages.Cancel)
        //        {
        //            return false;
        //        }

        //        Address = source;
        //        var address = addresses.FirstOrDefault(a => a.Name == source);
        //        _position = new Position(address.Latitude, address.Longitude);
        //    }

        //    return true;
        //}

    }
}
