namespace BarProject.DatabaseProxy.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class UnitsFunctions
    {
        public static List<ShowableUnit> GetAllUnits()
        {
            using (var db = new Entities())
            {
                return db.Units.Select(x => x).ToAnotherType<Unit, ShowableUnit>().ToList();
            }
        }
        public static List<string> GetTypes()
        {
            using (var db = new Entities())
            {
                return db.UnitTypes.Select(x => x.type_name).ToList();
            }
        }

        public static void AddUnit(ShowableUnit unit)
        {
            using (var db = new Entities())
            {
                var q = db.UnitTypes.FirstOrDefault(x => x.type_name == unit.Type);
                if (q != null)
                {
                    db.addUnit(unit.Name, unit.Factor, q.id);
                }
                else
                {
                    throw new ArgumentException("Problem with type name!");
                }
            }

        }

        public static void RemoveUnit(int id)
        {
            using (var db = new Entities())
            {
                db.removeUnit(id);
            }
        }
    }
}
