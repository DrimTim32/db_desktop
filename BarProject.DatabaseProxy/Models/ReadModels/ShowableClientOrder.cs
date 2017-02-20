using System;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableClientOrder
    {
        public int? Id { get; set; }
        public Nullable<System.DateTime> OrderTime { get; set; }
        public Nullable<System.DateTime> PaymentTime { get; set; }
        public Nullable<decimal> Value { get; set; }
        public string SpotName { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ShowableClientOrder() { }

        public ShowableClientOrder(Client_orders_pretty orders)
        {
            OrderTime = orders.order_time;
            PaymentTime = orders.payment_time;
            Value = orders.value;
            SpotName = orders.spot_name;
            Name = orders.user_name;
            LocationName = orders.location_name;
            Surname = orders.surname;
            Id = orders.id;

        }
    }
}
