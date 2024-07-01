using DeltaMauiScanner.Services;

namespace DeltaMauiScanner;

public partial class Model_Info : ContentPage
{
	public Model_Info()
	{
		InitializeComponent();

        Shell.SetNavBarIsVisible(this, false);

        infoLabel.Text = getInfo();
    }

	private string getInfo()
	{
        return $"Device Model: \n {DeviceInfoService.Model()}\n \n Platform: \n {DeviceInfoService.Platform()}";

    }
}