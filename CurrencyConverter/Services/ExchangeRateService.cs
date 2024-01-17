using CurrencyConverter.Model;
using Newtonsoft.Json;

namespace CurrencyConverter.Services
{

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly string _filePath;
        private readonly ILogger<IExchangeRateService> _logger;
        private readonly IFileReader _filerReader;

        public ExchangeRateService(ILogger<ExchangeRateService> logger, IConfiguration configuration, IFileReader fileReader)
        {
            _filePath = configuration.GetValue<string>("ExchangeRatesFilePath");
            _logger = logger;
            _filerReader = fileReader;
        }



        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            try
            {
                var fileContent = await _filerReader.ReadAllTextAsync(_filePath);
                var ratesDictionary = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(fileContent);
                var exchangeRates = new List<ExchangeRate>();

                foreach (var item in ratesDictionary)
                {
                    var currencies = item.Key.Split("_TO_");
                    var rateKey = $"{currencies[0]}_TO_{currencies[1]}_RATE";
                    var rate = Environment.GetEnvironmentVariable(rateKey, EnvironmentVariableTarget.Process) ??
                               item.Value.ToString();

                    var exchangeRate = new ExchangeRate
                    {
                        BaseCurrency = currencies[0],
                        TargetCurrency = currencies[1],
                        Amount = Convert.ToDecimal(rate)
                    };
                    exchangeRates.Add(exchangeRate);
                }

                return exchangeRates;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Exchange rates file not found.");
                throw new FileNotFoundException();
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error occurred while reading exchange rates file.");
                throw new IOException();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error occurred while parsing exchange rates file.");
                throw new JsonException();

            }
        }

    }
}
