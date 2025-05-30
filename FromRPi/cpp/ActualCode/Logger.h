// #### Logger.h
#pragma once
#include <iostream>
#include <string>
#include <chrono>
#include <iomanip>
#include <mutex>
#include <fstream>

enum class LogLevel {
    INFO,
    WARNING,
    ERROR
};

class Logger {
public:
    // write data in cmd
    static void log(LogLevel level, const std::string& message);
    static void setLogFile(const std::string& filename);
private:
    static std::string log_level_to_string(LogLevel level);
    static std::mutex log_mutex;
    static std::ofstream log_file;
    static std::string levelToColor(LogLevel level);
};
