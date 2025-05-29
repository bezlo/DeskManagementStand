// main.cpp
#include "StripRGB.h"

#include "Logger.h"
#include "tcp_server.h"

#include <boost/asio.hpp>
#include <iostream>
#include <exception>

int main() {
    Logger::setLogFile("server.log");
    try {
        boost::asio::io_service io_service;
        tcp_server server(io_service);
        io_service.run();
        
        RGBStrip strip;      // uzyje wartosci domyslnych
        strip.testRGB();
    }
    catch (const std::exception& e) {
        std::cerr << e.what() << std::endl;
    }
    return 0;
}
