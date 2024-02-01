var mqtt = require("mqtt");
const mysql = require('mysql2');

// Opciones para la conexión MQTT
const mqttOptions = {
  keepAlive: 60,
  reconnectPeriod: 1000,
  connectTimeout: 4000
};

const client = mqtt.connect("mqtt://localhost:1883", mqttOptions);

const dbConfig = {
  host: 'localhost',
  user: 'joseluis',
  password: 'UBU_tfg_23_24',
  database: 'TFG_UBU',
  port: 3307
};

const pool = mysql.createPool(dbConfig);

function EventoConectar() {
  client.subscribe("invernadero/sensores");
  client.subscribe("invernadero/umbrales");
}

function EventoMensaje(topic, message) {
  if (topic === "invernadero/sensores") {
    const [fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo] = message.toString().split(',');
    const query = 'INSERT INTO sensores (fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo) VALUES (?, ?, ?, ?, ?, ?)';
    pool.query(query, [fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo], (error) => {
      if (error) {
        console.error('Error al realizar la inserción en la base de datos:', error);
      } else {
        console.log('Inserción en base de datos exitosa');
      }
    });
  } else if (topic === "invernadero/umbrales") {
    const [temperatura_minima, temperatura_maxima, humedad_minima, humedad_maxima, luminosidad_minima, luminosidad_maxima, humedad_suelo_minima, humedad_suelo_maxima] = message.toString().split(',');

    const query = 'UPDATE umbrales SET temperatura_minima = ?, temperatura_maxima = ?, humedad_ambiente_minima = ?, humedad_ambiente_maxima = ?, luminosidad_minima = ?, luminosidad_maxima = ?, humedad_suelo_minima = ?, humedad_suelo_maxima = ?';

    pool.query(query, [temperatura_minima, temperatura_maxima, humedad_minima, humedad_maxima, luminosidad_minima, luminosidad_maxima, humedad_suelo_minima, humedad_suelo_maxima], (error) => {
      if (error) {
        console.error('Error al actualizar umbrales en la base de datos:', error);
      } else {
        console.log('Actualización de umbrales en base de datos exitosa');
      }
    });
  }
}

client.on("connect", EventoConectar);
client.on("message", EventoMensaje);

pool.on('error', (err) => {
  console.error('Error inesperado en la conexión del pool:', err);
});
