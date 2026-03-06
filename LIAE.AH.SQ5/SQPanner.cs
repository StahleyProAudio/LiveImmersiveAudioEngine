using LIAE.Core.Interfaces;

namespace LIAE.AH.SQ5
{
    public class SQPanner : IPan
    {
        public int MSB { get; set; }
        public int LSB { get; set; }
        public int VCoarse { get; set; }
        public int VFine { get; set; }

        public SQPanner(int panPos)
        {
            if(panPos < 0 || panPos > 16383)
            {
                throw new ArgumentOutOfRangeException(nameof(panPos), "Pan position must be between 0 and 16383.");
            }

            MSB = (panPos >> 7) & 0x7F; // Get the upper 7 bits for MSB
            LSB = panPos & 0x7F;        // Get the lower 7 bits for LSB
            VCoarse = MSB;
            VFine = LSB;
        }
    }
}
