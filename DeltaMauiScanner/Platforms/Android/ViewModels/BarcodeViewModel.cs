using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaMauiScanner.Platforms.Android.ViewModels
{
    public class BarcodeViewModel
    {
        public void displayData(string barcodeData)
        {
            var gamepage = GamePage.Instance;

            if (gamepage == null)
            {
                Console.WriteLine("GamePage instance is null");
                return;
            }

            if (gamepage.AddBarcode(barcodeData))
            {
                if (barcodeData.Contains("BAD"))
                {
                    Globals.totalpoints += 50;
                }
                else if (barcodeData.Contains("GOOD"))
                {
                    Globals.totalpoints -= 20;
                }
                else
                {
                    Globals.totaltime += 5;
                    Console.WriteLine("power up");
                }

                gamepage.SetTextForPoints(Globals.totalpoints.ToString());
            }
        }
    }
}
