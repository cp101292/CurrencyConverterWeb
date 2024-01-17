namespace CurrencyConverter.Services;

public class FileReader : IFileReader
{
    public async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }
}