using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaMauiScanner.Services;

/// <summary>
///  Testing partial classes -> gets the Model and Platform of each individual device
/// </summary>
internal static partial class DeviceInfoService
{
    internal static partial string Model();

    internal static partial string Platform();
}
