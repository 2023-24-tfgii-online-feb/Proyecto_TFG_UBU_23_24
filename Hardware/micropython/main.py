from machine import Pin
from mi_mqtt import MQTT
import utime
from mi_modulos import OLEDDisplay_I2C, LedRGB
from mi_sensores import DHTSensor, SensorHumedadSuelo, Sensor_nivel_luz, actualizar_valores_sensores
from mi_telegram import TELEGRAM
#CONEXION WIFI
from mi_wifi import WIFI
from config import wifi_config
wifi = WIFI(wifi_config["ssid"],wifi_config["p"])
wifi.conectarse_wifi(tiempo_maximo=20) #tiempo maximo de espera=20 segundos
#SENSORES
oled            = OLEDDisplay_I2C(0,Pin(9),Pin(8),400000,128, 64)
dht             = DHTSensor(tipo_sensor="dht22", pin=3)
luminosidad     = Sensor_nivel_luz(0,Pin(9), Pin(8),400000,0x23) #address = 0x23
humedad_suelo   = SensorHumedadSuelo(pin=26)
#LEDS RGB
led_temperatura     = LedRGB(0,1,12) #red, blue, green
led_humedad_ambiente= LedRGB(4,5,13)
led_luminosidad     = LedRGB(6,7,14)
led_humedad_suelo   = LedRGB(10,11,15)
#MQTT y Bot de Telegram para enviar informacion
mqtt = MQTT(led_temperatura, led_humedad_ambiente, led_luminosidad, led_humedad_suelo)
bot = TELEGRAM(led_temperatura, led_humedad_ambiente, led_luminosidad, led_humedad_suelo)
#INTERVALO DE TIEMPO
next_send, send_interval = 0, 0.5
#FUNCIONES
def actualizar_next_send(tiempo_actual):
    next_send = tiempo_actual + send_interval #en segundos
def manage_sending(temp, hum, lux, porcentaje_humedad_suelo):#gestiona envia alertas y datos
    bot.enviar_alertas_telegram(temp, hum, lux, porcentaje_humedad_suelo)
    mqtt.enviar("", "", temp, hum, lux, porcentaje_humedad_suelo)
    actualizar_next_send(utime.time())
def apagar_todo(led_temp,led_hum,led_lum,led_hs,oled):
    led_temp.apagar_todos()
    led_hum.apagar_todos()
    led_lum.apagar_todos()
    led_hs.apagar_todos()
    oled.apagar()
#BUCLE PRINCIPAL
def main_loop():
    update_id = 0
    mqtt.setup_mqtt_subscription()
    ultimo_intento_reconexion = utime.time()
    actualizar_next_send(utime.time())
    while True:
        try:
            current_time = utime.time()
            if current_time - ultimo_intento_reconexion > 120: # Reconectar MQTT si es necesario
                mqtt.reconectar_mqtt()
                ultimo_intento_reconexion = current_time
            actualizar_valores_sensores(dht, humedad_suelo, luminosidad)
            oled.mostrar_datos_oled(dht.temperatura, dht.humedad, luminosidad.valor, humedad_suelo.valor)
            # Procesar mensajes de Telegram si hay alguno
            new_update_id, message = bot.read_message(update_id)
            if new_update_id != update_id and message:
                update_id = new_update_id
                bot.handle_telegram_commands(message['chat']['id'], message['text'],dht.temperatura,dht.humedad, luminosidad.valor, humedad_suelo.valor)
            # Enviar datos y alertas si es el momento
            if current_time >= next_send:
                manage_sending(dht.temperatura,dht.humedad, luminosidad.valor, humedad_suelo.valor)
                actualizar_next_send(current_time)
            #mqtt.client_sub.check_msg() # Verificar mensajes MQTT
            utime.sleep(0.1) # Pequeña pausa para evitar el uso excesivo de CPU
        except KeyboardInterrupt:
            apagar_todo(led_temperatura,led_humedad_ambiente,led_luminosidad,led_humedad_suelo,oled)
            print("Ejecución cancelada por el usuario.")
            break  # Salir del bucle while
        except Exception as e:
            print("Error en el bucle principal:", e)

if __name__ == "__main__":
    try:
        mqtt.enviar_umbrales()
        main_loop()
    except Exception as e:
        print(f"Error: {e}")
