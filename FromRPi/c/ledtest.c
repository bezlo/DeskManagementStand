#include <stdio.h>
#include <stdint.h>
#include <unistd.h>
#include "ws2811.h"

#define TARGET_FREQ WS2811_TARGET_FREQ
#define GPIO_PIN    18
#define DMA         10
#define LED_COUNT   3

ws2811_t ledstring =
{
    .freq = TARGET_FREQ,
    .dmanum = DMA,
    .channel =
    {
        [0] =
        {
            .gpionum = GPIO_PIN,
            .count = LED_COUNT,
            .invert = 0,
            .brightness = 255,
            .strip_type = WS2811_STRIP_GRB,
        },
        [1] = {0},
    },
};

int main()
{
    if (ws2811_init(&ledstring) != WS2811_SUCCESS)
    {
        printf("Failed to initialize ws2811\n");
        return 1;
    }

    // Ustaw kolory dla 3 diod
    ledstring.channel[0].leds[0] = 0x00200000; // Czerwona
    ledstring.channel[0].leds[1] = 0x00002000; // Zielona
    ledstring.channel[0].leds[2] = 0x00000020; // Niebieska

    ws2811_render(&ledstring); // Wyslij dane do LED
    sleep(5); // Swiec przez 5 sekund

    // Wyczysc LEDy
    for (int i = 0; i < LED_COUNT; i++) {
        ledstring.channel[0].leds[i] = 0;
    }
    ws2811_render(&ledstring);

    ws2811_fini(&ledstring);
    return 0;
}
