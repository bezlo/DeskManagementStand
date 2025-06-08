#pragma once

#include "Logger.h"
#include "CommandDispatcher.h"
#include "CommandParser.h"
#include <boost/asio.hpp>
#include <memory>
#include <string>
#include <optional>
#include <vector>
#include <atomic>
#include <mutex>

using boost::asio::ip::tcp;

class tcp_connection : public std::enable_shared_from_this<tcp_connection>
{
public:
    using conn_pointer = std::shared_ptr<tcp_connection>;
    
    static conn_pointer create(boost::asio::io_service& io_service);
                          //std::function<void(int,int,int)> rgbcallback);

    tcp::socket& socket();
    
    void start();
    
    void set_rgb_callback(std::function<void(int,int,int)> callback);
    
    void sendMessage(const std::string& msg);
    
    void start();
    
    void send(const std::string& data);
    
    bool isOpen() const { return connected_; }

    // Zamyka polaczenie (np. wyjscie z watku)
    void close() { connected_ = false; /* close(socketFd_) w praktyce */ }

    // Oczekuje na zakonczenie watku polaczenia
    void join() {
        if (connThread_.joinable())
            connThread_.join();
    }
private:
    tcp_connection(boost::asio::io_service& io_service);

    void setup_commands();
    void do_read();
    void start_sending_time();
    void readLoop();
    
    std::function<void(int,int,int)> rgb_callback;
    
    std::string make_time_string();

    tcp::socket socket_;
    CommandDispatcher dispatcher_;
    enum { max_length = 1024 };
    char read_buf_[max_length];
    
    int socketFd_;
    ThreadSafeQueue<RGBColor>* colorQueue_;
    std::thread connThread_;
    std::atomic<bool> connected_;
    std::mutex sendMutex_;
};
