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
    
    public partial class soldProductDetails_Result
    {
        public int product_id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int unit_id { get; set; }
        public string unit_name { get; set; }
        public int tax_id { get; set; }
        public string tax_name { get; set; }
        public double tax_value { get; set; }
        public Nullable<int> recipe_id { get; set; }
        public System.DateTime period_start { get; set; }
        public decimal price { get; set; }
    }
}
