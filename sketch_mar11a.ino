#include <Adafruit_NeoPixel.h>




void setup() {
  Serial.begin(9600);
}
String a;

void getElenet(int* element, int howMany, int start)
{

  for (int i = 0; i < howMany; i++)
  {
    element[i] = 0;
    for (; start < a.length(); start++)
    {
      if (a[start] == ';' || a[start] == '\n')
      {
        start++;
        break;
      }
      element[i] *= 10;
      element[i] += a[start] - '0';
    }

  }
}
void loop() {

  //String abs[2] = {"@6;512", "15;0;36;254;10"};
  if (Serial.available())
  {

    a = Serial.readString();

createM:
    int element[2];
    getElenet(element, 2, 1);
    Serial.println(element[0]);
    Serial.println(element[1]);

    Adafruit_NeoPixel pixels(element[1], element[0], NEO_GRB + NEO_KHZ800);
    pixels.begin();
    pixels.show();
    while (true)
    {
      if (Serial.available())
      {
        a = Serial.readString();
        if (a[0] == '@')
        {
          Serial.println("go");

          goto createM;
        }
        element[5];
        getElenet(element, 5, 0);
        Serial.println(element[0]);
        Serial.println(element[1]);
        Serial.println(element[2]);
        Serial.println(element[3]);
        Serial.println(element[4]);

        pixels.begin();
        pixels.setBrightness(element[4]);
        pixels.setPixelColor(element[0], pixels.Color(element[1], element[2], element[3]));
        pixels.show();
      }
    }
  }
}
