using InventorySystem.Domain;

namespace InventorySystem.Infrastructure;

public class JsonProductRepository : IProductRepository
{
    private string _filePath = "Data/products.json";
    public Product? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Product product)
    {
        throw new NotImplementedException();
    }

    public void Update(Product product)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void SaveProductsToFile(List<Product> products)
    {
        _ensureDirectory();
        string json = System.Text.Json.JsonSerializer.Serialize(products);
        File.WriteAllText(_filePath, json);
    }

    public List<Product> LoadProductsFromFile()
    {
        _ensureDirectory();
        string json = File.ReadAllText(_filePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }

    private void _ensureDirectory()
    {
        string directory = Path.GetDirectoryName(_filePath)!;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
}