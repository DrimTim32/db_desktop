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
        public static List<ShowableRecipitDetails> GetDetails(int id)
        {
            using (var db = new Entities())
            {
                return (from I in db.Ingredients
                        join R in db.Receipts on I.receipt_id equals R.id
                        join P in db.Products on I.ingredient_id equals P.id
                        where I.receipt_id == id
                        select new ShowableRecipitDetails()
                        {
                            ProductName = P.name,
                            Quantity = I.quantity,
                        }
                ).ToList();
            }
        }
    }
}
