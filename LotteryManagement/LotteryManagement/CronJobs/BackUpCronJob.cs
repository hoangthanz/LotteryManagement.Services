using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LotteryManagement.CronJobs
{
    public class BackUpCronJob : CronJobService
    {
        private readonly ILogger<BackUpCronJob> _logger;

        public BackUpCronJob(IScheduleConfig<BackUpCronJob> config, ILogger<BackUpCronJob> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob BackUp-Data starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob BackUp-Data is working.");
            BackUpData();


            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob BackUp-Data is stopping.");
            return base.StopAsync(cancellationToken);
        }



        private void BackUpData()
        {

            //var conn = new SqlConnection("Data Source=DESKTOP-3ULBIDF;Initial Catalog=lotteryManagementDb;Integrated Security=True");
            //try
            //{
            //    conn.Open();
            //    SqlCommand command = conn.CreateCommand();
            //    command.CommandText = "BACKUP DATABASE lotteryManagementDb FROM DISK = 'C:\\AdventureWorks.BAK' WITH REPLACE GO";
            //    command.ExecuteNonQuery();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    conn.Dispose();
            //    conn.Close();
            //}
        }

    }
}
