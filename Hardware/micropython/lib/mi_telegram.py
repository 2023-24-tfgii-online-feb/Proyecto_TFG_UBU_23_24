# from utelegram import ubot
# from config import utelegram_config
# bot = ubot(utelegram_config['token'])
# bot.send(utelegram_config['chat_id'],"Hola como estas")
import utelegram, urequests
from mi_modulos import LedRGB
from config import utelegram_config, umbrales_sensores
class TELEGRAM:
    def __init__(self,led_temperatura,led_humedad_ambiente,led_luminosidad,led_humedad_suelo):
        self.API_URL = "https://api.telegram.org/bot" + utelegram_config['token']
        self.bot = utelegram.ubot(utelegram_config['token'])
        self.TEMPERATURA_MINIMA     = umbrales_sensores['TEMPERATURA_MINIMA']
        self.HUMEDAD_MINIMA         = umbrales_sensores['HUMEDAD_MINIMA']
        self.LUMINOSIDAD_MINIMA     = umbrales_sensores['LUMINOSIDAD_MINIMA']
        self.HUMEDAD_SUELO_MINIMA   = umbrales_sensores['HUMEDAD_SUELO_MINIMA']
        self.TEMPERATURA_MAXIMA     = umbrales_sensores['TEMPERATURA_MAXIMA']
        self.HUMEDAD_MAXIMA         = umbrales_sensores['HUMEDAD_MAXIMA']
        self.LUMINOSIDAD_MAXIMA     = umbrales_sensores['LUMINOSIDAD_MAXIMA']
        self.HUMEDAD_SUELO_MAXIMA   = umbrales_sensores['HUMEDAD_SUELO_MAXIMA']
        self.estado_temperatura     = "normal"
        self.estado_humedad         = "normal"
        self.estado_luminosidad     = "normal"
        self.estado_humedad_suelo   = "normal"
        self.led_temperatura        = led_temperatura
        self.led_humedad_ambiente   = led_humedad_ambiente
        self.led_luminosidad        = led_luminosidad
        self.led_humedad_suelo      = led_humedad_suelo
    def control_led_especifico(self,led,estado):
        if estado == "alta":
            led.prender_red()
            led.apagar_blue()
        elif estado == "baja":
            led.apagar_red()
            led.prender_blue()
        else:  # estado normal
            led.apagar_red()
            led.apaga_blue()
    def control_led(self, sensor, estado):
        print("controlando leds")
        if sensor == "temperatura":
            self.control_led_especifico(self.led_temperatura,estado)
        elif sensor == "humedad_ambiente":
            self.control_led_especifico(self.led_humedad_ambiente,estado)
        elif sensor == "luminosidad":
            self.control_led_especifico(self.led_luminosidad,estado)
        elif sensor == "humedad_suelo":
            self.control_led_especifico(self.led_humedad_suelo,estado)
    def read_message(self, update_id):
        # Esta función intenta leer el próximo mensaje no leído de Telegram usando el bot.
        try:
            url = f"{self.API_URL}/getUpdates?limit=1&offset={update_id}"  # Construye la URL para la API de Telegram. 'limit=1' obtiene solo el último mensaje y 'offset' para manejar la actualización del ID.
            response = urequests.get(url)  # Realiza la solicitud HTTP GET a la API de Telegram.
            data = response.json()  # Convierte la respuesta en formato JSON a un diccionario de Python.
            response.close()  # Cierra la conexión HTTP.
            if data['result']:  # Verifica si hay algún mensaje nuevo en el resultado.
                update = data['result'][0]  # Obtiene el primer mensaje del resultado (el más reciente).
                update_id = update['update_id'] + 1  # Prepara el siguiente ID de actualización (útil para obtener el próximo mensaje no leído en futuras llamadas).
                return update_id, update['message']  # Devuelve el nuevo ID de actualización y el mensaje.
            return update_id, None  # En caso de no haber nuevos mensajes, devuelve el ID de actualización actual y None.
        except Exception as e:
            print("Error al leer mensaje de Telegram:", e)
            return update_id, None  # Devuelve el ID de actualización actual y None en caso de excepción.

    def send_message(self, chat_id, message):
        try:
            #reemplazar los espacios y saltos de línea en el mensaje para codificarlo correctamente en la URL.
            # Los espacios (' ') se convierten en '%20' y los saltos de línea ('\n') en '%0A'.
            encoded_message = message.replace(' ', '%20').replace('\n', '%0A')
            url = f"{self.API_URL}/sendMessage?chat_id={chat_id}&text={encoded_message}"
            urequests.get(url)
        except Exception as e:
            print("Error al enviar mensaje a Telegram:", e)
    def enviar_alertas_telegram(self, temp, hum, lux, porcentaje_humedad_suelo):
        # Declaración de variables globales para mantener el estado de los sensores.
        #global self.estado_temperatura, self.estado_humedad, self.estado_luminosidad, self.estado_humedad_suelo
        # Verificar y actualizar estado de la temperatura
        nuevo_estado_temperatura = "alta" if temp > self.TEMPERATURA_MAXIMA else "baja" if temp < self.TEMPERATURA_MINIMA else "normal"
        if nuevo_estado_temperatura != self.estado_temperatura:
            # Actualizar el estado de la temperatura y controlar el LED correspondiente.
            self.estado_temperatura = nuevo_estado_temperatura
            self.control_led("temperatura", self.estado_temperatura)
            print("control_led (temperatura)",self.estado_temperatura)
            # Crear y enviar el mensaje de Telegram con el estado actual de la temperatura.
            mensaje_temp = f"La temperatura es {self.estado_temperatura}: {temp} °C" if self.estado_temperatura == "normal" else f"Alerta, la temperatura es {self.estado_temperatura}: {temp} °C"
            self.bot.send(utelegram_config['chat_id'], mensaje_temp)
        # Verificar y actualizar estado de la humedad ambiente
        nuevo_estado_humedad="alta" if hum > self.HUMEDAD_MAXIMA else "baja" if hum < self.HUMEDAD_MINIMA else "normal"
        if nuevo_estado_humedad != self.estado_humedad:
            # Actualizar el estado de la humedad ambiente y controlar el LED correspondiente.
            self.estado_humedad = nuevo_estado_humedad
            self.control_led("humedad_ambiente", self.estado_humedad)
            print("control_led (humedad_ambiente)",self.estado_humedad)
            # Crear y enviar el mensaje de Telegram con el estado actual de la humedad ambiente.
            mensaje_hum = f"La humedad ambiente es {self.estado_humedad}: {hum} %" if self.estado_humedad == "normal" else f"Alerta, la humedad ambiente es {self.estado_humedad}: {hum} %"
            self.bot.send(utelegram_config['chat_id'], mensaje_hum)
        # Verificar y actualizar estado de la luminosidad
        nuevo_estado_luminosidad = "alta" if lux > self.LUMINOSIDAD_MAXIMA else "baja" if lux < self.LUMINOSIDAD_MINIMA else "normal"
        if nuevo_estado_luminosidad != self.estado_luminosidad:
            # Actualizar el estado de la luminosidad y controlar el LED correspondiente.
            self.estado_luminosidad = nuevo_estado_luminosidad
            self.control_led("luminosidad", self.estado_luminosidad)
            print("control_led (luminosidad)",self.estado_luminosidad)
            # Crear y enviar el mensaje de Telegram con el estado actual de la luminosidad.
            mensaje_luz = f"La luminosidad es {self.estado_luminosidad}: {lux} lux" if self.estado_luminosidad == "normal" else f"Alerta, la luminosidad es {self.estado_luminosidad}: {lux} lux"
            self.bot.send(utelegram_config['chat_id'], mensaje_luz)

        # Verificar y actualizar estado de la humedad del suelo
        nuevo_estado_humedad_suelo = "alta" if porcentaje_humedad_suelo > self.HUMEDAD_SUELO_MAXIMA else "baja" if porcentaje_humedad_suelo < self.HUMEDAD_SUELO_MINIMA else "normal"
        if nuevo_estado_humedad_suelo != self.estado_humedad_suelo:
            # Actualizar el estado de la humedad del suelo y controlar el LED correspondiente.
            self.estado_humedad_suelo = nuevo_estado_humedad_suelo
            self.control_led("humedad_suelo", self.estado_humedad_suelo)
            print("control_led (humedad_suelo)",self.estado_humedad_suelo)
            # Crear y enviar el mensaje de Telegram con el estado actual de la humedad del suelo.
            mensaje_hum_suelo = f"La humedad del suelo es {self.estado_humedad_suelo}: {porcentaje_humedad_suelo} %" if self.estado_humedad_suelo == "normal" else f"Alerta, la humedad del suelo es {self.estado_humedad_suelo}: {porcentaje_humedad_suelo} %"
            self.bot.send(utelegram_config['chat_id'], mensaje_hum_suelo)

    def handle_telegram_commands(self, chat_id, text,temp,hum,lux,porcentaje_humedad_suelo):
        # Esta función gestiona los comandos recibidos a través de Telegram.
        print(text)
        if text == "/start": #bienvenida al usuario con la lista de comandos disponibles
            self.send_message(chat_id, "Bienvenido a Inver IoT Bot. ¿Qué deseas saber?\nLas opciones le darán el valor actual del sensor.\n1) Temperatura: /temp\n2) Humedad Ambiente: /huma\n3) Luminosidad: /lum\n4) Humedad Suelo: /hums\n5) Ver todos los valores: /todos\n* ACTIVAR MECANISMOS *\n6) Temperatura: /t_ON /t_OFF\n7)Humedad ambiente: /ha_ON /ha_OFF\n8)Luminosidad: /l_ON /l_OFF\n9)Humedad suelo: /hs_ON /hs_OFF")
        elif text == "/temp": #temperatura
            self.send_message(chat_id, f"Temperatura actual: {temp} °C")
        elif text == "/huma": #humedad ambiente
            self.send_message(chat_id, f"Humedad ambiente: {hum} %")
        elif text == "/lum":  #luminosidad
            self.send_message(chat_id, f"Luminosidad actual: {lux} lux")
        elif text == "/hums": #humedad del suelo
            self.send_message(chat_id, f"Humedad del suelo: {porcentaje_humedad_suelo} %")
        elif text == "/todos":#envia todos los valores actuales
            self.send_message(chat_id, f"Temperatura actual: {temp} °C\nHumedad Ambiente: {hum} %\nLuminosidad: {lux} lux\nHumedad del suelo: {porcentaje_humedad_suelo} %")
        elif text == "/t_ON":
            self.led_temperatura.prender_green()
        elif text == "/t_OFF":
            self.led_temperatura.apagar_green()
        elif text == "/ha_ON":
            self.led_humedad_ambiente.prender_green()
        elif text == "/ha_OFF":
            self.led_humedad_ambiente.apagar_green()
        elif text == "/l_ON":
            self.led_luminosidad.prender_green()
        elif text == "/l_OFF":
            self.led_luminosidad.apagar_green()
        elif text == "/hs_ON":
            self.led_humedad_suelo.prender_green()
        elif text == "/hs_OFF":
            self.led_humedad_suelo.apagar_green()
        else:
            self.send_message(chat_id, "Comando no reconocido.")

#LEDS RGB
# led_temperatura     = LedRGB(0,1,12) #red, blue, green
# led_humedad_ambiente= LedRGB(4,5,13)
# led_luminosidad     = LedRGB(6,7,14)
# led_humedad_suelo   = LedRGB(10,11,15)
# bot = TELEGRAM(led_temperatura, led_humedad_ambiente, led_luminosidad, led_humedad_suelo)
#bot.enviar_alertas_telegram(1,2,3,4)
#bot.control_led("temperatura","alta")
# update_id = 0
# new_update_id, message = bot.read_message(update_id)
# if new_update_id != update_id and message:
    # update_id = new_update_id
    # bot.handle_telegram_commands(message['chat']['id'], message['text'])
