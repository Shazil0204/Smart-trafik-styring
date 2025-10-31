#include <WiFiS3.h>

class WiFiConnection {
  const char* ssid;
  const char* password;
  
  unsigned long lastWiFiCheck = 0;

public:
  WiFiConnection(const char* ssid, const char* password) {
    this->ssid = ssid;
    this->password = password;
  }

  void connect() {
    Serial.print("Connecting to Wi-Fi: ");
    Serial.println(ssid);

    WiFi.begin(ssid, password);

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

  void ensureConnectivity(unsigned long runTime) {
    if (runTime - lastWiFiCheck >= 5000) {
      if (WiFi.status() != WL_CONNECTED) {
        Serial.println("WiFi lost, reconnecting...");
        connect();
      }
      lastWiFiCheck = runTime;
    }
  }
};
