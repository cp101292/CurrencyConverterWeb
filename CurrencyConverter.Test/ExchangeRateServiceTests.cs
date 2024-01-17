using System.Globalization;
using Castle.Core.Logging;
using CurrencyConverter.Model;
using Moq;
using NUnit.Framework;
using CurrencyConverter.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CurrencyConverter.Test
{
    [TestFixture]
    public class ExchangeRateServiceTests
    {
        private Mock<ILogger<ExchangeRateService>> _mockLogger;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IConfigurationSection> _mockConfigurationSection;
        private Mock<IFileReader> _mockFileReader;
        private ExchangeRateService _service;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateService>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockFileReader = new Mock<IFileReader>();
            _mockConfigurationSection = new Mock<IConfigurationSection>();

            _mockConfigurationSection.SetupGet(x => x.Value).Returns("CurrencyConverter/exchangeRates.json");
            _mockConfiguration.Setup(x => x.GetSection("ExchangeRatesFilePath")).Returns(_mockConfigurationSection.Object);

            _service = new ExchangeRateService(_mockLogger.Object, _mockConfiguration.Object, _mockFileReader.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_ReturnsExchangeRates_WhenFileIsValid()
        {
            // Arrange
            string validJson = "{ \"USD_TO_INR\": 74.00, \"INR_TO_USD\": 0.013 }";
            _mockFileReader.Setup(x => x.ReadAllTextAsync(It.IsAny<string>())).ReturnsAsync(validJson);

            // Act
            var result = await _service.GetExchangeRatesAsync();

            var exchangeRate = result.FirstOrDefault(rate =>
                rate is ExchangeRate { BaseCurrency: "USD", TargetCurrency: "INR", Amount: 74.00M });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(exchangeRate?.BaseCurrency?.Equals("USD"));
            Assert.IsTrue(exchangeRate?.TargetCurrency?.Equals("INR"));
            Assert.IsTrue(exchangeRate?.Amount.ToString(CultureInfo.InvariantCulture).Equals("74.00"));

        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsFileNotFoundException_WhenFileNotFound()
        {
            // Arrange
            _mockFileReader.Setup(x => x.ReadAllTextAsync(It.IsAny<string>())).Throws<FileNotFoundException>();

            // Act & Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await _service.GetExchangeRatesAsync());
        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsIOException_WhenIoExceptionOccurs()
        {
            // Arrange
            _mockFileReader.Setup(x => x.ReadAllTextAsync(It.IsAny<string>())).Throws<IOException>();

            // Act & Assert
            Assert.ThrowsAsync<IOException>(async () => await _service.GetExchangeRatesAsync());
        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsJsonException_WhenInvalidJson()
        {
            // Arrange
            string invalidJson = "Invalid JSON";
            _mockFileReader.Setup(x => x.ReadAllTextAsync(It.IsAny<string>())).ReturnsAsync(invalidJson);

            // Act & Assert
            Assert.ThrowsAsync<JsonException>(async () => await _service.GetExchangeRatesAsync());
        }

    }

}
