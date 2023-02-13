using Intus.Core.Entities;
using Intus.Services.Orders;
using Intus.Web.Framework.Contracts.V1.Element;
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
                Windows = order.OrderWindows.Select(window => new OrderWindowModel
                {
                    Id = window.Id,
                    Name = window.Window.Name,
                    Quantity = window.Quantity,
                    SubElements = window.OrderSubElements.Select(subElement => new OrderSubElementModel
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
                OrderWindows = model.Windows.Select(window => new OrderWindow
                {
                    WindowId = window.Id,
                    Quantity = window.Quantity,
                    OrderSubElements = window.SubElements.Select(subElement => new OrderSubElement
                    {
                        ElementId = subElement.ElementId,
                        Width = subElement.Width,
                        Height = subElement.Height
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

            var subElements = order.OrderWindows.SelectMany(s => s.OrderSubElements).ToList();

            foreach (var subElementModel in model.Windows.SelectMany(s => s.SubElements))
            {
                var subelement = subElements.FirstOrDefault(w => w.Id == subElementModel.Id);

                if (subelement is null)
                    continue;

                subelement.Width = subElementModel.Width;
                subelement.Height = subElementModel.Height;
            }

            order.UpdateDate = DateTime.UtcNow;
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

            order.IsDeleted = true;

            await _orderService.UpdateOrder(order);

            return Ok();
        } 
        #endregion
    }
}
