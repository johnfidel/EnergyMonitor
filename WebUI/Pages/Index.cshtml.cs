using System.Net;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EnergyMonitor.BusinessLogic;
using EnergyMonitor.Utils;

namespace WebUI.Pages {
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
  }
}
