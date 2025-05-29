#pragma once

#include "Logger.h"
#include "CommandDispatcher.h"
#include "CommandParser.h"
#include <boost/asio.hpp>
#include <memory>
#include <string>
#include <optional>
#include <vector>

using boost::asio::ip::tcp;

class tcp_connection : public std::enable_shared_from_this<tcp_connection>
{
public:
    using pointer = std::shared_ptr<tcp_connection>;
    static pointer create(boost::asio::io_service& io_service);

    tcp::socket& socket();
    void start();

private:
    tcp_connection(boost::asio::io_service& io_service);

    void setup_commands();
    void do_read();
    void start_sending_time();
    std::string make_time_string();

    tcp::socket socket_;
    CommandDispatcher dispatcher_;
    enum { max_length = 1024 };
    char read_buf_[max_length];
};
