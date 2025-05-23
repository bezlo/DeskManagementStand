#pragma once

#include <string>
#include <unordered_map>
#include <functional>
#include "Logger.h"

class CommandDispatcher {
public:
    using CommandHandler = std::function<void(const std::string& args)>;

    void register_command(const std::string& command, CommandHandler handler) {
        commands_[command] = handler;
    }

    void dispatch(const std::string& input) const {
        auto space_pos = input.find(' ');
        std::string command = (space_pos == std::string::npos) ? input : input.substr(0, space_pos);
        std::string args = (space_pos == std::string::npos) ? "" : input.substr(space_pos + 1);

        auto it = commands_.find(command);
        if (it != commands_.end()) {
            it->second(args);
        } else {
            Logger::log(LogLevel::WARNING, "Nieznana komenda: " + command);
        }
    }

private:
    std::unordered_map<std::string, CommandHandler> commands_;
};
