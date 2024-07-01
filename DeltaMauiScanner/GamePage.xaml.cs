using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit;
using DeltaMauiScanner.ScannerConfigurations;
using Com.Zebra.Barcode.Sdk;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace DeltaMauiScanner
{
    public partial class GamePage : ContentPage, INotifyPropertyChanged
    {
        private static GamePage instance;

        public ObservableCollection<BarcodeScannerFactory> BarcodeScans { get; set; } = new ObservableCollection<BarcodeScannerFactory>();
       

        public GamePage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static GamePage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GamePage();
                }
                return instance;
            }
        }

        private void Countmore(object sender, EventArgs e)
        {
            int total = Globals.totalpoints;
            Console.WriteLine(total);
        }

        private async void onPopupClicked(object sender, EventArgs e)
        {
            //bool ans = await DisplayAlert("Question?", "Would you like to play again?", "yes", "no");
            //Debug.WriteLine("answer: " + ans);
            //await Shell.Current.CurrentPage.ShowPopupAsync(new PopupPage());
            var popup = new PopupPage();

            this.ShowPopup(popup);

        }

        public void SetTextForPoints(string myText)
        {
                PointTracker.Text = "TOTAL: " + myText;
        }
    }
}
