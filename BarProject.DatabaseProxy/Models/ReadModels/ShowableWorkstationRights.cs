using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableWorkstationRights
    {
        public int workstation_id { get; set; }
        public string workstation_name { get; set; }
        public byte employe_permissions { get; set; }
        public int id { get; set; }
        public string location_name { get; set; }
        public string city { get; set; }
        public ShowableWorkstationRights() { }

        public ShowableWorkstationRights(Workstations_with_rights_pretty rights)
        {

        }
    }
}
