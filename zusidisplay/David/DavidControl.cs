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


namespace MMI.DAVID
{	   
	public class DavidControl : System.Windows.Forms.UserControl
	{
		const bool USE_DOUBLE_BUFFER = true;
		const float FramesPerSecond = 20f;
		const string fixed_font = "FixedSysTTF";
		const string other_font = "Tahoma";
		Color MMI_BLUE = Color.FromArgb(59,125,220);
		Brush b_MMI = new SolidBrush(Color.FromArgb(59,125,220));
		Pen p_dg = new Pen(Brushes.WhiteSmoke, 1.5f);
		Pen p_g = new Pen(Brushes.Gray, 1.5f);
		Pen p_b = new Pen(Brushes.Black, 1);
		Color STÖRUNG_FG = Color.Blue;
		Color STÖRUNG_BG = Color.Gold;
		public bool ShowKeys = false;
		Hashtable ht = new Hashtable(), ht2 = new Hashtable();
		int StörPos = 0;
		DateTime lastTime = new DateTime(0);

		SoundInterface sound;
		int stoerung_counter = 0;


		#region Declarations

		private float[] c_druck = new float[17];

		byte[] random = new byte[1];
		System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();


		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		private Bitmap m_backBuffer;
		private Graphics g;

		public DavidState localstate;
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

		bool on = true;

		#endregion
		public DavidControl(MMI.EBuLa.Tools.XMLLoader conf, ControlContainer cc)
		{
			InitializeComponent();

			m_conf = conf;
			this.cc = cc;

			localstate = new DavidState();
			
			localstate.ICEtype = ICETYPE.ICE1;
			localstate.Type = TYPE.David2;
			localstate.DISPLAY = CURRENT_DISPLAY.D2_B;
			olddisplay = CURRENT_DISPLAY.D1_I;

			//SetButtons();
			vtime = DateTime.Now;

			InitC_druck();

			int interval = Convert.ToInt32(Math.Round((1d/(double)FramesPerSecond)*1000d));
			timer1.Interval = interval;
			timer1.Enabled = true;

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
			Button_SW_Pressed(this, new EventArgs());			
		}

		protected override void Dispose( bool disposing )
		{
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
			// 
			// timer1
			// 
			this.timer1.Interval = 30;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timer_st
			// 
			this.timer_st.Enabled = true;
			this.timer_st.Interval = 1000;
			this.timer_st.Tick += new System.EventHandler(this.timer_st_Tick);
			// 
			// DavidControl
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.Name = "DavidControl";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DavidControl_Paint);

		}
		#endregion
		public string Version()
		{
			return Application.ProductVersion;
		}

		public void Inverse()
		{
		}

		public bool IsDavid1
		{
			get{return localstate.Type == TYPE.David1;}
		}


		#region Buttons
		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.D1_I)
			{
				if (localstate.maxOberstrom == 600)
				{
					localstate.maxOberstrom = 900;
					something_changed = true;
				}
			}
		}

		public void Button_Down_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.D1_I)
			{
				if (localstate.maxOberstrom == 900)
				{
					localstate.maxOberstrom = 600;
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

		}

		public void Button_Brightness_Pressed(object sender, System.EventArgs e)
		{

		}
 
		public void Button_Off_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_1_Pressed(object sender, System.EventArgs e)
		{
		}

		public void Button_2_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.NONE;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_W;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_3_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_BRH;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.NONE;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_4_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_D;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_LZB;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_5_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Br;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_Ub;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_6_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Fbr;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_A;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_7_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_Fdyn;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_F;
				
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_8_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_B;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_I;
				
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_9_Pressed(object sender, System.EventArgs e)
		{
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_HBL;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_U;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_0_Pressed(object sender, System.EventArgs e)
		{	
			if (localstate.Type == TYPE.David2)
				localstate.DISPLAY = CURRENT_DISPLAY.D2_HL;
			else
				localstate.DISPLAY = CURRENT_DISPLAY.D1_G;
				
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
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_V_GR_0_Pressed(object sender, System.EventArgs e)
		{
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
						if (localstate.Type == TYPE.David1)
						{
							localstate.DISPLAY = CURRENT_DISPLAY.D1_U;
						}
						else if (localstate.Type == TYPE.David2)
						{
							localstate.DISPLAY = CURRENT_DISPLAY.D2_B;
						}
					}
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
									sound.PlayMalfunctionBombardierSound();
									sound.PlayMalfunctionBombardierSound();
									sound.PlayMalfunctionBombardierSound();
									break;
								case 2:
									sound.PlayMalfunctionBombardierSound();
									sound.PlayMalfunctionBombardierSound();
									sound.PlayMalfunctionBombardierSound();
									break;
							}
						}
					}
				}
				CONNECTED = value;
				something_changed = true;
			}
		}
		public void DrawLines(ref Graphics pg)
		{
			Pen p_dg_3 = new Pen(Brushes.DarkGray, 3);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 3);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			pg.SmoothingMode = SmoothingMode.None;

			pg.DrawLine(p_ws_3, 1, 58, this.Width-2, 58);
			pg.DrawLine(p_ws_3, 1, 58,            1, 58+328);

			pg.DrawLine(p_dg_3, this.Width-2, 58,     this.Width-2, 58+328+1);
			pg.DrawLine(p_dg_3,            1, 58+328, this.Width-2, 58+328);

			pg.DrawRectangle(p_ws_1, 6, 63, this.Width-12, 328-9);

			pg.DrawLine(p_dg_1, this.Width-6, 58+6,     this.Width-6, 58+328-4);
			pg.DrawLine(p_dg_1,            6, 58+328-4, this.Width-6, 58+328-4);

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
			//timer1.Stop();
			UpdateScreen();
			//timer1.Start();
		}

		private void InitC_druck()
		{
			int max = int.MaxValue;

			if (localstate.ICEtype == ICETYPE.ICE1)
			{
				max = 13;
			}
			else if (localstate.ICEtype == ICETYPE.ICE2_DT)
			{
				max = 16;
			}

			for (int i = 0; i < c_druck.Length; i++)
			{
				prov.GetBytes(random);
				if (random[0] > 128 || i == 0 || i >= max)
				{
					prov.GetBytes(random);
					if (i == 0 || i >= max)
					{
						c_druck[i] = 0f - (512f / 12800f);
					}
					else
					{
						c_druck[i] = 0f - ((float)random[0] / 12800f);
					}
				}
				else
				{
					prov.GetBytes(random);
					c_druck[i] = ((float)random[0] / 12800f);
				}
			}
		}

		public bool addtionalhours
		{
			get{return localstate.addtionalhours;}
			set{localstate.addtionalhours = value;}
		}

		#region SET

		public void SetFahrstufenSchalter(float valu)
		{
			localstate.FahrstufenSchalter = Convert.ToInt32(valu);
			something_changed = true;
		}
		public void SetEBremse(float valu)
		{
			localstate.E_Bremse = Convert.ToInt32(valu);
			something_changed = true;
		}
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
		}

		public void SetLM_LZB_Ü(bool state)
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
		public void SetLM_Tür(bool state)
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
		public void SetBremsstellung(float valu)
		{
		}
		public void SetAFB_LZB_SollGeschwindigkeit(float valu)
		{
		}
		public void SetAFB_Sollgeschwindigkeit(float valu)
		{
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
		public void SetGeschwindigkeit(float valu)
		{
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

		public void SetFahrstufen(float valu)
		{
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
			cc.SetUhrzeitEbula(valu);
			something_changed = true;
		}
		#endregion

		public void UpdateScreen()
		{
			if (!something_changed)
				return;

			something_changed = false;

			DavidControl_Paint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0,0,this.Width, this.Height)));

		}


		private void DrawD1_G(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawStörung(ref pg);
			DrawU(ref pg, 1);
		}
		private void DrawD1_U(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawStörung(ref pg);
			DrawU(ref pg, 1);
			if (localstate.ICEtype != ICETYPE.ICE2_ET)
				DrawU(ref pg, 2);
		}
		private void DrawD1_I(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawStörung(ref pg);
			DrawU(ref pg, 1);
			DrawI(ref pg);
		}
		private void DrawD1_F(ref Graphics pg)
		{
			DrawAFB(ref pg);
			DrawStörung(ref pg);
			DrawU(ref pg, 1);
			DrawF(ref pg, 1);
			if (localstate.ICEtype == ICETYPE.ICE1 || localstate.ICEtype == ICETYPE.ICE2_DT) DrawF(ref pg, 2);
		}
		private void DrawD2_B(ref Graphics pg)
		{
			DrawStörung(ref pg);
			DrawHL_Druck(ref pg);
			DrawHBL_Druck(ref pg);
			DrawC_Druck(ref pg);
		}

		private void DrawD2_Fdyn(ref Graphics pg)
		{
			DrawStörung(ref pg);
			DrawHL_Druck(ref pg);
			DrawFdynTK(ref pg, 1);
			if (localstate.ICEtype == ICETYPE.ICE1 || localstate.ICEtype == ICETYPE.ICE2_DT ) DrawFdynTK(ref pg, 2);
		}
		private void DrawD2_HL(ref Graphics pg)
		{
			DrawStörung(ref pg);
			DrawHL_Druck(ref pg);
		}
		private void DrawD2_HBL(ref Graphics pg)
		{
			DrawStörung(ref pg);
			DrawHL_Druck(ref pg);
			if (localstate.ICEtype == ICETYPE.ICE2_ET)
				DrawHBL_vollstd_Druck(ref pg, 1);
			else
				DrawHBL_vollstd_Druck(ref pg, 2);
		}

		private void DrawV_EQ_0(ref Graphics pg)
		{
			DrawDatum(ref pg);
			DrawUhr(ref pg);

			if (localstate.störungmgr.StörStack != null)
			{
				switch(localstate.störungmgr.StörStack.Type)
				{
					case ENUMStörung.S01_ZUSIKomm:
						DrawS01(ref pg, false);
						break;
					case ENUMStörung.S02_Trennschütz:
						DrawS02(ref pg, false);
						break;
				}
			}
		}
		private void DrawV_GR_0(ref Graphics pg)
		{
			DrawDatum(ref pg);
			DrawUhr(ref pg);

			if (localstate.störungmgr.StörStack != null)
			{
				switch(localstate.störungmgr.StörStack.Type)
				{
					case ENUMStörung.S01_ZUSIKomm:
						DrawS01(ref pg, true);
						break;
					case ENUMStörung.S02_Trennschütz:
						DrawS02(ref pg, true);
						break;
				}
			}
		}
		private void DrawST(ref Graphics pg)
		{
			DrawDatum(ref pg);
			DrawUhr(ref pg);


			bool add = true;
			int y = 40;
			if (ht.Count > 0) add = false;
			Font f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);

			pg.DrawString("Störungsübersicht                 Seite 01", f, Brushes.Black, 245,23);

			int counter = -1; int pos = 0;
			if (localstate.störungmgr.getStörungsCount < 1)
			{
				pg.DrawString("Keine Störung gemeldet!", f, Brushes.Black, 50,60+y);
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
						pg.FillRectangle(Brushes.Blue, 20, 40+20*counter+y, 500, 18);
						pg.DrawString(st.Priority.ToString(), f, Brushes.WhiteSmoke, 20, 40+20*counter+y);
						pg.DrawString(st.Name, f, Brushes.WhiteSmoke, 50, 40+20*counter+y);
						pg.DrawString(st.Description, f, Brushes.WhiteSmoke, 140, 40+20*counter+y);
					}
					else
					{
						pg.DrawString(st.Priority.ToString(), f, Brushes.Black, 20, 40+20*counter+y);
						pg.DrawString(st.Name, f, Brushes.Black, 50, 40+20*counter+y);
						pg.DrawString(st.Description, f, Brushes.Black, 140, 40+20*counter+y);
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
						pg.FillRectangle(Brushes.Blue, 20, 40+20*counter+y, 500, 18);
						pg.DrawString("*"+st.Priority.ToString(), f, Brushes.WhiteSmoke, 20, 40+20*counter+y);
						pg.DrawString(st.Name, f, Brushes.WhiteSmoke, 50, 40+20*counter+y);
						pg.DrawString(st.Description, f, Brushes.WhiteSmoke, 140, 40+20*counter+y);
					}
					else
					{
						pg.DrawString("*"+st.Priority.ToString(), f, Brushes.Black, 20, 40+20*counter+y);
						pg.DrawString(st.Name, f, Brushes.Black, 50, 40+20*counter+y);
						pg.DrawString(st.Description, f, Brushes.Black, 140, 40+20*counter+y);
					}
					pos++;
				}

			}
		}

		private void DrawKasten(ref Graphics pg, int x, int y, bool hasIndicator, float height)
		{

			if (hasIndicator)
			{
				// Füllung Kasten links 
				if (height > 0.1f)
					pg.FillRectangle(b_MMI, x, 330, 55, 15);

				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b_MMI, x, 320-Iheight, 55, Iheight);

				// Großer Kasten
				pg.DrawLine(p_dg,    x, 320, x+55, 320);
				pg.DrawLine(p_dg, x+55, 100, x+55, 320);

				pg.DrawLine(p_g, x, 100, x+55, 100);
				pg.DrawLine(p_g, x, 100,    x, 320);

				// Kleiner Kasten
				pg.DrawLine(p_g,     x, 330, x+55, 330);
				pg.DrawLine(p_dg, x+55, 330, x+55, 345);

				pg.DrawLine(p_dg, x, 345, x+55, 345);
				pg.DrawLine(p_g,  x, 330,    x, 345);
			}
			else
			{
				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b_MMI, x, 345-Iheight, 55, Iheight);

				// Großer Kasten
				pg.DrawLine(p_dg,    x, 345, x+55, 345);
				pg.DrawLine(p_dg, x+55, 100, x+55, 345);

				pg.DrawLine(p_g, x, 100, x+55, 100);
				pg.DrawLine(p_g, x, 100,    x, 345);
			}
		}
		private void DrawKastenSmall(ref Graphics pg, int x, int y, bool hasIndicator, bool hasSpacer, float height, bool small)
		{

			if (hasIndicator)
			{
				
				// Füllung Kasten 
				pg.FillRectangle(b_MMI, x, 48+y, 14, 4);

				int Iheight = Convert.ToInt32(Math.Round(height));

				if (!small)
					pg.FillRectangle(b_MMI, x, 46+y-Iheight, 14, Iheight);

				if (hasSpacer) 
				{
					pg.DrawLine(p_dg, x+6, y, x+6, y+46);
					pg.DrawLine(p_g, x+8, y, x+8, y+46);

					pg.DrawLine(p_dg, x+6, y+48, x+6, y+52);
					pg.DrawLine(p_g, x+8, y+48, x+8, y+52);
				}

				if (!small)
				{
					// Großer Kasten 46x12
					pg.DrawLine(p_dg,    x, 46+y, 14+x, 46+y);
					pg.DrawLine(p_dg, 14+x,    y, 14+x, 46+y);

					pg.DrawLine(p_g, x, y, 14+x,    y);
					pg.DrawLine(p_g, x, y,    x, 46+y);
				}

				// Kleiner Kasten 4x12
				pg.DrawLine(p_g,     x, 48+y, 14+x, 48+y);
				pg.DrawLine(p_dg, 14+x, 48+y, 14+x, 52+y);

				pg.DrawLine(p_dg, x, 52+y, 14+x, 52+y);
				pg.DrawLine(p_g,  x, 48+y,    x, 52+y);
			}
			else
			{				  //52x12
				int Iheight = Convert.ToInt32(Math.Round(height));

				pg.FillRectangle(b_MMI, x, 52+y-Iheight, 14, Iheight);

				if (hasSpacer)
				{
					pg.DrawLine(p_dg, x+6, y, x+6, y+52);
					pg.DrawLine(p_g, x+8, y, x+8, y+52);
				}

				// Großer Kasten
				pg.DrawLine(p_dg,    x, 52+y, x+14, 52+y);
				pg.DrawLine(p_dg, 14+x,    y, x+14, 52+y);

				pg.DrawLine(p_g, x, y, 14+x,    y);
				pg.DrawLine(p_g, x, y,    x, 52+y);
			}
		}

		private void DrawU(ref Graphics pg, byte tk)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawLine(p_b, 35+175*(tk-1), 337, 45+175*(tk-1), 337); // 0
			pg.DrawLine(p_b, 40+175*(tk-1), 316, 45+175*(tk-1), 316); // 9
			pg.DrawLine(p_b, 35+175*(tk-1), 297, 45+175*(tk-1), 297); // 10
			pg.DrawLine(p_b, 40+175*(tk-1), 278, 45+175*(tk-1), 278); // 11
			pg.DrawLine(p_b, 40+175*(tk-1), 259, 45+175*(tk-1), 259); // 12
			pg.DrawLine(p_b, 40+175*(tk-1), 240, 45+175*(tk-1), 240); // 13
			pg.DrawLine(p_b, 40+175*(tk-1), 222, 45+175*(tk-1), 222); // 14
			pg.DrawLine(p_b, 35+175*(tk-1), 203, 45+175*(tk-1), 203); // 15
			pg.DrawLine(p_b, 40+175*(tk-1), 184, 45+175*(tk-1), 184); // 16
			pg.DrawLine(p_b, 40+175*(tk-1), 165, 45+175*(tk-1), 165); // 17
			pg.DrawLine(p_b, 40+175*(tk-1), 146, 45+175*(tk-1), 146); // 18
			pg.DrawLine(p_b, 40+175*(tk-1), 127, 45+175*(tk-1), 127); // 19
			pg.DrawLine(p_b, 35+175*(tk-1), 108, 45+175*(tk-1), 108); // 20

			pg.DrawString("0", f, Brushes.Black, 20f+175*(tk-1), 329f);
			pg.DrawString("10", f, Brushes.Black, 12f+175*(tk-1), 289f);
			pg.DrawString("15", f, Brushes.Black, 12f+175*(tk-1), 195f);
			pg.DrawString("20", f, Brushes.Black, 12f+175*(tk-1), 100f);

			float height = 0f;

			// Füllung Kasten
			if (localstate.Spannung > 0.1f)
				height = 0.5f;
				
			if (localstate.Spannung > 8.8f)	   // diff: 11,4 == 320
			{
				if (localstate.HL_Druck > 20.2f) localstate.HL_Druck = 20.2f;
				float hl = localstate.Spannung - 8.8f;
				height = Convert.ToSingle((double)hl * 19.0d);
			}

			DrawKasten(ref pg, 45+175*(tk-1), 100, true, height);

			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("kV", f, Brushes.Black, 16f+175*(tk-1), 70f);
			pg.DrawString("U", f, Brushes.Black, 52f+175*(tk-1), 354f);
			pg.DrawString("TK"+tk.ToString(), f, Brushes.Black, 60f+175*(tk-1), 358f);

			pg.SmoothingMode = SMOOTHING_MODE;

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

			if (localstate.ICEtype == ICETYPE.ICE2_ET) twice = 1d;

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

		private void DrawF(ref Graphics pg, byte tk)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawLine(p_b, 220+(tk-1)*170, 344, 210+(tk-1)*170, 344); // 0
			pg.DrawLine(p_b, 220+(tk-1)*170, 289, 210+(tk-1)*170, 289); // 50
			pg.DrawLine(p_b, 220+(tk-1)*170, 233, 210+(tk-1)*170, 233); // 100
			pg.DrawLine(p_b, 220+(tk-1)*170, 178, 210+(tk-1)*170, 178); // 150
			pg.DrawLine(p_b, 220+(tk-1)*170, 125, 210+(tk-1)*170, 125); // 200
			
			pg.DrawString("0",   f, Brushes.Black, 196+(tk-1)*170, 336f);
			pg.DrawString("50",  f, Brushes.Black, 188+(tk-1)*170, 282f);
			pg.DrawString("100", f, Brushes.Black, 178+(tk-1)*170, 225f);
			pg.DrawString("150", f, Brushes.Black, 178+(tk-1)*170, 170f);
			pg.DrawString("200", f, Brushes.Black, 178+(tk-1)*170, 115f);

			float height = 0f, height2 = 0f;

			if (localstate.ZugkraftGesammt > 0f)
			{
				height = Convert.ToSingle((double)localstate.ZugkraftGesammt * 1.105d);
				if (height > 244f) height = 243f;

				height2 = localstate.FahrstufenSchalter * 5.560f;
			}

			DrawKasten(ref pg, 220+(tk-1)*170, 100, false, height2);
			DrawKasten(ref pg, 278+(tk-1)*170, 100, false, height);

			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("kN", f, Brushes.Black, 188f+(tk-1)*170, 70f);
			pg.DrawString("Soll", f, Brushes.Black, 229f+(tk-1)*170, 70f);
			pg.DrawString("Ist", f, Brushes.Black, 290f+(tk-1)*170, 70f);
			pg.DrawString("F", f, Brushes.Black, 259f+(tk-1)*170, 354f);
			pg.DrawString("TK"+tk.ToString(), f, Brushes.Black, 266f+(tk-1)*170, 358f);

			pg.SmoothingMode = SMOOTHING_MODE;

		}

		private void DrawHL_Druck(ref Graphics pg)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala links
			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawLine(p_b, 35, 337, 45, 337); // 0
			pg.DrawLine(p_b, 40, 314, 45, 314); // 2,5
			pg.DrawLine(p_b, 35, 286, 45, 286); // 3
			pg.DrawLine(p_b, 40, 260, 45, 260); // 3,5
			pg.DrawLine(p_b, 35, 233, 45, 233); // 4
			pg.DrawLine(p_b, 40, 205, 45, 205); // 4,5
			pg.DrawLine(p_b, 35, 178, 45, 178); // 5
			pg.DrawLine(p_b, 40, 150, 45, 150); // 5,5
			pg.DrawLine(p_b, 35, 123, 45, 123); // 6

			pg.DrawString("0", f, Brushes.Black, 21f, 329f);
			pg.DrawString("3", f, Brushes.Black, 21f, 278f);
			pg.DrawString("4", f, Brushes.Black, 21f, 225f);
			pg.DrawString("5", f, Brushes.Black, 21f, 170f);
			pg.DrawString("6", f, Brushes.Black, 21f, 115f);

			float height = 0f;

			// Füllung Kasten links 
			if (localstate.HL_Druck > 0.1f)
				height = 0.5f;
				
			if (localstate.HL_Druck > 2.4f)
			{
				if (localstate.HL_Druck > 6.4f) localstate.HL_Druck = 6.4f;
				float hl = localstate.HL_Druck - 2.4f;
				height = Convert.ToSingle((double)hl * 55d);
			}

			DrawKasten(ref pg, 45, 100, true, height);

			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("bar", f, Brushes.Black, 21f, 70f);
			pg.DrawString("HL-Druck", f, Brushes.Black, 35f, 354f);

			pg.SmoothingMode = SMOOTHING_MODE;
		}

		private void DrawHBL_Druck(ref Graphics pg)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala rechts
			pg.DrawLine(p_b, 545, 337, 555, 337); // 0
			pg.DrawLine(p_b, 550, 314, 555, 314); // 3
			pg.DrawLine(p_b, 545, 286, 555, 286); // 4
			pg.DrawLine(p_b, 550, 260, 555, 260); // 5
			pg.DrawLine(p_b, 545, 233, 555, 233); // 6
			pg.DrawLine(p_b, 550, 205, 555, 205); // 7
			pg.DrawLine(p_b, 545, 178, 555, 178); // 8
			pg.DrawLine(p_b, 550, 150, 555, 150); // 9
			pg.DrawLine(p_b, 545, 123, 555, 123); // 10

			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("0", f, Brushes.Black, 525f, 329f);
			pg.DrawString("4", f, Brushes.Black, 525f, 278f);
			pg.DrawString("6", f, Brushes.Black, 525f, 225f);
			pg.DrawString("8", f, Brushes.Black, 525f, 170f);
			pg.DrawString("10", f, Brushes.Black, 521f, 115f);

			float height = 0f;
			// Füllung Kasten rechts
			if (localstate.HBL_Druck > 0.1f)
				height = 0.5f;

			if (localstate.HBL_Druck > 2.8f)
			{
				if (localstate.HBL_Druck > 10.8f) localstate.HBL_Druck = 10.8f;
				float hl = localstate.HBL_Druck - 2.8f;
				height = Convert.ToSingle(Math.Round((double)hl * 27.5d));
			}

			DrawKasten(ref pg, 555, 100, true, height);
			
			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("bar", f, Brushes.Black, 520f, 70f);
			pg.DrawString("HBL-Druck", f, Brushes.Black, 533f, 354f);

			pg.SmoothingMode = SMOOTHING_MODE;


		}

		private void DrawHBL_vollstd_Druck(ref Graphics pg, byte tk)
		{
			Brush b_MMI = new SolidBrush(MMI_BLUE);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);
			Pen p_g = new Pen(Brushes.Gray, 2);
			Pen p_b = new Pen(Brushes.Black, 1);

			pg.SmoothingMode = SmoothingMode.None;

			// Skala rechts
			pg.DrawLine(p_b, 210, 337, 220, 337); // 0
			pg.DrawLine(p_b, 215, 314, 220, 314); // 3
			pg.DrawLine(p_b, 210, 286, 220, 286); // 4
			pg.DrawLine(p_b, 215, 260, 220, 260); // 5
			pg.DrawLine(p_b, 210, 233, 220, 233); // 6
			pg.DrawLine(p_b, 215, 205, 220, 205); // 7
			pg.DrawLine(p_b, 210, 178, 220, 178); // 8
			pg.DrawLine(p_b, 215, 150, 220, 150); // 9
			pg.DrawLine(p_b, 210, 123, 220, 123); // 10

			Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("0", f, Brushes.Black, 190f, 329f);
			pg.DrawString("4", f, Brushes.Black, 190f, 278f);
			pg.DrawString("6", f, Brushes.Black, 190f, 225f);
			pg.DrawString("8", f, Brushes.Black, 190f, 170f);
			pg.DrawString("10", f, Brushes.Black, 186f, 115f);

			float height = 0f;
			// Füllung Kasten rechts
			if (localstate.HBL_Druck > 0.1f)
				height = 0.5f;

			if (localstate.HBL_Druck > 2.8f)
			{
				if (localstate.HBL_Druck > 10.8f) localstate.HBL_Druck = 10.8f;
				float hl = localstate.HBL_Druck - 2.8f;
				height = Convert.ToSingle(Math.Round((double)hl * 27.5d));
			}

			DrawKasten(ref pg, 220, 100, true, height);
			if (tk > 1) DrawKasten(ref pg, 278, 100, true, height);//+58
			
			f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("bar", f, Brushes.Black, 185f, 70f);
			pg.DrawString("TK1", f, Brushes.Black, 232f, 70f);
			if (tk == 1)
				pg.DrawString("HBL-Druck", f, Brushes.Black, 200f, 354f);
			else
			{
				pg.DrawString("HBL-Druck", f, Brushes.Black, 236f, 354f);
				pg.DrawString("TK2", f, Brushes.Black, 288f, 70f);
			}

			pg.SmoothingMode = SMOOTHING_MODE;


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
		private void DrawC_Druck(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			int target = 14;
			if (localstate.ICEtype == ICETYPE.ICE2_ET)
				target = 8;
			if (localstate.ICEtype == ICETYPE.ICE2_DT)
				target = 17;

			int height = 0;
			if (localstate.LM_MG) 
				height = 46;
			else 
				height = 0;

			// Mg Anzeige
			for(int i = 0; i < target; i++)
			{
				if (i == 0 || (i == target-1 && localstate.ICEtype != ICETYPE.ICE2_ET) )
				{
					int oldh = height;
					if (localstate.ZugkraftGesammt < 0f)
						height = 46;
					else
						height = 0;

					DrawKastenSmall(ref pg, 126+i*22, 150, true, true, height,false);
					height = oldh;
				}
				else if (i == Math.Floor(target/2) && localstate.ICEtype == ICETYPE.ICE2_DT)
				{
					DrawKastenSmall(ref pg, 126+i*22, 150, true, false, height, true);
				}
				else
					DrawKastenSmall(ref pg, 126+i*22, 150, true, false, height, false);
			}
				

			// Bremszylinderanzeige
			for(int i = 0; i < target; i++)
			{
				height = Convert.ToInt32(Math.Round(((c_druck[i] + localstate.C_Druck - 0.02f) * 5000), 0));

				if (height < 0) height = 0;
				if (height > 52) height = 52;

				
				if (c_druck[i] + localstate.C_Druck > 0.02f) 
				{
				//	height = 52;
				}
				else 
					height = 0;
				

				if (i == 0 || (i == target-1 && localstate.ICEtype != ICETYPE.ICE2_ET) )
					DrawKastenSmall(ref pg, 126+i*22, 240, false, true, height, false);
				else if (i == Math.Floor(target/2) && localstate.ICEtype == ICETYPE.ICE2_DT)
				{
					DrawKastenSmall(ref pg, 126+i*22, 240, true, false, height, true);
				}
				else
					DrawKastenSmall(ref pg, 126+i*22, 240, false, false, height, false);
			}

			// TEXT
			Font f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("Fdyn", f, Brushes.Black, 114, 205);
			pg.DrawString("Tk1", f, Brushes.Black, 114, 295);

			if (localstate.ICEtype == ICETYPE.ICE1)
			{
				pg.DrawString("Fdyn", f, Brushes.Black, 404, 205);
				pg.DrawString("Tk2", f, Brushes.Black, 407, 295);
				pg.DrawString("Mg-Bremse", f, Brushes.Black, 230, 205);
				pg.DrawString("C-Druck", f, Brushes.Black, 236, 320);
				
				int i = 1; int counter = 1;
				while(counter < target+1)
				{
					if (counter == 7 || counter == 8)
					{
						counter++;
						continue;
					}
					string s = counter.ToString();
					if (s.Length < 2) s = "0"+s;
					pg.DrawString(s, f, Brushes.Black, 122 + i * 22, 295);
					counter++; i++;
				}
			}
			else if (localstate.ICEtype == ICETYPE.ICE2_DT)
			{
				pg.DrawString("Fdyn", f, Brushes.Black, 471, 205);
				pg.DrawString("Tk2", f, Brushes.Black, 471, 295);
				pg.DrawString("Mg-Bremse", f, Brushes.Black, 272, 205);
				pg.DrawString("C-Druck", f, Brushes.Black, 280, 320);

				for(int i = 1; i < 8; i++)
				{
					pg.DrawString((28-i).ToString(), f, Brushes.Black, 122 + i * 22, 295);
				}

				for(int i = 9; i < target-1; i++)
				{
					pg.DrawString((22+i).ToString(), f, Brushes.Black, 122 + i * 22, 295);
				}

			}
			else if (localstate.ICEtype == ICETYPE.ICE2_ET)
			{
				pg.DrawString("Mg-Bremse", f, Brushes.Black, 166, 205);
				pg.DrawString("C-Druck", f, Brushes.Black, 172, 320);

				for(int i = 1; i < target; i++)
				{
					pg.DrawString((28-i).ToString(), f, Brushes.Black, 122 + i * 22, 295);
				}
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		private void DrawUhr(ref Graphics pg)
		{
			Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			pg.SmoothingMode = SmoothingMode.None;

			if (IsNotStörDisplay)
			{
				pg.DrawLine(p_ws_3, 488-1-7, 8, 490+this.Width-486-6, 8);
				pg.DrawLine(p_ws_3, 488-7,   8, 488-7,                8+44);

				pg.DrawLine(p_dg_3, 488-1-7,               8+44, 490+this.Width-486-6, 8+44);
				pg.DrawLine(p_dg_3, 490+this.Width-486-6, 8+44+1, 490+this.Width-486-6, 8-1);
			
				pg.DrawRectangle(p_ws_1, 488+3-7, 8+3, this.Width-486-4, 44-7);
			
				pg.DrawLine(p_dg_1, 488-4, 44+4, 488+3+this.Width-486-11, 44+4);
				pg.DrawLine(p_dg_1, 488+3+this.Width-486-11, 8+3, 488+3+this.Width-486-11, 44+4);

				Font f = new Font(fixed_font, 23, FontStyle.Regular, GraphicsUnit.Point);
				string s = "";
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
			
				pg.DrawString(s, f, Brushes.Black, 480, 15);
			}
			else
			{
				/*pg.DrawLine(p_ws_3, 488-1-7, 8, 490+this.Width-486-6, 8);
				pg.DrawLine(p_ws_3, 488-7,   8, 488-7,                8+44);

				pg.DrawLine(p_dg_3, 488-1-7,               8+44, 490+this.Width-486-6, 8+44);
				pg.DrawLine(p_dg_3, 490+this.Width-486-6, 8+44+1, 490+this.Width-486-6, 8-1);
			
				pg.DrawRectangle(p_ws_1, 488+3-7, 8+3, this.Width-486-4, 44-7);
			
				pg.DrawLine(p_dg_1, 488-4, 44+4, 488+3+this.Width-486-11, 44+4);
				pg.DrawLine(p_dg_1, 488+3+this.Width-486-11, 8+3, 488+3+this.Width-486-11, 44+4);*/
			}
		}
		private void DrawDatum(ref Graphics pg)
		{
			Pen p_dg = new Pen(Brushes.DarkGray, 2);

			pg.SmoothingMode = SmoothingMode.None;

			Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			if (IsNotStörDisplay)
			{
				pg.DrawLine(p_ws_3, 340-1, 8, 340+this.Width-490-4, 8);
				pg.DrawLine(p_ws_3, 340,   8, 340,      8+44);

				pg.DrawLine(p_dg_3, 340-1 ,               8+44,   340+this.Width-490-4, 8+44);
				pg.DrawLine(p_dg_3, 340+this.Width-490-4, 8+44+1, 340+this.Width-490-4, 8-1);
			
				pg.DrawRectangle(p_ws_1, 340+3, 8+3, this.Width-490-4-7, 44-7);
			
				pg.DrawLine(p_dg_1, 340+3, 44+4, 340+this.Width-490-8, 44+4);
				pg.DrawLine(p_dg_1, 340+this.Width-490-4-4, 8+3, 340+this.Width-490-4-4, 44+4);

				Font f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);
				string s = GetDayOfWeek(vtime.DayOfWeek.ToString())+ ", " + vtime.ToShortDateString();
			
				pg.DrawString(s, f, Brushes.Black, 342, 22);
			}
			else
			{
				pg.DrawLine(p_ws_3, 140-1, 8, 140+350+4, 8);
				pg.DrawLine(p_ws_3, 140,   8,    140,      8+44);

				pg.DrawLine(p_dg_3, 140-1 ,    8+44, 140+350+4, 8+44);
				pg.DrawLine(p_dg_3, 140+350+4, 8+44, 140+350+4, 8-1);
			
				pg.DrawRectangle(p_ws_1, 140+3, 8+3, 350-3, 44-7);
			
				pg.DrawLine(p_dg_1, 140+3, 44+4, 140+350, 44+4);
				pg.DrawLine(p_dg_1, 140+350, 8+3, 140+350, 44+4);
			}

		}
		private void DrawAFB(ref Graphics pg)
		{
			if (localstate.LM_AFB && !ShowKeys)
			{
				Pen p_dg = new Pen(Brushes.DarkGray, 2);

				Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
				Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
				Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
				Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

				pg.FillRectangle(Brushes.Gold, 409, 397, 61, 49);

				pg.DrawLine(p_ws_3, 405-1, 393, 405+70-2, 393);
				pg.DrawLine(p_ws_3, 405,   393, 405,      393+57);

				pg.DrawLine(p_dg_3, 405-1 ,               393+57,   405+70-2, 393+57);
				pg.DrawLine(p_dg_3, 405+70-2, 393+57+1, 405+70-2, 393-1);
			
				pg.DrawRectangle(p_ws_1, 405+3, 393+3, 70-2-7, 57-7);
			
				pg.DrawLine(p_dg_1, 405+3,                  393+57-4, 405+70-2-4, 393+57-4);
				pg.DrawLine(p_dg_1, 405+70-2-4, 393+3, 405+70-2-4, 393+57-4);

				Font f = new Font(other_font, 13, FontStyle.Regular, GraphicsUnit.Point);
				pg.DrawString("AFB", f, Brushes.Blue, 423, 410);

				pg.SmoothingMode = SMOOTHING_MODE;
			}
		}
		private void DrawStörung(ref Graphics pg)
		{
			string s = "";
			Pen p_dg_3 = new Pen(Brushes.DarkGray, 2);
			Pen p_dg_1 = new Pen(Brushes.DarkGray, 1);
			Pen p_ws_3 = new Pen(Brushes.WhiteSmoke, 2);
			Pen p_ws_1 = new Pen(Brushes.WhiteSmoke, 1);

			if (!ShowKeys && localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{

				pg.SmoothingMode = SmoothingMode.None;

				if (STÖRUNG_BG == Color.Gold)
				{
					pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 485, 397, this.Width-481-10, 49);
				}

				pg.DrawLine(p_ws_3, 481-1, 393, 481+this.Width-481-2, 393);
				pg.DrawLine(p_ws_3, 481,   393, 481,      393+57);

				pg.DrawLine(p_dg_3, 481-1 ,               393+57,   481+this.Width-481-2, 393+57);
				pg.DrawLine(p_dg_3, 481+this.Width-481-2, 393+57+1, 481+this.Width-481-2, 393-1);
		
				pg.DrawRectangle(p_ws_1, 481+3, 393+3, this.Width-481-2-7, 57-7);
		
				pg.DrawLine(p_dg_1, 481+3,                  393+57-4, 481+this.Width-481-2-4, 393+57-4);
				pg.DrawLine(p_dg_1, 481+this.Width-481-2-4, 393+3, 481+this.Width-481-2-4, 393+57-4);

				Störung st = localstate.störungmgr.Current;

				s = "St. in "+st.Name;

			
			}
			else if (localstate.störungmgr.GetPassives().Count > 1)
			{
				pg.SmoothingMode = SmoothingMode.None;

				if (STÖRUNG_BG == Color.Gold)
				{
					pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 485, 397, this.Width-481-10, 49);
				}

				pg.DrawLine(p_ws_3, 481-1, 393, 481+this.Width-481-2, 393);
				pg.DrawLine(p_ws_3, 481,   393, 481,      393+57);

				pg.DrawLine(p_dg_3, 481-1 ,               393+57,   481+this.Width-481-2, 393+57);
				pg.DrawLine(p_dg_3, 481+this.Width-481-2, 393+57+1, 481+this.Width-481-2, 393-1);
		
				pg.DrawRectangle(p_ws_1, 481+3, 393+3, this.Width-481-2-7, 57-7);
		
				pg.DrawLine(p_dg_1, 481+3,                  393+57-4, 481+this.Width-481-2-4, 393+57-4);
				pg.DrawLine(p_dg_1, 481+this.Width-481-2-4, 393+3, 481+this.Width-481-2-4, 393+57-4);

				Störung st = localstate.störungmgr.Current;

				s = "         St";
			}

			Font f = new Font(other_font, 13, FontStyle.Regular, GraphicsUnit.Point);
			if (STÖRUNG_BG != Color.Gold)
                pg.DrawString(s, f, new SolidBrush(Color.Black), 490, 410);
			else
				pg.DrawString(s, f, new SolidBrush(STÖRUNG_FG), 490, 410);

			pg.SmoothingMode = SMOOTHING_MODE;

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
			if (localstate.Type == TYPE.David2)	DrawFPL(ref pg);

			Font f = new Font(other_font, 42, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString("Störung", f, Brushes.Black, 210, 200);

		}
		private void DrawS01(ref Graphics pg, bool greater)
		{
			Font f = new Font(fixed_font, 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "", ip = "";
			IPHostEntry ipep = Dns.GetHostByName(Dns.GetHostName());

			Störung st = localstate.störungmgr.LastStörung;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";

			int y = 40;

			pg.DrawString(s, f, Brushes.Black, 20, 23);
			pg.DrawString(st.Description, f, Brushes.Black, 190, 23);

			//f = new Font(fixed_font, 12, FontStyle.Regular, GraphicsUnit.Point);

			if (ipep.AddressList.Length < 1)
			{
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gestört!", f, Brushes.Black, 10, 40+y);
				pg.DrawString("Ihr Rechner hat im Augenblick keine eindeutige IP-Adresse!", f, Brushes.Black, 10, 60+y);
				pg.DrawString("- Systemsteuerung benutzen", f, Brushes.Black, 10, 80+y);
				pg.DrawString("- Netzwerkkarte oder Loopback-Adapter installieren", f, Brushes.Black, 150, 100+y);
				pg.DrawString("- Statische IP-Adresse zuweisen", f, Brushes.Black, 10, 120+y);
				pg.DrawString("- Netzwerk aktivieren", f, Brushes.Black, 10, 140+y);
				pg.DrawString("- System neu starten", f, Brushes.Black, 10, 160+y);
			}
			else
			{	
				pg.DrawString("Die Kommunikation mit ZUSI ist im Augenblick gestört!", f, Brushes.Black, 10, 40+y);
				pg.DrawString("- ZUSI starten", f, Brushes.Black, 10, 60+y);
				pg.DrawString("- ZUSI TCP Server starten und einschalten", f, Brushes.Black, 10, 80+y);
				pg.DrawString("- Host: '"+m_conf.Host+"' überprüfen", f, Brushes.Black, 10, 100+y);
				pg.DrawString("- Port: "+m_conf.Port.ToString()+" überprüfen", f, Brushes.Black, 10, 120+y);
				pg.DrawString("- Automatisches Anmelden des Displays überwachen", f, Brushes.Black, 10, 140+y);
				pg.DrawString("- Im Fehlerfall TCP Server ausschalten und wieder einschalten", f, Brushes.Black, 10, 160+y);
				pg.DrawString("- Display neu starten", f, Brushes.Black, 10, 180+y);
				pg.DrawString("- Nach erfolgreicher Anmeldung, ZUSI verbinden", f, Brushes.Black, 10, 200+y);
				pg.DrawString("- Feld 'Angeforderte Größen' beobachten", f, Brushes.Black, 10, 220+y);

				/*int counter = -1;

				foreach (IPAddress add in ipep.AddressList)
				{
					counter++;
					ip = add.ToString();
					if (counter < 1)
					{
						pg.DrawString("- Adresse: "+ip+" und Port: "+m_conf.Port.ToString()+" eintragen", f, Brushes.Black, 10, 120+20*counter+y);
					}
					else
					{
						pg.DrawString("- oder Adresse: "+ip+" eintragen", f, Brushes.Black, 10, 120+20*counter+y);
					}
				}  */

				//pg.DrawString("- Auf Verbinden klicken", f, Brushes.Black, 10, 140+20*counter+y);
				//pg.DrawString("- Feld 'Angeforderte Größen' beobachten", f, Brushes.Black, 10, 160+20*counter+y);
			}
		}

		private void DrawS02(ref Graphics pg, bool greater)
		{
			Font f = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Point);
			string s = "";

			Störung st = localstate.störungmgr.StörStack;
			s += st.Priority.ToString()+"  ";
			s += st.Name+"  ";
			s += st.Description;

			pg.DrawString(s, f, Brushes.Black, 20, 1);

			f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Point);

			pg.DrawString("Die Trennschütze in Ihrem Triebfahrzeug sind gefallen!", f, Brushes.Black, 50, 40);
			if (greater)
			{
				pg.DrawString("- Zug anhalten", f, Brushes.Black, 50, 60);
				pg.DrawString("- Zugkraft auf Null", f, Brushes.Black, 50, 80);
				pg.DrawString("- Hauptschalter auschalten", f, Brushes.Black, 50, 100);
				pg.DrawString("- kurz warten", f, Brushes.Black, 50, 120);
				pg.DrawString("- Hauptschalter wieder einschalten", f, Brushes.Black, 50, 140);
				pg.DrawString("- Zugkraft aufschalten", f, Brushes.Black, 50, 160);
			}
			else
			{
				pg.DrawString("- Zugkraft auf Null", f, Brushes.Black, 50, 60);
				pg.DrawString("- Hauptschalter auschalten", f, Brushes.Black, 50, 80);
				pg.DrawString("- kurz warten", f, Brushes.Black, 50, 100);
				pg.DrawString("- Hauptschalter wieder einschalten", f, Brushes.Black, 50, 120);
				pg.DrawString("- Zugkraft aufschalten", f, Brushes.Black, 50, 140);
			}
		}
		private void DrawSoftKeys(ref Graphics pg)
		{
			//Rahmen
			for(int i = 0; i < 10; i++)
			{
				// 55 / 8
				DrawSoftKey(ref pg, i*63+2, 410);
			}

			Font f = new Font(other_font, 13, FontStyle.Regular, GraphicsUnit.Point);

			if (localstate.Type == TYPE.David2)
			{
				// DAVID1 Softkeys Text
				pg.DrawString("DS", f, Brushes.Black, 6, 412);
				// leer
				pg.DrawString("BRH", f, Brushes.Black, 6+63*2, 412);
				pg.DrawString("D", f, Brushes.Black, 6+63*3, 412);
				pg.DrawString("Br", f, Brushes.Black, 6+63*4, 412);
				pg.DrawString("Fbr", f, Brushes.Black, 6+63*5, 412);
				pg.DrawString("F", f, Brushes.Black, 6+63*6, 412);
				pg.DrawString("dyn", f, Brushes.Black, 6+63*6+8, 417);
				pg.DrawString("B", f, Brushes.Black, 6+63*7, 412);
				pg.DrawString("HBL", f, Brushes.Black, 6+63*8, 412);
				pg.DrawString("HL", f, Brushes.Black, 6+63*9, 412);
			}
			else if (localstate.Type == TYPE.David1)
			{
				// DAVID1 Softkeys Text
				pg.DrawString("DS", f, Brushes.Black, 6, 412);
				pg.DrawString("W", f, Brushes.Black, 6+63, 412);
				// leer
				pg.DrawString("LZB", f, Brushes.Black, 6+63*3, 412);
				pg.DrawString("U", f, Brushes.Black, 6+63*4, 412);
					pg.DrawString("B", f, Brushes.Black, 6+63*4+12, 417);
				pg.DrawString("A", f, Brushes.Black, 6+63*5, 412);
				pg.DrawString("F", f, Brushes.Black, 6+63*6, 412);
				pg.DrawString("I", f, Brushes.Black, 6+63*7, 412);
				pg.DrawString("U", f, Brushes.Black, 6+63*8, 412);
				pg.DrawString("G", f, Brushes.Black, 6+63*9, 412);
			}
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
		public void SetButtons()
		{
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

		private bool IsNotStörDisplay
		{
			get
			{
				return (localstate.DISPLAY != CURRENT_DISPLAY.ST && localstate.DISPLAY != CURRENT_DISPLAY.V_EQUAL_0 && localstate.DISPLAY != CURRENT_DISPLAY.V_GREATER_0);
			}
		}
		public void DavidControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (m_backBuffer == null)
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

			g.Clear(Color.FromArgb(202, 204, 208));

			DrawLines(ref g);

			switch (localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.D1_G:
					DrawD1_G(ref g);
					break;
				case CURRENT_DISPLAY.D1_U:
					DrawD1_U(ref g);
					break;
				case CURRENT_DISPLAY.D1_F:
					DrawD1_F(ref g);
					break;
				case CURRENT_DISPLAY.D1_I:
					DrawD1_I(ref g);
					break;
				case CURRENT_DISPLAY.D2_Fdyn:
					DrawD2_Fdyn(ref g);
					break;
				case CURRENT_DISPLAY.D2_B:
					DrawD2_B(ref g);
					break;
				case CURRENT_DISPLAY.D2_HL:
					DrawD2_HL(ref g);
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
				default:
					DrawNONE(ref g);
					break;
			}
			if (localstate.Type == TYPE.David2 && !ShowKeys || localstate.Type == TYPE.David1 && ShowKeys) 
			{
				if (!(localstate.DISPLAY == CURRENT_DISPLAY.ST || localstate.DISPLAY == CURRENT_DISPLAY.V_EQUAL_0 || localstate.DISPLAY == CURRENT_DISPLAY.V_GREATER_0))
                    DrawFPL(ref g);
			}
			else if (localstate.DISPLAY != CURRENT_DISPLAY.ST && localstate.DISPLAY != CURRENT_DISPLAY.V_EQUAL_0 && localstate.DISPLAY != CURRENT_DISPLAY.V_GREATER_0)
			{
				DrawUhr(ref g);
				DrawDatum(ref g);
			}
			else
			{

			}

			if (ShowKeys)
			{
				DrawSoftKeys(ref g);
			}

			if (USE_DOUBLE_BUFFER)
			{
				//g.Dispose();

				//Copy the back buffer to the screen
				e.Graphics.DrawImageUnscaled(m_backBuffer,0,0);
			}
		}

		private void timer_st_Tick(object sender, System.EventArgs e)
		{
			if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{
				if (STÖRUNG_BG == Color.Gold)
				{
					STÖRUNG_BG = Color.FromArgb(202, 204, 208);
					STÖRUNG_FG = Color.Gold;
				}
				else
				{
					STÖRUNG_BG = Color.Gold;
					STÖRUNG_FG = Color.Blue;
				}
				// SOUND
				stoerung_counter++;
				if (stoerung_counter == 30)
				{
					stoerung_counter = 0;
					sound.PlayMalfunctionBombardierSound();
				}
			}
			else
			{
				STÖRUNG_BG = Color.Gold;
				STÖRUNG_FG = Color.Blue;
			}

			

			something_changed = true;
		}
	}
}
