using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnergyMonitor.Utils;

namespace EnergyMonitor_UnitTest.Utils
{
    public class DummyTask : TaskBase {

        public bool RunPerformed{get;set;}
        protected override void Run() {
            RunPerformed = true;
            Terminate = true;
        }
    }


    [TestClass]
    public class TaskBase_UnitTest {
        public DummyTask ATask { get; private set; }

        [TestInitialize]
        public void Setup() {
            ATask = new DummyTask();
        }

        [TestMethod]
        public void Start_TaskStopped_DoesStartTask() {
            while (ATask.Status == System.Threading.Tasks.TaskStatus.Running) { Thread.Sleep(100);}
            Assert.IsTrue(ATask.RunPerformed);
        }
    }
}