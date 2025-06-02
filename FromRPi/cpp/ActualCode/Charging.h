//Charging.h
#include <string>


class Charging
{
	private:
	
	struct ChargingParameters
	{
		std::string DeviceName;
		double Voltage;
		double Current;
		double Power;
	};
	
	ChargingParameters PhoneParameters;
	ChargingParameters WatchParameters;
	ChargingParameters HeadphonesParameters;
	ChargingParameters EarphonesParameters;

	
	static int NoIterations;
	
	void initialize();
	
	public:
	Charging();
	void ReadHardwareData();
};
