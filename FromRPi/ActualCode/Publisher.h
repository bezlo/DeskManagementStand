// Publisher.h
#pragma once

#include <vector>
#include <functional>
#include <mutex>

template<typename T>
class Publisher {
public:
    using Callback = std::function<void(const T&)>;

    // Dodaje subskrybenta
    void subscribe(Callback cb) {
        std::lock_guard<std::mutex> lk(mu_);
        subscribers_.push_back(std::move(cb));
    }

    // Public new message, 
    void publish(const T& value) {
        std::lock_guard<std::mutex> lk(mu_);
        for (auto& cb : subscribers_) {
            cb(value);
        }
    }
    

private:
    std::vector<Callback> subscribers_;
    std::mutex mu_;
};
