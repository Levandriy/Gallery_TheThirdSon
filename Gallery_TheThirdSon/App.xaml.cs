using Gallery_TheThirdSon.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gallery_TheThirdSon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ImgsContainer Context = new ImgsContainer();
        public static Models.SaveModel SaveSelected { get; set; }
    }
}
