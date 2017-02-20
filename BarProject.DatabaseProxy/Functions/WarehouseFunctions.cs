using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;
using BarProject.DatabaseProxy.Extensions;
using BarProject.DatabaseProxy.Models.ReadModels;

namespace BarProject.DatabaseProxy.Functions
{
    public static class WarehouseFunctions
    {
        public static List<ShowableWarehouseOrder> GetWarehouseOrders()
        {
            using (var db = new Entities())
            {
                return db.Warehouse_orders_pretty.ToAnotherType<Warehouse_orders_pretty, ShowableWarehouseOrder>().ToList();
            }
        }
        public static List<ShowableWarehouseOrderDetails> GetWarehouseOrdersDetails(int id)
        {
            using (var db = new Entities())
            {
                return db.getWarehouseOrderDetailsPretty(id).ToAnotherType<getWarehouseOrderDetailsPretty_Result, ShowableWarehouseOrderDetails>().ToList();
            }
        }
        public static void AddWarehouseOrder(string username, ShowableWarehouseOrder order)
        {
            using (var db = new Entities())
            {
                var user = db.Users.FirstOrDefault(x => x.username == username);
                var supplier = db.Suppliers.FirstOrDefault(x => x.name == order.SupplierName);
                var location = db.Locations.FirstOrDefault(x => x.name == order.LocationName);
                db.addWarehouseOrder(user?.id, supplier?.id, location?.id, order.OrderDate, order.RequiredDate, null);
            }
        }
        public static void AddWarehouseOrderDetails(int orderId, ShowableWarehouseOrderDetails details)
        {
            using (var db = new Entities())
            {
                var product = db.Products.FirstOrDefault(x => x.name == details.Name);
                db.addWarehouseOrderDetail(orderId, product.id, details.UnitPrice, details.Quantity);
            }
        }
        public static void UpdateWarehouseOrderDetails(int orderId, ShowableWarehouseOrderDetails details)
        {
            using (var db = new Entities())
            {
                var product = db.Products.FirstOrDefault(x => x.name == details.Name);
                db.updateWarehouseOrderDetail(orderId, product.id, details.UnitPrice, details.Quantity);
            }
        }

        public static List<ShowableWarehouseItem> GetWarehouse()
        {
            using (var db = new Entities())
            {
                return db.Warehouse_pretty.ToAnotherType<Warehouse_pretty, ShowableWarehouseItem>().ToList();
            }
        }

        public static object AddWarehouseItem(ShowableWarehouseItem item)
        {
            using (var db = new Entities())
            {
                var location = db.Locations.FirstOrDefault(x => x.name == item.LocationName);
                var product = db.Products.FirstOrDefault(x => x.name == item.ProductName);
                return db.addToWarehouse(location?.id, product?.id, item.Quantity);
            }
        }
        public static object UpdateWarehouseItem(ShowableWarehouseItem item)
        {
            using (var db = new Entities())
            {

                var location = db.Locations.FirstOrDefault(x => x.name == item.LocationName);
                var product = db.Products.FirstOrDefault(x => x.name == item.ProductName);
                return db.updateQuantityInWarehouse(location?.id, product?.id, item.Quantity);
            }
        }
        public static object RemoveWarehouseItem(ShowableWarehouseItem item)
        {
            using (var db = new Entities())
            {

                var location = db.Locations.FirstOrDefault(x => x.name == item.LocationName);
                var product = db.Products.FirstOrDefault(x => x.name == item.ProductName);
                return db.removeFromWarehouse(location?.id, product?.id);
            }
        }
    }
}
