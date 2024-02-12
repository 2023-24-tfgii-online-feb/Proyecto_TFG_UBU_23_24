from machine import Pin, I2C
from sh1106 import SH1106_I2C
import time
class OLEDDisplay_I2C:
    def __init__(self,Ni2c,scl,sda,freq, width, height):
        self.i2c = I2C(Ni2c,scl=scl,sda=sda,freq=freq)
        self.oled = SH1106_I2C(width, height, self.i2c)
        self.oled.fill(0) #limpiar
        self.oled.flip(1) #invertir respecto al eje X
    def colocar_texto(self, entrada, x, y):
        text=str(entrada)
        self.oled.text(text, x, y)
    def mostrar(self):
        self.oled.show()
        self.oled.fill(0) #limpiar
    def apagar(self):
        self.oled.poweroff()
    def prender(self):
        self.oled.poweron()
    def mostrar_datos_oled(self, temp, hum, luz, hum_suelo):
        self.oled.fill(0)
        self.oled.text("Datos sensores:", 0, 0)
        temp_text = f"Temp: {temp} 'C"
        hum_text = f"Hum Amb.: {hum} %"
        luz_text = f"Luz: {luz} lux"
        hum_suelo_text = f"Hum Sue.: {hum_suelo} %"
        texts = [temp_text, hum_text, luz_text, hum_suelo_text]
        for i, text in enumerate(texts):
            self.oled.text(text, 0, 10 * (i + 2))
        self.oled.show()
# oled = OLEDDisplay_I2C(0,Pin(9),Pin(8),400000,128, 64)
# oled.colocar_texto("hola 1234",20,20)
# oled.mostrar()

class LedRGB:
    def __init__(self, gpio_red, gpio_blue, gpio_green):
        self.gpio_red, self.gpio_blue, self.gpio_green = gpio_red, gpio_blue, gpio_green
        self.red, self.blue, self.green = None, None, None
        self.declarar_leds_output()
        self.apagar_todos()
    def declarar_leds_output(self):
        self.red    = Pin(self.gpio_red, Pin.OUT)
        self.blue   = Pin(self.gpio_blue, Pin.OUT)
        self.green  = Pin(self.gpio_green, Pin.OUT)
    def prender(self, led):
        led.value(1)
    def apagar(self, led):
        led.value(0)
    def apagar_todos(self):
        self.red.value(0)
        self.blue.value(0)
        self.green.value(0)
    def intermitente(self, led):
        self.apagar_todos()
        while True:
            led.value(1)
            time.sleep(1)
            led.value(0)
            time.sleep(1)
    def prender_todos(self):
        self.prender_red()
        self.prender_blue()
        self.prender_green()
    def prender_red(self):
        self.red.value(1)
    def prender_blue(self):
        self.blue.value(1)
    def prender_green(self):
        self.green.value(1)
    def apagar_red(self):
        self.red.value(0)
    def apagar_blue(self):
        self.blue.value(0)
    def apagar_green(self):
        self.green.value(0)
#led_temperatura     = LedRGB(0,1,12) #red, blue, green
#led_humedad_ambiente = LedRGB(4,5,13)
#led_luminosidad     = LedRGB(6,7,14)
#led_humedad_suelo   = LedRGB(10,11,15)
#led_temperatura.prender_todos()
#led_humedad_ambiente.prender_todos()
#led_luminosidad.prender_todos()
#led_humedad_suelo.prender_todos()
#led_temperatura.apagar_todos()
#led_temperatura.prender_red()
