using System.ComponentModel;
using DeskManagementStand_App.Models;

namespace DeskManagementStand_App.Services
{
    internal class MessageParser : INotifyPropertyChanged
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

            if (string.IsNullOrEmpty(message))
            { throw new ArgumentException("Message cannot be null or empty"); }

            string[] messageParts = message.Split(';');
            if (messageParts.Length < 1)
            { throw new ArgumentException("Message must contain at least a command"); }

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
            if (messageParts.Length < 5)
            { throw new ArgumentException("CMD_GET_DATA must contain at least 4 parameters"); }

            switch (messageParts[1].Trim())
            {
                case "PHONE":
                    PhoneData = CreateDeviceData(messageParts);
                    break;
                case "WATCH":
                    WatchData = CreateDeviceData(messageParts);
                    break;
                case "HEADPHONES":
                    HeadPhonesData = CreateDeviceData(messageParts);
                    break;
                case "EARPHONES":
                    EarphonesData = CreateDeviceData(messageParts);
                    break;
                default:
                    throw new NotSupportedException($"Device type '{messageParts[1].Trim()}' is not supported");
            }
        }

        DeviceData CreateDeviceData(string[] messageParts)
        {
            return new DeviceData
            {
                DeviceType = messageParts[1].Trim(),
                Voltage = messageParts[2].Trim(),
                Current = messageParts[3].Trim(),
                Power = messageParts[4].Trim()
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
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            WatchData = new DeviceData
            {
                DeviceType = "WATCH",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            HeadPhonesData = new DeviceData
            {
                DeviceType = "HEADPHONES",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
            EarphonesData = new DeviceData
            {
                DeviceType = "EARPHONES",
                Voltage = "0",
                Current = "0",
                Power = "0"
            };
        }
    }
}
