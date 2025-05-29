//Strip_RGB_WS2812b.cpp
//led tape
#include "StripRGB.h"
#include <stdexcept>   // std::runtime_error
#include <unistd.h>    // sleep()
#include <cstring>

RGBStrip::RGBStrip(int target_freq,
				   int gpio_pin,
				   int dma,
				   int strip_type,
				   int led_count)
				   :
				   ledCount_(led_count), initialiazed_(false)
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
void RGBStrip::testRGB()
{
	// Ustaw kolory dla 3 diod
    leds_string_.channel[0].leds[0] = 0x00200000; // Czerwona
    leds_string_.channel[0].leds[1] = 0x00002000; // Zielona
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
