using System.ComponentModel;
using DeskManagementStand_App.Models;
using Windows.Devices.Bluetooth;

namespace DeskManagementStand_App.Services
{
    public class MessageParser : INotifyPropertyChanged
    {
        private bool _initialized = false;

        private DeviceData _phoneData = new DeviceData();
        private DeviceData _watchData = new DeviceData();
        private DeviceData _headphonesData = new DeviceData();
        private DeviceData _earphonesData = new DeviceData();

        public DeviceData PhoneData
        {
            get => _phoneData;
            set 
            { 
                _phoneData = value;
                OnPropertyChanged(nameof(PhoneData));
            }
        }
        public DeviceData WatchData
        {
            get => _watchData;
            set 
            { 
                _watchData = value;
                OnPropertyChanged(nameof(WatchData));
            }
        }
        public DeviceData HeadPhonesData
        {
            get => _headphonesData;
            set
            { 
                _headphonesData = value;
                OnPropertyChanged(nameof(HeadPhonesData));
            }
        }
        public DeviceData EarphonesData
        {
            get => _earphonesData;
            set 
            { 
                _earphonesData = value;
                OnPropertyChanged(nameof(EarphonesData));
            }
        }

        public MessageParser()
        {
            initializeData();
        }

        public void Parse(string message)
        {
            // Split the message into command and parameters
            // example message "CMD_GET_DATA;PHONE;5;3;15"
            Logger logger = new Logger();

            try
            {
                if (string.IsNullOrEmpty(message))
                { throw new ArgumentException("Message cannot be null or empty"); }

                string[] messagesArray = message.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                for(int i = 0; i < messagesArray.Length; i++)
                {
                    processMessage(messagesArray[i].Trim());
                }
            }
            catch (ArgumentException argEx)
            {
                logger.Log(Logger.LogLevel.ERROR, $"Błąd argumentu w MessageParser.Parse: {argEx.Message}");
            }
            catch (NotSupportedException notSupEx)
            {
                logger.Log(Logger.LogLevel.WARNING, $"Nieobsługiwana komenda w MessageParser.Parse: {notSupEx.Message}");
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.ERROR, $"Nieoczekiwany błąd w MessageParser.Parse: {ex.Message}");
            }
        }

        private void processMessage(string message)
        {
            Logger logger = new Logger();

            string[] messageParts = message.Split(';');

            if (messageParts.Length < 1)
            {
                throw new ArgumentException("Message must contain at least a command");
            }

            string Command = messageParts[0].Trim();

            switch (Command)
            {
                case "CMD_GET_DATA":
                    logger.Log(Logger.LogLevel.INFO, $"Parsing command: {Command}");
                    ParseCMD_GET_DATA(messageParts);
                    break;
                default:
                    throw new NotSupportedException($"Command '{Command}' is not supported");
            }
        }

        private void ParseCMD_GET_DATA(string[] messageParts)
        {
            if (messageParts.Length < 6)
            { throw new ArgumentException("CMD_GET_DATA must contain at least 4 parameters"); }

            switch (messageParts[1].Trim())
            {
                //deviceID - phone 0, wathch 1, headphones 2, earphones 3
                case "PHONE":
                    PhoneData = UpdateDeviceData(messageParts,0);
                    break;
                case "WATCH":
                    WatchData = UpdateDeviceData(messageParts,1);
                    break;
                case "HEADPHONES":
                    HeadPhonesData = UpdateDeviceData(messageParts,2);
                    break;
                case "EARPHONES":
                    EarphonesData = UpdateDeviceData(messageParts,3);
                    break;
                default:
                    throw new NotSupportedException($"Device type '{messageParts[1].Trim()}' is not supported");
            }
        }

        DeviceData UpdateDeviceData(string[] messageParts, int DeviceId)
        {
            if (DeviceId == 0)
            {
                PhoneData.DeviceType = messageParts[1].Trim();
                PhoneData.IsCharging = messageParts[2].Trim();
                PhoneData.Voltage = messageParts[3].Trim();
                PhoneData.Current = messageParts[4].Trim();
                PhoneData.Power = messageParts[5].Trim();
                return PhoneData;
            }
            else if (DeviceId == 1)
            {
                WatchData.DeviceType = messageParts[1].Trim();
                WatchData.IsCharging = messageParts[2].Trim();
                WatchData.Voltage = messageParts[3].Trim();
                WatchData.Current = messageParts[4].Trim();
                WatchData.Power = messageParts[5].Trim();
                return WatchData;
            }
            else if (DeviceId == 2)
            {
                HeadPhonesData.DeviceType = messageParts[1].Trim();
                HeadPhonesData.IsCharging = messageParts[2].Trim();
                HeadPhonesData.Voltage = messageParts[3].Trim();
                HeadPhonesData.Current = messageParts[4].Trim();
                HeadPhonesData.Power = messageParts[5].Trim();
                return HeadPhonesData;
            }
            else if (DeviceId == 3)
            {
                EarphonesData.DeviceType = messageParts[1].Trim();
                EarphonesData.IsCharging = messageParts[2].Trim();
                EarphonesData.Voltage = messageParts[3].Trim();
                EarphonesData.Current = messageParts[4].Trim();
                EarphonesData.Power = messageParts[5].Trim();
                return EarphonesData;
            }
            else
            {
                throw new NotSupportedException($"Device ID '{DeviceId}' is not supported");
            }
        }

            DeviceData CreateDeviceData(string[] messageParts)
        {
            return new DeviceData
            {
                DeviceType = messageParts[1].Trim(),
                IsCharging = messageParts[2].Trim(),
                Voltage = messageParts[3].Trim(),
                Current = messageParts[4].Trim(),
                Power = messageParts[5].Trim()
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        void initializeData()
        {
            PhoneData = new DeviceData
            {
                DeviceType = "PHONE",
                IsCharging = "0", 
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            WatchData = new DeviceData
            {
                DeviceType = "WATCH",
                IsCharging = "0",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            HeadPhonesData = new DeviceData
            {
                DeviceType = "HEADPHONES",
                IsCharging = "0",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            EarphonesData = new DeviceData
            {
                DeviceType = "EARPHONES",
                IsCharging = "0",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
        }
    }
}
