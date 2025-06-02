// main.cpp
#include "Logger.h"
#include "tcp_server.h"
#include "StripRGB.h"
#include "Charging.h"

#include <boost/asio.hpp>
#include <iostream>
#include <exception>
#include <thread>

int main() {
    Logger::setLogFile("server.log");
    //int RGB[3] = {0,0,0};
    try {
        //create io_service and server TCP
        boost::asio::io_service io_service;
        tcp_server server(io_service);
        // run network thread (asio loop)
        std::thread asioThread([&](){
            io_service.run();
        });
        // create and run
        ChargingPublisher chargingpublisher(io_service, server);
        publisher.start();
        // wait for end (never)
        asioThread.join();
        publisher.stop();
        /* OLD WORKING VERSION
        RGBStrip strip;
        
        server.set_rgb_callback([&RGB, &strip](int r,int g,int b)
        {std::cout <<"[CALLBACK]"<<" RGB:" << r <<","<<g<<","<<b<< std::endl;
            RGB[0] = r;
            RGB[1] = g;
            RGB[2] = b;
            strip.setColor(RGB[0],RGB[1],RGB[2]);
        });
        
        io_service.run();
        */
    }
    catch (const std::exception& e) {
        std::cerr << e.what() << std::endl;
    }
    return 0;
}
