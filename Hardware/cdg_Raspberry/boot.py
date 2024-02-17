import network
import machine
import time
from config import wifi_config

# Configuración del LED integrado
led_integrado = machine.Pin("LED", machine.Pin.OUT)

# Configuración de la red WiFi y MQTT
wlan = network.WLAN(network.STA_IF)
wlan.active(True)

wlan.ifconfig(('xxx', 'xxx', 'xxx', 'xxx')) # Mi wifi, ip estática para agilizar conexión.

wlan.connect(wifi_config['ssid'], wifi_config['p'])

# Intenta conectarte a la WiFi y verifica la conexión
tiempo_maximo = 20  # Tiempo máximo en segundos para intentar la conexión
tiempo_inicio = time.time()

while not wlan.isconnected():
    if time.time() - tiempo_inicio > tiempo_maximo:
        led_integrado.off()
        print("No se pudo conectar a la red WiFi.")
        raise RuntimeError("Fallo de conexión WiFi")
    time.sleep(1)

if wlan.isconnected():
    # Enciende el LED si la conexión es exitosa
    led_integrado.on()
    print("Conectado a la red WiFi.")
