using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace MMI.EBuLa
{
	public class XMLReader
	{
		Route route;
		static Control control;
		static ArrayList trains;

		public XMLReader()
		{
			route = new Route("", "");
			trains = new ArrayList();
		}

		public static ArrayList ReadTrainlistFromDB(string schedule, string trackpath , ref Control c)
		{
			control = c;
			trains = new ArrayList();
			string[] files = new string[8000];
			string trackp = trackpath;
			if (trackp == "") trackp = c.getTrackPath()+@"\EBula";
			else trackp += @"\EBuLa";
			try
			{
				files = Directory.GetFiles(trackp, "train_"+schedule+"*.xml");
			}
			catch (Exception)
			{
				System.Windows.Forms.MessageBox.Show("Der Pfad "+trackp+" existiert nicht!");
			}

			c.track_ht.Clear();

			foreach(string s in files)
			{
				string tr_num = "", tr_typ = "";
				string tr_name = parseTrainFile(s, out tr_num, out tr_typ);
				trains.Add(tr_name);
				try
				{
					c.track_ht.Add(tr_typ+tr_num, c.buffer_trackname);
				}
				catch(Exception){break;}
			}

			return trains;
		}

		private static string parseTrainFile(string filename, out string tr_num, out string tr_type)
		{
			string trainnumber = "", traintype ="", description = "", from = "";

			tr_num = trainnumber;
			tr_type = traintype;

			if (System.IO.File.Exists(filename))
			{
				XmlReader reader = new XmlTextReader(filename);				// create XmlDocument				XmlDocument doc = new XmlDocument();				try				{					// ignore xml doctype + header					reader.MoveToContent();					doc.Load(reader);					// create root node					XmlNode root = doc.DocumentElement;					foreach(XmlAttribute att in root.Attributes)					{						if (att.Name == "trackname")						{							control.buffer_trackname = att.Value;						}						if (att.Name == "trainnumber")						{							trainnumber = att.Value;						}						if (att.Name == "traintype")						{							traintype = att.Value;						}					}					// create node list (below root)					XmlNodeList xnl = root.ChildNodes;					if (xnl.Count < 3)					{						System.Windows.Forms.MessageBox.Show("Fehler im Fahrplan "+filename+" !");						doc = null;						reader.Close();						reader = null;						return "";					}					foreach(XmlNode node in xnl)					{						if (node.Name == "properties")						{							foreach(XmlAttribute att in node.Attributes)							{								if (att.Name == "from")								{									from = att.Value;									break;								}							}						}						if (node.Name == "description")						{							description = node.InnerText;						}						if (description != "" && from != "")						{							break;						}					}				}				catch (Exception)				{				}				tr_num = trainnumber;				tr_type = traintype;				return (trainnumber+" ["+traintype+"] ab "+from+" "+description);				doc = null;				reader.Close();				//reader = null;			}			else			{				return "";			}		}
		public static Route ReadTackFromDB(string track, bool right_side, bool gnt, bool ignore)		{			if (track == "") return null;			string mbr = "", description = "";			// setup route			Route r = null;			// setup filename			string filename = track;//  track.Remove(track.Length-1-3, 4);						if (!ignore)			{				filename = Path.GetDirectoryName(filename)+@"\track_"+Path.GetFileNameWithoutExtension(filename);				if (right_side) filename += "_right";				else			filename += "_left";				if (gnt) filename += "_gnt";				filename += ".xml";			}			// open file			if (System.IO.File.Exists(filename))
			{
				string tname = ""; string ts = ""; string tgnt = ""; string one_track = "";

				XmlReader reader = new XmlTextReader(filename);				// create XmlDocument				XmlDocument doc = new XmlDocument();				try				{					// ignore xml doctype + header					reader.MoveToContent();					doc.Load(reader);					// create root node					XmlNode root = doc.DocumentElement;					foreach(XmlAttribute att in root.Attributes)					{						if (att.Name == "trackname")							tname = att.Value;						else if (att.Name == "track_side")							ts = att.Value;						else if (att.Name == "gnt")							tgnt = att.Value;						else if (att.Name == "one_track")							one_track = att.Value;					}					XmlNodeList xnl = root.ChildNodes;					foreach(XmlNode node in xnl)					{						if (node.Name == "description")						{							foreach(XmlAttribute att in node.Attributes)							{								if (att.Name == "desc")									description = att.Value;								if (att.Name == "mbr")									mbr = att.Value;							}							r = new Route(tname, description);							r.Description = description;							r.mbr = mbr;							r.track_side = ts;							r.track_gnt = tgnt;							if (one_track == "yes") r.one_track = true;						}						if (node.Name == "entry")						{							if (r == null)								break;							string pos = "", speed = "", name = "",								sig_speed = "";							bool tunnel = false;							string zack = "0";							foreach(XmlAttribute att in node.Attributes)							{								switch(att.Name)								{									case "position":										pos = att.Value;										break;									case "speed":										speed = att.Value;										break;									case "name":										name = att.Value;										break;									case "sig_speed":										sig_speed = att.Value;										break;									case "zack":										zack = att.Value;										break;									case "tunnel":										if (att.Value == "yes")											tunnel = true;										break;								}							}							// new entry							Entry e = new Entry(EntryType.OPS_MARKER, pos, speed, "", name, sig_speed, "", new char(), "", new char());							e.tunnel = tunnel;							e.zack = Convert.ToByte(zack);							r.Entrys.Add(e);							continue;						}					}				}				catch (Exception e) 				{					doc = null;					reader.Close();					//reader = null;					System.Windows.Forms.MessageBox.Show("Fehler: "+e.Message);				}				doc = null;				reader.Close();				//reader = null;			}			return r;		}		public static Route AddTimetableToRoute(Route r, string filename)		{			return AddTimetableToRoute(r, filename, false);		}		public static Route AddTimetableToRoute(Route r, string filename, bool editor)		{			Route route = r;			if (System.IO.File.Exists(filename))
			{
				XmlTextReader reader = new XmlTextReader(filename);				// create XmlDocument				XmlDocument doc = new XmlDocument();				try				{					// ignore xml doctype + header					reader.MoveToContent();					doc.Load(reader);					// create root node					XmlNode root = doc.DocumentElement;					foreach(XmlAttribute att in root.Attributes)					{						if (att.Name == "trackname")						{							route.trackname = att.Value;						}						else if (att.Name == "traintype")						{							route.traintype = att.Value;						}						else if (att.Name == "trainnumber")						{							route.trainnumber = att.Value;						}					}					XmlNodeList xnl = root.ChildNodes;					string last_name = "";					foreach(XmlNode node in xnl)					{						if (node.Name == "properties")						{							foreach(XmlAttribute att in node.Attributes)							{								if (att.Name == "from")									route.from = att.Value;								else if (att.Name == "timetable_date")									route.schedule_date = att.Value;								else if (att.Name == "timetable_valid")									route.schedule_valid = att.Value;								else if (att.Name == "waypoints")									route.waypoints = att.Value;								else if (att.Name == "tfz")									route.tfz = att.Value;								else if (att.Name == "vmax")								{									if (editor)										route.Vmax = att.Value;									else										control.buffer_vmax = att.Value;								}							}						}						else if (node.Name == "description")						{							route.Description = node.InnerText;						}						else if (node.Name == "entry")						{							string name = "", arr = "", dep = "";							foreach(XmlAttribute att in node.Attributes)							{								if (att.Name == "name") name = att.Value;								if (att.Name == "arr_time") arr = att.Value;								if (att.Name == "dep_time") dep = att.Value;							}							if (name == "---LAST-ENTRY---")							{								int offset = -1;								// delete the remaining timetable								for (int i = 0; i < route.Entrys.Count; i++)								{									if (((Entry)route.Entrys[i]).m_ops_name == last_name)									{										((Entry)route.Entrys[i]).isLast = true;																				if (!editor)										{
											offset = i;											break;										}									}								}								if (offset != -1)								{									route.Entrys.RemoveRange(offset+1, route.Entrys.Count-(offset+1));									continue;								}							}							foreach(Entry e in route.Entrys)							{								if (e.m_ops_name == name)								{									e.m_eta = arr;									e.m_etd = dep;									break;								}							}							last_name = name;						}					}				}
				catch (Exception) 
				{
					doc = null;					reader.Close();					//reader = null;				};				doc = null;				if (reader != null)				{					reader.Close();					//reader = null;				}			}			else			{				System.Windows.Forms.MessageBox.Show("Die Datei "+filename+" wurde nicht gefunden!");			}			return route;					}		public static Route RemoveHighSpeed(Route r, Control control)		{			int old_vmax = -1;			int train_vmax;			try			{				train_vmax = Convert.ToInt32(control.buffer_vmax);			}			catch (Exception)			{				train_vmax = 0;			}			for(int i=0; i < r.Entrys.Count; i++)			{				int vmax = -1;				Entry e = (Entry)r.Entrys[i];								if (e.m_speed != "")				{					vmax = Convert.ToInt32(e.m_speed);				}				if (vmax > train_vmax) 				{					if (old_vmax < train_vmax) //geschwindigkeitserhoehung					{						e.m_speed=train_vmax.ToString();						r.Entrys[i] = e;					}					else					{						if (e.m_ops_name == "")						{							r.Entrys.Remove(e);							i--;							continue;						}						else						{							e.m_speed = "";							r.Entrys[i] = e;						}					}									}				else if (vmax == train_vmax)				{					if (old_vmax > train_vmax) //geschwindigkeitsreduzierung					{						if (e.m_ops_name == "")						{							r.Entrys.Remove(e);							i--;							continue;						}						else						{							e.m_speed = "";							r.Entrys[i] = e;						}					}				}				if (vmax != -1)					old_vmax = vmax;			}			return r; 		}		public static void SaveRouteToTrackFile(Route r, string filename)		{			GC.Collect();			XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);			// write XML file with indentation ("einruecken")
			writer.Formatting = Formatting.Indented;			// write standard document beginning (<!xml...>)
			writer.WriteStartDocument();			// write doctype (<!DOCTYPE...>)
			//writer.WriteDocType("ebula_track",null,"ebula_track.dtd",null);            			writer.WriteStartElement("ebula_track");

			writer.WriteAttributeString("version","1.0");
          
			writer.WriteAttributeString("trackname",r.Name);
			writer.WriteAttributeString("track_side",r.track_side);
			writer.WriteAttributeString("gnt",r.track_gnt);
			if (r.one_track)
                writer.WriteAttributeString("one_track","yes");
			else
                writer.WriteAttributeString("one_track","no");

			// create XML Document Object			XmlDocument xdoc = new XmlDocument();			XmlElement root = xdoc.DocumentElement;
			XmlElement xdesc = xdoc.CreateElement("description");

			xdesc.Attributes.Append(CreateXmlAttribute(ref xdoc, "desc",r.Description));			xdesc.Attributes.Append(CreateXmlAttribute(ref xdoc, "mbr",r.mbr));			xdesc.WriteTo(writer);			foreach (Entry e in r.Entrys)			{				XmlElement xentry = xdoc.CreateElement("entry");

				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "position",e.m_position));				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "speed",e.m_speed));				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name",e.m_ops_name));				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "sig_speed",e.m_ops_speed));				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "zack",e.zack.ToString()));				if (e.tunnel)					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "tunnel","yes"));				else 					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "tunnel","no"));				xentry.WriteTo(writer);			}
			xdoc.Save(writer);			writer.Flush();			writer.Close();		}		public static void SaveRouteToTrainFile(Route r, string filename)		{			GC.Collect();			XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);			// write XML file with indentation ("einruecken")
			writer.Formatting = Formatting.Indented;			// write standard document beginning (<!xml...>)
			writer.WriteStartDocument();			// write doctype (<!DOCTYPE...>)
			//writer.WriteDocType("ebula_track",null,"ebula_track.dtd",null);            			writer.WriteStartElement("ebula_train");

			writer.WriteAttributeString("version","1.0");
          
			writer.WriteAttributeString("trackname",r.trackname);
			writer.WriteAttributeString("traintype",r.traintype);
			writer.WriteAttributeString("trainnumber",r.trainnumber);

			// create XML Document Object			XmlDocument xdoc = new XmlDocument();			XmlElement root = xdoc.DocumentElement;
			XmlElement xprop = xdoc.CreateElement("properties");

			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "from",r.from));			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "timetable_date",r.schedule_date));			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "timetable_valid",r.schedule_valid));			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "waypoints",r.waypoints));			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "tfz",r.tfz));			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "vmax",r.Vmax));			xprop.WriteTo(writer);
			XmlElement xdesc = xdoc.CreateElement("description");

			xdesc.InnerText = r.Description;			xdesc.WriteTo(writer);			foreach (Entry e in r.Entrys)			{				if (e.isLast)				{					XmlElement xentry = xdoc.CreateElement("entry");

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name",e.m_ops_name));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "arr_time",e.m_eta));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "dep_time",e.m_etd));					xentry.WriteTo(writer);					xentry = xdoc.CreateElement("entry");

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name", "---LAST-ENTRY---"));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "arr_time",""));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "dep_time",""));					xentry.WriteTo(writer);					break;				}				else				{					if (e.m_eta == "" && e.m_etd == "") continue;					XmlElement xentry = xdoc.CreateElement("entry");

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name",e.m_ops_name));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "arr_time",e.m_eta));					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "dep_time",e.m_etd));					xentry.WriteTo(writer);				}			}
			xdoc.Save(writer);			writer.Flush();			writer.Close();		}				private static XmlAttribute CreateXmlAttribute(ref XmlDocument doc, String m_name, String m_value)		{			XmlAttribute xatt = doc.CreateAttribute(m_name);			xatt.Value = m_value;			return xatt;		}	}}
