# Diseño de aplicación de escritorio (Aplicación de windows forms) en lenguaje C#, IDE Visual Studio 2022

En este proyecto, se diseña e implementa una aplicación de escritorio windows, con el cual el usuario puede monitorizar en tiempo real los datos que mandan los sensores junto a la placa.
Estos datos se mandan por mqtt (pub) y la aplicación se suscribe al mismo topic (susc).
Una vez recibido los datos, se formatean y se coloca cada dato en el textbox correspondiente, añadiéndole la unidad de medida.
Se añaden los botones para activa o desactivar los mecanismos (en este caso led verde) para solventar los valores fuera del umbral ideal, los cuales se indican en la parte derecha.
En la parte de abajo se han añadido 8 textboxes, valores mínimos y máximos de cada valor, para poder actualziar desde la propia ventana los umbrales.
La recepción y actualización de los umbrales se realizarn accediendo a la base de datos y tabla "umbrales" del servidor LAMP.
De esta manera, si un valor está por debajo del valor ideal, el fondo del texbox se pondrá de color azul, si el valor es normal, se mantendrá en su gris original, y si el valor es superior, el color de fondo cambiará a rojo, de esta manera tendremos avisos visuales de los valores.

# Imagenes de la aplicación.

![InverIoT_Desktop](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/4ef8faed-c5b4-45b1-8103-5fa0446f055f)
![InverIoT_Histórico](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/3f90ce65-1fa5-434f-a41b-99b01af60217)
![InverIoT_Gráficas](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/a424f066-3d7e-4bf1-a5bc-4fe084fd51fb)



Pruebas hechas con pantalla de aplicación v1.
![IMG_1443](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/1bb64560-8f46-417b-80a2-a5d7c634a3be)

![IMG_1444](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/948b443b-0701-4106-b8f5-42cd9529347f)

Al activar el mecanismo (led) desde la aplicación, en este caso de la humedad ambiente (que está en gris y se aprovech apara que se vea mejor).
![IMG_1442](https://github.com/JLCaballeroMQ/Proyecto_TFG_UBU_23_24/assets/127446383/8243e58d-2df5-4364-8990-a8fb169fdeb7)


