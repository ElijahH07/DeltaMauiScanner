using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaMauiScanner
{
    public static class Globals
    {
        public static int totalecount = 0;

        public static int totalpoints { get; set; } = 0;

        public static int totaltime { get; set; } = 15;
        public static string scannedBarcodes { get; set; } = "";
        public static int bestscore { get; set; } = 0;

    }
}
