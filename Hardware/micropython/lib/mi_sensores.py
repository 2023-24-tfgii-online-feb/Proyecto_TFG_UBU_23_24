from machine import Pin, I2C, ADC
from time import sleep
import dht
from bh1750 import BH1750
class DHTSensor:
    def __init__(self, tipo_sensor, pin):
        if(tipo_sensor == "dht11"): self.sensor = dht.DHT11(Pin(pin))
        if(tipo_sensor == "dht22"): self.sensor = dht.DHT22(Pin(pin))
        self.temperatura    = None
        self.humedad                = None
        self.actualizar_valores()
    def actualizar_valores(self):
        try:
            self.sensor.measure()
            self.temperatura    = self.sensor.temperature()
            self.humedad                = self.sensor.humidity()
            return self.temperatura, self.humedad
        except OSError as e:
            self.temperatura    = -1
            self.humedad                = -1
            print("DHT: Error al leer sensor")
    def mostrar_valores(self):
        self.actualizar_valores()
        print('Temperatura: %3.1f C' % self.temperatura)
        print('Humedad: %3.1f %%' % self.humedad,"\n")
    def mostrar_valores_repetidamente(self):
        while True:
            self.mostrar_valores()

class SensorHumedadSuelo:
    def __init__(self, pin):
        self.soil = ADC(Pin(pin))
        self.min_humedad= 44698 #65535
        self.max_humedad= 18324 #0
        self.adc_16bits = None
        self.valor      = None
        self.actualizar_valor()
    def actualizar_valor(self):
        adc_16bits      = self.soil.read_u16()
        self.valor      = (self.min_humedad - adc_16bits) * 100 / (self.min_humedad - self.max_humedad)
        self.valor      = float("%.2f" % self.valor)
        return self.valor
    def mostrar_valor(self):
        self.actualizar_valor()
        print(f"Humedad: {self.valor}")
    def mostrar_valor_continuamente(self, delay):
        while True:
            self.mostrar_valor()

class Sensor_nivel_luz:
    def __init__(self,Ni2c,scl,sda,freq,address):
        self.i2c = I2C(Ni2c,scl=scl,sda=sda,freq=freq)
        self.bh1750 = BH1750(address, self.i2c)
        self.valor  = None
    def actualizar_valor(self):
        self.valor  = self.bh1750.measurement
        return self.valor
    def mostrar_valor(self):
        self.actualizar_valor()
        print(f"Intensidad de luz: {self.valor}")
    def mostrar_valores_continuamente(self):
        while True:
            self.mostrar_valor()

def actualizar_valores_sensores(sensor_dht, sensor_humedad_suelo, sensor_luminosidad):
    sensor_dht.actualizar_valores()
    sensor_humedad_suelo.actualizar_valor()
    sensor_luminosidad.actualizar_valor()

#humedad_suelo   = SensorHumedadSuelo(pin=26)
#print(humedad_suelo.valor, type(humedad_suelo.valor))
