using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Domain.Entities
{
    public class Location
    {
        public int Column { get; private set; }
        public int Shelf { get; private set; }
        public int Item { get; private set; }

        public Location(int column, int shelf, int item)
        {
            if (column < 1 || shelf < 1 || item < 1)
            {
                throw new ArgumentException("Invalid location");
            }
            
            Column = column;
            Shelf = shelf;
            Item = item;
        }
    }
}