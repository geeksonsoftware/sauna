# sauna
Sauna automation

# Temperature AM2302 

https://learn.adafruit.com/dht

## Pinouts

To checkout pinout on RPI run `$ pinout`.

Left:
Brown - Pin 1 - 3V power
Black - Pin 9 - Ground
White - Pin 10 (GPIO15) - Data

Right
Red - Pin 17 - 3V power
Black - Pin 14 - Ground
White - Pin 12 (GPIO18) - Data

## Setup

```
pip3 install adafruit-circuitpython-dht
sudo apt-get install libgpiod2
```

## Test app

```python
import time
import board
import adafruit_dht
 
# Initial the dht device, with data pin connected to:
dhtDevice1 = adafruit_dht.DHT22(board.D15)
dhtDevice1 = adafruit_dht.DHT22(board.D18)
 
while True:
    try:
        # Print the values to the serial port
        print("Temp1: {:.1f} C    Humidity: {}% "
              .format(dhtDevice1.temperature, dhtDevice1.humidity))
        print("Temp2: {:.1f} C    Humidity: {}% "
              .format(dhtDevice2.temperature, dhtDevice2.humidity))
 
    except RuntimeError as error:
        # Errors happen fairly often, DHT's are hard to read, just keep going
        print(error.args[0])
 
    time.sleep(2.0)
```

Results:
```
$ python3 temp.py
Temp1: 22.8 C    Humidity: 42.6%
Temp2: 23.0 C    Humidity: 41.6%
Temp1: 22.9 C    Humidity: 37.9%
Temp2: 22.9 C    Humidity: 38.6%
```