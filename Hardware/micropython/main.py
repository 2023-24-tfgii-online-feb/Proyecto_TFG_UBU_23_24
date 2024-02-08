from machine import Pin, I2C, ADC, Pin  # Importa clases para manejar pines digitales, I2C, ADC (Convertidor Analógico a Digital) de la biblioteca 'machine'.
import dht  # Importa la biblioteca para manejar sensores DHT (sensores de temperatura y humedad).
import network, utelegram, utime, urequests, umqtt as mqtt  # Importa varias bibliotecas para manejar red, un cliente Telegram, utilidades de tiempo, solicitudes HTTP y MQTT.
from bh1750 import BH1750  # Importa la clase BH1750 de la biblioteca bh1750 para manejar el sensor de luminosidad BH1750.
from sh1106 import SH1106_I2C  # Importa la clase SH1106_I2C para manejar la pantalla OLED SH1106 a través de I2C.
from config import utelegram_config, umbrales_sensores, mqtt_config  # Importa variables de configuración específicas (para Telegram y umbrales de sensores) desde un archivo de configuración 'config.py'.

# Importar umbrales desde config.py
TEMPERATURA_MINIMA = umbrales_sensores['TEMPERATURA_MINIMA']  # Establece el umbral mínimo de temperatura a partir de la configuración importada de 'config.py'.
HUMEDAD_MINIMA = umbrales_sensores['HUMEDAD_MINIMA']  # Establece el umbral mínimo de humedad a partir de la configuración importada de 'config.py'.
LUMINOSIDAD_MINIMA = umbrales_sensores['LUMINOSIDAD_MINIMA']  # Establece el umbral mínimo de luminosidad a partir de la configuración importada de 'config.py'.
HUMEDAD_SUELO_MINIMA = umbrales_sensores['HUMEDAD_SUELO_MINIMA']  # Establece el umbral mínimo de humedad del suelo a partir de la configuración importada de 'config.py'.

TEMPERATURA_MAXIMA = umbrales_sensores['TEMPERATURA_MAXIMA']  # Establece el umbral máximo de temperatura a partir de la configuración importada de 'config.py'.
HUMEDAD_MAXIMA = umbrales_sensores['HUMEDAD_MAXIMA']  # Establece el umbral máximo de humedad a partir de la configuración importada de 'config.py'.
LUMINOSIDAD_MAXIMA = umbrales_sensores['LUMINOSIDAD_MAXIMA']  # Establece el umbral máximo de luminosidad a partir de la configuración importada de 'config.py'.
HUMEDAD_SUELO_MAXIMA = umbrales_sensores['HUMEDAD_SUELO_MAXIMA']  # Establece el umbral máximo de humedad del suelo a partir de la configuración importada de 'config.py'.

# Configuración del cliente MQTT
mqtt_server = mqtt_config['server']
mqtt_port = mqtt_config['port']
mqtt_topic_pub = mqtt_config['topic_pub']
mqtt_topic_sub = mqtt_config['topic_sub']

client_pub = mqtt.MQTTClient(mqtt_config['client_id_pub'], mqtt_server, mqtt_port)
client_sub = mqtt.MQTTClient(mqtt_config['client_id_sub'], mqtt_server, mqtt_port)

# Inicialización de sensores y dispositivos, pines
d = dht.DHT22(machine.Pin(3))  # Inicializar el sensor DHT22 en el pin 3. Este sensor mide la temperatura y humedad ambiental.
pin_salida_lamp = Pin(11, Pin.OUT)  # Configurar el pin 11 como salida para controlar una lámpara (u otro dispositivo).
pin_salida_riego = Pin(15, Pin.OUT)  # Configurar el pin 15 como salida para controlar un sistema de riego (u otro dispositivo).
i2c = I2C(0, scl=Pin(9), sda=Pin(8), freq=400000)  # Inicializar el bus I2C con el SCL en el pin 9, SDA en el pin 8 y una frecuencia de 400kHz.
oled = SH1106_I2C(128, 64, i2c)  # Inicializar una pantalla OLED SH1106 (128x64 píxeles) utilizando el bus I2C configurado anteriormente.
bh1750 = BH1750(0x23, i2c)  # Inicializar el sensor de luminosidad BH1750 con dirección I2C 0x23, usando el mismo bus I2C.
sensor_humedad_suelo = ADC(Pin(26))  # Inicializar un sensor de humedad del suelo en el pin analógico 26.
minima_humedad, maxima_humedad = 44698, 18324  # Establecer valores para la calibración de la lectura de humedad del suelo. Estos valores se usan para convertir la lectura analógica en porcentaje.

# Estados iniciales para cada sensores
estado_temperatura = "normal"
estado_humedad = "normal"
estado_luminosidad = "normal"
estado_humedad_suelo = "normal"

#### --- INICIO LEDS --- ###

# Inicialización de pines LED RGB de Temperatura
temp_pin_led_rojo = Pin(0, Pin.OUT)  # Rojo
temp_pin_led_azul = Pin(1, Pin.OUT)  # Azul

# Apagamos ambos al inicio
temp_pin_led_rojo.low()
temp_pin_led_azul.low()

# Inicialización de pines LED RGB de Humedad ambiente
humambiente_pin_led_rojo = Pin(4, Pin.OUT)  # Rojo
humambiente_pin_led_azul = Pin(5, Pin.OUT)  # Azul

# Apagamos ambos al inicio
humambiente_pin_led_rojo.low()
humambiente_pin_led_azul.low()

# Inicialización de pines LED RGB de Luminosidad
lum_pin_led_rojo = Pin(6, Pin.OUT)  # Rojo
lum_pin_led_azul = Pin(7, Pin.OUT)  # Azul

# Apagamos ambos al inicio
lum_pin_led_rojo.low()
lum_pin_led_azul.low()

# Inicialización de pines LED RGB de Humedad del suelo
humsuelo_pin_led_rojo = Pin(10, Pin.OUT)  # Rojo
humsuelo_pin_led_azul = Pin(11, Pin.OUT)  # Azul

# Apagamos ambos al inicio
humsuelo_pin_led_rojo.low()
humsuelo_pin_led_azul.low()

### INICIO LEDS VERDES ###
### Estos leds se encederán con una instrucción desde fuera
### En vez de activar un mecanismo, se activa la luz verde

temp_pin_led_verde = Pin(12, Pin.OUT)  # Verde
humambiente_pin_led_verde = Pin(13, Pin.OUT)  # Verde
lum_pin_led_verde = Pin(14, Pin.OUT)  # Verde
humsuelo_pin_led_verde = Pin(15, Pin.OUT)  # Verde

### --- FIN LEDS --- ###

# Configuración global
# send_interval = 1 # Intervalo de envío de mensajes en segundos, 900 son 15 minutos, lo pongo a 2 para probar
send_interval = 0.5 # Intervalo de envío de mensajes en segundos, 900 son 15 minutos, lo pongo a 2 para probar
next_send = utime.time() + send_interval

# Telegram API url.
API_URL = "https://api.telegram.org/bot" + utelegram_config['token']

def control_led(sensor, estado):
    # Esta función se utiliza para controlar los LEDs en función del estado de los sensores.

    if sensor == "temperatura":
        # Controla los LEDs para el sensor de temperatura.
        if estado == "alta":
            # Si la temperatura es alta, enciende el LED rojo y apaga el azul.
            temp_pin_led_rojo.high()
            temp_pin_led_azul.low()
        elif estado == "baja":
            # Si la temperatura es baja, enciende el LED azul y apaga el rojo.
            temp_pin_led_rojo.low()
            temp_pin_led_azul.high()
        else:  # estado normal
            # Si la temperatura está en un rango normal, apaga ambos LEDs.
            temp_pin_led_rojo.low()
            temp_pin_led_azul.low()

    elif sensor == "humedad_ambiente":
        # Controla los LEDs para el sensor de humedad ambiente.
        if estado == "alta":
            # Si la humedad ambiente es alta, enciende el LED rojo y apaga el azul.
            humambiente_pin_led_rojo.high()
            humambiente_pin_led_azul.low()
        elif estado == "baja":
            # Si la humedad ambiente es baja, enciende el LED azul y apaga el rojo.
            humambiente_pin_led_rojo.low()
            humambiente_pin_led_azul.high()
        else:  # estado normal
            # Si la humedad ambiente está en un rango normal, apaga ambos LEDs.
            humambiente_pin_led_rojo.low()
            humambiente_pin_led_azul.low()

    elif sensor == "luminosidad":
        # Controla los LEDs para el sensor de luminosidad.
        if estado == "alta":
            # Si la luminosidad es alta, enciende el LED rojo y apaga el azul.
            lum_pin_led_rojo.high()
            lum_pin_led_azul.low()
        elif estado == "baja":
            # Si la luminosidad es baja, enciende el LED azul y apaga el rojo.
            lum_pin_led_rojo.low()
            lum_pin_led_azul.high()
        else:  # estado normal
            # Si la luminosidad está en un rango normal, apaga ambos LEDs.
            lum_pin_led_rojo.low()
            lum_pin_led_azul.low()

    elif sensor == "humedad_suelo":
        # Controla los LEDs para el sensor de humedad del suelo.
        if estado == "alta":
            # Si la humedad del suelo es alta, enciende el LED rojo y apaga el azul.
            humsuelo_pin_led_rojo.high()
            humsuelo_pin_led_azul.low()
        elif estado == "baja":
            # Si la humedad del suelo es baja, enciende el LED azul y apaga el rojo.
            humsuelo_pin_led_rojo.low()
            humsuelo_pin_led_azul.high()
        else:  # estado normal
            # Si la humedad del suelo está en un rango normal, apaga ambos LEDs.
            humsuelo_pin_led_rojo.low()
            humsuelo_pin_led_azul.low()

def enviar_alertas_telegram(temp, hum, lux, porcentaje_humedad_suelo, bot):
    # Declaración de variables globales para mantener el estado de los sensores.
    global estado_temperatura, estado_humedad, estado_luminosidad, estado_humedad_suelo
    
    # Verificar y actualizar estado de la temperatura
    nuevo_estado_temperatura = "alta" if temp > TEMPERATURA_MAXIMA else "baja" if temp < TEMPERATURA_MINIMA else "normal"
    if nuevo_estado_temperatura != estado_temperatura:
        # Actualizar el estado de la temperatura y controlar el LED correspondiente.
        estado_temperatura = nuevo_estado_temperatura
        control_led("temperatura", estado_temperatura)
        # Crear y enviar el mensaje de Telegram con el estado actual de la temperatura.
        mensaje_temp = f"La temperatura es {estado_temperatura}: {temp} °C" if estado_temperatura == "normal" else f"Alerta, la temperatura es {estado_temperatura}: {temp} °C"
        bot.send(utelegram_config['chat_id'], mensaje_temp)

    # Verificar y actualizar estado de la humedad ambiente
    nuevo_estado_humedad = "alta" if hum > HUMEDAD_MAXIMA else "baja" if hum < HUMEDAD_MINIMA else "normal"
    if nuevo_estado_humedad != estado_humedad:
        # Actualizar el estado de la humedad ambiente y controlar el LED correspondiente.
        estado_humedad = nuevo_estado_humedad
        control_led("humedad_ambiente", estado_humedad)
        # Crear y enviar el mensaje de Telegram con el estado actual de la humedad ambiente.
        mensaje_hum = f"La humedad ambiente es {estado_humedad}: {hum} %" if estado_humedad == "normal" else f"Alerta, la humedad ambiente es {estado_humedad}: {hum} %"
        bot.send(utelegram_config['chat_id'], mensaje_hum)

    # Verificar y actualizar estado de la luminosidad
    nuevo_estado_luminosidad = "alta" if lux > LUMINOSIDAD_MAXIMA else "baja" if lux < LUMINOSIDAD_MINIMA else "normal"
    if nuevo_estado_luminosidad != estado_luminosidad:
        # Actualizar el estado de la luminosidad y controlar el LED correspondiente.
        estado_luminosidad = nuevo_estado_luminosidad
        control_led("luminosidad", estado_luminosidad)
        # Crear y enviar el mensaje de Telegram con el estado actual de la luminosidad.
        mensaje_luz = f"La luminosidad es {estado_luminosidad}: {lux} lux" if estado_luminosidad == "normal" else f"Alerta, la luminosidad es {estado_luminosidad}: {lux} lux"
        bot.send(utelegram_config['chat_id'], mensaje_luz)

    # Verificar y actualizar estado de la humedad del suelo
    nuevo_estado_humedad_suelo = "alta" if porcentaje_humedad_suelo > HUMEDAD_SUELO_MAXIMA else "baja" if porcentaje_humedad_suelo < HUMEDAD_SUELO_MINIMA else "normal"
    if nuevo_estado_humedad_suelo != estado_humedad_suelo:
        # Actualizar el estado de la humedad del suelo y controlar el LED correspondiente.
        estado_humedad_suelo = nuevo_estado_humedad_suelo
        control_led("humedad_suelo", estado_humedad_suelo)
        # Crear y enviar el mensaje de Telegram con el estado actual de la humedad del suelo.
        mensaje_hum_suelo = f"La humedad del suelo es {estado_humedad_suelo}: {porcentaje_humedad_suelo} %" if estado_humedad_suelo == "normal" else f"Alerta, la humedad del suelo es {estado_humedad_suelo}: {porcentaje_humedad_suelo} %"
        bot.send(utelegram_config['chat_id'], mensaje_hum_suelo)

# Función que reconecta, ya que mqtt simple no incluye isconnect
def reconectar_mqtt():
    try:
        client_pub.connect()
        #print("Intento de reconexión MQTT (publicación) realizado")
    except Exception as e:
        print("Error al reconectar MQTT (publicación):", e)

    try:
        client_sub.connect()
        #print("Intento de reconexión MQTT (suscripción) realizado")
    except Exception as e:
        print("Error al reconectar MQTT (suscripción):", e)

def apagar_todo():
    # Iterar sobre cada LED definido para apagarlos.
    # Esto establece la salida de cada pin LED a 'bajo' (off).
    for led in [temp_pin_led_rojo, temp_pin_led_azul, temp_pin_led_verde, lum_pin_led_rojo, lum_pin_led_azul, lum_pin_led_verde, 
                humambiente_pin_led_rojo, humambiente_pin_led_azul, humambiente_pin_led_verde, 
                humsuelo_pin_led_rojo, humsuelo_pin_led_azul, humsuelo_pin_led_verde]:
        led.low()

    # Apagar la pantalla OLED para reducir el consumo de energía.
    oled.poweroff()

def mostrar_datos_oled(temp, hum, luz, hum_suelo):
    # Esta función muestra los datos de los sensores en la pantalla OLED.
    oled.fill(0)  # Limpia la pantalla OLED rellenándola con el color negro (0).

    oled.text("Datos sensores:", 0, 0)  # Muestra el texto "Datos sensores:" en la parte superior izquierda de la pantalla OLED (coordenadas 0,0).

    # Agregamos los símbolos y unidades a cada valor
    temp_text = f"Temp: {temp} 'C"  # Prepara el texto para mostrar la temperatura con el símbolo '°C'.
    hum_text = f"Hum Amb.: {hum} %"  # Prepara el texto para mostrar la humedad del ambiente con el símbolo '%'.
    luz_text = f"Luz: {luz} lux"  # Prepara el texto para mostrar la intensidad de luz en lux.
    hum_suelo_text = f"Hum Sue.: {hum_suelo} %"  # Prepara el texto para mostrar la humedad del suelo con el símbolo '%'.

    # Creamos una lista con los textos a mostrar
    texts = [temp_text, hum_text, luz_text, hum_suelo_text]  # Almacena todos los textos en una lista para iterar sobre ellos.

    # Mostramos cada texto en la pantalla OLED
    for i, text in enumerate(texts):
        # Itera sobre cada texto en la lista y los muestra en la pantalla OLED.
        # El índice 'i' se usa para colocar cada texto en una nueva línea en la pantalla.
        oled.text(text, 0, 10 * (i + 2))  # El texto se muestra a 10 píxeles de altura por cada línea, comenzando desde la línea 2 (debido a 'i + 2').
    
    oled.show()  # Actualiza la pantalla OLED para mostrar todos los textos agregados.

def read_message(bot, update_id):
    # Esta función intenta leer el próximo mensaje no leído de Telegram usando el bot.

    try:
        url = f"{API_URL}/getUpdates?limit=1&offset={update_id}"  # Construye la URL para la API de Telegram. 'limit=1' obtiene solo el último mensaje y 'offset' para manejar la actualización del ID.
        response = urequests.get(url)  # Realiza la solicitud HTTP GET a la API de Telegram.
        data = response.json()  # Convierte la respuesta en formato JSON a un diccionario de Python.
        response.close()  # Cierra la conexión HTTP.

        if data['result']:  # Verifica si hay algún mensaje nuevo en el resultado.
            update = data['result'][0]  # Obtiene el primer mensaje del resultado (el más reciente).
            update_id = update['update_id'] + 1  # Prepara el siguiente ID de actualización (útil para obtener el próximo mensaje no leído en futuras llamadas).
            return update_id, update['message']  # Devuelve el nuevo ID de actualización y el mensaje.
        return update_id, None  # En caso de no haber nuevos mensajes, devuelve el ID de actualización actual y None.
    except Exception as e:
        print("Error al leer mensaje de Telegram:", e)  # Imprime un mensaje de error si algo falla en el proceso.
        return update_id, None  # Devuelve el ID de actualización actual y None en caso de excepción.

# Funciones del programa
def enviar(fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo):
    fecha_actual, hora_actual = obtener_fecha_hora()
    try:
        client_pub.connect()
        mensaje = f'{fecha_actual},{hora_actual},{temperatura},{humedad},{intensidad_luz},{humedad_suelo}'
        client_pub.publish(mqtt_topic_pub, mensaje.encode())
        client_pub.disconnect()
    except OSError as e:
        print("Error de conexión MQTT al enviar datos:", e)
    except Exception as e:
        print("Error al enviar datos por MQTT:", e)

# Obtener fecha actual
def obtener_fecha_hora():
    try:
        response = urequests.get('http://worldtimeapi.org/api/ip')
        if response.status_code == 200:
            datetime = response.json()['datetime'].split('T')
            return datetime[0], datetime[1].split('.')[0]
        else:
            print("Error al obtener fecha y hora: código de estado HTTP", response.status_code)
            return None, None
    except Exception as e:
        print("Error al obtener fecha y hora:", e)
        return None, None

def handle_mqtt_messages(topic, msg):
    # Esta función es un 'callback' que se ejecuta cuando se recibe un mensaje MQTT, dependiendo lo que reciba, hago una cosa u otra.
    mensaje = msg.decode().lstrip()  # Decodifica el mensaje MQTT (que se recibe en bytes) a una cadena de texto y elimina los espacios al principio.
    print("Mensaje MQTT recibido:", mensaje)  # Imprime el mensaje MQTT recibido en la consola.
    
    # Aquí es donde funciona la lógica de los mensajes recibidos por mqtt
    if mensaje == "/t_ON":
        # Se enciende el led (podría ser un mecanismo, como activar ventilador, abrir ventana...)
        temp_pin_led_verde.high()        
    elif mensaje == "/t_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        temp_pin_led_verde.low()
    elif mensaje == "/ha_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        humambiente_pin_led_verde.high()
    elif mensaje == "/ha_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        humambiente_pin_led_verde.low()
    elif mensaje == "/l_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        lum_pin_led_verde.high()
    elif mensaje == "/l_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        lum_pin_led_verde.low()
    elif mensaje == "/hs_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        humsuelo_led_verde.high()
    elif mensaje == "/hs_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        humsuelo_pin_led_verde.low()
    else:
        # Si el comando recibido no se reconoce, envía un mensaje indicando que el comando no es reconocido.
        print("Comando no reconocido.")

def manage_sending(next_send):
    # Esta función gestiona el envío de alertas y datos a través de Telegram y MQTT.
    global next_send
    # Llama a la función enviar_alertas_telegram para verificar si los valores actuales de los sensores están fuera de los umbrales definidos y, en caso afirmativo, enviar alertas a Telegram.
    enviar_alertas_telegram(temp, hum, lux, porcentaje_humedad_suelo, bot)

    # Llama a la función enviar para publicar los datos actuales de los sensores a través de MQTT.
    # Aquí se pasan valores vacíos para fecha y hora ya que estos se obtienen dentro de la función.
    enviar("", "", temp, hum, lux, porcentaje_humedad_suelo)

    # Actualiza el momento en el que se debe realizar el próximo envío de datos.
    # Se suma el intervalo de envío a la hora actual para programar el próximo envío.
    next_send = utime.time() + send_interval

def send_message(chat_id, message):
    # Esta función se utiliza para enviar mensajes a través del bot de Telegram.

    try:
        # Primero, se reemplazan los espacios y saltos de línea en el mensaje para codificarlo correctamente en la URL.
        # Los espacios (' ') se convierten en '%20' y los saltos de línea ('\n') en '%0A'.
        encoded_message = message.replace(' ', '%20').replace('\n', '%0A')

        # Se construye la URL para la API de Telegram utilizando el chat_id y el mensaje codificado.
        # API_URL es una variable global que contiene la URL base de la API de Telegram para el bot.
        url = f"{API_URL}/sendMessage?chat_id={chat_id}&text={encoded_message}"

        # Se envía la solicitud GET a la URL de la API de Telegram.
        urequests.get(url)

    except Exception as e:
        # Si ocurre un error durante el envío del mensaje, se imprime un mensaje de error.
        print("Error al enviar mensaje a Telegram:", e)

def handle_telegram_commands(chat_id, text):
    # Esta función gestiona los comandos recibidos a través de Telegram.

    if text == "/start":
        # Si el comando recibido es "/start", envía un mensaje de bienvenida al usuario con una lista de comandos disponibles.
        send_message(chat_id, "Bienvenido a Inver IoT Bot. ¿Qué deseas saber?\nLas opciones le darán el valor actual del sensor.\n1) Temperatura: /temp\n2) Humedad Ambiente: /huma\n3) Luminosidad: /lum\n4) Humedad Suelo: /hums\n5) Ver todos los valores: /todos\n* ACTIVAR MECANISMOS *\n6) Temperatura: /t_ON /t_OFF\n7)Humedad ambiente: /ha_ON /ha_OFF\n8)Luminosidad: /l_ON /l_OFF\n9)Humedad suelo: /hs_ON /hs_OFF")
    
    elif text == "/temp":
        # Si el comando recibido es "/temp", envía el valor actual de la temperatura al usuario.
        send_message(chat_id, f"Temperatura actual: {temp} °C")
    
    elif text == "/huma":
        # Si el comando recibido es "/huma", envía el valor actual de la humedad ambiente al usuario.
        send_message(chat_id, f"Humedad ambiente: {hum} %")
    
    elif text == "/lum":
        # Si el comando recibido es "/lum", envía el valor actual de la luminosidad al usuario.
        send_message(chat_id, f"Luminosidad actual: {lux} lux")
    
    elif text == "/hums":
        # Si el comando recibido es "/hums", envía el valor actual de la humedad del suelo al usuario.
        send_message(chat_id, f"Humedad del suelo: {porcentaje_humedad_suelo} %")
    
    elif text == "/todos":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        send_message(chat_id, f"Temperatura actual: {temp} °C\nHumedad Ambiente: {hum} %\nLuminosidad: {lux} lux\nHumedad del suelo: {porcentaje_humedad_suelo} %")
    elif text == "/t_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        temp_pin_led_verde.high()
    elif text == "/t_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        temp_pin_led_verde.low()
    elif text == "/ha_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        humambiente_pin_led_verde.high()
    elif text == "/ha_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        humambiente_pin_led_verde.low()
    elif text == "/l_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        lum_pin_led_verde.high()
    elif text == "/l_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        lum_pin_led_verde.low()
    elif text == "/hs_ON":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.
        humsuelo_led_verde.high()
    elif text == "/hs_OFF":
        # Si el comando recibido es "/todos", envía todos los valores actuales de los sensores al usuario.humambiente_pin_led_green.low()
        humsuelo_pin_led_verde.low()
    else:
        # Si el comando recibido no se reconoce, envía un mensaje indicando que el comando no es reconocido.
        send_message(chat_id, "Comando no reconocido.")

def update_sensor_data():
    # Esta función actualiza los datos de los sensores y muestra estos datos en la pantalla OLED.
    global temp, hum, lux, porcentaje_humedad_suelo  # Declara variables globales para que puedan ser accesibles fuera de la función.

    d.measure()  # Inicia una medición con el sensor DHT22.
    temp = d.temperature()  # Obtiene y almacena la temperatura medida por el sensor DHT22.
    hum = d.humidity()  # Obtiene y almacena la humedad medida por el sensor DHT22.
    lux = round(bh1750.measurement, 2)  # Obtiene y almacena la medición de luminosidad del sensor BH1750, redondeada a dos decimales.

    # Calcula el porcentaje de humedad del suelo basado en la lectura del sensor ADC y los valores mínimos y máximos calibrados.
    porcentaje_humedad_suelo = round((minima_humedad - sensor_humedad_suelo.read_u16()) * 100 / (minima_humedad - maxima_humedad), 2)

    # Llama a la función para mostrar los datos actualizados en la pantalla OLED.
    mostrar_datos_oled(temp, hum, lux, porcentaje_humedad_suelo)

# Suscribirse al broker mqtt
def setup_mqtt_subscription():
    try:
        client_sub.set_callback(handle_mqtt_messages)
        client_sub.connect()
        client_sub.subscribe(mqtt_topic_sub)
        print("Suscripción MQTT establecida en topic:", mqtt_topic_sub)
    except OSError as e:
        print("Error de conexión MQTT al configurar la suscripción:", e)
    except Exception as e:
        print("Error al configurar la suscripción MQTT:", e)

# Enviar los umbrales para que se guarden en la base de datos mysql
def enviar_umbrales():
    mensaje_umbrales = f'{TEMPERATURA_MINIMA},{TEMPERATURA_MAXIMA},{HUMEDAD_MINIMA},{HUMEDAD_MAXIMA},{LUMINOSIDAD_MINIMA},{LUMINOSIDAD_MAXIMA},{HUMEDAD_SUELO_MINIMA},{HUMEDAD_SUELO_MAXIMA}'
    topic = "invernadero/umbrales"  # Definición del topic específico para umbrales

    try:
        client_pub.connect()
        client_pub.publish(topic, mensaje_umbrales.encode())
        client_pub.disconnect()
    except OSError as e:
        print("Error de conexión MQTT:", e)
    except Exception as e:
        print("Error al enviar umbrales por MQTT:", e)

# Bucle principal
def main_loop():
    update_id = 0
    setup_mqtt_subscription()
    ultimo_intento_reconexion = utime.time()
    next_send = utime.time() + send_interval

    while True:
        try:
            current_time = utime.time()

            # Reconectar MQTT si es necesario
            if current_time - ultimo_intento_reconexion > 120:
                reconectar_mqtt()
                ultimo_intento_reconexion = current_time

            # Leer y actualizar datos de los sensores
            update_sensor_data()

            # Procesar mensajes de Telegram si hay alguno
            new_update_id, message = read_message(bot, update_id)
            if new_update_id != update_id and message:
                update_id = new_update_id
                handle_telegram_commands(message['chat']['id'], message['text'])

            # Enviar datos y alertas si es el momento
            if current_time >= next_send:
                manage_sending(next_send)
                next_send = current_time + send_interval

            # Verificar mensajes MQTT
            client_sub.check_msg()

            # Pequeña pausa para evitar el uso excesivo de CPU
            utime.sleep(0.1)

        except KeyboardInterrupt:
            apagar_todo()
            print("Ejecución cancelada por el usuario.")
            break  # Salir del bucle while

        except Exception as e:
            print("Error en el bucle principal:", e)


# Este bloque se ejecutará si el script se está ejecutando como programa principal.
if __name__ == "__main__":
    try:
        # Intentar conectar al servidor MQTT para publicar datos.
        #client_pub.connect()
        # Enviar umbrales a la web
        enviar_umbrales()
        # Crear una instancia de bot de Telegram utilizando el token de configuración.
        bot = utelegram.ubot(utelegram_config['token'])
        # Ejecutar el bucle principal del programa.
        main_loop()
    except Exception as e:
        # Imprimir cualquier excepción que ocurra durante la ejecución.
        print(f"Error: {e}")
