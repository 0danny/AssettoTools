using AssettoTools.Core.Helper;
using AssettoTools.Core.Tools;
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

        public Controller() 
        {
            Logger.log("AssettoTools created.");

            //Populate cars within list
            carExplorer.populateList();
        }
    }
}
