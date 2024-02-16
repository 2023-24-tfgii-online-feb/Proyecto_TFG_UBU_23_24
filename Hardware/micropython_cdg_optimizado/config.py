wifi_config = {
    'ssid':'xxx',
    'p':'xxx'
}
utelegram_config = {
    'token': 'xxx',
    'chat_id': 'xxx'
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
    'topic_umb': 'invernadero/umbrales',
    'client_id_pub': 'TFG_UBU_mqtt_id_pub',
    'client_id_sub': 'TFG_UBU_mqtt_id_sub',
    'client_id_umb': 'TFG_UBU_mqtt_id_umb'
}
# Configuración interfaz red inalámbrica wlan
wlan_config = {
    'ip':'192.168.1.232',
    'mask':'255.255.255.0',
    'gateway':'192.168.1.1',
    'dns':'8.8.8.8' 
}
