﻿using DeltaMauiScanner.ScannerConfigurations;

namespace DeltaMauiScanner
{
    public partial class MainPage : ContentPage
    {
        ScannerConfiguration config = new ScannerConfiguration();

        public MainPage()
        {
            InitializeComponent();

        }

        private async void OnModelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Model_Info());
        }

        private async void OnRFIDButtonClick(object sender, EventArgs e)
        {
            config.disconnectScanner();
            config.setUpRfid();
            var rfidPageInstance = RFIDPage.Instance;
            Navigation.PushAsync(rfidPageInstance);
        }

        private async void OnGameButtonClick(object sender, EventArgs e)
        {
            config.disconnectRfid();
            config.setUpBarcode();
            var gamePageInstance = GamePage.Instance;
            Navigation.PushAsync(gamePageInstance);
        }

    }

}
