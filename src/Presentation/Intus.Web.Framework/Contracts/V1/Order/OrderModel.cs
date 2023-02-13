using Intus.Web.Framework.Contracts.V1.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Web.Framework.Contracts.V1.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public List<OrderWindowModel> Windows { get; set; } = new List<OrderWindowModel>();
    }
}
