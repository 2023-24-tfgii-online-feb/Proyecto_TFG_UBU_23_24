const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const mqtt = require('mqtt');
const path = require('path');
const mysql = require('mysql2');
const cors = require('cors');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const mqttClient = mqtt.connect('mqtt://localhost:1883', {
  keepAlive: 60,            // Enviar un mensaje de ping cada 60 segundos para mantener la conexión
  reconnectPeriod: 1000,    // Intentar reconectar cada 1000 milisegundos (1 segundo) en caso de desconexión
  connectTimeout: 4000      // Tiempo de espera de 4000 milisegundos (4 segundos) para la conexión inicial
});


// Configuración de la base de datos
const dbConfig = {
    host: 'localhost',
    port: 3307, // Puerto especificado aquí
    user: 'joseluis',
    password: 'UBU_tfg_23_24',
    database: 'TFG_UBU'
};
const pool = mysql.createPool(dbConfig);

// Intentar obtener una conexión para probar si la conexión a la base de datos es exitosa
pool.getConnection((err, connection) => {
    if (err) {
        console.error('Error al conectar a la base de datos:', err);
        return;
    }
    console.log('Conectado exitosamente a la base de datos MySQL');
    connection.release(); // No olvides liberar la conexión
});

// Conexión a MQTT y manejo de mensajes
mqttClient.on('connect', () => {
    console.log('Conectado al broker MQTT');
    mqttClient.subscribe('invernadero/sensores');
});

mqttClient.on('message', (topic, message) => {
    console.log(`Mensaje recibido en el topic ${topic}: ${message}`);
    if (topic === 'invernadero/sensores') {
        io.emit('mqtt_data', message.toString());
    }
});

// Middleware
app.use(express.static(path.join(__dirname, './')));
app.use(cors());

// Rutas para servir las páginas
app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, './', 'index.html'));
});

app.get('/historico', (req, res) => {
    res.sendFile(path.join(__dirname, './', 'historico.html'));
});

// Ruta para obtener datos históricos en un rango de fechas
app.get('/datos-historicos', (req, res) => {
    const { fechaInicio, fechaFin } = req.query;
    
    let query = "SELECT fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo FROM sensores";
    let params = [];

    if (fechaInicio && fechaFin) {
        query += " WHERE fecha BETWEEN ? AND ?";
        params.push(fechaInicio, fechaFin);
    }

    pool.query(query, params, (error, results, fields) => {
        if (error) {
            console.error('Error al realizar la consulta a la base de datos:', error);
            res.status(500).send('Error al obtener los datos');
            return;
        }
        res.json(results);
    });
});

app.get('/umbrales', (req, res) => {
    pool.query('SELECT * FROM umbrales ORDER BY fecha_actualizacion DESC LIMIT 1', (error, results, fields) => {
        if (error) {
            console.error('Error al realizar la consulta a la base de datos:', error);
            res.status(500).send('Error al obtener los datos de los umbrales');
            return;
        }
        res.json(results[0]);
    });
});

const PORT = 3000;
server.listen(PORT, () => console.log(`Servidor corriendo en el puerto ${PORT}`));
