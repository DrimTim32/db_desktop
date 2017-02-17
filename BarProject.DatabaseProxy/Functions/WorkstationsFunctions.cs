using System.Collections.Generic;
using System.Linq;

namespace BarProject.DatabaseProxy.Functions
{
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class WorkstationsFunctions
    {
        public static List<ShowableWorkstation> GetWorkstations()
        {
            using (var db = new BarProjectEntities())
            {
                return db.Workstations.ToAnotherType<Workstation, ShowableWorkstation>().ToList();
            }
        }
        public static void RemoveWorkstation(int id)
        {
            using (var db = new BarProjectEntities())
            {
                db.removeWorkstation(id);
            }
        }
        public static void AddWorkstation(ShowableWorkstation workstation)
        {
            using (var db = new BarProjectEntities())
            {
                db.addWorkstation(workstation.Name);
            }
        }
        public static void UpdateWorkstation(int id, ShowableWorkstation workstation)
        {
            using (var db = new BarProjectEntities())
            {
                db.updateWorkstation(id, workstation.Name);
                
            }
        }
    }
}
