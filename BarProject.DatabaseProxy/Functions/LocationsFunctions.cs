using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Functions
{
    using System.Web.Http.Routing;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class LocationsFunctions
    {
        public static List<ShowableLocation> GetLocations()
        {
            using (var db = new Entities())
            {
                return db.Locations.ToAnotherType<Location, ShowableLocation>().ToList();
            }
        }

        public static void RemoveLocation(int id)
        {
            using (var db = new Entities())
            {
                db.removeLocation(id);
            }
        }
        public static void AddLocation(ShowableLocation location)
        {
            using (var db = new Entities())
            {
                db.addLocation(location.Name, location.Address, location.City, location.PostalCode, location.Country,
                    location.Phone);
            }
        }
        public static void UpdateLocation(int id, ShowableLocation location)
        {
            using (var db = new Entities())
            {
                db.updateLocation(id, location.Name, location.Address, location.City, location.PostalCode, location.Country,
                    location.Phone);
            }
        }
    }
}
