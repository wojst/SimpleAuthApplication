using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;
using SimpleAuthApplication.Repositories;
using System.Text.Json;

namespace SimpleAuthApplication.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyService(HttpClient httpClient, ICurrencyRateRepository currencyRateRepository)
        {
            _httpClient = httpClient;
            _currencyRateRepository = currencyRateRepository;
        }

        public async Task FetchCurrencyRates()
        {
            var yesterday = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd");
            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/A/EUR/{yesterday}/?format=json";

            Console.WriteLine($"Fetching data from: {apiUrl}");

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {content}");

                var currencyRateDto = JsonConvert.DeserializeObject<CurrencyRateDto>(content);

                var newRate = new CurrencyRate
                {
                    Id = Guid.NewGuid(),
                    Currency = currencyRateDto.Currency,
                    Code = currencyRateDto.Code,   
                    Mid = currencyRateDto.Mid,
                    EffectiveDate = currencyRateDto.EffectiveDate,
                    CreatedAt = DateTime.Now,
                };

                await _currencyRateRepository.AddAsync(newRate);
            }
            else
            {
                Console.WriteLine($"Error fetching currency rates: {response.StatusCode}");
            }
        }


    }
}
