#pragma once

#include <string>
#include <unordered_map>
#include <functional>
#include <vector>
#include <memory>

#include "Logger.h"
#include "Command.h"
#include "IDataListener.h"


class CommandDispatcher {
public:
    using CommandHandler = std::function<void(const std::vector<std::optional<int>>& args)>;
    
    

    void register_command(const std::string& name, CommandHandler handler) {
        handlers_[name] = handler;
    }

    void dispatch(const Command& cmd) const {
        auto it = handlers_.find(cmd.name);
        if (it != handlers_.end()) {
            // wywolanie funkcji ktora zapisalismy w lamdzie
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
    
    //void register_listener(IDataListener* l) {
    //   listeners_.push_back(l);
    //}
    
    //void notify_all(const Command& cmd) {
    //    for (auto l : listeners_) l->onDataReceived(cmd);
    //}

private:
    std::unordered_map<std::string, CommandHandler> handlers_;
    //std::vector<IDataListener*> listeners_;
};
