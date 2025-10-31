#include <PubSubClient.h>
#include <WiFiS3.h>
#include "config.h"
#include "trafficLight.h"

// Button pin (set as INPUT)
const int blue_btn_pin = 8;

// Timing variables
unsigned long lastSensorRead = 0;
unsigned long lastWiFiCheck  = 0;

// Cache traffic light state
bool didLastCheckDetectVehicle;

WiFiClient wifiClient;
PubSubClient client(wifiClient);

TrafficLight pedestrianTrafficLight(PEDESTRIAN_TRAFFIC_LIGHT_RED_PIN, PEDESTRIAN_TRAFFIC_LIGHT_GREEN_PIN, PEDESTRIAN_TRAFFIC_LIGHT_BLUE_PIN);
TrafficLight vehicleTrafficLight(VEHICLE_TRAFFIC_LIGHT_RED_PIN, VEHICLE_TRAFFIC_LIGHT_GREEN_PIN, VEHICLE_TRAFFIC_LIGHT_BLUE_PIN);

void setup() {
  Serial.begin(9600);

  setupDepthSensor();
  setupWiFi();
  setupMQTT();
}

void setupMQTT() {
  client.setServer(MQTT_SERVER, MQTT_PORT);
}

void setupWiFi() {
  Serial.print("Connecting to WiFi: ");
  Serial.println(WIFI_SSID);

  WiFi.begin(WIFI_SSID, WIFI_PASS);

  unsigned long start = millis();
  while (WiFi.status() != WL_CONNECTED && millis() - start < 15000) {
    delay(500);
    Serial.print(".");
  }

  if (WiFi.status() == WL_CONNECTED) {
    Serial.println("\nWiFi connected!");
    Serial.print("IP Address: ");
    Serial.println(WiFi.localIP());
  } else {
    Serial.println("\nWiFi connection failed.");
  }
}

void setupDepthSensor() {
  pinMode(DISTANCE_SENSOR_TRANS_PIN, OUTPUT); // transmit
  pinMode(DISTANCE_SENSOR_ECHO_PIN, INPUT);   // receive
}

void loop() {
  unsigned long runTime = millis();

  if (!client.connected()) {
    handleMQTT();
  }
  client.loop(); // keep MQTT alive

  if (runTime - lastSensorRead >= 200) {
    trafficLightController(runTime);
    lastSensorRead = runTime;
  }
  handleWiFi(runTime);
}

void trafficLightController(unsigned long runTime) {
  float vehicleDistance = calculateDistanceToVehicle();
  if(vehicleDistance < 20 && !didLastCheckDetectVehicle) {
    pedestrianTrafficLight.setRedColor(); 
    vehicleTrafficLight.setGreenColor(); 
    client.publish("traffic/light", "VEHICLE_GREEN");
    didLastCheckDetectVehicle = true;
  } else if (vehicleDistance > 20 && didLastCheckDetectVehicle) {
    pedestrianTrafficLight.setGreenColor();
    vehicleTrafficLight.setRedColor(); 
    client.publish("traffic/light", "VEHICLE_RED");
    didLastCheckDetectVehicle = false;
  }
}

void handleWiFi(unsigned long runTime) {
  if (runTime - lastWiFiCheck >= 5000) {
    if (WiFi.status() != WL_CONNECTED) {
      Serial.println("WiFi lost, reconnecting...");
      setupWiFi();
    }
    lastWiFiCheck = runTime;
  }
}

void handleMQTT() {
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    if (client.connect("ArduinoClient")) {
      Serial.println("connected");
      client.subscribe("traffic/control"); // example subscription
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      delay(5000);
    }
  }
}

float calculateDistanceToVehicle() {
  unsigned long duration;

  // Trigger ultrasonic pulse
  digitalWrite(DISTANCE_SENSOR_TRANS_PIN, LOW);
  delayMicroseconds(5);
  digitalWrite(DISTANCE_SENSOR_TRANS_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(DISTANCE_SENSOR_TRANS_PIN, LOW);

  // Listen for echo with timeout (30 ms)
  duration = pulseIn(DISTANCE_SENSOR_ECHO_PIN, HIGH, 30000);
 
  float depth_sensor_dist_raw;

  // Convert to cm
  depth_sensor_dist_raw = (duration * 0.034) / 2.0;

  Serial.print("Distance: ");
  Serial.print(depth_sensor_dist_raw);
  Serial.println(" cm");

  return depth_sensor_dist_raw;
}
