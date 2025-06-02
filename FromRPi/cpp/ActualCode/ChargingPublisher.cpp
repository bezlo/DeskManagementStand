//ChargingPublisher.cpp
#include "charging_publisher.h"
#include <chrono>
#include <sstream>

ChargingPublisher::ChargingPublisher(boost::asio::io_service& io_service,
									 tcp_server& server) :
									 io_service_(io_service),
									 server_(server) {}

void ChargingPublisher::start(){
	//if it is working, dont run it twice
	bool expected = false;
	if(!running_.compare_exchange_strong(expected, true))
	{ return;}
	workerThread_ = std::thread(&ChargingPublisher::runLoop, this);
}						 
	
void ChargingPublisher::stop(){
	//bool running_ = false thread is ended
	running_.store(false);
    if (workerThread_.joinable()) {
        workerThread_.join();
    }
}

void ChargingPublisher::runLoop()
{
    while (running_.load())
    {
        std::this_thread::sleep_for(5s);


        // take current parameters
        auto phone      = Charging::getPhoneParameters();
        auto watch      = Charging::getWatchParameters();
        auto headphones = Charging::getHeadphonesParameters();
        auto earphones  = Charging::getEarphonesParameters();

		string cmd = "CMD_CHARGING_DATA";
        // Format charging data
        std::ostringstream phoneData;
        phoneData << cmd << ";" 
            << phone.DeviceName << ";" << phone.OnOff << ";" << phone.Voltage << ";" << phone.Current << ";" << phone.Power << ";";
            
        std::ostringstream watchData;
        watchData << cmd << ";"
			<< watch.DeviceName << ";" << watch.OnOff << ";" << watch.Voltage << ";" << watch.Current << ";" << watch.Power << ";";
		
		std::ostringstream headphonesData;
		headphonesData << cmd << ";"
			<< headphones.DeviceName << ";" << headphones.OnOff << ";" << headphones.Voltage << ";" << headphones.Current << ";" << headphones.Power << ";";
		
		std::ostringstream earphonesData;
		earphonesData << cmd << ";"
			<< earphonesData.DeviceName << ";" << earphones.OnOff << ";" << earphones.Voltage << ";" << earphones.Current << ";" << earphones.Power << ";";
		
		
        std::string payloadPhone = phoneData.str();
        std::string payloadWatch = watchData.str();
        std::string payloadheadphones = headphonesData.str();
        std::string payloadearphones = earphonesData.str();

        // copy of active connetions
        auto connections = server_.getActiveConnections();
        for (auto& connPtr : connections) {
            if (!connPtr) continue;
            // Wrzucamy do io_service zadanie wysylki,
            // by watek sieciowy (Boost.Asio) zajal sie faktycznym socket.write().
            io_service_.post([connPtr, payloadPhone]() {
                connPtr->send_message(payloadPhone);
            });
            io_service_.post([connPtr, payloadWatch]() {
                connPtr->send_message(payloadWatch);
            });
            io_service_.post([connPtr, payloadheadphones]() {
                connPtr->send_message(payloadheadphones);
            });
            io_service_.post([connPtr, payloadearphones]() {
                connPtr->send_message(payloadearphones);
            });
            
        }
    }
}
