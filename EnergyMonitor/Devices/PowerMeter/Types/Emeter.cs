using System;
namespace EnergyMonitor.Devices.PowerMeter.Shelly.Types
{
    public class Emeter
    {
        public double power { get; set; }
        public double pf { get; set; }
        public double current { get; set; }
        public double voltage { get; set; }
        public bool is_valid { get; set; }
        public double total { get; set; }
        public double total_returned { get; set; }

        public Emeter() { }

        public override string ToString()
        {
            return $"{voltage}V {current}A {power}W";
        }
    }
}
