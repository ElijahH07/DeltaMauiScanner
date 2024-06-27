using CommunityToolkit.Maui.Views;
namespace DeltaMauiScanner;

public partial class PopupPage : Popup
{
	public PopupPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}