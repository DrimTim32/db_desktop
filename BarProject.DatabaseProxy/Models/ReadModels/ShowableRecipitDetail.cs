using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableRecipitDetail
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public ShowableRecipitDetail() { }

        public ShowableRecipitDetail(recipeDetails_Result result)
        {
            Quantity = result.quantity;
            ProductId = result.ingredient_id;
            ProductName = result.ingredient_name; 
        }
    }
}
