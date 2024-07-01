using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit;
using DeltaMauiScanner.ScannerConfigurations;
using Com.Zebra.Barcode.Sdk;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

using DeltaMauiScanner;

namespace DeltaMauiScanner;

public partial class GamePage : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<Barcode> _barcodes;

    public ObservableCollection<Barcode> Barcodes
    {
        get { return _barcodes; }
        set
        {
            _barcodes = value;
            OnPropertyChanged();
        }
    }

    public GamePage()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);

        // Initialize the ObservableCollection
        Barcodes = new ObservableCollection<Barcode>();

        // Set the BindingContext to this page
        BindingContext = this;
    }

    // Singleton instance (if needed)
    private static GamePage instance;

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

    // Method to add a barcode to the collection
    public void AddBarcode(string data)
    {
        // Add a new Barcode object to the collection
        Barcodes.Add(new Barcode { BData = data });
    }

    // Implement INotifyPropertyChanged interface
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void onPopupClicked(object sender, EventArgs e)
    {
        var popup = new PopupPage();

        this.ShowPopup(popup);

    }

    public void SetTextForPoints(string myText)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            PointTracker.Text = "TOTAL: " + myText;
        });
            
    }
}

public class Barcode
{
    public string BData { get; set; }
}
