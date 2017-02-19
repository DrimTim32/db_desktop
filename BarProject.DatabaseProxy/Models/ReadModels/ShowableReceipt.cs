using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableReceipt
    {
        public string Description { get; set; }
        public int? Id { get; set; }
        public ShowableReceipt() { }

        public ShowableReceipt(Recipe reci)
        {
            Description = reci.description;
            Id = reci.id;
        }
    }
}
