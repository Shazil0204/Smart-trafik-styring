#include <PubSubClient.h>
#include <WiFiS3.h>
#include <UUID.h>

// WiFi credentials
char ssid[] = "Lund iPhone";
char pass[] = "ZXiDbVg7R2";

// Ultrasonic sensor pins
const int depth_sensor_trans_pin = 11; // Trig
const int depth_sensor_recv_pin  = 10; // Echo

// Traffic light pins
const int vehicle_traffic_light_red_pin   = 6;
const int vehicle_traffic_light_blue_pin  = 4;
const int vehicle_traffic_light_green_pin = 5;

const int pedestrian_traffic_light_red_pin   = 7;
const int pedestrian_traffic_light_blue_pin  = 3;
const int pedestrian_traffic_light_green_pin = 2;

// Button pin (set as INPUT)
const int blue_btn_pin = 8;

// Timing variables
unsigned long lastSensorRead = 0;
unsigned long lastWiFiCheck  = 0;

enum TrafficLightType { PEDESTRIAN, VEHICLE };
enum TrafficLightColor { RED, GREEN, BLUE };

class TrafficLight {
  char id[37];

  public:
    TrafficLight(TrafficLightType type) {
      id = createUUID();
    }

  private:
    char* createUUID() {
      // TODO: Create UUID
    }
};

TrafficLightColor vehicleTrafficLightState    = RED;
TrafficLightColor pedestrianTrafficLightState = RED;

void setup() {
  Serial.begin(9600);

  setupDepthSensor();
  setupTrafficLight();
  setupWiFi();

  pinMode(blue_btn_pin, INPUT_PULLUP); // safer for button wiring
  digitalWrite(vehicle_traffic_light_red_pin, HIGH);
}

void setupTrafficLight() {
  pinMode(vehicle_traffic_light_red_pin, OUTPUT);
  pinMode(vehicle_traffic_light_blue_pin, OUTPUT);
  pinMode(vehicle_traffic_light_green_pin, OUTPUT);
  digitalWrite(vehicle_traffic_light_red_pin, LOW);
  digitalWrite(vehicle_traffic_light_blue_pin, LOW);
  digitalWrite(vehicle_traffic_light_green_pin, LOW);

  pinMode(pedestrian_traffic_light_red_pin, OUTPUT);
  pinMode(pedestrian_traffic_light_blue_pin, OUTPUT);
  pinMode(pedestrian_traffic_light_green_pin, OUTPUT);
  digitalWrite(pedestrian_traffic_light_red_pin, LOW);
  digitalWrite(pedestrian_traffic_light_blue_pin, LOW);
  digitalWrite(pedestrian_traffic_light_green_pin, LOW);
}

void setupWiFi() {
  Serial.print("Connecting to WiFi: ");
  Serial.println(ssid);

  WiFi.begin(ssid, pass);

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
  pinMode(depth_sensor_trans_pin, OUTPUT); // transmit
  pinMode(depth_sensor_recv_pin, INPUT);   // receive
}

void loop() {
  unsigned long runTime = millis();

  if (runTime - lastSensorRead >= 200) {
    trafficLightController(runTime);
    lastSensorRead = runTime;
  }
  handleWiFi(runTime);
}

void trafficLightController(unsigned long runTime) {
  float vehicleDistance = calculateDistanceToVehicle();
  if(vehicleDistance > 20) {
    changeTrafficLightColor(PEDESTRIAN, RED);
    changeTrafficLightColor(VEHICLE, GREEN);
  } else {
    changeTrafficLightColor(PEDESTRIAN, GREEN);
    changeTrafficLightColor(VEHICLE, RED);
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

float calculateDistanceToVehicle() {
  unsigned long duration;

  // Trigger ultrasonic pulse
  digitalWrite(depth_sensor_trans_pin, LOW);
  delayMicroseconds(5);
  digitalWrite(depth_sensor_trans_pin, HIGH);
  delayMicroseconds(10);
  digitalWrite(depth_sensor_trans_pin, LOW);

  // Listen for echo with timeout (30 ms)
  duration = pulseIn(depth_sensor_recv_pin, HIGH, 30000);

  float depth_sensor_dist_raw;

  // Convert to cm
  depth_sensor_dist_raw = (duration * 0.034) / 2.0;

  Serial.print("Distance: ");
  Serial.print(depth_sensor_dist_raw);
  Serial.println(" cm");

  return depth_sensor_dist_raw;
}

void toggleTrafficLight(TrafficLightType trafficLight) {
}

void changeTrafficLightColor(TrafficLightType trafficLight, TrafficLightColor color) {
  int redPin    = trafficLight == VEHICLE ? vehicle_traffic_light_red_pin : pedestrian_traffic_light_red_pin;
  int bluePin = trafficLight == VEHICLE ? vehicle_traffic_light_blue_pin : pedestrian_traffic_light_blue_pin;
  int greenPin  = trafficLight == VEHICLE ? vehicle_traffic_light_green_pin : pedestrian_traffic_light_green_pin;

  digitalWrite(redPin, LOW);
  digitalWrite(greenPin, LOW);
  digitalWrite(bluePin, LOW);

  if(color == RED) {
    digitalWrite(redPin, HIGH);
  } else if(color == GREEN) {
    digitalWrite(greenPin, HIGH);
  } else {
    digitalWrite(greenPin, HIGH);
    digitalWrite(redPin, HIGH);
  }
}