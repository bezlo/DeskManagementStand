#pragma once
#include "Command.h"
#include <string>
#include <unordered_map>
#include <functional>
#include "Logger.h"

class CommandDispatcher {
public:
    using CommandHandler = std::function<void(const std::vector<std::optional<int>>& args)>;

    void register_command(const std::string& name, CommandHandler handler) {
        handlers_[name] = handler;
    }

    void dispatch(const Command& cmd) const {
        auto it = handlers_.find(cmd.name);
        if (it != handlers_.end()) {
            it -> second(cmd.args);
        } else {
            Logger::log(LogLevel::WARNING, "Nieznana komenda: " + cmd.name);
        }
            
        /*
        auto space_pos = input.find(' ');
        std::string command = (space_pos == std::string::npos) ? input : input.substr(0, space_pos);
        std::string args = (space_pos == std::string::npos) ? "" : input.substr(space_pos + 1);

        auto it = commands_.find(command);
        if (it != commands_.end()) {
            it->second(args);
        } else {
            Logger::log(LogLevel::WARNING, "Nieznana komenda: " + command);
        }
        */
    }

private:
    std::unordered_map<std::string, CommandHandler> handlers_;
};
