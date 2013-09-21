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

        private static UserSettingsData instance = null;
        
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
