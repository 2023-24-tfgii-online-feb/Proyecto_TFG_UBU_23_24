# Diccionario de configuración para la conexión WiFi
wifi_config = {
    'ssid':'Luzdelejos', # El SSID (nombre) de la red WiFi a la que se conectará el dispositivo
    'password':'luzNV75@' # La contraseña de la red WiFi
    #'ssid':'iPhone de JoseLuisCMQ',
    #'password':'fiat1028'
}

# Diccionario de configuración para el bot de Telegram
utelegram_config = {
    'token': '6638292008:AAGFpvByHl_omYf-nNnKRaYX_isnJdFJ_Eg', # El token del bot de Telegram.
    'chat_id': '1922244044' # El ID del chat en Telegram donde el bot enviará y recibirá mensajes.
}

# Diccionario de configuración para los umbrales de los sensores
umbrales_sensores = {
    'TEMPERATURA_MINIMA': 30,
    'HUMEDAD_MINIMA': 30,
    'LUMINOSIDAD_MINIMA': 30,
    'HUMEDAD_SUELO_MINIMA': 20,
    'TEMPERATURA_MAXIMA': 35,
    'HUMEDAD_MAXIMA': 70,
    'LUMINOSIDAD_MAXIMA': 150,
    'HUMEDAD_SUELO_MAXIMA': 80
}

# Configuración MQTT
mqtt_config = {
    'server': '46.24.8.196',
    'port': 1883,
    'topic_pub': 'invernadero/sensores',
    'topic_sub': 'invernadero/ordenes',
    'client_id_pub': 'TFG_UBU_mqtt_id_pub',
    'client_id_sub': 'TFG_UBU_mqtt_id_sub'
}