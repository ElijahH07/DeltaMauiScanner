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
    private TimeSpan _elapsedTime;
    private Timer _timer;
    private bool _isTimerRunning;

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
    public bool AddBarcode(string data)
    {
        // Add a new Barcode object to the collection
        //Barcodes.Add(new Barcode { BData = data });
        var existingBarcode = Barcodes.FirstOrDefault(barcode => barcode.Id.Contains(data[0]));
        if (existingBarcode == null) 
        {
            Barcodes.Add(new Barcode { BData = data.Substring(1), Id = data[0].ToString() });
            return true;
        }
        return false;
    }

    // Implement INotifyPropertyChanged interface
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SetTextForPoints(string myText)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            PointTracker.Text = "TOTAL: " + myText;
        });
            
    }
    public void clearBarcodes()
    {
        Barcodes.Clear();
    }


    // Creating the Timer

    public void onStartClicked(object sender, EventArgs e)
    {
        Globals.totalTime = 15;

        Globals.totalpoints = 0;
        StartGameButton.IsVisible = false; // Make the button disappear when clicked

        if (!_isTimerRunning)
        {
            _elapsedTime = TimeSpan.Zero;
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            _isTimerRunning = true;

            // Automatically stop the timer after 15 seconds
            //Device.StartTimer(TimeSpan.FromSeconds(Globals.totalTime - 1), () =>
            //{
            //    StopTimer();
            //    StartGameButton.IsVisible = true; // Make the button reappear when the timer stops
            //    return false; // Stop recurring timer
            //});
        }
    }


    private void StopTimer()
    {
        _timer.Dispose();
        _isTimerRunning = false;

        // Call your end function here
        endFunction();
    }

    private void TimerCallback(object state)
    {
        _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
        UpdateTimerLabel();
        int totalTime = Globals.totalTime;
        if (_elapsedTime.TotalSeconds == totalTime)
        {
            StopTimer();
        }
    }

    private void UpdateTimerLabel()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            int res = Globals.totalTime - int.Parse(_elapsedTime.ToString(@"ss"));
            countDown.Text = "CountDown: " + res + " s";
        });
    }

    public void endFunction()
    {
        clearBarcodes();
        SetTextForPoints("0");
        Globals.totalTime = 15;

        Device.BeginInvokeOnMainThread(() =>
        {
            countDown.Text = "CountDown: " + Globals.totalTime + " s";

            StartGameButton.IsVisible = true;

            var popup = new PopupPage();

            this.ShowPopup(popup);
        });
        
    }

}

public class Barcode
{
    public string BData { get; set; }
    public string Id { get; set; }
    public string ImageSource { get; set; }
}
