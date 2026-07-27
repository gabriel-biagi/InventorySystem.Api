

namespace InventorySystem.Domain
{
    public abstract class Employee
    {
       public string Name { get; private set; }
        public int Registration { get; private set; }
        public Role Role { get; private set; }

        public Employee(string name, int registration, Role role)
        {
            Name = name;
            Registration = registration;
            Role = role;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
