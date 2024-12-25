namespace backendMuseum.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Giá trị mặc định
        public decimal Amount { get; set; }
        public string? PaymentStatus { get; set; } // Có thể null
        public string? Status { get; set; }
        public Order? Order { get; set; } // Có thể null
    }
}
