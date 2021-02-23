using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;
using System.IO;

namespace EnergyMonitor.BusinessLogic {
    public class Logic : TaskBase {

        private Shelly3EM Shelly {get;set;}
        private AveragerOverTime Averager {get;set;}
        private Configuration Configuration {get;set;}

        protected override void Run() {
            // if (!File.Exists("data.csv")) {
            //     File.WriteAllText("data.csv", $"Date/Time;{Shelly.Phase1.CsvHeader()};"+
            //         $"{Shelly.Phase2.CsvHeader()};"+
            //         $"{Shelly.Phase3.CsvHeader()};\n");
            // }
            // File.AppendAllText("data.csv", $"{DateTime.UtcNow};{Shelly.Phase1.ToCsvString()};"+
            //     $"{Shelly.Phase2.ToCsvString()};"+
            //     $"{Shelly.Phase3.ToCsvString()};\n");

            Averager.Add(DateTime.Now, Shelly.ActualPowerTotal);            
            var average = Averager.GetAverage();
            Console.WriteLine(average);

            if (average <= Configuration.OnThreshold) {
                Console.WriteLine("On");
            }
            else if (average > Configuration.OffThreshold) {
                Console.WriteLine("Off");
            }

        }     

        public Logic() : base(5000, true) {
            Shelly = new Shelly3EM();
            Averager = new AveragerOverTime(new TimeSpan(0,5,0), false);
            Configuration = Configuration.Load();

            Start();
        }             
    }
}