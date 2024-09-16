using FluentScheduler;

namespace SimpleAuthApplication.Jobs
{
    public class CurrencyRateRegistry : Registry
    {
        public CurrencyRateRegistry()
        {
            Console.WriteLine("CurrencyRateRegistry initialized"); // LOG
            // Codziennie o 12
            Schedule<CurrencyRateJob>().ToRunNow().AndEvery(1).Minutes();
            Schedule<TestJob>().ToRunNow().AndEvery(1).Seconds();

            Console.WriteLine("CurrencyRateJob scheduled to run every minute."); // LOG
        }
    }
}
