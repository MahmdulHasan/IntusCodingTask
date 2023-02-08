using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Core.Entities
{
    public class Element : BaseEntity
    {
        public string Type { get; set; }

        public ICollection<OrderSubElement> OrderSubElements { get; set; }
    }
}
