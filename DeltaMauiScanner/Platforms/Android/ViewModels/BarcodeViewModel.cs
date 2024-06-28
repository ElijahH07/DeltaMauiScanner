using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaMauiScanner.Platforms.Android.ViewModels
{
    internal class BarcodeViewModel
    {
        public void displayData(String barcodeData)
        {
            if (barcodeData.Contains("BAD"))
            {
                Console.WriteLine("found bad guy");
            } else if(barcodeData.Contains("GOOD"))
            {
                Console.WriteLine("found good guy");
            }
        }
    }
}
