# Diseño de un sistema económico IoT de monitorización de invernaderos de cannabis medicinal.

En este proyecto, se diseña e implementa un prototipo de sistema IoT de bajo coste que monitoriza la luminosidad, temperatura, humedad ambiente y humedad de suelo dentro del invernadero. Este sistema permite ajustar de manera precisa estas condiciones, fundamentales para el desarrollo saludable de las plantas de cannabis. Además, se propone una solución económica y eficiente para que el cultivador monitoree el estado del invernadero de forma remota, utilizando una plataforma web.

# Descripción del Proyecto.

El objetivo de este proyecto es utilizar una Raspberry Pi Pico w para obtener mediciones del ambiente y mostrarlas en una pantalla OLED y también en una plataforma web.

El programa ejecuta las siguientes tareas:

* Se inicializan los sensores DHT22 (temperatura y humedad), BH1750 (intensidad de luz) y el sensor de humedad del suelo.
* Obtener las lecturas de los sensores.
* Mostrar los datos en la pantalla OLED.
* Cada intervalo de tiempo se envia a servidor LAMP (por MQtt) los datos obtenidos, se muestran y se guardan en base de datos mySQL.
* Avisa de valores fuera del umbral esperado / ideal, ya sea con leds RGB luminosos (valor bajo azul, valor alto rojo) o avisos por telegram.
* Esperar el próximo intervalo de actualización.
* Desde el bot de telegram se pueden hacer consultas para que nos devuelva el valor del valor del sensor que hayamos elegido, también podremos activar mecanismos para solventar valores fuera de los umbrales, como riego, ventilación, calefacción, focos luminosos... (en este caso se enciende un led verde, pero lo comentado se podría implementar).


# Añadida una presentación creada con IA con la voz de Iker Jiménez.
Archivo: Presentación_IA_Iker_Jiménez_PF.mp3

# Subidos 3 videos a youtube con explicación y comprobación del funcionamiento.
1. https://youtu.be/VwgxjhJzbKk
2. https://youtu.be/c7tfhW0rUnw
3. https://youtu.be/uWBjEbmtJAI

# Acceso online al dashboard.

http://www.inveriot.com
