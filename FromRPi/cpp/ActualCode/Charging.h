//Charging.h
#ifndef CHARGING_H
#define CHARGING_H

#include "ThreadSafeQueue.h"
#include "Data.h"

#include <string>
#include <atomic>
#include <memory>


class Charging
{
	private:
		
	ChargingParameters PhoneParameters;
	ChargingParameters WatchParameters;
	ChargingParameters HeadphonesParameters;
	ChargingParameters EarphonesParameters;

	
	static int NoIterations;
	
	void initialize();
	
	ThreadSafeQueue<std::shared_ptr<ChargingParameters>>* dataQueue_;
    std::atomic<bool> running_;
    
	public:
	// Konstruktor przyjmuje wskaznik do kolejki, do ktorej beda wrzucane dane
    Charging(ThreadSafeQueue<std::shared_ptr<ChargingParameters>>* queue);
	void ReadHardwareData();
	void run();
	void stop();
};
#endif
