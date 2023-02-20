using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Web.Framework.Contracts.V1.Order
{
    public class OrderWindowModel
    {
        public int Id { get; set; }
        public int WindowId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<OrderSubElementModel> SubElements { get; set; } = new List<OrderSubElementModel>();
    }
}
