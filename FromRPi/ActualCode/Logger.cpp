// #### Logger cpp
#include "Logger.h"

std::mutex Logger::log_mutex;
std::ofstream Logger::log_file;

std::string Logger::levelToColor(LogLevel level) {
	switch (level) {
        case LogLevel::INFO: return "\033[32m";     // green
        case LogLevel::WARNING: return "\033[33m";  // yellow
        case LogLevel::ERROR: return "\033[31m";    // red
    }
    return "\033[0m";
}

void Logger::setLogFile(const std::string& filename) {
		std::lock_guard<std::mutex> lock(log_mutex);
		log_file.open(filename, std::ios::app);

}

void Logger::log(LogLevel level, const std::string& message) {
        std::lock_guard<std::mutex> lock(log_mutex);

        auto now = std::chrono::system_clock::now();
        std::time_t now_time = std::chrono::system_clock::to_time_t(now);

        std::tm tm_now;
#ifdef _WIN32
        localtime_s(&tm_now, &now_time);
#else
        localtime_r(&now_time, &tm_now);
#endif
		std::string color = levelToColor(level);
		
		
        std::cout << "[" << std::put_time(&tm_now, "%Y-%m-%d %H:%M:%S") << "] ";
        std::cout << color << "[" << log_level_to_string(level) << "] " << "\033[0m";
        std::cout << message << std::endl;
        
        if (log_file.is_open()) {
        log_file << log_level_to_string(level) << "	" << message << std::endl;
		}
    }
    
std::string Logger::log_level_to_string(LogLevel level) {
        switch (level) {
            case LogLevel::INFO: return "INFO";
            case LogLevel::WARNING: return "WARN";
            case LogLevel::ERROR: return "ERROR";
            default: return "UNKNOWN";
        }
    }
