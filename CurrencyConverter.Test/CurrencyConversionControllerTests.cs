using CurrencyConverter.Controllers;
using CurrencyConverter.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace CurrencyConverter.Test
{

    [TestFixture]
    public class CurrencyConversionControllerTests
    {
        private CurrencyConversionController _controller;
        private IConfiguration configuration;
        [SetUp]
        public void Setup()
        {
            // Arrange

            configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"USD_TO_INR", "74.00"},
                    {"INR_TO_USD", "0.013"},
                    {"USD_TO_EUR", "0.85"},
                    {"EUR_TO_USD", "1.18"},
                    {"INR_TO_EUR", "0.011"},
                    {"EUR_TO_INR", "88.00"},
                })
                .Build();

            _controller = new CurrencyConversionController(configuration);
        }

        [TestCase("USD", "INR",1.00 )]
        [TestCase("INR", "USD",74.00)]
        [TestCase("USD", "EUR",200.00)]
        [TestCase("EUR", "USD",300.00)]
        [TestCase("EUR", "INR",1.00)]
        [TestCase("INR", "EUR",1000.00)]
        public void ConvertCurrency_ValidInput_ReturnsOkResult(string sourceCurrency, string targetCurrency, decimal baseAmount)
        {
            // Act
            var resultUsdToInr =  _controller.ConvertCurrency(sourceCurrency, targetCurrency, baseAmount) as ObjectResult;

            var currencyConversionResult = resultUsdToInr.Value as CurrencyConversion;
            var key = ($"{sourceCurrency}_TO_{targetCurrency}").ToUpper();
            var rate = Convert.ToDecimal(configuration[key]);
        
            // Assert
            Assert.IsNotNull(resultUsdToInr);
            Assert.That(200, Is.EqualTo(resultUsdToInr?.StatusCode));
            Assert.IsNotNull(currencyConversionResult);
            Assert.That(rate,  Is.EqualTo(currencyConversionResult?.ExchangeRate));
            Assert.That(rate * baseAmount, Is.EqualTo(currencyConversionResult?.ConvertedAmount));
        }

        [Test]
        public void ConvertCurrency_InvalidInput_ReturnsBadRequest()
        {
            // Arrange: Simulate ModelState.IsValid = false
            _controller.ModelState.AddModelError("sourceCurrency", "Invalid sourceCurrency");

            // Act
            var result =  _controller.ConvertCurrency("USD", "INR", 100.0m) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(400, Is.EqualTo(result?.StatusCode));
            // Add more assertions for ModelState errors as needed
        }

    }
}
