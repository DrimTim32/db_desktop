using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using DatabaseConnector;

    public class ShowableLocation
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public ShowableLocation(Location location)
        {
            Id = location.id;
            Name = location.name;
            Address = location.address;
            City = location.city;
            PostalCode = location.postal_code;
            Country = location.country;
            Phone = location.phone;
        }

        public ShowableLocation()
        {
            Id = null;
        }
    }
}
