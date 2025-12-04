using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(PosRestaurantContext context) : base(context)
        {
        }
        public async Task<int> GetNextDailySequenceNumberAsync(int restaurantId, DateTime orderDate)
        {
            var maxNumber = await _context.Orders
                .Where(o => o.RestaurantId == restaurantId && o.OrderDate.Date == orderDate.Date)
                .MaxAsync(o => (int?)o.DailySequenceNumber) ?? 0;

            return maxNumber + 1;
        }
        public async Task<Order?> GetByIdWithItemsAsync(int restaurantId, int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(o => o.RestaurantId == restaurantId && o.Id == orderId);
        }
    }
}
