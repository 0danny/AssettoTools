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
using AdonisUI.Controls;
using AssettoTools.Core;
using AssettoTools.ViewModels;

namespace AssettoTools.Views.Windows
{
    public partial class MainWindow : AdonisWindow
    {
        public MainWindowViewModel viewModel { get; set; }

        public Controller assettoTools { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel.Instance = new();
            viewModel = MainWindowViewModel.Instance;
            DataContext = this;

            assettoTools = new();
        }
    }
}
