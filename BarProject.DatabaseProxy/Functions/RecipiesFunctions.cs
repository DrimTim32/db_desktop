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
                return db.Recipes.Select(x => x).ToAnotherType<Recipe, ShowableReceipt>().ToList();
            }
        }

        public static void AddRecipt(ShowableReceipt receipt)
        {
            using (var db = new Entities())
            {
                db.addRecipe(receipt.Description);
            }
        }
        public static void UpdateRecipt(int id, ShowableReceipt receipt)
        {
            using (var db = new Entities())
            {
                db.updateRecipe(id, receipt.Description);
            }
        }
        public static void RemoveRecipt(int id)
        {
            using (var db = new Entities())
            {
                db.removeRecipe(id);
            }
        }
        public static List<ShowableRecipitDetails> GetDetails(int id)
        {
            using (var db = new Entities())
            {
                return (from I in db.Ingredients
                        join R in db.Recipes on I.recipe_id equals R.id
                        join P in db.Products on I.ingredient_id equals P.id
                        where I.recipe_id == id
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
