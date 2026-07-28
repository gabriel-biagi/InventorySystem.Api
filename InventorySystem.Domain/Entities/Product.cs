using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public UnitType UnitType { get; private set; }

        public Product(string name, int id, UnitType unitType)
        {
            Name = name;
            Id = id;
            UnitType = unitType;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
