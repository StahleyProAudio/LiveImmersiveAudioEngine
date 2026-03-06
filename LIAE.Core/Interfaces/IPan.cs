namespace LIAE.Core.Interfaces
{
    public interface IPan
    {
        // MSB and LSB for 14-bit pan value (0..16383) as per MIDI NRPN spec.
        public int MSB { get; set; }
        public int LSB { get; set; }
        public int VCoarse { get; set; }
        public int VFine { get; set; }
    }
}
