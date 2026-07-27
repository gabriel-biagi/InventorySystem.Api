using InventorySystem.Domain;

namespace InventorySystem.Infrastructure
{
    public class JsonEmployeeRepository : Domain.IEmployeeRepository
    {
        public Employee? GetByRegistration(int registration)
        {
            throw new NotImplementedException();
        }
        public void Add(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
