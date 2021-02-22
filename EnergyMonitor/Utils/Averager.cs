using System;
using System.Linq;
using System.Collections.Generic;

namespace EnergyMonitor.Utils
{
    public class Averager : List<double> {
        public Averager(int order) : base(order) { }
        public double GetAverage() { return this.Average(); }
        public double GetMin() { return this.Min(); }
        public double GetMax() {return this.Max(); }
        public double GetSummary() {return this.Sum();}
    }
}