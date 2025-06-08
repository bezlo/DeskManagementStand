//Charging.cpp
#include "Charging.h"

#include <cstdlib>
#include <ctime>
#include <string>

int Charging::NoIterations = 0;

Charging::Charging(){
	std::srand(static_cast<unsigned>(std::time(nullptr)));
	initialize();
}

void Charging::ReadHardwareData(){

	//simulation

	if(NoIterations <= 10)
	{
	PhoneParameters.OnOff 		= false;
	WatchParameters.OnOff 		= true;
	HeadphonesParameters.OnOff 	= false;
	EarphonesParameters.OnOff 	= true;
	PhoneParameters.Voltage 		= 0.0;
	WatchParameters.Voltage 		= 5.03;
	HeadphonesParameters.Voltage	= 0.0;
	EarphonesParameters.Voltage		= 4.93;
	PhoneParameters.Current 		= 0.0;
	WatchParameters.Current 		= rand() % 5 + 1;
	HeadphonesParameters.Current	= 0.0;
	EarphonesParameters.Current		= rand() % 5 + 1;
	}else{
	PhoneParameters.OnOff 		= true;
	WatchParameters.OnOff 		= false;
	HeadphonesParameters.OnOff 	= true;
	EarphonesParameters.OnOff 	= false;
	PhoneParameters.Current 		= rand() % 5 + 1;
	WatchParameters.Current 		= 0.0;
	HeadphonesParameters.Current	= rand() % 5 + 1;
	EarphonesParameters.Current		= 0.0;
	PhoneParameters.Voltage 		= 5.1;
	WatchParameters.Voltage 		= 0.0;
	HeadphonesParameters.Voltage	= 4.88;
	EarphonesParameters.Voltage		= 0.0;
	}
	
	PhoneParameters.Power 		= PhoneParameters.Voltage * PhoneParameters.Current;
	WatchParameters.Power 		= WatchParameters.Voltage * WatchParameters.Current;
	HeadphonesParameters.Power	= HeadphonesParameters.Voltage * HeadphonesParameters.Current;
	EarphonesParameters.Power	= EarphonesParameters.Voltage * EarphonesParameters.Current;
	
	
	NoIterations++;
	
	if(NoIterations >= 21)
	{
		NoIterations = 0;
	}

}

void Charging::initialize() {
	
	PhoneParameters.DeviceName = "PHONE";
	PhoneParameters.OnOff = false;
	PhoneParameters.Voltage = 0.0;
	PhoneParameters.Current = 0.0;
	PhoneParameters.Power 	= 0.0;
	

	WatchParameters.DeviceName = "WATCH";
	WatchParameters.OnOff = false;
	WatchParameters.Voltage = 0.0;
	WatchParameters.Current = 0.0;
	WatchParameters.Power 	= 0.0;
	

	HeadphonesParameters.DeviceName = "HEADPHONES";
	HeadphonesParameters.OnOff = false;
	HeadphonesParameters.Voltage = 0.0;
	HeadphonesParameters.Current = 0.0;
	HeadphonesParameters.Power 	= 0.0;
	

	EarphonesParameters.DeviceName = "EARPHONES";
	EarphonesParameters.OnOff = false;
	EarphonesParameters.Voltage = 0.0;
	EarphonesParameters.Current = 0.0;
	EarphonesParameters.Power 	= 0.0;

}
