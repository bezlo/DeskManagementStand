//tcp_server.h
#pragma once

#include "tcp_connection.h"
#include "ThreadSafeQueue.h"
#include "Data.h"

#include <boost/asio.hpp>
#include <functional>
#include <vector>


using RGB_Callback = std::function<void(int r, int g, int b)>;

//thanks to this alias now i can use, because its alias not strict func its like type, so i can have diff commands inside
//RGB_Callback myFunc = [](int r, int g, int b) {}
//same as
//std::function<void(int, int, int)> myFunc = [](int r, int g, int b) {}

class tcp_server
{
public:
    explicit tcp_server(boost::asio::io_service& io_service, ThreadSafeQueue<RGBColor>* colorQueue);

    void set_rgb_callback(std::function<void(int,int,int)> callback);

    //std::vector<tcp_connection::conn_pointer> getActiveConnections() const {
    //    return connections_;
    // }
    //broadcast for more connected device, like pC app and mobile app
    void broadcast(const std::string& msg);
    void broadcast_device(const ChargingParameters& params);
    void start_accept();
private:
    
    void handle_accept(tcp_connection::conn_pointer new_connection, const boost::system::error_code& error);
    
    boost::asio::io_service& io_service_;
    boost::asio::ip::tcp::acceptor acceptor_;
    
    std::function<void(int,int,int)> rgb_callback;

    std::vector<tcp_connection::conn_pointer> connections_;
    std::mutex connectionsMutex;
    ThreadSafeQueue<RGBColor>* colorQueue_;
    int socketFd_;
};
