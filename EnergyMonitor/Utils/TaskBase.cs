using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyMonitor.Utils
{
  abstract public class TaskBase : IDisposable
  {
    protected bool disposedValue;

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
          Logging.Instance().Log(new LogMessage($"Terminated Thread {this.GetType().Name}"));
        }

        catch (Exception e) { 
          Logging.Instance().Log(new LogMessage($"Exception in {this.GetType().Name} {e.Message}"));
        }
      }, CancellationToken.Token);
      
      Logging.Instance().Log(new LogMessage($"Started Thread {GetType().Name}"));
    }

    public void Stop()
    {
      CancellationToken.Cancel();
    }

    public TaskStatus Status
    {
      get => Worker?.Status ?? TaskStatus.WaitingToRun;
    }

    public TaskBase() : this(100, false) { }

    public bool Terminate { get; set; }

    public TaskBase(int cycle, bool suspended)
    {
      Cycle = cycle;
      CancellationToken = new CancellationTokenSource();
      if (!suspended) Start();
    }

    protected virtual void Dispose(bool disposing) {
      if (!disposedValue) {
        if (disposing) {
          Terminate = true;
        }
        disposedValue = true;
      }
    }

    public void Dispose() {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}