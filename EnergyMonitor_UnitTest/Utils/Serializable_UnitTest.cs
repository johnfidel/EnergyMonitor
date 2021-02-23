using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils
{

  class Dummy : Serializable
  {
    public string Property1 { get; set; }
  }

  [TestClass]
  public class Serializable_UnitTest
  {
    [TestMethod]
    public void Serialize_PassDummyInstance_DoesCreateExpectedString()
    {
        var instance = new Dummy {
            Property1 = "test"
        };
        var json = instance.ToJson();
        Assert.AreEqual("{\"Property1\":\"test\"}", json);
    }

    [TestMethod]
    public void Deserialize_PassJsonString_DoesCreateInstance() {
        var json = "{\"Property1\":\"test\"}";
        var instance = Serializable.FromJson<Dummy>(json);
        Assert.IsNotNull(instance);
        Assert.AreEqual("test", instance.Property1);
    }
  }

}