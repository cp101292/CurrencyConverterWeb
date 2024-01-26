using CurrencyConverter.Model;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CurrencyConversionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("convert")]
        public IActionResult ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var key = ($"{sourceCurrency}_TO_{targetCurrency}").ToUpper();
                var rate = Convert.ToDecimal(_configuration[key]);
                    //_exchangeRates.SingleOrDefault(r => r.BaseCurrency == sourceCurrency && r.TargetCurrency == targetCurrency)?.Amount;
                    
                var convertedAmount = amount * rate;
                var result = new CurrencyConversion
                {
                    ExchangeRate = rate,
                    ConvertedAmount = convertedAmount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request."+ex);
            }

        }
    }

}
