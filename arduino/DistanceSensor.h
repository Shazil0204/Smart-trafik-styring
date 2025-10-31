class DistanceSensor {
  int transmitPin;
  int echoPin;

  public:
    DistanceSensor(int transmitPin, int echoPin) {
      this->transmitPin = transmitPin;
      this->echoPin = echoPin;
    }

  public:
    float calculateDistanceToObject() {
      unsigned long duration;

      // Trigger ultrasonic pulse
      digitalWrite(transmitPin, LOW);
      delayMicroseconds(5);
      digitalWrite(transmitPin, HIGH);
      delayMicroseconds(10);
      digitalWrite(transmitPin, LOW);

      // Listen for echo with timeout (30 ms)
      duration = pulseIn(echoPin, HIGH, 30000);

      // Convert to cm
      float rawDistance = (duration * 0.034) / 2.0;

      Serial.print("Distance: ");
      Serial.print(rawDistance);
      Serial.println(" cm");

      return rawDistance;
    }

  
};