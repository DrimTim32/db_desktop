using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableSpot
    {
        public ShowableSpot() { }

        public int? Id { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public ShowableSpot(Spot spot)
        {
            Id = spot.id;
            LocationName = spot.Location.name;
            Name = spot.name;
        }
    }
}
