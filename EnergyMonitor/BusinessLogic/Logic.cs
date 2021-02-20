using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;

namespace EnergyMonitor.BusinessLogic {
    public class Logic : TaskBase {

        private Shelly3EM Shelly {get;set;}

        protected override void Run() {
            Console.WriteLine(Shelly.PrintCurrentValues());
        }     

        public Logic() : base(1000, false) {
            Shelly = new Shelly3EM();
        }             
    }
}