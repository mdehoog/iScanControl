using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

namespace Qixle.iScanDuo.Controller.Settings
{
    public class UserSettings : AbstractSettings
    {
        private static UserSettings instance;

        public static UserSettings Instance
        {
            get { return instance; }
        }

        static UserSettings()
        {
            instance = Load<UserSettings>(Constants.UserSettingsFile);
            instance.FixInvalidSettings();
            Save(); //save if dirty
        }

        public static void Save()
        {
            instance.Save(Constants.UserSettingsFile);
        }

        public UserSettings()
        {
            LoadDefaults();
        }

        private void LoadDefaults()
        {
            LastPortName = null;
            LastBaudRate = 0;
            SavedColors = new List<SavedColor>();
        }

        private void FixInvalidSettings()
        {
            try
            {
                if (SavedColors == null)
                {
                    SavedColors = new List<SavedColor>();
                }
            }
            catch
            {
                //if any exceptions occur, load the defaults just to be safe
                LoadDefaults();
            }
        }

        private string lastPortName;
        private int lastBaudRate;
        private List<SavedColor> savedColors = new List<SavedColor>();

        public string LastPortName
        {
            get { return lastPortName; }
            set { if (lastPortName != value) dirty = true; lastPortName = value; }
        }

        public int LastBaudRate
        {
            get { return lastBaudRate; }
            set { if (lastBaudRate != value) dirty = true; lastBaudRate = value; }
        }

        public List<SavedColor> SavedColors
        {
            get { return savedColors; }
            set { if (savedColors != value) dirty = true; savedColors = value; }
        }

        public void AddSavedColor(SavedColor color)
        {
            SavedColors.Add(color);
            dirty = true;
        }

        public void RemoveSavedColor(SavedColor color)
        {
            SavedColors.Remove(color);
            dirty = true;
        }
    }
}
