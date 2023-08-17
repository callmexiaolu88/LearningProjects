using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace taskScheduler
{
    public class CustomTaskScheduler : TaskScheduler, IDisposable
    {
        BlockingCollection<Task?> _queue;
        Thread[] _threads;
        private bool disposedValue;

        public CustomTaskScheduler(int maxthreads)
        {
            if (maxthreads < 0 ^ maxthreads > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(maxthreads), maxthreads, $"Thread max number must be greater than 0 and  less than {ushort.MaxValue}");
            _queue = new BlockingCollection<Task?>();
            _threads = new Thread[maxthreads];
            for (int i = 0; i < _threads.Length; i++)
            {
                var thread = new Thread(executeTask)
                {
                    IsBackground = true,
                    Name = $"CustomTaskScheduler worker {i + 1}"
                };
                thread.UnsafeStart();
                _threads[i] = thread;
            }
        }

        public CustomTaskScheduler() : this(Environment.ProcessorCount)
        {

        }

        private void executeTask()
        {
            while (true)
            {
                try
                {
                    var task = _queue.Take();
                    if (task == null)
                        return;
                    TryExecuteTask(task);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine(ex);
                }
            }
        }

        public Task Run(Action action)
            => Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, this);

        protected override IEnumerable<Task>? GetScheduledTasks()
        {
            List<Task> list = new List<Task>();
            foreach (var item in _queue.ToArray())
            {
                if (item != null)
                    list.Add(item);
            }
            return list;
        }

        protected override void QueueTask(Task task)
        {
            if (disposedValue)
                throw new ObjectDisposedException(nameof(CustomTaskScheduler));
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                _queue.Add(task);
            }
            catch (ObjectDisposedException e)
            {
                throw new ObjectDisposedException(nameof(CustomTaskScheduler), e);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (disposedValue)
                throw new ObjectDisposedException(nameof(CustomTaskScheduler));
            return !taskWasPreviouslyQueued && TryExecuteTask(task);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    for (int i = 0; i < _threads.Length; i++)
                        _queue.Add(null);
                    foreach (var item in _threads)
                    {
                        item.Join();
                    }
                    _threads = Array.Empty<Thread>();
                    _queue.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CustomTaskScheduler()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}