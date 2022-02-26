# kiwiprojekt.tourbox

Small library to read actions from [TourBox](https://www.tourboxtech.com/en/) controller by serial port.


### nuget.org
[kiwiprojekt.tourbox](https://www.nuget.org/packages/kiwiprojekt.tourbox/)

### How to use
```csharp
  class Program
  {
      static void Main()
      {
          using var handler = new TourBoxHandler("COM3", TourBoxEventHandler);
          Console.ReadLine();
      }

      private static void TourBoxEventHandler(TourBoxEvent tourBoxEvent)
      {
          if(tourBoxEvent.Is(ActionType.Click, TourBoxKey.C1, TourBoxKey.Tall))
          {
              Console.WriteLine("Hello World!");
          }
          Console.WriteLine(tourBoxEvent);
      }
  }
```


### License
[![MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
