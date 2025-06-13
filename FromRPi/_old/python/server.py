import asyncio
import time

class TCPServer:
    def __init__(self, host='0.0.0.0', port=5000, on_message=None, logger=None):
        self.host = host
        self.port = port
        self.on_message = on_message
        self.logger = logger

    async def handle_client(self, reader, writer):
        addr = writer.get_extra_info('peername')
        self.logger.info(f"Polaczono z {addr}")

        try:
            while True:
                data = await reader.readline()
                if not data:
                    break
                message = data.decode().strip()
                self.logger.debug(f"Odebrano: {message}")
                if self.on_message:
                    self.on_message(message)

                writer.write(f"ACK: {message}\n".encode())
                await writer.drain()

                # Przyklad: wysylanie czasu
                current_time = time.strftime("%H:%M:%S")
                writer.write(f"Time on Pi: {current_time}\n".encode())
                await writer.drain()

                await asyncio.sleep(5)
        except asyncio.TimeoutError:
            self.logger.warning("Timeout klienta.")
        except (ConnectionResetError, BrokenPipeError):
            self.logger.warning("Polaczenie zostalo przerwane.")
        finally:
            writer.close()
            await writer.wait_closed()
            self.logger.info(f"Rozlaczono {addr}")

    async def start(self):
        server = await asyncio.start_server(
            self.handle_client, self.host, self.port)
        addr = server.sockets[0].getsockname()
        self.logger.info(f"Serwer dziala na {addr}")
        async with server:
            await server.serve_forever()
