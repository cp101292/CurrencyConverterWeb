namespace CurrencyConverter.Services
{
    public interface IFileReader
    {
        Task<string> ReadAllTextAsync(string path);
    }
}
