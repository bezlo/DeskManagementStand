using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskManagementStand_App.Services
{
    internal class ChargingParametersService
    {
        private readonly Func<double> _getChargingPower;
        private readonly Func<double> _getChargingVoltage;
        private readonly Func<double> _getChargingCurrent;

        public ChargingParametersService(Func<double> getChargingPower, Func<double> getChargingVoltage, Func<double> getChargingCurrent)
        {
            _getChargingPower = getChargingPower;
            _getChargingVoltage = getChargingVoltage;
            _getChargingCurrent = getChargingCurrent;
        }
    }
}
