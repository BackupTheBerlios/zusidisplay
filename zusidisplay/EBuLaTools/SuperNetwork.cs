using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace MMI.EBuLa.Tools
{
	public class NetworkCollector
	{
		public byte[] buffer;
		public int BUFFER_SIZE = 4096*128;
		public Socket socket;

		public NetworkCollector()
		{
			buffer = new byte[BUFFER_SIZE];
		}
	}

	public class SuperNetwork
	{
		public const string VERSION = "1.5.0";
		const int BUFFER_SIZE = 4096*128;
		bool ZusiAknSent = false;
		INetwork m_net;
		public bool DoRead = true;
		string client_id = "";

		public SuperNetwork(INetwork net)
		{
			m_net = net;
		}

		public void Connect(ref Socket s, string server, int port, string id)
		{
			SocketPermission permission = new SocketPermission(System.Security.Permissions.PermissionState.Unrestricted);
			permission.Demand();

			IPHostEntry heserver = Dns.Resolve(server);
			IPAddress iadd = (IPAddress)heserver.AddressList[0];
			IPEndPoint ip = new IPEndPoint(iadd, port);

			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			//s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.NoDelay, 1);
			//s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, 1);

			while(!s.Connected)
			{
				try
				{
					s.Connect(ip);
				}
				catch (Exception e)
				{
					//MessageBox.Show("Verbindung fehlgeschlagen! "+e.Message);
					m_net.SetConnected(false);
					Thread.Sleep(1000);
				}
			}

			m_net.SetConnected(true);

			client_id = id;

			SendHelloToZusi(s);

		}

		public void Read(Socket s)
		{
			byte[] buffer = new byte[BUFFER_SIZE];
			
			int index = 0;
			int read = -1;
			int avail = 0;
			bool connected = s.Connected;

			if (!DoRead) return;

			do
			{
				
				//GC.Collect(); // nicht mehr benutzte variablen freigeben

				/*try
				{	bool read_ok = s.Poll(100, SelectMode.SelectRead);
					Thread.Sleep(1);
					bool write_ok = s.Poll(100, SelectMode.SelectWrite);
					Thread.Sleep(1);

                    if (!( read_ok || write_ok ))
					{
						throw new Exception("Auf Socket kann nicht mehr zugegriffen werden!");
					}
				}  
				catch(Exception e)*/
				if (!connected)
				{
					//MessageBox.Show("Verbindung getrennt! "+e.Message);
					DoRead = false;
					m_net.SetConnected(false);
					Thread.Sleep(1);
					return;
				}
                
				avail = s.Available;

				if (avail == 0)
				{
					Thread.Sleep(1);
					continue;
				}

				int take_away = 0;
				if (avail >= BUFFER_SIZE+index)
					take_away = BUFFER_SIZE-index-1;
				else
					take_away = avail;
					
				read = s.Receive(buffer, index, take_away, SocketFlags.Partial);

				index += read;

				if (buffer == null || buffer.Length < 4)
				{
					Thread.Sleep(50);
					break;
				}

				BinaryReader reader = new BinaryReader(new MemoryStream(buffer));
				int length = reader.ReadInt32();

				if (length < 0) length = 0;

				//Console.WriteLine("LENGTH: "+length.ToString()+"  INDEX: "+index.ToString()+"\r\nGOT(buffer): " + BufferToString(buffer, index));

				int org_index = index;

				while ((length + 4) <= index)
				{
					int bufLen = length + 4;

					byte[] buffer_now = new byte[bufLen];

					for(int i=0; i < bufLen; i++)
					{
						buffer_now[i] = buffer[i];
					}

					HandleInput(s, buffer_now, length, bufLen);

					if ((length +4 ) == index)
					{
						// das war alles
						index = 0;
						break;
					}

					for(int i=0; i < (buffer.Length-bufLen); i++)
					{
						buffer[i] = buffer[i+bufLen];
					}

					index -= bufLen;
					length = (new BinaryReader(new MemoryStream(buffer))).ReadInt32();
				}

				Thread.Sleep(1);
			}
			while(connected && DoRead);
		}

		private void HandleInput(Socket s, byte[] buffer, int length, int bufLen)
		{
			// OK 
			byte byte1, byte2, byte4;
			byte1 = buffer[0+4];
			byte2 = buffer[1+4];					

			//Console.WriteLine("LENGTH: "+length+"  GOT(buffer_now): " + BufferToString(buffer, bufLen));

			if (byte1 == 0)
			{
				switch (byte2)
				{
					case 01: // ZUSI SAID HELLO
						byte4 = buffer[3+4];
						if (byte4 == 1)
						{
							//Console.WriteLine("READ HELLO: ");
							ReadZusiHello(s, buffer, bufLen);
						}
						break;
					case 02:
						ReadAckHello(s, buffer, bufLen);
						break;
					case 03:
						break;
					case 04:
						//Console.WriteLine("READ AKN: ");
						ReadZusiAknNeededData(s, buffer);
						break;
					case 10:// == 0A
						//Console.WriteLine("READ DATA: ");
						ReadZusiData(s, buffer, bufLen);
						break;
					default:
						//Console.WriteLine("DEFAULT: "+BufferToString(buffer, 20));
						MessageBox.Show("Unknown Type of Data received: "+BufferToString(buffer, buffer.Length));
						break;
				}
			}
			else
			{
				//Console.WriteLine("ELSE");
				MessageBox.Show("Unknown Data received: "+BufferToString(buffer, buffer.Length));
			}
		}

		private void ReadAckHello(Socket s, byte[] buffer, int read)
		{
			// reading string
			byte[] buffer2 = new byte[read];

			for (int i = 4; i < read; i++)
			{
				buffer2[i-4] = buffer[i];
			}

			if (buffer2 != null && buffer2.Length > 2)
			{
				if (buffer2[0] == 0 && buffer2[1] == 2 && buffer2[2] == 0)
				{
					// ok
					SendRequestToZusi(s);
					return;
				}
				else if (buffer2[0] == 0 && buffer2[1] == 2 && buffer2[2] == 1)
				{
					MessageBox.Show("Achtung! Es existieren bereits zu viele Verbindungen zum TCP Server!");
					m_net.SetConnected(false);
					DoRead = false;
					return;
				}
				else if (buffer2[0] == 0 && buffer2[1] == 2 && buffer2[2] == 2)
				{
					MessageBox.Show("Achtung! ZUSI ist bereits mit dem TCP Server verbunden! Bitte Verbindung mit ZUSI zuerst trennen!");
					m_net.SetConnected(false);
					DoRead = false;
					return;
				}
				else
				{
					MessageBox.Show("Server hat unerwartete Daten gesendet!");
					m_net.SetConnected(false);
					DoRead = false;
					return;
				}
												
			}
		}

		private void SendHelloToZusi(Socket s)
		{
			byte[] id = System.Text.ASCIIEncoding.UTF8.GetBytes(client_id);

			int length = id.Length + 9;

			byte[] buffer = new byte[length];

			buffer[0] = Convert.ToByte(length-4);
			buffer[1] = Convert.ToByte(0);
			buffer[2] = Convert.ToByte(0);
			buffer[3] = Convert.ToByte(0);

			buffer[4] = Convert.ToByte(0);
			buffer[5] = Convert.ToByte(1); // 4+5 Befehl 00 01

			buffer[6] = Convert.ToByte(1); // Prot Version

			buffer[7] = Convert.ToByte(2); // Client Type

			buffer[8] = Convert.ToByte(id.Length);

			for(int i = 0; i < id.Length; i++)
			{
				buffer[i+9] = id[i];
			}
								   
			byte[] empty = {0};
			//int length = 1+9;

			//MessageBox.Show("Sending ACK_HELLO: " + BufferToString(buffer, length) + "\r\n");

			try
			{
				s.Poll(1000, SelectMode.SelectWrite);
			}
			catch(Exception e)
			{
				MessageBox.Show("Fehler im Socket: "+e.Message);
				return;
			}

			s.Send(buffer);
			ZusiAknSent = true;
			Read(s);
		}

		private void ReadZusiHello(Socket s, byte[] buffer, int read)
		{
			//Console.WriteLine("Read: "+read.ToString()+"  Received: " + BufferToString(buffer,read));

			// reading string
			string zusi = "";
			byte[] buffer2 = new byte[read];

			for (int i = 8; i < read; i++)
			{
				buffer2[i-8] = buffer[i];
			}

			//Console.WriteLine("DECODING...");
			zusi = System.Text.Encoding.ASCII.GetString(buffer,8,read-8);

			//Console.WriteLine("Zusi found: " + zusi);

			SendAknToZusi(s);
		}

		private void SendAknToZusi(Socket s)
		{
			byte[] buffer = {7,0,0,0, 0,2,0};
			/*			buffer[0] = 0;
						buffer[1] = 2;
						buffer[2] = 0;
						buffer = AddLengthToBuffer(buffer);
			  */
			//Console.WriteLine("Sending ACK_HELLO: " + BufferToString(buffer, 7));

			s.Send(buffer);
			ZusiAknSent = true;
			SendRequestToZusi(s);
		}

		private void SendRequestToZusi(Socket s)
		{
			byte[] buffer = {51,0,0,0, 0,3,0,10, 1,2,3,4,5,6,7,8,9,10,13,16,18,20,21,22,23,24,25,27,30,32,33,34,36,37,41,47,51,53,54,56,64,74,75,76,85,86,88,89,94,95,96};
			//  1 = Geschwindigkeit
			//  2 = HLL
			//  3 = C-Druck
			//  4 = HBL
			//  5 = Zugkraft gesammt
			//  6 = Zugkraft pro Achse
			//  7 = Strom
			//  8 = Spannung
			// 10 = Uhrzeit Stunde
			// 16 = Fahrstufe
			// 13 = LZB Zielgeschwindigkeit
			// (14 = AFB/LZB Sollgeschwindigkeit)
			// 18 = AFB Zielgeschwindigkeit
			// 20 = LM PZB 1000 Hz
			// 21 = LM PZB 500 Hz
			// 22 = LM PZB Befehl
			// 23 = LM Zugart U
			// 24 = LM Zugart M
			// 25 =	LM Zugart O
			// 27 = LM LZB G (Geschwindigkeit reduzieren)
			// 30 = LM LZB Ende
			// 32 = LM LZB B (Betriebsbereit)
			// 33 = LM LZB S (St�rung)
			// 34 = LM LZB � (�bertragung)
			// 36 = LM Sifa
			// 37 = LM Hauptschalter
			// 41 = LM Mg-Bremse
			// 47 = LM T�ren
			// 51 = Schalter Fahrstufen
			// 53 = Schalter E-Bremse
			// 54 = Schalter Zusatzbremse
			// 56 = Schalter AFB
			// 64 = Schalter Fahrtrichtung
			// 74 = INTEGRA SIGNUM
			// 75 = LM LZB Zielweg (ab 0)
			// 76 = LZB Sollgeschwindigkeit
			// 83 Hintergrundbild
			// 84 Platzhalter Nachtinstr
			// 85 Strecken KM (X)
			// 86 T�ren	(X)
			// 87 Autopilot
			// 88 Reisezug (X)
			// 89 PZB System (X)
			// 90 fps
			// 91 F�hrerstand sichtbar
			// 92 Blockname
			// 93 gleis
			// 94 Brh (X)
			// 95 Bremsstellung (X)
			// 96 ZugDatei(X)

			s.Send(buffer);

			// send last request
			byte[] buffer2 = {8,0,0,0, 0,3,0,0};

			s.Send(buffer2);
		}

		private void ReadZusiAknNeededData(Socket s, byte[] got_buffer)
		{
			//Console.WriteLine("OK von ZUSI");

			// send last request
			byte[] buffer = new byte[7];
			buffer[0] = 0; buffer[1] = 4; // ACK_NEEDED_DATA
			buffer[2] = 0;

			buffer = AddLengthToBuffer(buffer);

			//Console.WriteLine("Sending ACK_NEEDED_DATA: " + BufferToString(buffer, 7));

			s.Send(buffer);

			//Read(s);

			//s.Close();
		}


		private void ReadZusiData(Socket s, byte[] buffer, int read)
		{					
			int count = (read-4-2)/5;
			if (count == 0) 
			{
				//Console.WriteLine("Count == 0");
				//Read(s);
			}

			for (int i = 0; i < count; i++)
			{
				if ((i*5+6) >= buffer.Length) 
				{
					MessageBox.Show("Puffer ist �bergelaufen!");
					break;
				}
				byte[] val = new byte[4];
				int type = buffer[i*5+6];
				val[0] = buffer[i*5+6+1];
				val[1] = buffer[i*5+6+2];
				val[2] = buffer[i*5+6+3];
				val[3] = buffer[i*5+6+4];

				/*if (type > 96)
				{
					// something went wrong
					return;
				}*/

				m_net.ChangeState(type, val);
			}
			//Read(s);
		}

		private byte[] AddLengthToBuffer(byte[] buffer)
		{
			int length = buffer.Length;
			byte[] result = new byte[length+4];

			result[0] = Convert.ToByte(length);

			for (int i = 0; i < length; i++)
			{
				result[i+4] = buffer[i];
			}
			return result;
		}


		private string BufferToString(byte[] buffer, int bytes)
		{
			int size = buffer.Length;

			if (bytes < size) size = bytes;

			string result = "";

			for (int i = 0; i < size; i++)
			{
				int b = buffer[i];
				string help = "";
				if (b < 10)
					help = "0"+ b.ToString();
				else 
					help = b.ToString();
				help += " ";
				if (i == 3) help += " ";	

				result += help;
			}

			return result;
		}

		private int CheckLength(byte[] buffer, out int outread, int read)
		{
			outread = -1;

			if (buffer.Length < 0) return -1;

			int length = buffer[0];

			if (length == read - 4)
			{
				outread = length;
				return 0;
			}
			else 
			{
				outread = length;
				return length - read;
			}
		}

	

	}
}
