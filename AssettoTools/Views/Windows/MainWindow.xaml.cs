using AdonisUI.Controls;
using AssettoTools.ViewModels;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Diagnostics;

namespace AssettoTools.Views.Windows
{
    public partial class MainWindow : AdonisWindow
    {
        public static MainWindow windowInstance;

        public MainWindow(MainViewModel _viewModel)
        {
            InitializeComponent();

            DataContext = _viewModel;

            windowInstance = this;

            // Subscribe to Closing event
            Closing += _viewModel.OnWindowClosing;
        }

        public string getEditorContent()
        {
            return avalonEditor.Text;
        }

        public void setEditorContent(string text)
        {
            avalonEditor.Text = text;
        }
    }
}
