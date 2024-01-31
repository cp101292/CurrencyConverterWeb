using CurrencyConverter.Model;

namespace CurrencyConverter.Service
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly IConfiguration _configuration;

        public CurrencyConversionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CurrencyConversion ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            var key = ($"{sourceCurrency}_TO_{targetCurrency}").ToUpper();
            var rate = Convert.ToDecimal(_configuration[key]);
            var convertedAmount = amount * rate;

            return new CurrencyConversion
            {
                ExchangeRate = rate,
                ConvertedAmount = convertedAmount
            };
        }
    }
}
