#include "CommandParser.h"
#include <sstream>
#include <cstdlib>

Command CommandParser::parse(const std::string& input) {
    std::stringstream ss(input);
    std::string token;
    Command cmd;
    
    // Pierwszy token to komenda
    //getline odczytuje csv
    if (!std::getline(ss, token, ',')) {
        return cmd;
    }
    
    cmd.name = token;
    
    while (std::getline(ss, token, ',')) {
        // proba konwersji do int
        char* endptr = nullptr;
        //strtol(char*,char**,int)
        // skad, dokad, system liczbowy
        //token.c_str() konwertuje na const char
        long val = std::strtol(token.c_str(), &endptr, 10);

        if (endptr != token.c_str() && *endptr == '\0') {
            // udana konwersja
            cmd.args.push_back(static_cast<int>(val));
        } else {
            // konwersja nieudana, argument nie jest liczba
            cmd.args.push_back(std::nullopt);
        }
       
    }
     return cmd;
}


/*
#include "CommandParser.h"
#include <sstream>
#include <cstdlib>  // std::strtol

std::pair<std::string, std::vector<std::optional<int>>> CommandParser::parse(const std::string& input)
{
    std::stringstream ss(input);
    std::string token;

    // Pierwszy token to komenda
    //getline odczytuje csv
    if (!std::getline(ss, token, ',')) {
        return {"", {}};
    }
    std::string command = token;

    std::vector<std::optional<int>> args;

    while (std::getline(ss, token, ',')) {
        // proba konwersji do int
        char* endptr = nullptr;
        //strtol(char*,char**,int)
        // skad, dokad, system liczbowy
        //token.c_str() konwertuje na const char
        long val = std::strtol(token.c_str(), &endptr, 10);

        if (endptr != token.c_str() && *endptr == '\0') {
            // udana konwersja
            args.push_back(static_cast<int>(val));
        } else {
            // konwersja nieudana, argument nie jest liczba
            args.push_back(std::nullopt);
        }
    }

    return {command, args};
}
*/ 
