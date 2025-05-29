#include "tcp_server.h"

using boost::asio::ip::tcp;

tcp_server::tcp_server(boost::asio::io_service& io_service)
    : io_service_(io_service), acceptor_(io_service, tcp::endpoint(tcp::v4(), 5000))
{
    start_accept();
}

void tcp_server::start_accept()
{
    tcp_connection::pointer new_connection = tcp_connection::create(io_service_);

    acceptor_.async_accept(new_connection->socket(),
        [this, new_connection](const boost::system::error_code& error) {
            handle_accept(new_connection, error);
        });
}

void tcp_server::handle_accept(tcp_connection::pointer new_connection,
    const boost::system::error_code& error)
{
    if (!error) {
        new_connection->start();
    }

    start_accept();
}
