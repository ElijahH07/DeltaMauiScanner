using CommunityToolkit.Maui.Views;
namespace DeltaMauiScanner;

public partial class PopupPage : Popup
{
	public PopupPage()
	{
		InitializeComponent();
        //this.Padding = new Thickness(20, 0);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}