using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace DowUmg.Presentation.Handlers
{
    public class DefaultExceptionHandler : IObserver<Exception>, IEnableLogger
    {
        private readonly ILogger logger;

        public DefaultExceptionHandler()
        {
            this.logger = this.Log();
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }

        public void OnError(Exception error)
        {
            HandleError(error);
        }

        public void OnNext(Exception value)
        {
            HandleError(value);
        }

        private void HandleError(Exception error)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            this.logger.Write(error, "Unhandled exception was thrown", LogLevel.Fatal);

            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }
    }
}
