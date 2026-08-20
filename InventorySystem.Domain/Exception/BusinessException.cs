namespace InventorySystem.Domain.Exception;

public class BusinessException : DomainException
{
    public BusinessException(string message) : base(message) { }
}