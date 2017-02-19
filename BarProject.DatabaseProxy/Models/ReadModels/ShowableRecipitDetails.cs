using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableRecipitDetails
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public ShowableRecipitDetails() { }

        public ShowableRecipitDetails(recipeDetails_Result result)
        {
            //TODO : !!!
        }
    }
}
