namespace BarProject.DatabaseProxy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    [Flags]
    public enum UserPrivileges : byte
    {
        [Description("No user")]
        NoUser = 0,
        [Description("Waiter")]
        Waitress = 1,                       // 00000001
        [Description("Chef")]
        Cook = 2,                           // 00000010
        [Description("Warehouse Administrator")]
        WarehouseAdministrator = 16,        // 00010000
        [Description("Owner")]
        Owner = 127,                        // 01111111
        [Description("Admin")]
        Admin = 255,                     // 11111111

        HighPrivlidges = 240,               // 11110000 

    }
    public static class UserPrivlidgesExtensions
    {
        public static readonly Dictionary<UserPrivileges, string> PrivligesNames = new Dictionary<UserPrivileges, string>()
        {
            {UserPrivileges.NoUser,  UserPrivileges.NoUser.GetEnumDescription()},
            {UserPrivileges.Waitress,UserPrivileges.Waitress.GetEnumDescription()},
            {UserPrivileges.Cook,UserPrivileges.Cook.GetEnumDescription()},
            {UserPrivileges.WarehouseAdministrator,UserPrivileges.WarehouseAdministrator.GetEnumDescription()},
            {UserPrivileges.Owner,UserPrivileges.Owner.GetEnumDescription()},
            {UserPrivileges.Admin,UserPrivileges.Admin.GetEnumDescription()},

        };

        public static readonly List<string> GetManagable = new List<string>()
        {
            PrivligesNames[UserPrivileges.Waitress], PrivligesNames[UserPrivileges.Cook],
            PrivligesNames[UserPrivileges.WarehouseAdministrator], PrivligesNames[ UserPrivileges.NoUser]
        };
        public static string ToReadable(this UserPrivileges data)
        {
            return PrivligesNames.ContainsKey(data) ? PrivligesNames[data] : "unknown";
        }
        public static UserPrivileges ToUserPrivledges(this byte input)
        {
            if (Enum.IsDefined(typeof(UserPrivileges), input))
            {
                return (UserPrivileges)input;
            }
            return UserPrivileges.NoUser;
        }
        public static string GetEnumDescription(this UserPrivileges value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        public static UserPrivileges GetValueFromDescription(string description)
        {
            var type = typeof(UserPrivileges);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (UserPrivileges)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (UserPrivileges)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.");
        }
    }

}
