using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using SimpleAuthApplication.Services;

namespace SimpleAuthApplication.Jobs
{
    public class CurrencyRateRegistry : Registry
    {
        public CurrencyRateRegistry(IServiceScopeFactory serviceScopeFactory)
        {
            Console.WriteLine("CurrencyRateRegistry initialized"); // LOG

            // Harmonogram dla CurrencyRateJob
            Schedule(() =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                    var job = new CurrencyRateJob(currencyService);
                    job.Execute();
                }
            }).ToRunNow().AndEvery(1).Days();

            //Schedule<TestJob>().ToRunNow().AndEvery(1).Seconds();

            Console.WriteLine("CurrencyRateJob scheduled to run every minute."); // LOG
        }
    }
}
