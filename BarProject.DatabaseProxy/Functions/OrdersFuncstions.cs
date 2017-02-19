using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.DatabaseProxy.Functions
{
    public static class OrdersFuncstions
    { 
        public static void AddOrder(WritableOrder order)
        {
            using (var db = new Entities())
            {

               // db.addClientOrderDetail()
            }
        }
    }
}
