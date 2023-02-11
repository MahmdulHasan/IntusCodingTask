using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Web.Framework.Contracts.V1.Order
{
    public class SubElementModel
    {
        public int Id { get; set; }
        public int ElementId { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
