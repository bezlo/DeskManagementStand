# Kompilator i flagi
CXX = g++
CXXFLAGS = -std=c++17 -Wall -Wextra -O2

# Nazwa wyjsciowego pliku wykonywalnego
TARGET = server

# pliki zrodlowe i obiektowe
SRCS = Async_TCP_Server.cpp Logger.cpp
OBJS = $(SRCS:.cpp=.o)

# domyslna regula
all: $(TARGET)

# Linkowanie
$(TARGET): $(OBJS)
	$(CXX) $(CXXFLAGS) -o $@ $^

# Kompilacja .cpp do .o
%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c $< -o $@

# Uruchomienie programu
run: all
	./$(TARGET)

# czyszczynie plikow tymczasowych
clean:
	rm -f $(OBJS) $(TARGET)
