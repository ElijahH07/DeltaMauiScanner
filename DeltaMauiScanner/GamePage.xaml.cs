using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DeltaMauiScanner.ScannerConfigurations;
using Com.Zebra.Barcode.Sdk;

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

        private void OnCounterClicked(object sender, EventArgs e)
        {
            // Ensure the method ShowPopup is available in your context.
            // this.ShowPopup(new PopupPage());
        }

        public void SetTextForPoints(string myText)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PointTracker.Text = "TOTAL: " + myText;
            });
        }
    }
}
