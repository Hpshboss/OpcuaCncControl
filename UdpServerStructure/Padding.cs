using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClientStructure
{
    class Padding
    {
        string property;
        public Padding(string property, int byteNum)
        {
            this.property = property;
            int standard = Encoding.ASCII.GetByteCount(this.property);
            for (byte i=0; i < byteNum * 2 - standard; i++)
            {
                this.property = "0" + this.property;
            }
        }
        public string GOGO()
        {
            return this.property;
        }
    }
}
