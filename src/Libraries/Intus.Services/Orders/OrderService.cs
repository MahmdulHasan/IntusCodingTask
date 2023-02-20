using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> windowRepository)
        {
            _orderRepository = windowRepository;
        }
        public async Task<Order?> GetOrderById(int orderId)
        {
            if (orderId == 0)
                ArgumentNullException.ThrowIfNull(nameof(orderId));

            return await _orderRepository.Entity
                                         .Include(ow => ow.OrderWindows)
                                            .ThenInclude(se => se.OrderSubElements)
                                         .Include(ow => ow.OrderWindows)
                                            .ThenInclude(w => w.Window)
                                         .Include(ow => ow.OrderWindows)
                                            .ThenInclude(se => se.OrderSubElements)
                                            .ThenInclude(e => e.Element)
                                         .FirstOrDefaultAsync(o => o.Id == orderId);                                      
        }

        public async Task<IList<Order>> GetAllOrders(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false)
        {
            var query = _orderRepository.EntityWithNoTracking;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));

            if (!showDeleted)
                query = query.Where(v => !v.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task InsertOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(nameof(Order));

            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(nameof(Order));

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(nameof(Order));

            await _orderRepository.DeleteAsync(order);
        }
    }
}
