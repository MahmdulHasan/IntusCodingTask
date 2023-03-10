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
                Windows = order.OrderWindows.Where(w=> !w.IsDeleted).Select(orderWindow => new OrderWindowModel
                {
                    Id = orderWindow.Id,
                    WindowId = orderWindow.WindowId,
                    Name = orderWindow.Window.Name,
                    Quantity = orderWindow.Quantity,
                    SubElements = orderWindow.OrderSubElements.Where(w => !w.IsDeleted).Select(subElement => new OrderSubElementModel
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

        OrderWindow PrepareOrderWindow(OrderWindowModel model)
        {
            return new OrderWindow
            {
                WindowId = model.WindowId,
                Quantity = model.Quantity,
                CreateDate = DateTime.UtcNow,
                OrderSubElements = model.SubElements.Select(subElement => new OrderSubElement
                {
                    ElementId = subElement.ElementId,
                    Width = subElement.Width,
                    Height = subElement.Height,
                    CreateDate = DateTime.UtcNow,
                }).ToList()
            };
        }

        OrderSubElement PrepareOrderSubElement(OrderSubElementModel subElementModel)
        {
            return new OrderSubElement
            {
                ElementId = subElementModel.ElementId,
                Width = subElementModel.Width,
                Height = subElementModel.Height,
                CreateDate = DateTime.UtcNow
            };
        }

        Order PrepareOrder(OrderModel model)
        {
            return new Order
            {
                Name = model.Name,
                State = model.State,
                CreateDate = DateTime.UtcNow,
                OrderWindows = model.Windows.Select(window => PrepareOrderWindow(window)).ToList()
            };
        }

        private void MapOrderModelToOrder(OrderModel model, Order? order)
        {
            if (model is null)
                ArgumentNullException.ThrowIfNull(nameof(OrderModel));

            if (order is null)
                ArgumentNullException.ThrowIfNull(nameof(Order));

            order.Name = model.Name;
            order.State = model.State;
            order.UpdateDate = DateTime.UtcNow;

            var orderWindows = order.OrderWindows.ToList();

            MapWindowModelToWindowWithSubElements(model, order, orderWindows);

            RemoveDeletedWindowsAndSubElements(model, order, orderWindows);

        }

        private static void RemoveDeletedWindowsAndSubElements(OrderModel model, Order? order, List<OrderWindow> orderWindows)
        {
            var subElements = order.OrderWindows.SelectMany(s => s.OrderSubElements).ToList();

            var deletedWindows = orderWindows.Where(w => !model.Windows.Select(s => s.Id).Contains(w.Id) && w.Id != 0);

            var deletedSubElements = subElements.Where(w => !model.Windows.SelectMany(s => s.SubElements)
                                                                          .Select(s => s.Id).Contains(w.Id)
                                                                          && w.Id != 0);
            foreach (var orderWindow in deletedWindows)
            {
                orderWindow.IsDeleted = true;
                orderWindow.UpdateDate = DateTime.UtcNow;
            }

            foreach (var subElement in deletedSubElements.Where(w => w.Id != 0))
            {
                subElement.IsDeleted = true;
                subElement.UpdateDate = DateTime.UtcNow;
            }
        }

        private void MapWindowModelToWindowWithSubElements(OrderModel model, Order? order, List<OrderWindow> orderWindows)
        {
            foreach (var orderWindowModel in model.Windows)
            {
                var orderWindow = orderWindows.FirstOrDefault(w => w.Id == orderWindowModel.Id);

                if (orderWindow is null)
                {
                    var newOrderWindow = PrepareOrderWindow(orderWindowModel);
                    order.OrderWindows.Add(newOrderWindow);

                    continue;
                }

                orderWindow.Quantity = orderWindowModel.Quantity;
                orderWindow.UpdateDate = DateTime.UtcNow;

                var orderSubElements = model.Windows.Where(w => w.Id == orderWindow.Id)
                                               .SelectMany(s => s.SubElements);

                foreach (var subElementModel in orderSubElements)
                {
                    var subelement = orderWindow.OrderSubElements.FirstOrDefault(w => w.Id == subElementModel.Id);

                    if (subelement is null)
                    {
                        orderWindow.OrderSubElements.Add(PrepareOrderSubElement(subElementModel));

                        continue;
                    }

                    subelement.Width = subElementModel.Width;
                    subelement.Height = subElementModel.Height;
                    subelement.UpdateDate = DateTime.UtcNow;
                }
            }
        }

        private void EnableDeleteFlag(Order? order)
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
