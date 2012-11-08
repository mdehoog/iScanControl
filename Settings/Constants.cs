using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Profiler.Settings
{
    public class Constants
    {
        private readonly static string userDirectory = ParentDirectory(Application.LocalUserAppDataPath);
        private readonly static string commonDirectory = ParentDirectory(Application.CommonAppDataPath);
        private readonly static string userSettingsFile = userDirectory + Path.DirectorySeparatorChar + "user.settings";
        private readonly static string commonSettingsFile = commonDirectory + Path.DirectorySeparatorChar + "common.settings";

        private static string ParentDirectory(string path)
        {
            return Directory.GetParent(path).FullName;
        }

        public static string UserSettingsFile
        {
            get { return userSettingsFile; }
        }

        public static string CommonSettingsFile
        {
            get { return commonSettingsFile; }
        }
    }
}
