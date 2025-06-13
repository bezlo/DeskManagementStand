// IDataListener.h
#pragma once
#include "CommandParser.h"

class IDataListener {
public:
  virtual ~IDataListener() = default;
  virtual void onDataReceived(const Command& data) = 0;
};
