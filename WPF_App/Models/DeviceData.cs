using System.ComponentModel;

namespace DeskManagementStand_App.Models
{
    public class DeviceData : INotifyPropertyChanged
    {
        private string _deviceType;
        public string DeviceType
        {
            get => _deviceType;
            set { _deviceType = value; OnPropertyChanged(nameof(DeviceType)); }
        }

        private string _isCharging;
        public string IsCharging
        {
            get => _isCharging;
            set { _isCharging = value; OnPropertyChanged(nameof(IsCharging)); }
        }
        private string _voltage;
        public string Voltage
        {
            get => _voltage;
            set { _voltage = value; OnPropertyChanged(nameof(Voltage)); }
        }

        private string _current;
        public string Current
        {
            get => _current;
            set { _current = value; OnPropertyChanged(nameof(Current)); }
        }

        private string _power;
        public string Power
        {
            get => _power;
            set { _power = value; OnPropertyChanged(nameof(Power)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
