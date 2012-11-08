using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Profiler.Settings
{
    public abstract class AbstractSettings
    {
        protected bool dirty;

        protected static T Load<T>(string file) where T : AbstractSettings, new()
        {
            T settings = null;
            FileStream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                {
                    stream = fi.OpenRead();
                    settings = (T)serializer.Deserialize(stream);
                    if (settings != null)
                        settings.dirty = false; //just loaded, so not dirty
                }
            }
            catch (Exception)
            {
                //ignore any exceptions; just create a new settings object with the defaults
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            if (settings == null)
            {
                settings = new T();
            }

            return settings;
        }

        protected bool Save(string file)
        {
            bool result = true;
            if (dirty)
            {
                StreamWriter writer = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(this.GetType());
                    writer = new StreamWriter(file, false);
                    serializer.Serialize(writer, this);
                    dirty = false;
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    if (writer != null) writer.Close();
                }
            }
            return result;
        }
    }
}
