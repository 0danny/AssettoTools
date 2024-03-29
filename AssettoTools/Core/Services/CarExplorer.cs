﻿using AssettoTools.Core.Helper;
using AssettoTools.Core.Interfaces;
using AssettoTools.Core.Models;
using AssettoTools.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssettoTools.Core.Services
{
    public class CarExplorer : ICarExplorer
    {
        private readonly string carsPrefix = "\\content\\cars";
        private readonly string uiPrefix = "\\ui";
        private readonly string skinsPrefix = "\\skins";

        //Cache this for later, in case we need it again
        public string carsPath = "";

        public CarExplorer()
        {
            Logger.log("CarExplorer created.");
        }

        public async Task<ObservableCollection<CarObject>> populateList(string filePath)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    Logger.log("File path is empty, config not filled in?");

                    //Utilities.showMessageBox("Please set your Assetto Corsa path, located at the bottom portion of the window.");
                    return null;
                }

                carsPath = $"{filePath}{carsPrefix}";

                if (!precautionChecks(carsPath))
                {
                    Logger.log("Precaution checks returned false, returning.");
                    return null;
                }

                Logger.log($"Cars path is {carsPath}");

                ObservableCollection<CarObject> carObjects = new();

                foreach (string carFolder in Directory.GetDirectories(carsPath))
                {
                    CarObject carObject = parseFolder(carFolder);

                    if (carObject != null)
                    {
                        carObjects.Add(carObject);
                    }
                }

                return carObjects;
            });
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
            returnObject.hasACD = File.Exists($"{carFolder}\\data.acd");

            returnObject.previewImages = getImages(carFolder);

            string carName = getCarName(carFolder);

            returnObject.carName = carName == null ? returnObject.folderName : carName;

            Logger.log($"[{returnObject.carName}] adding, hasACD: {returnObject.hasACD}.");

            return returnObject;
        }

        public string[] getImages(string carFolder)
        {
            if (!Directory.Exists($"{carFolder}{skinsPrefix}"))
            {
                Logger.log($"No skins for: {carFolder} returning...");
                return null;
            }

            //Get the car preview images.
            string[] folders = Directory.GetDirectories($"{carFolder}{skinsPrefix}");

            for (int i = 0; i < folders.Length; i++)
            {
                string imageLoc = $"{folders[i]}\\preview.jpg";

                if (File.Exists(imageLoc))
                {
                    folders[i] = imageLoc;
                }
                else
                {
                    Logger.log($"[{carFolder}] image number [{i}][{imageLoc}] does not exist.");
                }
            }

            Logger.log($"Fetched {folders.Length} images for: {carFolder}");

            return folders;
        }

        public string getUIJSON(string carFolder)
        {
            if (!Directory.Exists($"{carFolder}{uiPrefix}"))
            {
                Logger.log($"UI folder does not exist: {carFolder}");

                return null;
            }

            string[] jsonFiles = Directory.GetFiles($"{carFolder}{uiPrefix}", "*ui_car.json", SearchOption.TopDirectoryOnly);

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

            if (uiCarsFile == null)
            {
                return null;
            }

            string jsonContents = File.ReadAllText(uiCarsFile);

            try
            {
                return ((dynamic)JObject.Parse(jsonContents)).name;
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
}
