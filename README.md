# Servicio en Node.js llamado "nodeMqtt".

Se encarga de estar escuchando el topic "invernadero/sensores" y el topic "invernadero/umbrales".
El que manda los datos es la placa raspberry pi pico w rp2040 con los valores de los sensores. El servicio nodeMqtt recoge los datos mqtt de los topics comentados, los formatea, y los inserta en la base de datos mySQL (valores de los sensores) y actualiza los valores de los umbrales.
