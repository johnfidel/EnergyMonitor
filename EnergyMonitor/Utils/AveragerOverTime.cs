using System.Data.Common;
using System.Linq;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace EnergyMonitor.Utils
{
    public class AveragerOverTime : TaskBase {
        private ConcurrentDictionary<DateTime, double> Values {get; set;}
        private TimeSpan Span{get;set;}
        
        protected virtual DateTime TimeSource {get => DateTime.Now;}
        
        protected void UpdateValues() {
            var toRemove = Values.Where((o) => ((TimeSource - o.Key) > Span));            
            foreach (var i in toRemove) {
                Values.TryRemove(i.Key, out var _);
            }
        }

        public AveragerOverTime(TimeSpan span, bool suspended) : base(100, suspended) { 
            Span = span;
            Values = new ConcurrentDictionary<DateTime, double>();
        }

        public void Add(DateTime timeStamp, double value) {
            Values.TryAdd(timeStamp, value);
        }

        public double GetAverage() {
            return Values.Select((o) => o.Value).ToList().Average();
        }

        public double GetSummary() {
            return Values.Select((o) => o.Value).ToList().Sum();
        }
    
        protected override void Run() {
            UpdateValues();
        }
    }
}