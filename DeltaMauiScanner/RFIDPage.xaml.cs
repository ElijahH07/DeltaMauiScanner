using System.Collections.ObjectModel;
using DeltaMauiScanner.ScannerConfigurations;

namespace DeltaMauiScanner;

public partial class RFIDPage : ContentPage
{
    ScannerConfiguration config = new ScannerConfiguration();

    private static RFIDPage instance;
    public ObservableCollection<RFIDTag> RFIDTags { get; set; }
    private RFIDPage() { 
        
        InitializeComponent();

        RFIDTags = new ObservableCollection<RFIDTag>
            {
                //simply just makes the collection for rfidtags
            };

        // Set the binding context to the current instance
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

    private void OnClearRFIDTagsButtonClick(object sender, EventArgs e)
    {
        // Clear all RFID tags from the collection
        RFIDTags.Clear();
    }

    private async void OnButtonClick(object sender, EventArgs e)
    {
        config.setUpRfid();

    }

    
}

// Define the RFIDTag class
public class RFIDTag
{
    public string TagId { get; set; }
    public int Count { get; set; }
    public string OriginalTagId { get; set; } // This will store the original tag ID
}