#pragma once

struct ChargingParameters
	{
		std::string DeviceName;
		bool OnOff;
		double Voltage;
		double Current;
		double Power;
	};
	
// Struktura komendy RGB (przekazywana przez kolejke)
struct RGBColor { int r, g, b; };


