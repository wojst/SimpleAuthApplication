using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task AddAsync(CurrencyRate currencyRate);
    }
}