﻿using System;
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

            gamepage.AddBarcode(barcodeData);


            Debug.WriteLine("adding to list");

            if (barcodeData.Contains("BAD"))
            {
                Globals.totalpoints += 50;
                Console.WriteLine("found bad guy");
            } 
            else if(barcodeData.Contains("GOOD"))
            {
                Globals.totalpoints -= 20;
                Console.WriteLine("found good guy");
            }
            else
            {
                Console.WriteLine("power up");
            }

            gamepage.SetTextForPoints(Globals.totalpoints.ToString());
        }
    }
}
