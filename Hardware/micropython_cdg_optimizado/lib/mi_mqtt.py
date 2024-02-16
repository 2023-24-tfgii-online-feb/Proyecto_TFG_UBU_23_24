import umqtt, urequests
from config import mqtt_config, umbrales_sensores
from mi_modulos import LedRGB
class MQTT:
    def __init__(self,led_temperatura,led_humedad_ambiente,led_luminosidad,led_humedad_suelo):
        self.mqtt_server = mqtt_config['server']
        self.mqtt_port = mqtt_config['port']
        self.mqtt_topic_pub = mqtt_config['topic_pub']
        self.mqtt_topic_sub = mqtt_config['topic_sub']
        self.mqtt_topic_umbrales= mqtt_config['topic_umb']
        self.client_pub = umqtt.MQTTClient(mqtt_config['client_id_pub'], self.mqtt_server, self.mqtt_port)
        self.client_sub = umqtt.MQTTClient(mqtt_config['client_id_sub'], self.mqtt_server, self.mqtt_port)
        self.client_umbrales = umqtt.MQTTClient(mqtt_config['client_id_umb'],self.mqtt_server,self.mqtt_port)
        self.led_temperatura        = led_temperatura
        self.led_humedad_ambiente   = led_humedad_ambiente
        self.led_luminosidad        = led_luminosidad
        self.led_humedad_suelo      = led_humedad_suelo
    def obtener_fecha_hora(self):
        try:
            response = urequests.get('https://worldtimeapi.org/api/ip')
            if response.status_code == 200:
                datetime = response.json()['datetime'].split('T')
                return datetime[0], datetime[1].split('.')[0]
            else:
                print("Error al obtener fecha y hora: código de estado HTTP", response.status_code)
                return None, None
        except Exception as e:
            print("Error al obtener fecha y hora:", e)
            return None, None
    def enviar_mensaje(self,tipo_cliente,mensaje): #tipo_cliente=cliente_pub, cliente_sub
        topic={self.client_pub:self.mqtt_topic_pub, self.client_sub:self.mqtt_topic_sub, self.client_umbrales:self.mqtt_topic_umbrales}
        tipo_cliente.connect()
        tipo_cliente.publish(topic[tipo_cliente], mensaje.encode())
        tipo_cliente.disconnect()
    def enviar(self, fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo):
        fecha_actual, hora_actual = self.obtener_fecha_hora()
        try:
            mensaje=f'{fecha_actual},{hora_actual},{temperatura},{humedad},{intensidad_luz},{humedad_suelo}'
            self.enviar_mensaje(self.client_pub,mensaje)
            print("mensaje enviado")
        except OSError as e:
            print("Error de conexión MQTT al enviar datos:", e)
        except Exception as e:
            print("Error al enviar datos por MQTT:", e)
    def reconectar(self, tipo_cliente):
        tipo = {self.client_pub: "publicación",self.client_sub: "suscripción"}
        try:
            tipo_cliente.connect()
            print(f"Intento de reconexión MQTT ({tipo[tipo_cliente]}) realizado")
        except Exception as e:
            print(f"Error al reconectar MQTT ({tipo[tipo_cliente]}):", e)
    def reconectar_mqtt(self):
        self.reconectar(self.client_pub)
        self.reconectar(self.client_sub)
    def handle_mqtt_messages(self, topic, msg):
        mensaje = msg.decode().lstrip()  # Decodifica el mensaje MQTT
        print("Mensaje MQTT recibido:", mensaje)
        if mensaje == "/t_ON": #/dotos envia todos valores actuales
            self.led_temperatura.prender_green()
        elif mensaje == "/t_OFF":
            self.led_temperatura.apagar_green()
        elif mensaje == "/ha_ON":
            self.led_humedad_ambiente.prender_green()
        elif mensaje == "/ha_OFF":
            self.led_humedad_ambiente.apagar_green()
        elif mensaje == "/l_ON":
            self.led_luminosidad.prender_green()
        elif mensaje == "/l_OFF":
            self.led_luminosidad.apagar_green()
        elif mensaje == "/hs_ON":
            self.led_humedad_suelo.prender_green()
        elif mensaje == "/hs_OFF":
            self.led_humedad_suelo.apagar_green()
        else:
            print("Comando no reconocido.")
    def setup_mqtt_subscription(self):
        try:
            self.client_sub.set_callback(self.handle_mqtt_messages)
            self.client_sub.connect()
            self.client_sub.subscribe(self.mqtt_topic_sub)
            print("Suscripción MQTT establecida en topic: ", self.mqtt_topic_sub)
        except OSError as e:
            print("Error de conexión MQTT al configurar la suscripción:", e)
        except Exception as e:
            print("Error al configurar la suscripción MQTT:", e)
    def enviar_umbrales(self):
        temp_min=umbrales_sensores["TEMPERATURA_MINIMA"]
        temp_max=umbrales_sensores["TEMPERATURA_MAXIMA"]
        hum_min=umbrales_sensores["HUMEDAD_MINIMA"]
        hum_max=umbrales_sensores["HUMEDAD_MAXIMA"]
        lum_min=umbrales_sensores["LUMINOSIDAD_MINIMA"]
        lum_max=umbrales_sensores["LUMINOSIDAD_MAXIMA"]
        hs_min=umbrales_sensores["HUMEDAD_SUELO_MINIMA"]
        hs_max=umbrales_sensores["HUMEDAD_SUELO_MAXIMA"]
        mensaje_umbrales=f"{temp_min},{temp_max},{hum_min},{hum_max},{lum_min},{lum_max},{hs_min},{hs_max}"
        try:
            self.enviar_mensaje(self.client_umbrales,mensaje_umbrales)
        except OSError as e:
            print("Error de conexión MQTT:", e)
        except Exception as e:
            print("Error al enviar umbrales por MQTT:", e)
