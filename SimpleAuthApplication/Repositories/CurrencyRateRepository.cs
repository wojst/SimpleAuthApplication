using SimpleAuthApplication.Data;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CurrencyRateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CurrencyRate currencyRate)
        {
            await _dbContext.CurrencyRates.AddAsync(currencyRate);
            await _dbContext.SaveChangesAsync();
        }

    }
}
