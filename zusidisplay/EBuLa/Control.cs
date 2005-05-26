using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MMI.EBuLa												 
{
	public class Control
	{
		#region Members
		private Entry helper = null;

		public DateTime vtime;

		Route route = null;
		Entry marker = null;

		public DateTime date_buffer = new DateTime(0);

		public int verspaetung = 0;

		string current_speed = "";
		string current_radio = "";

		public MMI.EBuLa.Tools.XMLLoader XMLConf;

		public bool gnt = false;
		public bool left = false;
		public bool use_DB = false;
		public bool timer_on = false;
		public bool move_via_time = false;
		public bool use_zusi_time = true;
		public bool timer_disabled = false;
		public bool use_network = false;
		public bool inverse = false;
		public bool sound = false;

		public TrainInfo current_Train = null;

		public Hashtable track_ht = new Hashtable();


		string sched_name = "";

		public bool keysarenumbers = false;

		public string buffer_trainnumber = "";
		public string buffer_traintype = "";
		public string buffer_trainschedule = "";
		public string buffer_trainpath = "";
		public string buffer_trackname = "";
		public string buffer_vmax = "";

		public Hashtable train_list = new Hashtable();

		public bool addtionalhours = false;


		public string m_path = "";
		public string m_filename = "";

		public bool searchInTrackPath = false;
		#endregion

		public Control(int vers, MMI.EBuLa.Tools.XMLLoader xc)
		{
			verspaetung = vers;
			XMLConf = xc;
			LoadProps();
		}


		public void ResetMarker(string old_bs)
		{
			bool old_bs_is_there = false;
			for(int i = 0; i < route.Entrys.Count; i++)
			{
				if (((Entry)route.Entrys[i]).m_ops_name == old_bs)
				{
					old_bs_is_there = true;
				}				
				 
			}

			if (old_bs_is_there)
			{
				route.Position = 0;
				while (((Entry)route.Entrys[(int)route.Position]).m_ops_name != old_bs)
				{
					NextEntry(false);
				}
			}
		}

		public void LoadTrainDEBUG()
		{
			char c = new char();

			gnt = false;

			route = new Route("3313", "ab Saarbr�cken Hbf 14.12.03 - 12.06.04 (Sa) (au�er 25.12.03, 26.12.03, 31.12.03, 01.01.04, 10.02.04, 01.05.04, 26.05.04, 27.05.04)");
            
			Entry e1 = new Entry(EntryType.RADIO_MARKER, "141,9", "60", "60", "- ZF A 66 -", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "141,9", "", "", "Bft Saarbr�cken Hbf", "", "13:04", '0', "13:04", '0');
			route.Entrys.Add(e1);
			/*marker = e1;
			route.Position = 1;*/
			marker = null;
			route.Position = -1;

			gnt = true;

			e1 = new Entry(EntryType.OPS_MARKER, "141,8", "", "", "Asig", "A60", "", c, "", c);
			route.Entrys.Add(e1);

			e1 = new Entry(EntryType.OPS_MARKER, "140,8", "", "", "Abzw Saarbr�cken Hbf Srg", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.GNT_BEGINNING, "140,6", "90", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "138,4", "110", "", "J�gersfreude Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "137,1", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "136,3", "", "", "Dudweiler", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "136,0", "90", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "135,9", "", "", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "133,9", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "133,8", "", "100", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "133,6", "110", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "133,1", "", "", "Sulzbach (Saar)", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "132,9", "", "", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "130,9", "", "", "Sulzbach (Saar) Altenwald Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "130,3", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "129,6", "", "", "Friedrichsthal (Saar)", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "129,4", "90", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "129,2", "", "", "Asig", "A50", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "128,8", "", "130", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "128,7", "", "", "Friedr. Mitte Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.TUNNEL_BEGINNING, "128,4", "", "", "Bildstock-T", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "128,2", "", "", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.POSITION_JUMP, "+0,3", "", "", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.TUNNEL_ENDING, "127,9", "", "", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "127,8", "100", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "127,5", "", "", "Bildstock Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "125,9", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "125,6", "", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "124,9", "", "", "Landsweiler-Reden", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "124,5", "", "", "Asig", "A50", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "123,4", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "122,0", "", "", "Abzw Neunkirchen", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "", "", "", "(Saar) Hbf", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "121,8", "", "", "Zsig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "121,8", "70", "70", "Zsig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "120,8", "", "80", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.RADIO_MARKER, "120,6", "", "", "- ZF A 73 -", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "120,6", "", "", "Neunkirchen", "", "13:18", '0', "13:19", '0');
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "", "", "", "(Saar) Hbf", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "120,5", "", "", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "120,2", "90", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "118,5", "", "", "Wiebelskirchen Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "115,7", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "114,8", "", "", "Ottweiler (Saar)", "", "13:23", '0', "13:23", '5');
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "114,6", "", "", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "114,5", "", "100", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "114,3", "", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "113,9", "100", "130", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "111,6", "", "", "Sbk 34", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "113,3", "110", "140", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "113,2", "", "", "Niederlinxweiler Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "106,8", "", "", "Oberlinxweiler Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "107,6", "", "", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "106,3", "", "", "St Wendel", "", "13:29", '0', "13:30", '0');
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "106,2", "90", "130", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "105,7", "100", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "103,1", "", "100", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "103,0", "", "", "Baltersweiler Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "103,0", "", "", "Sbk 30", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "102,5", "", "110", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "101,9", "", "", "El 1", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "101,8", "", "", "El 2", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "100,9", "", "", "Hofeld Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "99,0", "", "", "Sbk 28", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "98,8", "", "", "Namborn Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "97,0", "", "100", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "96,4", "110", "120", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "96,0", "", "", "Sbk 26", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "94,4", "", "", "Walhausen (Saar) Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "92,5", "80", "90", "Esig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "91,9", "", "100", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "91,8", "90", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);
			e1 = new Entry(EntryType.RADIO_MARKER, "91,8", "", "", "- ZF A 78 -", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "91,8", "", "", "T�rkism�hle", "", "13:39", '0', "13:39", '5');
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "91,3", "", "", "Asig", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "90,9", "", "110", "", "", "", c, "", c);
			route.Entrys.Add(e1);
			e1 = new Entry(EntryType.OPS_MARKER, "89,5", "", "", "Nohfelden Hp", "", "", c, "", c);
			route.Entrys.Add(e1);
			//e1 = new Entry(EntryType.OPS_MARKER, "88,1", "100", "", "", "", "", c, "", c);
			//route.Entrys.Add(e1);


		}

		public Route Route
		{
			get {return route;}
			set {route = value;}
		}

		public Entry Marker
		{
			get {return marker;}
			set {marker = value;}
		}

		public bool LoadTrain(string number)
		{
			return false;
		}

		public string CurrentSpeed
		{
			get {return current_speed;}
			set {current_speed = value;}
		}

		public string CurrentRadio
		{
			get {return current_radio;}
			set {current_radio = value;}
		}


		private void DeleteDoubles()
		{
			for(int i = 0; i < route.Entrys.Count-1; i++)
			{
				Entry this_e = (Entry)route.Entrys[i];
				Entry next_e = (Entry)route.Entrys[i+1];

				// ohne pos angaben
				/*if (this_e.m_position == "" && i > 0 && !this_e.isJump)
				{
					route.Entrys.Remove(this_e);
					//continue;
				}*/
				
				// weder opsname noch geschw. angaben
				if (this_e.m_position != "" && (this_e.m_ops_name == "" && this_e.m_speed == "" && this_e.m_gnt_speed == "" && !this_e.isJump && !next_e.isJump))
				{
					route.Entrys.Remove(this_e);
				}
			}

			for(int i = 0; i < route.Entrys.Count; i++)
			{
				Entry this_e = (Entry)route.Entrys[i];

				int posSig = this_e.m_ops_name.IndexOf("...");

				if (posSig >= 0)
				{
					string help = this_e.m_ops_name;
					this_e.m_ops_name = help.Substring(0, posSig);
					this_e.m_ops_speed = help.Substring(posSig+3, help.Length - (posSig+3));
				}

				this_e.m_eta = this_e.m_eta.Replace(".", ":");
				this_e.m_etd = this_e.m_etd.Replace(".", ":");

				//LZB Anfang
				int pos = this_e.m_ops_name.IndexOf("@@LZB-Anfang");
				if (pos >= 0)
				{
					this_e.m_type = EntryType.LZB_BEGINNING;
					this_e.m_ops_name = "";
				}

				// LZB Ende
				pos = this_e.m_ops_name.IndexOf("@@LZB-Ende");
				if (pos >= 0)
				{
					this_e.m_type = EntryType.LZB_ENDING;
					this_e.m_ops_name = "";
				}

				pos = this_e.m_ops_name.IndexOf("@@LZB");
				if (pos >= 0)
				{
					this_e.m_type = EntryType.LZB_BEGINNING;
					this_e.m_ops_name = "";
				}

				// Zugfunk
				pos = this_e.m_ops_name.IndexOf("ZF");
				int pos2 = this_e.m_ops_name.IndexOf("ZF-Ende");
				if (pos >= 0)
				{
					if (pos2 > 0)
						this_e.m_type = EntryType.RADIO_MARKER_ENDING;
					else
						this_e.m_type = EntryType.RADIO_MARKER;
					this_e.m_ops_name = "";
				}

				// verk�rtzte Vorsignale, Teil 1
				pos = this_e.m_ops_name.IndexOf("�@@");
				if (pos >= 0)
				{
					this_e.m_type = EntryType.VERKUERTZT;
					this_e.m_ops_name = "";
				}

				// verk�rtzte Vorsignale, Teil 2
				pos = this_e.m_ops_name.IndexOf("�");
				if (pos >= 0)
				{
					this_e.m_type = EntryType.VERKUERTZT;
					this_e.m_ops_name = this_e.m_ops_name.Replace("�", "");
				}

				// E60 usw. verschieben
				if (this_e.m_ops_speed.Length == 0)
				{
					pos = this_e.m_ops_name.IndexOf("E50");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "E50";
					}

					pos = this_e.m_ops_name.IndexOf("Z50");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "Z50";
					}
					
					pos = this_e.m_ops_name.IndexOf("A50");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "A50";
					}

					pos = this_e.m_ops_name.IndexOf("E60");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "E60";
					}

					pos = this_e.m_ops_name.IndexOf("Z60");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "Z60";
					}
					
					pos = this_e.m_ops_name.IndexOf("A60");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "A60";
					}

					pos = this_e.m_ops_name.IndexOf("E70");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "E70";
					}

					pos = this_e.m_ops_name.IndexOf("Z70");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "Z70";
					}
					
					pos = this_e.m_ops_name.IndexOf("A70");
					if (pos >= 0)
					{
						this_e.m_ops_name = this_e.m_ops_name.Remove(pos, 3);
						this_e.m_ops_speed = "A70";
					}
				}

				this_e.m_ops_name.Replace("|", "");

				// REMOVE GNT
				if (gnt)
				{
					if (this_e.m_ops_name == "@@GNT-Anfang")
					{
						this_e.m_type = EntryType.GNT_BEGINNING;
						this_e.m_ops_name = "";
					}
					else if (this_e.m_ops_name == "@@GNT-Ende")
					{
						this_e.m_type = EntryType.GNT_ENDING;
						this_e.m_ops_name = "";
					}
					if (this_e.m_ops_name == "" && this_e.m_position != "" && this_e.m_speed != "" && this_e.m_gnt_speed == "")
					{
						route.Entrys.RemoveRange(i, 1);
						i--;
					}
				}
				else
				{
					if (this_e.m_ops_name == "@@GNT-Anfang" || this_e.m_ops_name == "@@GNT-Ende")
					{
						if (this_e.m_gnt_speed == "" && this_e.m_speed == "")
						{
							route.Entrys.RemoveRange(i, 1);
							i--;
						}
						else
						{
							this_e.m_ops_name = "";
						}
					}

					if (this_e.m_ops_name == "" && this_e.m_position != "" && this_e.m_gnt_speed != "" && this_e.m_speed == "")
					{
						route.Entrys.RemoveRange(i, 1);
						i--;
					}

				}
			}
		}

		private void ParseFile(string filename)
		{
			try 
			{
				string lastline = "<EMPTY>";
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF7)) 
				{
					String line = "";
					// Read and display lines from the file until the end of 
					// the file is reached.
					while ((line = sr.ReadLine()) != null) 
					{
						bool isjump = false;
						Entry entry;
						string[] line_elements;

						if (lastline == "<EMPTY>")
						{
							// erste zeile
							lastline = line;
							line_elements = line.Split(("\t").ToCharArray(), int.MaxValue);
							if (line_elements[0] == "")
								entry = new Entry(EntryType.OPS_MARKER, line_elements[5], line_elements[1], line_elements[3], line_elements[4], "", line_elements[6], new char(), line_elements[7],  new char());
							else
								entry = new Entry(EntryType.OPS_MARKER, line_elements[0], line_elements[1], line_elements[3], line_elements[4], "", line_elements[6], new char(), line_elements[7],  new char());							
							try
							{
								entry.zack = Convert.ToInt32(line_elements[8]);
							}
							catch (Exception) {}
							route.Entrys.Add(entry);
							continue;
						}

						line_elements = line.Split(("\t").ToCharArray(), int.MaxValue);
						string[] lastline_elements = lastline.Split(("\t").ToCharArray(), int.MaxValue);

						// Sprung erkennen
						if  (line_elements[7] == "@@")
						{
							line_elements[7] = "";
                            isjump = true;
						}

						// km Angaben mitnehmen
						if (line_elements[0] == "" && line_elements[2] == "")
						{
							// diese zeile hat keine km angaben -> in vorheriger zeile nachschauen
							if (lastline_elements.Length > 2)
							{
									line_elements[0] = lastline_elements[0];
									lastline_elements[0] = "";
									line_elements[2] = lastline_elements[2];
									lastline_elements[2] = "";
							}
						}

						// nicht 2 positionen in 1 zeile 
						if (line_elements[0] != "" || line_elements[2] != "")
						{
							if (line_elements[5] != "")
							{
								if (line_elements[0] != "")
									entry = new Entry(EntryType.OPS_MARKER, line_elements[0], line_elements[1], line_elements[3], "", "", "", new char(), "",  new char());
								else // (line_elements[2] != "")
									entry = new Entry(EntryType.OPS_MARKER, line_elements[2], line_elements[1], line_elements[3], "", "", "", new char(), "",  new char());

								try
								{
									entry.zack = Convert.ToInt32(line_elements[8]);
								}
								catch (Exception) {}

								route.Entrys.Add(entry);
								line_elements[0] = line_elements[5];
								line_elements[1] = "";
								line_elements[2] = line_elements[5];
								line_elements[3] = "";
							}
							else //(line_element[5] == "")
							{
								if (line_elements[0] == "")
									line_elements[0] = line_elements[2];
							}
						}
						else if (line_elements[0] == "" && line_elements[2] == "")
						{
							if (line_elements[5] != "")
							{
								line_elements[0] = line_elements[5];
								line_elements[2] = line_elements[5];
							}
						}

						entry = new Entry(EntryType.OPS_MARKER, line_elements[0], line_elements[1], line_elements[3], line_elements[4], "", line_elements[6], new char(), line_elements[7],  new char());
						entry.isJump = isjump;

						try
						{
							entry.zack = Convert.ToInt32(line_elements[8]);
						}
						catch (Exception) {}

						route.Entrys.Add(entry);

						lastline = line;
					}   
				}

				DeleteDoubles();

				Console.WriteLine("FILE LOADED!");
			}
			catch (Exception e) 
			{
				// Let the user know what went wrong.
				System.Windows.Forms.MessageBox.Show("Zusi Fahrplan nicht gefunden oder Fehler beim Lesen! ("+e.Message.ToString()+")");
			}

		}

        
		public void ParseDirectory(Label l_top)
		{
			string path = getZusiPath();
			if (path.IndexOf("\\Temp") < 0) path += @"\Temp";
			// Create a reference to the current directory.
			DirectoryInfo di = new DirectoryInfo(path);
			// Create an array representing the files in the current directory.
			FileInfo[] fi = di.GetFiles();
			
			train_list = new Hashtable();
			int counter = 0;
			

			foreach (FileInfo fiTemp in fi)
			{
				counter++;
				//fiTemp.Name
				if (fiTemp.Extension != ".txt") continue;

				// kein zusatzverkehr
				if (fiTemp.Name.ToLower().IndexOf("zusatz") > -1) continue;
				// kein dekoverkehr
				if (fiTemp.Name.ToLower().IndexOf("deko") > -1) continue;
				// kein dummy
				if (fiTemp.Name.ToLower().IndexOf("dummy") > -1) continue;
				// kein abstellung
				if (fiTemp.Name.ToLower().IndexOf("abstell") > -1) continue;
				// keine autos
				if (fiTemp.Name.ToLower().IndexOf("auto") > -1) continue;
				// 
				if (fiTemp.Name.ToLower().IndexOf("aktuellerzug") > -1) continue;

				Application.DoEvents();
				l_top.Text = "Daten werden gelesen!".ToUpper() + " ("+ counter.ToString()+" von " + fi.Length.ToString()+ ")";
				TrainInfo ti = new TrainInfo(fiTemp.Name, fiTemp.DirectoryName, XMLConf.SearchForDepAndArr);
				Application.DoEvents();
				if (ti.Number == "") continue;

				try
				{
					train_list.Add(ti.Number+" "+ti.Type, ti);
					if (ti.ti2 != null) train_list.Add(ti.ti2.Number+" "+ti.ti2.Type, ti.ti2);
				}
				catch(Exception){};
			}
		}

		private string ReadEntry(string strg, EntryPos where, Entry entry, char until)
		{
			string help = strg;
			string jump = "";
			foreach(char c in strg)
			{
				if (char.IsControl(c) || c == until)
				{
					help = help.Remove(0,1);
					if (where == EntryPos.ETD)
					{
						if (c == '@')
						{
							entry.isJump = true;						
							return help;							
						}
						if (c == until)
							return help;
					}
					else
					{
						return help;                    
					}
				}
				else
				{
					switch (where)
					{
						case EntryPos.POS_STD:
							entry.m_position += c.ToString();
							break;
						case EntryPos.POS_GNT:
							entry.m_gnt_speed += c.ToString(); // gnt_speed is ok
							break;
						case EntryPos.SPEED_STD:
							entry.m_speed += c.ToString();
							break;
						case EntryPos.SPEED_GNT:
							entry.m_gnt_speed += c.ToString();
							break;
						case EntryPos.OPS_NAME:
							entry.m_ops_name += c.ToString();
							break;
						case EntryPos.OPS_APP:
							entry.m_ops_speed += c.ToString();
							break;
						case EntryPos.POS:
							entry.m_position += c.ToString();
							break;
						case EntryPos.ETA:
							if (c.ToString() == ".")
							{
								entry.m_eta += ":";
							}
							
							else
							{
								entry.m_eta += c.ToString();
							}
							break;
						case EntryPos.ETD:
							if (c.ToString() == ".")
							{
								entry.m_etd += ":";
							}
							else
							{
								entry.m_etd += c.ToString();
							}
							break;
						case EntryPos.SL:
							if (c == '2')
							{
								entry.zack = 2;
							}
							else if (c == '1')
							{
								entry.zack = 1;
							}
							else
							{
								entry.zack = 0;
							}
							break;
					}
					//entry.m_ops_name += c.ToString();
					help = help.Remove(0, 1);
				}
			}
			return "";
		}

        
		public bool RouteHasGNT()
		{
			foreach(Entry e in route.Entrys)
			{
				if (e.m_speed != "" && e.m_gnt_speed != "")
				{
					if (e.m_gnt_speed != e.m_speed) return true;
				}
			}
			return false;
		}

		public void LoadTimeTableFromZusi()
		{
			string file = "";
			string path = m_path;
			string trainnumber = "";
			string traintype = "";
			RegistryKey rk = null;

			rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi");
			if (rk == null)
			{
				if (XMLConf.Path=="") 
				{
					MessageBox.Show("Weder Zusi noch Pfadangabe gefunden!");
				}
				else
				{
					path = XMLConf.Path+"\\Temp\\";
				}
			}

			if (buffer_trainnumber != "" /*&& buffer_traintype != ""*/)
			{
				trainnumber = buffer_trainnumber;
				traintype = buffer_traintype;
				if (rk != null)
				{
					path = rk.GetValue("ZusiDir").ToString();
					//if (!use_DB) path += "\\Temp\\";
					if (path.ToLower().IndexOf("path") < 0) path += "\\Temp\\";

				}
				else if (path == m_path)
				{
					MessageBox.Show("FEHLER! Zusi nicht in Registry gefunden!");
					return;
				}
			}
			/*
			else
			{
				if (rk != null) 
				{
					if (path == "")
					{
						path = rk.GetValue("ZusiDir").ToString();
						if (!use_DB) path += "\\Temp\\";
					}
					else
					{
						path = path+"\\";
					}
					trainnumber = rk.OpenSubKey("Zusi").GetValue("Zugnummer").ToString();
					traintype = rk.OpenSubKey("Zusi").GetValue("Gattung").ToString();
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("Zusi nicht gefunden! (Dieser Fehler d�rfte nie auftreten)");
					return;
				}
			}
			*/

			int bb = trainnumber.IndexOf("/");                
			if (bb == -1) bb = trainnumber.IndexOf("+");
			if (bb != -1)
			{
				trainnumber = trainnumber.Remove(bb,1);
			} 
			if (trainnumber[trainnumber.Length-1] == 'e')
			{
				trainnumber = trainnumber.Remove(trainnumber.Length-1,1);
			}
            
			m_filename = XMLConf.File;

			//file = m_filename;
			//path = m_path;

			//System.Console.WriteLine("Path: "+path+" MPath:"+m_path+"  File: "+file+" MFilename:"+m_filename+" Type: "+traintype+trainnumber+".txt");

			string [] files_org = new string[0];
			
			/*if (XMLConf.UseDB)
			{
				string schedule = "";
				string trackName =	"";
				
				buffer_trackname = (string)track_ht[traintype+trainnumber];

				trackName = buffer_trainpath + @"\" + buffer_trackname + ".str";//getTrackName();

				Route help_route = XMLReader.ReadTackFromDB(trackName, !left, gnt, false);

				if (help_route != null)
				{
					route = help_route;
				}
				else
				{
					route = null;
					return;
				}

				schedule = buffer_trainpath;

				schedule += @"\EBuLa\train_"+buffer_trainschedule+"_"+traintype+trainnumber+".xml";

				route = XMLReader.AddTimetableToRoute(route, schedule);

				route = XMLReader.RemoveHighSpeed(route, this);

			}
			else // no useDB*/
			{
				if (m_filename == "")
				{
					try
					{
						if (searchInTrackPath)
						{
							files_org = System.IO.Directory.GetFiles(getTrackPath(), "*"+getTrainName()+".txt");
						}
						else
						{
							//files_org  = System.IO.Directory.GetFiles(path, "*"+traintype+trainnumber+".txt");
							files_org  = System.IO.Directory.GetFiles(path, traintype+trainnumber+".txt");
							if (files_org.Length < 1 && current_Train != null) 
								files_org  = System.IO.Directory.GetFiles(path, current_Train.File);
						}
					}
					catch (Exception e)
					{
						MessageBox.Show("Datei nicht im Pfad: "+path+" gefunden! ("+e.Message.ToString()+")");
					}
				}
				else
				{
					try
					{
						files_org  = System.IO.Directory.GetFiles(path, "*"+m_filename);
					}
					catch (Exception e)
					{
						MessageBox.Show("Datei ("+m_filename+") nicht im Pfad: "+path+" gefunden! ("+e.Message.ToString()+")");
					}
				}

				bb = trainnumber.IndexOf("_");
				if ( bb != -1 )
				{
					trainnumber = trainnumber.Remove(bb,1);
				} 


				if (files_org.Length > 1)
				{
					System.Windows.Forms.MessageBox.Show("Zu viele Dateien zur Zugnummer gefunden! Zusi Temp Ordner l�schen!");
					return;
				}
				if (files_org.Length < 1)
				{
					System.Windows.Forms.MessageBox.Show("Keine Datei zur Zugnummer gefunden!");
					return;
				}
				file = files_org[0];

				// <DEBUG>
				foreach (string s in files_org)
				{
					//Console.WriteLine(s);
				}
				//Console.WriteLine("File found: " + file);
				// </DEBUG> */
				/*else // gotfilename != null
				{
					file = gotfilename;
					trainnumber = System.IO.Path.GetFileNameWithoutExtension(file);
					trainnumber = RemoveChars(trainnumber);
				}*/

				route = new Route(trainnumber.ToString(), "EMTPY");
				ParseFile(file);
			}	  

			if (route == null)
			{
				System.Windows.Forms.MessageBox.Show("Fehler! Fahrplan nicht gefunden!");
				m_filename = "";
				path = "";
				return;
			}

			route.Position = -1;
			Marker = null;
			//gnt = false;
			//if (route.Entrys.Count > 0) Marker = (Entry)route.Entrys[(int)route.Position];
			route.setVmax();
			m_filename = "";
			path = "";
		}

		public string getSchedule(string s)
		{
			//(...)\FiktiverTaktfahrplan_2_2_RB12052
			//(...)\Fahrplan_normal_S9672_9679
			string h = System.IO.Path.GetFileNameWithoutExtension(s);

			bool search_for_underline = false;
			int offset = -1;

			for (int i = h.Length-1; i >= 0; i--)
			{
				if (!search_for_underline && char.IsLetter(h[i]))
				{
					search_for_underline = true;
					continue;
				}
				if (search_for_underline && h[i].ToString() == "_")
				{
					offset = i;
					break;
				}
			}

			//int offset = h.IndexOf("_",0);
			if (offset >= 0) h = h.Substring(0, offset);
			return h;
		}

		public void NextEntry()
		{
            NextEntry(true);
		}

		public void NextEntry(bool only_timed_entries)
		{
			if (route.Entrys.Count < 1) return;
			Entry e = null;
			do
			{
				if ((int)route.Position >= route.Entrys.Count-1)
				{
					if ((int)route.Position == route.Entrys.Count-1)
					{
						marker = (Entry)route.Entrys[(int)route.Position];
					}
					break;
				}
				e = (Entry)route.Entrys[(int)route.Position+1];
				if (e.m_type == EntryType.OPS_MARKER && e.m_ops_name != "" && ((e.m_etd != "" || !only_timed_entries) || e.m_eta != "" ))
				{
					route.Position++;
					marker = e;

					int counter = 0;

					// now 11 instead of 12
					while(route.Position - (long)route.Offset >= 03 )
					{
						if (counter > 10)
						{
							//MessageBox.Show("Next Page lief mehr als 10 mal?");
							break;
						}
						counter++;
						NextPage_Buttom();
					}
					return;
				}
				else
				{
					route.Position++;
				}

			}
			while((int)route.Position <= route.Entrys.Count);
		}

		public void PrevEntry()
		{
			Entry e = null;
			int help = (int)route.Position;
			do
			{
				if ((help == 0) || (route.Entrys.Count <= 0) || route.Position < 0)
				{
					return;
				}
				e = (Entry)route.Entrys[help-1];
				if (e.m_type == EntryType.OPS_MARKER && e.m_ops_name != "")
				{
					marker = e;
					route.Position = (long)help - 1;
					// TODO
					while(route.Position - (long)route.Offset < 0 )
					{
						PrevPage();
					}
					return;
				}
				else
				{
					help--;
				}

			}
			while((int)route.Position >= 0);
		}

		public void NextPage()
		{
			NextPage(11);
		}

		public void NextPage(int shiftvalue)
		{
			// now shiftvalue instead of 12
			if ((int)route.Offset < (route.Entrys.Count-shiftvalue-13))
			{
				route.Offset += (ulong)shiftvalue;
			}
			else if ((int)route.Offset < (route.Entrys.Count-shiftvalue))
			{
				route.Offset += (ulong)( Math.Max( ((int)route.Entrys.Count - (int)route.Offset - 12) , 0));
			}
		}

		public void NextPage_Buttom()
		{
			NextPage(Convert.ToInt32(route.Position) - Convert.ToInt32(route.Offset) - 3);
		}

		public void PrevPage()
		{
			// now 11 instead of 12
			if (route.Offset > 11)
			{
				route.Offset -= 11;
			}
			else
			{
				route.Offset = 0;
			}
		}

		public void MoveViaTime(System.DateTime vtime, int verspaetung)
		{
			if (route.Position < 0) return;
			
			System.DateTime mtime = vtime.AddMinutes(-verspaetung);

			System.DateTime date = new DateTime(1);

			for(int i = 0; i < route.Entrys.Count-1; i++)
			{
				// fetch the current position with time
				if (i < (int)route.Position) continue;
				Entry e = (Entry)route.Entrys[i];
				
				if (e.m_eta == "" && e.m_etd == "") continue;

				// prefer the Depature Time
				if (e.m_etd != "")
				{
					date = DateTime.Parse(vtime.Date.ToShortDateString() +" "+ e.m_etd);
				}
				else
				{
					date = DateTime.Parse(vtime.Date.ToShortDateString() +" "+ e.m_eta);
				}

				long add = int.MaxValue;

				for (int k = i+1; k < route.Entrys.Count - 2; k++)
				{
					// fetch the next value with time
					DateTime date_next = new DateTime(0);
					Entry e_next = (Entry)route.Entrys[k];
				
					// prefer the Arrival Time
					if (e_next.m_eta == "" && e_next.m_etd == "") continue;

					if (e_next.m_eta != "")
					{
						date_next = DateTime.Parse(vtime.Date.ToShortDateString() +" "+ e_next.m_eta);
					}
					else
					{
						date_next = DateTime.Parse(vtime.Date.ToShortDateString() +" "+ e_next.m_etd);
						
					}

					// calculate half of the distance between
					// this time and the next one
					add = (date_next.Ticks - date.Ticks) / 2;
					//Console.WriteLine("ADD:"+add.ToString());
					break;					
				}
				

				// calculate the time between this entry
				// and the current time and
				// move to the next entry when half of
				// the difference between them is over
				long diff = date.Ticks - mtime.Ticks + add;
				//Console.WriteLine("DIFF (ticks) :"+diff.ToString()+"   ENTRY TIME:"+date.TimeOfDay.ToString()+"  CURRENT TME:"+mtime.TimeOfDay.ToString());

				if (diff < 0) 
				{
					NextEntry();					
				}
				break;
			}
		}

		public void LoadProps()
		{
			if (XMLConf == null)
			{
				return;
			}
			m_filename = XMLConf.File;			m_path = XMLConf.Path;		}

		public string GetFileName()
		{
			if (m_filename == "")			{				return null;			}			else			{				return m_path+m_filename;
			}
		}
        
		public string RemoveChars(string input)
		{
			string output = "";
			foreach(char c in input)
			{
				if (char.IsNumber(c))
				{
					output += c.ToString();
				}
			}
			if (output.Length > 6)
			{
				int bb = output.IndexOf("/");
				if (bb == -1) bb = output.IndexOf("_");
				if (bb == -1) bb = output.IndexOf("+");
				if (bb != -1)
				{
					output = output.Remove(bb,1);
				} 
			}
			return output;
		}


		public DateTime ConvertToDateTime(double input)
		{
			long days = (long)input;
			double time = input - (double)days;

			DateTime dt = new DateTime(0);
			dt = DateTime.Parse("30.12.1899 0:00");

			// date
			while (days > 0)
			{
				dt = dt.AddDays(1);
				days--;
			}
                  
			// time
			long hours = (long)(time * 24);
			long minutes = (long)((time - (double)hours / 24d) * 24d * 60d);
			long seconds = (long)((time - ((double)hours / 24d) - ((double)minutes / 60d / 24d)) * 24d * 60d * 60d);
			dt = dt.AddHours(hours);
			dt = dt.AddMinutes(minutes);
			dt = dt.AddSeconds(seconds);

			return dt;
		}

		public DateTime ConvertSingleToDateTime(float val, int year, int month, int day)
		{
			//Console.WriteLine("TIME: "+val.ToString());
			double valu = (double)val;
			int hour = Convert.ToInt32(Math.Floor(valu));
			double rest = valu - (double)hour;
			
			rest *= 60d;

			int min = Convert.ToInt32(Math.Floor(rest));			  

			rest -= (double)min;

			int sec = Convert.ToInt32( (rest * 60d) ) ;

			if (sec == 60) {min++; sec = 0;}
			if (min == 60) {hour++; min = 0;}
			if (hour == 24) {day++; hour = 0;}

			if (sec < 0) sec = 0;
			if (min < 0) min = 0;
			if (hour < 0) hour = 0;

			if (addtionalhours && hour < 12) hour+=12;

			return new DateTime(year, month, day, hour, min, sec);
		}

		public void SetFocusToZusi()
		{
			SystemTools.System s = new SystemTools.System();
			long ZusiHandler = s.GetZusiHwnd();
			if (ZusiHandler != 0)
			{
				SystemTools.System.SetForegroundWindow(ZusiHandler);
			}
		}

		public string getTrackPath()
		{
			return Path.GetDirectoryName(getTrackName());
		}

		public string getTrackName()
		{
			RegistryKey rk = null;
			string trackpath = "";
			try
			{
				rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi").OpenSubKey("Zusi");
				trackpath = rk.GetValue("Strecke").ToString();
				
			}
			catch (Exception e)
			{
				if (XMLConf != null && XMLConf.Path != null)
				{
					trackpath = XMLConf.Path;
				}
				else
				{
					MessageBox.Show("Zusi nicht in der Registry gefunden! ("+e.Message+")");
					return "";
				}
			}
			return trackpath;
		}
		
		public string getTrainName()
		{
			return Path.GetFileNameWithoutExtension(getTrainPath());
		}
		public string getTrainPath()
		{
			RegistryKey rk = null;
			string trainname = "";
			try
			{
				rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi").OpenSubKey("Zusi");
				trainname = rk.GetValue("Zug").ToString();
			}
			catch (Exception e)
			{
				MessageBox.Show("Zusi nicht in der Registry gefunden! ("+e.Message+")");
				return "";
			}
			return trainname;
		}

		public string getZusiPath()
		{
			RegistryKey rk = null;
			string trainname = "";
			try
			{
				rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi");
				trainname = rk.GetValue("ZusiDir").ToString();
			}
			catch (Exception e)
			{
				return XMLConf.Path;
			}
			return trainname;
		}
	}
}
