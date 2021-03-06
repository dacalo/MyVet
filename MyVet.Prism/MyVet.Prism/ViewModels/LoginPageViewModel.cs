﻿using Acr.UserDialogs;
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
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isEnabled;
        private bool _isRemember;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base (navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Login;
            IsEnabled = true;
            IsRemember = true;
            //TODO: Delete this lines
            Email = "divadchl@hotmail.com";
            Password = "123456";
        }

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRemember
        {
            get => _isRemember;
            set => SetProperty(ref _isRemember, value);
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPassword));

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return;
            }

            if(string.IsNullOrEmpty(Password)){
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return;
            }

            UserDialogs.Instance.ShowLoading(Languages.Loading);
            IsEnabled = false;

            if (!_apiService.CheckConnection())
            {
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Connection, Languages.Accept);
                return;
            }

            var request = new TokenRequest
            {
                Username = Email,
                Password = Password
            };

            var response = await _apiService.GetTokenAsync(Constants.URL_API, "/Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }

            var token = response.Result;

            var response2 = await _apiService.GetOwnerByEmailAsync(
                Constants.URL_API,
                Constants.PREFIX,
                "Owners/GetOwnerByEmail",
                Constants.TokenType,
                token.Token,
                Email);

            if (!response2.IsSuccess)
            {
                IsEnabled = true;
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorUser, Languages.Accept);
                return;
            }

            var owner = response2.Result;
            Settings.Owner = JsonConvert.SerializeObject(owner);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsRemembered = IsRemember;

            IsEnabled = true;
            UserDialogs.Instance.HideLoading();

            Password = string.Empty;
            await _navigationService.NavigateAsync("/VeterinaryMasterDetailPage/NavigationPage/PetsPage");
        }

        private async void Register()
        {
            await _navigationService.NavigateAsync("RegisterPage");
        }

        private async void ForgotPassword()
        {
            await _navigationService.NavigateAsync("RememberPasswordPage");
        }

    }
}
