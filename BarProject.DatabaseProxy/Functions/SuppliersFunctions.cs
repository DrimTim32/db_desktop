using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Functions
{
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class SuppliersFunctions
    {
        public static List<ShowableSupplier> GetSuppliers()
        {
            using (var db = new Entities())
            {
                return db.Suppliers.ToAnotherType<Supplier, ShowableSupplier>().ToList();
            }
        }
        public static void AddSupplier(ShowableSupplier supplier)
        {
            using (var db = new Entities())
            {
                db.addSupplier(supplier.Name, supplier.Address, supplier.City, supplier.PostalCode,
                    supplier.Country, supplier.ContactName, supplier.Phone, supplier.Fax, supplier.Website);
            }
        }
        public static void RemoveSuppliers(int id)
        {
            using (var db = new Entities())
            {
                db.removeSupplier(id);
            }
        }
        public static void UpdateSuppliers(int id, ShowableSupplier supplier)
        {
            using (var db = new Entities())
            {
                db.updateSupplier(id, supplier.Name, supplier.Address, supplier.City, supplier.PostalCode,
                    supplier.Country, supplier.ContactName, supplier.Phone, supplier.Fax, supplier.Website);
            }
        }
    }
}
