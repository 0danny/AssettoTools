using AssettoTools.Core.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Config
{
    public class ConfigReader
    {
        public Config configModel = new();

        private string configPath = "config.json";

        public ConfigReader()
        {
            Logger.log("ConfigReader created.");
        }

        public void readConfig()
        {
            if (!File.Exists(configPath))
            {
                Logger.log("Config does not exist, creating...");

                File.WriteAllText(configPath, JsonConvert.SerializeObject(configModel));
            }
            else
            {
                try
                {
                    configModel = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

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
                File.WriteAllText(configPath, JsonConvert.SerializeObject(configModel));

                Logger.log("Successfully saved config.");
            }
            catch (Exception ex)
            {
                Logger.log($"Config could not save: {ex.Message}");
            }
        }
    }
}
