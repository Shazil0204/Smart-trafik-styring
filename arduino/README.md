# Setup Arduino

## Prerequisite

TODO: Provide names and links to the required hardware

## The wiring

TODO: Show the wiring from the Arduino to the breadboard, to the sensors. Just take a picture of it.

## Install Arduino IDE

Install the Arduino IDE using their [installation guide](https://docs.arduino.cc/software/ide-v2/tutorials/getting-started/ide-v2-downloading-and-installing), for your specific operating system.

Open the IDE after installation.
You will then be prompted to install additional drivers. Install those as well.

## Select your Arduino

In the IDE, select your Arduino by clicking the dropdown.
Choose the Arduino R4 UNO WiFi
TODO: Insert image.

## Create the files and write the code

On the upper right side of the IDE, click the 3 dots (...) and select "new tab".<br>
<img width="398" height="372" alt="image" src="https://github.com/user-attachments/assets/dd442551-ec4c-45ad-999b-318fb4e9933e" />

In the text input, write the file names of the files located within this [repository directory](https://github.com/Shazil0204/Smart-trafik-styring/tree/main/arduino).
For each of the files, copy and then paste the code from the repository, into your Arduino project.

## Configuration

After having added all the files in the Arduino directory, in this repository, add another file called "Config.h".
This file will contain your own configurations, such as Wi-Fi credentials.<br> It should fit this format:

TODO: Show the format, by referencing the config.h on my laptop.
TODO: Help guide how the user gets the correct IP address for MQTT and whatever else. Maybe this can be described in a code example, showing the config file itself.

## Install and enable libraries dependencies

TODO: install and enable "WiFiS3" and "PubSubClient".

## Upload the code

At the top of the IDE, click the -> button to upload the code to the Arduino.<br>
<img width="120" height="93" alt="image" src="https://github.com/user-attachments/assets/b0ac03a7-8103-41c7-bfd0-b8a55c40953d" />

Now the Arduino should be successfully setup and tranferring data to the docker container using MQTT.
