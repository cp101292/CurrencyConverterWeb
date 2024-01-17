using CurrencyConverter.Model;

namespace CurrencyConverter.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
    }
}
