using System;
using System.Collections;
using System.Xml;
using System.Drawing;

namespace MMI.EBuLa.Tools
{
	public enum enumDisplay
	{
		EBuLa=1, MMI, DIAGNOSE, DAVID1, DAVID2, ICE3_1, ICE3_2, ET42X, VT612, Menu, FIS_TERM
	}

    public class XMLLoader
    {
		const string VERSION = SuperNetwork.VERSION;
		
		//const int m_fps = 15;
		
		public System.Threading.ThreadPriority thread_prio = System.Threading.ThreadPriority.Normal;

        string m_EBuLaVersion, m_Path, m_File, m_Host;
        bool m_Inverse, m_useDB, m_topMost, m_focusToZusi, m_WindowHasFocus, 
			m_SearchForDepAndArr, m_doubleBuffer, m_lowFPS;
        int m_Brightness,  m_Sound, m_Port, m_fps = 15;
        Hashtable m_Keys;
        string m_filename;
		Point m_position = new System.Drawing.Point(0);
		Hashtable m_Border = new Hashtable(), m_Width = new Hashtable(), m_Height = new Hashtable();
		double m_energie;

		public static String ToolsVersion = System.Reflection.Assembly.GetAssembly(new MMI.EBuLa.Tools.XMLLoader("").GetType()).GetName().Version.ToString();

        public XMLLoader(string filename)
        {
            m_EBuLaVersion = ""; m_Path = ""; m_File = ""; m_Host = "";
            m_Inverse = false; 
            m_Brightness = -1; m_Sound = 0;
            m_Keys = new Hashtable(20);
			m_energie = 0d;
			m_lowFPS = true;

			if (filename == "") return;
            m_filename = filename;
            ReadFile();
        }

		public void FastReadFile()
		{
			if (System.IO.File.Exists(m_filename))
			{
				XmlReader reader = new XmlTextReader(m_filename);				// create XmlDocument				XmlDocument doc = new XmlDocument();				try				{					// ignore xml doctype + header					reader.MoveToContent();					doc.Load(reader);					// create root node					XmlNode root = doc.DocumentElement;					// create node list (below root)					XmlNodeList xnl = doc.ChildNodes;					root = (XmlNode)xnl[0];					try					{						if ( ((XmlNode)root.Attributes[0]).Name == "version")						{							m_EBuLaVersion = ((XmlNode)root.Attributes[0]).Value;						}					}					catch					{						m_EBuLaVersion = "1.2";					}					// create node list (below root)					xnl = root.ChildNodes;					//if (xnl.Count > 1) System.Windows.Forms.MessageBox.Show("XML Datei beschädigt!");					foreach(XmlNode node in xnl)					{						if (node.Name == "Attributes")							foreach(XmlAttribute a in node.Attributes)							{								if (a.Name == "energy")									try									{										m_energie = Convert.ToDouble(a.Value);									}									catch(Exception){}							}					}					reader.Close();
				}				catch (Exception e) {}			}
		}

        public void ReadFile()
        {
            
            if (System.IO.File.Exists(m_filename))
            {
                XmlReader reader = new XmlTextReader(m_filename);                // create XmlDocument                XmlDocument doc = new XmlDocument();                try                {                    // ignore xml doctype + header                    reader.MoveToContent();                    doc.Load(reader);                    // create root node                    XmlNode root = doc.DocumentElement;					// create node list (below root)					XmlNodeList xnl = doc.ChildNodes;					root = (XmlNode)xnl[0];                    try                    {                        if ( ((XmlNode)root.Attributes[0]).Name == "version")                        {                            m_EBuLaVersion = ((XmlNode)root.Attributes[0]).Value;                        }                    }                    catch                    {                        m_EBuLaVersion = "1.2";                    }					// create node list (below root)					xnl = root.ChildNodes;					//if (xnl.Count > 1) System.Windows.Forms.MessageBox.Show("XML Datei beschädigt!");					foreach(XmlNode node in xnl)					{						switch(node.Name)						{							case "Menu":								SetValu(node, enumDisplay.Menu);								break;							case "EBuLa":								SetValu(node, enumDisplay.EBuLa);								break;							case "MMI":								SetValu(node, enumDisplay.MMI);								break;							case "DAVID1":								SetValu(node, enumDisplay.DAVID1);								break;							case "DAVID2":								SetValu(node, enumDisplay.DAVID2);								break;							case "DIAGNOSE":								SetValu(node, enumDisplay.DIAGNOSE);								break;							case "ET42X":								SetValu(node, enumDisplay.ET42X);								break;							case "VT612":								SetValu(node, enumDisplay.VT612);								break;							case "ICE3_1":								SetValu(node, enumDisplay.ICE3_1);								break;							case "ICE3_2":								SetValu(node, enumDisplay.ICE3_2);								break;							case "FIS_TERM":								SetValu(node, enumDisplay.FIS_TERM);								break;							case "Attributes":								foreach(XmlAttribute a in node.Attributes)								{									switch (a.Name)									{										case "inverse":											if (a.Value == "true")											{												m_Inverse = true;											}											else if (a.Value == "false")											{												m_Inverse = false;											}											else											{												throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");											}											break;										case "brightness":											break;										case "sound":											if (a.Value == "API")											{												m_Sound = 1;											}											else if (a.Value == "DX")											{												m_Sound = 2;											}											else if (a.Value == "off" || a.Value == "")											{												m_Sound = 0;											}											else											{												throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");											}											break;										case "topmost":											if (a.Value == "true")												m_topMost = true;											else 												m_topMost = false;											break;										case "focustozusi":											if (a.Value == "true")												m_focusToZusi = true;											else 												m_focusToZusi = false;											break;										case "doublebuffer":											if (a.Value == "true")												m_doubleBuffer = true;											else 												m_doubleBuffer = false;											break;										case "lowFPS":											if (a.Value == "true")												m_lowFPS = true;											else 												m_lowFPS = false;											break;										case "energy":											try											{												m_energie = Convert.ToDouble(a.Value);											}											catch(Exception){}											break;										case "fps":											m_fps = Convert.ToInt32(a.Value);											break;										default:											System.Windows.Forms.MessageBox.Show("Eintrag in XML Datei nicht erkannt: "+a.Name);											break;									}								}								break;							case "IOSystem":								foreach(XmlAttribute a in node.Attributes)								{									switch(a.Name)									{										case "port":											m_Port = Convert.ToInt32(a.Value);											break;										case "host":											m_Host = a.Value;											break;										/*case "useDB":											if (a.Value == "yes") m_useDB = true;											break;*/										case "EBuLADeepSearch":											if (a.Value == "yes") m_SearchForDepAndArr = true;											break;										case "path":											m_Path = a.Value;											break;										/*case "file":											m_File = a.Value;											break;*/									}								}								break;							case "Keys":								foreach(XmlAttribute a in node.Attributes)								{									int keynr = 0;									if (IsValidKey(a.Name))									{										try										{											if (a.Value != "") 											{												keynr = Convert.ToInt32(a.Value);												m_Keys.Add(a.Name,keynr);											}											else											{												m_Keys.Add(a.Name,-1);											}										}										catch (Exception)										{											throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");										}									}									else									{										System.Windows.Forms.MessageBox.Show(a.Name+" is kei gültiger Key!");										break;									}								}								break;						}					}                    reader.Close();
                }                catch (Exception e)                {                    System.Windows.Forms.MessageBox.Show("XML Lesefehler: " + e.Message.ToString());                    return;                }            }
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

                writer.WriteStartElement("ZusiDisplay");

				writer.WriteAttributeString("version",VERSION);

				WriteAttributes(ref writer);
				
				WriteIOSystem(ref writer);

				WriteKeys(ref writer);

				WriteValues(ref writer);

                //ZusiDisplay
                writer.WriteEndElement();

                // Puffer leeren und schließen
                writer.Flush();
                writer.Close();

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Beim Schreiben der XML Datei ist ein Fehler aufgetreten! ("+e.Message+")");
            }
        }

		private void WriteValues(ref XmlTextWriter writer)
		{
			WriteValue(ref writer, "Menu", enumDisplay.Menu);
			WriteValue(ref writer, "EBuLa", enumDisplay.EBuLa);
			WriteValue(ref writer, "MMI", enumDisplay.MMI);
			WriteValue(ref writer, "DIAGNOSE", enumDisplay.DIAGNOSE);
			WriteValue(ref writer, "DAVID1", enumDisplay.DAVID1);
			WriteValue(ref writer, "DAVID2", enumDisplay.DAVID2);
			WriteValue(ref writer, "ICE3_1", enumDisplay.ICE3_1);
			WriteValue(ref writer, "ICE3_2", enumDisplay.ICE3_2);
			WriteValue(ref writer, "ET42X", enumDisplay.ET42X);
			WriteValue(ref writer, "VT612", enumDisplay.VT612);
			WriteValue(ref writer, "FIS_TERM", enumDisplay.FIS_TERM);
		}

		private void WriteValue(ref XmlTextWriter writer, string tag, enumDisplay disp)
		{
			writer.WriteStartElement(tag);

			writer.WriteAttributeString("border", ((bool)m_Border[disp]).ToString().ToLower());

			writer.WriteAttributeString("width",((int)m_Width[disp]).ToString());

			writer.WriteAttributeString("height",((int)m_Height[disp]).ToString());

			writer.WriteEndElement();
		}

        private void WriteAttributes(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("Attributes");

            writer.WriteAttributeString("inverse",m_Inverse.ToString().ToLower());

            writer.WriteAttributeString("brightness",m_Brightness.ToString());

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

			writer.WriteAttributeString("doublebuffer",m_doubleBuffer.ToString().ToLower());

			writer.WriteAttributeString("energy",m_energie.ToString().ToLower());
			writer.WriteAttributeString("lowFPS",m_lowFPS.ToString().ToLower());
			writer.WriteAttributeString("fps",m_fps.ToString().ToLower());

            writer.WriteEndElement();
        }

        private void WriteIOSystem(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("IOSystem");

            writer.WriteAttributeString("path", m_Path);

            //writer.WriteAttributeString("file", m_File);

			/*if (m_useDB)
				writer.WriteAttributeString("useDB", "yes");
			else
				writer.WriteAttributeString("useDB", "no");*/
			if (m_SearchForDepAndArr)
				writer.WriteAttributeString("EBuLADeepSearch", "yes");
			else
				writer.WriteAttributeString("EBuLADeepSearch", "no");
			

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
        /// Gibt die Version von EBuLa zurück
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

		public void SetBorder(enumDisplay display, bool border)
		{
			m_Border[display] = border;
		}

		public bool GetBorder(enumDisplay display)
		{
			return (bool)m_Border[display];
		}

		public void SetWidth(enumDisplay display, int width)
		{
			m_Width[display] = width;
		}

		public int GetWidth(enumDisplay display)
		{
			return (int)m_Width[display];
		}

		public void SetHeight(enumDisplay display, int height)
		{
			m_Height[display] = height;
		}					  

		public int GetHeight(enumDisplay display)
		{
			return (int)m_Height[display];
		}                  

        public string Path {get{return m_Path;}set{m_Path=value;}}

        public string File {get{return m_File;}set{m_File=value;}}

        public int Sound {get{return m_Sound;}set{m_Sound=value;}}
		
		public int FramesPerSecondLow 
		{
			get
			{
				if (m_lowFPS)
					return 2;
				else
					return m_fps;
			}
		}
		public int FramesPerSecond 
		{
			get
			{
				return m_fps;
			}
			set
			{
				m_fps = value;
			}
		}
		public bool LowFPS{get{return m_lowFPS;}set{m_lowFPS=value;}}
		public bool UseDB{get{return m_useDB;}set{m_useDB=value;}}
		public bool TopMost{get{return m_topMost;}set{m_topMost=value;}}
		public bool FocusToZusi{get{return m_focusToZusi;}set{m_focusToZusi=value;}}
		public bool DoubleBuffer{get{return m_doubleBuffer;}set{m_doubleBuffer=value;}}
		public bool WindowHasFocus{get{return m_WindowHasFocus;}set{m_WindowHasFocus=value;}}
		public bool SearchForDepAndArr{get{return m_SearchForDepAndArr;}set{m_SearchForDepAndArr=value;}}
		public int Port{get{return m_Port;}set{m_Port=value;}}
		public string Host {get{return m_Host;}set{m_Host=value;}}
		
		public double Energie{get{return m_energie;}set{m_energie=value;}}
		public Point Position{get{return m_position;}set{m_position=value;}}
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

		private void SetValu(XmlNode node, enumDisplay display)
		{
			foreach(XmlAttribute a in node.Attributes)			{				switch (a.Name)				{					case "border":						if (a.Value == "true")						{							m_Border[display] = true;						}						else if (a.Value == "false")						{							m_Border[display] = false;						}						else						{							throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");						}						break;					case "width":						if (a.Value != "")						{							try							{								m_Width[display] = Convert.ToInt32(a.Value);							}							catch							{								throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");							}						}						else						{							m_Width[display] = 0;						}						break;                                					case "height":						if (a.Value != "")						{							try							{								m_Height[display] = Convert.ToInt32(a.Value);							}							catch							{								throw new Exception("Eintrag '"+a.Name+"' hat falschen Wert!");							}						}						else						{							m_Width[display] = 0;						}						break;				}
			}
		}
	}
}
