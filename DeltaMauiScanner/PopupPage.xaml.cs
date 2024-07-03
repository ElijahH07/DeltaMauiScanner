using CommunityToolkit.Maui.Views;
namespace DeltaMauiScanner;

public partial class PopupPage : Popup
{
	public PopupPage()
	{
		InitializeComponent();
        Score.Text = Globals.totalpoints.ToString();
        Highest.Text = "Best Score:"+ Globals.bestscore.ToString();
        //Globals.totalpoints= 0;

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}