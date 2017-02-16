using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using DatabaseConnector;

    public class ShowableSupplier
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }

        public ShowableSupplier()
        {
            Id = null;
        }

        public ShowableSupplier(Supplier supplier)
        {
            Id = supplier.id;
            Name = supplier.name;
            Address = supplier.address;
            City = supplier.city;
            PostalCode = supplier.postal_code;
            Country = supplier.country;
            ContactName = supplier.contact_name;
            Phone = supplier.phone;
            Fax = supplier.fax;
            Website = supplier.website;
        }
    }
}
