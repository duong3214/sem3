namespace backendMuseum.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; } // Có thể null
        public bool IsAuction { get; set; } // Đơn hàng đấu giá
        public decimal? BidAmount { get; set; } // Số tiền thầu nếu là đấu giá
        public User? User { get; set; } // Có thể null

        // Quan hệ: Một đơn hàng có nhiều giao dịch
        public ICollection<Transaction>? Transactions { get; set; } // Có thể null
    }
}


