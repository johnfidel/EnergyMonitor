using System.Collections.Generic;

namespace EnergyMonitor.Utils
{
    public class Average {
        public List<double> Values{get;set;}

        public Average(int order) {
            Values = new List<double>(order);
        }
    }
}