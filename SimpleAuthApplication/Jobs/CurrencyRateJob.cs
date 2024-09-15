using Quartz;
using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;
using SimpleAuthApplication.Repositories;
using System.Text.Json;

namespace SimpleAuthApplication.Jobs
{
    public class CurrencyRateJob : IJob
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly HttpClient _httpClient;

        public CurrencyRateJob(ICurrencyRateRepository currencyRateRepository)
        {
            _currencyRateRepository = currencyRateRepository;
            _httpClient = new HttpClient();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/A/EUR/{yesterday}/?format=json";

            try
            {
                // Pobieranie danych z API
                var response = await _httpClient.GetStringAsync(apiUrl);
                var currencyRateDto = JsonSerializer.Deserialize<CurrencyRateDto>(response);

                // Konwersja DTO na model bazy danych
                var currencyRate = new CurrencyRate
                {
                    Id = Guid.NewGuid(),
                    Currency = currencyRateDto.Currency,
                    Code = currencyRateDto.Code,
                    Mid = currencyRateDto.Mid,
                    EffectiveDate = currencyRateDto.EffectiveDate,
                    CreatedAt = DateTime.Now
                };

                // Zapis do bazy danych
                await _currencyRateRepository.AddAsync(currencyRate);
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Error during fetching rates: {ex.Message}");
            }
        }
    }
}
