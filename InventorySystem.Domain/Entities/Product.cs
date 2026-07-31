

namespace InventorySystem.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public UnitType UnitType { get; private set; }

        public Product(string name, UnitType unitType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.");
            }
            Name = name;
            UnitType = unitType;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.");
            }
            Name = name;
        }
    }
}
