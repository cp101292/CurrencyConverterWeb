using CurrencyConverter.Model;

namespace CurrencyConverter.Service
{
    public interface ICurrencyConversionService
    {
        CurrencyConversion ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount);

    }
}
