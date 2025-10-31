class TrafficLight {
  protected:
    int redPin, greenPin, bluePin;

  protected:
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

class VehicleTrafficLight : public TrafficLight {
  public:
    VehicleTrafficLight(int redPin, int greenPin, int bluePin) 
      : TrafficLight(redPin, greenPin, bluePin) {}
};

class PedestrianTrafficLight : public TrafficLight {
  public:
    PedestrianTrafficLight(int redPin, int greenPin, int bluePin, int crosswalkButtonPin) 
      : TrafficLight(redPin, greenPin, bluePin) {
        pinMode(crosswalkButtonPin, INPUT_PULLUP);
      }

    public:
      void clickCrosswalkButton() {
        // TODO: Implement crosswalk button functionality, if there is time for it
      }
};