using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;


namespace InventorySystem.Infrastructure;

public class JsonProductRepository : IProductRepository
{
    private string _filePath = "Data/products.json";
    public Product? GetById(int id)
    {
        var lista = LoadProductsFromFile();
        return lista.FirstOrDefault(x => id == x.ProductId);
    }

    public void Add(Product product)
    {
        var lista = LoadProductsFromFile();
        lista.Add(product);
        SaveProductsToFile(lista);
    }

    public void Update(Product product)
    {
        var lista = LoadProductsFromFile();
        var indice = lista.FindIndex(x => x.ProductId == product.ProductId);
        if (indice >= 0)
        {
            lista[indice] = product;
        }
        SaveProductsToFile(lista);
    }

    public void Delete(int id)
    {
        var lista = LoadProductsFromFile();
        lista.RemoveAll(x => x.ProductId == id);
        SaveProductsToFile(lista);
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