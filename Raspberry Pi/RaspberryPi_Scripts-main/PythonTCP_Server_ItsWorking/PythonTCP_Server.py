import socket
import threading
import time

HOST = '0.0.0.0'
PORT = 5000

def handle_client(conn, addr):
    print(f"[Polaczono z] {addr}")
    with conn:
        try:
            conn.settimeout(0.5)
            while True:
                try:
                    data = conn.recv(1024)
                    if data:
                        message = data.decode('utf-8')
                        print(f"[Odebrano] [{message}]")

                        try:
                            conn.sendall(f"ACK: {message}\n".encode('utf-8'))
                        except BrokenPipeError:
                            print("[Blad] Proba wyslania po zamknietym polaczeniu.")
                            break
                except socket.timeout:
                    pass

                try:
                    current_time = time.strftime("%H:%M:%S")
                    conn.sendall(f"Time on Pi: {current_time}\n".encode('utf-8'))
                except (BrokenPipeError, ConnectionResetError):
                    print("[Blad] Polaczenie zostalo przerwane.")
                    break

                time.sleep(5)

        except Exception as e:
            print(f"[Blad polaczenia] {e}")

def start_server():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind((HOST, PORT))
        s.listen()
        print(f"[Serwer dziala] {HOST}:{PORT}")
        while True:
            conn, addr = s.accept()
            thread = threading.Thread(target=handle_client, args=(conn, addr), daemon=True)
            thread.start()

if __name__ == "__main__":
    start_server()
