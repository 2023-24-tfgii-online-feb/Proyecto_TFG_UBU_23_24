import network, time, machine
from machine import Pin
from config import wlan_config
#from config import wifi_config
class WIFI:
    def __init__(self,ssid,password):
        self.wlan = None
        self.ssid = ssid
        self.password = password
        self.led_integrado = machine.Pin("LED", Pin.OUT)
    def activar_wlan(self,tipo):
        if tipo=="estacion": #STATION, se conecta a una red wifi
            self.wlan = network.WLAN(network.STA_IF) #tipo Estacion, para conectarse a una red wifi
        elif tipo=="AP": #ACCESS POINT, crea su propia red wifi
            self.wlan = network.WLAN(network.AP_IF)
        self.wlan.active(True) #activates the wlan interface 
    def ver_redes_wifi(self):
        self.activar_wlan("estacion")
        accessPoints = self.wlan.scan() #perform a WiFi Access Points scan 
        for ap in accessPoints: #this loop prints each AP found in a single row on shell     
            print(ap)
    def conectarse_wifi(self,tiempo_maximo):
        self.activar_wlan("estacion")
        self.wlan.ifconfig((wlan_config['ip'],wlan_config['mask'], wlan_config['gateway'], wlan_config['dns'])) #ip estatica
        self.wlan.connect(self.ssid, self.password)
        tiempo_inicio=time.time()
        while not self.wlan.isconnected() and self.wlan.status() >= 0:
            self.led_integrado.off()
            print("Esperando para conectarse:")
            time.sleep(1)
            print(self.wlan.ifconfig())
            if time.time() - tiempo_inicio > tiempo_maximo:
                print("No se pudo conectar a la red WiFi.")
                raise RuntimeError("Fallo de conexión WiFi")
        if self.wlan.isconnected():
            self.led_integrado.on() # Enciende el LED si la conexión es exitosa
            print("Conectado a la red WiFi.")

#wifi = WIFI(wifi_config["ssid"],wifi_config["password"])
#wifi.ver_redes_wifi()
#wifi.conectarse_wifi(tiempo_maximo=20) #tiempo maximo de espera=20 segundos
