using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Web.Framework.Contracts.V1.Order
{
    public class WindowModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<SubElementModel> SubElements { get; set; } = new List<SubElementModel>();
    }
}
