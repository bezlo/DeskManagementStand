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
    std::cout << ">>> Serwer startuje, nasluch na porcie 5000\n";

    try {
        boost::asio::io_service io_service;
        tcp_server server(io_service);

        // 1) Musisz wstrzyknac callback ZANIM uruchomisz run()
        /*StripRGB rgbstrip;
        server.set_rgb_callback(
            [&rgbstrip](int r, int g, int b) {
                rgbstrip.setColor(r, g, b);
            }
        );
*/
        // 2) Teraz uruchom serwer run() juz ma oczekujace accept-y
        io_service.run();
    }
    catch (const std::exception& e) {
        std::cerr << e.what() << std::endl;
    }
    return 0;
}

