using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Threading;
using Microsoft.Win32;

namespace MMI.EBuLa.Tools
{
	public enum FIS_DATA
	{
		Status=1, GPS_Status, Naechster_Halt, Route_Data, Next_Entry, Prev_Entry, 
		FIS_Ausloesen, FIS_Start, FIS_Aus,
		Set_Status, Set_NaechsterHalt, Set_Route_Netz, Set_Route_Linie, 
		Set_Route_Start, Set_Route_Ziel, Set_FISTYPE
	}

	public enum FIS_TYPE
	{
		NONE=1, FIS, GPS_FIS, GPS_ZN_FIS
	}

	public enum FIS_POS
	{
		NONE=1, LINIE, ZIEL, START
	}

	public class Struct_Netz
	{
		public string Netz;
		public string Linie;
		public ArrayList/*<Struct_Start>*/ Start;
		public int Start_ID;
		public string AnsagenPath;
	}                      
	public class Struct_Start
	{
		public string Start;
		public ArrayList/*<string>*/ Ziel;
		public ArrayList/*<string>*/ Verkehrshalte;
		public Hashtable/*<string,string>*/ Position;
		public ArrayList/*<string>*/ Ansagen;
		public int Ziel_ID;
	}

	public class FIS
	{
		public bool Status = false;
		public bool GPSStatus = false;
		public bool Deleted = false;
		public bool UserEnabled = false;

		public string NaechsterHalt = "";
		public string Netz = "";
		public string Linie = "";
		public string Start = "";
		public string Ziel = "";
		public string Zugnummer = "";
		SoundInterface dx;

		public string FahrzielID = "000000";
		public bool ZugtaufeOK = false;

		public ArrayList Netze/*<Struct_Netz>*/ = new ArrayList();		
		public int Netze_ID = 0;
		public int AktuellerHalt = -1;

		public FIS_POS position = FIS_POS.LINIE;

		private Struct_Netz m_Netz;
		private Struct_Start m_Start;

		private MMI.EBuLa.Tools.XMLLoader localconf;

		public FIS(ref MMI.EBuLa.Tools.XMLLoader state, ref SoundInterface sound)
		{
			/*
			NaechsterHalt = "Ostbahnhof";
			Netz = "München";
			Linie = "S1";
			Start = "Ostbahnhof";
			Ziel = "München Flughafen";
			Zugnummer = "99132";*/

			/*
			NaechsterHalt = "Neunkirchen(Saar)Hbf";
			Netz = "Saarland";
			Linie = "RB73";
			Start = "Saarbrücken Hbf";
			Ziel = "Türkismühle";
			Zugnummer = "";*/

			NaechsterHalt = "";
			Netz = "";
			Linie = "";
			Start = "";
			Ziel = "";
			Zugnummer = "";
			dx = sound;

			localconf = state;

			FillFIS();		
		}

		public void CreateNetz(string data)
		{
			m_Netz = new Struct_Netz();
			m_Netz.Netz = data;
			m_Netz.Start = new ArrayList();
		}

		public void SetLinie(string data)
		{
			if (m_Netz != null)	m_Netz.Linie = data;
		}

		public void CreateStart(string data)
		{
			m_Start = new Struct_Start();
			m_Start.Start = data;
			m_Start.Ziel = new ArrayList();
		}

		public void CreateZiel(string data)
		{
			m_Start.Ziel.Add(data);
		}

		public void NoMoreZiel()
		{
			m_Netz.Start.Add(m_Start);
		}

		public void NoMoreStart()
		{
			Netze.Add(m_Netz);
		}

		public void NoMoreData()
		{
			// empty
		}

		public void FillFISOld()
		{             
			Struct_Netz netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S1";
			netz.Netz = "München";

			// von Ostbahnhof
			Struct_Start start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Flughafen");
			start.Ziel.Add("Freising");
			start.Start = "Ostbahnhof";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von Neufahrn
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Flughafen");
			start.Ziel.Add("Freising");
			start.Ziel.Add("Ostbahnhof");
			start.Start = "Neufahrn";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von Flughafen
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Ostbahnhof");
			start.Start = "Flughafen";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von Freising
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Ostbahnhof");
			start.Start = "Freising";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			Netze.Add(netz);

			// NIX
			netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "99";
			netz.Netz = "kein Netz";

			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("nicht Einsteigen");
			start.Start = "nicht Einsteigen";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("nicht Einsteigen");
			start.Ziel.Add("Leerfahrt");
			start.Ziel.Add("Sonderfahrt");
			start.Ziel.Add("Fahrschule");
			start.Start = "Werkstatt";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			Netze.Add(netz);

			netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "73";
			netz.Netz = "Saarland";

			// von SSH
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Türkismühle");
			start.Ziel.Add("St Wendel");
			start.Ziel.Add("Neunkirchen(Saar)Hbf");
			start.Start = "Saarbrücken Hbf";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von SNK
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Saarbrücken Hbf");
			start.Ziel.Add("St Wendel");
			start.Ziel.Add("Türkismühle");
			start.Start = "Neunkirchen(Saar)Hbf";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von SSWD
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Neunkirchen(Saar)Hbf");
			start.Ziel.Add("Saarbrücken Hbf");
			start.Start = "St Wendel";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			// von STKM
			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Neunkirchen(Saar)Hbf");
			start.Ziel.Add("Saarbrücken Hbf");
			start.Ziel.Add("St Wendel");
			start.Start = "Türkismühle";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			Netze.Add(netz);
		}

		public void NextFISPos(IFISNetwork net, bool Connected)
		{
			switch(position)
			{
				case FIS_POS.LINIE:
					position = FIS_POS.START;
					break;
				case FIS_POS.START:
					position = FIS_POS.ZIEL;
					break;
				case FIS_POS.ZIEL:
					position = FIS_POS.NONE;
					SetUp(net, Connected);
					AktuellerHalt = -1;
					break;
			}
		}

		public void PrevFISPos()
		{
			switch(position)
			{
				case FIS_POS.LINIE:
					position = FIS_POS.NONE;
					break;
				case FIS_POS.START:
					position = FIS_POS.LINIE;
					break;
				case FIS_POS.ZIEL:
					position = FIS_POS.START;
					break;
			}
		}

		public void NextEntry()
		{
			switch(position)
			{
				case FIS_POS.LINIE:
					Netze_ID++;
					if (Netze_ID >= Netze.Count) Netze_ID = 0;
					break;

				case FIS_POS.START:
					((Struct_Netz)Netze[Netze_ID]).Start_ID++;
					if (((Struct_Netz)Netze[Netze_ID]).Start_ID >= ((Struct_Netz)Netze[Netze_ID]).Start.Count) ((Struct_Netz)Netze[Netze_ID]).Start_ID = 0;
					break;

				case FIS_POS.ZIEL:
					((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID++;
					if (((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID >= ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel.Count) ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID = 0;
					break;
			}
		}

		public void PrevEntry()
		{
			switch(position)
			{
				case FIS_POS.LINIE:
					Netze_ID--;
					if (Netze_ID < 0) Netze_ID = Netze.Count-1;
					break;

				case FIS_POS.START:
					((Struct_Netz)Netze[Netze_ID]).Start_ID--;
					if (((Struct_Netz)Netze[Netze_ID]).Start_ID < 0) ((Struct_Netz)Netze[Netze_ID]).Start_ID = ((Struct_Netz)Netze[Netze_ID]).Start.Count - 1;
					break;

				case FIS_POS.ZIEL:
					((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID--;
					if (((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID < 0) ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID = ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel.Count - 1;
					break;
			}
		}
		public string GetNetz()
		{
			try
			{
				return ((Struct_Netz)Netze[Netze_ID]).Netz;
			}
			catch(Exception)
			{
				return "                 ---";
			}
		}

		public string GetLinie()
		{
			try
			{
				return ((Struct_Netz)Netze[Netze_ID]).Linie;
			}
			catch(Exception)
			{
				return "                 ---";
			}
		}

		public string GetStart()
		{
			try
			{
				return ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Start;
			}
			catch(Exception)
			{
				return "                 ---";
			}
		}

		public string GetZiel()
		{
			try
			{
				return (string)((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel[((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID];
			}
			catch(Exception)
			{
				return "                 ---";
			}
		}

		public void Delete()
		{
			FISAus();
			FISAn();
			position = FIS_POS.NONE;
			Deleted = true;
		}

		public void SetUp(IFISNetwork net, bool Connected)
		{
			Deleted = false;

			/*if (Connected)
			{
				net.SendData(FIS_DATA.Set_Route_Netz, GetNetz());
				net.SendData(FIS_DATA.Set_Route_Linie, GetLinie());
				net.SendData(FIS_DATA.Set_Route_Start, GetStart());
				net.SendData(FIS_DATA.Set_Route_Ziel, GetZiel());
			}
			else*/
			{
				Linie = GetLinie();
				NaechsterHalt = GetStart();
				Ziel = GetZiel();
				Netz = GetNetz();
			}
		}

		public void SwitchNaechsterHalt()
		{
			string nae_halt = "";
			try
			{
				nae_halt = (string)((Struct_Start)((Struct_Netz)Netze[Netze_ID]).
					Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).
					Verkehrshalte[AktuellerHalt+1];
			}
			catch(Exception){}

			if (nae_halt == "") return;

			
			if (NaechsterHalt == (string)((Struct_Start)((Struct_Netz)Netze[Netze_ID]).
				Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel[((Struct_Start)((Struct_Netz)Netze[Netze_ID]).
				Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID])
			{
				return;
			}
			
			
			NaechsterHalt = nae_halt;

			if ((AktuellerHalt+1) < ((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Verkehrshalte.Count)
				AktuellerHalt++;
		}

		public void SwitchVorherigerHalt()
		{
			string nae_halt = "";
			try
			{
				nae_halt = (string)((Struct_Start)((Struct_Netz)Netze[Netze_ID]).
					Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).
					Verkehrshalte[AktuellerHalt-1];
			}
			catch(Exception){}

			if (nae_halt == "") 
			{
				NaechsterHalt = GetStart();
				return;
			}

			NaechsterHalt = nae_halt;

			if (AktuellerHalt > -1)
			{
				AktuellerHalt--;
			}
		}

		public void PlayNaechsterHalt()
		{
			new Thread(new ThreadStart(PlayNaechsterHaltInternal)).Start();
		}

		private void PlayNaechsterHaltInternal()
		{
			string filename = "";
			string path = "";
			try
			{
					filename = 
						(string)((Struct_Start)((Struct_Netz)Netze[Netze_ID]).
						Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).
						Ansagen[AktuellerHalt];

				path = (string)((Struct_Netz)Netze[Netze_ID]).AnsagenPath;
				if (filename != "" && path != "")
					dx.PlaySoundFromFile(Path.GetFullPath(path)+@"\"+filename);
			}
			catch (Exception) {}
		}
		private void PlayAusstiegLinksInternal()
		{
			string filename = "ausstieg_links.mp3";
			string path = "";
			try
			{
				path = (string)((Struct_Netz)Netze[Netze_ID]).AnsagenPath;
				if (filename != "" && path != "")
					dx.PlaySoundFromFile(Path.GetFullPath(path)+@"\"+filename);
			}
			catch (Exception) {}
		}

		private void PlayAusstiegRechtsInternal()
		{
			string filename = "ausstieg_rechts.mp3";
			string path = "";
			try
			{
				path = (string)((Struct_Netz)Netze[Netze_ID]).AnsagenPath;
				if (filename != "" && path != "")
					dx.PlaySoundFromFile(Path.GetFullPath(path)+@"\"+filename);
			}
			catch (Exception) {}
		}

		public void PlayAusstiegRechts()
		{
			new Thread(new ThreadStart(PlayAusstiegRechtsInternal)).Start();
		}

		public void PlayAusstiegLinks()
		{
			new Thread(new ThreadStart(PlayAusstiegLinksInternal)).Start();
		}

		public void FISAnAus()
		{
			if (Status)
				FISAus();
			else
				FISAn();
		}

		public void FISAus()
		{
			Netz = Linie = Start = Ziel = NaechsterHalt = "";
			Status = false;
		}

		public void FISAn()
		{
			Status = true;
		}

		public bool ShowEntry(bool Connected)
		{
			return ((Status || !Connected) && !Deleted);
		}

	
		public bool SetupFISWithNo()
		{	 
			try
			{
				int netz = Convert.ToInt32(FahrzielID.Substring(0, 2))-1;
				int start = Convert.ToInt32(FahrzielID.Substring(2, 2))-1;
				int ziel = Convert.ToInt32(FahrzielID.Substring(4, 2))-1;

				Netze_ID = netz;
				((Struct_Netz)Netze[Netze_ID]).Start_ID = start;
				((Struct_Start)((Struct_Netz)Netze[Netze_ID]).Start[((Struct_Netz)Netze[Netze_ID]).Start_ID]).Ziel_ID = ziel;
				
				SwitchNaechsterHalt();
				SwitchVorherigerHalt();
				AktuellerHalt = -1;
				return true;
			}
			catch(Exception){return false;}
			
		}

		public void FillFISManual()
		{         
			FillMuenchenStammstrecke();
			FillMuenchenS4S8West();
			FillDüsseldorfS6S7();
			FillSonstiges();
		}

		private void FillSonstiges()
		{
			// NIX
			Struct_Netz netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "99";
			netz.Netz = "kein Netz";

			Struct_Start start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("nicht einsteigen");
			start.Start = "nicht einsteigen";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			start = new Struct_Start();
			start.Ziel = new ArrayList();
			start.Ziel.Add("Leerfahrt");
			start.Ziel.Add("Sonderfahrt");
			//start.Ziel.Add("Fahrschule");
			start.Start = "Werkstatt";
			start.Ziel_ID = 0;

			netz.Start.Add(start);

			Netze.Add(netz);
		}
	
		private void FillMuenchenStammstrecke()
		{
			string Ordner = @"Ansagen\S-Bahn München\S2\";
			Struct_Netz netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S2";
			netz.Netz = "München03";

			// von Holzkirchen (Hbf)
			Struct_Start start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ansagen = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Hauptbahnhof";
			start.Ziel.Add("Petershausen");
			start.Ziel.Add("Röhrmoos");
			start.Ziel.Add("Dachau");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Ansagen.Add(Ordner+"hackerbruecke.mp3");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Ansagen.Add(Ordner+"donnersberger.mp3");
			start.Verkehrshalte.Add("Laim");
			start.Ansagen.Add(Ordner+"laim_frfhm.mp3");
			start.Verkehrshalte.Add("Obermenzing");
			start.Ansagen.Add(Ordner+"obermenzing.mp3");
			start.Verkehrshalte.Add("Allach");
			start.Ansagen.Add(Ordner+"allach.mp3");
			start.Verkehrshalte.Add("Karlsfeld");
			start.Ansagen.Add(Ordner+"karlsfeld.mp3");
			start.Verkehrshalte.Add("Dachau");
			start.Ansagen.Add(Ordner+"dachau.mp3");
			start.Verkehrshalte.Add("Hebertshausen");
			start.Ansagen.Add(Ordner+"hebertshausen.mp3");
			start.Verkehrshalte.Add("Röhrmoos");
			start.Ansagen.Add(Ordner+"roehrmoos.mp3");
			start.Verkehrshalte.Add("Vierkirchen-Esterhofen");
			start.Ansagen.Add(Ordner+"vierkirchen.mp3");
			start.Verkehrshalte.Add("Pertershausen");
			start.Ansagen.Add(Ordner+"petershausen.mp3");
			netz.Start.Add(start);

			// von Petershausen
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Start = "Pertershausen";
			start.Ziel = new ArrayList();
			start.Ziel.Add("Holzkirchen");
			start.Ziel.Add("Deisenhofen");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Vierkirchen-Esterhofen");
			start.Verkehrshalte.Add("Röhrmoos");
			start.Verkehrshalte.Add("Hebertshausen");
			start.Verkehrshalte.Add("Dachau");
			start.Verkehrshalte.Add("Karlsfeld");
			start.Verkehrshalte.Add("Allach");
			start.Verkehrshalte.Add("Obermenzing");
			start.Verkehrshalte.Add("Laim");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Verkehrshalte.Add("Hauptbahnhof");
			netz.Start.Add(start);

			// von Röhrmoss
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Start = "Röhrmoss";
			start.Ziel = new ArrayList();
			start.Ziel.Add("Holzkirchen");
			start.Ziel.Add("Deisenhofen");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Hebertshausen");
			start.Verkehrshalte.Add("Dachau");
			start.Verkehrshalte.Add("Karlsfeld");
			start.Verkehrshalte.Add("Allach");
			start.Verkehrshalte.Add("Obermenzing");
			start.Verkehrshalte.Add("Laim");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Verkehrshalte.Add("Hauptbahnhof");
			netz.Start.Add(start);

			// von Dachau
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Start = "Dachau";
			start.Ziel = new ArrayList();
			start.Ziel.Add("Holzkirchen");
			start.Ziel.Add("Deisenhofen");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Karlsfeld");
			start.Verkehrshalte.Add("Allach");
			start.Verkehrshalte.Add("Obermenzing");
			start.Verkehrshalte.Add("Laim");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Verkehrshalte.Add("Hauptbahnhof");
			netz.Start.Add(start);

			Netze.Add(netz);

			netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S4/S5/S6/S8";
			netz.Netz = "München03";

			// nach Pasing
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Hauptbahnhof";
			start.Ziel.Add("Nannhofen");
			start.Ziel.Add("Geltendorf");
			start.Ziel.Add("Herrsching");			
			start.Ziel.Add("Tutzing");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Verkehrshalte.Add("Laim");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);

			// nach Pasing
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Pasing";
			start.Ziel.Add("Flughafen");
			start.Ziel.Add("Ebersberg");
			start.Ziel.Add("Erding");			
			start.Ziel.Add("Ostbahnhof");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Laim");
			start.Verkehrshalte.Add("Donnersbergerbrücke");
			start.Verkehrshalte.Add("Hackerbrücke");
			start.Verkehrshalte.Add("Hauptbahnhof");
			netz.Start.Add(start);

			Netze.Add(netz);
		}
	
		private void FillMuenchenS4S8West()
		{
			Struct_Netz netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S8";
			netz.Netz = "München03";

			// von Pasing S8
			Struct_Start start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Pasing";
			start.Ziel.Add("Nannhofen");
			start.Ziel.Add("Maisach");
			start.Ziel.Add("Olching");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Langwied");
			start.Verkehrshalte.Add("Lochhausen");
			start.Verkehrshalte.Add("Gröbenzell");
			start.Verkehrshalte.Add("Olching");
			start.Verkehrshalte.Add("Esting");
			start.Verkehrshalte.Add("Gernlinden");
			start.Verkehrshalte.Add("Maisach");
			start.Verkehrshalte.Add("Malching");
			start.Verkehrshalte.Add("Nannhofen");
			netz.Start.Add(start);

			// von Nannhofen S8
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Nannhofen";
			start.Ziel.Add("Pasing");
			start.Ziel.Add("Ostbahnhof");
			start.Ziel.Add("Flughafen");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Malching");
			start.Verkehrshalte.Add("Maisach");
			start.Verkehrshalte.Add("Gernlinden");
			start.Verkehrshalte.Add("Esting");
			start.Verkehrshalte.Add("Olching");
			start.Verkehrshalte.Add("Gröbenzell");
			start.Verkehrshalte.Add("Lochhausen");
			start.Verkehrshalte.Add("Langwied");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);

			// von Maisach S8
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Maisach";
			start.Ziel.Add("Pasing");
			start.Ziel.Add("Ostbahnhof");
			start.Ziel.Add("Flughafen");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Gernlinden");
			start.Verkehrshalte.Add("Esting");
			start.Verkehrshalte.Add("Olching");
			start.Verkehrshalte.Add("Gröbenzell");
			start.Verkehrshalte.Add("Lochhausen");
			start.Verkehrshalte.Add("Langwied");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);

			// von Olching S8
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Olching";
			start.Ziel.Add("Pasing");
			start.Ziel.Add("Ostbahnhof");
			start.Ziel.Add("Flughafen");			
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Gröbenzell");
			start.Verkehrshalte.Add("Lochhausen");
			start.Verkehrshalte.Add("Langwied");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);			
			Netze.Add(netz);

			netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S4";
			netz.Netz = "München03";

			// von Pasing S4
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Pasing";
			start.Ziel.Add("Geltendorf");
			start.Ziel.Add("Grafrath");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Leienfelsstraße");
			start.Verkehrshalte.Add("Aubing");
			start.Verkehrshalte.Add("Puchhheim");
			start.Verkehrshalte.Add("Eichenau");
			start.Verkehrshalte.Add("Fürstenfeldbruck");
			start.Verkehrshalte.Add("Buchenau");
			start.Verkehrshalte.Add("Schöngeising");
			start.Verkehrshalte.Add("Grafrath");
			start.Verkehrshalte.Add("Türkenfeld");
			start.Verkehrshalte.Add("Geltendorf");
			netz.Start.Add(start);

			// von Geltendorf S4
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Geltendorf";
			start.Ziel.Add("Ostbahnhof");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Türkenfeld");
			start.Verkehrshalte.Add("Grafrath");
			start.Verkehrshalte.Add("Schöngeising");
			start.Verkehrshalte.Add("Buchenau");
			start.Verkehrshalte.Add("Fürstenfeldbruck");
			start.Verkehrshalte.Add("Eichenau");
			start.Verkehrshalte.Add("Puchhheim");
			start.Verkehrshalte.Add("Aubing");
			start.Verkehrshalte.Add("Leienfelsstraße");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);

			// von Grafrath S4
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Grafrath";
			start.Ziel.Add("Ostbahnhof");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Schöngeising");
			start.Verkehrshalte.Add("Buchenau");
			start.Verkehrshalte.Add("Fürstenfeldbruck");
			start.Verkehrshalte.Add("Eichenau");
			start.Verkehrshalte.Add("Puchhheim");
			start.Verkehrshalte.Add("Aubing");
			start.Verkehrshalte.Add("Leienfelsstraße");
			start.Verkehrshalte.Add("Pasing");
			netz.Start.Add(start);
			Netze.Add(netz);
		}

		private void FillDüsseldorfS6S7()
		{
			Struct_Netz netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S6";
			netz.Netz = "NRW";

			// von Essen S6
			Struct_Start start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Düsseldorf Hbf";
			start.Ziel.Add("Langenfeld(Rheinl)");
			start.Ziel.Add("Köln Ehrenfeld");			
			start.Ziel.Add("Köln Nippes");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("D-Volksgarten");
			start.Verkehrshalte.Add("D-Oberbilk");
			start.Verkehrshalte.Add("D-Eller Süd");
			start.Verkehrshalte.Add("D-Reisholz");
			start.Verkehrshalte.Add("D-Benrath");
			start.Verkehrshalte.Add("D-Garath");
			start.Verkehrshalte.Add("D-Hellerhof");
			start.Verkehrshalte.Add("Langefeld-Berghausen");
			start.Verkehrshalte.Add("Langenfeld(Rheinl)");
			start.Verkehrshalte.Add("Lev-Rheindorf");
			start.Verkehrshalte.Add("Lev-Küppersteg");
			start.Verkehrshalte.Add("Leverkusen Mitte");
			netz.Start.Add(start);

			// von Lev S6
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Leverkusen Mitte";
			start.Ziel.Add("Essen Hbf");
			start.Ziel.Add("Ratingen Ost");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Lev-Küppersteg");
			start.Verkehrshalte.Add("Lev-Rheindorf");
			start.Verkehrshalte.Add("Langenfeld(Rheinl)");
			start.Verkehrshalte.Add("Langefeld-Berghausen");
			start.Verkehrshalte.Add("D-Hellerhof");
			start.Verkehrshalte.Add("D-Garath");
			start.Verkehrshalte.Add("D-Benrath");
			start.Verkehrshalte.Add("D-Reisholz");
			start.Verkehrshalte.Add("D-Eller Süd");
			start.Verkehrshalte.Add("D-Oberbilk");
			start.Verkehrshalte.Add("D-Volksgarten");
			start.Verkehrshalte.Add("Düsseldorf Hbf");			
			netz.Start.Add(start);

			// von Langenfeld S6
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Langenfeld(Rheinl)";
			start.Ziel.Add("Essen Hbf");
			start.Ziel.Add("Ratingen Ost");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("Langefeld-Berghausen");
			start.Verkehrshalte.Add("D-Hellerhof");
			start.Verkehrshalte.Add("D-Garath");
			start.Verkehrshalte.Add("D-Benrath");
			start.Verkehrshalte.Add("D-Reisholz");
			start.Verkehrshalte.Add("D-Eller Süd");
			start.Verkehrshalte.Add("D-Oberbilk");
			start.Verkehrshalte.Add("D-Volksgarten");
			start.Verkehrshalte.Add("Düsseldorf Hbf");			
			netz.Start.Add(start);
			Netze.Add(netz);

			netz = new Struct_Netz();
			netz.Start = new ArrayList();
			netz.Start_ID = 0;
			netz.Linie = "S7";
			netz.Netz = "NRW";

			// von D-Flughafen S7
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Düsseldorf Hbf";
			start.Ziel.Add("Sohlingen-Oligs");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("D-Volksgarten");
			start.Verkehrshalte.Add("D-Oberbilk");
			start.Verkehrshalte.Add("D-Eller Mitte");
			start.Verkehrshalte.Add("Düsseldorf Eller");
			netz.Start.Add(start);

			// von SG-Oligs S7
			start = new Struct_Start();
			start.Verkehrshalte = new ArrayList();
			start.Ziel = new ArrayList();
			start.Start = "Düsseldorf Eller";
			start.Ziel.Add("Düsseldorf Flughafen");
			start.Ziel_ID = 0;
			start.Verkehrshalte.Add("D-Eller Mitte");
			start.Verkehrshalte.Add("D-Oberbilk");
			start.Verkehrshalte.Add("D-Volksgarten");
			start.Verkehrshalte.Add("Düsseldorf Hbf");
			
			netz.Start.Add(start);
			Netze.Add(netz);
		}

		
		private void FillFIS()
		{
			string dir = "";

			try
			{
				RegistryKey rk = null;

				rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi");
				if (rk == null)
				{
					dir = localconf.Path;
				}
				else
				{
					dir = rk.GetValue("ZusiDir").ToString();
				}
			}
			catch(Exception){}

			if (dir == "") return;

			dir += @"\Ansagen";

			FillSonstiges();
			FillFISFromDirectories(dir);
			
		}

		private void FillFISFromDirectories(string dir)
		{
			foreach(string directory in GetDirectoriesFromPath(dir))
			{
				foreach(string file in GetFilesFromPath(directory))
				{
					FillFISFromXml(file);
				}
				FillFISFromDirectories(directory);
			}
		}

		private string[] GetDirectoriesFromPath(string path)
		{
			return Directory.GetDirectories(path);
		}

		private string[] GetFilesFromPath(string path)
		{
			return Directory.GetFiles(path, "*.xml");
		}

		
		private void FillFISFromXml(string fileName)
		{
			if (fileName == null || fileName == "") return;

			// store the full path if "filename"
			string localpath = Path.GetDirectoryName(Path.GetFullPath(fileName));

			// create a xml text reader
			XmlTextReader reader = new XmlTextReader(fileName);

			// create XmlDocument
			XmlDocument doc = new XmlDocument();

			try
			{
				// ignore xml doctype + header
				reader.MoveToContent();
				// assign the reader to the xml document "doc"
				doc.Load(reader);
			}
			catch (Exception)
			{
				// an error occured -> exit with current model
				//MessageBox.Show("XML Loading error: " + e.Message.ToString());
				return;
			}

			// create root node
			XmlNode root = doc.DocumentElement;

			if (root.Name != "Railway") return;

			// create node list (below root)
			XmlNodeList xnl = root.ChildNodes;

			//Netze = new ArrayList();

			foreach(XmlNode node in xnl)
			{
				FillNetz(node, localpath);
			}

			reader.Close();
		}

		private void FillNetz(XmlNode node, string path)
		{
			try
			{
				string linie = ((XmlAttribute)node.Attributes["Name"]).Value;
				string netzname = ((XmlAttribute)node.Attributes["Folder"]).Value;

				int pos = SearchNetz(netzname, linie);
				Struct_Netz netz;
				if (pos < 0)
				{
					netz = new Struct_Netz();
					netz.Start = new ArrayList();
					netz.Linie = linie;
					netz.Netz = netzname;
					netz.AnsagenPath = path;
					foreach(XmlNode child in node.ChildNodes)
					{
						FillLinie(ref netz, linie, netzname, child);
					}
					Netze.Add(netz);
				}
				else
				{
					netz = (Struct_Netz)Netze[pos];
					foreach(XmlNode child in node.ChildNodes)
					{
						FillLinie(ref netz, linie, netzname, child);
					}
				}

			}
			catch(Exception){System.Windows.Forms.MessageBox.Show("FillNETZ");}
		}

		private void FillLinie(ref Struct_Netz netz, string linie, string netzName, XmlNode node)
		{
			try
			{
				string track = ((XmlAttribute)node.Attributes["Route"]).Value;
				string start = "", ziel = "";
				GetStartZiel(track, out start, out ziel);
				
				int pos = SearchStart(start, netz);
				Struct_Start ss;
				if (pos < 0)
				{
					ss = new Struct_Start();
					ss.Start = start;
					ss.Ziel = new ArrayList();
					ss.Verkehrshalte = new ArrayList();
					ss.Ansagen = new ArrayList();
					ss.Position = new Hashtable();
					ss.Ziel.Add(ziel);
					foreach(XmlNode child in node.ChildNodes) // <Track>
					{
						FillStations(ref ss, child);
					}
					netz.Start.Add(ss);               
				}
				else
				{
					ss = (Struct_Start)netz.Start[pos];
					ss.Ziel.Add(ziel);
					foreach(XmlNode child in node.ChildNodes) // <Track>
					{
						FillStations(ref ss, child);
					}
				}

			}
			catch(Exception){System.Windows.Forms.MessageBox.Show("FillLINIE");}
		}

		private void FillStations(ref Struct_Start start, XmlNode child)
		{
			try
			{
				string position = child.Attributes["Position"].Value;
				string ansage = (string)child.Attributes["Filename"].Value;
				string name = (string)child.InnerText;
			
				if (start.Verkehrshalte.IndexOf(name) < 0)
				{
					start.Verkehrshalte.Add(name);
					start.Ansagen.Add(ansage);
					start.Position.Add(name, position);
				}
			}
			catch(Exception e){System.Windows.Forms.MessageBox.Show("FillSTATIONS "+e.Message);}
		}


		private int SearchNetz(string name, string linie)
		{
			if (Netze == null || Netze.Count == 0) return -1;

			int retval = -1;
            
			foreach(Struct_Netz netz in Netze)
			{
				retval++;
				if (netz.Netz == name && netz.Linie == linie) return retval;
			}            

			return -1;
		}

		private int SearchStart(string name, Struct_Netz netz)
		{
			if (netz == null || netz.Start == null || netz.Start.Count == 0) return -1;

			int retval = -1;
            
			foreach(Struct_Start start in netz.Start)
			{
				retval++;
				if (start.Start == name) return retval;
			}            

			return -1;
		}

		private void GetStartZiel(string track, out string start, out string ziel)
		{
			int pos = track.IndexOf("-");
			start = track.Substring(0, pos).Trim();
			ziel = track.Substring(pos+1, track.Length-pos-1).Trim();
		}	
	}
}
