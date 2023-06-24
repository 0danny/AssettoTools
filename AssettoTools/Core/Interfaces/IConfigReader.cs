using AssettoTools.Core.Services.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoTools.Core.Interfaces
{
    public interface IConfigReader
    {
        ConfigModel ConfigModel { get; }

        public void readConfig();
        public void saveConfig();
    }
}
