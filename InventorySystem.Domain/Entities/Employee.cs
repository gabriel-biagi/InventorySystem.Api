

namespace InventorySystem.Domain
{
    public class Employee
    {
        public string Name { get; private set; }
        public int Registration { get; private set; }
        public Role Role { get; private set; }

        public Employee(string name, int registration, Role role)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Employee name can't be null or empty");
            }
            
            Name = name;
            Registration = registration;
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
