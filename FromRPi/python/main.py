import asyncio
from server import TCPServer
from commands import CommandInterpreter
from logger import setup_logger

import board
import neopixel
import time

# Konfiguracja
LED_COUNT = 3  # liczba diod WS2812B
LED_PIN = board.D18  # GPIO18

pixels = neopixel.NeoPixel(LED_PIN, LED_COUNT, brightness=0.3, auto_write=False)

COLORS = [(255, 0, 0), (0, 255, 0), (0, 0, 255)]

#async def led_test_loop():
#    i = 0
#    while True:
#        pixels.fill(COLORS[i % len(COLORS)])
#        print("Zmiana koloru")
#        pixels.show()
#        i += 1
#        await asyncio.sleep(1)  # zmieniaj kolor co sekunde
        
async def main():
    logger = setup_logger()
    interpreter = CommandInterpreter(logger)

    def handle_message(msg):
        interpreter.parse_message(msg)
        # Mozesz uzyc interpreter.r, g, b tutaj
        # Ustaw kolor na podstawie danych z interpreter
        color = (interpreter.r, interpreter.g, interpreter.b)
        pixels.fill(color)
        pixels.show()
        logger.debug(f"LED ustawiony na: {color}")
        
    server = TCPServer(on_message=handle_message, logger=logger)
    # oba zadaania rownoczesnie
    await server.start()

if __name__ == "__main__":
    asyncio.run(main())
