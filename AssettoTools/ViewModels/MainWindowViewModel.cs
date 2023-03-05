using ACDBackend;
using AssettoTools.Core;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Tools;
using AssettoTools.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        //I know this isn't ideal MVVM, but it's only a small project.
        public static MainWindowViewModel Instance { get; set; }

        private ACDWorker acdWorker = new();

        public Controller controller { get; set; }

        /* Home Tab */

        [ObservableProperty]
        public ObservableCollection<CarObject> carItems = new();

        [ObservableProperty]
        public ObservableCollection<ACDEntry> fileItems = new();

        [ObservableProperty]
        public string currentPath = "Current Path: ";

        //Doing this manually, because the toolkit plays up
        public CarObject carObject_Selected = new();

        public CarObject CarObject_Selected
        {
            get
            {
                return carObject_Selected;
            }
            set
            {
                carObject_Selected = value;

                carObject_Changed(value);

                OnPropertyChanged("CarObject_Selected");
            }
        }

        public ACDEntry fileObject_Selected = new();

        public ACDEntry FileObject_Selected
        {
            get
            {
                return fileObject_Selected;
            }
            set
            {
                fileObject_Changed(fileObject_Selected, value);

                fileObject_Selected = value;

                OnPropertyChanged("FileObject_Selected");
            }
        }

        public void carObject_Changed(CarObject carObject)
        {
            Logger.log($"Car object changed to: {carObject.carName}");

            //Clear editor
            setEditorContent("");

            //Clear file items
            FileItems.Clear();

            List<ACDEntry> entries = acdWorker.getEntries(carObject.fullPath);

            if (entries != null)
            {
                FileItems = new ObservableCollection<ACDEntry>(entries);
            }
            else
            {
                Logger.log($"Skipping {carObject.carName} entries null.");
            }
        }

        public void fileObject_Changed(ACDEntry previousEntry, ACDEntry newEntry)
        {
            if (newEntry == null)
            {
                return;
            }

            Logger.log($"ACD entry for {CarObject_Selected.carName}, changed to: {newEntry.name}.");

            if (previousEntry != null && !string.IsNullOrEmpty(previousEntry.name))
            {
                saveACDEntry(previousEntry);
            }

            setEditorContent(newEntry.fileData);
        }

        public void saveACDEntry(ACDEntry entry)
        {
            FileItems.Where(elem => elem == entry).First().fileData = getEditorContent();
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
            Logger.log($"Saving ACD for: {CarObject_Selected.carName}");

            //Save the current file if we haven't already.
            saveACDEntry(FileObject_Selected);

            acdWorker.saveEntries(CarObject_Selected.fullPath, FileItems.ToList());

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
