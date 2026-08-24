

using System.ComponentModel.DataAnnotations;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; private set; }
        
        [StringLength(80)]
        public string Name { get; private set; }
        public UnitType UnitType { get; private set; }

        public Product(string name, UnitType unitType)
        {
            ValidateName(name);
            Name = name;
            UnitType = unitType;
        }

        public void UpdateName(string name)
        {
            ValidateName(name);
            Name = name;
        }
        
        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 5)
            {
                throw new ArgumentException("The product name must be at least 5 characters long.");
            }
        }
    }
}
