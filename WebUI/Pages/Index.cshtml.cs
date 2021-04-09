using System.Net;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EnergyMonitor.BusinessLogic;
using EnergyMonitor.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using EnergyMonitor.Types;

namespace WebUI.Pages {

  public class ConfigurationModel {
    [BindProperty]
    public DateTime LockTimeStart { get; set; }
    [BindProperty]
    public DateTime LockTimeEnd { get; set; }
    [BindProperty]
    public double OffThreshold { get; set; }
    [BindProperty]
    public double OnThreshold { get; set; }
  }

  public class IndexModel : PageModel {
    public State GetState() {
      return Serializable.FromJson<State>(System.IO.File.ReadAllText(State.FILENAME));
    }

    public Configuration GetConfiguration() {
      return Serializable.FromJson<Configuration>(System.IO.File.ReadAllText(Configuration.CONFIG_FILE_NAME));
    }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger) {
      _logger = logger;
    }

    public void OnGet() {
    }

    public void OnPostSave(ConfigurationModel config) {
      var c = GetConfiguration();
      if (config.LockTimeStart != new DateTime()) {
        c.LockTimeStart = config.LockTimeStart;
      }
      if (config.LockTimeEnd != new DateTime()) {
        c.LockTimeEnd = config.LockTimeEnd;
      }
      c.OffThreshold = config.OffThreshold;
      c.OnThreshold = config.OnThreshold;
      c.Save();
    }
  }
}
