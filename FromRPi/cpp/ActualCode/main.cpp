// main.cpp
#include "Logger.h"
#include "tcp_server.h"
#include "StripRGB.h"
#include "Charging.h"
#include "Data.h"

#include <boost/asio.hpp>
#include <iostream>
#include <exception>
#include <thread>




int main() {
    Logger::setLogFile("server.log");
    std::cout << ">>> Serwer startuje, nasluch na porcie 5000\n";

    try {
        boost::asio::io_service io_service;
        
        ThreadSafeQueue<RGBColor> colorQueue;
        ThreadSageQueue<std::shared_ptr<DeviceParameters>> deviceQueue;
        
        RGBStrip rgbstrip(&colorQueue);
        Charging charging(&deviceQueue);
        tcp_server server(io_service, &colorQueue);
        
        std::thread t_asio([&]{ io_service.run();});
        std::thread t_rgb([&RGBStrip::run, &rgbstrip);
        std::thread t_charging(&Charging::run, &charging);
        std::thread t_deviceData([&server, &deviceQueue](){
            while (auto p = deviceQueue.pop()) {
                server.broadcast_device(*p);
            }
        });
        
        
    std::cout<<"Nacisnij Enter, aby zakonczyc...";
    std::cin.get();

    // zakonczenie
    io_service.stop();
    rgbStrip.stop();
    charging.stop();
    colorQueue.push(RGBColor{-1,0,0});

    t_asio.join();
    t_rgb.join();
    t_charge.join();
    t_deviceData.join();
    }
    catch (const std::exception& e) {
        std::cerr << e.what() << std::endl;
    }
    return 0;
}

