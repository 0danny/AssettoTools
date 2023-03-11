using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AssettoTools.Core.Helper
{
    public class Utilities
    {
        public static string fixFilePath(string name)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string fullName = System.IO.Path.Combine(desktopPath, name);

            return fullName;
        }

        public static string getACDFolderName(string fileName)
        {
            var name = Path.GetFileName(fileName) ?? "";
            return name.StartsWith("data", StringComparison.OrdinalIgnoreCase) ? Path.GetFileName(Path.GetDirectoryName(fileName)) : name;
        }

        public static StreamReader stringToReader(string data)
        {
            return new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(data)));
        }

        public static bool isDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static AdonisUI.Controls.MessageBoxResult showMessageBox(string messageBoxText)
        {
            return AdonisUI.Controls.MessageBox.Show(messageBoxText, "Assetto Tools", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Warning, AdonisUI.Controls.MessageBoxResult.Yes);
        }
    }
}
