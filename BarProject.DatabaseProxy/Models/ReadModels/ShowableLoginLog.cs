using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableLoginLog
    {
        public string Username { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public ShowableLoginLog() { }

        public ShowableLoginLog(LoginLog log)
        {
            Username = log.username;
            LoginTime = log.login_time;
        }
    }
}
