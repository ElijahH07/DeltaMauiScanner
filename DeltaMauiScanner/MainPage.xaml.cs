using System.Collections.ObjectModel;
using DeltaMauiScanner.Services;
using DeltaMauiScanner.ScannerConfigurations;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace DeltaMauiScanner
{
    public partial class MainPage : ContentPage
    {
        ScannerConfiguration config = new ScannerConfiguration();

        public MainPage()
        {
            InitializeComponent();

            
            //config.setUpRfid();
            //config.setUpBarcode();

        }

        private async void OnModelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Model_Info());
        }

        private async void OnRFIDButtonClick(object sender, EventArgs e)
        {
            config.setUpRfid();
            var rfidPageInstance = RFIDPage.Instance;
            Navigation.PushAsync(rfidPageInstance);
        }

        private async void OnGameButtonClick(object sender, EventArgs e)
        {
            config.setUpBarcode();
            var gamePageInstance = GamePage.Instance;
            Navigation.PushAsync(gamePageInstance);
        }

    }

}
