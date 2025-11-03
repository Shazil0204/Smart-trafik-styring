# Setup Arduino

## Prerequisite

- 1x [Arduino UNO R4 WiFi](https://docs.arduino.cc/hardware/uno-r4-wifi) with USB cable to connect to your computer
- 1x [HC-SR04 distance sensor](https://projecthub.arduino.cc/Isaac100/getting-started-with-the-hc-sr04-ultrasonic-sensor-7cabe1)
- 2x [RGB LEDs](https://projecthub.arduino.cc/semsemharaz/interfacing-rgb-led-with-arduino-b59902)
- 12x jumper cables

## The wiring

TODO: Show the wiring from the Arduino to the breadboard, to the sensors. Just take a picture of it.

## Install Arduino IDE

Install the Arduino IDE using their [installation guide](https://docs.arduino.cc/software/ide-v2/tutorials/getting-started/ide-v2-downloading-and-installing), for your specific operating system.

Open the IDE after installation.
You will then be prompted to install additional drivers. Install those as well.

## Select your Arduino

In the IDE, select your Arduino by clicking the dropdown.
Choose the Arduino R4 UNO WiFi<br>
<img width="549" height="135" alt="image" src="https://github.com/user-attachments/assets/293b4f7e-3c51-45d3-86ad-da152a2d3911" />

## Create the files and write the code

On the upper right side of the IDE, click the 3 dots (...) and select "new tab".<br>
<img width="398" height="372" alt="image" src="https://github.com/user-attachments/assets/dd442551-ec4c-45ad-999b-318fb4e9933e" />

In the text input, write the file names of all the \*.h files located within this [repository directory](https://github.com/Shazil0204/Smart-trafik-styring/tree/main/arduino).
For each of the files, copy and then paste the code from the repository, into your Arduino project.

## Configuration

After having added all the files in the Arduino directory, in this repository, add another file called "Config.h".
This file will contain your own configurations, such as Wi-Fi credentials.<br> It should fit this format:

```
// Wi-Fi - Does not work with WPS enterprise networks. Use a standard WiFi network or a mobile hotspot.
#define WIFI_SSID "WiFi SSID name"
#define WIFI_PASSWORD "WiFi password"

// MQTT - Use the docker container for easy setup of an MQTT broker.
#define MQTT_SERVER "hostname" // Get the ip by writing "docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' mosquitto"
#define MQTT_PORT 1883

// Ultrasonic distance sensor
#define DISTANCE_SENSOR_TRIG_PIN 11
#define DISTANCE_SENSOR_ECHO_PIN 10

// Traffic light pins
#define VEHICLE_TRAFFIC_LIGHT_RED_PIN 6
#define VEHICLE_TRAFFIC_LIGHT_BLUE_PIN 4
#define VEHICLE_TRAFFIC_LIGHT_GREEN_PIN 5

#define PEDESTRIAN_TRAFFIC_LIGHT_RED_PIN 7
#define PEDESTRIAN_TRAFFIC_LIGHT_GREEN_PIN 2
#define PEDESTRIAN_TRAFFIC_LIGHT_BLUE_PIN 3
```

## Install and enable libraries dependencies

Inside the Arduino IDE, click the books on the left side. <br>
<img width="404" height="523" alt="image" src="https://github.com/user-attachments/assets/78c84110-6045-481f-ab28-4183f43c8202" />

Search for PubSubClient and install.
This library will handle MQTT communication.

In addition, we will be using WiFiS3 to handle wireless networking.
This library is already installed on the Arduino UNO R4 WiFi, so there is no need to install it.

## Upload the code

At the top of the IDE, click the -> button to upload the code to the Arduino.<br>
<img width="120" height="93" alt="image" src="https://github.com/user-attachments/assets/b0ac03a7-8103-41c7-bfd0-b8a55c40953d" />

Now the Arduino should be successfully setup and transferring data to the docker container using MQTT. If you have not setup the docker container, check out the README file in [this directory](https://github.com/Shazil0204/Smart-trafik-styring/tree/main/docker).
