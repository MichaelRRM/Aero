using Aero.Application.Workers;
using Aero.Base.Constants;
using Aero.Base.Exceptions;

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
                var applicationName = _configuration[$"{WorkerArgumentNames.Application}"] ?? throw new MissingConfigurationException($"No application specified. Please use --{WorkerArgumentNames.Application}=<applicationName>");
                var workerName = _configuration[$"{WorkerArgumentNames.WorkerName}"] ?? throw new MissingConfigurationException($"No worker specified. Please use --{WorkerArgumentNames.WorkerName}=<workerName>");
                var moduleCode = _configuration[$"{WorkerArgumentNames.ModuleCode}"] ?? throw new MissingConfigurationException($"No module specified. Please use --{WorkerArgumentNames.ModuleCode}=<moduleName>");
                var environment = _configuration[$"{WorkerArgumentNames.WorkerEnvironment}"];
                var user = _configuration[$"{WorkerArgumentNames.UserName}"];
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} with application: {applicationName}, worker: {workerName}, module: {moduleCode}, environment: {environment}, user: {user}");

                using var scope = _serviceProvider.CreateScope();
                var workerFactory = scope.ServiceProvider.GetRequiredService<WorkerFactory>();
                var worker = workerFactory.GetWorker(applicationName, workerName);
                var module = worker.GetModule(moduleCode);
                
                await module.RunAsync();
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1;
                _logger.LogError(e, $"An error occured during module execution.");
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
