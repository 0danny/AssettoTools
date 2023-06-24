using AssettoTools.Core.Helper;
using AssettoTools.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Services.Config
{
    public class ConfigReader : IConfigReader
    {
        private ConfigModel _configModel = new();

        private string configPath = "config.json";

        public ConfigReader()
        {
            Logger.log("ConfigReader created.");
        }

        public ConfigModel ConfigModel
        {
            get { return _configModel; }
            private set { _configModel = value; }
        }

        public void readConfig()
        {
            if (!File.Exists(configPath))
            {
                Logger.log("Config does not exist, creating...");

                File.WriteAllText(configPath, JsonConvert.SerializeObject(_configModel));
            }
            else
            {
                try
                {
                    _configModel = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(configPath));

                    Logger.log("Config has successfully been read.");
                }
                catch (Exception ex)
                {
                    Logger.log($"There was an error reading config file: {ex.Message}");
                }
            }
        }

        public void saveConfig()
        {
            try
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(_configModel));

                Logger.log("Successfully saved config.");
            }
            catch (Exception ex)
            {
                Logger.log($"Config could not save: {ex.Message}");
            }
        }
    }
}
