#pragma once
#include <string>
#include <vector>
#include <optional>
#include <utility>

class CommandParser {
public:
    // Parsuje string formatu "CMD,arg1,arg2,arg3"
    // Zwraca nazwe komendy i wektor optional<int> dla argumentow
    static std::pair<std::string, std::vector<std::optional<int>>> parse(const std::string& input);
};
