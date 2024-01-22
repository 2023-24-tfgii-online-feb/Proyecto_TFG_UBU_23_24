# Diseño de un sistema económico IoT de monitorización de invernaderos de cannabis medicinal

En este proyecto, se diseña e implementa un prototipo de sistema IoT de bajo coste que monitoriza la luminosidad, temperatura, humedad ambiente y humedad de suelo dentro del invernadero. Este sistema permite ajustar de manera precisa estas condiciones, fundamentales para el desarrollo saludable de las plantas de cannabis. Además, se propone una solución económica y eficiente para que el cultivador monitoree el estado del invernadero de forma remota, utilizando una plataforma web.

# Descripción del Proyecto

El objetivo de este proyecto es utilizar una Raspberry Pi Pico para obtener mediciones del ambiente y mostrarlas en una pantalla OLED y también en una plataforma web.

El programa ejecuta las siguientes tareas:

* Se inicializan los sensores DHT11 (temperatura y humedad), BH1750 (intensidad de luz) y el sensor de humedad del suelo.
* Obtener las lecturas de los sensores.
* Mostrar los datos en la pantalla OLED.
* Enviar actualizaciones a la plataforma web en intervalos predefinidos.
* Esperar el próximo intervalo de actualización.
