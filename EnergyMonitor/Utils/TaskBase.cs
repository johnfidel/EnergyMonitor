using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyMonitor.Utils
{
  abstract public class TaskBase
  {
    private Task Worker { get; set; }
    protected CancellationTokenSource CancellationToken { get; set; }
    public int Cycle { get; set; }

    protected abstract void Run();

    public void Start()
    {
      Worker = Task.Factory.StartNew(() =>
      {
        try
        {
          while (!Terminate && !CancellationToken.IsCancellationRequested)
          {
            Run();

            Thread.Sleep(Cycle);
          }
        }

        catch (Exception e) { 
          Logging.Instance().Log(new LogMessage($"Exception in {this.GetType().Name} {e.Message}"));
        }
      }, CancellationToken.Token);
    }

    public void Stop()
    {
      CancellationToken.Cancel();
    }

    public TaskStatus Status
    {
      get => Worker.Status;
    }

    public TaskBase() : this(100, false) { }

    public bool Terminate { get; set; }

    public TaskBase(int cycle, bool suspended)
    {
      Cycle = cycle;
      CancellationToken = new CancellationTokenSource();
      if (!suspended) Start();
    }

  }
}