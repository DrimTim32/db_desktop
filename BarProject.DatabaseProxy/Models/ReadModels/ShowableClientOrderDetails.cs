using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableClientOrderDetails
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public Nullable<System.DateTime> OrderTime { get; set; }
        public Nullable<System.DateTime> PaymentTime { get; set; }
        public string SpotName { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ShowableClientOrderDetails() { }

        public ShowableClientOrderDetails(Client_order_details_pretty order)
        {
            ProductName = order.product_name;
            CategoryName = order.category_name;
            Price = order.price;
            Quantity = order.quantity;
            OrderTime = order.order_time;
            PaymentTime = order.payment_time;
            SpotName = order.spot_name;
            Address = order.address;
            LocationName = order.location_name;
            Name = order.user_name;
            Surname = order.surname;
        }
    }
}
