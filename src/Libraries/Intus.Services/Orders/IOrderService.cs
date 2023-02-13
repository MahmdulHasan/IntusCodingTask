using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Services.Orders
{
    public interface IOrderService
    {
        Task<Order?> GetOrderById(int windowId);
        Task<IList<Order>> GetAllOrders(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false);
        Task DeleteOrder(Order order);
        Task InsertOrder(Order order);
        Task UpdateOrder(Order order);
    }
}
