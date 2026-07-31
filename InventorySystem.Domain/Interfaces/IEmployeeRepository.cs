using InventorySystem.Domain.Entities;
namespace InventorySystem.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee? GetByRegistration(int registration);
        void Add(Employee employee);
    }
}
