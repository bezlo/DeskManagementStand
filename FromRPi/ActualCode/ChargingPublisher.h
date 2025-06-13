//ChargingPublisher.h
#ifndef CHARGING_PUBLISHER_H
#define CHARGING_PUBLISHER_H

#include <boost/asio.hpp>
#include <thread>
#include <atomic>
#include "tcp_server.h"
#include "charging.h"

class ChargingPublisher
{
	public:
	//constructor take ref to io_service and server
	ChargingPublisher(boost::asio::io_service& io_service, tcp_server& server);
	
	void start();
	void stop();
	
	private:
	void runLoop();
	
	boost::asio::io_service& io_service_;
	tcp_server&		server_;
	std::thread		workerThread_;
	// atomic for safe sharing variables beetwen threads for easy write/read
	std::atomic<bool> running_(false);
};


#endif
