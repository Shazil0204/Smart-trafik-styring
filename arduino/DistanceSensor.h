class DistanceSensor {
  int triggerPin;
  int echoPin;

  public:
    DistanceSensor(int triggerPin, int echoPin) {
      this->triggerPin = triggerPin;
      this->echoPin = echoPin;

      pinMode(triggerPin, OUTPUT);
      pinMode(echoPin, INPUT);
    }

  public:
    float calculateDistanceToObject() {
      // Trigger ultrasonic pulse
      digitalWrite(triggerPin, LOW);
      delayMicroseconds(5);
      digitalWrite(triggerPin, HIGH);
      delayMicroseconds(10);
      digitalWrite(triggerPin, LOW);

      // Listen for echo with timeout (30 ms)
      unsigned long duration = pulseIn(echoPin, HIGH, 30000);

      // Convert to cm
      float rawDistance = (duration * 0.034) / 2.0;

      Serial.print("Distance: ");
      Serial.print(rawDistance);
      Serial.println(" cm");

      return rawDistance;
    }  
};