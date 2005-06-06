using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Windows.Forms;
//using Microsoft.DirectX.Direct3D;

using MMI.EBuLa.Tools;


namespace MMI.DIAGNOSE
{	
	public class DIAGNOSEControl : System.Windows.Forms.UserControl
	{
		bool USE_DOUBLE_BUFFER = false;
		const int ZUGKRAFT = 96;
		const int BREMSKRAFT = 120;
		const string DSK_SPERRCODE = "1234";

		/*
		Microsoft.DirectX.Direct3D.Device device = null;
		Microsoft.DirectX.Direct3D.VertexBuffer vertexBuffer = null;
		Microsoft.DirectX.Direct3D.Texture texture = null;
		*/

		#region Declarations
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		int stoerung_counter = 0;
		bool inverse_display = false;

		private Bitmap m_backBuffer;
		private Graphics g;
		private string[,] buttons = new string[11,2];

		private DateTime vtime;

		private bool CONNECTED = false;
		int StörPos = 0;
		SoundInterface Sound;

		Color STÖRUNG_BG = Color.Gold, STÖRUNG_FG = Color.Black;
		Color BRIGHT = Color.WhiteSmoke; 
		Color DARK =  Color.DarkGray;
		Color BLACK = Color.Black;
		Color EINGABE = Color.Blue;
		Color EINGABE_BACKG = Color.WhiteSmoke;
		MMI.MMIBR185.BR185Control mmi_widget;

		private Thread zugkraft_thread;
		Hashtable ht = new Hashtable(), ht2 = new Hashtable();
		Point center;

		ArrayList uhrAußenList, uhrInnenklList, uhrInnengrList, uhrminList;
		ArrayList uhrstdList, uhrsecList, uhrrestList;

		bool INIT = true;
		DateTime lastTime = new DateTime(0);


		/*bool first_time_tacho = true;
		bool first_time_zugkraft = true;
		bool first_time_kombi = true;*/
		bool something_changed = true;
		Color MMI_BLUE = Color.FromArgb(0,128,255);
		Color STATUS_BLUE = Color.FromArgb(149,201,255);
		Color MMI_ORANGE = Color.FromArgb(179, 177, 142);

		public MMI.EBuLa.Tools.XMLLoader m_conf;
		private const SmoothingMode SMOOTHING_MODE = SmoothingMode.AntiAlias;
		private const TextRenderingHint TEXT_MODE = TextRenderingHint.AntiAliasGridFit;
		float old_valu = 0f;

		//DBGraphics graph_main;

		public MMI.DIAGNOSE.DIAGNOSEState localstate;
		private System.Windows.Forms.Timer timer_st;
		private System.Windows.Forms.Timer timer_eingabe;
		private System.Windows.Forms.Timer timer_verbrauch;

		bool on = true;

		#endregion
		public DIAGNOSEControl(MMI.EBuLa.Tools.XMLLoader conf, ref MMI.MMIBR185.BR185Control mmi)
		{
			if (!conf.DoubleBuffer)
			{
				//This turns off internal double buffering of all custom GDI+ drawing
				this.SetStyle(ControlStyles.DoubleBuffer, true);
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.UserPaint, true);
				USE_DOUBLE_BUFFER = false;
			}
			else
				USE_DOUBLE_BUFFER = true;

			InitializeComponent();

			center = new Point(468+80, 30+80);

			uhrAußenList = (new ISphere(468+80, 30+80, 74)).PointList();
			uhrInnenklList = (new ISphere(468+80, 30+80, 69)).PointList();
			uhrInnengrList = (new ISphere(468+80, 30+80, 62)).PointList();
			uhrminList = (new ISphere(468+80, 30+80, 60)).PointList();
			uhrstdList = (new ISphere(468+80, 30+80, 47)).PointList();
			uhrsecList = (new ISphere(468+80, 30+80, 60)).PointList();
			uhrrestList = (new ISphere(468+80, 30+80, 10)).PointList();

			m_conf = conf;

			mmi_widget = mmi;

			localstate = new MMI.DIAGNOSE.DIAGNOSEState();
			
			localstate.Energie = conf.Energie;
			
			something_changed = true;


			// NOTBREMSE und E-BREMSE fehlen

			vtime = DateTime.Now;

			zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));

			int interval = Convert.ToInt32(Math.Round((1d/(double)conf.FramesPerSecondLow)*1000d));
			timer1.Interval = interval;
			timer1.Enabled = true;

			switch (m_conf.Sound)
			{
				case 1:
					Sound = new APISound();
					break;
				case 2:
					Sound = new DxSound();
					break;
				default:
					Sound = new NullSound();
					break;
			}
			Button_SW_Pressed(this, new EventArgs());				

			mmi_widget.PREVENT_DRAW = true;
			/*
			try
			{
				// Now  setup our D3D stuff
				Microsoft.DirectX.Direct3D.PresentParameters presentParams = new Microsoft.DirectX.Direct3D.PresentParameters();
				presentParams.Windowed=true;
				presentParams.SwapEffect = Microsoft.DirectX.Direct3D.SwapEffect.Discard;
				device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, 
					Microsoft.DirectX.Direct3D.CreateFlags.HardwareVertexProcessing, presentParams);
			}
			catch (Exception) {}
			*/
		}

		protected override void Dispose( bool disposing )
		{
			if (localstate != null && m_conf != null)
			{
				m_conf.Energie = localstate.Energie;
				m_conf.SaveFile();
			}
			if (zugkraft_thread != null) zugkraft_thread.Abort();
			zugkraft_thread = null;
			
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.timer_st = new System.Windows.Forms.Timer(this.components);
			this.timer_eingabe = new System.Windows.Forms.Timer(this.components);
			this.timer_verbrauch = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 1;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timer_st
			// 
			this.timer_st.Enabled = true;
			this.timer_st.Interval = 1000;
			this.timer_st.Tick += new System.EventHandler(this.timer_st_Tick);
			// 
			// timer_eingabe
			// 
			this.timer_eingabe.Enabled = true;
			this.timer_eingabe.Interval = 1000;
			this.timer_eingabe.Tick += new System.EventHandler(this.timer_eingabe_Tick);
			// 
			// timer_verbrauch
			// 
			this.timer_verbrauch.Enabled = true;
			this.timer_verbrauch.Interval = 2000;
			this.timer_verbrauch.Tick += new System.EventHandler(this.timer_verbrauch_Tick);
			// 
			// DIAGNOSEControl
			// 
			this.BackColor = System.Drawing.Color.LightGray;
			this.Name = "DIAGNOSEControl";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BR185Control_Paint);

		}
		#endregion
		public string Version()
		{
			return Application.ProductVersion;
		}

		public void DisableSound()
		{
			Sound = new NullSound();
		}
		public bool addtionalhours
		{
			get{return localstate.addtionalhours;}
			set{localstate.addtionalhours = value;}
		}
		#region Buttons
		public void Button_Down_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker+1 < 6 || (localstate.marker >5 && localstate.marker+1 < 12))
					{
						localstate.marker++;
					}
					break;
				case CURRENT_DISPLAY.DSK:
					if (localstate.marker+1 < 4)
					{
						localstate.marker++;
					}
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker-1 >= 0 || localstate.marker-1 > 5)
					{
						localstate.marker--;
					}
					break;
				case CURRENT_DISPLAY.DSK:
					if (localstate.marker-1 >= 0)
					{
						localstate.marker--;
					}
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_E_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						if (localstate.marker_changed)
							localstate.Zugnummer = MakeZugnummer(localstate.ZugnummerTemp);
						else if (localstate.ZugnummerTemp == "      ")
						{
							MessageBox.Show("MÖÖP! Ungültige Zugnummer!");
							return;
						}
						localstate.ZugnummerTemp = "";
						localstate.marker_changed = false;
						localstate.marker = 6;
					}
					else if (localstate.marker >= 6 && localstate.marker < 12)
					{
						if (localstate.marker_changed2)
							localstate.Tfnummer = localstate.TfnummerTemp;
						localstate.TfnummerTemp = "";
						localstate.marker_changed2 = false;
						localstate.marker = 0;
						if (IsBR101())
                            localstate.DISPLAY = CURRENT_DISPLAY.G;
						else
							localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					}
					break;
				case CURRENT_DISPLAY.DSK:
					if (localstate.DSK_BUFFER == DSK_SPERRCODE)
					{
						localstate.DSK_Gesperrt = !localstate.DSK_Gesperrt;
						localstate.DSK_BUFFER = "    ";
						localstate.marker = 0;
						localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					}
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_C_Pressed(object sender, System.EventArgs e)
		{
			CURRENT_DISPLAY retdisplay = CURRENT_DISPLAY.G;
			if (localstate.type == TRAIN_TYPE.BR182) retdisplay = CURRENT_DISPLAY.Z_BR;

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					localstate.ZugnummerTemp = "";
					localstate.marker_changed = false;
					localstate.TfnummerTemp = "";
					localstate.marker_changed2 = false;
					localstate.marker = 0;
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DISPLAY = retdisplay;
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_Inverse_Pressed(object sender, System.EventArgs e)
		{
			Inverse();
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_Brightness_Pressed(object sender, System.EventArgs e)
		{

		}
 
		public void Button_Off_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_1_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "1");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "1");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "1");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_2_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zugbesy:
					localstate.DISPLAY = CURRENT_DISPLAY.Zug_Tf_Nr;
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "2");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "2");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "2");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_3_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Z_BR:
					if (localstate.type == TRAIN_TYPE.BR182)
						localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "3");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "3");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "3");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_4_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					if (localstate.type == TRAIN_TYPE.BR182) {/*nix*/}
					if (IsBR101())
                        localstate.DISPLAY = CURRENT_DISPLAY.Zug_Tf_Nr;
					else
						localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					break;
				case CURRENT_DISPLAY.Z_BR:
					if (IsBombardier() || IsADtranz())
						localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "4");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "4");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "4");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_5_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "5");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "5");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "5");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_6_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "6");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "6");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "6");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_7_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Z_BR:
					if (localstate.type == TRAIN_TYPE.BR182)
						localstate.SPUS = !localstate.SPUS;
					break;
				case CURRENT_DISPLAY.Zugbesy:
					localstate.DISPLAY = CURRENT_DISPLAY.DSK;
					break;
				case CURRENT_DISPLAY.G:
					if (localstate.type == TRAIN_TYPE.BR182) {/*nix*/}
					else
						localstate.DISPLAY = CURRENT_DISPLAY.Z_BR;
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "7");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "7");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "7");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_8_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Z_BR:
					if (localstate.type == TRAIN_TYPE.BR182) 
						localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					break;
				case CURRENT_DISPLAY.G:
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "8");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "8");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "9");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_9_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "9");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "9");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Remove(localstate.marker, 1);
					localstate.DSK_BUFFER = localstate.DSK_BUFFER.Insert(localstate.marker, "0");
					localstate.marker_changed = true;
					Button_Down_Pressed(sender, e);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_0_Pressed(object sender, System.EventArgs e)
		{
			if (!Visible) return;
			CURRENT_DISPLAY retdisplay = CURRENT_DISPLAY.G;
			if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20) retdisplay = CURRENT_DISPLAY.Z_BR;
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zugbesy:
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.Z_BR:
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.ST:
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.G:
					if (localstate.type == TRAIN_TYPE.BR182) localstate.DISPLAY = retdisplay;
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (localstate.marker >= 0 && localstate.marker < 6)
					{
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Remove(localstate.marker, 1);
						localstate.ZugnummerTemp = localstate.ZugnummerTemp.Insert(localstate.marker, "0");
						localstate.marker_changed = true;
						Button_Down_Pressed(sender, e);
					}
					else if (localstate.marker > 5 && localstate.marker < 12)
					{
						localstate.TfnummerTemp = localstate.TfnummerTemp.Remove(localstate.marker-6, 1);
						localstate.TfnummerTemp = localstate.TfnummerTemp.Insert(localstate.marker-6, "0");
						localstate.marker_changed2 = true;
						Button_Down_Pressed(sender, e);
					}
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_UD_Pressed(object sender, System.EventArgs e)
		{
		}

		public void Button_SW_Pressed(object sender, System.EventArgs e)
		{
			Switcher s = new Switcher(ref localstate, m_conf.TopMost);
			s.ShowDialog();
			something_changed = true;

			if (localstate.type == TRAIN_TYPE.ER20) localstate.DISPLAY = CURRENT_DISPLAY.Z_BR;

			mmi_widget.SetTrainType(localstate.type.ToString());
			mmi_widget.something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_V_GR_0_Pressed(object sender, System.EventArgs e)
		{
			if (ht2.Count > 0 && ht2[StörPos] != null)
			{
				Störung s = (Störung)ht[StörPos];
				localstate.störungmgr.getPStörungen.Remove(s);
				localstate.störungmgr.Add(s);
				ht2.Remove(StörPos);
			}
			if (localstate.störungmgr.StörStack != null && localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{
				localstate.störungmgr.PopCurrent();
				timer_st_Tick(sender, e);
				localstate.DISPLAY = CURRENT_DISPLAY.V_GREATER_0;
				ht.Clear(); ht2.Clear();
				something_changed = true;
			}


			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}
		public void Button_V_EQ_0_Pressed(object sender, System.EventArgs e)
		{
			if (ht2.Count > 0 && ht2[StörPos] != null)
			{
				Störung s = (Störung)ht[StörPos];
				localstate.störungmgr.getPStörungen.Remove(s);
				localstate.störungmgr.Add(s);
				ht2.Remove(StörPos);
			}

			if (localstate.störungmgr.StörStack != null && localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{
				localstate.störungmgr.PopCurrent();
				timer_st_Tick(sender, e);
				ht.Clear(); ht2.Clear();
				localstate.DISPLAY = CURRENT_DISPLAY.V_EQUAL_0;
				something_changed = true;
			}
                                                   
			
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}
		
		public void Button_ST_Pressed(object sender, System.EventArgs e)
		{
			//localstate.OLD_DISPLAY = localstate.DISPLAY;

			localstate.DISPLAY = CURRENT_DISPLAY.ST;
			something_changed = true;

			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}
		
		#endregion
		public bool IsCONNECTED
		{
			get{return CONNECTED;}
			set
			{
				if (value)
				{
					localstate.störungmgr.DeleteStörung(ENUMStörung.S01_ZUSIKomm);
					if (localstate.DISPLAY == CURRENT_DISPLAY.V_EQUAL_0 || localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0)
					{
						localstate.DISPLAY = CURRENT_DISPLAY.G;
					}
					if (INIT) INIT = false;
				}
				else
				{
					if (CONNECTED != value)
					{
						localstate.störungmgr.Add(new Störung(ENUMStörung.S01_ZUSIKomm));
						if (m_conf.Sound != 0)
						{
							switch (m_conf.Sound)
							{
								case 1:
									if (IsBombardier() || IsADtranz())
									{
										Sound.PlayMalfunctionBombardierSound();
										Sound.PlayMalfunctionBombardierSound();
										Sound.PlayMalfunctionBombardierSound();
									}
									else
									{
										Sound.PlayMalfunctionSiemensSound();
										Sound.PlayMalfunctionSiemensSound();
										Sound.PlayMalfunctionSiemensSound();
									}
									break;
								case 2:
									if (IsBombardier() || IsADtranz())
									{
										Sound.PlayMalfunctionBombardierSound();
										Sound.PlayMalfunctionBombardierSound();
										Sound.PlayMalfunctionBombardierSound();
									}
									else
									{
										Sound.PlayMalfunctionSiemensSound();
										Sound.PlayMalfunctionSiemensSound();
										Sound.PlayMalfunctionSiemensSound();
									}
									break;
							}
						}
					}
				}
				CONNECTED = value;
				something_changed = true;
			}
		}
		private void DrawFrame(ref Graphics pg, int x, int y, int width, int height)
		{
			if (IsBombardier() || IsADtranz())
			{
				DrawFrameSunken(ref pg, x, y, width, height);
				DrawFrameRaised(ref pg, x+2, y+2, width-3, height-3);
			}
			else
			{
				DrawFrameRaised(ref pg, x, y, width, height);
				DrawFrameSunken(ref pg, x+2, y+2, width-4, height-4);
			}
		}

		private void DrawFrameRaised(ref Graphics pg, int x, int y, int width, int height)
		{
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameRaised(ref pg, x, y, width, height, DARK, BRIGHT);
		}

		private void DrawFrameSunken(ref Graphics pg, int x, int y, int width, int height)
		{
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunken(ref pg, x, y, width, height, DARK, BRIGHT);
		}

		private void DrawFrameSunkenSmall(ref Graphics pg, int x, int y, int width, int height)
		{
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, x, y, width, height, DARK, BRIGHT);
		}

		public void FillFields(ref Graphics pg)
		{
		}

		public void SetText(ref DBGraphics pg)
		{			
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			//timer1.Stop();
			UpdateScreen();
			//timer1.Start();
		}

		#region SET
		public void SetLMZugart(bool state, string text)
		{
			if (this == null) return;

			if (localstate.Zugart == "" && state)
			{
				if (text == "95") localstate.Zugart = "95";
				if (text == "85") localstate.Zugart = "85";
				if (text == "75") localstate.Zugart = "75";
				if (text == "70") localstate.Zugart = "70";
				if (text == "60") localstate.Zugart = "60";
				if (text == "55") localstate.Zugart = "55";
			}
			if (text == "85" || text == "95") localstate.LM_Zugart_O = state;
			if (text == "70" || text == "75") localstate.LM_Zugart_M = state;
			if (text == "55" || text == "60") localstate.LM_Zugart_U = state;

			something_changed = true;
		}

		public void SetLM1000Hz(bool state)
		{
			localstate.LM_1000Hz = state;
			something_changed = true;
		}

		public void SetLM500Hz(bool state)
		{
			localstate.LM_500Hz = state;
			something_changed = true;
		}

		public void SetLMBefehl(bool state)
		{
			localstate.LM_Befehl = state;
			something_changed = true;
		}

		public void SetLMSifa(bool state)
		{
			localstate.LM_Sifa = state;
			something_changed = true;
		}
	
		public void SetLMHauptschalter(bool state)
		{
			localstate.LM_HS = state;
			something_changed = true;
		}

		public void SetLM_LZB_Ü(bool state)
		{
			localstate.LM_LZB_Ü = state;

			something_changed = true;
		}

		public void SetLM_LZB_S(bool state)
		{
			localstate.LM_LZB_S = state;
			something_changed = true;
		}

		public void SetLM_LZB_B(bool state)
		{
			localstate.LM_LZB_B = state;
			something_changed = true;
		}

		public void SetLM_LZB_G(bool state)
		{
			localstate.LM_LZB_G = state;
			something_changed = true;
		}

		public void SetLM_LZB_ENDE(bool state)
		{
			localstate.LM_LZB_ENDE = state;
			something_changed = true;
		}
		
		public void SetLM_LZB_H(bool state)
		{
			localstate.LM_LZB_H = state;
			something_changed = true;
		}
		public void SetLM_Tür(bool state)
		{
			localstate.LM_TÜR = state;
			something_changed = true;
		}

		public void SetLM_INTEGRA_GELB(bool state)
		{
			localstate.LM_INTEGRA_GELB = state;
			something_changed = true;
		}
		public void SetLM_INTEGRA_ROT(bool state)
		{
			localstate.LM_INTEGRA_ROT = state;
			something_changed = true;
		}
		public void SetReisezug(bool state)
		{
			localstate.Reisezug = state;
			something_changed = true;
		}
		public void SetPZBSystem(float valu)
		{
			/*
			if (valu * Math.Pow(10, 45) > 14)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.SIGNUM;
			}
			else if (valu * Math.Pow(10, 45) > 12)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.LZB80_I80;
			}
			else if (valu * Math.Pow(10, 45) > 11)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.PZ80R;
			}
			else if (valu * Math.Pow(10, 45) > 9)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.PZ80;
			}
			else if (valu * Math.Pow(10, 45) > 8)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.PZB90_16;
			}
			else if (valu * Math.Pow(10, 45) > 7)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.PZB90_15;
			}
			else if (valu * Math.Pow(10, 45) > 5)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.I60R;
			}
			else if (valu * Math.Pow(10, 45) > 4)
			{
				localstate.PZB_System = ZUGBEEINFLUSSUNG.I60;
			}

			if (localstate.PZB_System != ZUGBEEINFLUSSUNG.PZB90_16)
			{
				PZB_ZA_O.Text1 = "95";
				PZB_ZA_M.Text1 = "75";
				PZB_ZA_U.Text1 = "60";
				if (localstate.Zugart == "85") 
					PZB_1000_TEXT.Text = "Überwachungsgeschwindigkeit: 95km/h";
				else if (localstate.Zugart == "70") 
					PZB_1000_TEXT.Text = "Überwachungsgeschwindigkeit: 75km/h";
				else 
					PZB_1000_TEXT.Text = "Überwachungsgeschwindigkeit: 60km/h";
			}
			else
			{
				PZB_ZA_O.Text1 = "85";
				PZB_ZA_M.Text1 = "70";
				PZB_ZA_U.Text1 = "55";
				PZB_1000_TEXT.Text = "Überwachungsgeschwindigkeit: "+localstate.Zugart+"km/h";
			}

			something_changed = true;		
			*/
		}
		public void SetBrh(float valu)
		{
			localstate.Brh = valu;
			something_changed = true;
		}
		public void SetBremsstellung(float valu)
		{
			if (valu * Math.Pow(10, 45) > 5)
			{
				localstate.Bremsstellung = BREMSSTELLUNG.P_Mg;
			}
			else if (valu * Math.Pow(10, 45) > 4)
			{
				localstate.Bremsstellung = BREMSSTELLUNG.R_Mg;
			}
			else if (valu * Math.Pow(10, 45) > 2)
			{
				localstate.Bremsstellung = BREMSSTELLUNG.R;
			}
			else if (valu * Math.Pow(10, 45) > 1)
			{
				localstate.Bremsstellung = BREMSSTELLUNG.P;
			}
			// else localstate.Bremsstellung = BREMSSTELLUNG.G
			
			something_changed = true;
		}
		public void SetAFB_LZB_SollGeschwindigkeit(float valu)
		{
			localstate.AFB_LZB_SollGeschwindigkeit = valu;
			something_changed = true;
		}
		public void SetAFB_Sollgeschwindigkeit(float valu)
		{
			localstate.AFB_SollGeschwindigkeit = valu;
			something_changed = true;
		}
		public void SetLZB_Sollgeschwindigkeit(float valu)
		{
			localstate.LZB_SollGeschwindigkeit = valu;
			something_changed = true;
		}
		public void SetLZB_ZielGeschwindigkeit(float valu)
		{
			localstate.LZB_ZielGeschwindigkeit = valu;
			something_changed = true;
		}
		public void SetLZB_ZielWeg(int valu)
		{
			if (Math.Abs(localstate.LZB_ZielWeg - valu) > 9)
				something_changed = true;
			localstate.LZB_ZielWeg = valu;			
		}
		public void SetGeschwindigkeit(float valu)
		{
			localstate.Geschwindigkeit = valu;
			something_changed = true;
			
		}

		public void SetZugkraft(float valu, bool proAchse)
		{
			if (proAchse)
			{
				if (valu >= -150f && valu <= 80f)
				{
					zugkraft_thread.Abort();
					zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));
					localstate.Zugkraft = valu;
					zugkraft_thread.Start();
					something_changed = true;
				}
			}
			else
			{
				zugkraft_thread.Abort();
				zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));
				localstate.ZugkraftGesammt = valu;
				zugkraft_thread.Start();
				something_changed = true;
			}
		}

		public void SetFahrstufen(float valu)
		{
			localstate.Fahrstufe = Convert.ToInt32(valu);
			//SetMaschinenTeil(true);
			something_changed = true;
		}

		public void SetFahrstufenSchalter(float valu)
		{
			localstate.FahrstufenSchalter = Convert.ToInt32(valu);
			something_changed = true;
		}

		public void SetOberstrom(float valu)
		{
			localstate.Oberstrom = valu;
			something_changed = true;
			//SetMaschinenTeil(true);
		}
		
		public void SetDrehzahl(float valu)
		{
			localstate.Drehzahl = valu;
			something_changed = true;
			//SetMaschinenTeil(true);
		}
		
		public void SetSpannung(float valu)
		{
			localstate.Spannung = valu;
			something_changed = true;
			//SetMaschinenTeil(true);
		}

		public void SetUhrDatum(DateTime valu)
		{
			if (localstate.Spannung > 0f && localstate.Oberstrom > 0f)
			{
				double millisecs = Math.Abs(valu.Ticks - vtime.Ticks) / 10000000d;
				if (millisecs > 0d)
					SetVerbrauch( millisecs );
			}

			vtime = valu;

			TimeSpan span = new TimeSpan(DateTime.Now.Ticks - lastTime.Ticks);

			if (span.TotalMilliseconds < 400)
			{
				lastTime = DateTime.Now;
				return;
			}
			
			lastTime = DateTime.Now;
			if (localstate.SHOW_CLOCK) something_changed = true;
		}

		public void SetFahrtrichtung(float valu)
		{
			if (valu == 0f) localstate.Richtungsschalter = 1;
			else if (valu == 1f) localstate.Richtungsschalter = 0;
			else if (valu == 2f) localstate.Richtungsschalter = -1;
		}
		public void SetHL_Druck(float valu)
		{
			localstate.HL_Druck = valu;
			something_changed = true;
		}

		public void SetHBL_Druck(float valu)
		{
			localstate.HBL_Druck = valu;
			something_changed = true;
		}
		public void SetZusatzbremse(float valu)
		{
			localstate.Zusatzbremse = valu;
			something_changed = true;
		}
		public void SetEBremse(float valu)
		{
			localstate.E_Bremse = valu;
			something_changed = true;
		}
		public void SetCDruck(float valu)
		{
			localstate.C_Druck = valu;
			something_changed = true;
		}
		#endregion
		public void DrawZugkraft(ref Graphics pg)
		{
			// Rahmen
			DrawFrame(ref pg, 508, 281, 120, 38);

			float zugkraft = localstate.ZugkraftGesammt;

			zugkraft = ReduziereZugkraft(localstate.ZugkraftGesammt, 1) * 2f;

			if (zugkraft < 0) zugkraft *= -1;

			string s = Convert.ToInt32(zugkraft).ToString();

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Gesammtkraft", f, new SolidBrush(BLACK), 516, 258);
			pg.DrawString("kN", f, new SolidBrush(BLACK), 590, 293);
			if (zugkraft >= 100)
				pg.DrawString(s, f, new SolidBrush(BLACK), 564, 293);
			else if (zugkraft >= 10)
				pg.DrawString(s, f, new SolidBrush(BLACK), 572, 293);
			else
				pg.DrawString(s, f, new SolidBrush(BLACK), 580, 293);
		}

		public void DrawAnalogUhr(ref Graphics pg)
		{
			// Uhr analog
			// Rahmen
			DrawFrame(ref pg, 468, 30, 160, 160);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			Pen p = new Pen(new SolidBrush(BLACK), 1);
			Pen p2 = new Pen(new SolidBrush(BLACK), 3);
			Pen p3 = new Pen(new SolidBrush(BLACK), 4);

			for (int i = 0; i < 60; i++)
			{
				if (Math.IEEERemainder(i, 5) == 0)
				{
					if (Math.IEEERemainder(i, 15) == 0)
						pg.DrawLine(p2, (Point)uhrInnengrList[6*i], (Point)uhrAußenList[6*i]);
					else 
						pg.DrawLine(p, (Point)uhrInnengrList[6*i], (Point)uhrAußenList[6*i]);
				}
				else
					pg.DrawLine(p, (Point)uhrInnenklList[6*i], (Point)uhrAußenList[6*i]);

			}

			// Std
			int pos = vtime.Hour;
			if (pos > 11) 
				pos = ((pos-12) * 30) - 90;
			else
				pos = (pos * 30) - 90;


			pos += Convert.ToInt32(vtime.Minute * 0.5);

			if (pos < 0) pos += 360;
			int pos2 = pos - 180; if (pos2 < 0) pos2 += 360;
			pg.DrawLine(p3, (Point)uhrstdList[pos], center);
			pg.DrawLine(p3, (Point)uhrrestList[pos2], center);

			pos = (vtime.Minute * 6) - 90;
			if (pos < 0) pos += 360;
			pos2 = pos - 180; if (pos2 < 0) pos2 += 360;
			pg.DrawLine(p2, (Point)uhrminList[pos], center);
			pg.DrawLine(p2, (Point)uhrrestList[pos2], center);

			pos = (vtime.Second * 6) - 90;
			if (pos < 0) pos += 360;
			pos2 = pos - 180; if (pos2 < 0) pos2 += 360;
			pg.DrawLine(p, (Point)uhrsecList[pos], center);
			pg.DrawLine(p, (Point)uhrrestList[pos2], center);
		}
		public void DrawDigitalUhr(ref Graphics pg)
		{
			// Uhr digital
			// Rahmen
			DrawFrameSunken(ref pg, 468, 1, 159, 111);
			DrawFrameSunken(ref pg, 472, 5, 152, 50);
			DrawFrameSunken(ref pg, 472, 54, 152, 55);
			
			Font f = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Millimeter);
			
			string s = "";

			s = vtime.ToShortTimeString() + ":";

			if (vtime.Second < 10)
				s += "0"+vtime.Second.ToString();
			else
				s += vtime.Second.ToString();

			pg.DrawString(s, f, new SolidBrush(BLACK), 483, 65);
		}
		public void DrawSmallUhr(ref Graphics pg)
		{
			// Uhr oben rechts digital
			DrawFrame(ref pg, 508, 1, 120, 38);

			string s = "";

			s = vtime.ToShortTimeString() + ":";

			if (vtime.Second < 10)
				s += "0"+vtime.Second.ToString();
			else
				s += vtime.Second.ToString();

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString(s, f, new SolidBrush(BLACK), 538, 12);
		}
		public void DrawUhr(ref Graphics pg)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.G || localstate.DISPLAY == CURRENT_DISPLAY.Zugbesy || localstate.DISPLAY == CURRENT_DISPLAY.Z_BR)
			{
				if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20)
                    DrawDigitalUhr(ref pg);
				else
					DrawAnalogUhr(ref pg);
					
			}
			else
			{
				DrawSmallUhr(ref pg);
			}

		}

		public void DrawSpannung(ref Graphics pg)
		{
			if (IsBombardier() || IsADtranz()) 
				DrawSpannungBombardier(ref pg);
			else if (localstate.type == TRAIN_TYPE.BR182)
				DrawSpannungTaurus(ref pg);
			else if (localstate.type != TRAIN_TYPE.ER20)
				DrawSpannungSiemens(ref pg);
		}
		public void DrawSpannungSiemens(ref Graphics pg)
		{
			float height = (localstate.Spannung-10) * 23f;

			if (height < 0) height = 0;
			else if (height > 220) height = 220;

			int allone = 30;

			if (localstate.traction > 1) allone = 0;

			for(int i = 1; i <= localstate.traction; i++)
			{
				pg.FillRectangle(new SolidBrush(MMI_BLUE), i*60+allone, 50+220-height, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, i*60+allone, 50, 40, 220);
				if (!localstate.LM_HS)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*60+allone, 280, 40, 20);
				// HS Rahmen
				DrawFrameSunken(ref pg, i*60+allone, 280, 40, 20);
			}

			//Beschriftung
			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < 11; i++)
			{
				pg.SmoothingMode = SmoothingMode.None;
				if (Math.IEEERemainder(i+10, 5) == 0)
				{
					pg.DrawLine(new Pen(BLACK),48+allone, 269-i*21.75f, 59+allone, 269-i*21.75f);
					pg.DrawString((i+10).ToString(), f, new SolidBrush(BLACK), 25+allone, 260-i*21.6f);
				}
				else
					pg.DrawLine(new Pen(BLACK),54+allone, 269-i*21.75f, 59+allone, 269-i*21.75f);
				pg.SmoothingMode = SMOOTHING_MODE;	
			}
			pg.DrawString("kV", f, new SolidBrush(BLACK), 23+allone, 80);
			pg.DrawLine(new Pen(BLACK),48+allone, 289, 59+allone, 289);
			pg.DrawString("0", f, new SolidBrush(BLACK), 33+allone, 280);

			Pen p = new Pen(BLACK);
			p.DashStyle = DashStyle.Dash;

			if (localstate.traction > 1)
			{

				for(int i = 0; i < localstate.traction; i++)
				{
					pg.DrawLine(p,60+allone+i*60, 160, 99+allone+i*60, 160);
					pg.DrawString("Lok"+(i+1).ToString(), f, new SolidBrush(BLACK), 60+i*60, 310);
				}
				DrawFrame(ref pg, 15, 15, 120+(localstate.traction-1)*60, 320);
			}
			else
			{
				pg.DrawLine(p,60+allone, 160, 99+allone, 160);
				// TODO
				DrawFrameRaised(ref pg, 60, 305, 78, 25);

				pg.DrawString("Spannung", f, new SolidBrush(BLACK), 60, 309);

				DrawFrame(ref pg, 40, 15, 120, 320);
			}

			f = new Font("Arial", 3.5f, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < localstate.traction; i++)
			{
				if (localstate.LM_HS)
				{
					f = new Font("Arial", 3.0f, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString("HS aus", f, new SolidBrush(BLACK), 60+i*60+allone, 283);
				}
				else
				{
					pg.DrawString("HS an", f, new SolidBrush(Color.WhiteSmoke), 59+i*60+allone, 283);
				}
			}
		}
		public void DrawSpannungTaurus(ref Graphics pg)
		{
			float height = (localstate.Spannung-10) * 23f;

			if (height < 0) height = 0;
			else if (height > 220) height = 220;

			int allone = 30;
			int y = -35;

			if (localstate.traction > 1) allone = 0;

			for(int i = 1; i <= localstate.traction; i++)
			{
				pg.FillRectangle(new SolidBrush(MMI_BLUE), i*60+allone, 50+220-height+y, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, i*60+allone, 50+y, 40, 220);
				if (!localstate.LM_HS)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*60+allone, 280+y, 40, 20);
				// HS Rahmen
				DrawFrameSunken(ref pg, i*60+allone, 280+y, 40, 20);
			}

			//Beschriftung
			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < 11; i++)
			{
				pg.SmoothingMode = SmoothingMode.None;
				if (Math.IEEERemainder(i+10, 5) == 0)
				{
					pg.DrawLine(new Pen(BLACK),48+allone, 269-i*21.75f+y, 59+allone, 269-i*21.75f+y);
					pg.DrawString((i+10).ToString(), f, new SolidBrush(BLACK), 25+allone, 260-i*21.6f+y);
				}
				else
					pg.DrawLine(new Pen(BLACK),54+allone, 269-i*21.75f+y, 59+allone, 269-i*21.75f+y);
				pg.SmoothingMode = SMOOTHING_MODE;	
			}
			pg.DrawString("kV", f, new SolidBrush(BLACK), 23+allone, 80+y);
			pg.DrawLine(new Pen(BLACK),48+allone, 289+y, 59+allone, 289+y);
			pg.DrawString("0", f, new SolidBrush(BLACK), 33+allone, 280+y);

			Pen p = new Pen(BLACK);
			p.DashStyle = DashStyle.Dash;

			if (localstate.traction > 1)
			{

				for(int i = 0; i < localstate.traction; i++)
				{
					pg.DrawLine(p,60+allone+i*60, 160+y, 99+allone+i*60, 160+y);
					pg.DrawString("Lok"+(i+1).ToString(), f, new SolidBrush(BLACK), 60+i*60, 310+y);
				}
				DrawFrameSunken(ref pg, 15, 2, 120+(localstate.traction-1)*60, 335+y);
			}
			else
			{
				pg.DrawLine(p,60+allone, 160+y, 99+allone, 160+y);
				// TODO
				DrawFrameRaised(ref pg, 60, 305+y, 78, 25);

				pg.DrawString("Spannung", f, new SolidBrush(BLACK), 60, 309+y);

				DrawFrameSunken(ref pg, 40, 2, 120, 335+y);
			}

			f = new Font("Arial", 3.5f, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < localstate.traction; i++)
			{
				if (localstate.LM_HS)
				{
					f = new Font("Arial", 3.0f, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString("HS aus", f, new SolidBrush(BLACK), 60+i*60+allone, 283+y);
				}
				else
				{
					pg.DrawString("HS an", f, new SolidBrush(Color.WhiteSmoke), 59+i*60+allone, 283+y);
				}
			}
		}
		public void DrawSpannungBombardier(ref Graphics pg)
		{
			float height = (localstate.Spannung-10) * 23f;

			if (height < 0) height = 0;
			else if (height > 220) height = 220;

			int allone = 20;

			if (localstate.traction > 1) allone = 0;

			for(int i = 1; i <= localstate.traction; i++)
			{
				pg.FillRectangle(new SolidBrush(MMI_BLUE), i*70+allone, 50+220-height, 50, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, i*70+allone, 50, 50, 220);
				if (!localstate.LM_HS)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*70+allone, 280, 50, 20);
				// HS Rahmen
				DrawFrameSunken(ref pg, i*70+allone, 280, 50, 20);
			}

			//Beschriftung
			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < 11; i++)
			{
				pg.SmoothingMode = SmoothingMode.None;
				if (Math.IEEERemainder(i+10, 5) == 0)
				{
					pg.DrawLine(new Pen(BLACK),58+allone, 269-i*21.75f, 69+allone, 269-i*21.75f);
					pg.DrawString((i+10).ToString(), f, new SolidBrush(BLACK), 35+allone, 260-i*21.6f);
				}
				else
					pg.DrawLine(new Pen(BLACK),64+allone, 269-i*21.75f, 69+allone, 269-i*21.75f);
				pg.SmoothingMode = SMOOTHING_MODE;	
			}
			pg.DrawString("kV", f, new SolidBrush(BLACK), 30+allone, 20);

			// Seitenschrift
			float oldsize = f.Size;
			if (IsADtranz())
			{
				f = new Font(GetFontString(), 22, FontStyle.Bold, GraphicsUnit.Point);
				pg.DrawString("Spannung", f, new SolidBrush(BLACK), 465, 230);
				pg.DrawString("Oberstrom", f, new SolidBrush(BLACK), 460, 260);
			}
			else
			{
				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Spannung", f, new SolidBrush(BLACK), 500, 230);
				pg.DrawString("Oberstrom", f, new SolidBrush(BLACK), 500, 255);
			}

			f = new Font("Arial", oldsize, FontStyle.Bold, GraphicsUnit.Millimeter);

			if (localstate.traction > 1)
			{
				for(int i = 0; i < localstate.traction; i++)
				{
					pg.DrawString("Lok"+(i+1).ToString(), f, new SolidBrush(BLACK), 75+i*70, 305);
				}
			}
			else
			{
				pg.DrawString("U", f, new SolidBrush(BLACK), 85+allone, 305);
				pg.DrawString("f", f, new SolidBrush(BLACK), 100+allone, 310);
			}

			f = new Font("Arial", 3.8f, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 0; i < localstate.traction; i++)
			{
				if (localstate.LM_HS)
				{
					pg.DrawString("HS aus", f, new SolidBrush(BLACK), 70+i*70+allone, 283);
				}
				else
				{
					pg.DrawString("HS an", f, new SolidBrush(Color.WhiteSmoke), 73+i*70+allone, 283);
				}
			}
		}
		public void DrawOberstrom(ref Graphics pg)
		{
			if (IsBombardier()  || IsADtranz())
				DrawOberstromBombardier(ref pg);
			else if (localstate.type == TRAIN_TYPE.BR182)
				DrawOberstromTaurus(ref pg);
			else if (localstate.type == TRAIN_TYPE.ER20)
				DrawTankinhaltEurorunner(ref pg);
			else
				DrawOberstromSiemens(ref pg);
		}
		public void DrawOberstromSiemens(ref Graphics pg)
		{
			if (localstate.traction == 1)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.Z_BR) return;
				float height = (localstate.Oberstrom) / 2.4f;

				if (height < 0) height = 0;
				else if (height > 250) height = 250;


				pg.FillRectangle(new SolidBrush(MMI_BLUE), 240, 50+250-height, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, 240, 50, 40, 250);

				//Beschriftung
				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			
				DrawFrame(ref pg, 190, 15, 120, 320);
	
				DrawFrameRaised(ref pg, 208, 305, 82, 25);

				for(int i = 0; i < 13; i++)
				{
					pg.SmoothingMode = SmoothingMode.None;
					if (Math.IEEERemainder(i, 2) == 0 || i == 0)
					{
						pg.DrawLine(new Pen(BLACK),150+48+30, 299-i*20.82f, 150+59+30, 299-i*20.82f);
						if (i == 0)
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+35+30, 290-i*20.82f);
						else
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+18+30, 290-i*20.82f);
					}
					else
						pg.DrawLine(new Pen(BLACK),150+54+30, 299-i*20.82f, 150+59+30, 299-i*20.82f);
					pg.SmoothingMode = SMOOTHING_MODE;	
				}

				pg.DrawString("Oberstrom", f, new SolidBrush(BLACK), 208, 309);
				pg.DrawString("  A", f, new SolidBrush(BLACK), 203, 62);
			}
			else
			{
				float height = (localstate.Oberstrom) / 4.8f * localstate.traction;

				if (height < 0) height = 0;
				else if (height > 250) height = 250;


				pg.FillRectangle(new SolidBrush(MMI_BLUE), 390, 50+250-height, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, 390, 50, 40, 250);

				//Beschriftung
				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			
				DrawFrame(ref pg, 330, 15, 130, 320);
	
				//DrawFrameRaised(ref pg, 358, 305, 82, 25);

				for(int i = 0; i < 13; i++)
				{
					pg.SmoothingMode = SmoothingMode.None;
					if (Math.IEEERemainder(i, 2) == 0 || i == 0)
					{
						pg.DrawLine(new Pen(BLACK),300+48+30, 299-i*20.82f, 300+59+30, 299-i*20.82f);
						if (i == 0)
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+35+30, 290-i*20.82f);
						else if (i >= 10)
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+10+30, 290-i*20.82f);
						else
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+18+30, 290-i*20.82f);
					}
					else
						pg.DrawLine(new Pen(BLACK),300+54+30, 299-i*20.82f, 300+59+30, 299-i*20.82f);
					pg.SmoothingMode = SMOOTHING_MODE;	
				}

				pg.DrawString("Gesammtstrom", f, new SolidBrush(BLACK), 340, 309);
				pg.DrawString("  A", f, new SolidBrush(BLACK), 353, 62);
			}
		}
		public void DrawOberstromTaurus(ref Graphics pg)
		{
			if (localstate.traction == 1)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.Z_BR) return;
				float height = (localstate.Oberstrom) / 2.4f;

				if (height < 0) height = 0;
				else if (height > 250) height = 250;

				int y = -35;

				pg.FillRectangle(new SolidBrush(MMI_BLUE), 240, 50+250-height+y, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, 240, 50+y, 40, 250);

				//Beschriftung
				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			
				DrawFrameSunken(ref pg, 185, 2, 125, 335+y);
	
				DrawFrameRaised(ref pg, 208, 305+y, 82, 25);

				for(int i = 0; i < 13; i++)
				{
					pg.SmoothingMode = SmoothingMode.None;
					if (Math.IEEERemainder(i, 2) == 0 || i == 0)
					{
						pg.DrawLine(new Pen(BLACK),150+48+30, 299-i*20.82f+y, 150+59+30, 299-i*20.82f+y);
						if (i == 0)
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+35+30, 290-i*20.82f+y);
						else
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+18+30, 290-i*20.82f+y);
					}
					else
						pg.DrawLine(new Pen(BLACK),150+54+30, 299-i*20.82f+y, 150+59+30, 299-i*20.82f+y);
					pg.SmoothingMode = SMOOTHING_MODE;	
				}

				pg.DrawString("Oberstrom", f, new SolidBrush(BLACK), 208, 309+y);
				pg.DrawString("  A", f, new SolidBrush(BLACK), 203, 62+y);
			}
			else
			{
				// Mehrfachtraktion
				float height = (localstate.Oberstrom) / 4.528f * localstate.traction;

				if (height < 0) height = 0;
				else if (height > 265) height = 265;

				int y = -35;

				pg.FillRectangle(new SolidBrush(MMI_BLUE), 390, 50+250-height+y+15-7, 40, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, 390, 50+y-7, 40, 265);

				//Beschriftung
				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			
				DrawFrameSunken(ref pg, 330, 2, 130, 310-10);
	
				//DrawFrameRaised(ref pg, 358, 305, 82, 25);

				for(int i = 0; i < 13; i++)
				{
					pg.SmoothingMode = SmoothingMode.None;
					if ((Math.IEEERemainder(i, 2) == 0 || i == 0) && i != 12)
					{
						pg.DrawLine(new Pen(BLACK,2), 300+48+30, 299-i*22.08f+y+15-7, 300+59+30, 299-i*22.08f+y+15-7);
						if (i == 0)
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+35+30, 290-i*22.08f+y+15-7);
						else if (i >= 10)
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+10+30, 290-i*22.08f+y+15-7);
						else
							pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+18+30, 290-i*22.08f+y+15-7);
					}
					else if (i == 12)
						pg.DrawLine(new Pen(BLACK,2),300+48+30, 299-i*22.08f+y+15-7, 300+59+30, 299-i*22.08f+y+15-7);
					else
						pg.DrawLine(new Pen(BLACK),300+54+30, 299-i*22.08f+y+15-7, 300+59+30, 299-i*22.08f+y+15-7);

					pg.SmoothingMode = SMOOTHING_MODE;	
				}

				if (localstate.Oberstrom > 0)
				{
					pg.FillRectangle(new SolidBrush(MMI_BLUE), 336, 309+y+15-8, 120, 18);
					pg.DrawString("Gesammtstrom", f, new SolidBrush(Color.WhiteSmoke), 340, 309+y+15-8);
				}
				else
                    pg.DrawString("Gesammtstrom", f, new SolidBrush(BLACK), 340, 309+y+15-8);

				DrawFrameRaised(ref pg, 336, 309+y+15-8, 120, 18);

				pg.DrawString("  A", f, new SolidBrush(BLACK), 353, 62+y-7);
			}
		}
		public void DrawTankinhaltEurorunner(ref Graphics pg)
		{
			// Mehrfachtraktion
			float height = 250;

			int y = -35;

			pg.FillRectangle(new SolidBrush(MMI_BLUE), 390, 50+250-height+y+15-7, 40, height);
			// großer Rahmen
			DrawFrameSunken(ref pg, 390, 50+y-7, 40, 265);

			//Beschriftung
			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
		
			DrawFrameSunken(ref pg, 330, 2, 130, 310-10);

			//DrawFrameRaised(ref pg, 358, 305, 82, 25);

			float factor = 25.76f/2f;

			for(int i = 0; i < 21; i++)
			{
				pg.SmoothingMode = SmoothingMode.None;
				if ((Math.IEEERemainder(i, 4) == 0 || i == 0))
				{
					pg.DrawLine(new Pen(BLACK,2), 300+48+30, 299-i*factor+y+15-7, 300+59+30, 299-i*factor+y+15-7);
					if (i == 0)
						pg.DrawString((i*5).ToString(), f, new SolidBrush(BLACK), 300+35+30, 290-i*factor+y+15-7);
					else if (i < 20)
						pg.DrawString((i*5).ToString(), f, new SolidBrush(BLACK), 300+27+30, 290-i*factor+y+15-7);
					else
						pg.DrawString((i*5).ToString(), f, new SolidBrush(BLACK), 300+18+30, 290-i*factor+y+15-7);
				}
				else if (Math.IEEERemainder(i, 2) == 0)
					pg.DrawLine(new Pen(BLACK,2), 300+48+30, 299-i*factor+y+15-7, 300+59+30, 299-i*factor+y+15-7);
				else
					pg.DrawLine(new Pen(BLACK),300+54+30, 299-i*factor+y+15-7, 300+59+30, 299-i*factor+y+15-7);

				pg.SmoothingMode = SMOOTHING_MODE;	
			}

			pg.DrawString("  Tankfüllung", f, new SolidBrush(BLACK), 343, 309+y+15-8);

			DrawFrameRaised(ref pg, 336, 309+y+15-8, 120, 18);

			pg.DrawString(" %", f, new SolidBrush(BLACK), 353, 72+y-7);

			Pen p = new Pen(BLACK); p.DashStyle = DashStyle.Dash;
			pg.DrawLine(p, 300+59+31, 221, 300+59+31+40, 221);
		}

		public void DrawOberstromBombardier(ref Graphics pg)
		{
			if (localstate.traction == 1)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.Z_BR) return;
				float height = (localstate.Oberstrom) / 2.4f;

				if (height < 0) height = 0;
				else if (height > 250) height = 250;


				pg.FillRectangle(new SolidBrush(MMI_BLUE), 240, 50+250-height, 50, height);
				// großer Rahmen
				DrawFrameSunken(ref pg, 240, 50, 50, 250);

				//Beschriftung
				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			
				for(int i = 0; i < 13; i++)
				{
					pg.SmoothingMode = SmoothingMode.None;
					if (Math.IEEERemainder(i, 2) == 0 || i == 0)
					{
						pg.DrawLine(new Pen(BLACK),150+48+30, 299-i*20.82f, 150+59+30, 299-i*20.82f);
						if (i == 0)
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+35+30, 290-i*20.82f);
						else
							pg.DrawString((i*50).ToString(), f, new SolidBrush(BLACK), 150+18+30, 290-i*20.82f);
					}
					else
						pg.DrawLine(new Pen(BLACK),150+54+30, 299-i*20.82f, 150+59+30, 299-i*20.82f);
					pg.SmoothingMode = SMOOTHING_MODE;	
				}

				pg.DrawString("I", f, new SolidBrush(BLACK), 87+170, 305);
				pg.DrawString("f", f, new SolidBrush(BLACK), 98+170, 310);
				pg.DrawString("  A", f, new SolidBrush(BLACK), 203, 20);
		 }
		 else
		 {
			 float height = (localstate.Oberstrom) / 4.8f * localstate.traction;

			 if (height < 0) height = 0;
			 else if (height > 250) height = 250;


			 pg.FillRectangle(new SolidBrush(MMI_BLUE), 400, 50+250-height, 50, height);
			 // großer Rahmen
			 DrawFrameSunken(ref pg, 400, 50, 50, 250);

			 //Beschriftung
			 Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			 for(int i = 0; i < 13; i++)
			 {
				 pg.SmoothingMode = SmoothingMode.None;
				 if (Math.IEEERemainder(i, 2) == 0 || i == 0)
				 {
					 pg.DrawLine(new Pen(BLACK),300+48+40, 299-i*20.82f, 300+59+40, 299-i*20.82f);
					 if (i == 0)
						 pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+35+40, 290-i*20.82f);
					 else if (i >= 10)
						 pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+10+40, 290-i*20.82f);
					 else
						 pg.DrawString((i*100).ToString(), f, new SolidBrush(BLACK), 300+18+40, 290-i*20.82f);
				 }
				 else
					 pg.DrawLine(new Pen(BLACK),300+54+40, 299-i*20.82f, 300+59+40, 299-i*20.82f);
				 pg.SmoothingMode = SMOOTHING_MODE;	
			 }

			 pg.DrawString("Summe", f, new SolidBrush(BLACK), 397, 305);
			 pg.DrawString("  A", f, new SolidBrush(BLACK), 363, 20);
		 }
		}
		public void DrawStörung(ref Graphics pg)
		{
			if (INIT) 
			{
				if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
					INIT = false;
			}

			string s = "";
			int y, x;
			Pen p_dg_3 = new Pen(new SolidBrush(DARK), 2);
			Pen p_dg_1 = new Pen(new SolidBrush(DARK), 1);
			Pen p_ws_3 = new Pen(new SolidBrush(BRIGHT), 2);
			Pen p_ws_1 = new Pen(new SolidBrush(BRIGHT), 1);

			Font f = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Millimeter);

			Color StTextColor = BLACK;
			if (inverse_display) StTextColor = MMI_BLUE;

			if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20)
			{
				y = 20-50;
				x = -435;
				if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
				{
					if (STÖRUNG_BG == Color.Gold)
					{
						pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438+x, 338+y, 312, 30);
					}
					DrawFrame(ref pg, 438+x, 338+y, 312, 30);
				
					Störung st = localstate.störungmgr.Current;

					s = "St in "+st.Name;
					pg.DrawString(s, f, new SolidBrush(StTextColor), 455+x+61, 352+y-10);
					if (IsSiemens() && localstate.type != TRAIN_TYPE.ER20) DrawStatusStörRahmen(ref pg, true, false);
				}
				else if (localstate.störungmgr.GetPassives().Count > 1 || INIT)
				{
					if (STÖRUNG_BG == Color.Gold)
					{
						if (inverse_display)
							pg.FillRectangle(new SolidBrush(MMI_ORANGE), 438+x, 338+y, 312, 30);
						else
							pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438+x, 338+y, 312, 30);
					}	
					DrawFrame(ref pg, 438+x, 338+y, 312, 30);
					s = "          St";
					pg.DrawString(s, f, new SolidBrush(StTextColor), 462+x+61, 352+y-10);
					if (IsSiemens() && localstate.type != TRAIN_TYPE.ER20) DrawStatusStörRahmen(ref pg, true, false);
				}
			}
			else
			{
				y = 20;
				x = 0;
				if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
				{
					if (STÖRUNG_BG == Color.Gold)
					{
						pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438, 338+y, 190, 50);
					}
				
					Störung st = localstate.störungmgr.Current;

					s = "St in "+st.Name;
					pg.DrawString(s, f, new SolidBrush(StTextColor), 455, 352+y);
					if (IsSiemens()) DrawStatusStörRahmen(ref pg, true, false);
				}
				else if (localstate.störungmgr.GetPassives().Count > 1 || INIT)
				{
					if (STÖRUNG_BG == Color.Gold)
					{
						if (inverse_display)
							pg.FillRectangle(new SolidBrush(MMI_ORANGE), 438, 338+y, 190, 50);
						else
							pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438, 338+y, 190, 50);
					}				
					s = "          St";
					pg.DrawString(s, f, new SolidBrush(StTextColor), 462, 352+y);
					if (IsSiemens()) DrawStatusStörRahmen(ref pg, true, false);
				}
			}
				

			pg.SmoothingMode = SMOOTHING_MODE;

		}
		public void DrawStatus(ref Graphics pg)
		{
			if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20)
				return;


			int y = 20;
			Pen p_dg_3 = new Pen(new SolidBrush(DARK), 2);
			Pen p_dg_1 = new Pen(new SolidBrush(DARK), 1);
			Pen p_ws_3 = new Pen(new SolidBrush(BRIGHT), 2);
			Pen p_ws_1 = new Pen(new SolidBrush(BRIGHT), 1);

			Font f = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Millimeter);

			Color StaTextColor = Color.White;
			if (inverse_display) StaTextColor = Color.WhiteSmoke;

			Color StöTextColor = BLACK;
			if (inverse_display) StöTextColor = MMI_BLUE;

			if (IsStatus())
			{
				string s = GetStatusText();

				if	(s.Substring(0,1) == "!" &&IsSiemens())
				{
					pg.FillRectangle(new SolidBrush(Color.Gold), 196, 338+y, 242, 50);
					s = s.Substring(1, s.Length-1);
					pg.DrawString(s, f, new SolidBrush(StöTextColor), 208, 352+y);
				}
				else
				{
					pg.FillRectangle(new SolidBrush(STATUS_BLUE), 196, 338+y, 242, 50);
					if (s.Substring(0,1) == "!") s = s.Substring(1, s.Length-1);
					pg.DrawString(s, f, new SolidBrush(StaTextColor), 208, 352+y);
				}
				
				

				if (IsSiemens()) DrawStatusStörRahmen(ref pg, false, true);
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		public void DrawStatusStörRahmen(ref Graphics pg, bool störung, bool status)
		{
			int y = 20;
			//Stör
			if (störung) DrawFrame(ref pg, 438, 338+y, 190, 50);

			//Status
			if (status) DrawFrame(ref pg, 196, 338+y, 242, 50);

			if (IsBombardier())
			{	
				//kleine Felder
				DrawFrame(ref pg, 145, 338+y, 50, 50);
				DrawFrame(ref pg, 94, 338+y, 50, 50);

				// großer Rahmen
				DrawFrame(ref pg, 0, 335+y, 630, 56);
			}
			else if (IsADtranz())
			{
				//kleine Felder
				DrawFrame(ref pg, 145, 338+y, 50, 50);
				DrawFrame(ref pg, 4, 338+y, 140, 50);

				// großer Rahmen
				DrawFrame(ref pg, 0, 335+y, 630, 56);
			}

		}
		public void DrawZugkraftBalken(ref Graphics pg)
		{
			if (IsBombardier())
			{
				if (localstate.traction > 1)
					DrawZugkraftBalkenBombardierMehrfach(ref pg);
				else
					DrawZugkraftBalkenBombardierEinfach(ref pg);
			}
			else if (localstate.type == TRAIN_TYPE.BR182)
			{
				if (localstate.traction > 1)
					DrawZugkraftBalkenTaurusMehrfach(ref pg);
				else
					DrawZugkraftBalkenTaurusEinfach(ref pg);
			}
			else if (localstate.type == TRAIN_TYPE.ER20)
			{
				DrawZugkraftBalkenEurorunnerMehrfach(ref pg);
			}
			else
			{
				if (localstate.traction > 1)
					DrawZugkraftBalkenSiemensMehrfach(ref pg);
				else
					DrawZugkraftBalkenSiemensEinfach(ref pg);
			}
		}
		public void DrawZugkraftBalkenBombardierEinfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -1.5625f;
			}
			else
			{
				height = (localstate.Zugkraft) * 3.3333f;
			}

			if (height > 250) height = 250;

			//Beschriftung
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-", f, new SolidBrush(MMI_BLUE), 490, 225);
			pg.DrawString("Brems-", f, new SolidBrush(MMI_ORANGE), 540, 225);
			pg.DrawString("/", f, new SolidBrush(BLACK), 534, 225);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 525, 245);
			
			pg.DrawString("Summe:", f, new SolidBrush(BLACK), 510, 270);
			f = new Font("Arial", 6f, FontStyle.Bold, GraphicsUnit.Millimeter);
			
			DrawFrameRaised(ref pg, 497, 267, 103, 28);
			
			int zk = 0; 
			Brush b;
			if (localstate.E_Bremse <= 0)
			{           
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f));
				}
				catch (Exception) {}
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft));
				}
				catch (Exception) {}
				b = new SolidBrush(MMI_ORANGE);
			}
			
			pg.DrawString("kN", f, b, 560, 302);
			
			if (zk >= 100)
				pg.DrawString(zk.ToString(), f, b, 515, 302);
			else if (zk >= 10)
				pg.DrawString(zk.ToString(), f, b, 528, 302);
			else
				pg.DrawString(zk.ToString(), f, b, 541, 302);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 1; i <= 4; i++)
			{
				pg.FillRectangle(b, i*110-60, 40+250-height, 50, height);
				DrawFrameSunken(ref pg, i*110-60, 40, 50, 250);
				pg.FillRectangle(b, i*110-60, 292, 50, 7);
				DrawFrameSunken(ref pg, i*110-60, 292, 50, 7);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 16; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 4) == 0 || k == 0 || k == 15)
						{
							pg.DrawLine(new Pen(BLACK), i*110-60-12, 290-k*16.6667f, i*110-60, 290-k*16.6667f);
							if (k==0)
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 282-k*16.6667f);
							else
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 282-k*16.6667f);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 290-k*16.6667f, i*110-60, 290-k*16.6667f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 17; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*110-60-12, 290-k*15.625f, i*110-60, 290-k*15.625f);
							if (k < 3)
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 282-k*15.625f);
							else
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 282-k*15.625f);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 290-k*15.6251f, i*110-60, 290-k*15.625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}

				pg.DrawString("kN", f, new SolidBrush(BLACK), i*110-60-35, 10);
				f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
				
				pg.DrawString("FM "+i.ToString(), f, new SolidBrush(BLACK), i*110-60+7, 305);

				//Pfeil
				int iheight = 0;
				try
				{
					if (localstate.E_Bremse <= 0)
					{
						iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 6.25f);
						//b = new SolidBrush(MMI_BLUE);
					}
					else
					{
						iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
						//b = new SolidBrush(MMI_ORANGE);
					}
				}
				catch (Exception) {}

				Point[] p = {new Point(i*110-60,290-iheight), new Point(i*110-60-22,290-iheight+7), new Point(i*110-60-14,290-iheight), new Point(i*110-60-22,290-iheight-7)};
				pg.FillPolygon(b, p);
			}		
		}
		public void DrawZugkraftBalkenBombardierMehrfach(ref Graphics pg)
		{					   
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -1.5625f;
			}
			else
			{
				height = (localstate.Zugkraft) * 3.3333f;
			}

			if (height > 250) height = 250;

			//Beschriftung
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-", f, new SolidBrush(MMI_BLUE), 490, 225);
			pg.DrawString("Brems-", f, new SolidBrush(MMI_ORANGE), 540, 225);
			pg.DrawString("/", f, new SolidBrush(BLACK), 534, 225);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 525, 245);
			
			pg.DrawString("Summe:", f, new SolidBrush(BLACK), 510, 270);
			f = new Font("Arial", 6f, FontStyle.Bold, GraphicsUnit.Millimeter);
			
			DrawFrameRaised(ref pg, 497, 267, 103, 28);
			
			int zk = 0; 
			Brush b;
			if (localstate.Zugkraft >= 0)
			{           
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_ORANGE);
			}
			
			pg.DrawString("kN", f, b, 560, 302);
			
			if (zk >= 100)
				pg.DrawString(zk.ToString(), f, b, 515, 302);
			else if (zk >= 10)
				pg.DrawString(zk.ToString(), f, b, 528, 302);
			else
				pg.DrawString(zk.ToString(), f, b, 541, 302);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 1; i <= localstate.traction; i++)
			{
				pg.FillRectangle(b, i*86-50, 50+250-height, 50, height);
				DrawFrameSunken(ref pg, i*86-50, 50, 50, 250);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 17; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*86-50-10, 300-k*15.625f, i*86-50, 300-k*15.625f);
							if (k==0)
								pg.DrawString((k*20).ToString(), f, new SolidBrush(BLACK), i*86-50-21, 292-k*15.625f);
							else if (k > 5)
								pg.DrawString((k*20).ToString(), f, new SolidBrush(BLACK), i*86-50-37, 292-k*15.625f);
							else
								pg.DrawString((k*20).ToString(), f, new SolidBrush(BLACK), i*86-50-29, 292-k*15.625f);
						}
						//else
						pg.DrawLine(new Pen(BLACK),i*86-50-6, 300-k*15.625f, i*86-50, 300-k*15.625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 17; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*86-50-12, 300-k*15.625f, i*86-50, 300-k*15.625f);
							if (k == 0)
								pg.DrawString((k*10f).ToString(), f, new SolidBrush(BLACK), i*86-50-21, 292-k*15.625f);
							else if (k > 9)
								pg.DrawString((k*10).ToString(), f, new SolidBrush(BLACK), i*86-50-37, 292-k*15.625f);
							else
								pg.DrawString((k*10f).ToString(), f, new SolidBrush(BLACK), i*86-50-29, 292-k*15.625f);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*86-50-6, 300-k*15.6251f, i*86-50, 300-k*15.625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}

				pg.DrawString("kN", f, new SolidBrush(BLACK), i*86-50-35, 20);
				f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
				
				pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(BLACK), i*86-50+2, 305);

				//Pfeil
				int iheight = 0;
				if (localstate.E_Bremse <= 0)
				{
					try
					{
						iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 6.25f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					try
					{
						iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*86-50, 300-iheight), new Point(i*86-50-20, 300-iheight+7), new Point(i*86-50-12, 300-iheight), new Point(i*86-50-20, 300-iheight-7)};
				pg.FillPolygon(b, p);
			}
		}
		public void DrawZugkraftBalkenSiemensEinfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -1.5625f;
			}
			else
			{
				height = (localstate.Zugkraft) * 2.77778f;
			}

			if (height > 250f) height = 250f;

			//Beschriftung
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-", f, new SolidBrush(MMI_BLUE), 490, 225);
			pg.DrawString("Brems-", f, new SolidBrush(MMI_ORANGE), 540, 225);
			pg.DrawString("/", f, new SolidBrush(BLACK), 534, 225);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 525, 245);
			
			pg.DrawString("Summe:", f, new SolidBrush(BLACK), 510, 270);
			f = new Font("Arial", 6f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, new SolidBrush(BLACK), 560, 302);

			int zk = 0; 
			Brush b;
			if (localstate.Zugkraft >= 0f)
			{           
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f));
				}
				catch (Exception) {}
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft));
				}
				catch (Exception) {}
				b = new SolidBrush(MMI_ORANGE);
			}

			if (zk >= 100)
				pg.DrawString(zk.ToString(), f, b, 515, 302);
			else if (zk >= 10)
				pg.DrawString(zk.ToString(), f, b, 528, 302);
			else
				pg.DrawString(zk.ToString(), f, b, 541, 302);


			DrawFrame(ref pg, 495, 295, 105, 40);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for(int i = 1; i <= 4; i++)
			{
				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*110-60, 50+250-height, 40, height);
				else
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*110-60, 50+250-height, 40, height);
				
				DrawFrameSunken(ref pg, i*110-60, 50, 40, 250);
				// Rand
				DrawFrame(ref pg, i*110-105, 5, 105, 330);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 19; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*110-60-12, 300-k*13.889f, i*110-60, 300-k*13.889f);
							if (k==0)
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 292-k*13.889f);
							else
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 292-k*13.889f);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 300-k*13.889f, i*110-60, 300-k*13.889f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 17; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*110-60-12, 300-k*15.625f, i*110-60, 300-k*15.625f);
							if (k < 3)
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 292-k*15.625f);
							else
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 292-k*15.625f);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 300-k*15.6251f, i*110-60, 300-k*15.625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}


				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("kN", f, new SolidBrush(BLACK), i*110-60-2, 20);
				f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
				
				if (height > 0)
				{
					if (localstate.Zugkraft >= 0)
						pg.FillRectangle(new SolidBrush(MMI_BLUE), i*110-60-40, 308, 94, 22);
					else
						pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*110-60-40, 308, 94, 22);
					pg.DrawString("Fahrmotor "+i.ToString(), f, Brushes.WhiteSmoke, i*110-60-38, 310);
				}
				else
				{
					pg.DrawString("Fahrmotor "+i.ToString(), f, new SolidBrush(BLACK), i*110-60-38, 310);
				}
				DrawFrameRaised(ref g, i*110-60-40, 308, 94, 22);

				//Pfeil
				int iheight = 0;
				if (localstate.E_Bremse <= 0)
				{
					try
					{
						iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 5.208333f);
					}
					catch (Exception) {}
					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					try
					{
						iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
					}
					catch (Exception) {}
					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*110-40,300-iheight), new Point(i*110-10,300-iheight+6), new Point(i*110-10,300-iheight-7)};
				Point[] p2 = {new Point(i*110-40-1,300-iheight), new Point(i*110-10+1,300-iheight+6+1), new Point(i*110-10+1,300-iheight-7-1)};

				Point plus_w_1 = new Point(i*110-35+15,300-iheight); Point plus_w_2 = new Point(i*110-35+21,300-iheight); 
				Point plus_s_1 = new Point(i*110-35+18,300-iheight-3); Point plus_s_2 = new Point(i*110-35+18,300-iheight+3);
				
				pg.FillPolygon(b, p);
				pg.DrawLine(Pens.WhiteSmoke, p[0], p[2]);
				pg.DrawLine(Pens.Gray, p[1], p[2]);
				pg.DrawLine(Pens.Gray, p[0], p[1]);

				pg.DrawLine(Pens.Black, p2[0], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[0], p2[1]);

				pg.DrawLine(Pens.WhiteSmoke, plus_w_1, plus_w_2);
				if (localstate.E_Bremse <= 0)
					pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}
		}
		public void DrawZugkraftBalkenSiemensMehrfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -0.890625f * 2f;
			}
			else
			{
				height = (localstate.Zugkraft * 4) * 0.890625f;
			}

			if (height > 285) height = 285;

			DrawFrame(ref pg, 1, 4, 320, 330);

			//Beschriftung
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-", f, new SolidBrush(MMI_BLUE), 490, 225);
			pg.DrawString("Brems-", f, new SolidBrush(MMI_ORANGE), 540, 225);
			pg.DrawString("/", f, new SolidBrush(BLACK), 534, 225);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 525, 245);
			
			pg.DrawString("Summe:", f, new SolidBrush(BLACK), 510, 270);
			f = new Font("Arial", 6f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, new SolidBrush(BLACK), 560, 302);
			                                                        
			int zk = 0; 
			Brush b;
			if (localstate.Zugkraft >= 0)
			{           
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_ORANGE);
			}

			if (zk >= 1000)
				pg.DrawString(zk.ToString(), f, b, 502, 302);
			else if (zk >= 100)
				pg.DrawString(zk.ToString(), f, b, 515, 302);
			else if (zk >= 10)
				pg.DrawString(zk.ToString(), f, b, 528, 302);
			else
				pg.DrawString(zk.ToString(), f, b, 541, 302);

			DrawFrame(ref pg, 495, 295, 105, 40);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, new SolidBrush(BLACK), 16, 47);

			for(int i = 1; i <= localstate.traction; i++)
			{
				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*68-18, 15+285-height, 40, height);
				else
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*68-18, 15+285-height, 40, height);
				
				DrawFrameSunken(ref pg, i*68-18, 15, 40, 285);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 33; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 5) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*68-18-12, 300-k*8.90625f, i*68-18-6, 300-k*8.90625f);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*10).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*8.90625f);
								else if (k > 7)
									pg.DrawString((k*10).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*8.90625f);
								else
									pg.DrawString((k*10).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*8.90625f);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*8.90625f, i*68-18, 300-k*8.90625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 33; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 5) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK), i*68-18-12, 300-k*8.90625f, i*68-18-6, 300-k*8.90625f);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*8.90625f);
								else if (k > 15)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*8.90625f);
								else
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*8.90625f);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*8.90625f, i*68-18, 300-k*8.90625f);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}

				pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(BLACK), i*68-18, 310);


				//Pfeil
				int iheight = 0;
				if (localstate.E_Bremse <= 0)
				{
					try
					{
						iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 6.6796875f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					try
					{
						iheight = Convert.ToInt32(localstate.E_Bremse * 44.53125f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*68-18+20,300-iheight), new Point(i*68-18+50,300-iheight+6), new Point(i*68-18+50,300-iheight-7)};
				Point[] p2 = {new Point(i*68-18+20-1,300-iheight), new Point(i*68-18+50+1,300-iheight+6+1), new Point(i*68-18+50+1,300-iheight-7-1)};

				Point plus_w_1 = new Point(i*68+8+15,300-iheight); Point plus_w_2 = new Point(i*68+8+21,300-iheight); 
				Point plus_s_1 = new Point(i*68+8+18,300-iheight-3); Point plus_s_2 = new Point(i*68+8+18,300-iheight+3);
				
				pg.FillPolygon(b, p);
				pg.DrawLine(Pens.WhiteSmoke, p[0], p[2]);
				pg.DrawLine(Pens.Gray, p[1], p[2]);
				pg.DrawLine(Pens.Gray, p[0], p[1]);

				pg.DrawLine(Pens.Black, p2[0], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[0], p2[1]);

				pg.DrawLine(Pens.WhiteSmoke, plus_w_1, plus_w_2);
				if (localstate.E_Bremse <= 0)
					pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}
		}
		public void DrawZugkraftBalkenTaurusEinfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -1.5625f;
			}
			else
			{
				height = (localstate.Zugkraft) * 2.5f;
			}

			if (height > 250) height = 250;

			Brush b;
			Font f = new Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-/", f, new SolidBrush(MMI_BLUE), 510, 220);
			pg.DrawString("Brems-", f, new SolidBrush(Color.Gold), 500, 245);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 510, 270);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			int y = -30;
			for(int i = 1; i <= 4; i++)
			{
				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*110-60, 50+250-height+y, 40, height);
				else
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*110-60, 50+250-height+y, 40, height);
				                                                                        
				DrawFrameSunken(ref pg, i*110-60, 50+y, 40, 250);
				// Rand
				DrawFrame(ref pg, i*110-105, 5, 105, 330+y);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 21; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*110-60-12, 300-k*12.566f+y, i*110-60, 300-k*12.566f+y);
							if (k==0)
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 292-k*12.566f+y);
							else if (k != 20)
								pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 292-k*12.566f+y);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 300-k*12.566f+y, i*110-60, 300-k*12.566f+y);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 17; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*110-60-12, 300-k*15.625f+y, i*110-60, 300-k*15.625f+y);
							if (k < 3)
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-24, 292-k*15.625f+y);
							else if (k != 16)
								pg.DrawString((k*2.5f).ToString(), f, new SolidBrush(BLACK), i*110-60-32, 292-k*15.625f+y);
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*110-60-6, 300-k*15.6251f+y, i*110-60, 300-k*15.625f+y);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}


				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("kN", f, new SolidBrush(BLACK), i*110-100-2, 8);
				f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
				
				if (height > 0)
				{
					if (localstate.Zugkraft >= 0)
						pg.FillRectangle(new SolidBrush(MMI_BLUE), i*110-60-40, 308+y, 94, 22);
					else
						pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*110-60-40, 308+y, 94, 22);
					pg.DrawString("Fahrmotor "+i.ToString(), f, Brushes.WhiteSmoke, i*110-60-38, 310+y);
				}
				else
				{
					pg.DrawString("Fahrmotor "+i.ToString(), f, new SolidBrush(BLACK), i*110-60-38, 310+y);
				}
				DrawFrameRaised(ref g, i*110-60-40, 308+y, 94, 22);

				//Pfeil
				int iheight = 0;
				if (localstate.E_Bremse <= 0)
				{
					try
					{
						if (localstate.IsOEBB)
							iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 4.6875f /*5.208333f*/ * (40/30f) * (280f/300f));
						else
							iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 4.6875f /*5.208333f*/);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					if (!(localstate.E_Bremse > 11 || localstate.E_Bremse < 0))
					{
						try
						{
							if (localstate.IsOEBB)
								iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f * (6f/10f));
							else
								iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
						}
						catch (Exception) {}
					}
					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*110-40,300-iheight+y), new Point(i*110-10,300-iheight+6+y), new Point(i*110-10,300-iheight-7+y)};
				Point[] p2 = {new Point(i*110-40-1,300-iheight+y), new Point(i*110-10+1,300-iheight+6+1+y), new Point(i*110-10+1,300-iheight-7-1+y)};

				Point plus_w_1 = new Point(i*110-35+15,300-iheight+y); Point plus_w_2 = new Point(i*110-35+21,300-iheight+y); 
				Point plus_s_1 = new Point(i*110-35+18,300-iheight-3+y); Point plus_s_2 = new Point(i*110-35+18,300-iheight+3+y);
				
				pg.FillPolygon(b, p);
				pg.DrawLine(Pens.WhiteSmoke, p[0], p[2]);
				pg.DrawLine(Pens.Gray, p[1], p[2]);
				pg.DrawLine(Pens.Gray, p[0], p[1]);

				pg.DrawLine(Pens.Black, p2[0], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[0], p2[1]);

				pg.DrawLine(Pens.WhiteSmoke, plus_w_1, plus_w_2);
				if (localstate.E_Bremse <= 0)
					pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}
		}
		public void DrawZugkraftBalkenTaurusMehrfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -0.828125f*2f;
			}
			else
			{
				height = (localstate.Zugkraft * 4) * 0.75714f;
			}

			if (height > 285) height = 285;

			//DrawFrame(ref pg, 1, 4, 320, 310);

			const int y = -20;

			//Beschriftung
			Font f = new Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-/", f, new SolidBrush(MMI_BLUE), 510, 220);
			pg.DrawString("Brems-", f, new SolidBrush(Color.Gold), 500, 245);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 510, 270);

			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			int zk = 0; 
			Brush b;
			if (localstate.Zugkraft >= 0)
			{       
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_ORANGE);
			}

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if ((localstate.type == TRAIN_TYPE.BR182 && localstate.Zugkraft < 0) || (localstate.type == TRAIN_TYPE.ER20 && localstate.FahrstufenSchalter < 6))
                pg.DrawString("kN", f, new SolidBrush(BLACK), 14, 43-7);
			else
				pg.DrawString("kN", f, new SolidBrush(BLACK), 14, 25-7);

			for(int i = 1; i <= localstate.traction; i++)
			{
				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*68-18, 15+285-height+y-7, 40, height);
				else
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*68-18, 15+285-height+y-7, 40, height);
				
				DrawFrameSunken(ref pg, i*68-18, 15-7, 40, 285+y);

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 15; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*68-18-12, 300-k*18.90625f+y-7, i*68-18, 300-k*18.90625f+y-7);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*18.90625f+y-7);
								else if (k > 3)
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*18.90625f+y-7);
								else
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*18.90625f+y-7);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*18.90625f+y-7, i*68-18, 300-k*18.90625f+y-7);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 33; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 5) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*68-18-12, 300-k*8.28125f+y-7, i*68-18-6, 300-k*8.28125f+y-7);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*8.28125f+y-7);
								else if (k > 15)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*8.28125f+y-7);
								else
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*8.28125f+y-7);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*8.28125f+y-7, i*68-18, 300-k*8.28125f+y-7);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}

				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*68-20, 310+y-7, 48, 18);
				else if (localstate.Zugkraft < 0)
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*68-20, 310+y-7, 48, 18);

				if (localstate.Zugkraft == 0)
                    pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(BLACK), i*68-18, 310+y-7);
				else
					pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(Color.WhiteSmoke), i*68-18, 310+y-7);

				//Pfeil
				int iheight = 0;
				if (localstate.E_Bremse <= 0)
				{
					try
					{
						if (localstate.IsOEBB)
                            iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 5.67857f * (40/30f) * (280f/300f));
						else
							iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 5.67857f);

						b = new SolidBrush(MMI_BLUE);
					}
					catch (Exception) {}
				}
				else
				{
					try
					{
						if (localstate.IsOEBB)
                            iheight = Convert.ToInt32(localstate.E_Bremse * 27.62f * 1.5f * (6f/10f));
						else
							iheight = Convert.ToInt32(localstate.E_Bremse * 27.62f * 1.5f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*68-18+20,300-iheight+y-7), new Point(i*68-18+50,300-iheight+6+y-7), new Point(i*68-18+50,300-iheight-7+y-7)};
				Point[] p2 = {new Point(i*68-18+20-1,300-iheight+y-7), new Point(i*68-18+50+1,300-iheight+6+1+y-7), new Point(i*68-18+50+1,300-iheight-7-1+y-7)};

				Point plus_w_1 = new Point(i*68+8+15,300-iheight+y-7); Point plus_w_2 = new Point(i*68+8+21,300-iheight+y-7); 
				Point plus_s_1 = new Point(i*68+8+18,300-iheight-3+y-7); Point plus_s_2 = new Point(i*68+8+18,300-iheight+3+y-7);
				
				pg.FillPolygon(b, p);
				pg.DrawLine(Pens.WhiteSmoke, p[0], p[2]);
				pg.DrawLine(Pens.Gray, p[1], p[2]);
				pg.DrawLine(Pens.Gray, p[0], p[1]);


				pg.DrawLine(new Pen(BLACK), p2[0], p2[2]);
				pg.DrawLine(new Pen(BLACK, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(BLACK, 2), p2[0], p2[1]);

				/*
				pg.DrawLine(Pens.Black, p2[0], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[0], p2[1]);
				*/

				pg.DrawLine(Pens.WhiteSmoke, plus_w_1, plus_w_2);
				if (localstate.E_Bremse <= 0)
                    pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}

			for (int i = 1; i < 5; i++)
			{
				// Rahmen um "Lok x"
				DrawFrameRaised(ref pg, i*68-20, 310+y-7, 48, 18);
			}
		}
		
		public void DrawZugkraftBalkenEurorunnerMehrfach(ref Graphics pg)
		{
			float height = 0f;
			if (localstate.Zugkraft < 0)
			{
				height = (localstate.Zugkraft) * -0.828125f*2f * 1.4f;
			}
			else
			{
				height = (localstate.Zugkraft * 4) * 0.75714f * 1.4f;
			}

			if (height > 285) height = 285;

			//DrawFrame(ref pg, 1, 4, 320, 310);

			const int y = -20;

			//Beschriftung
			Font f = new Font("Arial", 7, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Zug-/", f, new SolidBrush(MMI_BLUE), 510, 220);
			pg.DrawString("Brems-", f, new SolidBrush(Color.Gold), 500, 245);
			pg.DrawString("Kraft", f, new SolidBrush(BLACK), 510, 270);

			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			int zk = 0; 
			Brush b;
			if (localstate.Zugkraft >= 0)
			{       
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				try
				{
					zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
				}
				catch (Exception) {}

				b = new SolidBrush(MMI_ORANGE);
			}

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if ((localstate.type == TRAIN_TYPE.BR182 && localstate.Zugkraft < 0) || (localstate.type == TRAIN_TYPE.ER20 && localstate.FahrstufenSchalter < 6))
				pg.DrawString("kN", f, new SolidBrush(BLACK), 14, 20-7);
			else
				pg.DrawString("kN", f, new SolidBrush(BLACK), 14, 25-7);

			for(int i = 1; i <= localstate.traction; i++)
			{
				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*68-18, 15+285-height+y-7, 40, height);
				else
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*68-18, 15+285-height+y-7, 40, height);
				
				DrawFrameSunken(ref pg, i*68-18, 15-7, 40, 285+y);

				float factor = 18.90625f * 1.4f;
				float neg_factor = 8.28125f * 1.4f;

				// Skala pos
				if (localstate.E_Bremse <= 0)
					for(int k = 0; k < 13; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 2) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*68-18-12, 300-k*factor+y-7, i*68-18, 300-k*factor+y-7);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*factor+y-7);
								else if (k > 3)
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*factor+y-7);
								else
									pg.DrawString((k*25).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*factor+y-7);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*factor+y-7, i*68-18, 300-k*factor+y-7);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}
				else
					for(int k = 0; k < 33; k++)
					{
						pg.SmoothingMode = SmoothingMode.None;
						if (Math.IEEERemainder(k, 5) == 0 || k == 0)
						{
							pg.DrawLine(new Pen(BLACK,2), i*68-18-12, 300-k*neg_factor+y-7, i*68-18-6, 300-k*neg_factor+y-7);
							if (i == 1)
							{
								if (k==0)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-24, 292-k*neg_factor+y-7);
								else if (k > 15)
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-40, 292-k*neg_factor+y-7);
								else
									pg.DrawString((k*5).ToString(), f, new SolidBrush(BLACK), i*68-18-32, 292-k*neg_factor+y-7);
							}
						}
						//else
						pg.DrawLine(new Pen(BLACK), i*68-18-6, 300-k*neg_factor+y-7, i*68-18, 300-k*neg_factor+y-7);
						pg.SmoothingMode = SMOOTHING_MODE;	
					}

				if (localstate.Zugkraft > 0)
					pg.FillRectangle(new SolidBrush(MMI_BLUE), i*68-20, 310+y-7, 48, 18);
				else if (localstate.Zugkraft < 0)
					pg.FillRectangle(new SolidBrush(MMI_ORANGE), i*68-20, 310+y-7, 48, 18);

				if (localstate.Zugkraft == 0)
					pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(BLACK), i*68-18, 310+y-7);
				else
					pg.DrawString("Lok "+i.ToString(), f, new SolidBrush(Color.WhiteSmoke), i*68-18, 310+y-7);

				//Pfeil
				int iheight = 0;
				int steller = localstate.FahrstufenSchalter;
				if (steller > 5)
				{
					steller -= 6;
					try
					{
						iheight = Convert.ToInt32(steller * 5.67857f / 25f * 40f / 300f * 235f * 1.4f);
						b = new SolidBrush(MMI_BLUE);
					}
					catch (Exception) {}
				}
				else
				{
					try
					{
						iheight = Convert.ToInt32((6-steller)  * 27.62f * 1.4f);
					}
					catch (Exception) {}

					b = new SolidBrush(MMI_ORANGE);
				}

				Point[] p = {new Point(i*68-18+20,300-iheight+y-7), new Point(i*68-18+50,300-iheight+6+y-7), new Point(i*68-18+50,300-iheight-7+y-7)};
				Point[] p2 = {new Point(i*68-18+20-1,300-iheight+y-7), new Point(i*68-18+50+1,300-iheight+6+1+y-7), new Point(i*68-18+50+1,300-iheight-7-1+y-7)};

				Point plus_w_1 = new Point(i*68+8+15,300-iheight+y-7); Point plus_w_2 = new Point(i*68+8+21,300-iheight+y-7); 
				Point plus_s_1 = new Point(i*68+8+18,300-iheight-3+y-7); Point plus_s_2 = new Point(i*68+8+18,300-iheight+3+y-7);
				
				pg.FillPolygon(b, p);
				pg.DrawLine(Pens.WhiteSmoke, p[0], p[2]);
				pg.DrawLine(Pens.Gray, p[1], p[2]);
				pg.DrawLine(Pens.Gray, p[0], p[1]);


				pg.DrawLine(new Pen(BLACK), p2[0], p2[2]);
				pg.DrawLine(new Pen(BLACK, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(BLACK, 2), p2[0], p2[1]);

				/*
				pg.DrawLine(Pens.Black, p2[0], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[1], p2[2]);
				pg.DrawLine(new Pen(Color.Black, 2), p2[0], p2[1]);
				*/

				pg.DrawLine(Pens.WhiteSmoke, plus_w_1, plus_w_2);
				if (localstate.E_Bremse <= 0)
					pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}

			for (int i = 1; i < 5; i++)
			{
				// Rahmen um "Lok x"
				DrawFrameRaised(ref pg, i*68-20, 310+y-7, 48, 18);
			}
		}
		
		private void DrawST(ref Graphics pg)
		{
			if (localstate.type == TRAIN_TYPE.BR152 || localstate.type == TRAIN_TYPE.BR189)
			{
				pg.FillRectangle(new SolidBrush(Color.LemonChiffon), 0, 40, 800, 353);
			}

			int y = 60;
			Brush b_ws;

			if (BRIGHT == Color.DarkGray)
                b_ws = new SolidBrush(Color.White);
			else
				b_ws = new SolidBrush(BRIGHT);

			Brush b_ws_alt = new SolidBrush(BLACK);
			Brush b_blue = new SolidBrush(Color.Blue);
			bool add = true;
			if (ht.Count > 0) add = false;
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			int counter = -1; int pos = 0;
			if (localstate.störungmgr.getStörungsCount < 1)
			{
				pg.DrawString("Keine Störung gemeldet!", f, b_ws, y+10 ,60);
			}
			else
			{
				foreach(Störung st in localstate.störungmgr.getStörungen)
				{
					if (st.Priority == int.MaxValue) continue;
					counter++;

					if (add)
					{
						ht.Add(pos, st);
					}

					if (pos == StörPos)
					{
						pg.FillRectangle(b_blue, 20, y+20*counter, 500, 18);
						pg.DrawString(st.Priority.ToString(), f, b_ws, 20, y+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, y+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, y+20*counter);
					}
					else
					{
						pg.DrawString(st.Priority.ToString(), f, b_ws, 20, y+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, y+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, y+20*counter);
					}
					pos++;
				}
				foreach(Störung st in localstate.störungmgr.getPStörungen)
				{
					if (st.Priority == int.MaxValue) continue;
					counter++;

					if (add)
					{
						ht.Add(pos, st);
						ht2.Add(pos, st);
					}

					if (pos == StörPos)
					{
						pg.FillRectangle(b_blue, 20, y+20*counter, 500, 18);
						pg.DrawString("*"+st.Priority.ToString(), f, b_ws, 20, y+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, y+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, y+20*counter);
					}
					else
					{
						pg.DrawString("*"+st.Priority.ToString(), f, b_ws, 20, y+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, y+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, y+20*counter);
					}
					pos++;
				}

			}
		}
		private void DrawV_EQ_0(ref Graphics pg)
		{
			if (localstate.störungmgr.StörStack != null)
			{
				switch(localstate.störungmgr.StörStack.Type)
				{
					case ENUMStörung.S01_ZUSIKomm:
						DrawS01(ref pg, false);
						break;
					case ENUMStörung.S11_ZUSIKomm:
						DrawS11(ref pg, false);
						break;
					case ENUMStörung.S02_Trennschütz:
						DrawS02(ref pg, false);
						break;
				}
			}
		}
		private void DrawV_GR_0(ref Graphics pg)
		{
			if (localstate.störungmgr.StörStack != null)
			{
				switch(localstate.störungmgr.StörStack.Type)
				{
					case ENUMStörung.S01_ZUSIKomm:
						DrawS01(ref pg, true);
						break;
					case ENUMStörung.S11_ZUSIKomm:
						DrawS11(ref pg, false);
						break;
					case ENUMStörung.S02_Trennschütz:
						DrawS02(ref pg, true);
						break;
				}
			}
		}
		private void DrawS01(ref Graphics pg, bool greater)
		{
			Brush b_ws;

			if (BRIGHT == Color.DarkGray)
				b_ws = new SolidBrush(Color.White);
			else
				b_ws = new SolidBrush(BRIGHT);

			Brush b_ws_alt = new SolidBrush(BLACK);
			Brush b_blue = new SolidBrush(Color.Blue);

			int y = 60;

			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);
			string s = "", ip = "";
			IPHostEntry ipep = Dns.GetHostByName(Dns.GetHostName());

			Störung st = localstate.störungmgr.LastStörung;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws_alt, 140, 12);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			if (ipep.AddressList.Length < 1)
			{
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gestört!", f, b_ws_alt, 50, y);
				pg.DrawString("Ihr Rechner hat im Augenblick keine eindeutige IP-Adresse!", f, b_ws_alt, 50, y+20);
				pg.DrawString("- Systemsteuerung benutzen", f, b_ws_alt, 50, y+40);
				pg.DrawString("- Netzwerkkarte oder Loopback-Adapter installieren", f, b_ws_alt, 50, y+60);
				pg.DrawString("- Statische IP-Adresse zuweisen", f, b_ws_alt, 50, y+80);
				pg.DrawString("- Netzwerk aktivieren", f, b_ws_alt, 50, y+100);
				pg.DrawString("- System neu starten", f, b_ws_alt, 50, y+120);
			}
			else
			{
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gestört!", f, b_ws_alt, 50, y);
				pg.DrawString("- ZUSI starten", f, b_ws_alt, 50, y+20);
				pg.DrawString("- ZUSI TCP Server starten und einschalten", f, b_ws_alt, 50, y+40);
				pg.DrawString("- Host: '"+m_conf.Host+"' überprüfen", f, b_ws_alt, 50, y+60);
				pg.DrawString("- Port: "+m_conf.Port.ToString()+" überprüfen", f, b_ws_alt, 50, y+80);
				pg.DrawString("- Automatisches Anmelden des Displays überwachen", f, b_ws_alt, 50, y+100);
				pg.DrawString("- Im Fehlerfall TCP Server ausschalten und wieder einschalten", f, b_ws_alt, 50, y+120);
				pg.DrawString("- Display neu starten", f, b_ws_alt, 50, y+140);
				pg.DrawString("- Nach erfolgreicher Anmeldung, ZUSI verbinden", f, b_ws_alt, 50, y+160);
				pg.DrawString("- Feld 'Angeforderte Größen' beobachten", f, b_ws_alt, 50, y+180);

				/*				int counter = -1;

				

								foreach (IPAddress add in ipep.AddressList)
								{
									counter++;
									ip = add.ToString();
									if (counter < 1)
									{
										pg.DrawString("- Adresse: "+ip+" und Port: "+m_conf.Port.ToString()+" eintragen", f, b_ws, 50, 120+20*counter);
									}
									else
									{
										pg.DrawString("- oder Adresse: "+ip+" eintragen", f, b_ws, 50, 120+20*counter);
									}
								}	 */

			}
		}

		private void DrawS11(ref Graphics pg, bool greater)
		{
			Brush b_ws;

			if (BRIGHT == Color.DarkGray)
				b_ws = new SolidBrush(Color.White);
			else
				b_ws = new SolidBrush(BRIGHT);

			Brush b_ws_alt = new SolidBrush(BLACK);
			Brush b_blue = new SolidBrush(Color.Blue);

			int y = 60;

			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);
			string s = "";

			Störung st = localstate.störungmgr.LastStörung;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws_alt, 140, 12);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gestört!", f, b_ws_alt, 50, y);
			pg.DrawString("ZUSI ist bereits mit dem TCP Server verbunden!", f, b_ws_alt, 50, y+20);
			pg.DrawString("- Verbindung zwischen ZUSI und TCP Server trennen", f, b_ws_alt, 50, y+40);
			pg.DrawString("- Display meldet sich automatisch an Server an", f, b_ws_alt, 50, y+60);
			pg.DrawString("- ZUSI wieder verbinden", f, b_ws_alt, 50, y+80);
		}

		private void DrawS02(ref Graphics pg, bool greater)
		{
			Brush b_ws;

			if (BRIGHT == Color.DarkGray)
				b_ws = new SolidBrush(Color.White);
			else
				b_ws = new SolidBrush(BRIGHT);

			Brush b_ws_alt = new SolidBrush(BLACK);
			Brush b_blue = new SolidBrush(Color.Blue);
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			Störung st = localstate.störungmgr.StörStack;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws, 20, 1);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			pg.DrawString("Die Trennschütze in Ihrem Triebfahrzeug sind gefallen!", f, b_ws, 50, 40);
			if (greater)
			{
				pg.DrawString("- Zug anhalten", f, b_ws, 50, 60);
				pg.DrawString("- Zugkraft auf Null", f, b_ws, 50, 80);
				pg.DrawString("- Hauptschalter auschalten", f, b_ws, 50, 100);
				pg.DrawString("- kurz warten", f, b_ws, 50, 120);
				pg.DrawString("- Hauptschalter wieder einschalten", f, b_ws, 50, 140);
				pg.DrawString("- Zugkraft aufschalten", f, b_ws, 50, 160);
			}
			else
			{
				pg.DrawString("- Zugkraft auf Null", f, b_ws, 50, 60);
				pg.DrawString("- Hauptschalter auschalten", f, b_ws, 50, 80);
				pg.DrawString("- kurz warten", f, b_ws, 50, 100);
				pg.DrawString("- Hauptschalter wieder einschalten", f, b_ws, 50, 120);
				pg.DrawString("- Zugkraft aufschalten", f, b_ws, 50, 140);
			}
		}
		public void UpdateScreen()
		{
			if (!something_changed)
				return;

			something_changed = false;

			if (m_conf.DoubleBuffer)
				BR185Control_Paint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0,0,this.Width, this.Height)));
			else
				this.Refresh();

			#region alter Code
			//GC.Collect();
			/*
			if (!this.IsDisposed)
			{
				try
				{
					if (something_changed)
					{
						if (!graph_main.CanDoubleBuffer())
							return;

						graph_main.g.Clear(Color.Black);

						if (localstate.LM_LZB_B) 
						{
							LZB_B.Draw(ref graph_main);	
						}
			
						PZB_AKTIV.Draw(ref graph_main);

						if (localstate.LM_LZB_Ü)
							UpdateLZB();
						else
							UpdatePZB();

						if (localstate.Netz == NETZ.SBB)
							UpdateINTEGRA();

						UpdateMaschLM();  

						DrawLines();

						DrawLZBAnzeige();

						DrawGeschwindigkeit();
						SetMaschinenTeil();

						graph_main.Render(this.CreateGraphics());
					}
					something_changed = false;
				}
				catch (Exception e) {}
			}
			*/
			#endregion
		}

		public void DrawDatum(ref Graphics pg)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.G || localstate.DISPLAY == CURRENT_DISPLAY.Zugbesy || localstate.DISPLAY == CURRENT_DISPLAY.Z_BR)
			{

				string s = MMI.EBuLa.Tools.Misc.getDateString(vtime);

				

				if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20)
				{
					Font f = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Millimeter);
					pg.DrawString(s, f, new SolidBrush(BLACK), 480, 18);
				}
				else
				{
					// Datum oben rechts
					DrawFrame(ref pg, 468, 1, 160, 30);

					Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString(s, f, new SolidBrush(BLACK), 498, 8);
				}
			}
			else
			{
				// Datum oben links
				DrawFrame(ref pg, 1, 1, 120, 38);

				string s = MMI.EBuLa.Tools.Misc.getDateString(vtime);

				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

				pg.DrawString(s, f, new SolidBrush(BLACK), 10, 12);
			}

		}
		public void DrawZug(ref Graphics pg)
		{
			string help = localstate.Zugnummer;
			int length = help.Length;

			for (int i = 0; i < length; i++)
			{
				if (help[0] != '0') break;
				help = help.Remove(0,1);
			}

			string s = "Zug " + help;

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			if (localstate.type == TRAIN_TYPE.BR182)
			{
				DrawFrame(ref pg, 468, 118, 160, 30);
				if (localstate.Zugnummer == "<INIT>") return;

				pg.DrawString("Zug", f, new SolidBrush(BLACK), 475, 125);
				if (help.Length > 5)
					pg.DrawString(help, f, new SolidBrush(BLACK), 560, 125);
				else if (help.Length > 4)
					pg.DrawString(help, f, new SolidBrush(BLACK), 568, 125);
				else if (help.Length > 3)
					pg.DrawString(help, f, new SolidBrush(BLACK), 576, 125);
				else if (help.Length > 2)
					pg.DrawString(help, f, new SolidBrush(BLACK), 584, 125);
				else if (help.Length > 1)
					pg.DrawString(help, f, new SolidBrush(BLACK), 592, 125);
				else
					pg.DrawString(help, f, new SolidBrush(BLACK), 600, 125);
			}
			else
			{
				// Zugnummer
				DrawFrame(ref pg, 468, 189, 160, 30);
				if (localstate.Zugnummer == "<INIT>") return;
				pg.DrawString(s, f, new SolidBrush(BLACK), 510, 196);
			}
		}
		public void DrawVerbrauch(ref Graphics pg)
		{
			bool kwh = true; string help;
			//kwh = (localstate.Energie > 999f);                            
			
			if (kwh)
                help = Math.Round(localstate.Energie/1000d, 0).ToString();
			else
				help = Math.Round(localstate.Energie, 0).ToString();

			Font f = new Font("Arial", 5f, FontStyle.Bold, GraphicsUnit.Millimeter);

			if (localstate.type == TRAIN_TYPE.BR182)
			{
				DrawFrame(ref pg, 468, 150, 160, 32);
				//DrawFrame(ref pg, 468, 184, 160, 32);
                
				if (kwh)
					pg.DrawString("kWh", f, new SolidBrush(BLACK), 575, 155);
				else
					pg.DrawString("Wh", f, new SolidBrush(BLACK), 575, 155);

				const int x = 45;

				help = InsertPoints(help);

				if (help.Length > 13)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 483-x, 155);
				else if (help.Length > 12)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 492-x, 155);
				else if (help.Length > 11)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 501-x, 155);
				else if (help.Length > 10)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 510-x, 155);
			    else if (help.Length > 9)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 519-x, 155);
				else if (help.Length > 8)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 528-x, 155);
				else if (help.Length > 7)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 537-x, 155);
				else if (help.Length > 6)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 546-x, 155);
				else if (help.Length > 5)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 555-x, 155);
				else if (help.Length > 4)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 564-x, 155);
				else if (help.Length > 3)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 573-x, 155);
				else if (help.Length > 2)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 582-x, 155);
				else if (help.Length > 1)
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 591-x, 155);
				else
					pg.DrawString(help, f, new SolidBrush(MMI_BLUE), 600-x, 155);
			}
			else
			{
				//DrawFrame(ref pg, 468, 189, 160, 30);
				//pg.DrawString(help, f, new SolidBrush(BLACK), 510, 196);
			}
		}
		public void DrawLinie(ref Graphics pg)
		{
			// Rahmen oben
			DrawFrame(ref pg, 1, 1, 505, 38);

			string s = "  nicht Einsteigen / Werkstatt";

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString(s, f, new SolidBrush(BLACK), 8, 12);

		}
		public void SetButtons()
		{
			if (localstate.type == TRAIN_TYPE.BR182)
			{
				switch(localstate.DISPLAY)
				{
					case CURRENT_DISPLAY.G:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";//"W";
						buttons[3,0] = ""; buttons[3,1] = "";
						buttons[4,0] = "Status"; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = "";
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = ""; buttons[7,1] = "";
						buttons[8,0] = " Ein-"; buttons[8,1] = "gabe";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Z_BR:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";//"W";
						buttons[3,0] = " U/I-"; buttons[3,1] = "Prim";
						buttons[4,0] = "Status"; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = "";
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = "SPUS"; 
						if (localstate.SPUS)
							buttons[7,1] = "  aus";
						else
							buttons[7,1] = "  ein";
						buttons[8,0] = " Ein-"; buttons[8,1] = "gabe";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = ""; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Zugbesy:
						buttons[1,0] = ""; buttons[1,1] = "";//"ZDE";
						buttons[2,0] = "Zug-/"; buttons[2,1] = "Tf-Nr.";
						buttons[3,0] = ""; buttons[3,1] = "";
						buttons[4,0] = ""; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = ""; 
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = " DSK"; buttons[7,1] = "";
						buttons[8,0] = ""; buttons[8,1] = "";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Zug_Tf_Nr:
						buttons[1,0] = "   1"; buttons[1,1] = "";
						buttons[2,0] = "   2"; buttons[2,1] = "";
						buttons[3,0] = "   3"; buttons[3,1] = "";
						buttons[4,0] = "   4";	buttons[4,1] = "";
						buttons[5,0] = "   5"; buttons[5,1] = ""; 
						buttons[6,0] = "   6"; buttons[6,1] = "";
						buttons[7,0] = "   7"; buttons[7,1] = "";
						buttons[8,0] = "   8"; buttons[8,1] = "";
						buttons[9,0] = "   9"; buttons[9,1] = "";
						buttons[10,0] = "   0"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.DSK:
						buttons[1,0] = "   1"; buttons[1,1] = "";
						buttons[2,0] = "   2"; buttons[2,1] = "";
						buttons[3,0] = "   3"; buttons[3,1] = "";
						buttons[4,0] = "   4";	buttons[4,1] = "";
						buttons[5,0] = "   5"; buttons[5,1] = ""; 
						buttons[6,0] = "   6"; buttons[6,1] = "";
						buttons[7,0] = "   7"; buttons[7,1] = "";
						buttons[8,0] = "   8"; buttons[8,1] = "";
						buttons[9,0] = "   9"; buttons[9,1] = "";
						buttons[10,0] = "   0"; buttons[10,1] = "";
						break;
					default:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";
						buttons[3,0] = ""; buttons[3,1] = "";
						buttons[4,0] = ""; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = ""; 
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = ""; buttons[7,1] = "";
						buttons[8,0] = ""; buttons[8,1] = "";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
				}
			}
			else if (localstate.type == TRAIN_TYPE.ER20)
			{
				buttons[1,0] = ""; buttons[1,1] = "";
				buttons[2,0] = ""; buttons[2,1] = "";
				buttons[3,0] = ""; buttons[3,1] = "";
				buttons[4,0] = ""; buttons[4,1] = "";
				buttons[5,0] = ""; buttons[5,1] = ""; 
				buttons[6,0] = ""; buttons[6,1] = "";
				buttons[7,0] = ""; buttons[7,1] = "";
				buttons[8,0] = ""; buttons[8,1] = "";
				buttons[9,0] = ""; buttons[9,1] = "";
				buttons[10,0] = ""; buttons[10,1] = "";
			}
			else
			{
				switch(localstate.DISPLAY)
				{
					case CURRENT_DISPLAY.G:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";//"W";
						buttons[3,0] = ""; buttons[3,1] = "";
						if (IsBR101())
						{
							buttons[4,0] = "Zug-";                        
							buttons[4,1] = "Num.";
						}
						else
						{
							buttons[4,0] = "Zug-";
							buttons[4,1] = "Besy";
						}
						buttons[5,0] = ""; buttons[5,1] = "";
						if (IsMMI())
						{
							buttons[6,0] = " Ein";
							buttons[6,1] = "Displ.";
						}
						else
						{
							buttons[6,0] = "";
							buttons[6,1] = "";
						}
						buttons[7,0] = "Z / Br"; buttons[7,1] = "";
						buttons[8,0] = ""; buttons[8,1] = "";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = ""; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Z_BR:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";//"W";
						buttons[3,0] = ""; buttons[3,1] = "";
						if (IsBR101() || IsBombardier())
						{
							buttons[4,0] = "Zug-";                        
							buttons[4,1] = "Num.";
						}
						else
						{
							buttons[4,0] = "";
							buttons[4,1] = "";
						}
						buttons[5,0] = ""; buttons[5,1] = "";
						if (IsMMI())
						{
							buttons[6,0] = " Ein";
							buttons[6,1] = "Displ.";
						}
						else
						{
							buttons[6,0] = "";
							buttons[6,1] = "";
						}
						buttons[7,0] = ""; buttons[7,1] = "";
						buttons[8,0] = ""; buttons[8,1] = "";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Zugbesy:
						buttons[1,0] = ""; buttons[1,1] = "";//"ZDE";
						buttons[2,0] = "Zug-/"; buttons[2,1] = "Tf-Nr.";
						buttons[3,0] = ""; buttons[3,1] = "";
						buttons[4,0] = ""; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = ""; 
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = " DSK"; buttons[7,1] = "";
						if (IsBombardier() || IsADtranz())
						{
							buttons[8,0] = "";//"Prü-fen";
							buttons[8,1] = "";
						}
						else
						{
							buttons[8,0] = "";//Prü-lauf";
							buttons[8,1] = "";
						}
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.Zug_Tf_Nr:
						buttons[1,0] = "   1"; buttons[1,1] = "";
						buttons[2,0] = "   2"; buttons[2,1] = "";
						buttons[3,0] = "   3"; buttons[3,1] = "";
						buttons[4,0] = "   4";	buttons[4,1] = "";
						buttons[5,0] = "   5"; buttons[5,1] = ""; 
						buttons[6,0] = "   6"; buttons[6,1] = "";
						buttons[7,0] = "   7"; buttons[7,1] = "";
						buttons[8,0] = "   8"; buttons[8,1] = "";
						buttons[9,0] = "   9"; buttons[9,1] = "";
						buttons[10,0] = "   0"; buttons[10,1] = "";
						break;
					case CURRENT_DISPLAY.DSK:
						buttons[1,0] = "   1"; buttons[1,1] = "";
						buttons[2,0] = "   2"; buttons[2,1] = "";
						buttons[3,0] = "   3"; buttons[3,1] = "";
						buttons[4,0] = "   4";	buttons[4,1] = "";
						buttons[5,0] = "   5"; buttons[5,1] = ""; 
						buttons[6,0] = "   6"; buttons[6,1] = "";
						buttons[7,0] = "   7"; buttons[7,1] = "";
						buttons[8,0] = "   8"; buttons[8,1] = "";
						buttons[9,0] = "   9"; buttons[9,1] = "";
						buttons[10,0] = "   0"; buttons[10,1] = "";
						break;
					default:
						buttons[1,0] = ""; buttons[1,1] = "";
						buttons[2,0] = ""; buttons[2,1] = "";
						buttons[3,0] = ""; buttons[3,1] = "";
						buttons[4,0] = ""; buttons[4,1] = "";
						buttons[5,0] = ""; buttons[5,1] = ""; 
						buttons[6,0] = ""; buttons[6,1] = "";
						buttons[7,0] = ""; buttons[7,1] = "";
						buttons[8,0] = ""; buttons[8,1] = "";
						buttons[9,0] = ""; buttons[9,1] = "";
						buttons[10,0] = "   G"; buttons[10,1] = "";
						break;
				}
			}
		}	

		public void DrawButtons(ref Graphics pg)
		{
			for(int i = 0; i < 10; i++)
			{
				if (IsADtranz() || IsBombardier())
				{
					// Kasten
					DrawFrame(ref pg, i*63, 410, 62, 49);

					// Text
					string s1 = buttons[i+1,0];
					string s2 = buttons[i+1,1];

					Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

					pg.DrawString(s1, f, new SolidBrush(BLACK), i*63+8, 415);
					pg.DrawString(s2, f, new SolidBrush(BLACK), i*63+8, 435);
				}
				else
				{
					// Kasten
					DrawFrameRaised(ref pg, i*63, 413, 62, 45);
					DrawFrameSunkenSmall(ref pg, i*63+1, 413+1, 62-2, 45-2);

					// Text
					string s1 = buttons[i+1,0];
					string s2 = buttons[i+1,1];

					Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

					pg.DrawString(s1, f, new SolidBrush(BLACK), i*63+8, 415);
					pg.DrawString(s2, f, new SolidBrush(BLACK), i*63+8, 435);
				}
			}
        }
		public void DrawLargeButtons(ref Graphics pg)
		{
			for(int i = 0; i < 10; i++)
			{
				// Kasten
				pg.FillRectangle(new SolidBrush(Misc.FILL_BLACK), i*90, 342, 90, 70); 
			}

			// Feld 1: Teilnehmer
			Font f = new Font("Arial", 3.7f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Teilnehmer", f, Brushes.WhiteSmoke, 5, 345);

			pg.FillRectangle(Brushes.DarkGray, 5, 375, 80, 30);

			f = new Font("Arial", 5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString(localstate.traction.ToString(), f, Brushes.Black, 37, 380);

			// Feld 2: kN oder HS
			if (localstate.LM_HS && localstate.type == TRAIN_TYPE.BR182)
			{
				// "El"-Symbol
				Point[] points1 = {new Point(90+6,342+35), new Point(135,342+1), new Point(180-6,342+35), new Point(135,342+69)};
				Point[] points2 = {new Point(90+6+4,342+35), new Point(135,342+1+4), new Point(180-6-4,342+35), new Point(135,342+69-4)};
				pg.FillPolygon(Brushes.WhiteSmoke, points1);
				pg.FillPolygon(new SolidBrush(MMI_BLUE), points2);
				pg.DrawPolygon(new Pen(Brushes.Black, 2), points2);

				pg.FillRectangle(Brushes.WhiteSmoke, 120, 363, 6, 12);
				pg.DrawRectangle(Pens.Black, 120, 363, 6, 12); 
				pg.FillRectangle(Brushes.WhiteSmoke, 150-6, 363, 6, 12);
				pg.DrawRectangle(Pens.Black, 150-6, 363, 6, 12); 

				pg.FillRectangle(Brushes.WhiteSmoke, 120, 383, 30, 8);
				pg.DrawRectangle(Pens.Black, 120, 383, 30, 8); 
			}
			else if (localstate.Drehzahl < 500 && localstate.type == TRAIN_TYPE.ER20)
			{
				// Text "Motor steht"
				f = new Font("Arial", 7f, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Motor", f, Brushes.WhiteSmoke, 94, 346);
				pg.DrawString("steht", f, Brushes.WhiteSmoke, 98, 374);
			}
			else
			{
				if (localstate.Zugkraft <= 0)
				{
					f = new Font("Arial", 16f, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString("kN", f, Brushes.WhiteSmoke, 88, 342);

					Pen p = new Pen(Brushes.WhiteSmoke, 7);
					p.DashCap = DashCap.Round;
					p.EndCap = LineCap.Triangle;
					pg.DrawLine(p, 90+1, 342+69, 180-1, 342+1);
				}
			}

			// Feld 3: "Fz steht"
			/*
			if (localstate.Geschwindigkeit < 0.5f)
			{
				// Text "Fz steht"
				f = new Font("Arial", 7f, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Fz", f, Brushes.WhiteSmoke, 116+90, 346);
				pg.DrawString("steht", f, Brushes.WhiteSmoke, 98+90, 374);
			}
			*/

			// Feld 4: Zugsammelschiene
			f = new Font("Arial", 5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("1000 V", f, Brushes.WhiteSmoke, 290, 388);

			// Feld 5: Bremse
			if (localstate.C_Druck > 0.1f)
			{
				pg.FillRectangle(Brushes.WhiteSmoke, 381, 363, 48, 25);
				pg.FillEllipse(Brushes.WhiteSmoke, 387, 358, 35, 35);
				pg.DrawEllipse(new Pen(Misc.FILL_BLACK), 387, 358, 35, 35);
				pg.FillEllipse(new SolidBrush(Misc.FILL_BLACK), 390, 361, 29, 29);

				pg.FillRectangle(Brushes.WhiteSmoke, 363, 371, 9, 9);
				pg.FillRectangle(Brushes.WhiteSmoke, 438, 371, 9, 9);

				Point[] p1 = {new Point(370,375-8), new Point(370,375+9), new Point(370+9,375)};
				Point[] p2 = {new Point(370+69,375-8), new Point(370+69,375+9), new Point(370-9+69,375)};
				pg.FillPolygon(Brushes.WhiteSmoke, p1);
				pg.FillPolygon(Brushes.WhiteSmoke, p2);
			}

			f = new Font("Arial", 5.7f, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.Bremsstellung == BREMSSTELLUNG.R_Mg || localstate.Bremsstellung == BREMSSTELLUNG.R)
			{
				pg.DrawString("R", f, Brushes.WhiteSmoke, 393, 363);
			}
			else if (localstate.Bremsstellung == BREMSSTELLUNG.P_Mg || localstate.Bremsstellung == BREMSSTELLUNG.P)
			{
				pg.DrawString("P", f, Brushes.Gold, 394, 363);
			}
			else if (localstate.Bremsstellung == BREMSSTELLUNG.G)
			{
				pg.DrawString("G", f, Brushes.Red, 393, 363);
			}

			// Feld 6: (leer)

			// Feld 7: Zugkraft
			f = new Font("Arial", 6f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, Brushes.WhiteSmoke, 565, 345);

			pg.FillRectangle(Brushes.DarkGray, 545, 375, 80, 30);

			f = new Font("Arial", 5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			
			Color col = MMI_BLUE;
			float kn = localstate.ZugkraftGesammt * localstate.traction;

			if (kn < 0)
			{
				kn *= -1;
				col = Color.Gold;
			}

			if (kn >= 1000)
				pg.DrawString(kn.ToString(), f, new SolidBrush(col), 570, 380);
			else if (kn >= 100)
				pg.DrawString(kn.ToString(), f, new SolidBrush(col), 580, 380);
			else if (kn >= 10)
				pg.DrawString(kn.ToString(), f, new SolidBrush(col), 590, 380);
			else
				pg.DrawString(kn.ToString(), f, new SolidBrush(col), 600, 380);

			for(int i = 0; i < 10; i++)
			{
				// Kasten
				DrawFrameRaised(ref pg, i*90, 342, 90, 70);
				DrawFrameSunkenSmall(ref pg, i*90+1, 342+1, 90-2, 70-2);
			}
		}
		public void FillKasten(ref Graphics pg, int tz, bool fill)
		{
			Color color = MMI_BLUE;
			float zugkraft = localstate.ZugkraftGesammt;

			zugkraft = ReduziereZugkraft(zugkraft, tz);

			int height;
			if (zugkraft < 0)
			{
				color = Color.Firebrick;
				zugkraft *= -1;
				height = Convert.ToInt32(zugkraft * 230/BREMSKRAFT);
				if (height > 160) height = 160;
			}
			else
			{
				//height = Convert.ToInt32(zugkraft * 230/ZUGKRAFT);
				height = Convert.ToInt32(localstate.Fahrstufe * 230/15);
			}
			
			if (height > 230) height = 230;

			pg.FillRectangle(new SolidBrush(color), 70+(tz-1)*90, 230 - height + 60, 20, height);
			pg.FillRectangle(new SolidBrush(color), 96+(tz-1)*90, 230 - height + 60, 20, height);

		}
		
		public void DrawKasten(ref Graphics pg)
		{
			DrawFrame(ref pg, 1, 1, 460, 330);

			for (int tz = 1; tz <= 4; tz++)
			{
				DrawFrameSunkenSmall(ref pg, 70+(tz-1)*90, 60, 20, 230);
				DrawFrameSunkenSmall(ref pg, 96+(tz-1)*90, 60, 20, 230);
			}

			float height = Math.Abs(localstate.FahrstufenSchalter - 15) * 15.333333f;			

			Pen p = new Pen(Color.DarkViolet, 4);
			Pen p2 = new Pen(Color.Violet, 2);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			for (int i = 0; i <= 100; i+=25)
			{
				if (i < 10)
				{
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 42, 282 - i*2.3f);
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 42+371, 282 - i*2.3f);
				}
				else if (i < 100)
				{
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 36, 282 - i*2.3f);
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 36+371, 282 - i*2.3f);
				}
				else
				{
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 28, 282 - i*2.3f);
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 28+372, 282 - i*2.3f);
				}

				pg.SmoothingMode = SmoothingMode.None;
				pg.DrawLine(new Pen(BLACK), 58, 290 - i*2.3f, 70, 290 - i*2.3f);
				pg.DrawLine(new Pen(BLACK), 58+329, 290 - i*2.3f, 70+329, 290 - i*2.3f);
				pg.SmoothingMode = SMOOTHING_MODE;
			}

			pg.SmoothingMode = SmoothingMode.None;

			pg.DrawLine(p2, 55, 230 - height + 60-3, 405, 230 - height + 60-3);
			pg.DrawLine(p, 55, 230 - height + 60, 405, 230 - height + 60);

			pg.SmoothingMode = SMOOTHING_MODE;

			/*
			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("kN", f, new SolidBrush(BLACK), 40+(tz-1)*105+tz, 47);

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);

			for (int i = 0; i < 110; i+=10)
			{           
                if (i <= 0)
                    pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 54+(tz-1)*105+tz, 66 + (150 - i * 1.5f));
				else if (i < 100)
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 48+(tz-1)*105+tz, 66 + (150 - i * 1.5f));
				else
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 42+(tz-1)*105+tz, 66 + (150 - i * 1.5f));

				pg.DrawLine(new Pen(BLACK), 62+(tz-1)*105+tz, 74 + (150 - i * 1.5f), 62+(tz-1)*105+tz+5, 74 + (150 - i * 1.5f));
			}

			// Zugteil			
			DrawFrame(ref pg, 1+(tz-1)*105+tz, 248, 105, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString(tz.ToString()+". Zug", f, new SolidBrush(BLACK), 30+(tz-1)*105+tz, 256);


			// Zugnummer

			string zug = "";

			switch (tz)
			{
				case 1:
					zug = TrainType(localstate.VT612type1);
					break;
				case 2:
					zug = TrainType(localstate.VT612type2);
					break;
				case 3:
					zug = TrainType(localstate.VT612type3);
					break;					
				case 4:
					zug = TrainType(localstate.VT612type4);
					break;					
			}

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			DrawFrame(ref pg, 1+(tz-1)*105+tz, 280, 52, 30);
			pg.DrawString(zug+" 00"+tz.ToString(), f, new SolidBrush(BLACK), 6+(tz-1)*105+tz, 289);
			
			DrawFrame(ref pg, 54+(tz-1)*105+tz, 280, 52, 30);
			pg.DrawString(zug+" 50"+tz.ToString(), f, new SolidBrush(BLACK), 60+(tz-1)*105+tz, 289);
			*/
			
		}
		public void DrawTitle(ref Graphics pg)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					break;
				case CURRENT_DISPLAY.Z_BR:
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					// Rahmen
					DrawFrame(ref pg, 122, 1, 384, 38);
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					// Rahmen
					DrawFrame(ref pg, 122, 1, 384, 38);
					break;
				default:
					// Rahmen
					DrawFrame(ref pg, 122, 1, 384, 38);

					string s = TitleString();

					Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

					pg.DrawString(s, f, new SolidBrush(BLACK), 140, 12);
					break;
			}
		}
		public void DrawZugbeeinflussungssysteme(ref Graphics pg)
		{
			// Rahmen
			if (IsBombardier())
			{
				DrawFrame(ref pg, 5, 5, 400, 190);
				DrawFrame(ref pg, 5, 200, 400, 50);
			}
			else
			{
				DrawFrame(ref pg, 5, 5, 420, 240);
				DrawFrame(ref pg, 5, 250, 420, 50);
				if (localstate.type != TRAIN_TYPE.BR182)
					DrawFrame(ref pg, 5, 305, 420, 30);
			}

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Wirksame Zugdaten:", f, new SolidBrush(BLACK), 30, 20);

			string text = "", info = "";
			for(int i = 0; i < 4; i++)
			{
				DrawFrame(ref pg, 30+i*80, 80, 70, 40);
				if (i == 0)
				{
					text = "BRA"; info = "6";
				}
				else if (i == 1)
				{
					text = "BRH"; 
					if (localstate.Brh < 10)
						info = "00"+localstate.Brh.ToString();
					else if (localstate.Brh < 100)
						info = "0"+localstate.Brh.ToString();
					else
						info = localstate.Brh.ToString();
				}
				else if (i == 2) 
				{
					text = "ZL"; info = "400";
				}
				else
				{
					text = "VMZ"; info = "120";
				}
				pg.DrawString(text, f, new SolidBrush(BLACK), 30+i*80, 55);
				pg.DrawString(info, f, new SolidBrush(BLACK), 38+i*80, 90);
			}

			if (IsBombardier() || IsADtranz())
			{
				if (localstate.Brh > 0f && IsCONNECTED)
					text = "Gültige Zugdaten aus Eingabedaten";
				else if (localstate.Zugnummer == "<INIT>")
					text = "Bitte Zug- und Tf-Nummer eingeben!";
				else
					text = "Keine Verbindung zum LZB Rechner!";
					
				pg.DrawString(text, f, new SolidBrush(BLACK), 10, 165);
				if (localstate.DSK_Gesperrt)
					pg.DrawString("DSK Kurzzeitspeicher geperrt", f, new SolidBrush(BLACK), 60, 215);
			}
			else
			{
				pg.DrawString("Eingestellte Daten", f, new SolidBrush(BLACK), 110, 185);
				if (localstate.LM_LZB_Ü)
					pg.DrawString("LZB-Betrieb", f, new SolidBrush(BLACK), 140, 265);
				else
					pg.DrawString("Kein LZB-Betrieb", f, new SolidBrush(BLACK), 110, 265);
				if (localstate.DSK_Gesperrt)
				{
					if (localstate.type == TRAIN_TYPE.BR182)
						pg.DrawString("DSK Kurzzeitspeicher geperrt", f, new SolidBrush(BLACK), 60, 210);
					else
						pg.DrawString("DSK Kurzzeitspeicher geperrt", f, new SolidBrush(BLACK), 60, 310);
				}
			}

		}
		public void DrawSeitlicheSoftkeys(ref Graphics pg)
		{
			// Seitliche Soft-Keys
			DrawFrame(ref pg, 518, 43, 110, 48);
			DrawFrame(ref pg, 518, 105, 110, 50);
			DrawFrame(ref pg, 518, 170, 110, 50);
			if (localstate.type == TRAIN_TYPE.BR182)
                DrawFrame(ref pg, 518, 235, 110, 102);
			else
				DrawFrame(ref pg, 518, 235, 110, 112);

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (IsBombardier() || IsADtranz())
					{
						pg.DrawString("Grundbild", f, new SolidBrush(BLACK), 525, 56);
						pg.DrawString("zurück", f, new SolidBrush(BLACK), 537, 120);
						pg.DrawString("weiter", f, new SolidBrush(BLACK), 540, 185);

						if (localstate.marker >= 6 && localstate.marker < 12)
							pg.DrawString("Tf-Nr.", f, new SolidBrush(BLACK), 538, 264);
						else if (localstate.marker >= 0 && localstate.marker < 6)
							pg.DrawString("Zug-Nr.", f, new SolidBrush(BLACK), 538, 264);
							
						pg.DrawString("über-", f, new SolidBrush(BLACK), 538, 282);
						pg.DrawString("nehmen", f, new SolidBrush(BLACK), 538, 299);
					}
					else
					{
						pg.DrawString("Grundbild", f, new SolidBrush(BLACK), 525, 55);
						pg.DrawString("Eingabe", f, new SolidBrush(BLACK), 533, 111);
						pg.DrawString("rückwärts", f, new SolidBrush(BLACK), 525, 129);

						pg.DrawString("Eingabe", f, new SolidBrush(BLACK), 533, 176);
						pg.DrawString("vorwärts", f, new SolidBrush(BLACK), 530, 194);

						if (localstate.marker >= 6 && localstate.marker < 12)
							pg.DrawString("Tf-", f, new SolidBrush(BLACK), 548, 250);
						else if (localstate.marker >= 0 && localstate.marker < 6)
							pg.DrawString("Zug-", f, new SolidBrush(BLACK), 548, 250);
							
						pg.DrawString("nummer", f, new SolidBrush(BLACK), 530, 268);
						pg.DrawString("über-", f, new SolidBrush(BLACK), 548, 286);
						pg.DrawString("nehmen", f, new SolidBrush(BLACK), 530, 303);
					}
					break;
				case CURRENT_DISPLAY.DSK:
					if (IsBombardier() || IsADtranz())
					{
						pg.DrawString("Grundbild", f, new SolidBrush(BLACK), 525, 56);
						pg.DrawString("zurück", f, new SolidBrush(BLACK), 537, 120);
						pg.DrawString("weiter", f, new SolidBrush(BLACK), 540, 185);

						pg.DrawString("Kurzzeit-", f, new SolidBrush(BLACK), 538, 264);
						pg.DrawString("speicher", f, new SolidBrush(BLACK), 538, 282);
						if (localstate.DSK_Gesperrt)
							pg.DrawString("entsperren", f, new SolidBrush(BLACK), 518, 299);
						else
							pg.DrawString("sperren", f, new SolidBrush(BLACK), 538, 299);
					}
					else
					{
						pg.DrawString("Grundbild", f, new SolidBrush(BLACK), 525, 55);
						pg.DrawString("Eingabe", f, new SolidBrush(BLACK), 533, 111);
						pg.DrawString("rückwärts", f, new SolidBrush(BLACK), 525, 129);

						pg.DrawString("Eingabe", f, new SolidBrush(BLACK), 533, 176);
						pg.DrawString("vorwärts", f, new SolidBrush(BLACK), 530, 194);

						pg.DrawString("Kurzzeit-", f, new SolidBrush(BLACK), 538, 264);
						pg.DrawString("speicher", f, new SolidBrush(BLACK), 538, 282);
						if (localstate.DSK_Gesperrt)
                            pg.DrawString("entsperren", f, new SolidBrush(BLACK), 518, 299);
						else
							pg.DrawString("sperren", f, new SolidBrush(BLACK), 538, 299);
					}
					break;
			}
		}
		public void DrawZugTfNummer(ref Graphics pg)
		{
			DrawSeitlicheSoftkeys(ref pg);

			// großer Rahmen
			if (localstate.type == TRAIN_TYPE.BR182)
				DrawFrame(ref pg, 1, 43, 505, 294);
			else
				DrawFrame(ref pg, 1, 43, 505, 304);

			DrawZugnummer(ref pg);
			DrawTfNummer(ref pg);
		}
		public void DrawZugnummer(ref Graphics pg)
		{
			DrawZugnummer(ref pg, 90, 110);
		}
		public void DrawTfNummer(ref Graphics pg)
		{
			DrawTfNummer(ref pg, 90, 230);
		}
		public void DrawZugnummer(ref Graphics pg,int x, int y)
		{
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			DrawFrame(ref pg, x, y, 300, 60);
			
			pg.DrawString("Zugnummer", f, new SolidBrush(BLACK), x+90, y-30);

			if (localstate.ZugnummerTemp == "" && localstate.Zugnummer == "<INIT>") localstate.ZugnummerTemp = localstate.Zugnummer;
			if (localstate.ZugnummerTemp == "<INIT>" || localstate.ZugnummerTemp == "") localstate.ZugnummerTemp = "      ";

			string org = localstate.Zugnummer;
			if (org == "<INIT>") org = "000000";
			string nr = localstate.ZugnummerTemp;
			
			//if (localstate.marker > 5) localstate.marker = 0;

			for (int i = 0; i < 6; i++)
			{
				if (localstate.marker == i)
				{
					pg.FillRectangle(new SolidBrush(EINGABE_BACKG), 105+i*48, y+10, 30, 40);
					if (localstate.marker_changed)
						pg.DrawString(nr.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48, y +19);
					else
						pg.DrawString(org.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48, y +19);
				}
				else                                                       
				{
					pg.FillRectangle(Brushes.WhiteSmoke, 105+i*48, y+10, 30, 40);
					if ((i < localstate.marker && localstate.marker_changed))
						if (IsBombardier() || IsADtranz())
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
					else if (!localstate.marker_changed || localstate.marker > 5)
						if (IsBombardier() || IsADtranz())
							pg.DrawString(org.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(org.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
				}

				DrawFrame(ref pg, 105+i*48, y+10, 30, 40);
			}
		}

		public void DrawTfNummer(ref Graphics pg, int x, int y)
		{
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			DrawFrame(ref pg, x, y, 300, 60);
			
			pg.DrawString("Tf-Nummer", f, new SolidBrush(BLACK), x+95, y-30);

			if (localstate.TfnummerTemp == "") localstate.TfnummerTemp = localstate.Tfnummer;
			if (localstate.TfnummerTemp == "<INIT>") localstate.TfnummerTemp = "      ";

			string org = localstate.Tfnummer;
			if (org == "<INIT>" || !localstate.marker_changed2) org = "******";

			string nr = localstate.TfnummerTemp;
			
			for (int i = 0; i < 6; i++)
			{
				if (localstate.marker-6 == i)
				{
					pg.FillRectangle(new SolidBrush(EINGABE_BACKG), 105+i*48, y+10, 30, 40);
					if (localstate.marker_changed2)
						pg.DrawString(nr.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48, y +19);
					else
						pg.DrawString(org.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48, y +19);
				}
				else                                                       
				{
					pg.FillRectangle(Brushes.WhiteSmoke, 105+i*48, y+10, 30, 40);
					if ((i < localstate.marker-6 && localstate.marker_changed2))
						if (IsBombardier() || IsADtranz())
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
					else if (!localstate.marker_changed2 || localstate.marker-6 > 5)
						if (IsBombardier() || IsADtranz())
							pg.DrawString(org.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(org.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
				}

				DrawFrame(ref pg, 105+i*48, y+10, 30, 40);
			}
		}

		public void DrawDSK(ref Graphics pg)
		{
			DrawSeitlicheSoftkeys(ref pg);

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			if (localstate.type == TRAIN_TYPE.BR182)
                DrawFrame(ref pg, 5, 43, 500, 294);
			else
				DrawFrame(ref pg, 5, 43, 500, 304);

			if (localstate.DSK_Gesperrt)
				pg.DrawString("Entsperren des", f, new SolidBrush(BLACK), 180, 80);
			else
				pg.DrawString("Sperren und Sichern des", f, new SolidBrush(BLACK), 140, 80);

			pg.DrawString("Kurzzeitspeichers durch Eingabe", f, new SolidBrush(BLACK), 100, 100);
			string s = DSK_SPERRCODE[0]+"-"+DSK_SPERRCODE[1]+"-"+DSK_SPERRCODE[2]+"-"+DSK_SPERRCODE[3];
			pg.DrawString("von "+s, f, new SolidBrush(BLACK), 205, 120);
			pg.DrawString("Abschließen mit E", f, new SolidBrush(BLACK), 175, 250);
			
			int y  = 165; int x = 70;

			for (int i = 0; i < 4; i++)
			{
				if (localstate.marker == i)
				{
					pg.FillRectangle(new SolidBrush(EINGABE_BACKG), 105+x+i*48, y+10, 30, 40);
					if (localstate.marker_changed2)
						pg.DrawString(localstate.DSK_BUFFER.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48+x, y +19);
					else
						pg.DrawString(localstate.DSK_BUFFER.Substring(i, 1), f, new SolidBrush(EINGABE), 110+i*48+x, y +19);
				}
				else                                                       
				{
					pg.FillRectangle(Brushes.WhiteSmoke, 105+i*48+x, y+10, 30, 40);
					if (i < localstate.marker)
						if (IsBombardier() || IsADtranz())
							pg.DrawString(localstate.DSK_BUFFER.Substring(i, 1), f, Brushes.Black, 110+i*48+x, y +19);
						else
							pg.DrawString(localstate.DSK_BUFFER.Substring(i, 1), f, Brushes.Blue, 110+i*48+x, y +19);
				}

				DrawFrame(ref pg, 105+i*48+x, y+10, 30, 40);
			} 

			DrawFrame(ref pg, x+80, y, 225, 60);

		}
		public string getLZBDistString(int i)
		{
			switch(i)
			{
				case 1:
					return "4000";
				case 2:
					return "3000";
				case 3:
					return "2000";
				case 4:
					return "1000";
				case 5:
					return "750";
				case 6:
					return "500";
				case 7:
					return "250";
				case 8:
					return "0m";
			}
			return "";
		}
		public void UpdatePZB(ref Graphics graph_main)
		{
			/*
			// --- ZUGART LM --------------------------------
			if (localstate.LM_Zugart_O) 
				PZB_ZA_O.Draw(ref graph_main);
			else if (localstate.LM_Zugart_U) 
				PZB_ZA_U.Draw(ref graph_main);

			if (localstate.LM_Zugart_M) 
				PZB_ZA_M.Draw(ref graph_main);

			// --- 1000Hz LM --------------------------------
			if (localstate.LM_1000Hz && !localstate.LM_LZB_S)
			{
				PZB_1000.Draw(ref graph_main);
				PZB_1000_TEXT.Draw(ref graph_main);
			}

			// --- 500Hz LM --------------------------------
			if (localstate.LM_500Hz && !localstate.LM_LZB_S)
			{
				PZB_500.Draw(ref graph_main);
				PZB_500_TEXT.Draw(ref graph_main);

			}

			// --- Befehl LM --------------------------------
			if (localstate.LM_Befehl && !localstate.LM_LZB_Ü)
			{
				PZB_Befehl.Draw(ref graph_main);

			}

			// --- LZB S LM ------------------------------------
			if (localstate.LM_LZB_S) 
			{
				LZB_S.Draw(ref graph_main);
				LZB_S_TEXT.Draw(ref graph_main);
			}

			*/

		}
		
		public void UpdateLZB(ref Graphics graph_main)
		{
			/*
			// --- 1000Hz LM --------------------------------
			if (localstate.LM_1000Hz && !localstate.LM_LZB_S)
			{
				PZB_1000.Draw(ref graph_main);
			}

			// --- LZB S LM ------------------------------------
			if (localstate.LM_LZB_S) 
			{
				LZB_S.Draw(ref graph_main);
				LZB_S_TEXT.Draw(ref graph_main);
			}

			// --- LZB H LM ------------------------------------
			if (localstate.LM_LZB_H) 
			{
				LZB_H.Draw(ref graph_main);
			}

			// --- LZB G LM ------------------------------------
			if (localstate.LM_LZB_G) 
			{
				LZB_G.Draw(ref graph_main);
				LZB_G_TEXT.Draw(ref graph_main);
			}

			// --- LZB Ende LM ------------------------------------
			if (localstate.LM_LZB_ENDE) 
			{
				LZB_Ende.Draw(ref graph_main);
			}

			// --- LZB B LM ------------------------------------
			if (localstate.LM_LZB_B)
			{
				LZB_B.Draw(ref graph_main);
			}
			*/
		}

		public void UpdateMaschLM(ref Graphics graph_main)
		{
			/*
			// HS und ZS
			if (localstate.LM_HS)
			{
				HS.Draw(ref graph_main);
				ZS.Draw(ref graph_main);
			}

			// SIFA
			if (localstate.LM_Sifa)
				SIFA.Draw(ref graph_main);

			// TÜR
			if (localstate.Türschliesung != TÜRSCHlIESSUNG.KEINE)
			{
				if (localstate.Türschliesung == TÜRSCHlIESSUNG.SAT || localstate.Türschliesung == TÜRSCHlIESSUNG.TAV)
				{
					// TAV oder SAT
					TÜR.Text1 = "TAV";
					if (!localstate.LM_TÜR)
						TÜR.Draw(ref graph_main);
				}
				else
				{
					// TB0 oder ICE
					TÜR.Text1 = "T";
					if (localstate.LM_TÜR)
						TÜR.Draw(ref graph_main);
				}
			}

			// NBÜ EP
			if (localstate.LM_NBÜ_EP)
			{
				if (localstate.TrainType == TRAIN_TYPE.BR146_1 || localstate.TrainType == TRAIN_TYPE.DBpzfa766_1) 
					NBÜ_EP.Draw(ref graph_main);
				if (localstate.Geschwindigkeit > 19f)
					localstate.LM_NBÜ_EP = false;
			}
			*/
		}
		public void UpdateINTEGRA(ref Graphics graph_main)
		{
			/*
			if (localstate.LM_INTEGRA_GELB)
				INTEGRA_GELB.Draw(ref graph_main);
			else 
				INTEGRA_GELB.Clear(ref graph_main);

			if (localstate.LM_INTEGRA_ROT)
				INTEGRA_ROT.Draw(ref graph_main);
			else 
				INTEGRA_ROT.Clear(ref graph_main);
			*/
		}

		private void BR185Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (m_backBuffer == null && USE_DOUBLE_BUFFER)
			{
				m_backBuffer= new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			}

			if (USE_DOUBLE_BUFFER)
				g = Graphics.FromImage(m_backBuffer);
			else
				g = e.Graphics;

			//Paint your graphics on g here

			g.SmoothingMode = SMOOTHING_MODE;
			g.TextRenderingHint = TEXT_MODE;

			if (inverse_display)
				g.Clear(Misc.FILL_BLACK);
			else
				g.Clear(Misc.FILL_GRAY);

			SetButtons();

			DrawDatum(ref g);
			DrawUhr(ref g);

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.ST:
					DrawTitle(ref g);
					DrawST(ref g);
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					DrawTitle(ref g);
					DrawV_EQ_0(ref g);
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					DrawTitle(ref g);
					DrawV_GR_0(ref g);
					break;
				case CURRENT_DISPLAY.G:
					DrawZug(ref g);
					DrawSpannung(ref g);
					DrawOberstrom(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier() || IsADtranz()) DrawStatusStörRahmen(ref g, true, true);
					break;
				case CURRENT_DISPLAY.Z_BR:
					DrawZug(ref g);
					DrawZugkraftBalken(ref g);
					DrawOberstrom(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier() || IsADtranz())	DrawStatusStörRahmen(ref g, true, true);
					DrawVerbrauch(ref g);
					break;
				case CURRENT_DISPLAY.Zugbesy:
					DrawZug(ref g);
					DrawZugbeeinflussungssysteme(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier() || IsADtranz())	DrawStatusStörRahmen(ref g, true, true);					
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					DrawTitle(ref g);
					DrawZugTfNummer(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier() || IsADtranz()) DrawStatusStörRahmen(ref g, true, true);
					break;
				case CURRENT_DISPLAY.DSK:
					DrawTitle(ref g);
					DrawDSK(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					break;
				default:
					DrawTitle(ref g);
					if (IsBombardier() || IsADtranz()) DrawStatusStörRahmen(ref g, true, true);
					break;
			}

			DrawButtons(ref g);
			if (localstate.type == TRAIN_TYPE.BR182 || localstate.type == TRAIN_TYPE.ER20) 
				DrawLargeButtons(ref g);

			/*
			try
			{
				 
				if (device == null) return;

				device.RenderState.CullMode = Microsoft.DirectX.Direct3D.Cull.None;
    
				// Turn off Direct3D lighting
				device.RenderState.Lighting = false;
    
				// Turn on the z-buffer
				device.RenderState.ZBufferEnable = false;

				//texture = Microsoft.DirectX.Direct3D.Texture.FromBitmap(device, m_backBuffer, Microsoft.DirectX.Direct3D.Usage.AutoGenerateMipMap, Microsoft.DirectX.Direct3D.Pool.Managed);
				texture = Microsoft.DirectX.Direct3D.TextureLoader.FromFile(device, Application.StartupPath + 
					@"\Pictures\abb.jpg");
 

				vertexBuffer = new Microsoft.DirectX.Direct3D.VertexBuffer(
					typeof(Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured), 6, device, 0, 
					Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured.Format, Microsoft.DirectX.Direct3D.Pool.Default);

				Microsoft.DirectX.GraphicsStream stm = vertexBuffer.Lock(0, 0, 0);

				Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured[] verts1 =
					new Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured[3];

				verts1[0].X=0; verts1[0].Y=0; verts1[0].Z=0.5f;
				verts1[0].Tu = 0; verts1[0].Tv = 0;
				
				verts1[1].X=100; verts1[1].Y=100; verts1[1].Z=0.5f;
				verts1[1].Tu = 1; verts1[1].Tv = 1;

				verts1[2].X=0; verts1[2].Y=100; verts1[2].Z=0.5f;
				verts1[2].Tu = 0; verts1[2].Tv = 1;
				stm.Write(verts1);

				
				Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured[] verts2 =
					new Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured[3];

				verts2[0].X=0; verts2[0].Y=0; verts2[0].Z=0.5f;
				verts2[0].Tu = 0; verts2[0].Tv = 0;

				verts2[1].X=100; verts2[1].Y=0; verts2[1].Z=0.5f;
				verts2[1].Tu = 1; verts2[1].Tv = 0;
				
				verts2[2].X=100; verts2[2].Y=100; verts2[2].Z=0.5f;
				verts2[2].Tu = 1; verts2[2].Tv = 1;

				stm.Write(verts2);
				
				
				vertexBuffer.Unlock();


				//Clear the backbuffer to a blue color 
				device.Clear(Microsoft.DirectX.Direct3D.ClearFlags.Target, Misc.FILL_GRAY, 1.0f, 0);
				//Begin the scene
				device.BeginScene();
    
				// Rendering of scene objects can happen here
				device.SetTexture(0,texture);
				device.SetStreamSource(0, vertexBuffer, 0);
				device.VertexFormat = Microsoft.DirectX.Direct3D.CustomVertex.PositionTextured.Format;
				device.DrawPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleList, 0, 2);
			

				//End the scene
				device.EndScene();
				device.Present();

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			*/
						
			if (USE_DOUBLE_BUFFER)
			{
				//g.Dispose();

				//Copy the back buffer to the screen
				e.Graphics.DrawImage(m_backBuffer,0,0);

				//m_backBuffer.Dispose();
				e.Graphics.Dispose();
			}
			
			

			//graph_main.Render(this.CreateGraphics());
		}
	
		private void MoveZugkraft()
		{
			while (Math.Abs(localstate.Zugkraft_Thread - localstate.Zugkraft) > 15f)
			{
				if ((localstate.Zugkraft_Thread - localstate.Zugkraft) > 0) // pos
				{
					Monitor.Exit(localstate);
					localstate.Zugkraft_Thread -= 15f;
					Monitor.Exit(localstate);
				}
				else // neg
				{
					Monitor.Exit(localstate);
					localstate.Zugkraft_Thread += 15f;
					Monitor.Exit(localstate);
				}
				something_changed = true;
				Thread.Sleep(40);
			}
			localstate.Zugkraft_Thread = localstate.Zugkraft;
		}

		private void timer_st_Tick(object sender, System.EventArgs e)
		{
			if (timer_st.Interval != 1000) timer_st.Interval = 1000;
			
			if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{
				if (STÖRUNG_BG == Color.Gold)
				{
					STÖRUNG_BG = Color.Black;
					STÖRUNG_FG = Color.Gold;
				}
				else
				{
					STÖRUNG_BG = Color.Gold;
					STÖRUNG_FG = Color.Black;
				}
				// SOUND
				stoerung_counter++;
				if (stoerung_counter == 30)
				{
					stoerung_counter = 0;
					if (IsBombardier() || IsADtranz())
						Sound.PlayMalfunctionBombardierSound();
					else
						Sound.PlayMalfunctionSiemensSound();

				}
			}
			else
			{
				STÖRUNG_BG = Color.Gold;
				STÖRUNG_FG = Color.Black;
			}

			if (localstate.störungmgr.Current.Type != ENUMStörung.NONE || localstate.störungmgr.GetPassives().Count > 1)
				something_changed = true;

			
		}

		private string TrainType(TRAIN_TYPE type)
		{
			/*
			switch (type)
			{
				case VT612TYPE.VT612:
					return "612";
				case VT612TYPE.VT611:
					return "611";
			}
			*/
			return "";
		}
		private string TitleString()
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.ST:
					return "                           Störungsübersicht";
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					return "                          Zug-/Tf-Nr. Eingaben";
				case CURRENT_DISPLAY.DSK:
					return "                            Kurzzeitspeicher";
			}
			
			return "";
		}
		public void Inverse()
		{
			if (!inverse_display)
			{
				DARK = Color.WhiteSmoke;
				BRIGHT = Color.DarkGray;
				BLACK = Color.White;
			}
			else
			{
				DARK = Color.DarkGray;
				BRIGHT = Color.WhiteSmoke;
				BLACK = Color.Black;
			}

			/*
			foreach(Control c in this.p_Buttons.Controls)
			{
				if (c.GetType() == typeof(System.Windows.Forms.Panel) && c.Name.StartsWith("p_Button"))
				{
					if (!inverse_display)
						c.BackColor = Color.Black;
					else
						c.BackColor = Color.LightGray;
				}
				foreach(Control cc in c.Controls)
				{
					if (cc.GetType() == typeof(System.Windows.Forms.Panel) && cc.Name.StartsWith("p_Li"))
					{
						if (!inverse_display)
							cc.BackColor = SystemColors.ControlDark;
						else
							cc.BackColor = SystemColors.ControlLight;
					}
					else if (cc.GetType() == typeof(System.Windows.Forms.Panel) && cc.Name.StartsWith("p_Da"))
					{
						if (inverse_display)
							cc.BackColor = SystemColors.ControlDark;
						else
							cc.BackColor = SystemColors.ControlLight;
					}
					else if (cc.GetType() == typeof(System.Windows.Forms.Label))
					{
						if (inverse_display)
							cc.ForeColor = Color.Black;
						else
							cc.ForeColor = Color.White;
					}

				}

			}
			*/


            
			something_changed = true;
			inverse_display = !inverse_display;
		}
		public bool IsBombardier()
		{
			return (localstate.type == TRAIN_TYPE.BR101_MET || localstate.type == TRAIN_TYPE.BR145 || localstate.type == TRAIN_TYPE.BR146 || localstate.type == TRAIN_TYPE.BR146_1 || localstate.type == TRAIN_TYPE.BR185);
		}
		public bool IsSiemens()
		{
			return (!IsBombardier() && !IsADtranz());
		}
		public bool IsADtranz()
		{
			return localstate.type == TRAIN_TYPE.BR101;
		}
		public bool IsMMI()
		{
			return (localstate.type == TRAIN_TYPE.BR185 || localstate.type == TRAIN_TYPE.BR146_1 || localstate.type == TRAIN_TYPE.BR189);
		}
		public bool IsBR101()
		{
			return (localstate.type == TRAIN_TYPE.BR101 || localstate.type == TRAIN_TYPE.BR101_MET);
		}
		public bool SwitchToMMIAllowed
		{
			get
			{
				   return (IsMMI() && (localstate.DISPLAY == CURRENT_DISPLAY.G || localstate.DISPLAY == CURRENT_DISPLAY.Z_BR));
			}
		}

		public bool IsStatus()
		{
			return (GetStatusText() != "");
		}
		public string GetStatusText()
		{
			string text = "";

			if (localstate.DSK_Gesperrt)
				text = "  DSK gesperrt";
			if (localstate.Zusatzbremse < 2)
				text = "  FspBr lösen";	
			if (localstate.Richtungsschalter == 0)
				text = " Fahrtrichtung wählen";
			if (localstate.LM_HS)
				text = "Hauptschalter einschalten";
			if (localstate.LM_HS && localstate.HBL_Druck < 5f)
				text = "!Druckluft HS zu gering";
			if (localstate.Zugnummer == "<INIT>")
				text = "AFB: Zugdaten eingeben";
			
			return text;
		}
		public string GetFontString()
		{
			if (IsBR101())
				return "FixedSysTTF";
			else
				return "Arial";
		}
		public float ReduziereZugkraft(float zugkraft, int tz)
		{
			/*
			switch(tz)
			{
				case 2:
                    if (localstate.ET42Xtype1 == ET42XTYPE.ET426 && localstate.ET42Xtype2 != ET42XTYPE.ET426)
						return zugkraft * 2f;
					else if (localstate.ET42Xtype1 != ET42XTYPE.ET426 && localstate.ET42Xtype2 == ET42XTYPE.ET426)
						return zugkraft / 2f; 
					break;
				case 3:
					if (localstate.ET42Xtype1 == ET42XTYPE.ET426 && localstate.ET42Xtype3 != ET42XTYPE.ET426)
						return zugkraft * 2f;
					else if (localstate.ET42Xtype1 != ET42XTYPE.ET426 && localstate.ET42Xtype3 == ET42XTYPE.ET426)
						return zugkraft / 2f; 
					break;
			} 
			*/
			return zugkraft;
		}
		public string MakeZugnummer(string old)
		{
			int length = old.Length;
			int pos = old.IndexOf(" ", 0);
			if (pos < 0) return old;
			old = old.Remove(pos, length-pos);
			for (int i=0; i < length-pos; i++)
			{
				old = old.Insert(0, "0");
			}
			return old;
		}

		private void timer_eingabe_Tick(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.Zug_Tf_Nr || localstate.DISPLAY == CURRENT_DISPLAY.DSK)
			{
				if (EINGABE_BACKG == Color.Blue)
				{
					if (IsBombardier())
						EINGABE = Color.Black;
					else
						EINGABE = Color.Blue;
					EINGABE_BACKG = Color.WhiteSmoke;
				}
				else
				{
					EINGABE = Color.WhiteSmoke;
					EINGABE_BACKG = Color.Blue;
				}
				something_changed = true;
			}   			
		}

		private void SetVerbrauch(double time)
		{
			if (time <= 0d || time > 10d) return;
			
			double add = ((double)localstate.Spannung * 1000d * (double)localstate.Oberstrom / (3600d/time) * (double)localstate.traction );
			localstate.Energie += add;
		}

		private void timer_verbrauch_Tick(object sender, System.EventArgs e)
		{
			if (localstate != null && m_conf != null)
			{
				m_conf.Energie = localstate.Energie;
				m_conf.SaveFile();
			}
		}

		private string InsertPoints(string text)
		{
			string retval = ""; int counter = -1;

			for(int i = text.Length-1; i >= 0; i--)
			{   
                counter++;     
				if (counter == 3)
				{
					counter = 0;
					retval = text[i].ToString() + "." + retval;
				}
				else
					retval = text[i].ToString() + retval;				
			}

			return retval;
		}
	}
}
