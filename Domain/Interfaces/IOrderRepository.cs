using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<int> GetNextDailySequenceNumberAsync(int restaurantId, DateTime orderDate);

        Task<Order?> GetByIdWithItemsAsync(int restaurantId, int orderId);
    }
}
