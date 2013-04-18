using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace CodeFreakout.Spektrum.AR8000.Sample
{
    public class Program
    {
        public static void Main()
        {
            var receiver = new SatelliteReceiver(SerialPorts.COM1);

            while (true)
            {
                Debug.Print("Throttle: " + receiver.Throttle);
                Debug.Print("Aileron: " + receiver.Aileron);
                Debug.Print("Elevator: " + receiver.Elevator);
                Debug.Print("Rudder: " + receiver.Rudder);
                Debug.Print("Gear: " + receiver.Gear);
                Debug.Print("Auxilery2: " + receiver.Auxilery2);
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
