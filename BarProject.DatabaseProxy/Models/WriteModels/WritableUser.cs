namespace BarProject.DatabaseProxy.Models.WriteModels
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    public class PureWritableUser
    {

        public string Password { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserPrivileges Permission { get; set; }
    }
    public class WritableUser : IDataErrorInfo
    {
        private string password;
        private string username;
        private string name;
        private string surname;
        private UserPrivileges permission;
        private string error;

        public PureWritableUser ToPure()
        {
            return new PureWritableUser()
            {
                Password = Password,
                Name = Name,
                Username = Username,
                Permission = UserPrivlidgesExtensions.GetValueFromDescription(Permission),
                Surname = Surname
            };
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public string Permission
        {
            get { return permission.ToReadable(); }
            set { permission = UserPrivlidgesExtensions.GetValueFromDescription(value); }
        }
        [JsonIgnore]
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        error = "Name is required.";
                        return "Required value";
                    }
                }
                if (columnName == "Password")
                {
                    return null;
                }
                if (columnName == "Permission")
                {
                    return null;
                }
                if (columnName == "Surname")
                {
                    if (string.IsNullOrEmpty(Surname))
                    {
                        error = "Surname is required.";
                        return "Required value";
                    }
                }
                if (columnName == "Username")
                {
                    if (string.IsNullOrEmpty(Username))
                    {
                        error = "Username is required.";
                        return "Required value";
                    }
                }
                error = null;
                return null;
            }
        }

        public string Error
        {
            get { return error; }
        }
    }
}