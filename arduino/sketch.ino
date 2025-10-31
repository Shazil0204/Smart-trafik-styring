#include "Config.h"
#include "WiFiConnection.h"
#include "TrafficLight.h"
#include "MQTT.h"
#include "DistanceSensor.h"

// Timing variables
unsigned long lastSensorRead = 0;

// Cache traffic light state
bool didLastCheckDetectVehicle;

WiFiConnection wiFiConnection(WIFI_SSID, WIFI_PASSWORD);
MQTT mqtt(MQTT_SERVER, MQTT_PORT);

DistanceSensor distanceSensor(DISTANCE_SENSOR_TRANS_PIN, DISTANCE_SENSOR_ECHO_PIN);

PedestrianTrafficLight pedestrianTrafficLight(PEDESTRIAN_TRAFFIC_LIGHT_RED_PIN, PEDESTRIAN_TRAFFIC_LIGHT_GREEN_PIN, PEDESTRIAN_TRAFFIC_LIGHT_BLUE_PIN, TRAFFIC_LIGHT_BUTTON_PIN);
VehicleTrafficLight vehicleTrafficLight(VEHICLE_TRAFFIC_LIGHT_RED_PIN, VEHICLE_TRAFFIC_LIGHT_GREEN_PIN, VEHICLE_TRAFFIC_LIGHT_BLUE_PIN);

void setup() {
  Serial.begin(9600);

  wiFiConnection.connect();
  mqtt.connect();
}

void loop() {
  unsigned long runTime = millis();

  mqtt.ensureConnectivity();

  if (runTime - lastSensorRead >= 200) {
    trafficLightController(runTime);
    lastSensorRead = runTime;
  }
  wiFiConnection.ensureConnectivity(runTime);
}

void trafficLightController(unsigned long runTime) {
  float vehicleDistance = distanceSensor.calculateDistanceToObject();
  if(vehicleDistance < 20 && !didLastCheckDetectVehicle) {
    pedestrianTrafficLight.setRedColor(); 
    vehicleTrafficLight.setGreenColor(); 
    mqtt.publishMessage("VEHICLE_GREEN");
    didLastCheckDetectVehicle = true;
  } else if (vehicleDistance > 20 && didLastCheckDetectVehicle) {
    pedestrianTrafficLight.setGreenColor();
    vehicleTrafficLight.setRedColor(); 
    mqtt.publishMessage("VEHICLE_RED");
    didLastCheckDetectVehicle = false;
  }
}