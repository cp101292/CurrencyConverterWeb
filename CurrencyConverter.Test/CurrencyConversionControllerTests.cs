using CurrencyConverter.Controllers;
using CurrencyConverter.Model;
using CurrencyConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CurrencyConverter.Test
{

    [TestFixture]
    public class CurrencyConversionControllerTests
    {
        private CurrencyConversionController _controller;
        private Mock<IExchangeRateService> _exchangeRateServiceMock;

        [SetUp]
        public void Setup()
        {
            _exchangeRateServiceMock = new Mock<IExchangeRateService>();
            _controller = new CurrencyConversionController(_exchangeRateServiceMock.Object);
        }

        [Test]
        public async Task ConvertCurrency_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var exchangeRates = new List<ExchangeRate>
            {
                new() { BaseCurrency = "USD", TargetCurrency = "INR", Amount = 74.00m },
                new() { BaseCurrency = "INR", TargetCurrency = "USD", Amount = 100.00m },
                new() { BaseCurrency = "USD", TargetCurrency = "EUR", Amount = 200.00m },
                new() { BaseCurrency = "EUR", TargetCurrency = "USD", Amount = 300.00m },
                new() { BaseCurrency = "EUR", TargetCurrency = "INR", Amount = 400.00m },
                new() { BaseCurrency = "INR", TargetCurrency = "EUR", Amount = 500.00m }
            };

            _exchangeRateServiceMock.Setup(service => service.GetExchangeRatesAsync()).ReturnsAsync(exchangeRates);

            // Act
            var result = await _controller.ConvertCurrency("USD", "INR", 100.0m) as ObjectResult;
            //TODO : All the conversion parameter can be tested.

            // Assert
            Assert.IsNotNull(result);
            Assert.That(200, Is.EqualTo(result?.StatusCode));
            var currencyConversionResult = result.Value as CurrencyConversion;
            Assert.IsNotNull(currencyConversionResult);
            Assert.That(74.00m,  Is.EqualTo(currencyConversionResult?.ExchangeRate));
            Assert.That(7400.0m, Is.EqualTo(currencyConversionResult?.ConvertedAmount));
        }

        [Test]
        public async Task ConvertCurrency_InvalidInput_ReturnsBadRequest()
        {
            // Arrange: Simulate ModelState.IsValid = false
            _controller.ModelState.AddModelError("sourceCurrency", "Invalid sourceCurrency");

            // Act
            var result = await _controller.ConvertCurrency("USD", "INR", 100.0m) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(400, Is.EqualTo(result?.StatusCode));
            // Add more assertions for ModelState errors as needed
        }

        [Test]
        public async Task ConvertCurrency_RateNotFound_ReturnsNotFound()
        {
            // Arrange: Simulate that the exchange rate is not found
            var exchangeRates = new List<ExchangeRate>();

            _exchangeRateServiceMock.Setup(service => service.GetExchangeRatesAsync()).ReturnsAsync(exchangeRates);

            // Act
            var result = await _controller.ConvertCurrency("USD", "INR", 100.0m) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(404, Is.EqualTo(result?.StatusCode));
            Assert.That("Exchange rate not found.", Is.EqualTo(result?.Value));
        }
    }
}
