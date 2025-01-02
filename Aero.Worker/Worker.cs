namespace Aero.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var module = _configuration["module"];
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} with module: {module}");

            try
            {
                await Task.Delay(1_000, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
