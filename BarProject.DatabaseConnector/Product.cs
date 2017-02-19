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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductsUsages = new HashSet<ProductsUsage>();
            this.Warehouses = new HashSet<Warehouse>();
            this.Warehouse_order_details = new HashSet<Warehouse_order_details>();
        }
    
        public int id { get; set; }
        public Nullable<int> category_id { get; set; }
        public Nullable<int> unit_id { get; set; }
        public Nullable<int> tax_id { get; set; }
        public string name { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ProductsStored ProductsStored { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsUsage> ProductsUsages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse_order_details> Warehouse_order_details { get; set; }
        public virtual ProductsSold ProductsSold { get; set; }
        public virtual ProductsStored ProductsStored1 { get; set; }
    }
}
