namespace InventorySystem.Domain
{
    public class InventoryItem
    {
        public Product Product { get; private set; }
        public Location Location { get; private set; }
        public decimal Quantity { get; private set; }

        public InventoryItem(Product product, Location location)
        {
            Product = product;
            Location = location;
            Quantity = 0;
        }
        public InventoryItem(Product product, Location location, decimal quantity)
        {
            Product = product;
            Location = location;
            Quantity = quantity;
        }


        public void AddQuantity(decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            Quantity += quantity;
        }

        public void RemoveQuantity(decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            if (quantity > Quantity)
            {
                throw new ArgumentException("Insufficient stock quantity.");
            }

            Quantity -= quantity;
            
        }
    }
}
