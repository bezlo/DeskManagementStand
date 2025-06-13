#pragma once

#include <mutex>
#include <condition_variable>
#include <queue>

// Szablon watkowo-bezpiecznej kolejki (ThreadSafeQueue)
template<typename T>
class ThreadSafeQueue {
private:
    std::queue<T> queue_;               // podstawa: std::queue
    mutable std::mutex mutex_;          // chroni dostep do kolejki
    std::condition_variable cond_;      // powiadamia o dodaniu elementu
public:
    // Dodaje element do kolejki i powiadamia potencjalnych oczekujacych konsumentow
    void push(const T& value) {
        std::lock_guard<std::mutex> lock(mutex_);
        queue_.push(value);
        cond_.notify_one();
    }

    // Blokujaco pobiera element z kolejki; czeka, jesli jest pusta
    T pop() {
        std::unique_lock<std::mutex> lock(mutex_);
        cond_.wait(lock, [this](){ return !queue_.empty(); });
        T val = queue_.front();
        queue_.pop();
        return val;
    }

    // Sprawdza, czy kolejka jest pusta (bezpieczne dla watkow)
    bool empty() const {
        std::lock_guard<std::mutex> lock(mutex_);
        return queue_.empty();
    }
};
