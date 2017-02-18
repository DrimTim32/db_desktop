using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.WriteModels
{
    public class WritableProduct
    {
        public WritableProduct()
        {
            
        }
        public string Name { get; set; }
        public bool IsSold { get; set; }
        public bool IsStored { get; set; }
        public string UnitName { get; set; }
        public string TaxName { get; set; }
        public string CategoryName { get; set; }
        public string RecipitDescription { get; set; }

    }
}
