using AssettoTools.Core.Config;
using AssettoTools.Core.Helper;
using AssettoTools.Core.Tools;
using AssettoTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core
{
    public class Controller
    {
        public CarExplorer carExplorer = new();
        public FileExplorer fileExplorer = new();

        public ConfigReader configReader = new();

        public Controller() 
        {
            Logger.log("AssettoTools created.");

            //Read the config into model.
            configReader.readConfig();

            //Populate cars within list.
            carExplorer.populateList(configReader.configModel.ACDDirectory);

            //Set the current path
            MainWindowViewModel.Instance.CurrentPath = configReader.configModel.ACDDirectory;
        }
    }
}
