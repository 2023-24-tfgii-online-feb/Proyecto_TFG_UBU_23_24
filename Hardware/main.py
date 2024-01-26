from machine import Pin, I2C, ADC
from time import sleep
import dht
import ssd1306
from bh1750 import BH1750
from mq135 import MQ135
class DHTSensor:
    def __init__(self, tipo_sensor, pin):
        if(tipo_sensor == "dht11"): self.sensor = dht.DHT11(Pin(pin))
        if(tipo_sensor == "dht22"): self.sensor = dht.DHT22(Pin(pin))
        self.temperatura_celsius    = None
        self.humedad                = None
        self.actualizar_valores()
    def actualizar_valores(self):
        try:
            self.sensor.measure()
            self.temperatura_celsius    = self.sensor.temperature()
            self.humedad                = self.sensor.humidity()
            return self.temperatura_celsius, self.humedad
        except OSError as e:
            self.temperatura_celsius    = -1
            self.humedad                = -1
            print("DHT: Error al leer sensor")
    def mostrar_valores(self):
        self.actualizar_valores()
        print('Temperatura: %3.1f C' % self.temperatura_celsius)
        print('Humedad: %3.1f %%' % self.humedad,"\n")
    def mostrar_valores_repetidamente(self):
        while True:
            self.mostrar_valores()

class OLEDDisplay_I2C:
    def __init__(self,Ni2c,scl,sda,freq, width, height, address):
        self.i2c = I2C(Ni2c,scl=scl,sda=sda,freq=freq)
        self.oled = ssd1306.SSD1306_I2C(width, height, self.i2c, address)
        self.oled.fill(0) #limpiar
    def colocar_texto(self, entrada, x, y):
        text=str(entrada)
        self.oled.text(text, x, y)
    def mostrar(self):
        self.oled.show()
        self.oled.fill(0) #limpiar
class SensorAnalogico:
    def __init__(self, pin):
        self.soil = ADC(Pin(pin))
        self.min_value  = 0
        self.max_value  = 65535
        self.adc_16bits = None
        self.valor      = None
        self.actualizar_valor()
    def actualizar_valor(self):
        adc_16bits      = self.soil.read_u16()
        self.valor      = (self.max_value - adc_16bits) * 100 / (self.max_value - self.min_value)
        self.valor      = "%.2f" % self.valor
        return self.valor
    def mostrar_valor(self):
        self.actualizar_valor()
        print(f"valor: {self.valor}")
    def mostrar_valor_continuamente(self):
        while True:
            self.mostrar_valor()
class Sensor_nivel_luz:
    def __init__(self,Ni2c,scl,sda,freq,address):
        self.i2c = I2C(Ni2c,scl=scl,sda=sda,freq=freq)
        self.bh1750 = BH1750(address, self.i2c)
        self.intensidad = None
    def actualizar_valor(self):
        self.intensidad = self.bh1750.measurement
        return self.intensidad
    def mostrar_valor(self):
        self.actualizar_valor()
        print(f"Intensidad de luz: {self.intensidad}")
    def mostrar_valores_continuamente(self):
        while True:
            self.mostrar_valor()
if __name__ == "__main__":
    oled            = OLEDDisplay_I2C(0,Pin(9),Pin(8),400000,128, 64, 0x3c)
    dht             = DHTSensor(tipo_sensor="dht11", pin=3)
    humedad_suelo   = SensorAnalogico(pin=26)
    calidad_aire    = MQ135(27)
    nivel_luz       = Sensor_nivel_luz(0,Pin(9), Pin(8),400000,0x23) #address = 0x23
    while True:
        dht.actualizar_valores()
        humedad_suelo.actualizar_valor()
        nivel_luz.actualizar_valor()
        oled.colocar_texto("TEMPERATURA: "+str(dht.temperatura_celsius), 0, 0)
        oled.colocar_texto("HUMEDAD: "+str(dht.humedad), 0, 10)
        oled.colocar_texto("SUELO: "+str(humedad_suelo.valor), 0, 20)
        oled.colocar_texto("LUZ: "+str(nivel_luz.intensidad), 0, 30)
        oled.colocar_texto("AIRE: "+str(calidad_aire.get_corrected_ppm(22,60)), 0, 40)
        oled.mostrar()
