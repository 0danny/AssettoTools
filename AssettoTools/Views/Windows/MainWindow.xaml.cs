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
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Runtime.Versioning;

namespace AssettoTools.Views.Windows
{
    //This is to stop the call site errors, from below.
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : AdonisWindow
    {
        public MainWindowViewModel viewModel { get; set; }

        public Controller assettoTools { get; set; }

        public static MainWindow mainWindow { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel.Instance = new();
            viewModel = MainWindowViewModel.Instance;
            DataContext = this;

            assettoTools = new();

            MainWindowViewModel.Instance.controller = assettoTools;

            prepareEditor();

            mainWindow = this;

            Closing += viewModel.OnWindowClosing;
        }

        public void prepareEditor()
        {
            using (XmlTextReader reader = new XmlTextReader("Core\\External\\INIDefinition.xshd"))
            {
                viewModel.AvalonDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }
    }
}
