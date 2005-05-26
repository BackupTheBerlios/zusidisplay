using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MMI.ICE3
{
	public class Network : MMI.EBuLa.Tools.INetwork
	{
		Socket s;
		TextBox messages;
		ICE3Control c;
		string side_text = "";
		
		public Network(/*ref TextBox tb, */ref ICE3Control control, string side)
		{
			messages = new TextBox();
			c = control;
			side_text = side;
		}

		public void Connect()
		{
			MMI.EBuLa.Tools.SuperNetwork sn = new MMI.EBuLa.Tools.SuperNetwork(this);
			c.IsCONNECTED = true;
			try
			{
				sn.Connect(ref s, c.m_conf.Host, c.m_conf.Port, "ICE3/T Display "+side_text+" "+MMI.EBuLa.Tools.SuperNetwork.VERSION);
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

		public bool GetConnectedStatus()
		{
			return c.IsCONNECTED;
		}
		public void SetStatus(MMI.EBuLa.Tools.ENUMStörung Stoerung)
		{
			SetStatus(Stoerung, false);
		}

		public void SetStatus(MMI.EBuLa.Tools.ENUMStörung Stoerung, bool delete)
		{
			if (delete)
			{
				c.localstate.störungmgr.DeleteStörung(Stoerung);
			}
			else
			{
				MMI.EBuLa.Tools.Störung st = new MMI.EBuLa.Tools.Störung(MMI.EBuLa.Tools.ENUMStörung.S11_ZUSIKomm);
				c.localstate.störungmgr.Add(st);
			}
		}

		/*public void Listen()
		{
			SocketPermission permission = new SocketPermission(System.Security.Permissions.PermissionState.Unrestricted);
			do
			{
				permission.Demand();
				s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ip = new IPEndPoint(IPAddress.Any, c.m_conf.Port);
				Socket new_socket;
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
		}*/

		public void ChangeFISState(int type, byte[] buffer) {}

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
					c.SetGeschwindigkeit(valu); // NIE benutzt???
					break;
				case 2: // HLL
					c.SetHL_Druck(valu);
					break;
				case 3: // C-Druck
					c.SetC_Druck(valu);
					break;
				case 4: // HBL
					c.SetHBL_Druck(valu);
					break;
				case 5: // Zugkraft gesammt
					c.SetZugkraft(valu, false);	// NIE benutzt
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
				case 9: // Drehzahl
					c.SetDrehzahl(valu);
					break;
				case 10:
					AddClockToZusi(valu);
					break;
				case 13: // LZB Zielgeschw. //c.SetLZB_ZielGeschwindigkeit(valu);
					break;
				case 14: // AFB/LZB Sollgeschw.	//c.SetAFB_LZB_SollGeschwindigkeit(valu);
					break;
				case 16: // Fahrstufen //c.SetFahrstufen(valu);
					break;
				case 18: // AFB Sollgeschw.
					c.SetAFB_Sollgeschwindigkeit(valu);
					break;
				case 20: // 1000 Hz Melder //c.SetLM1000Hz(state);
					break;
				case 21://c.SetLM500Hz(state);
					break;
				case 22:  // PZB Befehl
					break;
				case 23: //c.SetLMZugart(state, "55"); // Zugart U
					break;
				case 24: //c.SetLMZugart(state, "70"); // Zugart M
					break;
				case 25: //c.SetLMZugart(state, "85"); // Zugart O
					break;
				case 27: // LZB G //c.SetLM_LZB_G(state);
					break;
				case 30://c.SetLM_LZB_ENDE(state);
					break;
				case 32://c.SetLM_LZB_B(state); // LZB B
					break;
				case 33://c.SetLM_LZB_S(state); // LZB S
					break;
				case 34: // LZB Ü //c.SetLM_LZB_Ü(state);
					break;
				case 36: // Sifa  //c.SetLMSifa(state); 
					break;
				case 37: // Hauptschalter
					c.SetLMHauptschalter(!state); 
					break;
				case 41:
					c.SetLM_Mg(state);
					break;
				case 47: // Türen offen //c.SetLM_Tür(state);
					break;
				case 51: // Fahrstufen
					c.SetFahrstufen(valu);
					break;
				case 53: // E-Bremse
					c.SetE_Bremse(valu);
					break;
				case 54: // Zusatzbremse
					c.SetZusatzbremse(valu);
					break;					
				case 56: // AFB
					c.SetLM_AFB(state);
					break;
				case 70: // unknown
					break;
				case 74: // INTEGRA SIGNUM //c.SetLM_INTEGRA_GELB(state);
					break;
				case 75: // LZB Zielweg ab 0
					/*float old_valu = c.localstate.LZB_ZielWeg;
					double rest = Math.IEEERemainder(valu, 10);
					if (rest != 0)
					{
						valu += (10f - (float)rest);
					}
					old_valu = Math.Abs(old_valu - valu);
					if (old_valu >= 10) // mehr als 10 geändert
						//c.SetLZB_ZielWeg(Convert.ToInt32(valu));*/
					break;
				case 76: //c.SetLZB_Sollgeschwindigkeit(valu);
					break;
				case 85:
					// nur EBuLa
					break;
				case 86: // Türen
					c.SetTüren(valu);
					break;
				case 88: //c.SetReisezug(state);
					break;
				case 89: //c.SetPZBSystem(valu);
					break;
				case 94: //c.SetBrh(valu);
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
