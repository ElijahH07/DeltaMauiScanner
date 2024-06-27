using DeltaMauiScanner.Services;

namespace DeltaMauiScanner;

public partial class Model_Info : ContentPage
{
	public Model_Info()
	{
		InitializeComponent();

        infoLabel.Text = getInfo();
    }

	private string getInfo()
	{
        return $"Device Model: {DeviceInfoService.Model()}\nPlatform: {DeviceInfoService.Platform()}";

    }
}