using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using log4net;
using log4net.Repository.Hierarchy;

namespace BarProject.DesktopApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ILog logger = LogManager.GetLogger("App logger");
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            logger.Error("UnhandledException caught : " + ex.Message);
            logger.Error("UnhandledException StackTrace : " + ex.StackTrace); 
        }
    }
}
