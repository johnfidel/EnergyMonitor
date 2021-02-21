using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;
using System.IO;

namespace EnergyMonitor.BusinessLogic {
    public class Logic : TaskBase {

        private Shelly3EM Shelly {get;set;}

        protected override void Run() {
            if (!File.Exists("data.csv")) {
                File.WriteAllText("data.csv", $"Date/Time;{Shelly.Phase1.CsvHeader()};"+
                    $"{Shelly.Phase2.CsvHeader()};"+
                    $"{Shelly.Phase3.CsvHeader()};\n");
            }
            File.AppendAllText("data.csv", $"{DateTime.UtcNow};{Shelly.Phase1.ToCsvString()};"+
                $"{Shelly.Phase2.ToCsvString()};"+
                $"{Shelly.Phase3.ToCsvString()};\n");
            
        }     

        public Logic() : base(1000, false) {
            Shelly = new Shelly3EM();
        }             
    }
}