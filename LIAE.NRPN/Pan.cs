using LIAE.Core.Interfaces;

namespace LIAE.NRPN
{
    public class Pan : IPan
    {
        public int MSB { get; set; }
        public int LSB { get; set; }
        public int VCoarse { get; set; }
        public int VFine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // Constructor to initialize pan with default values (center position).
        public Pan()
        {
            MSB = 64;
            LSB = 0;
            VCoarse = 64;
            VFine = 0;
        }

        public Pan(int msb, int lsb)
        {
            if (msb < 0 || msb > 127)
                throw new ArgumentOutOfRangeException(nameof(msb), "MSB must be between 0 and 127.");

            if (lsb < 0 || lsb > 127)
                throw new ArgumentOutOfRangeException(nameof(msb), "LSB must be between 0 and 127.");

            MSB = msb;
            LSB = lsb;
            VCoarse = MSB;
            VFine = LSB;

        }

        public Pan(int location)
        {
           if(location < 0 || location > 16383)
               throw new ArgumentOutOfRangeException(nameof(location), "Location must be between 0 and 16383.");

           MSB = (location >> 7) & 0x7F; // Get the upper 7 bits for MSB
           LSB = location & 0x7F;        // Get the lower 7 bits for LSB
        }
    }
}
