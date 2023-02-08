using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Core.Entities
{
    public class OrderSubElement : BaseEntity
    {
        public int OrderWindowId { get; set; }
        public int ElementId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public OrderWindow OrderWindow { get; set; }
        public Element Element { get; set; }
    }
}
