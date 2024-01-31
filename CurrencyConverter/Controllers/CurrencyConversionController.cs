using CurrencyConverter.Model;
using CurrencyConverter.Service;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversionService _conversionService;

        public CurrencyConversionController(ICurrencyConversionService conversionService)
        {
            _conversionService = conversionService;
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

                var result = _conversionService.ConvertCurrency(sourceCurrency, targetCurrency, amount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request." + ex);
            }
        }
    }
}