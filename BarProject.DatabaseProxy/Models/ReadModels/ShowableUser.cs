namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DatabaseConnector;

    public class ShowableUser
    {
        public ShowableUser() { }
        public ShowableUser(User user)
        {
            Name = user.name;
            Permission = (user.EmployePermission.value ?? 0).ToUserPrivledges().ToReadable();
            Username = user.username;
            Surname = user.surname;
        }
        [Key, Column("Username", Order = 3)]
        public string Username { get; set; }
        [Key, Column("Name", Order = 0)]
        public string Name { get; set; }
        [Key, Column("Surname", Order = 1)]
        public string Surname { get; set; }
        [Key, Column("Position", Order = 2)]
        public string Permission { get; set; }
    }

}
