namespace BarProject.DatabaseProxy.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class TaxesFunctions
    {
        public static IEnumerable<ShowableTax> GetAllTaxes()
        {
            using (var db = new Entities())
            {
                return db.Taxes.Select(x => x).ToAnotherType<Tax, ShowableTax>().ToList();
            }
        }
        public static void AddTax(ShowableTax tax)
        {
            using (var db = new Entities())
            {
                db.addTax(tax.TaxName, tax.TaxValue);
            }

        }

        public static void RemoveTax(int id)
        {
            using (var db = new Entities())
            {
                db.removeTax(id);
            }
        }
    }
}
