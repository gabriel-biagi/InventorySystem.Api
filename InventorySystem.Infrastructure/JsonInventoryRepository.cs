using InventorySystem.Domain;

namespace InventorySystem.Infrastructure
{
    public class JsonInventoryRepository : IInventoryRepository
    {
        private string _filePath = "Data/inventory.json";
        public InventoryItem? GetByProductId(int productId)
        {
            var lista = LoadInventoryFromFile();
            return lista.FirstOrDefault(x => productId == x.Product.Id);
        }
        public void Add(InventoryItem inventoryitem)
        {
            var lista = LoadInventoryFromFile();
            lista.Add(inventoryitem);
            SaveInventoryToFile(lista);
        }
        public void Update(InventoryItem inventoryItem)
        {
            var lista = LoadInventoryFromFile();
            var indice = lista.FindIndex(x => inventoryItem.Product.Id == x.Product.Id);
            if (indice >= 0)
            {
                lista[indice] = inventoryItem;
            }
            SaveInventoryToFile(lista);
        }

        public List<InventoryItem> GetAll()
        {
            var lista = LoadInventoryFromFile();
            return lista;
        }

        public void SaveInventoryToFile(List<InventoryItem> inventoryitem)
        {
            _ensureDirectory();
            string json = System.Text.Json.JsonSerializer.Serialize(inventoryitem);
            File.WriteAllText(_filePath, json);
        }

        public List<InventoryItem> LoadInventoryFromFile()
        {
            _ensureDirectory();
            string json = File.ReadAllText(_filePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<InventoryItem>>(json) ?? new List<InventoryItem>();
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
}
