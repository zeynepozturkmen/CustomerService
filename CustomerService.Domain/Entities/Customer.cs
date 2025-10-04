

namespace CustomerService.Domain.Entities
{
    public enum CustomerType { Individual, Corporate }
    public enum CustomerStatus { Active, Passive, Blocked }
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TaxNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CustomerType Type { get; set; }
        public CustomerStatus Status { get; set; } = CustomerStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
