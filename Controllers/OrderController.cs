using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backendMuseum.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace backendMuseum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("history/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetPurchaseHistory(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Transactions!)
                    .ThenInclude(t => t.Artwork)
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No purchase history found." });
            }

            var result = orders.Select(o => new
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Transactions = o.Transactions?.Select(t => new
                {
                    TransactionId = t.Id,
                    ArtworkId = t.ArtworkId,
                    ArtworkTitle = t.Artwork?.Title ?? "Unknown",
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status ?? "Unknown"
                })
            });

            return Ok(result);
        }
    }
}
