namespace kiwiprojekt.tourbox.consoleapp
{
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
}