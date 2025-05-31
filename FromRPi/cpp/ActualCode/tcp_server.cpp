#include "tcp_server.h"

#include <functional>

using boost::asio::ip::tcp;

tcp_server::tcp_server(boost::asio::io_service& io_service)
    : io_service_(io_service), acceptor_(io_service, tcp::endpoint(tcp::v4(), 5000))
{
    //start_accept(); is commented cause set_rgb_callback need to be call first
    //start_accept();
}

void tcp_server::start_accept()
{   //tcp_connection::create call setup_commands(), where rgb_callback shouldnt be empty
    tcp_connection::pointer new_connection = tcp_connection::create(io_service_);
    //with first and every next connection we add right function
    new_connection->set_rgb_callback(rgb_callback);
    
    acceptor_.async_accept(new_connection->socket(),
        [this, new_connection](const boost::system::error_code& error) {
            handle_accept(new_connection, error);
        });
}

void tcp_server::handle_accept(tcp_connection::pointer new_connection,
    const boost::system::error_code& error)
{
    if (!error) {
        //just in case call set_rgb_callback again -> check if this is needed
        new_connection->set_rgb_callback(rgb_callback);
        new_connection->start();
    }

    start_accept();
}
// this function neeed to be call before start_accept();
void tcp_server::set_rgb_callback(std::function<void(int,int,int)> callback)
{
    rgb_callback = std::move(callback);
    //when the callback is setted, start connection
    start_accept();
}
