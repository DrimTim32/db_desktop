using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using DatabaseConnector;

    public class ShowableWorkstation
    {
        public int? Id { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public ShowableWorkstation()
        {
            Id = null;
        }

        public ShowableWorkstation(Workstation workstation)
        {
            Id = workstation.id;
            Name = workstation.name;
            LocationName = workstation.Location.name;
        }
    }
}
