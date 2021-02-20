using EnergyMonitor.Devices.PowerMeter.Shelly.Types;

namespace EnergyMonitor.Devices.PowerMeter.Types
{
    public class Phase
    {
        public double Voltage {get;set;}
        public double Current {get;set;}
        public double Power {get;set;}

        public void FromEmeterData(Emeter data) {
            Voltage = data.voltage;
            Current = data.current;
            Power = data.power;
        }

        public override string ToString() {
            return $"{Voltage}V {Current}A {Power}W";
        }
    }
}