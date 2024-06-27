using CommunityToolkit.Maui.Views;

namespace DeltaMauiScanner;

public partial class GamePage : ContentPage
{
    public GamePage()
    {
        InitializeComponent();
        RunThreads();
    }

    private void RunThreads()
    {
        //Thread t1 = new Thread(() =>
        //{
        //    var instance = SharedData.Instance(1,0);
        //});

        //Thread t2 = new Thread(() =>
        //{
        //    var instance = SharedData.Instance(2,0);
        //});

        //t1.Start();
        //t2.Start();

        //t1.Join();
        //t2.Join();

    }
    private void Countmore(object sender, EventArgs e)
    {
        int x = -500;
        var sharedInstance = SharedData.Instance(0, 0); // Pass 0 or any default values if initialization is not needed
        Console.WriteLine($"Current Points: {sharedInstance.Points}");
        sharedInstance.ChangePoints(x); // Set Points to 100
        Console.WriteLine($"New Points: {sharedInstance.Points}");
    }
    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new PopupPage());
    }
}