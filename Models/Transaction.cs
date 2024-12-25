namespace backendMuseum.Models
{
    public class Transaction
    {
        public int Id { get; set; } // ID giao dịch
        public int ArtworkId { get; set; } // ID tác phẩm nghệ thuật
        public Artwork? Artwork { get; set; } // Tác phẩm nghệ thuật liên quan
        public string? UserId { get; set; } // ID người dùng (kiểu string)
        public User? User { get; set; } // Người dùng liên quan
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Ngày giao dịch
        public decimal Amount { get; set; } // Số tiền giao dịch
        public string? Status { get; set; } // Trạng thái giao dịch
        public int OrderId { get; set; } // ID đơn hàng
        public Order? Order { get; set; } // Đơn hàng liên quan
    }
}
