namespace DeltaMauiScanner.ScannerConfigurations;

/// <summary>
/// ScannerConfiguration is where all the setting up of the barcodes is done and also where the data is retreived. 
/// setUpRfid points to rfidConfiguration in Platforms/Android/Models and is not implemented on other devices yet
/// setUpBarcode points to barcodeConfiguration in Platforms/Android/Models and is also not implemented elsewhere yet
/// </summary>
public partial class ScannerConfiguration
{
    public partial void setUpRfid();
    public partial void setUpBarcode();
    public partial void disconnectScanner();
}
