using LIAE.AH.SQ5.Enums;

namespace LIAE.AH.SQ5
{
    public class SQ5Config
    {
        public static string IPAddress = "192.168.1.124"; // default IP address for SQ5 (can be changed in settings)
        public static int Port = 51325; // default port for SQ5 MIDI over TCP/IP (per A&H documentation)



        // Map percent to SQ pan range 0x0000..0x7F7F, with center 0x3F7F. [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
        public static int PAN_MIN = 0x0000;   // full left 00 00 [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
        public static int PAN_MAX = 0x7F7F;   // 32,639 full right 7F 7F [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
        public static int PAN_CTR = 0x3F7F;   // center    3F 7F [1](https://www.youtube.com/playlist?list=PL5TghGDaQ_pgeZEKLRMfOCIq2qnnpBWA5)
        public static int GetSqInputSourceValue(
            SqInputSourceType type,
            int index // 1-based
        )
        {
            return type switch
            {
                SqInputSourceType.Off => 0,

                SqInputSourceType.Local => index,                 // 1–12

                SqInputSourceType.SLink => 12 + index,             // 13–36

                SqInputSourceType.USB => 36 + index,             // 37–44

                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
