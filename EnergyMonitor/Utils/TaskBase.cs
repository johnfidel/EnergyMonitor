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

    protected virtual void Dispose(bool disposing) {
      if (!disposedValue) {
        if (disposing) {
          // TODO: dispose managed state (managed objects)
        }

        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        // TODO: set large fields to null
        disposedValue = true;
      }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TaskBase()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose() {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}