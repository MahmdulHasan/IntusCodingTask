using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Core.Entities
{
    public class Order : BaseEntity
    {
        public string Name { get; set; }
        public string State { get; set; }

        public ICollection<OrderWindow> OrderWindows { get; set; }
    }
}
