﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace OLX_copy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            var window = new LoginWindow(); // или MainWindow
            app.Run(window);
        }
    }

}
