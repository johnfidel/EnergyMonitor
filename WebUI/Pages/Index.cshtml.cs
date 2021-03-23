using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EnergyMonitor.BusinessLogic;
using System.Runtime.Serialization;
using ASPNET_MVC_ChartsDemo.Models;

namespace ASPNET_MVC_ChartsDemo.Models
{
  //DataContract for Serializing Data - required to serve in JSON format
  [DataContract]
  public class DataPoint
  {
    public DataPoint(string label, double y)
    {
      this.Label = label;
      this.Y = y;
    }

    //Explicitly setting the name to be used while serializing to JSON.
    [DataMember(Name = "label")]
    public string Label = "";

    //Explicitly setting the name to be used while serializing to JSON.
    [DataMember(Name = "y")]
    public Nullable<double> Y = null;
  }
}

namespace WebUI.Pages {
  public class IndexModel : PageModel {
    public Logic GetLogic() {
      return Program.Logic;
    }

    public JsonResult GetAreaChartData() {
      List<string[]> data = new List<string[]>();
      data.Add(new[] { "name", "score" });
      data.Add(new[] { "xyz", "30" });
      data.Add(new[] { "aaa", "135", });
      return new JsonResult(data);
    }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger) {
      _logger = logger;
    }

    public void OnGet() {
    }
  }
}
