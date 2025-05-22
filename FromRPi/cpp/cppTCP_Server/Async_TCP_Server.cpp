  #include <ctime>
#include <iostream>
#include <string>
#include <memory>
#include <boost/asio.hpp>
#include <boost/bind/bind.hpp>
#include <thread>
#include <chrono>

using boost::asio::ip::tcp;

class tcp_connection : public std::enable_shared_from_this<tcp_connection>
{
public:
    typedef std::shared_ptr<tcp_connection> pointer;

    static pointer create(boost::asio::io_service& io_service)
    {
        return pointer(new tcp_connection(io_service));
    }

    tcp::socket& socket() { return socket_; }

    void start()
    {
        try {
            std::cout << "Polaczenie od: "
                      << socket_.remote_endpoint().address().to_string()
                      << ":" << socket_.remote_endpoint().port()
                      << std::endl;
        } catch (std::exception& e) {
            std::cerr << "Blad przy pobieraniu informacji o kliencie: " << e.what() << std::endl;
        }

        do_read();
        start_sending_time();
    }

private:
    tcp_connection(boost::asio::io_service& io_service)
        : socket_(io_service) {}

    void do_read()
    {
        auto self(shared_from_this());
        socket_.async_read_some(boost::asio::buffer(read_buf_, max_length),
            [this, self](boost::system::error_code ec, std::size_t length)
            {
                if (!ec)
                {
                    std::string received(read_buf_, length);
                    std::cout << "[Odebrano] >" << received << "\n";

                    std::string ack = "ACK: " + received + "\n";
                    boost::asio::async_write(socket_, boost::asio::buffer(ack),
                        [this, self](boost::system::error_code /*ec*/, std::size_t /*length*/){});

                    do_read();
                }
                else
                {
                    std::cerr << "[Blad odczytu] " << ec.message() << std::endl;
                }
            });
    }

    void start_sending_time()
    {
        auto self(shared_from_this());
        std::thread([this, self]() {
            try {
                while (true)
                {
                    std::this_thread::sleep_for(std::chrono::seconds(5));
                    std::string current_time = "Time on Pi: " + make_time_string() + "\n";

                    boost::system::error_code ec;
                    boost::asio::write(socket_, boost::asio::buffer(current_time), ec);

                    if (ec)
                    {
                        std::cerr << "[Blad wysylania czasu] " << ec.message() << std::endl;
                        break;
                    }
                }
            } catch (std::exception& e) {
                std::cerr << "[Wyjatek watku czasu] " << e.what() << std::endl;
            }
        }).detach();
    }

    std::string make_time_string()
    {
        time_t now = time(0);
        char buf[64];
        strftime(buf, sizeof(buf), "%H:%M:%S", localtime(&now));
        return std::string(buf);
    }
    tcp::socket socket_;
    enum { max_length = 1024 };
    char read_buf_[max_length];
};

class tcp_server
{
public:
    tcp_server(boost::asio::io_service& io_service)
        : io_service_(io_service), acceptor_(io_service, tcp::endpoint(tcp::v4(), 5000))
    {
        start_accept();
    }

private:
    void start_accept()
    {
        tcp_connection::pointer new_connection = tcp_connection::create(io_service_);

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

    boost::asio::io_service& io_service_;
    tcp::acceptor acceptor_;
};

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
    return 0;
}
