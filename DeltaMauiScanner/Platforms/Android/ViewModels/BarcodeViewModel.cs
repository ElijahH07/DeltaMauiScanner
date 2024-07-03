namespace DeltaMauiScanner.Platforms.Android.ViewModels
{
    public class BarcodeViewModel
    {
        GamePage gamepage = GamePage.Instance;
        public void displayData(string barcodeData)
        {

            if (gamepage == null)
            {
                Console.WriteLine("GamePage instance is null");
                return;
            }

            if (addBarcode(barcodeData))
            {
                if (barcodeData.Contains("BAD"))
                {
                    Globals.totalpoints += 50;
                }
                else if (barcodeData.Contains("GOOD"))
                {
                    Globals.totalpoints -= 20;
                }
                else if (barcodeData.Contains("POWER")) {
                    Globals.totalTime += 5;
                }
                else
                {
                    Console.WriteLine("implement some other powerup");
                }

                gamepage.SetTextForPoints(Globals.totalpoints.ToString());
            }
        }

        // Method to add a barcode to the collection
        public bool addBarcode(string data)
        {
            // Add a new Barcode object to the collection
            //Barcodes.Add(new Barcode { BData = data });
            if (Globals.gameRunning)
            {
                //for the barcodes make sure the first character in the barcode data is unique
                //ie: "aBAD GUY ! 50 POINTS !!" would display on the device "BAD GUY ! 50 POINTS !!" and the game will not display any other barcodes that start with the character a
                var existingBarcode = gamepage.Barcodes.FirstOrDefault(barcode => barcode.Id.Contains(data[0]));
                if (existingBarcode == null)
                {
                    gamepage.Barcodes.Add(new Barcode { BData = data.Substring(1), Id = data[0].ToString() });
                    return true;
                }
                return false;
            }
            return false;

        }

    }
}
