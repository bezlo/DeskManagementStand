#include <ctime>
#include <iostream>
#include <string>
#include <boost/asio.hpp>
#include <boost/enable_shared_from_this.hpp>  
#include <boost/bind.hpp>                     
#include <boost/bind/bind.hpp>

using boost::asio::ip::tcp;
// === tcp_connection ===
class tcp_connection : public boost::enable_shared_from_this<tcp_connection>
{
public:
  typedef boost::shared_ptr<tcp_connection> pointer;

  static pointer create(boost::asio::io_service& io_service)
  {
    return pointer(new tcp_connection(io_service));
  }

  tcp::socket& socket() { return socket_; }

  void start()
  {
    message_ = make_daytime_string();

    boost::asio::async_write(socket_, boost::asio::buffer(message_),
        boost::bind(&tcp_connection::handle_write, shared_from_this(),
          boost::asio::placeholders::error,
          boost::asio::placeholders::bytes_transferred));
  }

private:
  tcp_connection(boost::asio::io_service& io_service)
    : socket_(io_service) {}

  void handle_write(const boost::system::error_code& /*error*/,
      size_t /*bytes_transferred*/) {}

  tcp::socket socket_;
  std::string message_;

  std::string make_daytime_string()
  {
    time_t now = time(0);
    return ctime(&now);
  }
};

// === tcp_server ===
class tcp_server
{
public:
	
	
	
  tcp_server(boost::asio::io_service& io_service)
    : io_service_(io_service),
    acceptor_(io_service, tcp::endpoint(tcp::v4(), 13))
  {
    start_accept();
  }

private:
boost::asio::io_service& io_service_;
	tcp::acceptor acceptor_;
  void start_accept()
  {
    tcp_connection::pointer new_connection =
      tcp_connection::create(io_service_);

    acceptor_.async_accept(new_connection->socket(),
        boost::bind(&tcp_server::handle_accept, this, new_connection,
          boost::asio::placeholders::error));
  }

  void handle_accept(tcp_connection::pointer new_connection,
      const boost::system::error_code& error)
  {
    if (!error)
    {
      new_connection->start();
    }

    start_accept();
  }
};

// === main ===
int main()
{
  try
  {
    boost::asio::io_service io_service;
    tcp_server server(io_service);
    io_service.run();
  }
  catch (std::exception& e)
  {
    std::cerr << e.what() << std::endl;
  }
l
  return 0;
}
