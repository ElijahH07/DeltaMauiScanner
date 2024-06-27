# DeltaMauiScanner

## Summary
DeltaMauiScanner is a cross-platform application designed to run on Android, macOS, iOS, and Windows. Currently, Bluetooth connection functionality is supported only on Android devices. The app features a game mode where users can score points by scanning "bad guys" using the barcode scanner. Additionally, it offers an RFID inventory mode for scanning and managing RFID chips.

## Implementation
To run DeltaMauiScanner, follow these steps:

1. **Download Android Studio:**
   - Android Studio is required to load and build the project for Android devices.

2. **Load the Project:**
   - Make sure you have downloaded the Maui software package.
   - Open Android Studio and load the DeltaMauiScanner project.

## Expanded Use on iOS
For iOS devices, you can expand the functionality of DeltaMauiScanner by integrating the Maui SDK for the RFD40 scanner. Follow these steps:

1. **Download the iOS Maui SDK:**
   - Visit [Zebra's support page](https://www.zebra.com/us/en/support-downloads/software/mobile-computer-software/rfid-reader-maui-ios.html) to download the Maui SDK package for the RFD40 scanner on iOS.

2. **Integrate the SDK:**
   - Follow the installation instructions provided in the SDK package to integrate the Maui SDK into your iOS development environment.

## Notes
   - DeltaMauiScanner provides a fun app for barcode scanning and RFID chip management across multiple operating systems.
   - Future updates may include expanded Bluetooth support for iOS and additional features based on user feedback.

---

## IOS Scanner Implementation

   - See [Maui ios techdocs](https://techdocs.zebra.com/dcs/rfid/maui-ios/getting-started/) for information on connecting via bluetooth to the scanner
   - Copy the following into the Maui .csproj file:

	  <ItemGroup>
		<Reference Include="MauiIosSdkBinding">
			<HintPath>path to ios .dll sdk file</HintPath>
		</Reference>
	  </ItemGroup>

   - Edit the class under `Platforms/IOS/Models/ScannerConfiguration.cs` to configure the bluetooth connection to the RFD40 Zebra device