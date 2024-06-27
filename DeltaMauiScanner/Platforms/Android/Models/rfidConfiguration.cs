using Android.Widget;
using Com.Zebra.Rfid.Api3;
using Com.Zebra.Scannercontrol;
using Java.Nio.FileNio;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using DeltaMauiScanner.Platforms.Android.ViewModels;

namespace DeltaMauiScanner.ScannerConfigurations;

public partial class ScannerConfiguration : Java.Lang.Object, IRfidEventsListener
{
    private static ScannerConfiguration _ReaderModel;

    private static Readers readers;
    //private static IList availableRFIDReaderLists;
    private static ReaderDevice readerDevice;
    private static RFIDReader Reader;
    private static string Status;
    private EventHandler eventHandler;
    private Label headingLabel;
    private string rfidString = "Unique rfids found:";
    private int rfidCount;

    private RfidViewModel RfidViewModel = new RfidViewModel();

    private bool getConnectionStatus()
    {
        if (Reader != null)
        {
            try
            {
                return Reader.IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
        }
        return false;
    }
    public partial void setUpRfid()
    {

        if (readers == null)
        {
            readers = new Readers(global::Android.App.Application.Context, ENUM_TRANSPORT.Bluetooth);
        }
        GetAvailableReaders(); //also calls configure reader

    }

    private void GetAvailableReaders()
    {
        ThreadPool.QueueUserWorkItem(o =>
        {
            try
            {
                if (readers != null && readers.AvailableRFIDReaderList != null)
                {

                    if (readers.AvailableRFIDReaderList.Count > 0)
                    {
                        if (Reader == null)
                        {
                            // get first reader from list
                            readerDevice = readers.AvailableRFIDReaderList[0];
                            Reader = readerDevice.RFIDReader;
                            // Establish connection to the RFID Reader
                            Reader.Connect();
                            if (Reader.IsConnected)
                            {
                                System.Diagnostics.Trace.WriteLine("Reader connected");
                                Status = "Reader connected";
                                ConfigureReader();
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine("failed to connect");
                            }

                        }
                    }
                }
            }
            catch (InvalidUsageException e)
            {
                e.PrintStackTrace();
            }
            catch (OperationFailureException e)
            {
                e.PrintStackTrace();
                System.Diagnostics.Trace.WriteLine("OperationFailureException " + e.VendorMessage);
                Status = "OperationFailureException " + e.VendorMessage;
            }
        });
    }

    private void ConfigureReader()
    {
        System.Diagnostics.Trace.WriteLine("configure reader");
        if (Reader.IsConnected)
        {
            TriggerInfo triggerInfo = new TriggerInfo();
            triggerInfo.StartTrigger.TriggerType = START_TRIGGER_TYPE.StartTriggerTypeImmediate;
            triggerInfo.StopTrigger.TriggerType = STOP_TRIGGER_TYPE.StopTriggerTypeImmediate;
            try
            {
             
                TagStorageSettings tagStorageSettings = Reader.Config.TagStorageSettings;
                // set tag storage settings on the reader with all fields
                tagStorageSettings.SetTagFields(TAG_FIELD.AllTagFields);
                Reader.Config.TagStorageSettings = tagStorageSettings;

                Reader.Events.AddEventsListener(this);
                // HH event
                Reader.Events.SetHandheldEvent(true);

                // tag event with tag data
                Reader.Events.SetTagReadEvent(true);
                Reader.Events.SetAttachTagDataWithReadEvent(true);
                //
                Reader.Events.SetInventoryStartEvent(true);
                Reader.Events.SetInventoryStopEvent(true);
                Reader.Events.SetOperationEndSummaryEvent(true);
                Reader.Events.SetReaderDisconnectEvent(true);
                Reader.Events.SetBatteryEvent(true);
                Reader.Events.SetPowerEvent(true);
                Reader.Events.SetTemperatureAlarmEvent(true);
                Reader.Events.SetBufferFullEvent(true);
                Reader.Events.SetBufferFullWarningEvent(true);

                //WiFi Event
                //Reader.Events.AddWifiScanDataEventsListener(this);
                Reader.Events.SetWPAEvent(true);
                Reader.Events.SetScanDataEvent(true);

                Reader.Config.SetTriggerMode(ENUM_TRIGGER_MODE.RfidMode, true);

                // configure for antenna and singulation etc.
                var antenna = Reader.Config.Antennas.GetAntennaRfConfig(1);
                antenna.SetrfModeTableIndex(0);
                antenna.TransmitPowerIndex = Reader.ReaderCapabilities.GetTransmitPowerLevelValues().Length - 1;
                Reader.Config.Antennas.SetAntennaRfConfig(1, antenna);

                var singulation = Reader.Config.Antennas.GetSingulationControl(1);
                singulation.Session = SESSION.SessionS0;
                singulation.Action.InventoryState = INVENTORY_STATE.InventoryStateA;
                singulation.Action.SetPerformStateAwareSingulationAction(false);
                Reader.Config.Antennas.SetSingulationControl(1, singulation);

                var HostName = Reader.HostName;
                var region = Reader.Config.RegulatoryConfig.Region;
                var modelName = Reader.ReaderCapabilities.ModelName;
                var serialNumber = Reader.ReaderCapabilities.SerialNumber;
                var radioVersion = "";
                var moduleVersion = "";
                var deviceVersionInfo = new Android.Runtime.JavaDictionary<string, string>();
                Reader.Config.GetDeviceVersionInfo(deviceVersionInfo);
                if (deviceVersionInfo.ContainsKey(Constants.Nge))
                {
                    radioVersion = deviceVersionInfo[Constants.Nge]; //NGE
                }

                if (deviceVersionInfo.ContainsKey(Constants.GenxDevice))
                {
                    moduleVersion = deviceVersionInfo[Constants.GenxDevice]; //RFID_DEVICE
                }
                VersionInfo versioninfo = Reader.VersionInfo();

                var message = string.Format("HostName:{0} \n Region:{1} \n ModelName:{2} \n SerialNumber:{3} \n RadioVersion:{4} \n ModuleVersion:{5} \n SDKVersion:{6}",
                                                   HostName, region, modelName, serialNumber, radioVersion, moduleVersion, versioninfo.Version);

                System.Diagnostics.Debug.WriteLine(message);

                Reader.Config.GetDeviceStatus(true, true, true);

                TAG_FIELD[] tag_fields = { TAG_FIELD.PeakRssi, TAG_FIELD.TagSeenCount };
                Reader.Config.TagStorageSettings.SetTagFields(tag_fields);

                // If RFD8500 then disable batch mode and DPO
                if (Reader.ReaderCapabilities.ModelName.Contains("RFD8500"))
                {
                    Reader.Config.SetBatchMode(BATCH_MODE.Disable);
                    // Important: DPO should be disabled based on need here disabled for all operations
                    Reader.Config.DPOState = DYNAMIC_POWER_OPTIMIZATION.Disable;
                    //
                    Reader.Config.BeeperVolume = BEEPER_VOLUME.HighBeep;
                }

                Reader.Config.SaveConfig();
            }
            catch (InvalidUsageException e)
            {
                e.PrintStackTrace();
            }
            catch (OperationFailureException e)
            {
                e.PrintStackTrace();
            }
        }
    }
    // Read Event Notification
    public void EventReadNotify(RfidReadEvents e)
    {
        TagData[] myTags = Reader.Actions.GetReadTags(1000);
        if (myTags != null)
        {
            ThreadPool.QueueUserWorkItem(o => TagReadEvent(myTags));

        }
    }

    // Status Event Notification
    public void EventStatusNotify(RfidStatusEvents rfidStatusEvents)
    {
        if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.HandheldTriggerEvent)
        {
            if (rfidStatusEvents.StatusEventData.HandheldTriggerEventData.HandheldTriggerType.Equals(HANDHELD_TRIGGER_TYPE.HandheldTriggerRfid))
            {
                if (rfidStatusEvents.StatusEventData.HandheldTriggerEventData.HandheldEvent == HANDHELD_TRIGGER_EVENT_TYPE.HandheldTriggerPressed)
                {
                    ThreadPool.QueueUserWorkItem(o => TriggerEvent(true));
                }
                if (rfidStatusEvents.StatusEventData.HandheldTriggerEventData.HandheldEvent == HANDHELD_TRIGGER_EVENT_TYPE.HandheldTriggerReleased)
                {
                    ThreadPool.QueueUserWorkItem(o => TriggerEvent(false));
                }
            }
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.InventoryStartEvent)
        {
            System.Diagnostics.Debug.WriteLine("inventory Start Event");
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.InventoryStopEvent)
        {
            System.Diagnostics.Debug.WriteLine("Inventory Stop Event");
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.OperationEndSummaryEvent)
        {
            int rounds = rfidStatusEvents.StatusEventData.OperationEndSummaryData.TotalRounds;
            int totaltags = rfidStatusEvents.StatusEventData.OperationEndSummaryData.TotalTags;
            long timems = rfidStatusEvents.StatusEventData.OperationEndSummaryData.TotalTimeuS / 1000;
            System.Diagnostics.Debug.WriteLine("Summary: Rounds: " + rounds + " Tags: " + totaltags + " Time: " + timems);
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.DisconnectionEvent)
        {
            System.Diagnostics.Debug.WriteLine("Reader Disconnected");
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.BatteryEvent)
        {
            var battery = rfidStatusEvents.StatusEventData.BatteryData;
            System.Diagnostics.Debug.WriteLine("Battery: Cause: " + battery.Cause + " Charging: " + battery.Charging + " Level: " + battery.Level);
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.PowerEvent)
        {
            var power = rfidStatusEvents.StatusEventData.PowerData;
            System.Diagnostics.Debug.WriteLine("PowerData: Cause: " + power.Cause + " Current: " + power.Current + " Voltage: " + power.Voltage + " Power " + power.Power);
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.TemperatureAlarmEvent)
        {
            var temperature = rfidStatusEvents.StatusEventData.TemperatureAlarmData;
            System.Diagnostics.Debug.WriteLine("TemperatureAlarmEvent: AlarmLevel: " + temperature.AlarmLevel + " AmbientTemp: " + temperature.AmbientTemp + " Current: " + temperature.CurrentTemperature + " PATemp:" + temperature.PATemp);
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.BufferFullWarningEvent)
        {
            System.Diagnostics.Debug.WriteLine("BufferFullWarningEvent");
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.BufferFullEvent)
        {
            System.Diagnostics.Debug.WriteLine("BufferFullEvent: ");
        }
        else if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.WpaEvent)
        {
            string scanStatus = rfidStatusEvents.StatusEventData.WPAEventData.Type;
            System.Diagnostics.Debug.WriteLine("WPA Event");
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void TagReadEvent(TagData[] aryTags)
    {
        for (int index = 0; index < aryTags.Length; index++)
        {
            //COnfugure view model
            RfidViewModel.UpdateRfidListView(aryTags[index].TagID);
            //

            if (!rfidExists(aryTags[index].TagID))
            {
                if (aryTags[index].OpCode == ACCESS_OPERATION_CODE.AccessOperationRead &&
                    aryTags[index].OpStatus == ACCESS_OPERATION_STATUS.AccessSuccess)
                {
                    if (aryTags[index].MemoryBankData.Length > 0)
                    {
                        Console.WriteLine(" Mem Bank Data " + aryTags[index].MemoryBankData);
                    }
                }
            }
        }
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public void TriggerEvent(bool pressed)
    {
        if (pressed)
        {
            PerformInventory();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine(rfidString);
            StopInventory();
        }
    }

    internal bool rfidExists(string tagID)
    {
        if ( tagID != null && rfidString.Contains(tagID))
        {
            return true;
        } else
        {
            rfidString = rfidString + " \nTag ID " + tagID;
            return false;
        }
    }

    internal bool PerformInventory()
    {
        try
        {
            Reader.ReinitTransport();
            Reader.Actions.Inventory.Perform();
            System.Diagnostics.Debug.WriteLine("Trigger pressed - starting...");
            return true;
        }
        catch (InvalidUsageException e)
        {
            e.PrintStackTrace();
        }
        catch (OperationFailureException e)
        {
            e.PrintStackTrace();
            System.Diagnostics.Debug.WriteLine(e);
        }
        return false;
    }

    internal void StopInventory()
    {
        try
        {
            Reader.Actions.Inventory.Stop();
            System.Diagnostics.Debug.WriteLine("Trigger released - stopping...");
        }
        catch (InvalidUsageException e)
        {
            e.PrintStackTrace();
        }
        catch (OperationFailureException e)
        {
            e.PrintStackTrace();
            System.Diagnostics.Debug.WriteLine(e);
        }
    }
}