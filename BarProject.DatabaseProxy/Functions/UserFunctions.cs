﻿namespace BarProject.DatabaseProxy.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Diagnostics;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models;
    using Models.ExceptionHandlers;
    using Models.ReadModels;
    using Models.WriteModels;

    public static class UserFunctions
    {
        public static void LogUserLogin(string username)
        {
            using (var db = new Entities())
            {
                db.logLogin(username, DateTime.Now);
            }
        }
        public static UserPrivileges GetPrivileges(string username, string password)
        {
            var toReturn = UserPrivileges.NoUser;
            using (var db = new Entities())
            {
                var parameter = new ObjectParameter("tmp_credentials", typeof(short));
                db.checkCredentials(username, password, parameter);
                if (parameter.Value != null)
                {
                    return (BitConverter.GetBytes((short)parameter.Value)[0]).ToUserPrivledges();
                }
            }
            return toReturn;
        }

        [DbFunction("BarProjectEntities.Store", "getUserPermission")]
        public static byte? GetUserPermission(string username)
        {
            throw new NotSupportedException();
        }

        public static UserPrivileges GetPrivileges(string username)
        {
            var toReturn = UserPrivileges.NoUser;
            using (var db = new Entities())
            {
                var q = db.Users.First(x => x.username == username).EmployePermission.value;
                if (q.HasValue)
                {
                    return q.Value.ToUserPrivledges();
                }

            }
            return toReturn;
        }

        public static IEnumerable<ShowableUser> GetAllUsers()
        {
            using (var db = new Entities())
            {
                return db.Users.Select(x => x).ToAnotherType<User, ShowableUser>().ToList();//.ToShowableUsers().ToList();
            }
        }

        public static void AddUser(PureWritableUser userData)
        {
            using (var db = new Entities())
            {
                var permissionId = db.EmployePermissions.FirstOrDefault(x => x.value == (byte)userData.Permission);
                if (permissionId == null)
                    throw new ArgumentException("No such permission exists");
                db.addUser(userData.Password, userData.Username, userData.Name, userData.Surname, permissionId.id);
            }
        }

        public static User GetUserFullData(string username)
        {
            using (var db = new Entities())
            {
                return db.Users.FirstOrDefault(x => x.username == username);
            }
        }

        public static bool UserCanLoginOnLocation(UserPrivileges privileges, int locationId)
        {
            using (var db = new Entities())
            {
                var rights = db.Workstations_with_rights_pretty.FirstOrDefault(x => x.id == locationId);
                var id = rights.id;
                var v = db.EmployePermissions.Where(x => x.id == id);
                var rightValue = v.FirstOrDefault().value.Value;
                return rightValue <= (byte)privileges;
            }
        }

        public static void RemoveUser(string username)
        {
            using (var db = new Entities())
            {
                var usr = db.Users.FirstOrDefault(x => x.username == username);
                if (usr == null)
                    return;
                db.Users.Remove(usr);
            }
        }
        public static IEnumerable<LoginLog> GetLogs()
        {
            using (var db = new Entities())
            {
                return db.LoginLogs.Select(x => x).ToList();
            }
        }

    }
}
