using MyVet.Common.Business;
using MyVet.Common.Helpers;
using MyVet.Common.Models;
using MyVet.Common.Services;
using MyVet.Prism.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyVet.Prism.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly IApiService _apiService;

        public MapPage(
            IGeolocatorService geolocatorService,
            IApiService apiService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            _apiService = apiService;
            _ = ShowOwnersAsync();
            _ = MoveMapToCurrentPositionAsync();
        }

        private async Task MoveMapToCurrentPositionAsync()
        {
            await _geolocatorService.GetLocationAsync();

            Position position = new Position(
                _geolocatorService.Latitude,
                _geolocatorService.Longitude);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                position,
                Distance.FromKilometers(.5)));
        }

        private async Task ShowOwnersAsync()
        {
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response<object> response = await _apiService.GetListAsync<OwnerResponse>(
                Constants.URL_BASE, 
                Constants.PREFIX, 
                "Owners", 
                Constants.TokenType, 
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            List<OwnerResponse> owners = (List<OwnerResponse>)response.Result;

            foreach (OwnerResponse owner in owners)
            {
                MyMap.Pins.Add(new Pin
                {
                    Address = owner.Address,
                    Label = owner.FullName,
                    Position = new Position(owner.Latitude, owner.Longitude),
                    Type = PinType.Place
                });
            }
        }

    }
}
