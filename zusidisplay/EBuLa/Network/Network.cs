using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	public struct State
	{
		public Socket s;
	}

	public class Network
	{
		const string VERSION = MMI.EBuLa.Tools.SuperNetwork.VERSION;
		const int BUFFER_SIZE = 4096*128;
		const int REQUEST = 10;
		Socket s;
		TextBox messages;
		Control c;
		bool ZusiAknSent = false;
		public Socket new_socket;
		public bool DoRead = true;
		public bool isDAVID = false;

		public Network(/*ref TextBox tb, ref Control control*/bool DAV)
		{
			messages = new TextBox();
			//c = control;
			isDAVID = DAV;
		}


		public void Connect()
		{
			if (c == null) return;

			SocketPermission permission = new SocketPermission(System.Security.Permissions.PermissionState.Unrestricted);
			permission.Demand();

			IPHostEntry heserver = Dns.Resolve(c.XMLConf.Host);
			IPAddress iadd = (IPAddress)heserver.AddressList[0];
			IPEndPoint ip = new IPEndPoint(iadd, c.XMLConf.Port);

			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.NoDelay, 1);
			s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, 1);

			while(!s.Connected)
			{
				try
				{
					s.Connect(ip);
				}
				catch (Exception e)
				{
					//MessageBox.Show("Verbindung fehlgeschlagen! "+e.Message);
					c.use_network = false;
					Thread.Sleep(1000);
				}
			}

			c.use_network = true;

			SendHelloToZusi(s);

		}

		/*public void Listen()
		{
			if (c == null) return;
			SocketPermission permission = new SocketPermission(System.Security.Permissions.PermissionState.Unrestricted);
			permission.Demand();
			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
			try
			{
				s.Bind(ip);
				s.Listen(8);

				new_socket = null;
				new_socket = s.Accept();
				//
			}
			catch (Exception)
			{return;}

			

			Read(new_socket);
			
			s.Shutdown(SocketShutdown.Both);

			//s.BeginAccept(new AsyncCallback(Accept), state);
		}*/

		public void Dispose()
		{
			try
			{
				if (s != null)
				{
					s.Shutdown(SocketShutdown.Both);
					s.Close();
				}
			}
			catch(Exception){}
			s = null;
			new_socket =null;
		}

		#region OLD "Read ()"
		/*private void Read(Socket s)
		{
			do
			{
				GC.Collect(); // nicht mehr benutzte variablen freigeben
				int complete_read = -1;
				byte[] buffer = new byte[4096];
				int read = -1;
				read = s.Receive(buffer);

				if (buffer == null || buffer.Length == 0)
					continue;
			
				//Console.WriteLine("GOT: " + BufferToString(buffer, read));

				if ((CheckLength(buffer, out complete_read, read) == 0) || (buff_buffer != null))
				{
					// das war alles

					if (buff_buffer != null && buff_buffer.Length > 0)
					{
						byte[] helper = new byte[read];

						for (int i=0; i < read; i++)
						{
							helper[i] = buffer[i];
						}

						for (int i=0; i < buff_buffer.Length; i++)
						{
							buffer[i] = buff_buffer[i];
						}

						for (int i=0; i < read; i++)
						{
							try
							{
								buffer[i+buff_buffer.Length]  = helper[i];
							}
							catch (Exception)
							{
								break;
							}
						}

						read += buff_buffer.Length;

						Console.WriteLine("NEW BUFFER: "+BufferToString(buffer, read));
						buff_buffer = null;
					}

					// OK 
					byte byte1, byte2, byte4;
					byte1 = buffer[0+4];
					byte2 = buffer[1+4];
					byte4 = buffer[3+4];

					if (byte1 == 0)
					{
						switch (byte2)
						{
							case 01: // ZUSI SAID HELLO
								if (byte4 == 1)	ReadZusiHello(s, buffer, read);
								break;
							case 02:
								break;
							case 03:
								break;
							case 04:
								ReadZusiAknNeededData(s, buffer);
								break;
							case 10:// == 0A
								ReadZusiData(s, buffer, read);
								break;
							default:
								Console.WriteLine("DEFAULT: "+BufferToString(buffer, 20));
								break;
						}
					}
					else
					{
						Console.WriteLine("ELSE");
					}

				}
				else
				{
				
					// es kommt noch mehr...
					if (buff_buffer == null)
					{
						Console.WriteLine("AWAITING MORE");
						// bisher leer
						buff_buffer = new byte[read];
						for(int i = 0; i < read; i++)
						{
							buff_buffer[i] = buffer[i];
						}
					}
					else
					{
						Console.WriteLine("AWAITING MORE AND MORE");
						// das is schon was drin
						byte[] help_buffer = new byte[buff_buffer.Length+read];
						for(int i = 0; i < buff_buffer.Length; i++)
						{
							help_buffer[i] = buff_buffer[i];
						}

						for(int i = buff_buffer.Length; i < buff_buffer.Length+read; i++)
						{
							help_buffer[i] = buffer[i-buff_buffer.Length];
						}

						buff_buffer = help_buffer;
					}
				}
			}
			while(true);

			//s.Close();
		}*/
		#endregion

		public void setControl(ref MMI.EBuLa.Control cont)
		{
			c = cont;
		}

		private void Read(Socket s)
		{
			byte[] buffer = new byte[BUFFER_SIZE];
			int index = 0;

			try
			{
				if (!(s.Poll(1000, SelectMode.SelectRead) || s.Poll(1000, SelectMode.SelectWrite)))
				{
					throw new Exception("Auf Socket kann nicht mehr zugegriffen werden!");
				}
			}
			catch(Exception e)
			{
				//MessageBox.Show("Verbindung getrennt! "+e.Message);

				DoRead = false;
				return;
			}

			do
			{
				GC.Collect(); // nicht mehr benutzte variablen freigeben
				int read = -1;
				read = s.Receive(buffer, index, s.Available, SocketFlags.None);
				index += read;

				if (buffer == null || buffer.Length < 4)
					continue;

				BinaryReader reader = new BinaryReader(new MemoryStream(buffer));
				int length = reader.ReadInt32();

				//Console.WriteLine("LENGTH: "+length.ToString()+"  INDEX: "+index.ToString()+"\r\nGOT(buffer): " + BufferToString(buffer, index));

				if ((length + 4) < index)
				{
					// hui, mehere daten auf einmal
					int org_index = index;

					while ((length + 4) <= index)
					{
						int bufLen = length + 4;

						byte[] buffer_now = new byte[bufLen];

						for(int i=0; i < bufLen; i++)
						{
							buffer_now[i] = buffer[i/*+position*/];
						}

						HandleInput(s, buffer_now, length, bufLen);

						if ((length +4 ) == index)
						{
							// das war alles
							break;
						}

						//position += bufLen;
						for(int i=0; i < (buffer.Length-bufLen); i++)
						{
							buffer[i] = buffer[i+bufLen];
						}

						index -= bufLen;
						length = (new BinaryReader(new MemoryStream(buffer))).ReadInt32();
					}
				}
					#region Kompletter Block ist da
				else if ((length + 4) == index)
				{
					// das war alles
					int bufLen = index; // soviel is im buffer
					

					byte[] buffer_now = new byte[bufLen];

					for(int i=0; i < bufLen; i++)
					{
						buffer_now[i] = buffer[i];
					}
					
					HandleInput(s, buffer_now, length, bufLen);

					// puffer leeren
					buffer = new byte[BUFFER_SIZE];
					index = 0;
				}
				#endregion
			}
			while(s.Connected && DoRead);
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
						break;
				}
			}
			else
			{
				Console.WriteLine("ELSE");
			}
		}


		private void SendHelloToZusi(Socket s)
		{
			byte[] id;

			if (isDAVID)
				id = System.Text.ASCIIEncoding.UTF8.GetBytes("EBuLa "+VERSION+" (DAVID)");
			else
				id = System.Text.ASCIIEncoding.UTF8.GetBytes("EBuLa "+VERSION);

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

		private void ReadAckHello(Socket s, byte[] buffer, int read)
		{
			messages.Text += "Received: " + BufferToString(buffer,read) + "\r\n";

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
				else if (buffer2[0] == 0 && buffer2[1] == 2 && buffer2[2] == 2)
				{
					MessageBox.Show("Achtung! ZUSI ist bereits mit dem TCP Server verbunden! Bitte Verbindung mit ZUSI zuerst trennen!");
					DoRead = false;
					return;
				}
				else
				{
					MessageBox.Show("Server hat unerwartete Daten gesendet!");
					DoRead = false;
					return;
				}
												
			}
		}

		private void ReadZusiHello(Socket s, byte[] buffer, int read)
		{
			messages.Text += "Received: " + BufferToString(buffer,read) + "\r\n";

            // reading string
			string zusi = "";
			byte[] buffer2 = new byte[read];

			for (int i = 8; i < read; i++)
			{
				buffer2[i-8] = buffer[i];
			}

			zusi = System.Text.Encoding.ASCII.GetString(buffer,8,read-8);

			messages.Text += "Zusi found: " + zusi + " \r\n";

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
			messages.Text += "Sending ACK_HELLO: " + BufferToString(buffer, 7) + "\r\n";

			s.Send(buffer);
			ZusiAknSent = true;
			SendRequestToZusi(s);
		}

		private void SendRequestToZusi(Socket s)
		{
			byte[] buffer = {9,0,0,0, 0,3,0,10, REQUEST};
			// 50 = Uhrzeit (digital)

			/*
            buffer[0] = 0; buffer[1] = 3; // NEEDED_DATA
			buffer[2] = 0; buffer[3] = 10;

			buffer[4] = 1; buffer[5] = 2; // REQUEST 01 and 02

			buffer = AddLengthToBuffer(buffer);*/

			messages.Text += "Sending NEEDED_DATA: " + BufferToString(buffer, 10) + "\r\n";

			s.Send(buffer);

			// send last request
			byte[] buffer2 = {8,0,0,0, 0,3,0,0};
			/*
			buffer[0] = 0; buffer[1] = 3; // NEEDED_DATA
			buffer[2] = 0; buffer[3] = 0; // LAST REQUEST

			buffer = AddLengthToBuffer(buffer);
			  */
			messages.Text += "Sending NEEDED_DATA (LAST REQUEST): " + BufferToString(buffer2, 8) + "\r\n";

			s.Send(buffer2);

			//s.Close();

			//Read(s);

			//ReadZusiAknNeededData(s, buffer);
		}

		private void ReadZusiAknNeededData(Socket s, byte[] got_buffer)
		{
			messages.Text += "OK von ZUSI \r\n";

			// send last request
			byte[] buffer = new byte[7];
			buffer[0] = 0; buffer[1] = 4; // ACK_NEEDED_DATA
			buffer[2] = 0;

			buffer = AddLengthToBuffer(buffer);

			messages.Text += "Sending ACK_NEEDED_DATA: " + BufferToString(buffer, 7) + "\r\n";

			s.Send(buffer);

			//Read(s);

			//s.Close();
		}


		private void ReadZusiData(Socket s, byte[] buffer, int read)
		{					
			int count = (read-4-2)/5;
			if (count == 0) 
			{
				Console.WriteLine("Count == 0");
				//Read(s);
			}

			for (int i = 0; i < count; i++)
			{
				byte[] val = new byte[4];
				int type = buffer[i*5+6];
				val[0] = buffer[i*5+6+1];
				val[1] = buffer[i*5+6+2];
				val[2] = buffer[i*5+6+3];
				val[3] = buffer[i*5+6+4];

				switch(type)
				{
					case REQUEST:
						AddClockToZusi(type, val);
						break;
					case 11:
						//AddClockToZusi(type, val);
						break;
					case 12:
						//AddClockToZusi(type, val);
						break;
					default:
						break;
				}
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

		private void AddClockToZusi(int type, byte[] buffer)
		{
			c.use_network = true;
			
			BinaryReader reader = new BinaryReader(new MemoryStream(buffer));
			float valu = reader.ReadSingle();
			//double valu = reader.ReadDouble();
			//Console.Write("Type: " + type + "  Val: " + c.ConvertSingleToDateTime(valu, c.vtime.Year, c.vtime.Month, c.vtime.Day).ToString() + "\r\n");
			
			int year, month, day, hour, min, sec;

			hour = c.vtime.Hour;
			min = c.vtime.Minute;
			sec = c.vtime.Second;
			day = c.vtime.Day;
			month = c.vtime.Month;
			year = c.vtime.Year;

			if (type == REQUEST) // Stunde
			{
				//hour = Convert.ToInt32(valu);

				//Monitor.Enter(c.vtime);
				//c.vtime = c.ConvertToDateTime((double)valu);
				c.vtime =  c.ConvertSingleToDateTime(valu, year, month, day); // new DateTime(year,month,day,hour,min,sec);
				//Monitor.Exit(c.vtime);
			}
			else if (type == 11) // Minute
			{
				//min = Convert.ToInt32(valu);
			}
			else if (type == 12) // Sekunde
			{
				//sec = Convert.ToInt32(valu);
			}
			else
			{
				return;
			}
		}
	}
}
