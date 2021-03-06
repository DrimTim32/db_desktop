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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Client_orders = new HashSet<Client_orders>();
            this.Warehouse_orders = new HashSet<Warehouse_orders>();
        }
    
        public int id { get; set; }
        public string username { get; set; }
        public byte[] password { get; set; }
        public string password_salt { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public Nullable<byte> permission { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client_orders> Client_orders { get; set; }
        public virtual EmployePermission EmployePermission { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse_orders> Warehouse_orders { get; set; }
    }
}
