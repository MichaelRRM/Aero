using Aero.Application.Workers;

namespace Aero.Worker
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public WorkerService(ILogger<WorkerService> logger, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var applicationName = _configuration["application"] ?? throw new Exception("No application specified. Please use --module=<moduleName");
                var workerName = _configuration["worker"] ?? throw new Exception("No worker specified. Please use --worker=<moduleName");
                var moduleName = _configuration["module"] ?? throw new Exception("No module specified. Please use --module=<moduleName");
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} with application: {applicationName}, worker: {workerName}, module: {moduleName}");
                
                var worker = WorkerFactory.GetWorker(applicationName, workerName, serviceProvider: _serviceProvider);
                var module = worker.GetModule(moduleName);
                
                await module.RunAsync();
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1;
                _logger.LogError(e, $"An error occured during module execution.");
                throw;
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
