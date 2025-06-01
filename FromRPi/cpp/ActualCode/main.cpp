// main.cpp
#include "StripRGB.h"

#include "Logger.h"
#include "tcp_server.h"

#include <boost/asio.hpp>
#include <iostream>
#include <exception>

int main() {
    Logger::setLogFile("server.log");
    int RGB[3] = {0,0,0};
    try {
        boost::asio::io_service io_service;
        tcp_server server(io_service);
        
        RGBStrip strip;
        
        server.set_rgb_callback([&RGB, &strip](int r,int g,int b)
        {std::cout <<"[CALLBACK]"<<" RGB:" << r <<","<<g<<","<<b<< std::endl;
            RGB[0] = r;
            RGB[1] = g;
            RGB[2] = b;
            strip.setColor(RGB[0],RGB[1],RGB[2]);
        });
        
        io_service.run();
        
    }
    catch (const std::exception& e) {
        std::cerr << e.what() << std::endl;
    }
    return 0;
}
