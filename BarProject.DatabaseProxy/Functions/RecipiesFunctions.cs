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
    public static class RecipiesFunctions
    {
        public static List<ShowableReceipt> GetRecipts()
        {
            using (var db = new Entities())
            {
                return db.Receipts.Select(x => x).ToAnotherType<Receipt, ShowableReceipt>().ToList();
            }
        }

        public static void AddRecipt(ShowableReceipt receipt)
        {
            using (var db = new Entities())
            {
                db.addReceipt(receipt.Description);
            }
        }
        public static void UpdateRecipt(int id, ShowableReceipt receipt)
        {
            using (var db = new Entities())
            {
                db.updateReceipt(id, receipt.Description);
            }
        }
        public static void RemoveRecipt(int id)
        {
            using (var db = new Entities())
            {
                db.removeReceipt(id);
            }
        }
    }
}
