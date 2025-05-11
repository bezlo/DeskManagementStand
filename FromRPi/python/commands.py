import re

class CommandInterpreter:
    def __init__(self, logger):
        self.logger = logger
        self.r = 0
        self.g = 0
        self.b = 0

        self.commands = {
            r"Nowy kolor:\s*(\d+),\s*(\d+),\s*(\d+)": self.set_color
        }

    def parse_message(self, message):
        for pattern, handler in self.commands.items():
            match = re.search(pattern, message)
            if match:
                handler(*match.groups())
                return
        self.logger.warning(f"Nieznana komenda: {message}")

    def set_color(self, r, g, b):
        self.r = int(r)
        self.g = int(g)
        self.b = int(b)
        self.logger.info(f"Ustawiono kolor RGB: ({self.r}, {self.g}, {self.b})")
