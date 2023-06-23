﻿using ACDBackend;
using AssettoTools.Core;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Models;
using AssettoTools.Core.Tools;
using AssettoTools.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AssettoTools.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        //I know this isn't ideal MVVM, but it's only a small project (I think ): )
        public static MainWindowViewModel Instance { get; set; }

        public Controller controller { get; set; }

        /* Editor Tab */
        [ObservableProperty]
        public ObservableCollection<CarObject> carItems = new();

        [ObservableProperty]
        public ObservableCollection<FileObject> fileItems = new();

        [ObservableProperty]
        public string currentPath = "Current Path: ";

        //Doing this manually, because the toolkit plays up
        [ObservableProperty]
        public CarObject currentCarObject = new();

        [ObservableProperty]
        public FileObject currentFileObject = new();

        [ObservableProperty]
        public string imagePath = "";

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

                setEditorContent(value.fileData);
            }
        }

        partial void OnCurrentCarObjectChanged(CarObject value)
        {
            Logger.log($"Car object changed to: {value.carName}");

            //Clear editor
            setEditorContent("");

            //Clear file items
            FileItems.Clear();

            ObservableCollection<FileObject> entries = controller.fileExplorer.getEntries(value.fullPath);

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

            entry.fileData = getEditorContent();
        }

        //AvalonEdit does not have support for MVVM natively unfortunately due to its architecture. So I will need to modify and get text this way.
        public string getEditorContent()
        {
            return MainWindow.mainWindow.avalonEditor.Text;
        }

        public void setEditorContent(string text)
        {
            MainWindow.mainWindow.avalonEditor.Text = text;
        }

        [RelayCommand]
        public void saveACD()
        {
            Logger.log($"Saving ACD for: {CurrentCarObject.carName}");

            //Save the current file if we haven't already.
            saveACDEntry(CurrentFileObject);

            controller.fileExplorer.saveEntries(CurrentCarObject.fullPath, FileItems.ToList(), CurrentCarObject.hasACD);

            Utilities.showMessageBox("Successfully saved data.acd file.");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            controller.configReader.saveConfig();
        }

        [RelayCommand]
        public void setAssettoPath()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                Logger.log($"Got Assetto root path: {dialog.SelectedPath}");

                CurrentPath = $"Current Path: {dialog.SelectedPath}";

                controller.configReader.configModel.ACDDirectory = dialog.SelectedPath;

                controller.carExplorer.populateList(dialog.SelectedPath);
            }
        }

        [ObservableProperty]
        public IHighlightingDefinition avalonDefinition = null;
    }
}
