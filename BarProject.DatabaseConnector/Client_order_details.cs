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
    
    public partial class Client_order_details
    {
        public int client_order_id { get; set; }
        public int products_sold_id { get; set; }
        public short quantity { get; set; }
    
        public virtual Client_orders Client_orders { get; set; }
        public virtual ProductsSold ProductsSold { get; set; }
    }
}
