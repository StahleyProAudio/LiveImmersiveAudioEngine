using LIAE.Core.Interfaces;
using System.Net.Sockets;

namespace LIAE.AH.SQ5
{
    public class SQ5Controller : IPan
    {
        public byte Channel { get; set; }
        public int MSB { get; set; }
        public int LSB { get; set; }
        public int VCoarse { get; set; }
        public int VFine { get; set; }

        public SQ5Controller(byte channel, IPan pan)
        {
            Channel = channel;
            MSB = pan.MSB;
            LSB = pan.LSB;
            VCoarse = pan.VCoarse;
            VFine = pan.VFine;
        }

        public SQ5Controller(byte channel, int panLocation)
        {
            var clampPanner = Math.Clamp(panLocation, 0, 16383);

            Channel = channel;
            MSB = (clampPanner >> 7) & 0x7F; // Get the upper 7 bits for MSB
            LSB = clampPanner & 0x7F;        // Get the lower 7 bits for LSB
            VCoarse = MSB;
            VFine = LSB;
        }

        /// <summary>
        /// Set the pan value for the SQ5 per channel based on current values.
        /// </summary>
        public async Task RefreshPanAsync()
        {
            var sqTcp = new SQTcpClient(SQ5Config.IPAddress, SQ5Config.Port);
            if (sqTcp is null) throw new InvalidOperationException("SQ TCP client not initialized.");

            // This method can be used to refresh the the SQ5 board with the current pan values (MSB and LSB) for the specified channel.
            // Build NRPN message for Ip1 -> LR pan:
            // Status B0 = CC on MIDI Channel 1
            // CC#99 (0x63)=0x50, CC#98 (0x62)=0x00 for Ip1->LR [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
            // CC#6 (0x06)=VC, CC#38 (0x26)=VF [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
            byte[] msg = new byte[]
            {
                0xB0, 0x63, 0x50, // CC99: NRPN MSB = 0x50 (Input Pan)
                0xB0, 0x62, Channel, // CC98: NRPN LSB = 0x00 (IP1)
                0xB0, 0x06, 0x40, // CC6 : Value Coarse = 64 (center)
                0xB0, 0x26, 0x00  // CC38: Value Fine   = 0
            };

            await sqTcp.ConnectAsync();
            var sqStream = sqTcp.GetStream();


            await sqStream.WriteAsync(msg, 0, msg.Length);
            await sqStream.FlushAsync();
            sqTcp.Dispose();
        }

        /// <summary>
        /// Refresh the pan values for the SQ5 per channel based on the provided IPan instance.
        /// </summary>
        /// <param name="pan"></param>
        public async Task RefreshPan(IPan pan)
        {
            MSB = pan.MSB;
            LSB = pan.LSB;
            VCoarse = pan.VCoarse;
            VFine = pan.VFine;
            await RefreshPanAsync();
        }

        public void UpdateChannel(byte channel)
        {
            Channel = channel;
        }
    }
}
