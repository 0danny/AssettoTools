using AssettoTools.Core.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AssettoTools.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        //I know this isn't ideal MVVM, but it's only a small project.
        public static MainWindowViewModel Instance { get; set; }

        /* Home Tab */

        [ObservableProperty]
        public ObservableCollection<CarObject> carItems = new();


        /* Settings Tab */

        //Hard-coding mine for now, will be able to set in "Settings" menu.
        [ObservableProperty]
        public string assettoCorsaPath = "E:\\SteamLibrary\\steamapps\\common\\assettocorsa";
    }
}
