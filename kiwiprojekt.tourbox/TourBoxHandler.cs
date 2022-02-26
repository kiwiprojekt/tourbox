using System.IO.Ports;

namespace kiwiprojekt.tourbox
{
    public class TourBoxHandler : IDisposable
    {
        private readonly SerialPort port;
        private readonly Action<TourBoxEvent> handler;

        public TourBoxHandler(SerialPort serialPort, Action<TourBoxEvent> eventHandler)
        {
            port = serialPort;
            handler = eventHandler;

            port.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
            
            port.Open();
        }

        public TourBoxHandler(string comPortName, Action<TourBoxEvent> eventHandler)
            : this(new SerialPort(comPortName, 115200, Parity.None, 8, StopBits.One), eventHandler)
        {
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (port.BytesToRead > 0)
            {
                var code = (byte)port.ReadByte();
                var tourBoxEvent = EventParser.Parse(code);
                if (tourBoxEvent is not null && handler is not null)
                {
                    handler.Invoke(tourBoxEvent);
                }
            }
        }

        public void Dispose()
        {
            port?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}