using FluentScheduler;
using SimpleAuthApplication.Services;

namespace SimpleAuthApplication.Jobs
{
    public class CurrencyRateJob : IJob
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyRateJob(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public void Execute()
        {
            Console.WriteLine($"CurrencyRateJob executed at: {DateTime.Now}"); 

            try
            {
                _currencyService.FetchCurrencyRates().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching currency rates: {ex.Message}");
            }
        }
    }
}
