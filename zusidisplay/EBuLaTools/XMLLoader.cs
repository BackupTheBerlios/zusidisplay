using System;
using System.Collections;
using System.Xml;

namespace MMI.EBuLa.Tools
{
    public class XMLLoader
    {
		const string VERSION = "1.5.0";
		
		int m_fps = 20;

        string m_EBuLaVersion, m_Path, m_File, m_Host;
        bool m_Inverse, m_Border, m_useDB, m_topMost, m_focusToZusi, m_WindowHasFocus;
        int m_Brightness, m_Width, m_Height, m_Sound, m_Port;
        Hashtable m_Keys;
        string m_filename;

		public static String ToolsVersion = System.Reflection.Assembly.GetAssembly(new MMI.EBuLa.Tools.XMLLoader("").GetType()).GetName().Version.ToString();

        public XMLLoader(string filename)
        {
            m_EBuLaVersion = ""; m_Path = ""; m_File = ""; m_Host = "";
            m_Inverse = false; m_Border = true;
            m_Brightness = -1; m_Width = -1; m_Height = -1; m_Sound = 0;
            m_Keys = new Hashtable(20);

			if (filename == "") return;
            m_filename = filename;
            ReadFile();
        }


        public void ReadFile()
        {
            
            if (System.IO.File.Exists(m_filename))
            {
                XmlReader reader = new XmlTextReader(m_filename);
                }
            else
            {
                System.Windows.Forms.MessageBox.Show("XML Datei "+m_filename+" nicht gefunden!");
            }
        }

        public void SaveFile()
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter(m_filename, System.Text.Encoding.UTF8);
                
                // Formatierung festlegen
                writer.Formatting = Formatting.Indented;

                // Standarddeklarator schreiben
                writer.WriteStartDocument();

                writer.WriteStartElement("MMI");

                writer.WriteStartElement("EBuLa");

                writer.WriteAttributeString("version",VERSION);

                WriteAttributes(ref writer);

                WriteIOSystem(ref writer);

                WriteKeys(ref writer);

                //EBuLa
                writer.WriteEndElement();

                //MMI
                writer.WriteEndElement();

                // Puffer leeren und schlie�en
                writer.Flush();
                writer.Close();

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Beim Schreiben der XML Datei ist ein Fehler aufgetreten! ("+e.Message+")");
            }
        }


        private void WriteAttributes(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("Attributes");

            writer.WriteAttributeString("inverse",m_Inverse.ToString().ToLower());

            writer.WriteAttributeString("brightness",m_Brightness.ToString());

            writer.WriteAttributeString("border",m_Border.ToString().ToLower());
            
            writer.WriteAttributeString("width",m_Width.ToString());

            writer.WriteAttributeString("height",m_Height.ToString());

            if (m_Sound == 2)
            {
                writer.WriteAttributeString("sound", "DX");
            }
            else if (m_Sound == 1)
            {
                writer.WriteAttributeString("sound", "API");
            }
            else
            {
                writer.WriteAttributeString("sound", "off");
            }

			writer.WriteAttributeString("topmost",m_topMost.ToString().ToLower());
			writer.WriteAttributeString("focustozusi",m_focusToZusi.ToString().ToLower());

            writer.WriteEndElement();
        }

        private void WriteIOSystem(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("IOSystem");

            writer.WriteAttributeString("path", m_Path);

            writer.WriteAttributeString("file", m_File);

			if (m_useDB)
				writer.WriteAttributeString("useDB", "yes");
			else
				writer.WriteAttributeString("useDB", "no");

			writer.WriteAttributeString("port", m_Port.ToString());

			writer.WriteAttributeString("host", m_Host);

            writer.WriteEndElement();
        }

        private void WriteKeys(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("Keys");

            WriteKey(ref writer, "key_E");
            WriteKey(ref writer, "key_C");
            WriteKey(ref writer, "key_Left");
            WriteKey(ref writer, "key_Right");
            WriteKey(ref writer, "key_Up");
            WriteKey(ref writer, "key_Down");
            WriteKey(ref writer, "key_1");
            WriteKey(ref writer, "key_2");
            WriteKey(ref writer, "key_3");
            WriteKey(ref writer, "key_4");
            WriteKey(ref writer, "key_5");
            WriteKey(ref writer, "key_6");
            WriteKey(ref writer, "key_7");
            WriteKey(ref writer, "key_8");
            WriteKey(ref writer, "key_9");
            WriteKey(ref writer, "key_0");
            WriteKey(ref writer, "key_Brightness");
            WriteKey(ref writer, "key_Invert");
            WriteKey(ref writer, "key_OnOff");

            writer.WriteEndElement();
        }

        /// <summary>
        /// Gibt die Version von EBuLa zur�ck
        /// </summary>
        public int Version
        {
            get
            { 
                try
                {
                    // Version = "1.3" -> 1.3 -> 13
                    return Convert.ToInt32(Convert.ToDouble(m_EBuLaVersion));
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }

        public bool Inverse {get{return m_Inverse;}set{m_Inverse=value;}}

        public int Brightness {get{return m_Brightness;}set{m_Brightness=value;}}

        public bool Border {get{return m_Border;}set{m_Border=value;}}

        public int Width {get{return m_Width;}set{m_Width=value;}}

        public int Height {get{return m_Height;}set{m_Height=value;}}

        public string Path {get{return m_Path;}set{m_Path=value;}}

        public string File {get{return m_File;}set{m_File=value;}}

        public int Sound {get{return m_Sound;}set{m_Sound=value;}}
		
		public int FramesPerSecond {get{return m_fps;}set{m_fps=value;}}

		public bool UseDB{get{return m_useDB;}set{m_useDB=value;}}
		public bool TopMost{get{return m_topMost;}set{m_topMost=value;}}
		public bool FocusToZusi{get{return m_focusToZusi;}set{m_focusToZusi=value;}}
		public bool WindowHasFocus{get{return m_WindowHasFocus;}set{m_WindowHasFocus=value;}}
		public int Port{get{return m_Port;}set{m_Port=value;}}
		public string Host {get{return m_Host;}set{m_Host=value;}}
		
        public int Key (string keyName)    
        {
            if (m_Keys.Contains(keyName))
            {
                foreach (DictionaryEntry de in m_Keys)
                {
                    if (de.Key.ToString() == keyName)
                    {
                        return (int)de.Value;
                    }
                }
            }
            return -1;
        }

        public void SetKey(string keyName, int newValue)
        {
            if (m_Keys.Contains(keyName))
            {
                foreach (DictionaryEntry de in m_Keys)
                {
                    if (de.Key.ToString() == keyName)
                    {
                        m_Keys.Remove(keyName);
                        m_Keys.Add(keyName, newValue);
                        break;
                    }
                }
            }
        }

        public bool IsInUse(int keycode)
        {
            foreach (DictionaryEntry de in m_Keys)
            {
                if (de.Value.ToString() == keycode.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidKey(string k)
        {
            return (k == "key_E" || k == "key_C" || k == "key_0" || k == "key_1" || k == "key_2" || k == "key_3" || k == "key_4" || k == "key_5" || k == "key_6" || k == "key_7" || k == "key_8" || k == "key_9" || k == "key_Left" || k == "key_Right" || k == "key_Up" || k == "key_Down" || k == "key_Invert" || k == "key_Brightness" || k == "key_OnOff");
        }

        private void WriteKey(ref XmlTextWriter writer, string name)
        {
            int key = Key(name);
            if (key == -1)
            {
                writer.WriteAttributeString(name, "");
            }
            else
            {
                writer.WriteAttributeString(name, Key(name).ToString());
            }
        }
	}
}