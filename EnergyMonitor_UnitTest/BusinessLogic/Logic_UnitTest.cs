using EnergyMonitor.BusinessLogic;
using EnergyMonitor.Types;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EnergyMonitor_UnitTest.Utils {

  class LogicAccessor : Logic {

    private DateTime CurrentTime { get; set; }

    public LogicAccessor() : base(true) { }

    protected override DateTime TimeSource { get => CurrentTime; }

    [TestClass]
    public class Logic_UnitTest {
      public LogicAccessor Logic { get; private set; }

      [TestInitialize]
      public void Setup() {
        var config = new Configuration {
          LockTimeStart = new DateTime(1, 1, 1, 8, 0, 0),
          LockTimeEnd = new DateTime(1, 1, 1, 9, 0, 0),
          ForceSwitchOffDelayMinutes = 20
        };

        Logic = new LogicAccessor {
          Configuration = config
        };
      }

      [TestCleanup]
      public void TearDown() {
        Logic.Dispose();
      }

      [TestMethod]
      public void IsLocked_TimeSourceInLockedTimeRange_ReturnsTrue() {
        Logic.CurrentTime = new DateTime(1, 1, 1, 8, 30, 0, 0);
        Assert.IsTrue(Logic.IsLocked());
      }

      [TestMethod]
      public void IsLocked_TimeSourceOutsideLockedTimeRange_ReturnsFalse() {
        Logic.CurrentTime = new DateTime(1, 1, 1, 9, 1, 0, 0);
        Assert.IsFalse(Logic.IsLocked());
      }

      [TestMethod]
      public void IsLockedConsiderSwitchOffDelay_TimeSourceIsInsideLockedTimeConsideringDelay_ReturnsTrue() {
        // Locktime is from 8h to 9h, switchOffDelay is 20 Minutes
        // so the lock should be counting from 7:40h on
        Logic.CurrentTime = new DateTime(1, 1, 1, 8, 40, 0);
        Assert.IsTrue(Logic.IsLockedConsiderSwitchOffDelay());
      }

      [TestMethod]
      public void IsLockedConsiderSwitchOffDelay_TimeSourceIsNotYetInLockedTimeConsideringDelay_ReturnsFalse() {
        // Locktime is from 8h to 9h, switchOffDelay is 20 Minutes
        // so the lock should be counting from 7:40h on
        Logic.CurrentTime = new DateTime(1, 1, 1, 7, 39, 0);
        Assert.IsFalse(Logic.IsLockedConsiderSwitchOffDelay());
      }

      [TestMethod]
      public void IsLockedConsiderSwitchOffDelay_TimeSourceIsStillLockedTime_ReturnsTrue() {
        // Locktime is from 8h to 9h, switchOffDelay is 20 Minutes
        // so the lock should be counting from 7:40h on to 9h
        Logic.CurrentTime = new DateTime(1, 1, 1, 9, 0, 0);
        Assert.IsTrue(Logic.IsLockedConsiderSwitchOffDelay());
      }

      [TestMethod]
      public void IsLockedConsiderSwitchOffDelay_TimeSourceIsOutsideLockedTime_ReturnsFalse() {
        // Locktime is from 8h to 9h, switchOffDelay is 20 Minutes
        // so the lock should be counting from 7:40h on
        Logic.CurrentTime = new DateTime(1, 1, 1, 9, 1, 0);
        Assert.IsFalse(Logic.IsLockedConsiderSwitchOffDelay());
      }

      [TestMethod]
      public void IsLocked_NoLockTimeSet_ReturnsFalse() {
        Logic.Configuration.LockTimeStart = new DateTime();
        Logic.Configuration.LockTimeEnd = new DateTime();
        Assert.IsFalse(Logic.IsLocked());
      }

      [TestMethod]
      public void IsLockedConsiderSwitchOffDelay_NoLockTimeSet_ReturnsFalse() {
        Logic.Configuration.LockTimeStart = new DateTime();
        Logic.Configuration.LockTimeEnd = new DateTime();
        Assert.IsFalse(Logic.IsLockedConsiderSwitchOffDelay());
      }
    }
  }
}