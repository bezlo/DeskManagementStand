// Command.h
#pragma once
#include <string>
#include <vector>
#include <optional>

struct Command {
    std::string name;
    std::vector<std::optional<int>> args;
};
