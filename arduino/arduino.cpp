#include <PubSubClient.h>
#include <WiFiS3.h>

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

enum TrafficLightColor { RED, GREEN, YELLOW };

class TrafficLight {
  int redPin;
  int greenPin;
  int bluePin;

  public:
    TrafficLight(int redPin, int greenPin, int bluePin) {
      this->redPin = redPin;
      this->greenPin = greenPin;
      this->bluePin = bluePin;

      pinMode(redPin, OUTPUT);
      pinMode(greenPin, OUTPUT);
      pinMode(bluePin, OUTPUT);
    }
    
  public:
    void setRedColor() {
      setColor(255, 0, 0);
    }

  public:
    void setGreenColor() {
      setColor(0, 255, 0);
    }

  void setColor(int redValue, int greenValue, int blueValue) {
    analogWrite(redPin, redValue);
    analogWrite(greenPin, greenValue);
    analogWrite(bluePin, blueValue);
  }
};

TrafficLight pedestrianTrafficLight(pedestrian_traffic_light_red_pin, pedestrian_traffic_light_green_pin, pedestrian_traffic_light_blue_pin);
TrafficLight vehicleTrafficLight(vehicle_traffic_light_red_pin, vehicle_traffic_light_green_pin, vehicle_traffic_light_blue_pin);

void setup() {
  Serial.begin(9600);

  setupDepthSensor();
  setupWiFi();
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
    pedestrianTrafficLight.setRedColor(); 
    vehicleTrafficLight.setGreenColor(); 
  } else {
    pedestrianTrafficLight.setGreenColor();
    vehicleTrafficLight.setRedColor(); 
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