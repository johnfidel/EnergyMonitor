using EnergyMonitor.BusinessLogic;
using EnergyMonitor.Types;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.InteropServices;

namespace EnergyMonitor_UnitTest.Utils {

  class Dummy : Serializable {
    public string Property1 { get; set; }
  }

  [TestClass]
  public class Serializable_UnitTest {

    [TestInitialize]
    public void Setup() {
      if (File.Exists(State.FILENAME)) {
        File.Delete(State.FILENAME);
      }
    }

    [TestMethod]
    public void Serialize_PassDummyInstance_DoesCreateExpectedString() {
      var instance = new Dummy {
        Property1 = "test"
      };
      var json = instance.ToJson();
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
        Assert.AreEqual("{\r\n  \"Property1\": \"test\"\r\n}", json);
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
        Assert.AreEqual("{\n  \"Property1\": \"test\"\n}", json);
      }

    }

    [TestMethod]
    public void Deserialize_PassJsonString_DoesCreateInstance() {
      var json = "{\"Property1\":\"test\"}";
      var instance = Serializable.FromJson<Dummy>(json);
      Assert.IsNotNull(instance);
      Assert.AreEqual("test", instance.Property1);
    }

    [TestMethod]
    public void FromFile_PassExistingFile_DoesCreateInstance() {
      var state = new State {
        ActualAveragePower = 666,
        ActualOutputState = OutputState.On
      };
      state.Serialize();

      var instance = Serializable.FromFile<State>(State.FILENAME);
      Assert.AreEqual(666, instance.ActualAveragePower);
      Assert.AreEqual(OutputState.On, instance.ActualOutputState);
    }

    [TestMethod]
    public void FromFile_FileInexistent_DoesCreateDefaultInstance() {
      var instance = Serializable.FromFile<State>(State.FILENAME);
      Assert.AreEqual(0, instance.ActualAveragePower);
      Assert.AreEqual(OutputState.Unknown, instance.ActualOutputState);
    }

    //[TestMethod]
    public void Deserialize_PasInvalidJson_Does() {
      var test = Serializable.FromJson<Dummy>("blabla");
    }
  }

}