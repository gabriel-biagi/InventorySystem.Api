

namespace InventorySystem.Domain.Entities
{
    public class Employee
    {
        public int RegistrationNumber { get; set; }
        public string Name { get; private set; }
        public Role Role { get; private set; }

        public Employee(string name, Role role)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Employee name can't be null or empty");
            }
            
            Name = name;
            Role = role;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid employee name");
            }
            
            Name = name;
        }
    }
}
