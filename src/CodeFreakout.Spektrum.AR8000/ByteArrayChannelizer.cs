using System.Collections;

namespace CodeFreakout.Spektrum.AR8000
{
    public class ByteArrayChannelizer
    {
        public Hashtable Channelize(byte[] values)
        {
            var channelizedValues = new Hashtable();

            var channelValue = new byte[2];

            for (var i = 0; i < 32; i += 2)
            {
                byte byte1 = values[i];
                byte byte2 = values[i + 1];

                channelValue[0] = byte1;
                channelValue[1] = byte2;

                if ((byte1 != 0) && (byte1 != 255 && byte2 != 255))
                {
                    //ChannelId = Bits 2-6 in the first byte
                    var channelId = (byte1 << 1) >> 4;

                    //Value = 2nd Byte with 2s complement sign stored in bit 8 of first byte
                    var value = 0;
                    if ((short) (channelValue[0] & 0x01) == 1)
                    {
                        //Values msb set to 1 which indicates a 2s complement negative number
                        value = (short) ((0xFF << 8) | channelValue[1]);
                    }
                    else
                    {
                        //bitshift left 13 then right 5 to ditch the channel id from clouding up the value
                        value = (short) (((channelValue[0] & 0x01) << 8) | channelValue[1]);
                    }

                    if (channelizedValues.Contains(channelId))
                    {
                        channelizedValues[channelId] = value;
                    }
                    else
                    {
                        channelizedValues.Add(channelId, value);    
                    }
                }
            }

            return channelizedValues;
        }
    }
}
