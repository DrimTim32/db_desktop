//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BarProject.DatabaseConnector
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client_orders_pretty
    {
        public int id { get; set; }
        public Nullable<System.DateTime> order_time { get; set; }
        public Nullable<System.DateTime> payment_time { get; set; }
        public int spot_id { get; set; }
        public string spot_name { get; set; }
        public int location_id { get; set; }
        public string location_name { get; set; }
        public string address { get; set; }
        public int employee_id { get; set; }
        public string username { get; set; }
        public string user_name { get; set; }
        public string surname { get; set; }
    }
}