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
				XmlReader reader = new XmlTextReader(filename);

			{
				string tname = ""; string ts = ""; string tgnt = ""; string one_track = "";

				XmlReader reader = new XmlTextReader(filename);
			{
				XmlTextReader reader = new XmlTextReader(filename);
											offset = i;
				catch (Exception) 
				{
					doc = null;
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			//writer.WriteDocType("ebula_track",null,"ebula_track.dtd",null);

			writer.WriteAttributeString("version","1.0");
          
			writer.WriteAttributeString("trackname",r.Name);
			writer.WriteAttributeString("track_side",r.track_side);
			writer.WriteAttributeString("gnt",r.track_gnt);
			if (r.one_track)
                writer.WriteAttributeString("one_track","yes");
			else
                writer.WriteAttributeString("one_track","no");

			// create XML Document Object
			XmlElement xdesc = xdoc.CreateElement("description");

			xdesc.Attributes.Append(CreateXmlAttribute(ref xdoc, "desc",r.Description));

				xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "position",e.m_position));
			xdoc.Save(writer);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			//writer.WriteDocType("ebula_track",null,"ebula_track.dtd",null);

			writer.WriteAttributeString("version","1.0");
          
			writer.WriteAttributeString("trackname",r.trackname);
			writer.WriteAttributeString("traintype",r.traintype);
			writer.WriteAttributeString("trainnumber",r.trainnumber);

			// create XML Document Object
			XmlElement xprop = xdoc.CreateElement("properties");

			xprop.Attributes.Append(CreateXmlAttribute(ref xdoc, "from",r.from));
			XmlElement xdesc = xdoc.CreateElement("description");

			xdesc.InnerText = r.Description;

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name",e.m_ops_name));

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name", "---LAST-ENTRY---"));

					xentry.Attributes.Append(CreateXmlAttribute(ref xdoc, "name",e.m_ops_name));
			xdoc.Save(writer);