using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Core.Entities
{
    public class OrderWindow : BaseEntity
    {
        public int OrderId { get; set; }
        public int WindowId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Window Window { get; set; }
        public ICollection<OrderSubElement> OrderSubElements { get; set; }
    }
}
