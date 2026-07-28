namespace InventorySystem.Domain
{
    public class Location
    {
        public int Column { get; private set; }
        public int Shelf { get; private set; }
        public int Item { get; private set; }

        public Location(int column, int shelf, int item)
        {
            Column = column;
            Shelf = shelf;
            Item = item;
        }
    }
}