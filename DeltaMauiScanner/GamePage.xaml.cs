using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Views;

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


    // when start game button is clicked start the timer

    public void onStartClicked(object sender, EventArgs e)
    {
        Globals.gameRunning = true;

        StartGameButton.IsVisible = false; // Make the button disappear when clicked

        if (!_isTimerRunning)
        {
            _elapsedTime = TimeSpan.Zero;
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            _isTimerRunning = true;
        }
    }


    private void StopTimer()
    {
        _timer.Dispose();
        _isTimerRunning = false;

        endFunction();
    }

    // keep the timer going until it is the length of Globals.totalTime 
    private void TimerCallback(object state)
    {
        _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
        UpdateTimerLabel();
        if (_elapsedTime.TotalSeconds == Globals.totalTime)
        {
            StopTimer();
        }
    }

    //update the view so that the timer looks lik it is shrinking
    private void UpdateTimerLabel()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            int res = Globals.totalTime - int.Parse(_elapsedTime.ToString(@"ss"));
            countDown.Text = "CountDown: " + res + " s";
        });
    }


    // timer is finished so the game is finished, clear the data and reset the View 
    public void endFunction()
    {
        clearBarcodes();
        SetTextForPoints("0");
        Globals.totalTime = 15;
        Globals.gameRunning = false;
        Globals.totalpoints = 0;

        //change best score if the player got a new record
        if (Globals.totalpoints > Globals.bestScore)
        {
            Globals.bestScore = Globals.totalpoints;
        }

        //reset the View + start popup to display result
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
