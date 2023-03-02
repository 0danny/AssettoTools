using ACDBackend;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Tools;
using AssettoTools.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AssettoTools.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        //I know this isn't ideal MVVM, but it's only a small project.
        public static MainWindowViewModel Instance { get; set; }

        private ACDWorker acdWorker = new();

        /* Home Tab */

        [ObservableProperty]
        public ObservableCollection<CarObject> carItems = new();

        [ObservableProperty]
        public ObservableCollection<ACDEntry> fileItems = new();

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

            FileItems = new ObservableCollection<ACDEntry>(acdWorker.getEntries(carObject.fullPath));
        }

        public void fileObject_Changed(ACDEntry previousEntry, ACDEntry newEntry)
        {
            if(newEntry == null)
            {
                return;
            }

            Logger.log($"ACD entry for {CarObject_Selected.carName}, changed to: {newEntry.name}.");

            if(previousEntry != null && !string.IsNullOrEmpty(previousEntry.name))
            {
                FileItems.Where(elem => elem == previousEntry).First().fileData = getEditorContent();
            }

            setEditorContent(newEntry.fileData);
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

        [ObservableProperty]
        public IHighlightingDefinition avalonDefinition = null;

        /* Settings Tab */

        //Hard-coding mine for now, will be able to set in "Settings" menu.
        [ObservableProperty]
        public string assettoCorsaPath = "E:\\SteamLibrary\\steamapps\\common\\assettocorsa";
    }
}
