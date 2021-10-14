namespace Net6_Demo.Workers
{
    public class BackgroundTask : BackgroundService
    {
        private readonly ILogger<BackgroundTask> _log;
        private readonly IEnumerable<IWorker> _worker;

        public BackgroundTask(ILogger<BackgroundTask> log, IEnumerable<IWorker> worker)
        {
            _log = log;
            _worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.LogInformation("{Name} is starting.", GetType().Name);

            stoppingToken.Register(() =>
                _log.LogInformation("{Name} is stopping.", GetType().Name));

            // add all worker 
            var workers = new List<Task>();
            foreach (var x in _worker)
            {
                workers.Add(x.DoWork(stoppingToken));
            }

            await Task.WhenAll(workers);

            _log.LogInformation("{Name} is stopping.", GetType().Name);
        }
    }

    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
