using System;
using MMI.EBuLa;
using System.Xml;
using System.IO;

namespace MMI.EBuLaEditor
{
	public class DataControl
	{
		#region Members
		
		public Route route;
		public bool isChanged = false;
		public bool isTrack = true;
		public bool isNewFile = true;
		public string filename = "";

		public bool OWN_FILL = false;

		public int OLD_INDEX = 0;

		#endregion

		public DataControl()
		{
		}

		public void ReadTrack(string filename)
		{
			this.filename = filename;
			route = XMLReader.ReadTackFromDB(filename, false, false, true);
		}

		public void ReadTrain(string filename)
		{
			this.filename = filename;
			string trackfilename = GetTrackName(filename);
			route = XMLReader.ReadTackFromDB(trackfilename, false, false, true);
			route = XMLReader.AddTimetableToRoute(route, filename, true);		
		}
		
		public void AddEntriesToTrain(ref TrainForm form)
		{
			AddEntriesToTrain(ref form, 0);
		}
		public void AddEntriesToTrain(ref TrainForm form, int index)
		{
			string text = "";
			Entry e = null;
			form.lbEntries.Items.Clear();
			//form.lbEntries.BeginUpdate();
			for(int i = 0; i < route.Entrys.Count; i++)
			{
				e = (Entry)route.Entrys[i];
				if (i<9) text = "0"+(i+1).ToString();
				else text = (i+1).ToString();
				if (e.isLast) text = "*"+text;
				string ops_name = e.m_ops_name;
				if (e.m_ops_name == "") ops_name = "     ";
				if (e.isLast) ops_name += " (LB)";
				
				text += "\t" + ops_name + "\tAN: " + e.m_eta + "\tAB: " + e.m_etd;
				form.lbEntries.Items.Add(text);
			}
			form.lbEntries.SelectedIndex = index;
			//form.lbEntries.ResumeLayout();
			UpdateTrainForm(ref form);
			UpdateTrainEntries(ref form);			
		}
		public void AddEntriesToTrack(ref TrackForm form)
		{
			AddEntriesToTrack(ref form, 0);
		}
		
		public void AddEntriesToTrack(ref TrackForm form, int index)
		{
			string text = "";
			Entry e = null;
			form.lbEntries.Items.Clear();
			//form.lbEntries.BeginUpdate();
			for(int i = 0; i < route.Entrys.Count; i++)
			{
				e = (Entry)route.Entrys[i];
				if (i<9) text = "0"+(i+1).ToString();
				else text = (i+1).ToString();
				text += "\t" + e.m_position + "\t" + e.m_speed;
				text += "\t" + e.m_ops_name + "\t" + e.m_ops_speed;
				form.lbEntries.Items.Add(text);
			}
			if (form.lbEntries.Items.Count < index+1)
				index--;
			form.lbEntries.SelectedIndex = index;
			//form.lbEntries.ResumeLayout();
            UpdateTrackForm(ref form);
            UpdateTrackEntries(ref form);			
		}

		
		public void UpdateTrainForm(ref TrainForm form)
		{
			bool OWN_FILL_2 = false;
			if (OWN_FILL) OWN_FILL_2 = true;;
			OWN_FILL = true;
			form.tbName.Text = route.trackname;
			form.tbTrainType.Text = route.traintype;
			form.tbTrainNumber.Text = route.trainnumber;
			form.tbTfz.Text = route.tfz;
			form.tbVmax.Text = route.Vmax;
			form.tbWaypoints.Text = route.waypoints;
			form.tbValidSince.Text = route.schedule_date;
			form.tbValidSpan.Text = route.schedule_valid;
			form.tbDays.Text = route.Description;
			form.tbBegin.Text = route.from;
			OWN_FILL = OWN_FILL_2;
		}

		public void UpdateTrainEntries(ref TrainForm form)
		{
			if (form.lbEntries.SelectedIndex < 0 || form.lbEntries.SelectedIndex >= form.lbEntries.Items.Count)
				return;

			if (!OWN_FILL)
			{
				SaveOldValues(ref form);
				OLD_INDEX = form.lbEntries.SelectedIndex;
			}

			Entry e = (Entry)route.Entrys[form.lbEntries.SelectedIndex];

			if (e.isLast)
			{
				form.tbOpsName.Text = e.m_ops_name+" (Letzte Betriebsstelle)";
			}
			else
			{
				form.tbOpsName.Text = e.m_ops_name;
			}
			
			form.tbArr.Text = e.m_eta;
			form.tbDep.Text = e.m_etd;
			form.cb_LastEntry.Checked = e.isLast;

//			form.lEntryCounter.Text = "Eintrag: " + (form.lbEntries.SelectedIndex+1).ToString() + " von " + form.lbEntries.Items.Count.ToString();
		}
		
		public void UpdateTrackForm(ref TrackForm form)
		{
			bool OWN_FILL_2 = false;
			if (OWN_FILL) OWN_FILL_2 = true;;
			OWN_FILL = true;
			form.tbName.Text = route.Name;
			form.tbDescription.Text = route.Description;
			form.tbMbr.Text = route.mbr;
			if (route.track_side == "left")
			{
				form.cbTrack.SelectedIndex = 1;
			}
			if (route.track_gnt == "yes")
			{
				form.cbGNT.SelectedIndex = 1;
			}
			if (route.one_track)
			{
				form.cbOne_track.SelectedIndex = 1;
			}
			else
			{
				form.cbOne_track.SelectedIndex = 0;
			}
			OWN_FILL = OWN_FILL_2;
		} 	

		public void UpdateTrackEntries(ref TrackForm form)
		{
			if (form.lbEntries.SelectedIndex < 0 || form.lbEntries.SelectedIndex >= route.Entrys.Count)
				return;

			if (!OWN_FILL)
			{
				SaveOldValues(ref form);
				OLD_INDEX = form.lbEntries.SelectedIndex;
			}

			Entry e = (Entry)route.Entrys[form.lbEntries.SelectedIndex];

			form.tbSpeed.Text = e.m_speed;
			form.tbPos.Text = e.m_position;
			form.tbOps.Text = e.m_ops_name;
			form.tbSig.Text = e.m_ops_speed;

			if (e.zack == 1)
				form.rbSingle.Checked = true;
			else if (e.zack == 2)
				form.rbDouble.Checked = true;
			else form.rbNone.Checked = true;
			form.cbTunnel.Checked = e.tunnel;

			form.lEntryCounter.Text = "Eintrag: " + (form.lbEntries.SelectedIndex+1).ToString() + " von " + form.lbEntries.Items.Count.ToString();
		}

		public void SaveOldValues(ref TrackForm form)
		{
			Entry e = (Entry)route.Entrys[OLD_INDEX];

			int where = form.tbOps.Text.IndexOf(" (Letzte Betriebstelle)");

			if (where > 0) form.tbOps.Text.Remove(where, 23);

			e.m_ops_name = form.tbOps.Text;
			e.m_ops_speed = form.tbSig.Text;
			e.m_position = form.tbPos.Text;
			e.m_speed = form.tbSpeed.Text;
			e.tunnel = form.cbTunnel.Checked;

            if (form.rbNone.Checked) e.zack = 0;
			else if (form.rbSingle.Checked) e.zack = 1;
			else e.zack = 2;

			OWN_FILL = true;
			int index = form.lbEntries.SelectedIndex;
			AddEntriesToTrack(ref form);
			form.lbEntries.SelectedIndex = index;
			OWN_FILL = false;
		}

		public void SaveOldValues(ref TrainForm form)
		{
			Entry e = (Entry)route.Entrys[OLD_INDEX];

			e.m_eta = form.tbArr.Text;
			e.m_etd = form.tbDep.Text;
			e.isLast = form.cb_LastEntry.Checked;

			OWN_FILL = true;
			int index = form.lbEntries.SelectedIndex;
			AddEntriesToTrain(ref form);
			form.lbEntries.SelectedIndex = index;
			OWN_FILL = false;
		}

		public void UpdateDesc(TrackForm form)
		{
			if (OWN_FILL) return;
			route.Description = form.tbDescription.Text;
			route.Name = form.tbName.Text;
			route.mbr = form.tbMbr.Text;
			if (form.cbTrack.Text == "Gegengleis")
				route.track_side = "left";
			else route.track_side = "right";

			if (form.cbGNT.Text == "GNT")
				route.track_gnt = "yes";
			else route.track_gnt = "no";

			if (form.cbOne_track.Text == "ja")
				route.one_track = true;
			else route.one_track = false;
		}

		public void UpdateDesc(TrainForm form)
		{
			if (OWN_FILL) return;
			route.trackname = form.tbName.Text;
			route.traintype = form.tbTrainType.Text;
			route.trainnumber = form.tbTrainNumber.Text;
			route.schedule_valid = form.tbValidSpan.Text;
			route.schedule_date = form.tbValidSince.Text;
			route.waypoints = form.tbWaypoints.Text;
			route.Vmax = form.tbVmax.Text;
			route.tfz = form.tbTfz.Text;
			route.Description = form.tbDays.Text;
			route.from = form.tbBegin.Text;
		}
        
		public void AddLine(ref TrackForm form)
		{
			Entry e = new Entry(EntryType.OPS_MARKER, "", "", "", "", "", "", new char(), "", new char());

			int index = form.lbEntries.SelectedIndex;
			route.Entrys.Insert(form.lbEntries.SelectedIndex, e);
			OWN_FILL = true;
			AddEntriesToTrack(ref form);
			form.lbEntries.SelectedIndex = index;

			OWN_FILL = false;
		}

		public void DeleteLine(ref TrackForm form)
		{
			int index =	form.lbEntries.SelectedIndex;

			if (index < 0)
			{
				System.Windows.Forms.MessageBox.Show("Bitte zuerst eine Zeiel markieren!");
				return;
			}

			route.Entrys.RemoveAt(form.lbEntries.SelectedIndex);

			OWN_FILL = true;
			AddEntriesToTrack(ref form, index);
			OWN_FILL = false;
		}

		public string GetTrackName(string trainfilename)
		{
			string file = "";

			if (System.IO.File.Exists(trainfilename))
			{
				XmlReader reader = new XmlTextReader(trainfilename);				// create XmlDocument				XmlDocument doc = new XmlDocument();				try				{					// ignore xml doctype + header					reader.MoveToContent();					doc.Load(reader);					// create root node					XmlNode root = doc.DocumentElement;

					foreach(XmlAttribute att in root.Attributes)
					{
						if (att.Name == "trackname")
						{
							file = att.Value;
						}
					}

					trainfilename = Path.GetDirectoryName(trainfilename);
					int index = trainfilename.IndexOf("EBuLa");

					trainfilename = trainfilename.Remove(index, trainfilename.Length - index);
					trainfilename += "track_" + file + "_right.xml";

				}
				catch (Exception e) 
				{
					System.Windows.Forms.MessageBox.Show("Fehler beim lesen der Datei! "+e.Message);
				}
			}
			else 
			{
				System.Windows.Forms.MessageBox.Show("Datei nicht gefunden!");
			}

			return trainfilename;
		}
	}
}
