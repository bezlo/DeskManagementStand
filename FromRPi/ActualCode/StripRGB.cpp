//Strip_RGB_WS2812b.cpp
//led tape
#include "StripRGB.h"

#include <cstdint>
#include <stdexcept>   // std::runtime_error
#include <unistd.h>    // sleep()
#include <cstring>
#include <sstream>
#include <bitset>

RGBStrip::RGBStrip(ThreadSafeQueue<RGBColor>* queue,
				   int target_freq,
				   int gpio_pin,
				   int dma,
				   int strip_type,
				   int led_count)
				   :
				   ledCount_(led_count), initialiazed_(false),
				   colorQueue_(queue), running_(true)
{
	// *** WAZNE: wyzeruj cala strukture przed przypisaniami ***
    std::memset(&leds_string_, 0, sizeof(leds_string_));
    
	leds_string_.freq 	= target_freq;
	leds_string_.dmanum = dma;
	
	leds_string_.channel[0].gpionum = gpio_pin;
    leds_string_.channel[0].invert = 0;
    leds_string_.channel[0].count = led_count;
    leds_string_.channel[0].strip_type = strip_type;
    leds_string_.channel[0].brightness = 255;

    leds_string_.channel[1].gpionum = 0;
    leds_string_.channel[1].invert = 0;
    leds_string_.channel[1].count = 0;
    leds_string_.channel[1].brightness = 0;
    
    if(ws2811_init(&leds_string_) != WS2811_SUCCESS) {
		throw std::runtime_error("ws281x_init failed");
	}
	initialiazed_ = true;
}
RGBStrip::~RGBStrip() {
    if (initialiazed_) {
        ws2811_fini(&leds_string_);
    }
}

void RGBStrip::run() {
    //testRGB();
     Logger::log(LogLevel::DEBUG, "StepInsideRun");
        while (running_) {
	    Logger::log(LogLevel::DEBUG, "StepInWhileInsideRun");
            RGBColor color = colorQueue_->pop();
            // Sentinelna wartosc (r < 0) sygnalizuje koniec dzialania watku
	    Logger::log(LogLevel::INFO, "run gets : (r=" +
                std::to_string(color.r) + ", g=" + std::to_string(color.g) + ", b=" + std::to_string(color.b) + ")");
            if (color.r < 0) break;
            setColor(color.r, color.g, color.b);
	    logger.log(LogLevel::DEBUG, "debug RGBStrip::run() r=" +
                std::to_string(color.r) + ", g=" + std::to_string(color.g) + ", b=" + std::to_string(color.b) + ")");
        }
}

void RGBStrip::stop() { 
        running_ = false; 
        colorQueue_->push(RGBColor{-1,0,0}); 
}

void RGBStrip::testRGB()
{
	// Ustaw kolory dla 3 diod
    leds_string_.channel[0].leds[0] = 0x00200000; // Czerwona
    leds_string_.channel[0].leds[1] = 0x00002000; // Zielona
    leds_string_.channel[0].leds[2] = 0x00000020; // Niebieska

    ws2811_render(&leds_string_); // Wyslij dane do LED
    sleep(5); // Swiec przez 5 sekund
    
    leds_string_.channel[0].leds[0] = 0x00200000; // Czerwona
    leds_string_.channel[0].leds[1] = 0x00200000;
    leds_string_.channel[0].leds[2] = 0x00200000;

    ws2811_render(&leds_string_); // Wyslij dane do LED
    sleep(5); // Swiec przez 5 sekund
    
    leds_string_.channel[0].leds[0] = 0x00002000; // Zielona; // Czerwona
    leds_string_.channel[0].leds[1] = 0x00002000; // Zielona
    leds_string_.channel[0].leds[2] = 0x00002000; // Zielona; // Niebieska

    ws2811_render(&leds_string_); // Wyslij dane do LED
    sleep(5); // Swiec przez 5 sekund
    
    leds_string_.channel[0].leds[0] = 0x00000020; // Niebieska;
    leds_string_.channel[0].leds[1] = 0x00000020; // Niebieska;
    leds_string_.channel[0].leds[2] = 0x00000020; // Niebieska

    ws2811_render(&leds_string_); // Wyslij dane do LED
    sleep(5); // Swiec przez 5 sekund
    // Wyczysc LEDy
    for (int i = 0; i < ledCount_; i++) {
        leds_string_.channel[0].leds[i] = 0;
    }
    ws2811_render(&leds_string_);

    ws2811_fini(&leds_string_);
}

//void RGBStrip::onDataReceived(const ParsedData& data)  override {setColor(d.r, d.g, d.b);}


void RGBStrip::setColor(int r, int g, int b)
{
    Logger::log(LogLevel::DEBUG, "set color gets color:" + std::to_string(r) + "," + std::to_string(g) + "," + std::to_string(b));
    // 0xRRGGBB
    // it is 0xBBGGRR????
    uint32_t rgbColor = (static_cast<uint32_t>(b) << 16)
		      | (static_cast<uint32_t>(g) << 8)
		      | (static_cast<uint32_t>(r));
     // Formatowanie hex do logu:
    std::ostringstream oss;
    oss << std::hex << std::uppercase << std::setw(6) << std::setfill('0') << rgbColor;
    Logger::log(LogLevel::DEBUG, std::string("Calculated 0xRRGGBB: 0x") + oss.str());
     
    for (int i = 0; i < ledCount_; i++)
    {leds_string_.channel[0].leds[i] = rgbColor;}
    
    ws2811_render(&leds_string_);
}

void RGBStrip::effectRainbow()
{
//rainbow colors
	uint8_t RainbowColors[21][3] = {
		{15, 57, 35}, {79, 151, 55}, {192, 255, 2}, {253, 195, 0},
		{209, 108, 30}, {255, 120, 0}, {250, 55, 44}, {142, 0, 45},
		{188, 21, 125}, {111, 18, 102}, {217, 86, 122}, {255, 114, 195},
		{149, 104, 196}, {144, 60, 212}, {75, 0, 130}, {7, 0, 86},
		{65, 91, 132}, {42, 120, 187}, {99, 206, 242}, {0, 230, 230},
		{3, 124, 110}
	};
	long unsigned int i = 0;
    while (true)
    {
		setColor(RainbowColors[i][0], RainbowColors[i][1], RainbowColors[i][2]);
        
        i++;
		if (i >= sizeof(RainbowColors))
		{
			i = 0; // reset index to loop through colors again
		}

        sleep(1); // wait one second
    }
	
}
