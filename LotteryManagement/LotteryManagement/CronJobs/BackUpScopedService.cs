using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LotteryManagement.CronJobs
{

    public interface IBackUpScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }

    public class BackUpScopedService : IBackUpScopedService
    {
        private readonly ILogger<BackUpScopedService> _logger;

        public BackUpScopedService(ILogger<BackUpScopedService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} MyScopedService is working.");
            await Task.Delay(1000 * 20, cancellationToken);
        }
    }
  
}
