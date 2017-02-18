using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using DatabaseConnector;

    public class ShowableStoredProduct : ShowableProductBase
    { 
        public ShowableStoredProduct() { }

        public ShowableStoredProduct(productDetails_Result product)
        {
            Id = product.id;
            Name = product.name;
            CategoryName = product.category_name;
            UnitName = product.unit_name;
            TaxName = product.tax_name;
            TaxValue = product.tax_value;
        }

        public void LoadFromAnother(ShowableStoredProduct product)
        {
            Id = product.Id;
            Name = product.Name;
            CategoryName = product.CategoryName;
            UnitName = product.UnitName;
            TaxName = product.TaxName;
            TaxValue = product.TaxValue;
        } 
    }
}
