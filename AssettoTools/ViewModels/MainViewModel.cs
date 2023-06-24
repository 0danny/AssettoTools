using ACDBackend;
using AssettoTools.Core;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Interfaces;
using AssettoTools.Core.Models;
using AssettoTools.Core.Services;
using AssettoTools.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AssettoTools.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        /* Services */
        public IConfigReader configService { get; set; }
        public IFileExplorer fileService { get; set; }
        public ICarExplorer carService { get; set; }

        /* Editor Tab */
        [ObservableProperty]
        public ObservableCollection<CarObject> carItems = new();

        [ObservableProperty]
        public ObservableCollection<FileObject> fileItems = new();

        [ObservableProperty]
        public string currentPath = "Current Path: ";

        [ObservableProperty]
        public CarObject currentCarObject = new();

        [ObservableProperty]
        public FileObject currentFileObject = new();

        [ObservableProperty]
        public string imagePath = "";

        public MainViewModel(IConfigReader _configService, IFileExplorer _fileService, ICarExplorer _carService)
        {
            configService = _configService;
            fileService = _fileService;
            carService = _carService;

            initWindow();
        }

        public async void initWindow()
        {
            Logger.log("AssettoTools created.");

            //Read the config into model.
            configService.readConfig();

            CarItems = await carService.populateList(configService.ConfigModel.ACDDirectory);

            CurrentPath = configService.ConfigModel.ACDDirectory;

            loadSyntaxHighlighting();
        }

        public void loadSyntaxHighlighting()
        {
            //Load syntax highlighting for avalonEdit.
            using (XmlTextReader reader = new XmlTextReader("Resources\\INIDefinition.xshd"))
            {
                AvalonDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }

        partial void OnCurrentFileObjectChanging(FileObject value)
        {
            if(value != null)
            {
                if (CurrentFileObject != null && !string.IsNullOrEmpty(CurrentFileObject.name))
                {
                    Logger.log($"Saving the last ACD: {CurrentFileObject.name}");
                    saveACDEntry(CurrentFileObject);
                }

                Logger.log($"ACD entry changed to: {value.name}.");

                MainWindow.windowInstance.setEditorContent(value.fileData);
            }
        }

        partial void OnCurrentCarObjectChanged(CarObject value)
        {
            Logger.log($"Car object changed to: {value.carName}");

            //Clear editor
            MainWindow.windowInstance.setEditorContent("");

            //Clear file items
            FileItems.Clear();

            ObservableCollection<FileObject> entries = fileService.getEntries(value.fullPath);

            if (entries != null)
            {
                //Ask the File Explorer to populate the INI list.

                //Setup Editor Tab
                FileItems = entries;

                ImagePath = value.previewImages[0];
            }
            else
            {
                Utilities.showMessageBox($"{value.carName} does not have any data.");

                Logger.log($"Skipping {value.carName} entries null.");
            }
        }

        public void saveACDEntry(FileObject entry)
        {
            if(entry == null || FileItems.Count <= 0)
            {
                return;
            }

            entry.fileData = MainWindow.windowInstance.getEditorContent();
        }

        

        [RelayCommand]
        public void saveACD()
        {
            Logger.log($"Saving ACD for: {CurrentCarObject.carName}");

            //Save the current file if we haven't already.
            saveACDEntry(CurrentFileObject);

            fileService.saveEntries(CurrentCarObject.fullPath, FileItems.ToList(), CurrentCarObject.hasACD);

            Utilities.showMessageBox("Successfully saved data.acd file.");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            configService.saveConfig();
        }

        [RelayCommand]
        public void setAssettoPath()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                Logger.log($"Got Assetto root path: {dialog.SelectedPath}");

                CurrentPath = $"Current Path: {dialog.SelectedPath}";

                configService.ConfigModel.ACDDirectory = dialog.SelectedPath;

                carService.populateList(dialog.SelectedPath);
            }
        }

        [ObservableProperty]
        public IHighlightingDefinition avalonDefinition = null;
    }
}
