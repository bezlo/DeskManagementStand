# Kompilator i flagi
CXX = g++
CXXFLAGS = -std=c++17 -Wall -Wextra -O2

#sciezki dodatkowych naglowkow i bibliotek
INCLUDES = -I/usr/local/include
LDFLAGS	 = -L/usr/local/lib
LDLIBS	 = -lws2811 -lm

# Nazwa wyjsciowego pliku wykonywalnego
TARGET = DeskManagementStand

# pliki zrodlowe i obiektowe
SRCS = Logger.cpp CommandParser.cpp tcp_server.cpp tcp_connection.cpp StripRGB.cpp main.cpp
OBJS = $(SRCS:.cpp=.o)

# domyslna regula
all: $(TARGET)

# Linkowanie
$(TARGET): $(OBJS)
	$(CXX) $(CXXFLAGS) $(LDFLAGS) -o $@ $^ $(LDLIBS)

# Kompilacja .cpp do .o
%.o: %.cpp
	$(CXX) $(CXXFLAGS) $(INCLUDES) -c $< -o $@

# Uruchomienie programu
run: all
	./$(TARGET)

# czyszczynie plikow tymczasowych
clean:
	rm -f $(OBJS) $(TARGET)
