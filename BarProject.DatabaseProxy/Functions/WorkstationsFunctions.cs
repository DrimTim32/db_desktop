using System.Collections.Generic;
using System.Linq;
using BarProject.DatabaseProxy.Models;

namespace BarProject.DatabaseProxy.Functions
{
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class WorkstationsFunctions
    {
        public static List<ShowableWorkstation> GetWorkstations()
        {
            using (var db = new Entities())
            {
                return db.Workstations.ToAnotherType<Workstation, ShowableWorkstation>().ToList();
            }
        }
        public static void RemoveWorkstation(int id)
        {
            using (var db = new Entities())
            {
                db.removeWorkstation(id);
            }
        }
        public static void AddWorkstation(ShowableWorkstation workstation)
        {
            using (var db = new Entities())
            {
                var location = db.Locations.FirstOrDefault(x => x.name == workstation.LocationName);
                db.addWorkstation(workstation.Name, location?.id);
            }
        }
        public static void UpdateWorkstation(int id, ShowableWorkstation workstation)
        {
            using (var db = new Entities())
            {
                var location = db.Locations.FirstOrDefault(x => x.name == workstation.LocationName);
                db.updateWorkstation(id, location?.id, workstation.Name);
            }
        }
        public static List<ShowableWorkstationRights> GetWorkstationsRights()
        {
            using (var db = new Entities())
            {
                return
                    db.Workstations_with_rights_pretty
                        .ToAnotherType<Workstations_with_rights_pretty, ShowableWorkstationRights>().ToList();
            }
        }
        public static void AddWorkstationRights(int id, UserPrivileges privlidges)
        {
            using (var db = new Entities())
            {
                db.addWorkstationRights(id, (byte)privlidges);
            }
        }
        public static void UpdateWorkstationRights(int id, UserPrivileges workstation)
        {
            using (var db = new Entities())
            {
                // TODO : update workstation rights
            }
        }
        public static void RemoveWorkstationRights(int id, UserPrivileges workstation)
        {
            using (var db = new Entities())
            {
                db.removeWorkstationRights(id, (byte)workstation);
            }
        }

    }
}
