using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using MMI.EBuLa.Tools;


namespace MMI.ICE3
{	   
	public class ICE3Control : System.Windows.Forms.UserControl
	{
		bool USE_DOUBLE_BUFFER = false;
		const float FramesPerSecond = 100f;
		const string fixed_font = "FixedSysTTF";
		const string other_font = "Tahoma";
		Color MMI_BLUE = Color.FromArgb(59,128,255);			
		Color MMI_ORANGE = Color.FromArgb(192,128,0);
		Color MMI_CYAN = Color.MediumAquamarine;
		Color MMI_RED = Color.Magenta;
		Color ST�RUNG_FG = Color.Black;
		Color ST�RUNG_BG = Color.Yellow;
		//Brush b_MMI = new SolidBrush(Color.FromArgb(59,125,220));
		Brush b_MMI = new SolidBrush(Color.FromArgb(0,128,255));
		//Brush b_MMI = new SolidBrush(Color.FromArgb(59,125,255));
		Pen p_dg = new Pen(Brushes.WhiteSmoke, 1.5f);
		Pen p_g = new Pen(Brushes.Gray, 1.5f);
		Pen p_b = new Pen(Brushes.Black, 1);
		Brush b_ws = Brushes.WhiteSmoke;
		Brush b_ws_alt = Brushes.Black;
		Pen p_ws_2 = new Pen(Brushes.WhiteSmoke, 2);
		public bool ShowKeys = false;
		public bool Inverse = false;
		DateTime lastTime = new DateTime(0);
		int St�rPos = 0;
		int[] schw = new int[12];
		bool[] t�r = new bool[16];
		Hashtable ht = new Hashtable(), ht2 = new Hashtable();
		ThreadStart threadDelegate_T�ren;
		Thread ThreadT�ren;

		SoundInterface sound;

		bool DO_T�REN = false;

		string DEBUG = "";


		#region Declarations

		private float[] c_druck = new float[17];

		byte[] random = new byte[1];
		System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();


		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		private Bitmap m_backBuffer;
		private Graphics g;

		public ICE3State localstate;
		private bool CONNECTED = false;
		public CURRENT_DISPLAY olddisplay;

		public bool something_changed = true;
		

		public MMI.EBuLa.Tools.XMLLoader m_conf;
		private const SmoothingMode SMOOTHING_MODE = SmoothingMode.AntiAlias;
		private const TextRenderingHint TEXT_MODE = TextRenderingHint.AntiAliasGridFit;
		float old_valu = 0f;
		public ControlContainer cc;

		DBGraphics graph_main;

		
		public DateTime vtime;
		private System.Windows.Forms.Timer timer_st;
		private System.Windows.Forms.Timer timer_schw;

		bool on = true;

		#endregion
		public ICE3Control(MMI.EBuLa.Tools.XMLLoader conf, ControlContainer cc)
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

			schw.Initialize();
			t�r.Initialize();

			m_conf = conf;
			this.cc = cc;
			localstate = new ICE3State();
			
			localstate.ICEtype1 = ICE3TYPE.ICE403;
			localstate.ICEtype2 = ICE3TYPE.NONE;
			//localstate.Type = TYPE.David2;
			if (localstate.Type == TYPE.David1)
			{
				localstate.DISPLAY = CURRENT_DISPLAY.D1_Grundb;
			}
			else
			{
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Zustand;
			}
			//olddisplay = CURRENT_DISPLAY.D1_Grundb;

			switch (m_conf.Sound)
			{
				case 1:
					sound = new APISound();
					break;
				case 2:
					sound = new DxSound();
					break;
				default:
					sound = new NullSound();
					break;
			}

			//SetButtons();
			vtime = DateTime.Now;

			InitC_druck();

			threadDelegate_T�ren = new ThreadStart(SetUpT�ren);

			int interval = Convert.ToInt32(Math.Round((1d/(double)FramesPerSecond)*1000d));
			timer1.Interval = interval;
			timer1.Enabled = true;

			Button_SW_Pressed(this, new EventArgs());
		}

		protected override void Dispose( bool disposing )
		{
			if (ThreadT�ren != null) ThreadT�ren.Abort();
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
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.timer_st = new System.Windows.Forms.Timer(this.components);
			this.timer_schw = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 30;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timer_st
			// 
			this.timer_st.Enabled = true;
			this.timer_st.Interval = 750;
			this.timer_st.Tick += new System.EventHandler(this.timer_st_Tick);
			// 
			// timer_schw
			// 
			this.timer_schw.Enabled = true;
			this.timer_schw.Interval = 1997;
			this.timer_schw.Tick += new System.EventHandler(this.timer_schw_Tick);
			// 
			// ICE3Control
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.Name = "ICE3Control";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ICE3Control_Paint);

		}
		#endregion
		public string Version()
		{
			return Application.ProductVersion;
		}


		public bool IsDavid1
		{
			get{return localstate.Type == TYPE.David1;}
		}

		#region Buttons
		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.ST)
			{
				if (St�rPos > 0)
				{
					St�rPos--;
					something_changed = true;
				}
			}
		}

		public void Button_Down_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.ST)
			{
				if (St�rPos < localstate.st�rungmgr.getSt�rungsCount)
				{
					St�rPos++;
					something_changed = true;
				}
			}

		}

		public void Button_E_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_C_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_Inverse_Pressed(object sender, System.EventArgs e)
		{
			Inverse = !Inverse;
			something_changed = true;
		}

		public void Button_Brightness_Pressed(object sender, System.EventArgs e)
		{

		}
 
		public void Button_Off_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_1_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
			{
				switch (localstate.DISPLAY)
				{
					case CURRENT_DISPLAY.D2_Zustand:
						break;
					case CURRENT_DISPLAY.D2_Schalt:
						if (D2_Schalt_B1()) 
						{
							localstate.wbBremse = WBBremse.AUS;
							something_changed = true;
							sound.PlayWBGesperrt();
						}
						break;
				}
				
			}

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();

		}

		public void Button_2_Pressed(object sender, System.EventArgs e)
		{
			switch (localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.D2_Zustand:
					break;
				case CURRENT_DISPLAY.D2_Schalt:
					if (D2_Schalt_B2()) 
					{
						localstate.wbBremse = WBBremse.SB;
						something_changed = true;
						sound.PlayWBFreigabe();
					}
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					localstate.DISPLAY = CURRENT_DISPLAY.ST;
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					localstate.DISPLAY = CURRENT_DISPLAY.ST;
					break;
			}

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_3_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
			{
				switch (localstate.DISPLAY)
				{
					case CURRENT_DISPLAY.D2_Zustand:
						break;
					case CURRENT_DISPLAY.D2_Schalt:
						if (D2_Schalt_B3()) 
						{
							localstate.wbBremse = WBBremse.BB_SB;
							something_changed = true;
							sound.PlayWBFreigabe();
						}
						break;
				}
			}
			else
			{
				switch(localstate.DISPLAY)
				{
					case CURRENT_DISPLAY.D1_Grundb:
						localstate.DISPLAY = CURRENT_DISPLAY.D1_Abfert;
						break;
				}
				
			}

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_4_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.D2_Zustand)
			{
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Schalt;
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.ST)
			{
				if (ht2.Count > 0 && ht2[St�rPos] != null)
				{
					St�rung s = (St�rung)ht[St�rPos];
					localstate.st�rungmgr.getPSt�rungen.Remove(s);
					localstate.st�rungmgr.Add(s);
					ht2.Remove(St�rPos);
				}
			}

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_5_Pressed(object sender, System.EventArgs e)
		{
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_6_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.D2_Zustand)				
					localstate.DISPLAY = CURRENT_DISPLAY.D2_FspBr;
			}

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_7_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.D1_Grundb)
			{
				localstate.DISPLAY = CURRENT_DISPLAY.D1_FB;
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D1_Abfert)
			{
			}
				
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_8_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.D2_Zustand)
                    localstate.DISPLAY = CURRENT_DISPLAY.D2_HBL;
			}
				
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_9_Pressed(object sender, System.EventArgs e)
		{
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_0_Pressed(object sender, System.EventArgs e)
		{	
			if (localstate.Type == TYPE.David2)
			{
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Zustand;
		
			}
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_Grundb;
				
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}
		
		public void Button_UD_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David1)
			{
				localstate.Type = TYPE.David2;
			}
			else
			{
				localstate.Type = TYPE.David1;
			}
			CURRENT_DISPLAY oldwas = localstate.DISPLAY;
			localstate.DISPLAY = olddisplay;
			olddisplay = oldwas;
			ShowKeys = !ShowKeys;
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_SW_Pressed(object sender, System.EventArgs e)
		{
			Switcher s = new Switcher(ref localstate, m_conf.TopMost);
			s.ShowDialog();
			InitC_druck();
			if (!D2_Schalt_B3()) localstate.wbBremse = WBBremse.AUS;
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_V_GR_0_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.st�rungmgr.St�rStack != null && localstate.st�rungmgr.Current.Type != ENUMSt�rung.NONE)
			{
				localstate.st�rungmgr.PopCurrent();
				timer_st_Tick(sender, e);
				localstate.DISPLAY = CURRENT_DISPLAY.V_GREATER_0;
				ht.Clear(); ht2.Clear();
				something_changed = true;
			}


			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}
		public void Button_V_EQ_0_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.st�rungmgr.St�rStack != null && localstate.st�rungmgr.Current.Type != ENUMSt�rung.NONE)
			{
				localstate.st�rungmgr.PopCurrent();
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
					localstate.st�rungmgr.DeleteSt�rung(ENUMSt�rung.S01_ZUSIKomm);
					if (localstate.DISPLAY == CURRENT_DISPLAY.V_EQUAL_0 || localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0)
					{
						if (localstate.Type == TYPE.David1)
						{
							localstate.DISPLAY = CURRENT_DISPLAY.D1_Grundb;
						}
						else if (localstate.Type == TYPE.David2)
						{
							localstate.DISPLAY = CURRENT_DISPLAY.D2_Zustand;
						}
					}
				}
				else
				{
					if (CONNECTED != value)
						localstate.st�rungmgr.Add(new St�rung(ENUMSt�rung.S01_ZUSIKomm));
				}
				CONNECTED = value;
				something_changed = true;
			}
		}
		public void DrawLines(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Trennlinien unten
			pg.DrawLine(p_ws_2, 20, this.Height - 35, this.Width-20, this.Height - 35);

			pg.DrawLine(p_ws_2, 20, this.Height - 65, this.Width-20, this.Height - 65);

			// Trennlinie oben
			pg.DrawLine(p_ws_2, 20, 20, this.Width-20, 20);


			//Tastentrenner
			for (int i = 0; i < 11; i++)
			{
				pg.DrawLine(p_ws_2, i*59+20, this.Height - 35, i*59+20, this.Height);

				if ((i < 8 || i > 9) && !(localstate.LM_AFB && i==1)) 
					pg.DrawLine(p_ws_2, i*59+20, this.Height - 65, i*59+20, this.Height - 45);

				//oben
				if (i == 2 && !V_HELP)
					pg.DrawLine(p_ws_2, i*59+20, 0, i*59+20, 20);

				if (i >7)
					pg.DrawLine(p_ws_2, i*59+20, 0, i*59+20, 20);
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
        
		public void FillFields(ref Graphics pg)
		{
		}

		public void SetText(ref DBGraphics pg)
		{			
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if (something_changed)
			{
				Monitor.Enter(something_changed);
				something_changed = false;
				UpdateScreen();
				Monitor.Exit(something_changed);
			}
		}

		private void InitC_druck()
		{
			for (int i = 0; i < c_druck.Length; i++)
			{
				prov.GetBytes(random);
				if (random[0] > 128)
				{
					prov.GetBytes(random);
					c_druck[i] = 0f - ((float)random[0] / 12800f);
				}
				else
				{
					prov.GetBytes(random);
					c_druck[i] = ((float)random[0] / 12800f);
				}
			}

			for(int i = 0; i <12; i++)
			{
				schw[i] = 0;
			}
		}

		public bool addtionalhours
		{
			get{return localstate.addtionalhours;}
			set{localstate.addtionalhours = value;}
		}

		#region SET

    	public void SetLMZugart(bool state, string text)
		{
		}

		public void SetLM1000Hz(bool state)
		{
		}

		public void SetLM500Hz(bool state)
		{
		}

		public void SetLMBefehl(bool state)
		{
		}

		public void SetLMSifa(bool state)
		{
		}
	
		public void SetLMHauptschalter(bool state)
		{
			localstate.LM_HS = state;
			something_changed = true;
		}

		public void SetLM_LZB_�(bool state)
		{
		}

		public void SetLM_LZB_S(bool state)
		{
		}

		public void SetLM_LZB_B(bool state)
		{
		}

		public void SetLM_LZB_G(bool state)
		{
		}

		public void SetLM_LZB_ENDE(bool state)
		{
		}
		
		public void SetLM_LZB_H(bool state)
		{
		}
		public void SetLM_T�r(bool state)
		{
		}

		public void SetLM_INTEGRA_GELB(bool state)
		{
		}
		public void SetLM_INTEGRA_ROT(bool state)
		{
		}
		public void SetReisezug(bool state)
		{
		}
		public void SetPZBSystem(float valu)
		{
		}
		public void SetBrh(float valu)
		{
		}
		public void SetDrehzahl(float valu)
		{
			localstate.Drehzahl = valu;
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
			else localstate.Bremsstellung = BREMSSTELLUNG.G;
		}
		public void SetAFB_LZB_SollGeschwindigkeit(float valu)
		{
		}
		public void SetAFB_Sollgeschwindigkeit(float valu)
		{
			localstate.AFB_SollGeschwindigkeit = valu;
		}
		public void SetLZB_Sollgeschwindigkeit(float valu)
		{
		}
		public void SetLZB_ZielGeschwindigkeit(float valu)
		{
		}
		public void SetLZB_ZielWeg(int valu)
		{
		}
		public void SetFahrstufen(float valu)
		{
			localstate.Fahrstufe = valu;
			something_changed = true;
		}
		public void SetE_Bremse(float valu)
		{
			localstate.E_Bremse = valu;
			something_changed = true;
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
				if (valu >= -300f && valu <= 180f)
				{
					localstate.Zugkraft = valu;
					something_changed = true;
				}
			}
			else
			{
				localstate.ZugkraftGesammt = valu;
				something_changed = true;
			}
		}

		public void SetOberstrom(float valu)
		{
			localstate.Oberstrom = valu;
		}
		
		public void SetSpannung(float valu)
		{
			localstate.Spannung = valu;
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
		public void SetC_Druck(float valu)
		{
			localstate.C_Druck = valu;
			something_changed = true;
		}
		public void SetLM_Mg(bool valu)
		{
			localstate.LM_MG = valu;
			something_changed = true;
		}

		public void SetLM_AFB(bool valu)
		{
			localstate.LM_AFB = valu;
			something_changed = true;
		}
		public void SetUhrDatum(DateTime valu)
		{
			TimeSpan span = new TimeSpan(DateTime.Now.Ticks - lastTime.Ticks);

			if (span.TotalMilliseconds < 100)
			{
				lastTime = DateTime.Now;
				return;
			}

			vtime = valu;
			lastTime = DateTime.Now;
			something_changed = true;
		}
		public void SetZusatzbremse(float valu)
		{
			localstate.Zusatzbremse = valu;
			something_changed = true;
		}
		public void SetT�ren(float valu)
		{
			if ((valu * Math.Pow(10, 45) > 7.3) && (valu * Math.Pow(10, 45) < 8.7))
			{
				localstate.T�ren = T�REN.ZU_ABFAHRT;
			}
			else if ((valu * Math.Pow(10, 45) > 6.3) && (valu * Math.Pow(10, 45) < 7.7))
			{
				localstate.T�ren = T�REN.ZU;
			}
			else if ((valu * Math.Pow(10, 45) > 4.3) && (valu * Math.Pow(10, 45) < 5.7))
			{
				localstate.T�ren = T�REN.SCHLIESSEN;
			}
			else if ((valu * Math.Pow(10, 45) > 1.5) && (valu * Math.Pow(10, 45) < 2.9))
			{
				localstate.T�ren = T�REN.FAHRG�STE_I_O;
			}
			else if ((valu * Math.Pow(10, 45) < 1.5) && (valu * Math.Pow(10, 45) > 1.3))
			{
					localstate.T�ren = T�REN.AUF;
			}
			else if (valu * Math.Pow(10, 45) < 1.0)
			{
				localstate.T�ren = T�REN.FREIGEGEBEN;
			}

			if (localstate.T�ren == T�REN.AUF)
			{
				DO_T�REN = false;

				for(int i = 0; i < t�r.Length; i++)
				{
					t�r[i] = true;
				}
			}
			else if (localstate.T�ren == T�REN.SCHLIESSEN)
			{
				DO_T�REN = true;

				if (ThreadT�ren != null)
				{
					if (ThreadT�ren.ThreadState == ThreadState.Stopped) 
					{
						try
						{
							// Thread muss neu anglegt werden
							ThreadT�ren = new Thread(new ThreadStart(threadDelegate_T�ren));
							ThreadT�ren.Priority = ThreadPriority.BelowNormal;
							ThreadT�ren.IsBackground = true;
							ThreadT�ren.Start();
							//DEBUG = "THREAD: RUNNING";
						}
						catch(Exception e)
						{
							//MessageBox.Show("UNERWARTET EXCEPTION BEI THREAD-START: "+e.Message);
						}
					}
					else if (ThreadT�ren.ThreadState == ThreadState.Suspended)
					{
						ThreadT�ren.Suspend();
						//DEBUG = "THREAD: SUSPEND";
					}
					//else MessageBox.Show("Thread T�ren hat unerwarteten Zustand: "+ThreadT�ren.ThreadState.ToString());
				}
				else
				{
					ThreadT�ren = new Thread(new ThreadStart(threadDelegate_T�ren));
					ThreadT�ren.Priority = ThreadPriority.BelowNormal;
					ThreadT�ren.IsBackground = true;
					ThreadT�ren.Start();
					//DEBUG = "THREAD: RUNNING";
				}
			}
			else if (localstate.T�ren == T�REN.ZU || localstate.T�ren == T�REN.ZU_ABFAHRT)
			{
				DO_T�REN = false;

				for(int i = 0; i < t�r.Length; i++)
				{
					t�r[i] = false;
				}
			}
		}
		#endregion

		public void UpdateScreen()
		{
			//if (!something_changed) return;

			//something_changed = false;
			
			if (USE_DOUBLE_BUFFER)
				ICE3Control_Paint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0,0,this.Width, this.Height)));
			else
				this.Refresh();

			
		}


		private void DrawD1_Grundbild(ref Graphics pg)
		{
			DrawD1_I(ref pg);
			DrawD1_U(ref pg);
		}
		private void DrawD1_U(ref Graphics pg)
		{
			DrawAFB(ref pg);
			if (IsICETDLeading)
			{
				DrawDrehzahl(ref pg);
			}
			else
			{
				DrawSpannung(ref pg, false);
			}
		}
		private void DrawD1_I(ref Graphics pg)
		{
			DrawAFB(ref pg);
			if (!IsICETDLeading)
			{
				DrawOberstrom(ref pg);
			}
		}
		private void DrawD1_FB(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawSt�rung(ref pg);
			if (!IsICETDLeading) DrawSpannung(ref pg, true);

			int count = CountICET + CountICE3 + CountICETD;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawString("%", f, b_ws, 150f+count*120, 347f);
			pg.DrawString("0", f, b_ws, 151f+count*120, 347f-19f);
			pg.DrawString("50", f, b_ws, 143f+count*120, 328f-5f*19f);
			pg.DrawString("100", f, b_ws, 135f+count*120, 328f-10f*19f);
			if (localstate.Zugkraft >= 0)
			{
				pg.FillRectangle(b_MMI, 20+120,318f-15f*19f, 105, 18); 
				pg.DrawString(" Zugkr�fte", f, b_ws, 20f+120, 318f-15f*19f);
			}
			else
			{
				pg.FillRectangle(new SolidBrush(Color.Orange), 20+120,318f-15f*19f, 105, 18); 
				pg.DrawString(" E-Bremskr�fte", f, b_ws, 20f+120, 318f-15f*19f);
			}

			DrawF(ref pg, 1, true);

			if (count > 1)
				DrawF(ref pg, 2, true);
			if (count > 2)
				DrawF(ref pg, 3, true);
		}
		private void DrawD1_Abfertigen(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawSt�rung(ref pg);

			DrawT�rErkl�rung(ref pg);

			int count = CountWaggons;
			int trains = CountTrains();
			int move = 0;

			switch(trains)
			{
				case 1:
					DrawT�rfelder(ref pg, 1, 0);
					break;
				case 2:
					move += DrawT�rfelder(ref pg, 1, 0);
					DrawT�rfelder(ref pg, 2, move);
					break;
				case 3:
					move += DrawT�rfelder(ref pg, 1, 0);
					move += DrawT�rfelder(ref pg, 2, move);
					DrawT�rfelder(ref pg, 3, move);
					break;
			}

			DrawT�rSammelLM(ref pg);
		}
		private void DrawD2_Zustand(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawHL_Druck(ref pg);
			DrawHBL_Druck(ref pg);
			DrawC_Druck(ref pg);
		}

		private void DrawD2_Fdyn(ref Graphics pg)
		{
			DrawHL_Druck(ref pg);
			DrawFdynTK(ref pg, 1);
			//if (localstate.ICEtype == ICETYPE.ICE1 || localstate.ICEtype == ICETYPE.ICE2_DT ) DrawFdynTK(ref pg, 2);
		}
		private void DrawD2_Schaltzustand(ref Graphics pg)
		{
			DrawSchaltzustand(ref pg);
		}
		private void DrawD2_HBL(ref Graphics pg)
		{
			DrawHL_Druck(ref pg);
			DrawHBL_vollstd_Druck(ref pg);
		}

		private void DrawD2_FspBr(ref Graphics pg)
		{
			DrawHL_Druck(ref pg);
			DrawHBL_Druck(ref pg);
			DrawFspBr(ref pg);
		}

		private void DrawV_EQ_0(ref Graphics pg)
		{
			if (localstate.st�rungmgr.St�rStack != null)
			{
				switch(localstate.st�rungmgr.St�rStack.Type)
				{
					case ENUMSt�rung.S01_ZUSIKomm:
						DrawS01(ref pg, false);
						break;
					case ENUMSt�rung.S11_ZUSIKomm:
						DrawS11(ref pg, false);
						break;
					case ENUMSt�rung.S02_Trennsch�tz:
						DrawS02(ref pg, false);
						break;
				}
			}
		}
		private void DrawV_GR_0(ref Graphics pg)
		{
			if (localstate.st�rungmgr.St�rStack != null)
			{
				switch(localstate.st�rungmgr.St�rStack.Type)
				{
					case ENUMSt�rung.S01_ZUSIKomm:
						DrawS01(ref pg, true);
						break;
					case ENUMSt�rung.S11_ZUSIKomm:
						DrawS11(ref pg, false);
						break;
					case ENUMSt�rung.S02_Trennsch�tz:
						DrawS02(ref pg, true);
						break;
				}
			}
		}
		private void DrawST(ref Graphics pg)
		{
			bool add = true;
			if (ht.Count > 0) add = false;
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			int counter = -1; int pos = 0;
			if (localstate.st�rungmgr.getSt�rungsCount < 1)
			{
				pg.DrawString("Keine St�rung gemeldet!", f, b_ws, 50,60);
			}
			else
			{
				foreach(St�rung st in localstate.st�rungmgr.getSt�rungen)
				{
					if (st.Priority == int.MaxValue) continue;
					counter++;

					if (add)
					{
						ht.Add(pos, st);
					}

					if (pos == St�rPos)
					{
						pg.FillRectangle(b_ws, 20, 40+20*counter, 500, 18);
						pg.DrawString(st.Priority.ToString(), f, b_ws_alt, 20, 40+20*counter);
						pg.DrawString(st.Name, f, b_ws_alt, 50, 40+20*counter);
						pg.DrawString(st.Description, f, b_ws_alt, 140, 40+20*counter);
					}
					else
					{
						pg.DrawString(st.Priority.ToString(), f, b_ws, 20, 40+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, 40+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, 40+20*counter);
					}
					pos++;
				}
				foreach(St�rung st in localstate.st�rungmgr.getPSt�rungen)
				{
					if (st.Priority == int.MaxValue) continue;
					counter++;

					if (add)
					{
						ht.Add(pos, st);
						ht2.Add(pos, st);
					}

					if (pos == St�rPos)
					{
						pg.FillRectangle(b_ws, 20, 40+20*counter, 500, 18);
						pg.DrawString("*"+st.Priority.ToString(), f, b_ws_alt, 20, 40+20*counter);
						pg.DrawString(st.Name, f, b_ws_alt, 50, 40+20*counter);
						pg.DrawString(st.Description, f, b_ws_alt, 140, 40+20*counter);
					}
					else
					{
						pg.DrawString("*"+st.Priority.ToString(), f, b_ws, 20, 40+20*counter);
						pg.DrawString(st.Name, f, b_ws, 50, 40+20*counter);
						pg.DrawString(st.Description, f, b_ws, 140, 40+20*counter);
					}
					pos++;
				}

			}
		}
		private void DrawKasten(ref Graphics pg, int x, int y, bool hasIndicator, float height)
		{
			Pen p1 = new Pen(b_ws, 1);
			int Iheight = Convert.ToInt32(Math.Round(height));

			if (hasIndicator)
			{
				if (y == 20)
				{
					// F�llung Kasten links 
					if (height > 1f)
					{
						pg.FillRectangle(b_MMI, x, 345-7, 50, 7);
						pg.DrawRectangle(p1, x, 345-7, 50, 7);
					}
				
					pg.FillRectangle(b_MMI, x, 334-Iheight, 50, Iheight);
					pg.DrawRectangle(p1, x, 334-Iheight, 50, Iheight);
				}
				else
				{
					// F�llung Kasten links 
					if (height > 0.1f)
					{
						pg.FillRectangle(b_MMI, x, 365-7, 23, 7);
						pg.DrawRectangle(p1, x, 365-7, 23, 7);
					}
				
					pg.FillRectangle(b_MMI, x, 354-Iheight, 23, Iheight);
					pg.DrawRectangle(p1, x, 354-Iheight, 23, Iheight);
				}

			}
			else
			{
				if (y == 20)
				{
					pg.FillRectangle(b_MMI, x, 345-Iheight, 50, Iheight);
					pg.DrawRectangle(p1, x, 345-Iheight, 50, Iheight);
				}
				else if (y == 40)
				{
					if (Iheight >= 0)
					{
						pg.FillRectangle(b_MMI, x, 345-Iheight, 23, Iheight);
					}
					else
					{
						Iheight *= -1;
						pg.FillRectangle(new SolidBrush(Color.Orange), x, 345-Iheight, 23, Iheight);
					}
					pg.DrawRectangle(p1, x, 345-Iheight, 23, Iheight);
				}
				else
				{
					pg.FillRectangle(b_MMI, x, 365-Iheight, 23, Iheight);
					pg.DrawRectangle(p1, x, 365-Iheight, 23, Iheight);
				}
			}
		}
		private void DrawKastenSmall(ref Graphics pg, int x, int y, bool hasIndicator, float height, bool small, Color color)
		{
			Pen p1 = new Pen(b_ws, 1);
			Brush b = new SolidBrush(color);

			if (hasIndicator)
			{
				if (height >= 0f)
				{
					// F�llung Kasten 
					pg.FillRectangle(b, x, 48+y-55, 20, 6);
				}
				pg.DrawRectangle(p1, x, 48+y-55, 20, 6);

				int Iheight = Convert.ToInt32(Math.Round(height));
				
				if (height >= 0f)
				{
					pg.FillRectangle(b, x, y-4-54, 20, Iheight-2);
				}
				pg.DrawRectangle(p1, x, y-4-54, 20, 50-2);
			}
			else
			{				  //52x12
				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b, x, 2+y, 20, Iheight);
				pg.DrawRectangle(p1, x, 2+y, 20, 50);
			}
		}
		private void DrawKastenSmallSpacer(ref Graphics pg, int x, int y, bool hasIndicator, float height, bool small, bool fillRight, Color color)
		{
			Pen p1 = new Pen(b_ws, 1);
			Brush b = new SolidBrush(color);

			if (hasIndicator)
			{
				
				// F�llung Kasten 
				pg.FillRectangle(b, x, 48+y-55, 20, 6);
				pg.DrawRectangle(p1, x, 48+y-55, 20, 6);

				int Iheight = Convert.ToInt32(Math.Round(height));
				
				if (GetC_Brake_Standby()) // E-Bremse bremst allein
				{
					if (fillRight)
						pg.FillRectangle(b, x+10, y-4-54, 10, Iheight-2);
					else
						pg.FillRectangle(b, x, y-4-54, 10, Iheight-2);
				}
				else // (D)-Bremse aktiv
					pg.FillRectangle(b, x, y-4-54, 20, Iheight-2);

				pg.DrawRectangle(p1, x, y-4-54, 20, 50-2);
				pg.DrawLine(p1, x+10, y-4-54, x+10, y-4-3-4);
			}
			else
			{				  //52x12
				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b, x, 2+y, 20, Iheight);
				pg.DrawRectangle(p1, x, 2+y, 20, 50);
			}
		}
		private void DrawKastenSmallSmall(ref Graphics pg, int x, int y, bool hasIndicator, float height, bool small, Color color)
		{
			Pen p1 = new Pen(b_ws, 1);
			Brush b = new SolidBrush(color);

			if (hasIndicator)
			{
				
				if (height >= 0)
				{
					// F�llung Kasten 
					pg.FillRectangle(b, x, 48+y-55, 10, 6);
				}
				pg.DrawRectangle(p1, x, 48+y-55, 10, 6);

				int Iheight = Convert.ToInt32(Math.Round(height));
				
				pg.FillRectangle(b, x, y-4-54, 10, Iheight-2);
				pg.DrawRectangle(p1, x, y-4-54, 10, 50-2);
			}
			else
			{				  //52x12
				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b, x, 2+y, 10, Iheight);
				pg.DrawRectangle(p1, x, 2+y, 10, 50);
			}
		}

		private void DrawI(ref Graphics pg)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawLine(p_b, 220, 344, 210, 344); // 0
			pg.DrawLine(p_b, 220, 299, 210, 299); // 200
			pg.DrawLine(p_b, 220, 257, 210, 257); // 400
			pg.DrawLine(p_b, 220, 212, 210, 212); // 600
			pg.DrawLine(p_b, 220, 168, 210, 168); // 800
			pg.DrawLine(p_b, 220, 124, 210, 124); // 1000
			
			pg.DrawString("0",    f, Brushes.Black, 196, 336f);
			pg.DrawString("200",  f, Brushes.Black, 176, 291f);
			pg.DrawString("400",  f, Brushes.Black, 176, 248f);
			pg.DrawString("600",  f, Brushes.Black, 176, 203f);			
			pg.DrawString("800",  f, Brushes.Black, 176, 159f);			
			pg.DrawString("1000", f, Brushes.Black, 166, 115f);

			float height = 0f;

			double twice = 2d;

			//if (localstate.ICEtype == ICETYPE.ICE2_ET) twice = 1d;

			if (localstate.Oberstrom > 0f)
			{
				if (localstate.Oberstrom > 1000f) localstate.Oberstrom = 1000f;
				height = Convert.ToSingle((double)localstate.Oberstrom * twice * 0.221d);
			}

			DrawKasten(ref pg, 220, 100, false, height);

			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("A", f, Brushes.Black, 190f, 70f);
			pg.DrawString("I", f, Brushes.Black, 238f, 354f);
			pg.DrawString("F", f, Brushes.Black, 246f, 358f);

			if (!ShowKeys)
			{
				// Oberstromwahlkasten
				Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
				Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
				Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
				Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

				pg.FillRectangle(Brushes.LightBlue, 59, 397, 62, 49);

				pg.DrawLine(p_ws_3, 55-1, 393, 55+70-2, 393);
				pg.DrawLine(p_ws_3, 55,   393, 55,      393+57);

				pg.DrawLine(p_dg_3, 55-1,    393+57,   55+70-2, 393+57);
				pg.DrawLine(p_dg_3, 55+70-2, 393+57+1, 55+70-2, 393-1);
			
				pg.DrawRectangle(p_ws_1, 55+3, 393+3, 70-2-7, 57-7);
			
				pg.DrawLine(p_dg_1, 55+3,      393+57-4, 55+70-2-4, 393+57-4);
				pg.DrawLine(p_dg_1, 55+70-2-4, 393+3, 55+70-2-4, 393+57-4);

			
				//pg.DrawRectangle(p_dg, 55, 393, 70, 57);
				//pg.DrawRectangle(p_dg, 59, 397, 62, 49);

				f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
				pg.DrawString(localstate.maxOberstrom.ToString()+" A", f, Brushes.Black, 64, 413);
			}

			pg.SmoothingMode = SMOOTHING_MODE;

		}

		private void DrawF(ref Graphics pg, byte tk, bool isICE3)
		{
			pg.SmoothingMode = SmoothingMode.None;

			int count = CountICET + CountICE3 + CountICETD;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			// Skala links
			if (tk > 1)
			{
				pg.DrawLine(p_ws_2, 20+120*(tk), 52, 20+120*(tk), 345);
			}
			else
				pg.DrawLine(p_ws_2, 20+120*(tk), 34, 20+120*(tk), 345);

			for (int i = 0; i < 11; i++)
			{
				if ((i == 5 || i == 10 || i == 0) || count > tk)
				{
					if (count == tk)
                        pg.DrawLine(p_ws_2, 20+120*(tk), 345-19*i, 160+120*(tk), 345-19*i); // 0
					else
						pg.DrawLine(p_ws_2, 20+120*(tk), 345-19*i, 140+120*(tk), 345-19*i); // 0
						
				}
				else
				{
					if (count == tk)
						pg.DrawLine(p_ws_2, 20+120*(tk), 345-19*i, 140+120*(tk), 345-19*i); // 0
					else
						pg.DrawLine(p_ws_2, 20+120*(tk), 345-19*i, 120+120*(tk), 345-19*i); // 0
				}
			}

			pg.DrawString("Tz "+tk.ToString()+" ("+GetTrainType(tk)+")", f, b_ws, 21f+120*(tk), 318f-14f*19f);
	
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f, soll_height = 0f;
			double factor = 1d;

			string type = GetTrainType(1);

			// F�llung Kasten links 
			if (localstate.Zugkraft > 0f)
			{
				if ((type == "BR 403" || type == "BR 406"))
				{
					factor = (150d/75d);
				}
				else if (type == "BR 605")
				{
					factor = (150d/40d);
				}
				else if (type == "BR 415")
				{
					factor = (150d/50d);
				}
				else if (type == "BR 411")
				{
					factor = (150d/50d);
				}

				float hl = localstate.ZugkraftGesammt;
				height = Convert.ToSingle((double)hl * 2.5333d / 2d * factor);

				float sh = localstate.Fahrstufe;
				soll_height = Convert.ToSingle((double)sh * 4.75d);

			}
			else if (localstate.Zugkraft < 0f)
			{
				if ((type == "BR 403" || type == "BR 406"))
				{
					factor = (150d/60d);
				}
				else if (type == "BR 605")
				{
					factor = (150d/45d);
				}
				else if (type == "BR 415")
				{
					factor = (150d/63d);
				}
				else if (type == "BR 411")
				{
					factor = (150d/63d);
				}

				if (localstate.Zugkraft < -150f) localstate.Zugkraft = -150f;
				float hl = localstate.Zugkraft;
				height = Convert.ToSingle((double)hl * 2.5333d / 2d * factor);

				float sh = localstate.E_Bremse;
				soll_height = Convert.ToSingle((double)sh * 31.66666667d);
			}


			type = GetTrainType(tk);

			float norm_height = Math.Abs(soll_height);

			DrawKasten(ref pg, 20+120*(tk), 40, false, height);
			DrawFPfeil(ref pg, 20+120*(tk), 345-norm_height, true);
			if (type == "BR 403" || type == "BR 406")
			{
				DrawKasten(ref pg, 20+120*(tk)+27, 40, false, height);
				DrawFPfeil(ref pg, 20+120*(tk)+27+23, 345-norm_height, false);

				DrawKasten(ref pg, 20+120*(tk)+27*2+3, 40, false, height);
				DrawFPfeil(ref pg, 20+120*(tk)+27*2+3, 345-norm_height, true);
			}
			DrawKasten(ref pg, 20+120*(tk)+27*3+3, 40, false, height);
			DrawFPfeil(ref pg, 20+120*(tk)+27*3+3+23, 345-norm_height, false);

			pg.SmoothingMode = SMOOTHING_MODE;

		}

		private void DrawFPfeil(ref Graphics pg, int x, float F_y, bool toRight)
		{ 
			int y = Convert.ToInt32(F_y);
			Point[] pp = new Point[3];
			Point p1, p2, p3;

			if (toRight)
			{
				p1 = new Point(x-2,  y-10);
				p2 = new Point(x+18, y   );
				p3 = new Point(x-2,  y+10);
			}
			else
			{
				p1 = new Point(x+2,  y-10);
				p2 = new Point(x-18, y   );
				p3 = new Point(x+2,  y+10);
			}

			pp[0] = p1; pp[1] = p2; pp[2] = p3;

			pg.FillPolygon(Brushes.Magenta, pp);
		}
		private void DrawOberstrom(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			pg.DrawLine(p_ws_2, 500, 36, 500, 345);

			for (int i = 0; i < 16; i++)
			{
				if (Math.IEEERemainder((double)i, 2d) == 0d || i == 0)
				{
					pg.DrawLine(p_ws_2, 500, 345-19*i, 600, 345-19*i); // 0
				}
				else
				{
					pg.DrawLine(p_ws_2, 500, 345-19*i, 560, 345-19*i); // 0
				}
			}

			RecreateOberstrom();

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("A", f, b_ws, 590f, 347f);
			pg.DrawString("0", f, b_ws, 590f, 347f-19f);
			pg.DrawString("200", f, b_ws, 575f, 328f-2f*19f);
			pg.DrawString("400", f, b_ws, 575f, 328f-4f*19f);
			pg.DrawString("600", f, b_ws, 575f, 328f-6f*19f);
			pg.DrawString("800", f, b_ws, 575f, 328f-8f*19f);
			pg.DrawString("1000", f, b_ws, 568f, 328f-10f*19f);
			pg.DrawString("1200", f, b_ws, 568f, 328f-12f*19f);
			pg.DrawString("1400", f, b_ws, 568f, 328f-14f*19f);
			pg.DrawString("Oberstrom", f, b_ws, 500f, 318f-15f*19f);
			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			// F�llung Kasten links 
			if (localstate.OberstromToRender > 0f)
			{
				if (localstate.OberstromToRender > 1500f) localstate.OberstromToRender = 1500f;
				float hl = localstate.OberstromToRender;
				height = Convert.ToSingle((double)hl * 0.19d);
			}

			DrawKasten(ref pg, 500, 20, false, height);

			DrawOberstromPfeil(ref pg);

			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawOberstromPfeil(ref Graphics pg)
		{ 
			int i = Convert.ToInt32((float)localstate.OberstromGrenze/100f);
			int y = 345-19*i;

			Point[] pp = new Point[3];

			Point p1 = new Point(500-2, y-10);
			Point p2 = new Point(500+18, y);
			Point p3 = new Point(500-2, y+10);

			pp[0] = p1; pp[1] = p2; pp[2] = p3;

			pg.FillPolygon(Brushes.Magenta, pp);
		}

		private void DrawSpannung(ref Graphics pg, bool onlyTz1)
		{
			pg.SmoothingMode = SmoothingMode.None;

			int count = CountICET + CountICE3;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			// Skala links
			pg.DrawLine(p_ws_2, 20, 36, 20, 345);

			int large = 0;

			if (count > 1) large = 10;
			if (onlyTz1) large = 0;

			for (int i = 0; i < 14; i++)
			{
				if ((i == 2 || i == 7 || i == 12 || i == 0) || (count > 1 && !onlyTz1))
				{
					pg.DrawLine(p_ws_2, 20, 345-19*i, 130+large, 345-19*i); // 0
				}
				else
				{
					pg.DrawLine(p_ws_2, 20, 345-19*i, 100+large, 345-19*i); // 0
				}
			}

			
			if (!onlyTz1)
			{
				pg.DrawString("Tz 1 ("+GetTrainType(1)+")", f, b_ws, 21f, 318f-13f*19f);
				DrawHS(ref pg, 1);
			}
			else
			{
				pg.DrawString("Tz 1 ("+GetTrainType(1)+")", f, b_ws, 21f, 318f-14f*19f);
			}

			if (!onlyTz1)
			{
				if (count == 2)
				{
					// Skala links
					pg.DrawLine(p_ws_2, 20+120, 72, 20+120, 345);

					pg.DrawString("Tz 2 ("+GetTrainType(2)+")", f, b_ws, 21f+120, 318f-13f*19f);

					for (int i = 0; i < 14; i++)
					{
						if (i == 2 || i == 7 || i == 12 || i == 0)
						{
							pg.DrawLine(p_ws_2, 20+120, 345-19*i, 140+120, 345-19*i); // 0
						}
						else
						{
							pg.DrawLine(p_ws_2, 20+120, 345-19*i, 110+120, 345-19*i); // 0
						}
					}
					DrawHS(ref pg, 2);
				}
				else if (count == 3)
				{
					// Skala links
					pg.DrawLine(p_ws_2, 20+120, 72, 20+120, 345);
					pg.DrawLine(p_ws_2, 20+239, 72, 20+239, 345);

					pg.DrawString("Tz 2 ("+GetTrainType(2)+")", f, b_ws, 21f+120, 318f-13f*19f);
					pg.DrawString("Tz 3 ("+GetTrainType(3)+")", f, b_ws, 21f+239, 318f-13f*19f);

					for (int i = 0; i < 14; i++)
					{
						if ((i == 2 || i == 7 || i == 12 || i == 0) || count > 2)
						{
							pg.DrawLine(p_ws_2, 20+120, 345-19*i, 130+120+large, 345-19*i); // 0
						}
						else
						{
							pg.DrawLine(p_ws_2, 20+120, 345-19*i, 100+120+large, 345-19*i); // 0
						}
					}

					for (int i = 0; i < 14; i++)
					{
						if (i == 2 || i == 7 || i == 12 || i == 0)
						{
							pg.DrawLine(p_ws_2, 20+240, 345-19*i, 140+240, 345-19*i); // 0
						}
						else
						{
							pg.DrawLine(p_ws_2, 20+240, 345-19*i, 110+240, 345-19*i); // 0
						}
					}
					DrawHS(ref pg, 2);
					DrawHS(ref pg, 3);
				}
			}

			if (onlyTz1) count = 1;

			pg.DrawString("kV", f, b_ws, 113f+(count-1)*120+large, 347f);
			pg.DrawString("0", f, b_ws, 121f+(count-1)*120+large, 347f-19f);
			pg.DrawString("10", f, b_ws, 113f+(count-1)*120+large, 328f-2f*19f);
			pg.DrawString("15", f, b_ws, 113f+(count-1)*120+large, 328f-7f*19f);
			pg.DrawString("20", f, b_ws, 113f+(count-1)*120+large, 328f-12f*19f);
			pg.DrawString("Oberspannung", f, b_ws, 20f, 318f-15f*19f);
			if (!onlyTz1)
				pg.DrawString("(DB 15 kV)", f, b_ws, 20f, 318f-14f*19f);
			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			// F�llung Kasten links 
			if (localstate.Spannung > 8f)
			{
				if (localstate.Spannung > 21f) localstate.Spannung = 21f;
				float hl = localstate.Spannung - 8f;
				height = Convert.ToSingle((double)hl * 19d) - 11f;
			}
			else
			{
				if (localstate.Spannung > 2f)
					height = 2;

			}

			int sch = schw[0];

			if (localstate.Spannung == 0f) sch = 0;

			if (count == 1)
			{
				DrawKasten(ref pg, 20, 20, true, height + sch);
			}
			else if (count == 2 && !onlyTz1)
			{
				DrawKasten(ref pg, 20, 20, true, height + sch);
				DrawKasten(ref pg, 20+120, 20, true, height + sch);
			}
			else if (count == 3 && !onlyTz1)
			{
				DrawKasten(ref pg, 20, 20, true, height + sch);
				DrawKasten(ref pg, 20+120, 20, true, height + sch);
				DrawKasten(ref pg, 20+239, 20, true, height + sch);
			}


			pg.SmoothingMode = SMOOTHING_MODE;
		}
	
		private void DrawHS(ref Graphics pg, int tz)
		{
			pg.SmoothingMode = SmoothingMode.None;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			if (!localstate.LM_HS) // ein
			{
				pg.FillRectangle(b_MMI, 20+(tz-1)*120, 350, 50, 40);
				pg.DrawString("HS ein", f, Brushes.WhiteSmoke, 22+(tz-1)*120, 352);
				pg.DrawString("HS ein", f, Brushes.WhiteSmoke, 22+(tz-1)*120, 372);
			}
			else
			{
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 20+(tz-1)*120, 350, 50, 40);
				pg.DrawString("HS aus", f, b_ws, 20+(tz-1)*120, 352);
				pg.DrawString("HS aus", f, b_ws, 20+(tz-1)*120, 372);
			}
			pg.DrawRectangle(p_ws_2, 20+(tz-1)*120, 350, 50, 40);
			pg.DrawLine(p_ws_2, 20+(tz-1)*120, 370, 70+(tz-1)*120, 370);

			pg.SmoothingMode = SMOOTHING_MODE;

		}
		private void DrawDrehzahl(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			pg.DrawLine(p_ws_2, 20, 36, 20, 345);

			int count = CountICETD;

			for (int i = 0; i < 5; i++)
			{
				pg.DrawLine(p_ws_2, 20, 345-55*i, 118*(count) + 32, 345-55*i); // 0
			}


			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("U/min", f, b_ws, 113f+(count-1)*118, 347f);
			pg.DrawString("0", f, b_ws, 141f+(count-1)*118, 347f-19f);
			pg.DrawString("1000", f, b_ws, 117f+(count-1)*118, 347f-19f-2*55f);
			pg.DrawString("2000", f, b_ws, 117f+(count-1)*118, 347f-19f-4*55f);
			pg.DrawString("Drehzahl", f, b_ws, 21f, 318f-15f*19f);

			pg.DrawString("Tz 1 ("+GetTrainType(1)+")", f, b_ws, 21f, 318f-13f*19f);

			if (count > 1)
			{
				// Skala links
				pg.DrawLine(p_ws_2, 20+118, 70, 20+118, 345);
				pg.DrawString("Tz 2 ("+GetTrainType(2)+")", f, b_ws, 21f+118f, 318f-13f*19f);
			}

			if (count == 3)
			{
				// Skala links
				pg.DrawLine(p_ws_2, 20+118*2, 70, 20+118*2, 345);
				pg.DrawString("Tz 3 ("+GetTrainType(3)+")", f, b_ws, 21f+118*2, 318f-13f*19f);
			}
			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			if (localstate.Drehzahl > 0f)
			{
				if (localstate.Drehzahl > 2200f) localstate.Drehzahl = 2200f;
				float hl = localstate.Drehzahl /*+ ((localstate.Drehzahl-500f)*1.5f)*/;
				height = Convert.ToSingle((double)hl * 0.11d);

				DrawKasten(ref pg, 20, 40, false, height+schw[0]);
				DrawKasten(ref pg, 43, 40, false, height+schw[1]);
				DrawKasten(ref pg, 66, 40, false, height+schw[3]);
				DrawKasten(ref pg, 89, 40, false, height+schw[4]);

				switch (count)
				{
					case 2:
						
						DrawKasten(ref pg, 20+118, 40, false, height+schw[4]);
						DrawKasten(ref pg, 43+118, 40, false, height+schw[5]);
						DrawKasten(ref pg, 66+118, 40, false, height+schw[6]);
						DrawKasten(ref pg, 89+118, 40, false, height+schw[7]);
						break;
					case 3:
						DrawKasten(ref pg, 20+118, 40, false, height+schw[4]);
						DrawKasten(ref pg, 43+118, 40, false, height+schw[5]);
						DrawKasten(ref pg, 66+118, 40, false, height+schw[6]);
						DrawKasten(ref pg, 89+118, 40, false, height+schw[7]);

						DrawKasten(ref pg, 20+2*118, 40, false, height+schw[8]);
						DrawKasten(ref pg, 43+2*118, 40, false, height+schw[9]);
						DrawKasten(ref pg, 66+2*118, 40, false, height+schw[10]);
						DrawKasten(ref pg, 89+2*118, 40, false, height+schw[11]);
						break;
					default:
						break;
				}
			}
								
			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawHL_Druck(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			pg.DrawLine(p_ws_2, 20, 80, 20, 365);

			for (int i = 0; i < 14; i++)
			{
				if (Math.IEEERemainder((double)i+3d, 4d) == 0d || i == 0)
				{
					pg.DrawLine(p_ws_2, 20, 365-19*i, 60, 365-19*i); // 0
				}
				else
				{
					pg.DrawLine(p_ws_2, 20, 365-19*i, 48, 365-19*i); // 0
				}
			}

			Font f = new Font("Tahoma", 11, FontStyle.Bold, GraphicsUnit.Point);
			pg.DrawString("0", f, b_ws, 50f, 347f);
			pg.DrawString("3", f, b_ws, 50f, 347f-19f);
			pg.DrawString("4", f, b_ws, 50f, 328f-4f*19f);
			pg.DrawString("5", f, b_ws, 50f, 328f-8f*19f);
			pg.DrawString("6", f, b_ws, 50f, 328f-12f*19f);

			pg.DrawString("bar", f, b_ws, 38f, 368f);
			pg.DrawString("HL", f, b_ws, 21f, 76f);

			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			// F�llung Kasten links 
			if (localstate.HL_Druck > 0.1f)
				height = 0.5f;
				
			if (localstate.HL_Druck > 2.8f)
			{
				if (localstate.HL_Druck > 6.25f) localstate.HL_Druck = 6.25f;
				float hl = localstate.HL_Druck - 2.8f;
				height = Convert.ToSingle((double)hl * 75.5d) - 6f;
			}

			DrawKasten(ref pg, 20, 100, true, height);

			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawHBL_Druck(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Skala rechts  41  610
			pg.DrawLine(p_ws_2, 570, 80, 570, 365);

			for (int i = 0; i < 11; i++)
			{
				if (Math.IEEERemainder((double)i, 2d) == 0d || i == 0)
				{
					pg.DrawLine(p_ws_2, 570, 365-24.7f*i, 609, 365-24.7f*i); // 0
				}
				else
				{
					pg.DrawLine(p_ws_2, 570, 365-24.7f*i, 597, 365-24.7f*i); // 0
				}
			}

			Font f = new Font("Tahoma", 11, FontStyle.Bold, GraphicsUnit.Point);
			pg.DrawString("0", f, b_ws, 600f, 347f);
			pg.DrawString("2", f, b_ws, 600f, 365f-24.7f*2-18f);
			pg.DrawString("4", f, b_ws, 600f, 365f-4*24.7f-18f);
			pg.DrawString("6", f, b_ws, 600f, 365f-6*24.7f-18f);
			pg.DrawString("8", f, b_ws, 600f, 365f-8*24.7f-18f);
			pg.DrawString("10", f, b_ws, 590f, 328f-12f*19f);

			pg.DrawString("bar", f, b_ws, 588f, 368f);
			pg.DrawString("HBL", f, b_ws, 571f, 76f);
			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			// F�llung Kasten rechts
			if (localstate.HBL_Druck > 0.1f)
				height = 0.5f;
				
			if (localstate.HBL_Druck > 10.8f) localstate.HBL_Druck = 10.8f;
			float hl = localstate.HBL_Druck;
			height = Convert.ToSingle((double)hl * 24.8d) -1f;

			DrawKasten(ref pg, 570, 100, false, height);


			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawHBL_vollstd_Druck(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			int traincount = CountTrains();

			// Skala rechts  41  610
			pg.DrawLine(p_ws_2, 138, 50, 138, 365);

			if (traincount > 1)
				pg.DrawLine(p_ws_2, 138+118, 80, 138+118, 365);
			if (traincount > 2)
				pg.DrawLine(p_ws_2, 138+118*2, 80, 138+118*2, 365);

			for (int i = 0; i < 11; i++)
			{
				if (Math.IEEERemainder((double)i, 2d) == 0d || i == 0)
				{
					pg.DrawLine(p_ws_2, 138, 365-24.7f*i, 138+39*traincount*3, 365-24.7f*i); // 0
				}
				else
				{
					pg.DrawLine(p_ws_2, 138, 365-24.7f*i, 138+39*traincount*3-12, 365-24.7f*i); // 0
				}
			}

			Font f = new Font("Tahoma", 11, FontStyle.Bold, GraphicsUnit.Point);
			pg.DrawString("0", f, b_ws, 129+39*traincount*3, 347f);
			pg.DrawString("2", f, b_ws, 129+39*traincount*3, 365f-24.7f*2-18f);
			pg.DrawString("4", f, b_ws, 129+39*traincount*3, 365f-4*24.7f-18f);
			pg.DrawString("6", f, b_ws, 129+39*traincount*3, 365f-6*24.7f-18f);
			pg.DrawString("8", f, b_ws, 129+39*traincount*3, 365f-8*24.7f-18f);
			pg.DrawString("10", f, b_ws, 119+39*traincount*3, 328f-12f*19f);

			pg.DrawString("bar", f, b_ws, 117+39*traincount*3, 368f);
			pg.DrawString("HBL", f, b_ws, 139f, 50f);
			
			Pen p1 = new Pen(b_ws, 1);

			float height = 0f;

			// F�llung Kasten rechts
			if (localstate.HBL_Druck > 0.1f)
				height = 0.5f;
				
			if (localstate.HBL_Druck > 10.8f) localstate.HBL_Druck = 10.8f;
			float hl = localstate.HBL_Druck;
			height = Convert.ToSingle((double)hl * 24.8d) -1f;

			DrawKasten(ref pg, 138, 100, false, height);
			DrawKasten(ref pg, 138+59, 100, false, height);

			if (traincount > 1) //138+39*traincount*3
			{
				DrawKasten(ref pg, 138+39*3+1, 100, false, height);
				DrawKasten(ref pg, 138+39*3+59+1, 100, false, height);
			}
			if (traincount > 2)
			{
				DrawKasten(ref pg, 138+39*2*3+2, 100, false, height);
				DrawKasten(ref pg, 138+39*2*3+59+2, 100, false, height);
			}

			DrawHBL_Text(ref pg, traincount);

			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawHBL_Text(ref Graphics pg, int traincount)
		{
			Font f = new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.Point);

			int counter = 1;

			if (localstate.ICEtype1 == ICE3TYPE.ICE403)
			{
				pg.DrawString("Tz 1 (BR403)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+8).ToString(), f, b_ws, 197, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE406)
			{
				pg.DrawString("Tz 1 (BR406)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+8).ToString(), f, b_ws, 197, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE411)
			{
				pg.DrawString("Tz 1 (BR411)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+7).ToString(), f, b_ws, 197, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415)
			{
				pg.DrawString("Tz 1 (BR415)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+5).ToString(), f, b_ws, 197, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605)
			{
				pg.DrawString("Tz 1 (BR605)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+4).ToString(), f, b_ws, 197, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2)
			{
				pg.DrawString("Tz 1 (BR415)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+5).ToString(), f, b_ws, 197, 328f-12f*19f);
				counter++;
				pg.DrawString("Tz 2 (BR415)", f, b_ws, 139+39*3+1, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139+39*3+1, 328f-12f*19f);
				pg.DrawString((counter*10+5).ToString(), f, b_ws, 197+39*3+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2)
			{
				pg.DrawString("Tz 1 (BR605)", f, b_ws, 139, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139, 328f-12f*19f);
				pg.DrawString((counter*10+4).ToString(), f, b_ws, 197, 328f-12f*19f);
				counter++;
				pg.DrawString("Tz 2 (BR605)", f, b_ws, 139+39*3+1, 76f);
				pg.DrawString((counter*10+1).ToString(), f, b_ws, 139+39*3+1, 328f-12f*19f);
				pg.DrawString((counter*10+4).ToString(), f, b_ws, 197+39*3+1, 328f-12f*19f);
			}


			if (localstate.ICEtype2 == ICE3TYPE.ICE403)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR403)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+8).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE406)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR406)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+8).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE411)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR411)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+7).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR415)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+5).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR605)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+4).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR415)", f, b_ws, 139+(39*3)*counter+1, 76f);
				pg.DrawString("Tz "+(counter+2).ToString()+" (BR415)", f, b_ws, 139+(39*3)*(counter+1)+1, 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+5).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+2)*10+1).ToString(), f, b_ws, 139+(39*3)*(counter+1)+1, 328f-12f*19f);
				pg.DrawString(((counter+2)*10+5).ToString(), f, b_ws, 197+(39*3)*(counter+1)+1, 328f-12f*19f);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2)
			{
				pg.DrawString("Tz "+(counter+1).ToString()+" (BR605)", f, b_ws, 139+(39*3)*counter, 76f);
				pg.DrawString("Tz "+(counter+2).ToString()+" (BR605)", f, b_ws, 139+(39*3)*(counter+1), 76f);
				pg.DrawString(((counter+1)*10+1).ToString(), f, b_ws, 139+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+1)*10+4).ToString(), f, b_ws, 197+(39*3)*counter+1, 328f-12f*19f);
				pg.DrawString(((counter+2)*10+1).ToString(), f, b_ws, 139+(39*3)*(counter+1)+1, 328f-12f*19f);
				pg.DrawString(((counter+2)*10+4).ToString(), f, b_ws, 197+(39*3)*(counter+1)+1, 328f-12f*19f);
			}
			//+39*3


		}
		private void DrawFdynTK(ref Graphics pg, byte tk)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawLine(p_b, 220+(tk-1)*170, 344, 210+(tk-1)*170, 344); // 0
			pg.DrawLine(p_b, 220+(tk-1)*170, 271, 210+(tk-1)*170, 271); // 50
			pg.DrawLine(p_b, 220+(tk-1)*170, 197, 210+(tk-1)*170, 197); // 100
			pg.DrawLine(p_b, 220+(tk-1)*170, 125, 210+(tk-1)*170, 125); // 150
			
			pg.DrawString("0",   f, Brushes.Black, 196+(tk-1)*170, 336f);
			pg.DrawString("50",  f, Brushes.Black, 187+(tk-1)*170, 263f);
			pg.DrawString("100", f, Brushes.Black, 178+(tk-1)*170, 189f);
			pg.DrawString("150", f, Brushes.Black, 178+(tk-1)*170, 117f);

			float height = 0f;

			if (localstate.ZugkraftGesammt < 0f)
			{
				if (localstate.ZugkraftGesammt < -167f) localstate.ZugkraftGesammt = -167f;
				height = Convert.ToSingle((double)localstate.ZugkraftGesammt * -1.47d);
			}

			DrawKasten(ref pg, 220+(tk-1)*170, 100, false, height);
			DrawKasten(ref pg, 278+(tk-1)*170, 100, false, height);

			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("kN", f, Brushes.Black, 188f+(tk-1)*170, 70f);
			pg.DrawString("Soll", f, Brushes.Black, 229f+(tk-1)*170, 70f);
			pg.DrawString("Ist", f, Brushes.Black, 290f+(tk-1)*170, 70f);
			pg.DrawString("F", f, Brushes.Black, 247f+(tk-1)*170, 354f);
			pg.DrawString("dyn TK"+tk.ToString(), f, Brushes.Black, 254f+(tk-1)*170, 358f);

			pg.SmoothingMode = SMOOTHING_MODE;

		}
		private void DrawT�rErkl�rung(ref Graphics pg)
		{
			pg.DrawRectangle(p_ws_2, 440, 40, 170, 70);

			pg.DrawRectangle(p_ws_2, 450, 50, 21, 21);
			pg.FillRectangle(new SolidBrush(Color.Yellow), 450, 80, 21, 21);
			pg.DrawRectangle(p_ws_2, 450, 80, 21, 21);

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawString("T�r geschlossen", f, b_ws, 480 ,53);
			pg.DrawString("T�r offen", f, b_ws, 480 ,83);
		}
		private void DrawT�rSammelLM(ref Graphics pg)
		{
			Font f = new Font("Arial", 30, FontStyle.Regular, GraphicsUnit.Point);

			if (EineT�rAuf)
			{
				pg.FillRectangle(new SolidBrush(Color.Yellow), 70, 231, 40, 40); 
				pg.DrawString("T", f, Brushes.Black, 71, 230);
			}
			else
			{
				pg.DrawString("T", f, b_ws, 71, 230);
			}
			pg.DrawRectangle(p_ws_2, 70, 231, 40, 40);
			

			
		}
		private int DrawT�rfelder(ref Graphics pg, int tk, int pos)
		{ 
			int waggons = CountWaggonsInTrain(tk);
			int move = pos*29;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawLine(p_ws_2, 142+move, 180, 142+move, 272); 
			pg.DrawString("Tz "+tk.ToString()+" ("+GetTrainType(tk)+")",f, b_ws, 142+move+2, 180); 
			
			for (int i = 0; i < waggons; i++)
			{
				pg.DrawString((tk*(10)+1+i).ToString(), f, b_ws, 150+i*28+move ,225);
				pg.DrawRectangle(p_ws_2, 150+i*28+move, 250, 21, 21);
				if (t�r[i+pos])
                    pg.FillRectangle(new SolidBrush(Color.Yellow), 150+i*28+move, 250, 21, 21);
			}

			return waggons;
		}
		private void DrawC_Druck(ref Graphics pg)
		{
			//pg.SmoothingMode = SmoothingMode.None;

			// Text
			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);

			// Seitentext

			int pos = 0;

			Pen p = new Pen(b_ws, 2);
			
			byte tz = 1;
			float radius = 24;

			pg.DrawString("D", f, b_ws, 90.5f+28*pos, 322f);
			pg.DrawEllipse(p, 85f+28*pos, 318f, radius, radius);

			if (localstate.ICEtype1 == ICE3TYPE.ICE403)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				if ((localstate.ICEtype2 != ICE3TYPE.ICE403 && localstate.ICEtype2 != ICE3TYPE.ICE406) && localstate.ICEtype2 != ICE3TYPE.NONE)
					pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICE3(ref pg, 0, true, tz);
				tz++;
				pos = 8;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE406)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				if (localstate.ICEtype2 != ICE3TYPE.ICE403 || localstate.ICEtype2 != ICE3TYPE.ICE406 || localstate.ICEtype2 != ICE3TYPE.NONE)
					pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICE3(ref pg, 0, false, tz);
				tz++;
				pos = 8;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE411)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				if (localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406)
					pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICET(ref pg, 0, true, tz);
				tz++;
				pos = 7;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				if (localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406)
					pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICET(ref pg, 0, false, tz);
				tz++;
				pos = 5;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICET(ref pg, 0, false, tz);
				tz++;
				DrawC_ICET(ref pg, 5, false, tz);
				tz++;
				pos = 10;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				if (localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406)
					pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICETD(ref pg, 0, false, tz);
				tz++;
				pos = 4;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2)
			{
				pg.DrawString("E", f, b_ws, 95f+28*pos, 322f-70f);
				if (localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406)
					pg.DrawString("WB", f, b_ws, 85f+28*pos, 322f-140f);
				pg.DrawString("Mg", f, b_ws, 85f+28*pos, 322f-210f);
				DrawC_ICETD(ref pg, 0, false, tz);
				tz++;
				DrawC_ICETD(ref pg, 4, false, tz);
				tz++;
				pos = 8;
			}
			
			if (localstate.ICEtype2 == ICE3TYPE.ICE403)
			{
				DrawC_ICE3(ref pg, pos, true, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE406)
			{
				DrawC_ICE3(ref pg, pos, false, tz);
			}															   
			else if (localstate.ICEtype2 == ICE3TYPE.ICE411)
			{
				DrawC_ICET(ref pg, pos, true, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415)
			{
				DrawC_ICET(ref pg, pos, false, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2)
			{
				DrawC_ICET(ref pg, pos, false, tz);
				tz++;
				DrawC_ICET(ref pg, pos+5, false, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605)
			{
				DrawC_ICETD(ref pg, pos, false, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2)
			{
				DrawC_ICETD(ref pg, pos, false, tz);
				tz++;
				DrawC_ICETD(ref pg, pos+4, false, tz);
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		private void DrawC_ICE3(ref Graphics pg, int pos, bool isBr403, byte tz)
		{
			// Trenner
			pg.DrawLine(p_ws_2, 115+28*pos, 45, 115+28*pos, 365); 

			// Text
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			if (isBr403)
				pg.DrawString("Tz "+tz.ToString()+" (BR403)", f, b_ws, 117f+28*pos, 50f);
			else
				pg.DrawString("Tz "+tz.ToString()+" (BR406)", f, b_ws, 117f+28*pos, 50f);

			float height_d = 0f;
			float height_e = 0f;
			float height_wb = 0f;
			//float height_mg = 0f;

			// Variablenf�llung
			if (localstate.Zugkraft < 0) 
			{
				height_e = 50;
				if (localstate.Geschwindigkeit > 50f)
				{
					if (localstate.wbBremse == WBBremse.BB_SB) 
					{
						height_wb = 50;
					}
					else if ((localstate.HL_Druck < 3.4f) && localstate.wbBremse == WBBremse.SB) 
					{
						height_wb = 50;
					}
				}
			}

			if (localstate.wbBremse == WBBremse.AUS) height_wb = -1f;
			if (localstate.LM_HS) height_e = -1f;

			/*if (localstate.C_Druck > 0.1f)
			{
				height_d = 50;
			}*/

			
			// (D)
			for (int i = 0; i < 8; i++)
			{
				height_d = GetC_DruckHeight(i);

				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 76f);
				DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
			}

			// E
			for (int i = 0; i < 8; i++)
			{
				if (i==0 || i==2 || i==7 || i==5)
					DrawKastenSmall(ref pg, 119+28*(i+pos), 295, true, height_e, false, MMI_ORANGE);
			}
			
			// Wb
			for (int i = 0; i < 8; i++)
			{
				if (!(i==0 || i==2 || i==7 || i==5))
					DrawKastenSmall(ref pg, 119+28*(i+pos), 225, true, height_wb, false, MMI_CYAN);
			}
			
			// Mg
			/*for (int i = 0; i < 8; i++)
			{
				DrawKastenSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
			}*/
		}
		private void DrawC_ICET(ref Graphics pg, int pos, bool isBr411, byte tz)
		{
			// Trenner
			pg.DrawLine(p_ws_2, 115+28*pos, 45, 115+28*pos, 365); 

			// Text
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			if (isBr411)
				pg.DrawString("Tz "+tz.ToString()+" (BR411)", f, b_ws, 117f+28*pos, 50f);
			else
				pg.DrawString("Tz "+tz.ToString()+" (BR415)", f, b_ws, 117f+28*pos, 50f);

			float height_d = 0f;
			float height_e = 0f;
			float height_mg = 0f;

			// Variablenf�llung
			if (localstate.Zugkraft < 0) 
			{
				height_e = 50;
			}

			if (localstate.LM_MG) height_mg = 50;

			if (localstate.LM_HS) height_e = -1f;

			if (localstate.Bremsstellung != BREMSSTELLUNG.R_Mg) height_mg = -1;

			if (localstate.C_Druck > 0.1f)
			{
				height_d = 50;
			}

			int target = 5;
			if (isBr411) target = 7;
			
			// (D)
			for (int i = 0; i < target; i++)
			{
				height_d = GetC_DruckHeight(i);

				if (isBr411)
				{
					if (i==1 || i==2 || i==4 || i==5)
					{
						DrawKastenSmallSpacer(ref pg, 119+28*(i+pos), 365, true, height_d, false, true, MMI_BLUE);
					}
					else
					{
						DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);					
					}
				}
				else
				{
					if (i==1 || i==2 || i==3)
					{
						DrawKastenSmallSpacer(ref pg, 119+28*(i+pos), 365, true, height_d, false, true, MMI_BLUE);
					}
					else
					{
						DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);					
					}
				}
				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 76f);
				
			}

			// E  411: 2+3+5+6  415: 2+3+4
			for (int i = 0; i < target; i++)
			{
				if (isBr411)
				{
					if (i==1 || i==2 || i==4 || i==5)
						DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 295, true, height_e, false, MMI_ORANGE);
				}
				else
				{
					if (i==1 || i==2 || i==3)
						DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 295, true, height_e, false, MMI_ORANGE);
				}
			}
						
			// Mg
			for (int i = 0; i < target; i++)
			{
				if (isBr411)
				{
					if (i == 1 || i == 2 || i ==5)
						DrawKastenSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
					else
						DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
				}
				else
				{
					if (i == 2 || i == 3)
						DrawKastenSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
					else
						DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
				}
			}
		}
		private void DrawC_ICETD(ref Graphics pg, int pos, bool isBr411, byte tz)
		{
			// Trenner
			pg.DrawLine(p_ws_2, 115+28*pos, 45, 115+28*pos, 365); 

			// Text
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("Tz "+tz.ToString()+" (BR605)", f, b_ws, 117f+28*pos, 50f);

			float height_d = 0f;
			float height_e = 0f;
			float height_mg = 0f;

			// Variablenf�llung
			if (localstate.Zugkraft < 0) 
			{
				height_e = 50;
			}

			if (localstate.LM_MG) height_mg = 50;
			if (localstate.Bremsstellung != BREMSSTELLUNG.R_Mg) height_mg = -1;

			if (localstate.LM_HS) height_e = -1f;

			if (localstate.C_Druck > 0.1f)
			{
				height_d = 50;
			}

			int target = 4;
			
			// (D)
			for (int i = 0; i < target; i++)
			{
				height_d = GetC_DruckHeight(i);

				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 76f);
				if (i == 0 || i == 2)
					DrawKastenSmallSpacer(ref pg, 119+28*(i+pos), 365, true, height_d, false, false, MMI_BLUE);
				else
					DrawKastenSmallSpacer(ref pg, 119+28*(i+pos), 365, true, height_d, false, true, MMI_BLUE);
			}

			// E  411: 2+3+5+6  415: 2+3+4
			for (int i = 0; i < target; i++)
			{
				if (i== 0 || i == 2)
				{
					DrawKastenSmallSmall(ref pg, 119+10+28*(i+pos), 295, true, height_e, false, MMI_ORANGE);
				}
				else
				{
					DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 295, true, height_e, false, MMI_ORANGE);
				}
			}
						
			// Mg
			for (int i = 0; i < target; i++)
			{
				if (i==0)
					DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);
				else if (i==2)
				{}
				else
					DrawKastenSmall(ref pg, 119+28*(i+pos), 155, true, height_mg, false, MMI_RED);

			}
		}
		private void DrawFspBr(ref Graphics pg)
		{
			// Text
			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);

			// Seitentext

			int pos = 0;

			Pen p = new Pen(b_ws, 2);
			
			byte tz = 1;

			pg.DrawString("FspBr", f, b_ws, 65f, 322f);

			if (localstate.ICEtype1 == ICE3TYPE.ICE403)
			{
				DrawFspBr_ICE3(ref pg, 0, true, tz);
				tz++;
				pos = 8;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE406)
			{
				DrawFspBr_ICE3(ref pg, 0, false, tz);
				tz++;
				pos = 8;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE411)
			{
				DrawFspBr_ICET(ref pg, 0, true, tz);
				tz++;
				pos = 7;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415)
			{
				DrawFspBr_ICET(ref pg, 0, false, tz);
				tz++;
				pos = 5;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2)
			{
				DrawFspBr_ICET(ref pg, 0, false, tz);
				tz++;
				DrawFspBr_ICET(ref pg, 5, false, tz);
				tz++;
				pos = 10;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605)
			{
				DrawFspBr_ICETD(ref pg, 0, tz);
				tz++;
				pos = 4;
			}
			else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2)
			{
				DrawFspBr_ICETD(ref pg, 0, tz);
				tz++;
				DrawFspBr_ICETD(ref pg, 4, tz);
				tz++;
				pos = 8;
			}
			
			if (localstate.ICEtype2 == ICE3TYPE.ICE403)
			{
				DrawFspBr_ICE3(ref pg, pos, true, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE406)
			{
				DrawFspBr_ICE3(ref pg, pos, false, tz);
			}															   
			else if (localstate.ICEtype2 == ICE3TYPE.ICE411)
			{
				DrawFspBr_ICET(ref pg, pos, true, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415)
			{
				DrawFspBr_ICET(ref pg, pos, false, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2)
			{
				DrawFspBr_ICET(ref pg, pos, false, tz);
				tz++;
				DrawFspBr_ICET(ref pg, pos+5, false, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605)
			{
				DrawFspBr_ICETD(ref pg, pos, tz);
			}
			else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2)
			{
				DrawFspBr_ICETD(ref pg, pos, tz);
				tz++;
				DrawFspBr_ICETD(ref pg, pos+4, tz);
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		private void DrawFspBr_ICE3(ref Graphics pg, int pos, bool isBr403, byte tz)
		{					
			int height_d = 0;

			if (IsFspBrOn)
				height_d = 50;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawLine(p_ws_2, 117+28*(pos)-2, 250, 117+28*(pos)-2, 365);

			// Text
			if (isBr403)
				pg.DrawString("Tz "+tz.ToString()+" (BR403)", f, b_ws, 117f+28*pos, 250f);
			else
				pg.DrawString("Tz "+tz.ToString()+" (BR406)", f, b_ws, 117f+28*pos, 250f);

			for (int i = 0; i < 8; i++)
			{
				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 276f);
				if (i==1 || i==3 || i== 4|| i==6)
					DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
			}
		}
		private void DrawFspBr_ICET(ref Graphics pg, int pos, bool isBr411, byte tz)
		{
			int height_d = 0;

			if (IsFspBrOn)
				height_d = 50;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawLine(p_ws_2, 117+28*(pos)-2, 250, 117+28*(pos)-2, 365);

			// Text
			if (isBr411)
				pg.DrawString("Tz "+tz.ToString()+" (BR411)", f, b_ws, 117f+28*pos, 250f);
			else
				pg.DrawString("Tz "+tz.ToString()+" (BR415)", f, b_ws, 117f+28*pos, 250f);

			int target = 5;
			if (isBr411) target = 7;

			for (int i = 0; i < target; i++)
			{
				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 276f);
				if (isBr411)
				{
					if (i==1 || i==2 || i== 4|| i==5)
						DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
				}
				else
				{
					if (i==1 || i==2)
						DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
					else if (i== 3)
						DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
				}
			}
		}
		private void DrawFspBr_ICETD(ref Graphics pg, int pos, byte tz)
		{
			int height_d = 0;

			if (IsFspBrOn)
				height_d = 50;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawLine(p_ws_2, 117+28*(pos)-2, 250, 117+28*(pos)-2, 365);

			// Text
			pg.DrawString("Tz "+tz.ToString()+" (BR605)", f, b_ws, 117f+28*pos, 250f);

			for (int i = 0; i < 4; i++)
			{
				pg.DrawString((i+(tz-1)*10+11).ToString(), f, b_ws, 117f+28*(i+pos), 276f);
				if (i==1)
					DrawKastenSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);
				else if (i==2)
					DrawKastenSmallSmall(ref pg, 119+28*(i+pos), 365, true, height_d, false, MMI_BLUE);

			}
		}
		private void DrawS01(ref Graphics pg, bool greater)
		{
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "", ip = "";
			IPHostEntry ipep = Dns.GetHostByName(Dns.GetHostName());

			St�rung st = localstate.st�rungmgr.LastSt�rung;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws, 20, 1);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			if (ipep.AddressList.Length < 1)
			{
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gest�rt!", f, b_ws, 50, 40);
				pg.DrawString("Ihr Rechner hat im Augenblick keine eindeutige IP-Adresse!", f, b_ws, 50, 60);
				pg.DrawString("- Systemsteuerung benutzen", f, b_ws, 50, 80);
				pg.DrawString("- Netzwerkkarte oder Loopback-Adapter installieren", f, b_ws, 50, 100);
				pg.DrawString("- Statische IP-Adresse zuweisen", f, b_ws, 50, 120);
				pg.DrawString("- Netzwerk aktivieren", f, b_ws, 50, 140);
				pg.DrawString("- System neu starten", f, b_ws, 50, 160);
			}
			else
			{
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gest�rt!", f, b_ws, 50, 40);
				pg.DrawString("- ZUSI starten", f, b_ws, 50, 60);
				pg.DrawString("- ZUSI TCP Server starten und einschalten", f, b_ws, 50, 80);
				pg.DrawString("- Host: '"+m_conf.Host+"' �berpr�fen", f, b_ws, 50, 100);
				pg.DrawString("- Port: "+m_conf.Port.ToString()+" �berpr�fen", f, b_ws, 50, 120);
				pg.DrawString("- Automatisches Anmelden des Displays �berwachen", f, b_ws, 50, 140);
				pg.DrawString("- Im Fehlerfall TCP Server ausschalten und wieder einschalten", f, b_ws, 50, 160);
				pg.DrawString("- Display neu starten", f, b_ws, 50, 180);
				pg.DrawString("- Nach erfolgreicher Anmeldung, ZUSI verbinden", f, b_ws, 50, 200);
				pg.DrawString("- Feld 'Angeforderte Gr��en' beobachten", f, b_ws, 50, 220);

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
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			St�rung st = localstate.st�rungmgr.LastSt�rung;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws, 20, 1);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gest�rt!", f, b_ws, 50, 40);
			pg.DrawString("ZUSI ist bereits mit dem TCP Server verbunden!", f, b_ws, 50, 60);
			pg.DrawString("- Verbindung zwischen ZUSI und TCP Server trennen", f, b_ws, 50, 80);
			pg.DrawString("- Display meldet sich automatisch an Server an", f, b_ws, 50, 100);
			pg.DrawString("- ZUSI wieder verbinden", f, b_ws, 50, 120);
		}

		private void DrawS02(ref Graphics pg, bool greater)
		{
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			St�rung st = localstate.st�rungmgr.St�rStack;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, b_ws, 20, 1);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			pg.DrawString("Die Trennsch�tze in Ihrem Triebfahrzeug sind gefallen!", f, b_ws, 50, 40);
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
		private void DrawUhr(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			if (V_HELP)
			{
				if (localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0)
				{
					s = "   V>0";
				}
				else
				{
					s = "   V=0";
				}
			}
			else
			{

				if (vtime.Hour < 10)
					s += "0"+vtime.Hour+":";
				else
					s += vtime.Hour+":";
				if (vtime.Minute < 10)
					s += "0"+vtime.Minute+":";
				else
					s += vtime.Minute+":";
				if (vtime.Second < 10)
					s+= "0"+vtime.Second;
				else
					s+= vtime.Second;
			}
			
			pg.DrawString(s, f, b_ws, 550, 1);
		}
		private void DrawDatum(ref Graphics pg)
		{
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			if (!V_HELP)
			{
				s = vtime.ToShortDateString();
				s = s.Remove(6,2);
			}
			else
			{
				s = "00:00";
			}
			pg.DrawString(s, f, b_ws, 492, 1);
		}
		private void DrawZugNr(ref Graphics pg)
		{
			if (!V_HELP)
			{
				pg.SmoothingMode = SmoothingMode.None;

				Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
				pg.DrawString("Zug: "+localstate.Zugnummer, f, b_ws, 20, 1);
			}
		}
		private void DrawDispaly(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);
		
			if (DEBUG != "")
			{
				pg.DrawString(DEBUG, f, b_ws, 140, 1);
				return;
			}

			switch (localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.D2_Zustand:
					pg.DrawString("Bremszustand", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.D2_HBL:
					pg.DrawString("Hauptluftbeh�lterleitungsdruck", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.D2_Schalt:
					pg.DrawString("Schaltzustand Bremsen", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.D2_FspBr:
					pg.DrawString("Federspeicherbremsen", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.ST:
					pg.DrawString("St�rungs�bersicht", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					break;
				case CURRENT_DISPLAY.D1_Grundb:
					pg.DrawString("Grundbild", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.D1_FB:
					pg.DrawString("Fahren/Bremsen", f, b_ws, 140, 1);
					break;
				case CURRENT_DISPLAY.D1_Abfert:
					pg.DrawString("Abfertigen", f, b_ws, 140, 1);
					break;
				default:
					pg.DrawString("ST�RUNG!", f, b_ws, 140, 1);
					break;
			}
			
		}
		private void DrawAFB(ref Graphics pg)
		{
			if (localstate.LM_AFB)
			{
				Font f = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
				pg.DrawString("AFB "+localstate.AFB_SollGeschwindigkeit.ToString()+" km/h", f, b_ws, 30, 396);
			}
			else
			{
				Font f = new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.Point);
				pg.DrawString("AFB Aus", f, b_ws, 21, 396);
			}
		}
		private void DrawTfT�ren(ref Graphics pg)
		{
			if (localstate.T�ren == T�REN.FREIGEGEBEN)
			{
				pg.FillRectangle(Brushes.Red, 20+3*59, 395, 59, 21);
				Font f = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
				pg.DrawString("Tf-T frei", f, b_ws, 23+3*59, 397);
			}
		}
		private void DrawWB(ref Graphics pg)
		{
			if (D2_Schalt_B3() || D2_Schalt_B1() || D2_Schalt_B1())
			{
				if (localstate.wbBremse == WBBremse.AUS)
				{
					pg.FillRectangle(Brushes.Red, 256, 395, 59, 21);
					Font f = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
					pg.DrawString("WB Aus", f, new SolidBrush(Color.WhiteSmoke), 24+4*59, 397);
				}
				else if (localstate.wbBremse == WBBremse.SB)
				{
					pg.FillRectangle(Brushes.Red, 256, 395, 59, 21);
					Font f = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
					pg.DrawString("WB SB", f, new SolidBrush(Color.WhiteSmoke), 27+4*59, 397);
				}
			}
		}
		private void DrawSt�rung(ref Graphics pg)
		{
			string s = "";
			Font f = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
			Brush b_st_bg = new SolidBrush(ST�RUNG_BG);
			Brush b_st_fg = new SolidBrush(ST�RUNG_FG);
				 
			if (localstate.st�rungmgr.Current.Type != ENUMSt�rung.NONE)
			{
				pg.SmoothingMode = SmoothingMode.None;

				pg.FillRectangle(b_st_bg, 434, 396, 3*59-1, 20);

				St�rung st = localstate.st�rungmgr.Current;

				s = "St�rung in ";
				if (st.Priority < 10) s+= "0";
				s+= st.Priority.ToString();
				s+= " "+st.Name;
				
				pg.SmoothingMode = SMOOTHING_MODE;
			}
			else if (localstate.st�rungmgr.GetPassives().Count > 1)
			{
				pg.SmoothingMode = SmoothingMode.None;

				pg.FillRectangle(b_st_bg, 434, 396, 3*59-1, 20);

				St�rung st = localstate.st�rungmgr.Current;

				s = "St.";
				
				pg.SmoothingMode = SMOOTHING_MODE;
			}

			pg.DrawString(s, f, b_st_fg, 435, 397);
		}
		private void DrawFPL(ref Graphics pg)
		{
			Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			pg.SmoothingMode = SmoothingMode.None;

			pg.FillRectangle(Brushes.LightBlue, 60, 12, 50, 36);
			DrawSoftKey(ref pg, 55, 8);
			
			Font f = new Font(other_font, 13, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("Fpl", f, Brushes.Black, 70, 20);
			
			pg.SmoothingMode = SMOOTHING_MODE;
		}
		private void DrawNONE(ref Graphics pg)
		{
			Font f = new Font(other_font, 42, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("St�rung", f, b_ws, 210, 200);

		}
		private void DrawSoftKeys(ref Graphics pg)
		{
			Font f = new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.Point);
			//Font f = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);

			DrawText(ref pg);
		}
		private void DrawSoftKey(ref Graphics pg, int x, int y)
		{
			Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			pg.SmoothingMode = SmoothingMode.None;

			pg.DrawLine(p_ws_3, x  , y, x+58, y);
			pg.DrawLine(p_ws_3, x+1, y, x+1 , y+44);

			pg.DrawLine(p_dg_3, x   , y+44,   x+58, y+44);
			pg.DrawLine(p_dg_3, x+58, y+44+1, x+58, y-1);
			
			pg.DrawRectangle(p_ws_1, x+4, y+3, 50, 37);
			
			pg.DrawLine(p_dg_1, x+54, y+3, x+54, y+40);
			pg.DrawLine(p_dg_1, x+4, y+40, x+54, y+40);
			
			pg.SmoothingMode = SMOOTHING_MODE;

		}
		private void DrawSchaltzustand(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			Font f = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Point);

			if (IsICE3InTrain)
			{
				if (localstate.wbBremse == WBBremse.AUS)
				{
					pg.FillRectangle(b_ws, 59*2+20, 50, 59*6, 20);
					pg.DrawString("WB gesperrt", f, b_ws_alt, 59*2+20, 50);
					pg.DrawString("WB nur f�r Schnellbremsung", f, b_ws, 59*2+20, 75);
					pg.DrawString("WB f�r Schnellbremsung und Betriebsbremsung", f, b_ws, 59*2+20, 100);
				}
				else if (localstate.wbBremse == WBBremse.SB)
				{
					pg.DrawString("WB gesperrt", f, b_ws, 59*2+20, 50);
					pg.FillRectangle(b_ws, 59*2+20, 75, 59*6, 20);
					pg.DrawString("WB nur f�r Schnellbremsung", f, b_ws_alt, 59*2+20, 75);
					pg.DrawString("WB f�r Schnellbremsung und Betriebsbremsung", f, b_ws, 59*2+20, 100);
				}
				else
				{
					pg.DrawString("WB gesperrt", f, b_ws, 59*2+20, 50);
					pg.DrawString("WB nur f�r Schnellbremsung", f, b_ws, 59*2+20, 75);
					pg.FillRectangle(b_ws, 59*2+20, 100, 59*6, 20);
					pg.DrawString("WB f�r Schnellbremsung und Betriebsbremsung", f, b_ws_alt, 59*2+20, 100);
				}
			}

			pg.DrawString("Luftpresser Aus", f, b_ws, 59*2+20, 150);
			pg.FillRectangle(b_ws, 59*2+20, 175, 59*2, 20);
			pg.DrawString("Luftpresser An", f, b_ws_alt, 59*2+20, 175);

			pg.FillRectangle(b_ws, 59*2+20, 230, 59*4-10, 20);
			pg.DrawString("F�hrerbremsventil aufgesperrt", f, b_ws_alt, 59*2+20, 230);
			pg.DrawString("F�hrerbremsventil abgesperrt", f, b_ws, 59*2+20, 255);

			pg.DrawString("Sandtrocknung Aus", f, b_ws, 59*2+20, 310);
			pg.FillRectangle(b_ws, 59*2+20, 335, 59*2+25, 20);
			pg.DrawString("Sandtrocknung An", f, b_ws_alt, 59*2+20, 335);
			

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		public void SetButtons()
		{
		}	

		private void DrawText(ref Graphics pg)
		{
			Font f = new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.Point);

			if (localstate.DISPLAY == CURRENT_DISPLAY.D2_Zustand)
			{
				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 0*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("HL an-", f, b_ws, 26, 426);
				pg.DrawString("gleichen", f, b_ws, 22, 440);
				*/

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 1*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("", f, b_ws, 26+59*1, 426);
				//pg.DrawString("", f, b_ws, 22+59*1, 440);

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 2*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("BRH", f, b_ws, 34+59*2, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*2, 440);
				*/

				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 3*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Schalt-", f, b_ws, 26+59*3, 426);
				pg.DrawString("zustand", f, b_ws, 22+59*3, 440);

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 4*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Brems-", f, b_ws, 26+59*4, 426);
				pg.DrawString("probe", f, b_ws, 28+59*4, 440);
				*/

				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 5*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("FspBr", f, b_ws, 31+59*5, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*5, 440);

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 6*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("HL an-", f, b_ws, 26+59*6, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*6, 440);

				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 7*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("HBL", f, b_ws, 36+59*7, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*7, 440);

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 8*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("Fpl", f, b_ws, 38+59*8, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*8, 440);

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("HL an-", f, b_ws, 26+59*9, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*9, 440);
				
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D2_HBL)
			{
				// 9
				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 8*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("Fpl", f, b_ws, 38+59*8, 426);                                          				
				
				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("B", f, b_ws, 44+59*9, 426);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D2_Schalt)
			{
				if (D2_Schalt_B1())
				{
					//1
					if (!Inverse) pg.FillRectangle(Brushes.Wheat, 0*59+20+1, this.Height - 35, 59-1, 35); 
					pg.DrawString("WB", f, b_ws, 38, 426);
					pg.DrawString("sperren", f, b_ws, 24, 440);
				}
			
				if (D2_Schalt_B2())
				{
					//2
					if (!Inverse) pg.FillRectangle(Brushes.Wheat, 1*59+20+1, this.Height - 35, 59-1, 35); 
					pg.DrawString("WB", f, b_ws, 38+59*1, 426);
					pg.DrawString("nur SB", f, b_ws, 26+59*1, 440);
				}

				if (D2_Schalt_B3()) 
				{
					//3
					if (!Inverse) pg.FillRectangle(Brushes.Wheat, 2*59+20+1, this.Height - 35, 59-1, 35); 
					pg.DrawString("WB", f, b_ws, 38+59*2, 426);
					pg.DrawString("SB+BB", f, b_ws, 26+59*2, 440);
				}

				// 9
				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 8*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("Fpl", f, b_ws, 38+59*8, 426);                                          				
				
				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("B", f, b_ws, 44+59*9, 426);
				
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D2_FspBr)
			{
				// 9
				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 8*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("Fpl", f, b_ws, 38+59*8, 426);                                          				
				
				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("B", f, b_ws, 44+59*9, 426);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.V_EQUAL_0 || localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0)
			{
				//2
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 1*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("St.-", f, b_ws, 38+59*1, 426);
				pg.DrawString("�bers.", f, b_ws, 26+59*1, 440);

				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("zur�ck", f, b_ws, 28+59*9, 426);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.ST)
			{
				if (ht2.Count > 0 && ht2[St�rPos] != null)
				{
					// 4
					if (!Inverse) pg.FillRectangle(Brushes.Wheat, 3*59+20+1, this.Height - 35, 59-1, 35); 
					pg.DrawString("Marke", f, b_ws, 28+59*3, 426);
					pg.DrawString("l�schen", f, b_ws, 24+59*3, 440);
				}

				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("zur�ck", f, b_ws, 28+59*9, 426);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D1_Grundb)
			{
				/* 
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 0*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("HL an-", f, b_ws, 26, 426);
				pg.DrawString("gleichen", f, b_ws, 22, 440);
				*/

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 1*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("", f, b_ws, 26+59*1, 426);
				//pg.DrawString("", f, b_ws, 22+59*1, 440);

				
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 2*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Ab-", f, b_ws, 38+59*2, 426);
				pg.DrawString("fertigen", f, b_ws, 24+59*2, 440);
				

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 3*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Schalt-", f, b_ws, 26+59*3, 426);
				pg.DrawString("zustand", f, b_ws, 22+59*3, 440);
				*/

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 4*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Brems-", f, b_ws, 26+59*4, 426);
				pg.DrawString("probe", f, b_ws, 28+59*4, 440);
				*/

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 5*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("V/A", f, b_ws, 36+59*5, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*5, 440);
				*/

				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 6*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("F/B", f, b_ws, 36+59*6, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*6, 440);

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 7*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("HBL", f, b_ws, 36+59*7, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*7, 440);
				*/

				/*
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 8*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Fpl", f, b_ws, 38+59*8, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*8, 440);
				*/

				//if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				//pg.DrawString("HL an-", f, b_ws, 26+59*9, 426);
				//pg.DrawString("gleichen", f, b_ws, 22+59*9, 440);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D1_FB)
			{
				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Grund-", f, b_ws, 27+59*9, 426);
				pg.DrawString("bild", f, b_ws, 37+59*9, 440);
			}
			else if (localstate.DISPLAY == CURRENT_DISPLAY.D1_Abfert)
			{
				// 0
				if (!Inverse) pg.FillRectangle(Brushes.Wheat, 9*59+20+1, this.Height - 35, 59-1, 35); 
				pg.DrawString("Grund-", f, b_ws, 27+59*9, 426);
				pg.DrawString("bild", f, b_ws, 37+59*9, 440);
			}

			DrawLines(ref pg);

		}

		private string GetDayOfWeek(string dow)
		{
			switch (dow)
			{
				case "Sunday":
					return "So";
				case "Monday":
					return "Mo";
				case "Tuesday":
					return "Di";
				case "Wednesday":
					return "Mi";
				case "Thursday":
					return "Do";
				case "Friday":
					return "Fr";
				case "Saturday":
					return "Sa";
				default:
					return "";
			}
		}

		private int CountTrains()
		{
			int i = 1;

			if (localstate.ICEtype1 == ICE3TYPE.ICE415_2 || localstate.ICEtype1 == ICE3TYPE.ICE605_2)
				i++;
			if (localstate.ICEtype2 != ICE3TYPE.NONE)
				i++;
			if (localstate.ICEtype2 == ICE3TYPE.ICE415_2 || localstate.ICEtype2 == ICE3TYPE.ICE605_2)
				i++;

			return i;
		}
		private int GetC_DruckHeight(int i)
		{
			int height_d = 0;

			if (c_druck[i] + localstate.C_Druck > 0.1f) 
			{
				height_d = 50;
			}
			else 
				height_d = 0;

			return height_d;
		}
		private bool GetC_Brake_Standby()
		{
			return (
					(localstate.Zugkraft < 0f) && (localstate.HL_Druck > 3.5f)
				   );
		}
		private bool IsICE3InTrain
		{
			get
			{
				return (localstate.ICEtype1 == ICE3TYPE.ICE403 || localstate.ICEtype1 == ICE3TYPE.ICE406 || localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406);
			}
		}
		private bool IsICETDTrain
		{
			get
			{
				return ((localstate.ICEtype1 == ICE3TYPE.ICE605 || localstate.ICEtype1 == ICE3TYPE.ICE605_2) && (localstate.ICEtype2 == ICE3TYPE.ICE605 || localstate.ICEtype2 == ICE3TYPE.ICE605_2 || localstate.ICEtype2 == ICE3TYPE.NONE));
			}
		}
		private bool IsICETDLeading
		{
			get
			{
				return ((localstate.ICEtype1 == ICE3TYPE.ICE605 || localstate.ICEtype1 == ICE3TYPE.ICE605_2));
			}
		}
		private int  CountICETD
		{
			get
			{
				int counter = 0;
				if (localstate.ICEtype1 == ICE3TYPE.ICE605) counter++;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2) counter += 2;

				if (localstate.ICEtype2 == ICE3TYPE.ICE605) counter++;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2) counter += 2;
				return counter;
			}
		}
		private int CountICE3
		{
			get
			{
				int counter = 0;
				if (localstate.ICEtype1 == ICE3TYPE.ICE403) counter++;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE406) counter++;

				if (localstate.ICEtype2 == ICE3TYPE.ICE403) counter++;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE406) counter++;
				return counter;
			}
		}
		private int  CountICET
		{
			get
			{
				int counter = 0;
				if (localstate.ICEtype1 == ICE3TYPE.ICE411) counter++;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415) counter++;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2) counter += 2;

				if (localstate.ICEtype2 == ICE3TYPE.ICE411) counter++;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE415) counter++;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2) counter += 2;
				return counter;
			}
		}

		private int  CountWaggons
		{
			get
			{
				int counter = 0;
				if (localstate.ICEtype1 == ICE3TYPE.ICE411) counter += 7;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415) counter += 5;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2) counter += 10;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE403) counter += 8;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE406) counter += 8;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605) counter += 4;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2) counter += 8;

				if (localstate.ICEtype2 == ICE3TYPE.ICE411) counter += 7;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE415) counter += 5;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2) counter += 10;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE403) counter += 8;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE406) counter += 8;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE605) counter += 4;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2) counter += 8;

				return counter;
			}
		}

		private int CountWaggonsInTrain(int where)
		{
			int counter = 0;

			if (where == 1)
			{
				if (localstate.ICEtype1 == ICE3TYPE.ICE411) counter += 7;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415) counter += 5;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2) counter += 5;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE403) counter += 8;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE406) counter += 8;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605) counter += 4;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2) counter += 4;
			}
			else if (where == 2)
			{
				if (localstate.ICEtype1 == ICE3TYPE.ICE415_2) counter += 5;
				else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2) counter += 4;
				else
				{
					if (localstate.ICEtype2 == ICE3TYPE.ICE411) counter += 7;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE415) counter += 5;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2) counter += 4;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE403) counter += 8;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE406) counter += 8;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE605) counter += 4;
					else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2) counter += 4;
				}			
			}
			else if (where == 3)
			{
				if (localstate.ICEtype2 == ICE3TYPE.ICE415) counter += 5;
				else if (localstate.ICEtype2 == ICE3TYPE.ICE605) counter += 4;
			}
			else return 0;

			return counter;
		}

		private bool IsFspBrOn
		{
			get
			{
				return (
					    /*(localstate.C_Druck > 1f && localstate.HL_Druck > 4.9f) ||*/
					    (localstate.Zusatzbremse < 2f)
					   );
			}
		}
		private bool V_HELP
		{
			get
			{
				return (localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0 || localstate.DISPLAY == CURRENT_DISPLAY.V_EQUAL_0);
			}
		}
		private void RecreateOberstrom()
		{
			float newOberstrom = 0f;

			switch(localstate.ICEtype1)
			{
				case ICE3TYPE.ICE403:
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE406:
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE411:
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE415:
					newOberstrom += localstate.Oberstrom*3f;
					break;
				case ICE3TYPE.ICE415_2:
					newOberstrom += localstate.Oberstrom*6f;
					break;
			}

			switch(localstate.ICEtype2)
			{
				case ICE3TYPE.ICE403:
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE406:
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE411:					  
					newOberstrom += localstate.Oberstrom*4f;
					break;
				case ICE3TYPE.ICE415:
					newOberstrom += localstate.Oberstrom*3f;
					break;								  
				case ICE3TYPE.ICE415_2:
					newOberstrom += localstate.Oberstrom*6;
					break;
			}

			localstate.OberstromToRender = newOberstrom;

		}
		private string GetTrainType(int i)
		{
			switch (i)
			{
				case 1:
					if (localstate.ICEtype1 == ICE3TYPE.ICE403)
						return "BR 403";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE406)
						return "BR 406";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE411)
						return "BR 411";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE415)
						return "BR 415";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE415_2)
						return "BR 415";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE605)
						return "BR 605";
					else if (localstate.ICEtype1 == ICE3TYPE.ICE605_2)
						return "BR 605";
					break;
				case 2:
					if (localstate.ICEtype1 == ICE3TYPE.ICE415_2 || localstate.ICEtype1 == ICE3TYPE.ICE605_2)
					{
						if (localstate.ICEtype1 == ICE3TYPE.ICE415_2)
							return "BR 415";
						else
							return "BR 605";
					}
					else
					{
						if (localstate.ICEtype2 == ICE3TYPE.ICE403)
							return "BR 403";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE406)
							return "BR 406";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE411)
							return "BR 411";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE415)
							return "BR 415";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE415_2)
							return "BR 415";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE605)
							return "BR 605";
						else if (localstate.ICEtype2 == ICE3TYPE.ICE605_2)
							return "BR 605";
					}
					break;
				case 3:
					if (localstate.ICEtype1 == ICE3TYPE.ICE415_2 || localstate.ICEtype1 == ICE3TYPE.ICE605_2)
					{
						if (localstate.ICEtype2 == ICE3TYPE.ICE415)
							return "BR 415";
						else
							return "BR 605";
					}
					else
					{
						return "";
					}
			}
			return "";
		}
		private bool EineT�rAuf
		{
			get
			{
				bool val = false;

				foreach(bool b in t�r)
				{
					val = val || b;
				}

				return val;
			}
		}
		private void SetUpT�ren()
		{	
			try
			{
				int wag = CountWaggons;

				int time = Convert.ToInt32(Math.Round(1500d / (double)wag));

				int counter = wag;

				do
				{
					prov.GetBytes(random);
					int val = Convert.ToInt32(Math.Round((double)random[0] / 16d));
					if (val > 15) continue;
					if (val < 0) continue;

					if (t�r[val])
					{
						t�r[val] = false;
						counter--;
						something_changed = true;
						Thread.Sleep(time);
					}
					else
					{
						continue;
					}
				}
				while (counter > 0 && DO_T�REN);
			}
			catch(Exception) {}
		}
		public void ICE3Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
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

			if (!Inverse)
			{
				g.Clear(Misc.FILL_GRAY);
				b_ws = new SolidBrush(Color.Black);
				b_ws_alt = new SolidBrush(Color.WhiteSmoke);
				p_ws_2 = new Pen(b_ws, 1);
			}
			else
			{
				g.Clear(Misc.FILL_BLACK);
				b_ws = new SolidBrush(Color.WhiteSmoke);
				b_ws_alt = new SolidBrush(Color.Black);
				p_ws_2 = new Pen(b_ws, 1);
			}
			
			
			DrawUhr(ref g);
			DrawDatum(ref g);
			DrawZugNr(ref g);
			DrawDispaly(ref g);
			DrawSt�rung(ref g);
			if (localstate.Type == TYPE.David2)
			{
				DrawWB(ref g);
			}
			else if (localstate.Type == TYPE.David1)
			{
				DrawTfT�ren(ref g);
			}
			DrawLines(ref g);
			
			DrawSoftKeys(ref g);

			switch (localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.D2_Zustand:
					DrawD2_Zustand(ref g);
					break;
				case CURRENT_DISPLAY.D2_FspBr:
					DrawD2_FspBr(ref g);
					break;
				case CURRENT_DISPLAY.D2_Schalt:
					DrawD2_Schaltzustand(ref g);
					break;
				case CURRENT_DISPLAY.D2_HBL:
					DrawD2_HBL(ref g);
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					DrawV_EQ_0(ref g);
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					DrawV_GR_0(ref g);
					break;
				case CURRENT_DISPLAY.ST:
					DrawST(ref g);
					break;
				case CURRENT_DISPLAY.D1_Grundb:
					DrawD1_Grundbild(ref g);
					break;
				case CURRENT_DISPLAY.D1_FB:
					DrawD1_FB(ref g);
					break;
				case CURRENT_DISPLAY.D1_Abfert:
					DrawD1_Abfertigen(ref g);
					break;
				default:
					DrawNONE(ref g);
					break;
			}

			

			if (USE_DOUBLE_BUFFER)
			{
				//Copy the back buffer to the screen
				e.Graphics.DrawImageUnscaled(m_backBuffer,0,0);
			}
			Thread.Sleep(1);
		}

		#region ButtonAsignments
		private bool D2_Schalt_B1()
		{
			return (
				    (localstate.ICEtype1 == ICE3TYPE.ICE403 || localstate.ICEtype1 == ICE3TYPE.ICE406 || 
				     localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406) &&
			        (localstate.wbBremse != WBBremse.AUS && localstate.wbBremse_erlaubt != WBBremse.AUS)
			       );
		}

		private bool D2_Schalt_B2()
		{
			return (
				(localstate.ICEtype1 == ICE3TYPE.ICE403 || localstate.ICEtype1 == ICE3TYPE.ICE406 || 
				localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406) &&
				(localstate.wbBremse != WBBremse.SB && localstate.wbBremse_erlaubt != WBBremse.AUS)
				);
		}

		private bool D2_Schalt_B3()
		{
			return (
				(localstate.ICEtype1 == ICE3TYPE.ICE403 || localstate.ICEtype1 == ICE3TYPE.ICE406 || 
				localstate.ICEtype2 == ICE3TYPE.ICE403 || localstate.ICEtype2 == ICE3TYPE.ICE406) &&
				(localstate.wbBremse != WBBremse.BB_SB && localstate.wbBremse_erlaubt != WBBremse.AUS && localstate.wbBremse_erlaubt != WBBremse.SB)
				);
		}
		#endregion

		private void timer_st_Tick(object sender, System.EventArgs e)
		{
			if (localstate.st�rungmgr.Current.Type != ENUMSt�rung.NONE)
			{
				if (ST�RUNG_BG == Color.Yellow)
				{
					ST�RUNG_BG = Color.Black;
					ST�RUNG_FG = Color.Yellow;
				}
				else
				{
					ST�RUNG_BG = Color.Yellow;
					ST�RUNG_FG = Color.Black;
				}
			}
			else
			{
				ST�RUNG_BG = Color.Yellow;
				ST�RUNG_FG = Color.Black;
			}

			if (localstate.st�rungmgr.Current.Type != ENUMSt�rung.NONE || localstate.st�rungmgr.GetPassives().Count > 1)
                something_changed = true;
		}

		private void timer_schw_Tick(object sender, System.EventArgs e)
		{
			// schwankungen

			for (int i = 0; i < 12; i++)
			{
				prov.GetBytes(random);
				if (random[0] < 64)
				{
					prov.GetBytes(random);
					schw[i] = Convert.ToInt32(0f - ((float)random[0] / 100f));
				}
				else if (random[0] > 192) 
				{
					prov.GetBytes(random);
					schw[i] = Convert.ToInt32(((float)random[0] / 100f));
				}
			}

			//something_changed = true;
		
		}
	}
}
