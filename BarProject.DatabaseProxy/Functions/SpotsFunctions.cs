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
    public static class SpotsFunctions
    {
        public static List<ShowableSpot> GetSpots()
        {
            using (var db = new Entities())
            {
                return db.Spots.ToAnotherType<Spot, ShowableSpot>().ToList();
            }
        }

        public static void RemoveSpot(int id)
        {
            using (var db = new Entities())
            {
                db.removeSpot(id);
            }
        }
        public static void AddSpot(ShowableSpot spot)
        {
            using (var db = new Entities())
            {
                var loc = db.Locations.FirstOrDefault(x => x.name == spot.LocationName);
                db.addSpot(spot.Name, loc?.id);
            }
        }
        public static void UpdateSpot(int id, ShowableSpot spot)
        {
            using (var db = new Entities())
            {
                var loc = db.Locations.FirstOrDefault(x => x.name == spot.LocationName);
                db.updateSpot(id, spot.Name);
            }
        }
    }
}
