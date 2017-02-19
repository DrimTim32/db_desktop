using System;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableWarehouseOrderDetails
    {
        public ShowableWarehouseOrderDetails() { }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; } 
        public ShowableWarehouseOrderDetails(getWarehouseOrderDetailsPretty_Result result)
        {
            Name = result.name;
            UnitPrice = result.purchase_price;
            Quantity = result.quantity; 
        }
    }
    public class ShowableWarehouseOrder
    {
        public ShowableWarehouseOrder() { }

        public int? Id { get; set; }
        public decimal Value { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string SupplierName { get; set; }
        public string LocationName { get; set; }

        public ShowableWarehouseOrder(Warehouse_orders_pretty result)
        {
            Id = result.id;
            EmployeeName = result.employee_name;
            EmployeeSurname = result.employee_surname;
            SupplierName = result.supplier_name;
            LocationName = result.location_name;
            OrderDate = result.order_date;
            RequiredDate = result.required_date;
            DeliveryDate = result.delivery_date;

        }
    }
}
