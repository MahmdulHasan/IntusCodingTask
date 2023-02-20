using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Core.Entities
{
    public class Window : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<OrderWindow> OrderWindows { get; set; }
    }
}
