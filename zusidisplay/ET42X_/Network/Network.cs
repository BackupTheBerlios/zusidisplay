using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using MMI.EBuLa.Tools;

namespace MMI.ET42X
{
	public class Network : MMI.EBuLa.Tools.INetwork, MMI.EBuLa.Tools.IFISNetwork
	{
		Socket s;
		Socket new_socket;
		TextBox messages;
		ET42XControl c;
		ArrayList sockets_list = new ArrayList();
		MMI.EBuLa.Tools.SuperNetwork sn;
		
		public Network(ref ET42XControl control)
		{
			messages = new TextBox();
			c = control;
		}

		public void Dispose()
		{
			if (s!=null) s.Shutdown(SocketShutdown.Both);
			if (new_socket != null) new_socket.Shutdown(SocketShutdown.Both);
			s = null;
			new_socket =null;
		}

		public void Connect()
		{
			sn = new MMI.EBuLa.Tools.SuperNetwork(this);
			c.IsCONNECTED = true;

			try
			{
				sn.Connect(ref s, c.m_conf.Host, c.m_conf.Port, "ET42X Display "+MMI.EBuLa.Tools.SuperNetwork.VERSION);
			}
			catch(Exception)
			{
				//MessageBox.Show("Verbindung fehlgeschlagen! "+e.Message);
				c.IsCONNECTED = false;
				return;
			}
		}

		public void SetConnected(bool con)
		{
			c.IsCONNECTED = con;
		}

		/*public void Listen()
		{
			SocketPermission permission = new SocketPermission(System.Security.Permissions.PermissionState.Unrestricted);
			do
			{
				permission.Demand();
				s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ip = new IPEndPoint(IPAddress.Any, c.m_conf.Port);
				
				try
				{
					s.Bind(ip);
					s.Listen(8);

					new_socket = s.Accept();
				}
				catch (Exception)
				{return;}

				c.IsCONNECTED = true;

				MMI.EBuLa.Tools.SuperNetwork sn = new MMI.EBuLa.Tools.SuperNetwork(this);
				sn.Read(new_socket);
				s.Shutdown(SocketShutdown.Both);
			}
			while(true);
		}  */

		public void SendData(FIS_DATA type, string data)
		{
			switch(type)
			{
				case FIS_DATA.FIS_Aus:
					// TODO
					break;
				case FIS_DATA.FIS_Ausloesen:
					sn.SendToFISServer(s, 25, "");
					break;
				case FIS_DATA.FIS_Start:
					sn.SendToFISServer(s, 26, "");
					break;
				case FIS_DATA.GPS_Status:
					sn.SendToFISServer(s, 17, "");
					break;
				case FIS_DATA.Naechster_Halt:
					sn.SendToFISServer(s, 18, "");
					break;
				case FIS_DATA.Next_Entry:
					sn.SendToFISServer(s, 23, "");
					break;
				case FIS_DATA.Prev_Entry:
					sn.SendToFISServer(s, 24, "");
					break;
				case FIS_DATA.Route_Data:
					sn.SendToFISServer(s, 29, "");
					break;
				case FIS_DATA.Status:
					sn.SendToFISServer(s, 16, "");
					break;
				case FIS_DATA.Set_FISTYPE:
					if (data == "true")
						sn.SendToFISServer(s, 40, true);
					else if (data == "false")
						sn.SendToFISServer(s, 40, false);
					break;
				case FIS_DATA.Set_NaechsterHalt:
					// TODO
					break;
				case FIS_DATA.Set_Route_Netz:
					sn.SendToFISServer(s, 34, data);
					break;
				case FIS_DATA.Set_Route_Linie:
					sn.SendToFISServer(s, 35, data);
					break;
				case FIS_DATA.Set_Route_Start:
					sn.SendToFISServer(s, 36, data);
					break;
				case FIS_DATA.Set_Route_Ziel:
					sn.SendToFISServer(s, 37, data);
					break;
				case FIS_DATA.Set_Status:
					if (data == "true")
						sn.SendToFISServer(s, 32, true);
					else if (data == "false")
						sn.SendToFISServer(s, 32, false);
					break;
			}
		}
		public void ChangeFISState(int type, byte[] buffer) 
		{
			string data = "";
			float valu = 0f;
			bool state = false;
			BinaryReader reader = new BinaryReader(new MemoryStream(buffer));

			switch (type)
			{
				case  1: 
					valu = reader.ReadSingle();
					if (valu == 1f) state = true;
					c.SetFIS_Status(state);
					break;
				case  2: 
					valu = reader.ReadSingle();
					if (valu == 1f) state = true;
					c.SetFIS_GPSStatus(state);
					break;
				case  3:
					try
					{
						data = reader.ReadString();
						c.SetFIS_NaechsterHalt(data);
					}                              
					catch(Exception){}
					break;
				case  4: 
					try
					{
						data = reader.ReadString();
						c.SetFIS_Netz(data);
					}                              
					catch(Exception){}
					break;
				case  5: 
					try
					{
						data = reader.ReadString();
						c.SetFIS_Linie(data);
					}                              
					catch(Exception){}
					break;
				case  6: 
					try
					{
						data = reader.ReadString();
						c.SetFIS_Start(data);
					}                              
					catch(Exception){}
					break;
				case  7: 
					try
					{
						data = reader.ReadString();
						c.SetFIS_Ziel(data);
					}                              
					catch(Exception){}
					break;
				case  8: 
					c.SetFIS_NoMoreData();
					break;
				case  9: 
					c.SetFIS_NoMoreZiel();
					break;
				case  10: 
					c.SetFIS_NoMoreStart();
					break;
				default: // nix
					break;
			}
		}

		public void ChangeState(int type, byte[] buffer)
		{
			BinaryReader reader = new BinaryReader(new MemoryStream(buffer));
			float valu = reader.ReadSingle();

			bool state = false;

			if (valu == 1f) state = true;

			//Console.WriteLine("TYPE: "+type.ToString()+"  VAL: "+state.ToString());

			switch (type)
			{
				case  1: // Geschwindigkeit
					c.SetGeschwindigkeit(valu);
					break;
				case 2: // HLL
					break;
				case 4: // HBL
					break;
				case 5: // Zugkraft gesammt
					c.SetZugkraft(valu, false);
					break;
				case 6: // Zugkraft pro Achse
					c.SetZugkraft(valu, true);
					break;
				case 7:	// Oberstrom
					c.SetOberstrom(valu);
					break;
				case 8: // Spannung
					c.SetSpannung(valu);
					break;
				case 10:
					AddClockToZusi(valu);
					break;
				case 13: // LZB Zielgeschw.
					c.SetLZB_ZielGeschwindigkeit(valu);
					break;
				case 14: // AFB/LZB Sollgeschw.
					c.SetAFB_LZB_SollGeschwindigkeit(valu);
					break;
				case 16: // Fahrstufen
					c.SetFahrstufen(valu);
					break;
				case 18: // AFB Sollgeschw.
					c.SetAFB_Sollgeschwindigkeit(valu);
					break;
				case 20: // 1000 Hz Melder
					c.SetLM1000Hz(state);
					break;
				case 21:
					c.SetLM500Hz(state);
					break;
				case 22:  // PZB Befehl
					break;
				case 23:
					c.SetLMZugart(state, "55"); // Zugart U
					break;
				case 24:
					c.SetLMZugart(state, "70"); // Zugart M
					break;
				case 25:
					c.SetLMZugart(state, "85"); // Zugart O
					break;
				case 27: // LZB G
					c.SetLM_LZB_G(state);
					break;
				case 30:
					c.SetLM_LZB_ENDE(state);
					break;
				case 32:
					c.SetLM_LZB_B(state); // LZB B
					break;
				case 33:
					c.SetLM_LZB_S(state); // LZB S
					break;
				case 34: // LZB Ü
					c.SetLM_LZB_Ü(state);
					break;
				case 36: // Sifa
					c.SetLMSifa(state); 
					break;
				case 37: // Hauptschalter
					c.SetLMHauptschalter(!state); 
					break;
				case 47: // Türen offen
					c.SetLM_Tür(state);
					break;
				case 51: // Schalter Fahrstufen
					c.SetFahrstufenSchalter(valu);
					break;
				case 56: // AFB
					break;
				case 70: // unknown
					break;
				case 74: // INTEGRA SIGNUM
					c.SetLM_INTEGRA_GELB(state);
					break;
				case 75: // LZB Zielweg ab 0
					float old_valu = c.localstate.LZB_ZielWeg;
					double rest = Math.IEEERemainder(valu, 10);
					if (rest != 0)
					{
						valu += (10f - (float)rest);
					}
					old_valu = Math.Abs(old_valu - valu);
					if (old_valu >= 10) // mehr als 10 geändert
						c.SetLZB_ZielWeg(Convert.ToInt32(valu));
					break;
				case 76:
					c.SetLZB_Sollgeschwindigkeit(valu);
					break;
				case 85:
					// nur EBuLa
					break;
				case 88:
					c.SetReisezug(state);
					break;
				case 89:
					c.SetPZBSystem(valu);
					break;
				case 94:
					c.SetBrh(valu);
					break;
				case 95: // Bremsstellung
					c.SetBremsstellung(valu);
					break;
				case 96: // Zugdatei
					break;
				default:
					//System.Windows.Forms.MessageBox.Show("Unerwartet Daten empfangen! (Type = "+type.ToString()+"  Value = "+valu.ToString()+")");
					break;
			}
		}
	
		private void AddClockToZusi(float valu)
		{			
			int year, month, day, hour, min, sec;

			DateTime vtime = DateTime.Now;

			hour = vtime.Hour;
			min = vtime.Minute;
			sec = vtime.Second;
			day = vtime.Day;
			month = vtime.Month;
			year = vtime.Year;

			c.SetUhrDatum(ConvertSingleToDateTime(valu, year, month, day));
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

			if (c.addtionalhours && hour < 12) hour+=12;

			return new DateTime(year, month, day, hour, min, sec);
		}


	}
}
