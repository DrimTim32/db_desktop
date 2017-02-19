using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.DatabaseProxy.Functions
{
    public static class OrdersFuncstions
    {
        public static void AddOrder(string userName, WritableOrder order)
        {
            using (var db = new Entities())
            {
                var id = db.Users.First(x => x.username == userName).id;
                var localOrder = new Client_orders()
                {
                    employee_id = id,
                    order_time = DateTime.Now,
                    spot_id = 1,
                };
                db.Client_orders.Add(localOrder);
                db.SaveChanges();
                foreach (var o in order.Details)
                {
                    db.addClientOrderDetail(localOrder.id, o.ProductId, o.Quantity);
                }
            }
        }
    }
}
