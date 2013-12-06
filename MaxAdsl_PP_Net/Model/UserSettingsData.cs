using MaxAdsl_PP_Net.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MaxAdsl_PP_Net.Model
{
    [Serializable]
    internal class UserSettingsData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool CheckTrafficOnStartup { get; set; }

        private WebParserFactory.WebParserTypes useWebParser = WebParserFactory.WebParserTypes.Full;
        public WebParserFactory.WebParserTypes UseWebParser 
        {
            get { return useWebParser; }
            set { useWebParser = value; }
        }
        

        private static UserSettingsData instance = null;
        private UserSettingsData() { }
        public static UserSettingsData GetUserSettingsInstance()
        {
            if (instance == null)
                LoadSettings();

            return instance;
        }

        public void StoreSettings()
        {
            byte[] encSettings = Utility.Tools.EncryptData(instance);
            File.WriteAllBytes("settings.bin", encSettings);
        }

        public static void LoadSettings()
        {
            if (File.Exists("settings.bin"))
            {
                byte[] encSettings = File.ReadAllBytes("settings.bin");
                instance = Utility.Tools.DecryptData<UserSettingsData>(encSettings);
            }
            else
            {
                instance = new UserSettingsData();
            }
        }
    }
}
