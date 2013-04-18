using System.Collections;
using System.IO.Ports;

namespace CodeFreakout.Spektrum.AR8000
{
    public class SatelliteReceiver
    {
        private readonly SerialPort _serial;
        private readonly byte[] _values;
        private int _index = 0;
        private bool _radioSynchronized;
        private Hashtable _channelizedValues;
        private readonly ByteArrayChannelizer _channelizer;

        public int Throttle 
        { 
            get { return GetChannelValue(5); }
        }
        
        public int Aileron
        {
            get { return GetChannelValue(1); }
        }

        public int Elevator
        {
            get { return GetChannelValue(2); }
        }

        public int Rudder
        {
            get { return GetChannelValue(3); }
        }

        public int Gear
        {
            get { return GetChannelValue(4); }
        }

        public int Auxilery2
        {
            get { return GetChannelValue(6); }
        }
        
        public SatelliteReceiver(string portName)
        {
            _values = new byte[32];
            _channelizedValues = new Hashtable();
            _channelizer = new ByteArrayChannelizer();

            _serial = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            _serial.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived);
            _serial.Open();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var readBuffer = new byte[_serial.BytesToRead];

            // Read bytes from buffer
            _serial.Read(readBuffer, 0, readBuffer.Length);

            foreach (var b in readBuffer)
            {
                _values[_index] = b;
                _index++;

                if (!_radioSynchronized && _index >= 4)
                {
                    if (_values[_index - 1] == 255 && _values[_index - 2] == 255 && _values[_index - 3] == 255 && _values[_index - 4] == 255)
                    {
                        _radioSynchronized = true;
                        _index = 0;
                    }
                }

                if (_radioSynchronized && _index > 31)
                {
                    _index = 0;

                    _channelizedValues = _channelizer.Channelize(_values);
                }
            }
        }

        private int GetChannelValue(short channelId)
        {
            if (_channelizedValues.Contains(channelId))
            {
                //Convert the scale on the values to a %.  Min = -170, Max = 170
                return (((int)_channelizedValues[channelId]) / 170) * 100;
            }
            else
            {
                return 0;
            }
        }
    }
}
