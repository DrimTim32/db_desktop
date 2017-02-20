using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableWarehouseItem
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public short Quantity { get; set; }
        public string LocationName { get; set; }
        public ShowableWarehouseItem() { }

        public ShowableWarehouseItem(Warehouse_pretty warehouse)
        {
            ProductName = warehouse.name;
            Id = 0;
            Quantity = warehouse.quantity;
            LocationName = warehouse.location_name;
        }
    }
}
