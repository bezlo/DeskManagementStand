#pragma once

#include "tcp_connection.h"
#include <boost/asio.hpp>
#include <functional>

using RGB_Callback = std::function<void(int r, int g, int b)>;

//thanks to this alias now i can use, because its alias not strict func its like type, so i can have diff commands inside
//RGB_Callback myFunc = [](int r, int g, int b) {}
//same as
//std::function<void(int, int, int)> myFunc = [](int r, int g, int b) {}

class tcp_server
{
public:
    explicit tcp_server(boost::asio::io_service& io_service);

    void set_rgb_callback(std::function<void(int,int,int)> callback);

private:
    void start_accept();
    void handle_accept(tcp_connection::pointer new_connection, const boost::system::error_code& error);

    boost::asio::io_service& io_service_;
    boost::asio::ip::tcp::acceptor acceptor_;
    
    std::function<void(int,int,int)> rgb_callback;
};
