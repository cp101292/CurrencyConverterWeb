using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Model
{
    public class ExchangeRate
    {
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string? BaseCurrency { get; set; }

        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string? TargetCurrency { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue)]
        public decimal Amount { get; set; }
    }

}
