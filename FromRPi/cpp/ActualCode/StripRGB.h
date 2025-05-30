//Strip_RGB_WS2812b.h
//led tape
#ifndef STRIP_RGB_H
#define STRIP_RGB_H

#include <ws2811.h>
#include "IDataListener.h"

constexpr int DEFAULT_TARGER_FREQ 	= WS2811_TARGET_FREQ;
constexpr int DEFAULT_GPIO_PIN 		= 18; //GPIO18
constexpr int DEFAULT_DMA			= 10; 
constexpr int DEFAULT_STRIP_TYPE	= WS2811_STRIP_GBR; //chip+leds
constexpr int DEFAULT_LED_COUNT		= 3; //there will be more than one strip - think about it
//brightness???

class RGBStrip //: public IDataListener
{
	public:
		RGBStrip(int target_freq = DEFAULT_TARGER_FREQ,
				 int gpio_pin	 = DEFAULT_GPIO_PIN,
				 int dma		 = DEFAULT_DMA,
				 int strip_type  = DEFAULT_STRIP_TYPE,
				 int led_count	 = DEFAULT_LED_COUNT);
				 
		~RGBStrip();
			 
		void testRGB();
		
		//void onDataReceived(const ParsedData& d);
		
		void setColor(int r, int g, int b);
		
	private:
		int clear_on_exit = 0;
		int ledCount_;
		bool initialiazed_ = false;
		ws2811_t leds_string_;
		
		//string convertIntToHex(int val);
};







#endif
