//Charging.h
#ifndef CHARGING_H
#define CHARGING_H

#include <string>
#include <atomic>
#include "Data.h"
#include <memory>
#include "ThreadSafeQueue.h"

class Charging
{
	private:
		
	ChargingParameters PhoneParameters;
	ChargingParameters WatchParameters;
	ChargingParameters HeadphonesParameters;
	ChargingParameters EarphonesParameters;

	
	static int NoIterations;
	
	void initialize();
	
	ThreadSafeQueue<std::shared_ptr<DeviceParameters>>* dataQueue_;
    std::atomic<bool> running_;
    
	public:
	// Konstruktor przyjmuje wskaznik do kolejki, do ktorej beda wrzucane dane
    Charging(ThreadSafeQueue<std::shared_ptr<DeviceParameters>>* queue);
	void ReadHardwareData();
	void run();
	void stop();
};
#endif
