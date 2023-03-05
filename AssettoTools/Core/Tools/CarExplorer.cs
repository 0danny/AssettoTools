using AssettoTools.Core.Helper;
using AssettoTools.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssettoTools.Core.Tools
{
    public class CarExplorer
    {
        private readonly string carsPrefix = "\\content\\cars";
        private readonly string uiPrefix = "\\ui";

        //Cache this for later, in case we need it again
        public string carsPath = "";

        public CarExplorer()
        {
            Logger.log("CarExplorer created.");
        }

        public void populateList(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                Logger.log("File path is empty, config not filled in?");

                Utilities.showMessageBox("Please set your Assetto Corsa path, located at the bottom portion of the window.");

                return;
            }

            carsPath = $"{filePath}{carsPrefix}";

            if (!precautionChecks(carsPath))
            {
                Logger.log("Precaution checks returned false, returning.");
                return;
            }

            Logger.log($"Cars path is {carsPath}");

            //Create a temporary thread to not hold up the UI.
            Thread carExplorerThread = new Thread(() =>
            {
                foreach (string carFolder in Directory.GetDirectories(carsPath))
                {
                    CarObject carObject = parseFolder(carFolder);

                    App.Current.Dispatcher.Invoke(delegate
                    {
                        if(carObject != null)
                        {
                            MainWindowViewModel.Instance.CarItems.Add(carObject);
                        }
                    });
                }
            });

            carExplorerThread.Name = "Car Explorer Thread";
            carExplorerThread.IsBackground = true;
            carExplorerThread.Start();
        }

        public CarObject parseFolder(string carFolder)
        {
            //Ensure the folder actually has something in it.
            if (Utilities.isDirectoryEmpty(carFolder))
            {
                Logger.log($"Car: {carFolder} has nothing in it, skipping...");
                return null;
            }

            CarObject returnObject = new();

            returnObject.fullPath = carFolder;
            returnObject.folderName = carFolder.Replace($"{carsPath}\\", "");

            string carName = getCarName(carFolder);

            returnObject.carName = carName == null ? returnObject.folderName : carName;

            return returnObject;
        }

        public string getUIJSON(string carFolder)
        {
            if(!Directory.Exists($"{carFolder}{uiPrefix}"))
            {
                Logger.log($"UI folder does not exist: {carFolder}");

                return null;
            }

            string[] jsonFiles = Directory.GetFiles($"{carFolder}{uiPrefix}", "*ui_car.json",SearchOption.TopDirectoryOnly);

            if (jsonFiles.Length > 0)
            {
                //Grab the first file & hope for the best (:
                return jsonFiles[0];
            }
            else
            {
                Logger.log($"UI folder is empty: {carFolder}");

                return null;
            }
        }

        public string getCarName(string carFolder)
        {
            //Find the JSON file.

            string uiCarsFile = getUIJSON(carFolder);

            if(uiCarsFile == null)
            {
                return null;
            }

            string jsonContents = File.ReadAllText(uiCarsFile);

            try
            {
                dynamic parsedJSON = JObject.Parse(jsonContents);

                return parsedJSON.name;
            }
            catch (JsonReaderException ex)
            {
                Logger.log($"UI cars for: {carFolder}, could not be parsed: {ex.Message}");

                return null;
            }
        }

        public bool precautionChecks(string carsPath)
        {
            //Precautionary checks, ensure directory actually exists.

            return Directory.Exists(carsPath);
        }
    }

    public class CarObject
    {
        public string carName { get; set; }
        public string folderName { get; set; }
        public string fullPath { get; set; }
    }
}
