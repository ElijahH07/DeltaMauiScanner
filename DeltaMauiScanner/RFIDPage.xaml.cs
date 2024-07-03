using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DeltaMauiScanner.ScannerConfigurations;

namespace DeltaMauiScanner;

public partial class RFIDPage : ContentPage, INotifyPropertyChanged
{

    ScannerConfiguration config = new ScannerConfiguration();

    private static RFIDPage instance;
    public ObservableCollection<RFIDTag> RFIDTags { get; set; }



    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public RFIDPage()
    {
        InitializeComponent();
        RFIDTags = new ObservableCollection<RFIDTag> { };
        BindingContext = this;
    }

    public static RFIDPage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RFIDPage();
            }
            return instance;
        }
    }

    public void SetTextForLabel(string mytext)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            counttotal.Text = "TOTAL: " + mytext;
        });
    }

    private void OnClearRFIDTagsButtonClick(object sender, EventArgs e)
    {
        int total = Globals.totalecount;
        Console.WriteLine(total);
        total = 0;
        RFIDTags.Clear();
        Globals.totalecount = total;
        Console.WriteLine(total);
        Console.WriteLine(Globals.totalecount);
        SetTextForLabel(total.ToString());
        OnPropertyChanged(nameof(total));
    }

    //reconnect rfid scanner button
    private async void OnButtonClick(object sender, EventArgs e)
    {
        config.setUpRfid();
    }
}

// Define the RFIDTag class
public class RFIDTag
{
    public string Id { get; set; }
    public int Count { get; set; }
    public string OriginalTagId { get; set; }
}