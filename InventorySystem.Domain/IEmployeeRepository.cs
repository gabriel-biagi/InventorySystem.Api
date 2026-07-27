namespace InventorySystem.Domain
{
    public interface IEmployeeRepository
    {
        Employee? GetByRegistration(int registration);
        void Add(Employee employee);
    }
}
