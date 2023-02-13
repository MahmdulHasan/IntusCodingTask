using Intus.Core.Entities;
using Intus.Services.Orders;
using Intus.Web.Framework.Contracts.V1.Order;

namespace Intus.Web.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        #region Fields

        private readonly IOrderService _orderService;

        #endregion

        #region Ctor
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region Utilities

        OrderModel PrepareOrderModel (Order order)
        {
            return new OrderModel
            {
                Id = order.Id,
                Name = order.Name,
                State = order.State,
                Windows = order.OrderWindows.Select(orderWindow => new OrderWindowModel
                {
                    Id = orderWindow.Id,
                    WindowId = orderWindow.WindowId,
                    Name = orderWindow.Window.Name,
                    Quantity = orderWindow.Quantity,
                    SubElements = orderWindow.OrderSubElements.Select(subElement => new OrderSubElementModel
                    {
                        Id = subElement.Id,
                        ElementId = subElement.ElementId,
                        Type = subElement.Element.Type,
                        Width = subElement.Width,
                        Height = subElement.Height
                    }).ToList()
                }).ToList()

            };
        }

        Order PrepareOrder(OrderModel model)
        {
            return new Order
            {
                Name = model.Name,
                State = model.State,
                CreateDate = DateTime.UtcNow,
                OrderWindows = model.Windows.Select(window => new OrderWindow
                {
                    WindowId = window.WindowId,
                    Quantity = window.Quantity,
                    CreateDate = DateTime.UtcNow,
                    OrderSubElements = window.SubElements.Select(subElement => new OrderSubElement
                    {
                        ElementId = subElement.ElementId,
                        Width = subElement.Width,
                        Height = subElement.Height,
                        CreateDate = DateTime.UtcNow,
                    }).ToList()
                }).ToList()
            };
        }

        private static void MapOrderModelToOrder(OrderModel model, Order? order)
        {
            if(model is null )
                ArgumentNullException.ThrowIfNull(nameof(OrderModel));

            if (order is null)
                ArgumentNullException.ThrowIfNull(nameof(Order));

            order.Name = model.Name;
            order.State = model.State;
            order.UpdateDate = DateTime.UtcNow;

            var orderWindows = order.OrderWindows.ToList();

            foreach (var orderWindowModel in model.Windows)
            {
                var orderWindow = orderWindows.FirstOrDefault(w => w.Id == orderWindowModel.Id);

                if (orderWindow is null)
                    continue;

                orderWindow.Quantity = orderWindowModel.Quantity;
                orderWindow.UpdateDate = DateTime.UtcNow;
            }

            var subElements = order.OrderWindows.SelectMany(s => s.OrderSubElements).ToList();

            foreach (var subElementModel in model.Windows.SelectMany(s => s.SubElements))
            {
                var subelement = subElements.FirstOrDefault(w => w.Id == subElementModel.Id);

                if (subelement is null)
                    continue;

                subelement.Width = subElementModel.Width;
                subelement.Height = subElementModel.Height;
                subelement.UpdateDate = DateTime.UtcNow;
            }
        }
        private static void EnableDeleteFlag(Order? order)
        {
            order.IsDeleted = true;
            order.UpdateDate = DateTime.UtcNow;

            foreach (var orderWindow in order.OrderWindows)
            {
                orderWindow.IsDeleted = true;
                orderWindow.UpdateDate = DateTime.UtcNow;
            }

            foreach (var prderSubElement in order.OrderWindows.SelectMany(s => s.OrderSubElements))
            {
                prderSubElement.IsDeleted = true;
                prderSubElement.UpdateDate = DateTime.UtcNow;
            }
        }
        #endregion


        #region Methods

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllOrders();

            var orderList = orders.Select(order => new OrderModel
            {
                Id = order.Id,
                Name = order.Name,
                State = order.State
            }).ToList();

            return Ok(orderList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderService.GetOrderById(id);

            var orderModel = PrepareOrderModel(order);

            return Ok(orderModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderModel model)
        {

            var order = PrepareOrder(model);

            await _orderService.InsertOrder(order);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderModel model)
        {
            var order = await _orderService.GetOrderById(id);

            MapOrderModelToOrder(model, order);

            await _orderService.UpdateOrder(order);

            return Ok();
        }

     

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderById(id);

            EnableDeleteFlag(order);

            await _orderService.UpdateOrder(order);

            return Ok();
        }
        
        #endregion
    }
}
