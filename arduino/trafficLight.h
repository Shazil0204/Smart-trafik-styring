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
      setColor(204, 0, 0);
    }

  public:
    void setGreenColor() {
      setColor(0, 128, 0);
    }

  void setColor(int redValue, int greenValue, int blueValue) {
    analogWrite(redPin, redValue);
    analogWrite(greenPin, greenValue);
    analogWrite(bluePin, blueValue);
  }
};