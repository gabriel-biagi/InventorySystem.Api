using InventorySystem.Domain;

namespace InventorySystem.Infrastructure
{
    public class JsonEmployeeRepository : IEmployeeRepository
    {
        private string _filePath = "Data/employees.json";
        public Employee? GetByRegistration(int registration)
        {
            var lista = LoadEmployeesFromFile();
            return lista.FirstOrDefault(x => registration == x.Registration);
        }
        public void Add(Employee employee)
        {
            var lista = LoadEmployeesFromFile();
            lista.Add(employee);
            SaveEmployeesToFile(lista);
        }

        public void SaveEmployeesToFile(List<Employee> employees)
        {
            _ensureDirectory();
            string json = System.Text.Json.JsonSerializer.Serialize(employees);
            File.WriteAllText(_filePath, json);
        }

        public List<Employee> LoadEmployeesFromFile()
        {
            _ensureDirectory();
            string json = File.ReadAllText(_filePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
        }

        private void _ensureDirectory()
        {
            string directory = Path.GetDirectoryName(_filePath)!;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }
    }
}
