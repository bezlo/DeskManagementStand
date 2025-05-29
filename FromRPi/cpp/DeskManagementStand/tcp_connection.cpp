#include "tcp_connection.h"

#include <iostream>
#include <thread>
#include <chrono>
#include <ctime>

using namespace std::chrono_literals;

tcp_connection::tcp_connection(boost::asio::io_service& io_service)
    : socket_(io_service) {}

tcp_connection::pointer tcp_connection::create(boost::asio::io_service& io_service)
{
    auto conn = pointer(new tcp_connection(io_service));
    conn->setup_commands();
    return conn;
}

tcp::socket& tcp_connection::socket() { return socket_; }

void tcp_connection::start()
{
    try {
        Logger::log(LogLevel::INFO,
            "Polaczenie od: " + socket_.remote_endpoint().address().to_string() +
            ":" + std::to_string(socket_.remote_endpoint().port()));
    } catch (std::exception& e) {
        std::cerr << "Blad przy pobieraniu informacji o kliencie: " << e.what() << std::endl;
    }

    do_read();
    start_sending_time();
}

void tcp_connection::setup_commands()
{
    dispatcher_.register_command("CMD_SET_RGB", [this](const std::vector<std::optional<int>>& args) {
        if (args.size() >= 3 && args[0] && args[1] && args[2]) {
            int r = *args[0], g = *args[1], b = *args[2];
            Logger::log(LogLevel::INFO, "RGB was set as: (" +
                std::to_string(r) + ", " + std::to_string(g) + ", " + std::to_string(b) + ")");
        } else {
            Logger::log(LogLevel::WARNING, "Usage: CMD_SET_RGB,<r>,<g>,<b>");
        }
    });
}

void tcp_connection::do_read()
{
    auto self(shared_from_this());

    socket_.async_read_some(boost::asio::buffer(read_buf_, max_length),
        [this, self](boost::system::error_code ec, std::size_t length) {
            if (!ec) {
                std::string received(read_buf_, length);
                Logger::log(LogLevel::INFO, "Odebrano: " + received);

                Command cmd = CommandParser::parse(received);
                dispatcher_.dispatch(cmd);

                std::string ack = "ACK: " + received + "\n";

                boost::asio::async_write(socket_, boost::asio::buffer(ack),
                    [this, self](boost::system::error_code /*ec*/, std::size_t /*length*/) {});

                do_read();
            } else {
                Logger::log(LogLevel::ERROR, "Blad odczytu: " + ec.message());
            }
        });
}

void tcp_connection::start_sending_time()
{
    auto self(shared_from_this());
    std::thread([this, self]() {
        try {
            while (true) {
                std::this_thread::sleep_for(5s);
                std::string current_time = "Time on Pi: " + make_time_string() + "\n";

                boost::system::error_code ec;
                boost::asio::write(socket_, boost::asio::buffer(current_time), ec);

                if (ec) {
                    std::cerr << "[Blad wysylania czasu] " << ec.message() << std::endl;
                    break;
                }
            }
        } catch (std::exception& e) {
            std::cerr << "[Wyjatek watku czasu] " << e.what() << std::endl;
        }
    }).detach();
}

std::string tcp_connection::make_time_string()
{
    time_t now = time(0);
    char buf[64];
    strftime(buf, sizeof(buf), "%H:%M:%S", localtime(&now));
    return std::string(buf);
}
