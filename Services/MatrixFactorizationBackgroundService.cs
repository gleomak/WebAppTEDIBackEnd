using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class MatrixFactorizationBackgroundService : BackgroundService
    {
        private readonly ILogger<MatrixFactorizationBackgroundService> _logger;
        private readonly IServiceProvider _services;
        private Timer _timer;

        public MatrixFactorizationBackgroundService(ILogger<MatrixFactorizationBackgroundService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Schedule the initial execution immediately.
            PerformMatrixFactorizationAsync();

            // Set the interval (x minutes) for the timer.
            var intervalInMinutes = 5; // Change this to your desired interval in minutes.
            var intervalInMilliseconds = TimeSpan.FromMinutes(intervalInMinutes).TotalMilliseconds;

            _timer = new Timer(PerformMatrixFactorizationAsync, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(intervalInMilliseconds));

            // Continue running until the application is stopped.
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async void PerformMatrixFactorizationAsync(object state = null)
        {
            using (var scope = _services.CreateScope())
            {
                var matrixFactorizationService = scope.ServiceProvider.GetRequiredService<MatrixFactorization>();
                matrixFactorizationService.ResidenceMatrixFactorization();
                _logger.LogInformation("Running matrix factorization");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop the timer when the application is stopped.
            _timer?.Change(Timeout.Infinite, 0);
            await base.StopAsync(cancellationToken);
        }
    }
}