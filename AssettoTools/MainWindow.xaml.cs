using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ACDBackend;

namespace AssettoTools
{
    public partial class MainWindow : Window
    {
        private string filePath = "ks_audi_sport_quattro\\data.acd";

        private ACDWorker acdWorker;

        public MainWindow()
        {
            InitializeComponent();

            acdWorker = new ACDWorker(fixFilePath(filePath));
        }

        private string fixFilePath(string name)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string fullName = System.IO.Path.Combine(desktopPath, name);

            return fullName;
        }
    }
}
