using CurrencyConverter.Model;
using CurrencyConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyConversionController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();
                var rate = exchangeRates.FirstOrDefault(r => r.BaseCurrency == sourceCurrency && r.TargetCurrency == targetCurrency)?.Amount;

                if (rate == null)
                {
                    return NotFound("Exchange rate not found.");
                }

                var convertedAmount = amount * rate.Value;
                var result = new CurrencyConversion
                {
                    ExchangeRate = rate.Value,
                    ConvertedAmount = convertedAmount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }
    }

}
