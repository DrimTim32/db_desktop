using System;
using System.Collections.Generic;
using System.Linq;
using BarProject.DatabaseConnector;
using BarProject.DatabaseProxy.Extensions;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.DatabaseProxy.Functions
{
    public static class OrdersFuncstions
    {

        public static List<ShowableClientOrderDetails> GetDetails(int id)
        {
            using (var db = new Entities())
            {
                return db.Client_order_details_pretty.ToAnotherType<Client_order_details_pretty, ShowableClientOrderDetails>()
                    .ToList();
            }
        }
        public static int AddOrder(string userName, WritableOrder order)
        {
            using (var db = new Entities())
            {
                var id = db.Users.First(x => x.username == userName).id;
                var localOrder = new Client_orders()
                {
                    employee_id = id,
                    order_time = order.OrderTime,
                    spot_id = 1,
                };
                db.Client_orders.Add(localOrder);
                db.SaveChanges();
                foreach (var o in order.Details)
                {
                    db.addClientOrderDetail(localOrder.id, o.ProductId, o.Quantity);
                }
                return localOrder.id;
            }
        }

        public static void MarkPaid(int id)
        {
            using (var db = new Entities())
            {
                db.markPaid(id);
            }
        }

        public static IEnumerable<ShowableClientOrder> GetOrders()
        {
            using (var db = new Entities())
            {
                return db.Client_orders_pretty.ToAnotherType<Client_orders_pretty, ShowableClientOrder>().ToList();
            }
        }
    }
}
