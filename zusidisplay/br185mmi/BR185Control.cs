using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Data;
using System.Windows.Forms;

using MMI.EBuLa.Tools;


namespace MMI.MMIBR185
{	   
	public class BR185Control : System.Windows.Forms.UserControl
	{
		bool USE_DOUBLE_BUFFER = false;

		public bool PREVENT_DRAW = false;

		public bool ZUSI_AT_TCP_SERVER = false;

		private bool DrawDots = true, InternalDrawDots = true;

		Square[] LMs = new Square[7];
		Line[] LINEs = new Line[3];
		DateTime lastTime = new DateTime(0);

		#region Declarations
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		private Bitmap m_backBuffer;
		private Graphics g;

		private DateTime vtime;

		private bool CONNECTED = false;

		private Thread zugkraft_thread;


		/*bool first_time_tacho = true;
		bool first_time_zugkraft = true;
		bool first_time_kombi = true;*/
		public bool something_changed = true;
		Color MMI_BLUE = Color.FromArgb(0,128,255);

		public MMI.EBuLa.Tools.XMLLoader m_conf;
		private const SmoothingMode SMOOTHING_MODE = SmoothingMode.AntiAlias;
		private const TextRenderingHint TEXT_MODE = TextRenderingHint.AntiAliasGridFit;
		float old_valu = 0f;

		//DBGraphics graph_main;

		public State localstate;		

		Square LZB_B, LZB_S, LZB_G, LZB_H, LZB_Ende,
			PZB_1000, PZB_500, PZB_Befehl,
			PZB_ZA_O, PZB_ZA_M, PZB_ZA_U,
			PZB_AKTIV, HS, ZS, T�R, SIFA, NB�_EP,
			INTEGRA_GELB, INTEGRA_ROT, ZUB_GELB, ZUB_ROT, ZUB_BLAU;
		Line LZB_S_TEXT, LZB_G_TEXT, PZB_1000_TEXT, PZB_500_TEXT,
			M_Status1, M_Status2, M_Status3;
		private System.Windows.Forms.Panel p_Buttons;
		private System.Windows.Forms.Panel p_Button1;
		private System.Windows.Forms.Panel p_UpperLine;
		private System.Windows.Forms.Panel p_Divider1;
		private System.Windows.Forms.Label l_Button1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel p_Button2;
		private System.Windows.Forms.Label l_Button2;
		private System.Windows.Forms.Panel p_Button3;
		private System.Windows.Forms.Label l_Button3;
		private System.Windows.Forms.Panel p_Button4;
		private System.Windows.Forms.Label l_Button4;
		private System.Windows.Forms.Panel p_Button5;
		private System.Windows.Forms.Label l_Button5;
		private System.Windows.Forms.Panel p_Button6;
		private System.Windows.Forms.Label l_Button6;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel p_Button7;
		private System.Windows.Forms.Label l_Button7;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel p_Button8;
		private System.Windows.Forms.Label l_Button8;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel p_Button9;
		private System.Windows.Forms.Label l_Button9;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel p_Button0;
		private System.Windows.Forms.Label l_Button0;
		private System.Windows.Forms.PictureBox pictureBox1;

		bool on = true;
		private System.Windows.Forms.Timer timer_Doppelpunkt;
		private System.Windows.Forms.Timer timerDisableDots;
		bool isEmbeded = false;

		#endregion
		public BR185Control(MMI.EBuLa.Tools.XMLLoader conf, bool isEmbeded) : this(conf) 
		{
			this.isEmbeded = isEmbeded;
		}

		public BR185Control(MMI.EBuLa.Tools.XMLLoader conf)
		{
			USE_DOUBLE_BUFFER = conf.DoubleBuffer;
			if (!USE_DOUBLE_BUFFER)
			{
				//This turns off internal double buffering of all custom GDI+ drawing
				this.SetStyle(ControlStyles.DoubleBuffer, true);
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.UserPaint, true);
			}

			InitializeComponent();

			m_conf = conf;

			localstate = new State();

			//graph_geschw = new DBGraphics();
			//graph_zugkraft = new DBGraphics();
			//graph_main = new DBGraphics();
			//while 
			//	(!graph_main.CreateDoubleBuffer(this.Width, this.Height)) {}			

			//graph_main.g.SmoothingMode = SMOOTHING_MODE;
			//graph_main.g.TextRenderingHint = TEXT_MODE;

			LZB_B = new Square(this, new Point(57,263), new Point(57+39,263+47), "B", MMI_BLUE, false, 0, false, "");

			LZB_G = new Square(this, new Point(96, 263), new Point(96+38,263+47), "G", Color.Red, false, 0, false, "");
			LZB_G_TEXT = new Line(this, new Point(57,311), new Point(325,337), "Geschwindigkeit reduzieren", Color.WhiteSmoke, false, 0);

			LZB_S = new Square(this, new Point(134,263), new Point(134+38,263+47), "S", Color.Red, false, 0, false, "");
			LZB_S_TEXT = new Line(this, new Point(57,337), new Point(57+268,364), "LZB/PZB-Zwangsbremsung", Color.Red, false, 0);

			LZB_H = new Square(this, new Point(172,263), new Point(172+38,263+47), "H", Color.Red, false, 0, false, "");
			// LZB_H_TEXT fehlt noch

			LZB_Ende = new Square(this, new Point(210,263), new Point(210+38,263+47), "LZB", Color.Yellow, false, 0, true, "Ende");

			PZB_AKTIV = new Square(this, new Point(248,263), new Point(248+38,263+47), "PZB", MMI_BLUE, true, 1000, false, "");

			PZB_ZA_O = new Square(this, new Point(96, 263), new Point(96+38,263+47), "85", MMI_BLUE, false, 0, false, "");
			PZB_ZA_U = new Square(this, new Point(96, 263), new Point(96+38,263+47), "55", MMI_BLUE, false, 0, false, "");
			PZB_ZA_M = new Square(this, new Point(210,263), new Point(210+38,263+47), "70", MMI_BLUE, false, 0, false, "");


			PZB_1000 = new Square(this, new Point(134,263), new Point(134+38,263+47), "1000", Color.Yellow, false, 0, true, "Hz");
			PZB_1000_TEXT = new Line(this, new Point(57,311), new Point(325,337), "�berwachungsgeschwindigkeit: "+localstate.Zugart+"km/h", Color.Yellow, false, 0);
			
			PZB_500 = new Square(this, new Point(172,263), new Point(172+38,263+47), "500", Color.Red, false, 0, true, "Hz");
			PZB_500_TEXT = new Line(this, new Point(57,337), new Point(325,364), "�berwachungsgeschwindigkeit: 45km/h", Color.Red, false, 0);

			PZB_Befehl = new Square(this, new Point(286,263), new Point(286+39, 263+47), "Befehl", Color.WhiteSmoke, false, 0, false, "");

			HS = new Square(this, new Point(364, 263), new Point(364+38,263+47), "HS", Color.OldLace, false, 0, false, "");
			ZS = new Square(this, new Point(325, 263), new Point(325+38,263+47), "ZS", Color.Black, false, 0, false, "");

			T�R = new Square(this, new Point(440, 263), new Point(440+38,263+47), "T", Color.Yellow, false, 0, false, "");

			// NOTBREMSE und E-BREMSE fehlen

			SIFA = new Square(this, new Point(554, 263), new Point(571+58, 263+47), "Sifa", Color.WhiteSmoke, false, 0, false, "");

			NB�_EP = new Square(this, new Point(516, 263), new Point(516+38,263+47), "NB�", Color.Yellow, false, 0, true, "ep");

			// INTEGRA
			INTEGRA_GELB = new Square(this, new Point(57,263), new Point(57+39,263+47), "INT", Color.Yellow, false, 0, false, "");
			INTEGRA_ROT = new Square(this, new Point(96, 263), new Point(96+38,263+47), "INT", Color.Red, false, 0, false, "");
			ZUB_GELB = new Square(this, new Point(57,263), new Point(57+39,263+47), "ZUB", Color.Yellow, false, 0, false, "");
			ZUB_ROT = new Square(this, new Point(96, 263), new Point(96+38,263+47), "ZUB", Color.Red, false, 0, false, "");
			ZUB_BLAU = new Square(this, new Point(57,263), new Point(57+39,263+47), "ZUB", MMI_BLUE, false, 0, false, "");


			// Maschinenstatus
			M_Status1 = new Line(this, new Point(325,310), new Point(554,337), "Keine Verbindung zu TCP Server", MMI_BLUE, false, 0);

			SetButtons();

			vtime = DateTime.Now;

			zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));

			int interval = Convert.ToInt32(Math.Round((1d/(double)conf.FramesPerSecond)*1000d));
			timer1.Interval = interval;
			timer1.Enabled = true;
		}

		protected override void Dispose( bool disposing )
		{
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
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BR185Control));
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.p_Buttons = new System.Windows.Forms.Panel();
			this.panel12 = new System.Windows.Forms.Panel();
			this.p_Button0 = new System.Windows.Forms.Panel();
			this.l_Button0 = new System.Windows.Forms.Label();
			this.panel10 = new System.Windows.Forms.Panel();
			this.p_Button9 = new System.Windows.Forms.Panel();
			this.l_Button9 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.p_Button8 = new System.Windows.Forms.Panel();
			this.l_Button8 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.p_Button7 = new System.Windows.Forms.Panel();
			this.l_Button7 = new System.Windows.Forms.Label();
			this.panel9 = new System.Windows.Forms.Panel();
			this.p_Button6 = new System.Windows.Forms.Panel();
			this.l_Button6 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.p_Button5 = new System.Windows.Forms.Panel();
			this.l_Button5 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.p_Button4 = new System.Windows.Forms.Panel();
			this.l_Button4 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.p_Button3 = new System.Windows.Forms.Panel();
			this.l_Button3 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.p_Button2 = new System.Windows.Forms.Panel();
			this.l_Button2 = new System.Windows.Forms.Label();
			this.p_Divider1 = new System.Windows.Forms.Panel();
			this.p_Button1 = new System.Windows.Forms.Panel();
			this.l_Button1 = new System.Windows.Forms.Label();
			this.p_UpperLine = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.timer_Doppelpunkt = new System.Windows.Forms.Timer(this.components);
			this.timerDisableDots = new System.Windows.Forms.Timer(this.components);
			this.p_Buttons.SuspendLayout();
			this.p_Button0.SuspendLayout();
			this.p_Button9.SuspendLayout();
			this.p_Button8.SuspendLayout();
			this.p_Button7.SuspendLayout();
			this.p_Button6.SuspendLayout();
			this.p_Button5.SuspendLayout();
			this.p_Button4.SuspendLayout();
			this.p_Button3.SuspendLayout();
			this.p_Button2.SuspendLayout();
			this.p_Button1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 30;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// p_Buttons
			// 
			this.p_Buttons.BackColor = System.Drawing.SystemColors.Control;
			this.p_Buttons.Controls.Add(this.panel12);
			this.p_Buttons.Controls.Add(this.p_Button0);
			this.p_Buttons.Controls.Add(this.panel10);
			this.p_Buttons.Controls.Add(this.p_Button9);
			this.p_Buttons.Controls.Add(this.panel6);
			this.p_Buttons.Controls.Add(this.p_Button8);
			this.p_Buttons.Controls.Add(this.panel1);
			this.p_Buttons.Controls.Add(this.p_Button7);
			this.p_Buttons.Controls.Add(this.panel9);
			this.p_Buttons.Controls.Add(this.p_Button6);
			this.p_Buttons.Controls.Add(this.panel7);
			this.p_Buttons.Controls.Add(this.p_Button5);
			this.p_Buttons.Controls.Add(this.panel5);
			this.p_Buttons.Controls.Add(this.p_Button4);
			this.p_Buttons.Controls.Add(this.panel3);
			this.p_Buttons.Controls.Add(this.p_Button3);
			this.p_Buttons.Controls.Add(this.panel2);
			this.p_Buttons.Controls.Add(this.p_Button2);
			this.p_Buttons.Controls.Add(this.p_Divider1);
			this.p_Buttons.Controls.Add(this.p_Button1);
			this.p_Buttons.Location = new System.Drawing.Point(0, 395);
			this.p_Buttons.Name = "p_Buttons";
			this.p_Buttons.Size = new System.Drawing.Size(632, 64);
			this.p_Buttons.TabIndex = 0;
			// 
			// panel12
			// 
			this.panel12.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel12.Location = new System.Drawing.Point(629, 0);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(3, 64);
			this.panel12.TabIndex = 19;
			// 
			// p_Button0
			// 
			this.p_Button0.BackColor = System.Drawing.Color.LightGray;
			this.p_Button0.Controls.Add(this.l_Button0);
			this.p_Button0.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button0.Location = new System.Drawing.Point(569, 0);
			this.p_Button0.Name = "p_Button0";
			this.p_Button0.Size = new System.Drawing.Size(60, 64);
			this.p_Button0.TabIndex = 18;
			this.p_Button0.Click += new System.EventHandler(this.Button_0_Pressed);
			// 
			// l_Button0
			// 
			this.l_Button0.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button0.Location = new System.Drawing.Point(6, 20);
			this.l_Button0.Name = "l_Button0";
			this.l_Button0.Size = new System.Drawing.Size(48, 27);
			this.l_Button0.TabIndex = 3;
			this.l_Button0.Text = "Button Null";
			this.l_Button0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.l_Button0.Click += new System.EventHandler(this.Button_0_Pressed);
			// 
			// panel10
			// 
			this.panel10.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel10.Location = new System.Drawing.Point(567, 0);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(2, 64);
			this.panel10.TabIndex = 17;
			// 
			// p_Button9
			// 
			this.p_Button9.BackColor = System.Drawing.Color.LightGray;
			this.p_Button9.Controls.Add(this.l_Button9);
			this.p_Button9.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button9.Location = new System.Drawing.Point(506, 0);
			this.p_Button9.Name = "p_Button9";
			this.p_Button9.Size = new System.Drawing.Size(61, 64);
			this.p_Button9.TabIndex = 16;
			// 
			// l_Button9
			// 
			this.l_Button9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button9.Location = new System.Drawing.Point(6, 20);
			this.l_Button9.Name = "l_Button9";
			this.l_Button9.Size = new System.Drawing.Size(48, 27);
			this.l_Button9.TabIndex = 3;
			this.l_Button9.Text = "Button Neun";
			this.l_Button9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel6.Location = new System.Drawing.Point(504, 0);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(2, 64);
			this.panel6.TabIndex = 15;
			// 
			// p_Button8
			// 
			this.p_Button8.BackColor = System.Drawing.Color.LightGray;
			this.p_Button8.Controls.Add(this.l_Button8);
			this.p_Button8.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button8.Location = new System.Drawing.Point(443, 0);
			this.p_Button8.Name = "p_Button8";
			this.p_Button8.Size = new System.Drawing.Size(61, 64);
			this.p_Button8.TabIndex = 14;
			// 
			// l_Button8
			// 
			this.l_Button8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button8.Location = new System.Drawing.Point(6, 20);
			this.l_Button8.Name = "l_Button8";
			this.l_Button8.Size = new System.Drawing.Size(48, 27);
			this.l_Button8.TabIndex = 3;
			this.l_Button8.Text = "Button Acht";
			this.l_Button8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(441, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(2, 64);
			this.panel1.TabIndex = 13;
			// 
			// p_Button7
			// 
			this.p_Button7.BackColor = System.Drawing.Color.LightGray;
			this.p_Button7.Controls.Add(this.l_Button7);
			this.p_Button7.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button7.Location = new System.Drawing.Point(380, 0);
			this.p_Button7.Name = "p_Button7";
			this.p_Button7.Size = new System.Drawing.Size(61, 64);
			this.p_Button7.TabIndex = 12;
			// 
			// l_Button7
			// 
			this.l_Button7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button7.Location = new System.Drawing.Point(6, 20);
			this.l_Button7.Name = "l_Button7";
			this.l_Button7.Size = new System.Drawing.Size(48, 27);
			this.l_Button7.TabIndex = 3;
			this.l_Button7.Text = "Button Sieben";
			this.l_Button7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel9.Location = new System.Drawing.Point(378, 0);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(2, 64);
			this.panel9.TabIndex = 11;
			// 
			// p_Button6
			// 
			this.p_Button6.BackColor = System.Drawing.Color.LightGray;
			this.p_Button6.Controls.Add(this.l_Button6);
			this.p_Button6.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button6.Location = new System.Drawing.Point(316, 0);
			this.p_Button6.Name = "p_Button6";
			this.p_Button6.Size = new System.Drawing.Size(62, 64);
			this.p_Button6.TabIndex = 10;
			this.p_Button6.Click += new System.EventHandler(this.Button_6_Pressed);
			// 
			// l_Button6
			// 
			this.l_Button6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button6.Location = new System.Drawing.Point(6, 20);
			this.l_Button6.Name = "l_Button6";
			this.l_Button6.Size = new System.Drawing.Size(50, 27);
			this.l_Button6.TabIndex = 3;
			this.l_Button6.Text = "Button Sechs";
			this.l_Button6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.l_Button6.Click += new System.EventHandler(this.Button_6_Pressed);
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel7.Location = new System.Drawing.Point(314, 0);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(2, 64);
			this.panel7.TabIndex = 9;
			// 
			// p_Button5
			// 
			this.p_Button5.BackColor = System.Drawing.Color.LightGray;
			this.p_Button5.Controls.Add(this.l_Button5);
			this.p_Button5.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button5.Location = new System.Drawing.Point(253, 0);
			this.p_Button5.Name = "p_Button5";
			this.p_Button5.Size = new System.Drawing.Size(61, 64);
			this.p_Button5.TabIndex = 8;
			// 
			// l_Button5
			// 
			this.l_Button5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button5.Location = new System.Drawing.Point(6, 20);
			this.l_Button5.Name = "l_Button5";
			this.l_Button5.Size = new System.Drawing.Size(48, 27);
			this.l_Button5.TabIndex = 3;
			this.l_Button5.Text = "Button F�nf";
			this.l_Button5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel5.Location = new System.Drawing.Point(251, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(2, 64);
			this.panel5.TabIndex = 7;
			// 
			// p_Button4
			// 
			this.p_Button4.BackColor = System.Drawing.Color.LightGray;
			this.p_Button4.Controls.Add(this.l_Button4);
			this.p_Button4.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button4.Location = new System.Drawing.Point(190, 0);
			this.p_Button4.Name = "p_Button4";
			this.p_Button4.Size = new System.Drawing.Size(61, 64);
			this.p_Button4.TabIndex = 6;
			// 
			// l_Button4
			// 
			this.l_Button4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button4.Location = new System.Drawing.Point(6, 20);
			this.l_Button4.Name = "l_Button4";
			this.l_Button4.Size = new System.Drawing.Size(48, 27);
			this.l_Button4.TabIndex = 3;
			this.l_Button4.Text = "Button Vier";
			this.l_Button4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel3.Location = new System.Drawing.Point(188, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(2, 64);
			this.panel3.TabIndex = 5;
			// 
			// p_Button3
			// 
			this.p_Button3.BackColor = System.Drawing.Color.LightGray;
			this.p_Button3.Controls.Add(this.l_Button3);
			this.p_Button3.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button3.Location = new System.Drawing.Point(127, 0);
			this.p_Button3.Name = "p_Button3";
			this.p_Button3.Size = new System.Drawing.Size(61, 64);
			this.p_Button3.TabIndex = 4;
			// 
			// l_Button3
			// 
			this.l_Button3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button3.Location = new System.Drawing.Point(6, 20);
			this.l_Button3.Name = "l_Button3";
			this.l_Button3.Size = new System.Drawing.Size(48, 27);
			this.l_Button3.TabIndex = 3;
			this.l_Button3.Text = "Button Drei";
			this.l_Button3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(125, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(2, 64);
			this.panel2.TabIndex = 3;
			// 
			// p_Button2
			// 
			this.p_Button2.BackColor = System.Drawing.Color.LightGray;
			this.p_Button2.Controls.Add(this.l_Button2);
			this.p_Button2.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button2.Location = new System.Drawing.Point(64, 0);
			this.p_Button2.Name = "p_Button2";
			this.p_Button2.Size = new System.Drawing.Size(61, 64);
			this.p_Button2.TabIndex = 2;
			// 
			// l_Button2
			// 
			this.l_Button2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button2.Location = new System.Drawing.Point(6, 20);
			this.l_Button2.Name = "l_Button2";
			this.l_Button2.Size = new System.Drawing.Size(48, 27);
			this.l_Button2.TabIndex = 3;
			this.l_Button2.Text = "Button Zwei";
			this.l_Button2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Divider1
			// 
			this.p_Divider1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Divider1.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Divider1.Location = new System.Drawing.Point(62, 0);
			this.p_Divider1.Name = "p_Divider1";
			this.p_Divider1.Size = new System.Drawing.Size(2, 64);
			this.p_Divider1.TabIndex = 1;
			// 
			// p_Button1
			// 
			this.p_Button1.BackColor = System.Drawing.Color.LightGray;
			this.p_Button1.Controls.Add(this.l_Button1);
			this.p_Button1.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button1.Location = new System.Drawing.Point(0, 0);
			this.p_Button1.Name = "p_Button1";
			this.p_Button1.Size = new System.Drawing.Size(62, 64);
			this.p_Button1.TabIndex = 0;
			// 
			// l_Button1
			// 
			this.l_Button1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button1.Location = new System.Drawing.Point(6, 20);
			this.l_Button1.Name = "l_Button1";
			this.l_Button1.Size = new System.Drawing.Size(48, 27);
			this.l_Button1.TabIndex = 3;
			this.l_Button1.Text = "Button Eins";
			this.l_Button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_UpperLine
			// 
			this.p_UpperLine.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_UpperLine.Location = new System.Drawing.Point(0, 393);
			this.p_UpperLine.Name = "p_UpperLine";
			this.p_UpperLine.Size = new System.Drawing.Size(700, 2);
			this.p_UpperLine.TabIndex = 1;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(555, 351);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(74, 41);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// timer_Doppelpunkt
			// 
			this.timer_Doppelpunkt.Enabled = true;
			this.timer_Doppelpunkt.Interval = 1000;
			this.timer_Doppelpunkt.Tick += new System.EventHandler(this.timer_Doppelpunkt_Tick);
			// 
			// timerDisableDots
			// 
			this.timerDisableDots.Interval = 500;
			this.timerDisableDots.Tick += new System.EventHandler(this.timerDisableDots_Tick);
			// 
			// BR185Control
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.p_UpperLine);
			this.Controls.Add(this.p_Buttons);
			this.Name = "BR185Control";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BR185Control_Paint);
			this.p_Buttons.ResumeLayout(false);
			this.p_Button0.ResumeLayout(false);
			this.p_Button9.ResumeLayout(false);
			this.p_Button8.ResumeLayout(false);
			this.p_Button7.ResumeLayout(false);
			this.p_Button6.ResumeLayout(false);
			this.p_Button5.ResumeLayout(false);
			this.p_Button4.ResumeLayout(false);
			this.p_Button3.ResumeLayout(false);
			this.p_Button2.ResumeLayout(false);
			this.p_Button1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		public string Version()
		{
			return Application.ProductVersion;
		}

		public void Inverse()
		{
		}

		public bool addtionalhours
		{
			get{return localstate.addtionalhours;}
			set{localstate.addtionalhours = value;}
		}
		#region Buttons
		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
		}

		public void Button_Down_Pressed(object sender, System.EventArgs e)
		{

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

		}

		public void Button_3_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_4_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_5_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_6_Pressed(object sender, System.EventArgs e)
		{
			localstate.Ein_Display = !localstate.Ein_Display;
			something_changed = true;
			SetButtons();
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_7_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_8_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_9_Pressed(object sender, System.EventArgs e)
		{

		}

		public void Button_0_Pressed(object sender, System.EventArgs e)
		{	
			Switcher s = new Switcher(ref localstate, m_conf.TopMost);
			s.ShowDialog();
			localstate.ResetBackups();
			LMs = new Square[7];
			something_changed = true;

			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		#endregion
		public bool IsCONNECTED
		{
			get{return CONNECTED;}
			set
			{
				CONNECTED = value;
				something_changed = true;
			}
		}
		public void DrawLines(ref Graphics pg)
		{
			//Graphics pg = graph_main;
			// Stift
			Pen pen = new Pen(Color.Silver, 1);

			// Umrahmung
			Point[] points = {new Point(0,0), new Point(630-1,0), new Point(630-1,460), new Point(0,460), new Point(0,0)};
			pg.DrawLines(pen,points);
            
			// L�ngstrenner
			pg.DrawLine(pen, new Point(325, 0), new Point(325, 460)); 

			// Quertrenner
			pg.DrawLine(pen, new Point(0, 264), new Point(630, 264));

			// LZB Zielweg Anzeige
			pg.DrawLine(pen, new Point(57, 0), new Point(57, 460));
			pg.DrawLine(pen, new Point(0, 57), new Point(57, 57));
			pg.DrawLine(pen, new Point(0, 235), new Point(57, 235));

			Font f = new Font("Arial", 8);
			pen.Color = Color.DarkSlateGray;
			Pen pen2 = new Pen(Color.Silver, 1);
			for (int k = 1; k < 9; k++)
			{
				pg.DrawLine(pen2, new Point(33, 67+k*20), new Point(57, 67+k*20));
				if (k < 8)
				pg.DrawLine(pen, new Point(33, 67+k*20+4), new Point(57, 67+k*20+4));
				if (k <= 7)
				{
					pg.DrawLine(pen, new Point(33, 67+k*20+8), new Point(57, 67+k*20+8));
					pg.DrawLine(pen, new Point(33, 67+k*20+12), new Point(57, 67+k*20+12));
					pg.DrawLine(pen, new Point(33, 67+k*20+16), new Point(57, 67+k*20+16));
				}
				if (k < 5)
					pg.DrawString(getLZBDistString(k), f, new SolidBrush(Color.White), new Point(5, 60+k*20));
				else if (k < 8)
					pg.DrawString(getLZBDistString(k), f, new SolidBrush(Color.White), new Point(11, 60+k*20));
				else if (k < 9)
				{
					pg.DrawString(getLZBDistString(k), f, new SolidBrush(Color.White), new Point(17, 60+k*20));
					pg.DrawLine(pen2, new Point(33, 67+k*20-10), new Point(57, 67+k*20-10));
					pg.DrawString("100", f, new SolidBrush(Color.White), new Point(11, 60+k*20-9));
				}
			}

			pen.Color = Color.Silver;

			// LZB Zielgeschwindigkeit
			if (localstate.TrainType != TRAIN_TYPE.DBpzfa766_1)
			{
				Brush bw = new SolidBrush(Color.Gray);
				Pen pw = new Pen(bw, 1);
				int xpos = 160;
				int ypos = 215;
				pg.DrawRectangle(pw, xpos, ypos, 20,30);
				pg.DrawRectangle(pw, xpos+20, ypos, 20,30);
				pg.DrawRectangle(pw, xpos+40, ypos, 20,30);
			}

			// Zugbeeinflussungssysteme (Reihe Links)
			pg.DrawLine(pen, new Point(0, 310), new Point(630, 310));
            
			pg.DrawLine(pen, new Point(57, 264), new Point(57, 310));
			pg.DrawLine(pen, new Point(96, 264), new Point(96, 310));
			pg.DrawLine(pen, new Point(134, 264), new Point(134, 310));
			pg.DrawLine(pen, new Point(172, 264), new Point(172, 310));
			pg.DrawLine(pen, new Point(210, 264), new Point(210, 310));
			pg.DrawLine(pen, new Point(248, 264), new Point(248, 310));
			pg.DrawLine(pen, new Point(286, 264), new Point(286, 310));
			pg.DrawLine(pen, new Point(325, 264), new Point(325, 310));

			// Zugbeeinflussungssysteme (Reihe Rechts)
			pg.DrawLine(pen, new Point(364, 264), new Point(364, 310));
			pg.DrawLine(pen, new Point(402, 264), new Point(402, 310));
			pg.DrawLine(pen, new Point(440, 264), new Point(440, 310));
			pg.DrawLine(pen, new Point(478, 264), new Point(478, 310));
			pg.DrawLine(pen, new Point(516, 264), new Point(516, 310));
			pg.DrawLine(pen, new Point(554, 264), new Point(554, 391));
			pg.DrawLine(pen, new Point(554, 350), new Point(630, 350));

            
			// Klartextanweisungen
			pg.DrawLine(pen, new Point(0, 337), new Point(554, 337));
			pg.DrawLine(pen, new Point(0, 364), new Point(554, 364));
			pg.DrawLine(pen, new Point(0, 392), new Point(630, 392));
            
		}

		public void FillFields(ref Graphics pg)
		{
			// Graphics Object
			//Graphics pg = graph_main.g;

			// Brush Object
			SolidBrush sb = new SolidBrush(Color.FromArgb(58,41,121));
			//pg.FillRectangle(sb, 0, 264, 45, 45);
			pg.FillRectangle(sb, 45, 264, 45, 45);
			//pg.FillRectangle(sb, 270, 264, 45, 45);

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
			if (text == "85" || text == "95")
			{
				localstate.LM_Zugart_O = state;
			}
			else if (text == "70" || text == "75")
			{ 
				localstate.LM_Zugart_M = state;
			}
			else if (text == "55" || text == "60")
			{
				localstate.LM_Zugart_U = state;
			}

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

		public void SetLM_LZB_�(bool state)
		{
			localstate.LM_LZB_� = state;

			if (state) // LZB
			{
				PZB_AKTIV = new Square(this, new Point(248,263), new Point(248+38,263+47), "LZB", MMI_BLUE, true, 1000, false, "");
			}
			else // PZB
			{
				PZB_AKTIV = new Square(this, new Point(248,263), new Point(248+38,263+47), "PZB", MMI_BLUE, true, 1000, false, "");
			}
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
		public void SetLM_T�r(bool state)
		{
			localstate.LM_T�R = state;
			something_changed = true;
		}

		public void SetLM_INTEGRA(float state)
		{
			if (state == 0f) // Keine LM + Kein Schalter
			{
				localstate.LM_INTEGRA_GELB = false;
				localstate.LM_INTEGRA_ROT = false;
			}
			else if (state == 1f) // Keine LM + Schalter
			{
				localstate.LM_INTEGRA_GELB = false;
				localstate.LM_INTEGRA_ROT = false;
			}
			else if (state == 2f) // LM Gelb + Kein Schalter
			{
				localstate.LM_INTEGRA_GELB = true;
				localstate.LM_INTEGRA_ROT = false;
			}
			else if (state == 3f) // LM Gelb + Schalter
			{
				localstate.LM_INTEGRA_GELB = true;
				localstate.LM_INTEGRA_ROT = false;
			}
			else if (state == 4f) // LM Rot + Kein Schalter
			{
				localstate.LM_INTEGRA_GELB = false;
				localstate.LM_INTEGRA_ROT = true;
			}
			else if (state == 5f) // LM Rot + Schalter
			{
				localstate.LM_INTEGRA_GELB = false;
				localstate.LM_INTEGRA_ROT = true;
			}
			something_changed = true;
		}
		public void SetLM_GNT_�(bool state)
		{
			localstate.LM_GNT_� = state;
			something_changed = true;
		}

		public void SetLM_GNT_S(bool state)
		{
			localstate.LM_GNT_S = state;
			something_changed = true;
		}

		public void SetLM_GNT_B(bool state)
		{
			localstate.LM_GNT_B = state;
			something_changed = true;
		}

		public void SetLM_GNT_G(bool state)
		{
			localstate.LM_GNT_G = state;
			something_changed = true;
		}

		public void SetReisezug(bool state)
		{
			localstate.Reisezug = state;
			something_changed = true;
		}
		public void SetPZBSystem(float valu)
		{
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
					PZB_1000_TEXT.Text = "�berwachungsgeschwindigkeit: 95km/h";
				else if (localstate.Zugart == "70") 
					PZB_1000_TEXT.Text = "�berwachungsgeschwindigkeit: 75km/h";
				else 
					PZB_1000_TEXT.Text = "�berwachungsgeschwindigkeit: 60km/h";
			}
			else
			{
				PZB_ZA_O.Text1 = "85";
				PZB_ZA_M.Text1 = "70";
				PZB_ZA_U.Text1 = "55";
				PZB_1000_TEXT.Text = "�berwachungsgeschwindigkeit: "+localstate.Zugart+"km/h";
			}

			InitTrainControl();

			something_changed = true;		
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
		public void SetLZB_ZielWeg(float valu)
		{
			//if (Math.Abs(localstate.LZB_ZielWeg - valu) > 9f)
			{
				something_changed = true;
				localstate.LZB_ZielWeg = valu;			
			}
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

		public void SetOberstrom(float valu)
		{
			localstate.Oberstrom = valu;
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
			TimeSpan span = new TimeSpan(DateTime.Now.Ticks - lastTime.Ticks);

			if (span.TotalMilliseconds < 100)
			{
				lastTime = DateTime.Now;
				return;
			}

			vtime = valu;
			lastTime = DateTime.Now;
			//DrawDots = !DrawDots;

			if (localstate.SHOW_CLOCK) something_changed = true;
		}

		public void SetUhrDatumDigital(double valu)
		{
			try
			{
				SetUhrDatum(MMI.EBuLa.Tools.State.ConvertToDateTime(valu));
			}
			catch(Exception){}
		}

		public void DrawZugkraft_Fahrstufen(ref Graphics pg)
		{
			int fahrstufe = localstate.Fahrstufe;

			//---ZUGKRAFT--------------------------------------------
			DrawZugkraftRing(ref pg);


			//---FAHRSTUFE-------------------------------------------
			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				Font f = new Font("ZusiDigital standard", 22, FontStyle.Bold, GraphicsUnit.Pixel);
	
				string txt = localstate.Fahrstufe.ToString();
				if (fahrstufe < 10)
				{
					txt = " "+txt;
				}

				pg.DrawString(txt, f, Brushes.WhiteSmoke, 453f, 138f);

				f = new Font("Tahoma", 11, FontStyle.Bold);

				pg.DrawString("Stufe", f, Brushes.WhiteSmoke, 452, 118f);
			}
		}

		public void DrawZugkraftRing(ref Graphics pg)
		{
			#region DRAW

			// Brush Object
			SolidBrush sb = new SolidBrush(Color.LightGray);
			SolidBrush sb_white = new SolidBrush(Color.WhiteSmoke);
			SolidBrush sb_black = new SolidBrush(Color.Black);
			SolidBrush sb_blue = new SolidBrush(Color.DarkBlue);
			SolidBrush sb_orange = new SolidBrush(Color.Orange);

			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				sb_orange = new SolidBrush(Color.FromArgb(179, 177, 142));
			}
           
			Pen p = new Pen(sb, 2);
			Pen p_blue = new Pen(sb_blue, 1);
			Pen p_orange = new Pen(sb_orange, 1);

			Point center2 = new Point(355,20);
			Point center = new Point(475,140);

			int radius = 120;
			int small_radius = 85;

			// grauer Kreis
			pg.FillEllipse(sb_white, center2.X, center2.Y, radius*2, radius*2);

			// f�llst�cke f�r leeraum unten
			pg.FillPie(sb_black, center2.X, center2.Y, radius*2+1, radius*2+1, 89, 36);
			pg.FillPie(sb_black, center2.X, center2.Y, radius*2+1, radius*2+1, 91, -36);

			// farbige balken
			if (localstate.Zugkraft_Thread < 0)
			{
				float norm_zugkraft = localstate.Zugkraft_Thread / 150f * 145f;
				if (localstate.Zugkraft_Thread >= -2f) norm_zugkraft = 0f;
				pg.FillPie(sb_orange, center2.X, center2.Y, radius*2, radius*2, 270, norm_zugkraft);
			}
			else if (localstate.Zugkraft_Thread > 0)
			{
				float norm_zugkraft = localstate.Zugkraft_Thread / 75f * 145f - 2f;
				pg.FillPie(sb_blue, center2.X, center2.Y, radius*2, radius*2, 270, norm_zugkraft);
			}

			// markierungen
			for (double i = 35d; i < 325d; i=i+0.25d)
			{
				double angle = (double)i * (Math.PI / 180);
				int x = Convert.ToInt32((double)radius * Math.Sin(angle));
				int y = Convert.ToInt32((double)radius * Math.Cos(angle));

				// Markierungne
				if (Math.IEEERemainder((double)i-37.5d, 9.5d) == 0)
				{
					int x2 = Convert.ToInt32(((double)radius-35d) * Math.Sin(angle));
					int y2 = Convert.ToInt32(((double)radius-35d) * Math.Cos(angle));
					pg.DrawLine(p, center.X+x2, center.Y+y2, center.X+x, center.Y+y);
				}
			}

			// innerer schwarzer kreis
			pg.FillEllipse(sb_black, center2.X+35, center2.Y+35, small_radius*2, small_radius*2);

			Point old = new Point(0);

			// kleines Dreieck
			float norm_zugkraft2 = 0f;
			if (localstate.Zugkraft_Thread > 0)
			{
				norm_zugkraft2 = localstate.Zugkraft_Thread / 75f * 145f - 2f;
			}
			else if (localstate.Zugkraft_Thread < 0)
			{
				norm_zugkraft2 = localstate.Zugkraft_Thread / 150f * 145f;
				if (localstate.Zugkraft_Thread >= -2f) norm_zugkraft2 = 0f;

				if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
				{
					norm_zugkraft2 = 0f;
				}
			}
			

			//radius = 112;

			norm_zugkraft2 = 180 - norm_zugkraft2;
			if (norm_zugkraft2 < 0) norm_zugkraft2 += 360;

			double angle2 = (double)norm_zugkraft2 * (Math.PI / 180);
			int x3 = Convert.ToInt32((double)radius * Math.Sin(angle2));
			int y3 = Convert.ToInt32((double)radius * Math.Cos(angle2));

			double angle2_2 = (double)(norm_zugkraft2+3)* (Math.PI / 180);
			int x3_2 = Convert.ToInt32((double)(radius+12) * Math.Sin(angle2_2));
			int y3_2 = Convert.ToInt32((double)(radius+12) * Math.Cos(angle2_2));

			double angle2_3 = (double)(norm_zugkraft2-3)* (Math.PI / 180);
			int x3_3 = Convert.ToInt32((double)(radius+12) * Math.Sin(angle2_3));
			int y3_3 = Convert.ToInt32((double)(radius+12) * Math.Cos(angle2_3));

			Point[] points = new Point[3];

			points[0] = new Point(center.X+x3, center.Y+y3);
			points[1] = new Point(center.X+x3_2, center.Y+y3_2);
			points[2] = new Point(center.X+x3_3, center.Y+y3_3);

			Brush b_red = new SolidBrush(Color.Yellow);

			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				b_red = new SolidBrush(Color.WhiteSmoke);
			}

			Pen p_red = new Pen(b_red, 5);

			p_red.StartCap = LineCap.Triangle;
			pg.FillPolygon(b_red, points);

			// Zahlen
			Font f = new Font("Arial", 11f, FontStyle.Bold);

			
			// positiv
			pg.DrawString("0", f, sb_white, 470f, 56f);
			pg.DrawString("10", f, sb_white, 489f, 61f);
			pg.DrawString("20", f, sb_white, 509f, 72f);
			pg.DrawString("30", f, sb_white, 525f, 90f);
			pg.DrawString("40", f, sb_white, 536f, 112f);
			pg.DrawString("50", f, sb_white, 539f, 138f);
			pg.DrawString("60", f, sb_white, 533f, 164f);
			pg.DrawString("70", f, sb_white, 519f, 185f);

			// negativ
			pg.DrawString("20", f, sb_white, 444f, 61f);
			pg.DrawString("40", f, sb_white, 421f, 72f);
			pg.DrawString("60", f, sb_white, 406f, 90f);
			pg.DrawString("80", f, sb_white, 395f, 112f);
			pg.DrawString("100", f, sb_white, 391f, 138f);
			pg.DrawString("120", f, sb_white, 399f, 164f);
			pg.DrawString("140", f, sb_white, 414f, 185f);
			
			// kN
			f = new Font("Tahoma", 16, FontStyle.Bold);
			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				pg.DrawString("kN", f, Brushes.WhiteSmoke, 458f, 180f);
			}
			else
			{
				pg.DrawString("kN", f, Brushes.WhiteSmoke, 458f, 125f);
			}

			
			// senkrechter Strich
			p = new Pen(sb_white, 5);
			p.EndCap = LineCap.Round;
			p.StartCap = LineCap.Round;
			pg.DrawLine(p, 475f, 212f, 475f, 250f);

			// Text "Bremsen + Zugkr. pro FM"
			f = new Font("Tahoma", 8, FontStyle.Regular);
			pg.DrawString("Bremsen", f, Brushes.WhiteSmoke, 424f, 228f);
			pg.DrawString("Zugkraft", f, Brushes.WhiteSmoke, 482f, 222f);
			pg.DrawString(" pro FM", f, Brushes.WhiteSmoke, 482f, 234f);
		

			#endregion
		}

		public void DrawSpeedOMeter(ref Graphics pg)
		{
			//Graphics pg = graph_main.g;
			//pg.SmoothingMode = SMOOTHING_MODE;

			#region DRAW

			Color m_dark_color = Color.Black;

			// Brush Object
			SolidBrush sb = new SolidBrush(Color.WhiteSmoke);
           
			Point center = new Point(191,135);

			int radius = 120;

			Point old = new Point(0);

			Pen p = new Pen(sb, 1);
			Pen p_ring = new Pen(sb, 2);

			int test = 0;

			if (localstate.TrainType == TRAIN_TYPE.BR146_1 || localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				for (double i = 40d; i < 320d; i=i+0.25d)
				{
					double angle = (double)i * (Math.PI / 180);
					int x = Convert.ToInt32((double)radius * Math.Sin(angle));
					int y = Convert.ToInt32((double)radius * Math.Cos(angle));

					// Skala
					if (Math.IEEERemainder((double)i-9d, 15.5d) == 0)
					{
						test++;
						int x2 = Convert.ToInt32(((double)radius-18d) * Math.Sin(angle));
						int y2 = Convert.ToInt32(((double)radius-18d) * Math.Cos(angle));
						pg.DrawLine(p, center.X+x2, center.Y+y2, center.X+x, center.Y+y);
					}
					else if (Math.IEEERemainder(((double)i-9d), 7.75d) == 0)
					{
						test++;
						int x2 = Convert.ToInt32(((double)radius-7d) * Math.Sin(angle));
						int y2 = Convert.ToInt32(((double)radius-7d) * Math.Cos(angle));
						pg.DrawLine(p, center.X+x2, center.Y+y2, center.X+x, center.Y+y);
					}

					if (old.IsEmpty)
					{
						old = new Point(center.X + x, center.Y + y);
						continue;
					}
                
					// Kreis
					//if (i < 319)
					pg.DrawLine(p_ring, old.X, old.Y, center.X+x, center.Y+y);

					old = new Point(center.X + x, center.Y + y);
				}
			}
			else
			{
				for (double i = 40d; i < 320d; i=i+0.25d)
				{
					double angle = (double)i * (Math.PI / 180);
					int x = Convert.ToInt32((double)radius * Math.Sin(angle));
					int y = Convert.ToInt32((double)radius * Math.Cos(angle));

					// Skala
					if (Math.IEEERemainder((double)i-5d, 17.5d) == 0 || i == 319)
					{
						test++;
						int x2 = Convert.ToInt32(((double)radius-18d) * Math.Sin(angle));
						int y2 = Convert.ToInt32(((double)radius-18d) * Math.Cos(angle));
						pg.DrawLine(p, center.X+x2, center.Y+y2, center.X+x, center.Y+y);
					}
					else if (Math.IEEERemainder(((double)i+3.75), 17.5d) == 0)
					{
						test++;
						int x2 = Convert.ToInt32(((double)radius-7d) * Math.Sin(angle));
						int y2 = Convert.ToInt32(((double)radius-7d) * Math.Cos(angle));
						pg.DrawLine(p, center.X+x2, center.Y+y2, center.X+x, center.Y+y);
					}

					if (old.IsEmpty)
					{
						old = new Point(center.X + x, center.Y + y);
						continue;
					}
                
					// Kreis
					//if (i < 319)
					pg.DrawLine(p_ring, old.X, old.Y, center.X+x, center.Y+y);

					old = new Point(center.X + x, center.Y + y);
				}
			}
	
			// Schrift
			Font f = new Font("Arial", 11f, FontStyle.Bold);

			if (localstate.TrainType == TRAIN_TYPE.BR146_1 || localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				pg.DrawString("0", f, sb, 125f, 199f);
				pg.DrawString("20", f, sb, 96f, 157f);
				pg.DrawString("40", f, sb, 92f, 106f);
				pg.DrawString("60", f, sb, 117f, 63f);
				pg.DrawString("80", f, sb, 158f, 36f);
				pg.DrawString("100", f, sb, 204f, 36f);
				pg.DrawString("120", f, sb, 241f, 64f);			
				pg.DrawString("140", f, sb, 262f, 106f);
				pg.DrawString("160", f, sb, 260f, 157f);
				pg.DrawString("180", f, sb, 232f, 199f);
			}
			
			if (localstate.TrainType == TRAIN_TYPE.BR185 || localstate.TrainType == TRAIN_TYPE.BR189)
			{
				pg.DrawString("80", f, sb, 182f, 31f);
				pg.DrawString("0", f, sb, 125f, 199f);
				pg.DrawString("160", f, sb, 232f, 199f);
				
				pg.DrawString("10", f, sb, 105f, 180f);
				pg.DrawString("20", f, sb, 94f, 152f);
				pg.DrawString("30", f, sb, 90f, 123f);
				pg.DrawString("40", f, sb, 96f, 94f);
				pg.DrawString("50", f, sb, 110f, 69f);
				pg.DrawString("60", f, sb, 130f, 50f);
				pg.DrawString("70", f, sb, 154f, 38f);
				pg.DrawString("90", f, sb, 208f, 38f);
				pg.DrawString("100", f, sb, 226f, 50f);
				pg.DrawString("110", f, sb, 245f, 69f);
				pg.DrawString("120", f, sb, 260f, 94f);
				pg.DrawString("130", f, sb, 266f, 123f);
				pg.DrawString("140", f, sb, 262f, 152f);
				pg.DrawString("150", f, sb, 250f, 180f);
			}
			#endregion
		}

		public void DrawSpeedNeedle(ref Graphics pg)
		{
			//Graphics pg = graph_main.g;
			float valu = localstate.Geschwindigkeit;

			#region DRAW

			int radius = 85;

			// Brush Object
			SolidBrush sb = new SolidBrush(Color.White);
			if (valu > 160.9f && (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1 || localstate.TrainType == TRAIN_TYPE.BR146_1)) 
				sb = new SolidBrush(Color.Red);
			if (valu > 140.9f && (localstate.TrainType == TRAIN_TYPE.BR185 || localstate.TrainType == TRAIN_TYPE.BR189)) 
				sb = new SolidBrush(Color.Red);
			Point center = new Point(191,135);
			//-------------------------------------------------
			double i = ((double)valu+0.5d) * 1.5555555555555556d;
			if (localstate.TrainType == TRAIN_TYPE.BR185 || localstate.TrainType == TRAIN_TYPE.BR189) 
				i = i / 160d * 180d; // nur max 160 statt 180

			i = 320d - i;

			double angle = i * (Math.PI / 180);
			int x = Convert.ToInt32((double)radius * Math.Sin(angle));
			int y = Convert.ToInt32((double)radius * Math.Cos(angle));
			int x2 = Convert.ToInt32(((double)radius+30d) * Math.Sin(angle));
			int y2 = Convert.ToInt32(((double)radius+30d) * Math.Cos(angle));
            
			Pen p = new Pen(sb, 10);
			Pen p2 = new Pen(sb, 3);
			p.EndCap = LineCap.Triangle;
			p2.EndCap = LineCap.Triangle;

			// Zeiger
			pg.DrawLine(p2, center.X, center.Y, center.X+x2, center.Y+y2);
			pg.DrawLine(p, center.X, center.Y, center.X+x, center.Y+y);
			old_valu = valu;

			//--------------------------------------------------

			int circle_radius = 46;
			Brush b = new SolidBrush(Color.White);

			pg.FillEllipse(b, center.X - (circle_radius/2), center.Y - (circle_radius/2), 
				circle_radius,circle_radius);

			// LZB Zielgeschwindigkeit
			radius = 122;
																	  
			double k = ((double)localstate.LZB_SollGeschwindigkeit + 0.5d)* 1.5555555555555556d;
			if (localstate.TrainType == TRAIN_TYPE.BR185 || localstate.TrainType == TRAIN_TYPE.BR189) 
				k = k / 160d * 180d; // nur max 160 statt 180

			k = 320d - k;

			double angle2 = k * (Math.PI / 180);
			int x3 = Convert.ToInt32((double)radius * Math.Sin(angle2));
			int y3 = Convert.ToInt32((double)radius * Math.Cos(angle2));

			double angle2_2 = (double)(k+3)* (Math.PI / 180);
			int x3_2 = Convert.ToInt32((double)(radius+12) * Math.Sin(angle2_2));
			int y3_2 = Convert.ToInt32((double)(radius+12) * Math.Cos(angle2_2));

			double angle2_3 = (double)(k-3)* (Math.PI / 180);
			int x3_3 = Convert.ToInt32((double)(radius+12) * Math.Sin(angle2_3));
			int y3_3 = Convert.ToInt32((double)(radius+12) * Math.Cos(angle2_3));

			Point[] points = new Point[3];

			points[0] = new Point(center.X+x3, center.Y+y3);
			points[1] = new Point(center.X+x3_2, center.Y+y3_2);
			points[2] = new Point(center.X+x3_3, center.Y+y3_3);

			Brush b_red = new SolidBrush(Color.Red);			
			if (localstate.LM_LZB_�) pg.FillPolygon(b_red, points);

			// AFB Zielgeschwindigkeit
			if (localstate.TrainType != TRAIN_TYPE.DBpzfa766_1)
			{
				radius = 108;
					
				double k2 = ((double)localstate.AFB_SollGeschwindigkeit + 0.5d)* 1.5555555555555556d;

				if (localstate.TrainType == TRAIN_TYPE.BR185 || localstate.TrainType == TRAIN_TYPE.BR189) 
					k2 = k2 / 160d * 180d; // nur max 160 statt 180
                
				k2 = 320d - k2;

				double angle2_2_1 = k2 * (Math.PI / 180);
				int x3_2_1 = Convert.ToInt32((double)radius * Math.Sin(angle2_2_1));
				int y3_2_1 = Convert.ToInt32((double)radius * Math.Cos(angle2_2_1));

				double angle2_2_1a = k2 * (Math.PI / 180);
				int x3_2_1a = Convert.ToInt32((double)(radius+10) * Math.Sin(angle2_2_1a));
				int y3_2_1a = Convert.ToInt32((double)(radius+10) * Math.Cos(angle2_2_1a));

				double angle2_2_2 = (double)(k2+2)* (Math.PI / 180);
				int x3_2_2 = Convert.ToInt32((double)(radius+5) * Math.Sin(angle2_2_2));
				int y3_2_2 = Convert.ToInt32((double)(radius+5) * Math.Cos(angle2_2_2));

				double angle2_3_2 = (double)(k2-2)* (Math.PI / 180);
				int x3_3_2 = Convert.ToInt32((double)(radius+5) * Math.Sin(angle2_3_2));
				int y3_3_2 = Convert.ToInt32((double)(radius+5) * Math.Cos(angle2_3_2));

				points = new Point[3];

				points[0] = new Point(center.X+x3_2_1, center.Y+y3_2_1);
				points[1] = new Point(center.X+x3_2_2, center.Y+y3_2_2);
				points[2] = new Point(center.X+x3_3_2, center.Y+y3_3_2);

			
				b_red = new SolidBrush(Color.Yellow);
				Pen ppp = new Pen(b_red,1);
				pg.DrawPolygon(ppp, points);
				pg.FillPolygon(b_red, points);

				points[0] = new Point(center.X+x3_2_1a, center.Y+y3_2_1a);
				points[1] = new Point(center.X+x3_2_2, center.Y+y3_2_2);
				points[2] = new Point(center.X+x3_3_2, center.Y+y3_3_2);
				pg.DrawPolygon(ppp, points);
				pg.FillPolygon(b_red, points);
			}

			#endregion
		}

		public void UpdateScreen()
		{
			if (!something_changed /*&& !m_conf.DoubleBuffer*/)
				return;

			something_changed = false;

			if (USE_DOUBLE_BUFFER)
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

						if (localstate.LM_LZB_�)
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

		public void SetMaschinenTeil(ref Graphics pg)
		{
			if (localstate.Ein_Display)
			{
				DrawOberstrom_Spannung(ref pg);
			}
			else
			{
				DrawZugkraft_Fahrstufen(ref pg);
			}

			DrawMelder(ref pg);
		}

		public void DrawMelder(ref Graphics pg)
		{
			if (!CONNECTED || ZUSI_AT_TCP_SERVER)
			{
				M_Status1.Draw(ref pg);
			}
		}

		public void DrawOberstrom_Spannung(ref Graphics pg)
		{
			Brush b = new SolidBrush(System.Drawing.SystemColors.Control);
			pg.FillRectangle(b,325,0,304,264);

			//---SPANNUNG--------------------------------------------
			DrawSpannung(ref pg);

			//---OBERSTROM-------------------------------------------
			DrawOberstrom(ref pg);

			//---ZUGKRAFT--------------------------------------------
			DrawZugkraftSkala(ref pg);

			DrawEindisplaySchrift(ref pg);
			}

		public void DrawSpannung(ref Graphics pg)
		{
			// gro�es Spannungsfeld
			Brush sb = new SolidBrush(MMI_BLUE);
			int height = Convert.ToInt32((localstate.Spannung-10d) * (16.9d));
			pg.FillRectangle(sb, 365, 190-height-1, 40, height-1);

			// gro�er Rahmen um Spannung
			pg.SmoothingMode = SmoothingMode.None;
			sb = new SolidBrush(SystemColors.ControlDarkDark);

			Pen p = new Pen(sb, 2);
			pg.DrawLine(p, 365, 20, 405, 20);
			pg.DrawLine(p, 365, 20, 365, 191);

			p = new Pen(Brushes.WhiteSmoke, 2);
			pg.DrawLine(p, 365, 191, 405 , 191);
			pg.DrawLine(p, 405, 20, 405, 191);
			

			// kleines Spannungsfeld
			if (localstate.Spannung > 1f)
			{
				Brush sb2 = new SolidBrush(MMI_BLUE);
				pg.FillRectangle(sb2, 365, 195, 40, 20);
			}

			// kleiner Rahmen um Spannung
			p = new Pen(sb, 2);
			pg.DrawLine(p, 365, 195, 365, 215);
			pg.DrawLine(p, 365, 195, 405, 195);

			p = new Pen(Brushes.WhiteSmoke, 2);
			pg.DrawLine(p, 365, 215, 405, 215);
			pg.DrawLine(p, 405, 195, 405, 215);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (!localstate.LM_HS)
			{
				if (localstate.Spannung < 1f)
					pg.DrawString("Ein", f, sb, 370f, 197f);
				else
					pg.DrawString("Ein", f, Brushes.White, 370f, 197f);
			}
			else
			{	
				if (localstate.Spannung < 1f)
                    pg.DrawString("Aus", f, sb, 369f, 197f);
				else
					pg.DrawString("Aus", f, Brushes.White, 369f, 197f);
			}

			p = new Pen(sb, 1);

			// Text
			f = new Font("Tahoma", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kV", f, sb, 340f, 2f);

			// Skala
			f = new Font("Tahoma", 3, FontStyle.Regular, GraphicsUnit.Millimeter);
			for (int i = 0; i < 11; i++)
			{
				int ypos = Convert.ToInt32((double)i*16.9d+20d);
				if (i == 0 || i == 5 || i == 10)
					pg.DrawLine(p, 355, ypos, 365, ypos); 
				else
					pg.DrawLine(p, 360, ypos, 365, ypos); 
			}
			pg.DrawString("20", f, sb, 340, 14); 
			pg.DrawString("15", f, sb, 340, 98); 
			pg.DrawString("10", f, sb, 340, 183); 

			pg.SmoothingMode = SMOOTHING_MODE;
		}

		public void DrawOberstrom(ref Graphics pg)
		{
			// Stromfeld
			Brush sb = new SolidBrush(MMI_BLUE);
			int height = Convert.ToInt32(localstate.Oberstrom);
			height = Convert.ToInt32((double)height * (182d / 600d));
			pg.FillRectangle(sb, 465, 210-height-1, 40, height-1);

			pg.SmoothingMode = SmoothingMode.None;
			// Rahmen um Stromfeld
			sb = new SolidBrush(SystemColors.ControlDarkDark);
			Pen p = new Pen(sb, 2);
			//pg.DrawRectangle(p, 445, 20, 40, 189);
			pg.DrawLine(p, 465, 20, 505, 20);
			pg.DrawLine(p, 465, 20, 465, 209);

			p = new Pen(Brushes.WhiteSmoke, 2);
			pg.DrawLine(p, 465, 209, 505, 209);
			pg.DrawLine(p, 505, 20, 505, 209);
			
			pg.SmoothingMode = SMOOTHING_MODE;

			p = new Pen(sb, 1);

			//Skala
			Font f = new Font("Tahoma", 3, FontStyle.Regular, GraphicsUnit.Millimeter);
			for (int i = 0; i < 13; i++)
			{
				string val = (600 - i * 50).ToString();
				if (Math.IEEERemainder(i, 2) != 0)
				{
					val = "";
					pg.DrawLine(p, 465, i*15+29, 460, i*15+29);
				}
				else
				{
					pg.DrawLine(p, 465, i*15+29, 455, i*15+29);
				}
				if (val == "0")
					pg.DrawString(val, f, sb, 446, i*15+29-7); 
				else
					pg.DrawString(val, f, sb, 434, i*15+29-7); 
			}

			// Text
			f = new Font("Tahoma", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("A", f, sb, 442f, 2f);
		}

		public void DrawZugkraftSkala(ref Graphics pg)
		{
			// Stromfeld
			Brush sb_org = new SolidBrush(Color.Orange);

			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
			{
				sb_org = new SolidBrush(Color.FromArgb(179, 177, 142));
			}

			Brush sb_blu = new SolidBrush(MMI_BLUE);
			int height = 0;

			
			if (localstate.Zugkraft > 0)
			{
				// Fahren
				height = Convert.ToInt32(Math.Round((double)localstate.Zugkraft * (189d / 75d)));
				pg.FillRectangle(sb_blu, 565, 209-height, 40, height);
			}
			else if (localstate.Zugkraft < 0)
			{
				// Bremsen
				height = Convert.ToInt32(Math.Round((double)-localstate.Zugkraft * (189d / 150d)));
				pg.FillRectangle(sb_org, 565, 209-height, 40, height);
			}

			// Rahmen
			pg.SmoothingMode = SmoothingMode.None;
			Brush sb = new SolidBrush(SystemColors.ControlDarkDark);
			Pen p = new Pen(sb, 2);
			pg.DrawLine(p, 565, 20, 605, 20);
			pg.DrawLine(p, 565, 20, 565, 209);

			p = new Pen(Brushes.WhiteSmoke, 2);
			pg.DrawLine(p, 565, 209, 605, 209);
			pg.DrawLine(p, 605, 20, 605, 209);

			p = new Pen(sb, 1);

			// Skalen
			Font f = new Font("Tahoma", 3, FontStyle.Regular, GraphicsUnit.Millimeter);
			for (int i = 0; i < 16; i++)
			{
				string val = (Math.Round(15d-(double)i)/2d*10d).ToString();
				string val_neg = (Math.Round(15d-(double)i)/2d*20d).ToString();
				if (val == "25" || val == "50" || val == "75" || val == "0")
				{
					pg.DrawLine(p, 565, (float)i*12.6f+20f, 555, (float)i*12.6f+20f);
					if (val == "0")
					{
						pg.DrawString(val, f, sb, 546, (float)i*12.6f+20f-6.2f);
					}
					else if (val_neg.Length < 3 || localstate.Zugkraft >= 0)
					{
						if (localstate.Zugkraft < 0) 
							val = val_neg;
						pg.DrawString(val, f, sb, 540, (float)i*12.6f+20f-6.2f); 
					}
					else
					{
						if (localstate.Zugkraft < 0) 
							val = val_neg;
						pg.DrawString(val, f, sb, 534, (float)i*12.6f+20f-6.2f); 
					}
				}
				else
				{
					pg.DrawLine(p, 565, (float)i*12.6f+20f, 560, (float)i*12.6f+20f);
				}
				
			}

			// Text
			f = new Font("Tahoma", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, sb, 537f, 2f);
			if (localstate.Zugkraft >= 0)
                pg.DrawString("FM", f, sb, 575f, 2f);
			else
				pg.DrawString("Lok", f, sb, 572f, 2f);
			pg.SmoothingMode = SMOOTHING_MODE;


			// kleines blaues Dreieck
			p = new Pen(sb_blu, 1);
			Point[] points = new Point[5];

			points[0] = new Point(565, 209-height);
			points[1] = new Point(552, 209-height+5);
			points[4] = new Point(552, 209-height-5);
			points[2] = new Point(550, 209-height+5);
			points[3] = new Point(550, 209-height-5);
                
			if (localstate.Zugkraft >=0)
                pg.FillPolygon(sb_blu, points);
			else
				pg.FillPolygon(sb_org, points);
		}

		public void DrawEindisplaySchrift(ref Graphics pg)
		{
			pg.SmoothingMode = SmoothingMode.None;

			// Stromfeld
			Brush sb_lg = new SolidBrush(Color.LightGray);
			Brush sb_wh = new SolidBrush(Color.WhiteSmoke);
			Brush sb_bl = new SolidBrush(SystemColors.ControlDarkDark);
			Brush sb_mb = new SolidBrush(MMI_BLUE);
			Brush sb_ye = new SolidBrush(Color.Yellow);

			Pen p_wh = new Pen(sb_wh, 2);
			Pen p_lg = new Pen(sb_lg, 2);
			Pen p_bl = new Pen(sb_bl, 2);
			Pen p_dg = new Pen(Brushes.DarkGray, 2);

			// Statusmelder
			if (!CONNECTED)
			{
				pg.FillRectangle(sb_mb, 326, 230, 182, 30);
				Font f = new Font("Tahoma", 4, FontStyle.Regular, GraphicsUnit.Millimeter);
				pg.DrawString("Keine Verbindung zu TCP Server", f, sb_wh, 332f, 236f);
			}
			if (ZUSI_AT_TCP_SERVER)
			{
				pg.FillRectangle(sb_mb, 326, 230, 182, 30);
				Font f = new Font("Tahoma", 4, FontStyle.Regular, GraphicsUnit.Millimeter);
				pg.DrawString("Zusi mit TCP Server verbunden", f, sb_wh, 332f, 236f);
			}

			// St�rmelder
			//pg.FillRectangle(sb_ye, 508, 230, 120, 30);

			// obere Kante
			pg.DrawLine(p_wh, 326, 230, 628, 230);
			pg.DrawLine(p_dg, 326, 232, 628, 232);

			// rechte Kante
			pg.DrawLine(p_dg, 628, 232, 628, 262);
			pg.DrawLine(p_wh, 626, 233, 626, 262);

			// linke Kante
			pg.DrawLine(p_dg, 328, 232, 328, 262);
			pg.DrawLine(p_wh, 326, 230, 326, 262);

			// Trenner
			pg.DrawLine(p_dg, 508, 232, 508, 262);
			pg.DrawLine(p_wh, 507, 233, 507, 262);

			// untere Kante
			pg.DrawLine(p_wh, 329, 260, 627, 260);
			pg.DrawLine(p_dg, 328, 262, 628, 262);

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		public void DrawLZBAnzeige(ref Graphics pg)
		{
			Font f = new Font("ZusiDigital standard", 11, FontStyle.Bold);
			Brush b = new SolidBrush(Color.Red);
			Brush sb = new SolidBrush(Color.Black);

			if (localstate.LM_LZB_�)
			{
				//Graphics pg = graph_main.g;
				string weg = (Convert.ToInt32(localstate.LZB_ZielWeg)).ToString();
				int pos = weg.IndexOf("-");
				if (pos != -1) weg = weg.Remove(pos, 1);
				if (weg.Length < 2) weg = "000" + weg;
				else if (weg.Length < 3) weg = "00" + weg;
				else if (weg.Length < 4) weg = "0" + weg;
			
				//Pen p = new Pen(b, 1);
			
				// Digital Anzeige: WEG
				pg.FillRectangle(sb, 5, 242, 45, 15);
				pg.DrawString(weg, f, b, 4, 62);

				// Balken: WEG

				// y = [87; 140] |y| = 53
				int position = 227;

				pg.FillRectangle(sb, 40, 87, 10, 140);

				if (localstate.LZB_ZielWeg > 4000f)
				{
					localstate.LZB_ZielWeg = 4000f;
				}

				if (localstate.LZB_ZielWeg >= 1000f)
				{
					position = 87 + Convert.ToInt32(((double)(4000 - localstate.LZB_ZielWeg) / 50d));
				}
				else if (localstate.LZB_ZielWeg >= 250f)
				{
					position = 147 + Convert.ToInt32(((double)(1000 - localstate.LZB_ZielWeg) / 12.5d));
				}
				else if (localstate.LZB_ZielWeg >= 100f)
				{
					position = 207 + Convert.ToInt32(((double)(250 - localstate.LZB_ZielWeg) / 15d));
				}
				else if (localstate.LZB_ZielWeg > 0f)
				{
					position = Convert.ToInt32(227f - localstate.LZB_ZielWeg / 10f);
				}

				pg.FillRectangle(b, 40, position, 10, 140-(position-87));

				// LZB Zielgeschwindigkeit
				int v = Convert.ToInt32(localstate.LZB_ZielGeschwindigkeit);
				int v1 = Convert.ToInt32(Math.Floor((double)v / 100d));
				int v2 = Convert.ToInt32(Math.Floor((double)(v - v1*100) / 10d));
				int v3 = v - v1*100 - v2*10;

				f = new Font("ZusiDigital standard", 14, FontStyle.Bold);

				string h = "";
				if (v1 > 0) h = v1.ToString();
				pg.DrawString(h, f, b, 160f, 221f);
				if (v2 > 0 || v1 > 0) h = v2.ToString();
				else h = "";
				pg.DrawString(h, f, b, 180f, 221f);
				//if (localstate.LM_LZB_�) 
				h = v3.ToString();
				//else h = "";
				pg.DrawString(h, f, b, 200f, 221f);


				DrawLines(ref pg);
			}
			if (localstate.LM_GNT_�)
			{
				f = new Font("ZusiDigital standard", 14, FontStyle.Bold);
				pg.DrawString("G", f, b, 160f, 218f);
				pg.DrawString("N", f, b, 180f, 218f);
				pg.DrawString("T", f, b, 202f, 218f);
			}
		}
		public void DrawGeschwindigkeit(ref Graphics pg)
		{
			float valu = localstate.Geschwindigkeit;

			if (true)
			{
				DrawSpeedOMeter(ref pg);
				DrawSpeedNeedle(ref pg);

				// --- digitale V Anzeige -------------------------
				//Graphics pg = graph_main.g;
				Font f = new Font("Tahoma", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
				string txt = Convert.ToInt32(Math.Round(valu)).ToString();

				if (valu < 9.5f)
				{
					pg.DrawString(txt, f, Brushes.Black, 184f, 126f);
				}
				else if (valu < 99.5f)
				{
					pg.DrawString(txt, f, Brushes.Black, 179f, 126f);
				}
				else
				{
					pg.DrawString(txt, f, Brushes.Black, 173f, 126f);
				}

				// --- km/h TEXT --------------------------------
				f = new Font("Tahoma", 5, FontStyle.Regular, GraphicsUnit.Millimeter);
				pg.DrawString("km/h", f, Brushes.WhiteSmoke, 167f, 170f);
			}

			/*
			if (!this.IsDisposed)
			{
				graph_main.Render(graph_main.g);
			}
			*/
		}
		public void DrawUhr(ref Graphics pg)
		{
			if (!localstate.SHOW_CLOCK) return;
			string s = "", min = "", sec = "";
			if (vtime.Hour < 10)
				s += "0"+vtime.Hour;
			else
				s += vtime.Hour;

			if (vtime.Minute < 10)
				min += "0"+vtime.Minute;
			else
				min += vtime.Minute;

			if (vtime.Second < 10)
				sec += "0"+vtime.Second;
			else
				sec += vtime.Second;

			if (DrawDots)
			{
				s += ":"; min += ":";
			}

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString(s, f, Brushes.WhiteSmoke, 260, 4);
			pg.DrawString(min, f, Brushes.WhiteSmoke, 280, 4);
			pg.DrawString(sec, f, Brushes.WhiteSmoke, 300, 4);

		}
		public void SetButtons()
		{
			l_Button1.Text = "";
			l_Button2.Text = "";
			l_Button3.Text = "";
			l_Button5.Text = "";
			l_Button7.Text = "";
			l_Button8.Text = "";
			l_Button9.Text = "";

			l_Button0.Text = "Netz";

			l_Button4.Text = "";

			if (localstate.Ein_Display)
                l_Button6.Text = "Normale Anzeige";
			else
				l_Button6.Text = "Ein Display";
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
		}
		
		public void UpdateLZB(ref Graphics graph_main)
		{
			
		}

		public void UpdateMaschLM(ref Graphics graph_main)
		{
			// HS und ZS
			if (localstate.LM_HS)
			{
				HS.Draw(ref graph_main);
				ZS.Draw(ref graph_main);
			}
			/*else 
			{
				HS.Clear(ref graph_main);
				if (localstate.TrainType == TRAIN_TYPE.BR146_1 || localstate.TrainType == TRAIN_TYPE.DBpzfa766_1)
                    ZS.Clear(ref graph_main);
				else
					ZS.Draw(ref graph_main);
			} */

			// SIFA
			if (localstate.LM_Sifa)
				SIFA.Draw(ref graph_main);
			/*else 
				SIFA.Clear(ref graph_main);*/

			// T�R
			if (localstate.T�rschliesung != T�RSCHlIESSUNG.KEINE)
			{
				if (localstate.T�rschliesung == T�RSCHlIESSUNG.SAT || localstate.T�rschliesung == T�RSCHlIESSUNG.TAV)
				{
					// TAV oder SAT
					T�R.Text1 = "TAV";
					if (!localstate.LM_T�R)
						T�R.Draw(ref graph_main);
					/*else
						T�R.Clear(ref graph_main);*/
				}
				else
				{
					// TB0 oder ICE
					T�R.Text1 = "T";
					if (localstate.LM_T�R)
						T�R.Draw(ref graph_main);
					/*else
						T�R.Clear(ref graph_main);*/
				}
			}
			/*else
				T�R.Clear(ref graph_main);*/

			// NB� EP
			if (localstate.Geschwindigkeit < 19f && localstate.NB�_Aktiv && IsCONNECTED)
				localstate.LM_NB�_EP = true;
			else
				localstate.LM_NB�_EP = false;

			if (localstate.LM_NB�_EP)
			{
				if (localstate.TrainType == TRAIN_TYPE.BR146_1 || localstate.TrainType == TRAIN_TYPE.DBpzfa766_1) 
					NB�_EP.Draw(ref graph_main);
			}

			/*else
				NB�_EP.Clear(ref graph_main);*/
		}
		public void UpdateINTEGRA(ref Graphics graph_main)
		{
			if (localstate.LM_INTEGRA_GELB)
				INTEGRA_GELB.Draw(ref graph_main);
			else 
				INTEGRA_GELB.Clear(ref graph_main);

			if (localstate.LM_INTEGRA_ROT)
				INTEGRA_ROT.Draw(ref graph_main);
			else 
				INTEGRA_ROT.Clear(ref graph_main);
		}

		private void BR185Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (PREVENT_DRAW) return;

			if (m_backBuffer == null && USE_DOUBLE_BUFFER)
			{
				m_backBuffer= new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			}

			
			if (USE_DOUBLE_BUFFER)
				g = Graphics.FromImage(m_backBuffer);
			else
				g = e.Graphics;

			//Paint your graphics on g here

			if (ZUSI_AT_TCP_SERVER)
			{
				M_Status1.Text = "Zusi mit TCP Server verbunden";
			}
			else
			{
				M_Status1.Text = "Keine Verbindung zu TCP Server";
			}
            
			g.SmoothingMode = SMOOTHING_MODE;
			g.TextRenderingHint = TEXT_MODE;

			g.Clear(Color.Black);

			MoveLM();

			MoveTextZeile();

			foreach(Square s in LMs)
			{
				if (s != null) s.Draw(ref g);
			}

			foreach(Line s in LINEs)
			{
				if (s != null) s.Draw(ref g);
			}

			if (!localstate.LM_LZB_�)
				UpdatePZB(ref g);
			else
				UpdateLZB(ref g);

			if (localstate.Netz == NETZ.SBB)
				UpdateINTEGRA(ref g);

			UpdateMaschLM(ref g);  
			SetMaschinenTeil(ref g);

			DrawLines(ref g);

			DrawLZBAnzeige(ref g);

			DrawGeschwindigkeit(ref g);
			

			DrawUhr(ref g);

			if (isEmbeded)
			{
				l_Button0.Text = "";
				l_Button0.Click -= new System.EventHandler(this.Button_0_Pressed);
				l_Button6.Click -= new System.EventHandler(this.Button_6_Pressed);
			}

			if (USE_DOUBLE_BUFFER)
			{
				//g.Dispose();

				//Copy the back buffer to the screen
				e.Graphics.DrawImageUnscaled(m_backBuffer,0,0);
			}

			//graph_main.Render(this.CreateGraphics());
		}

		private void InitTrainControl()
		{
			LMs = new Square[7];
			LMs.Initialize();
		}
		private void MoveLM()
		{
			if (localstate.PZB_System == ZUGBEEINFLUSSUNG.SIGNUM)
				MoveLM_INTEGRA();
			else if (localstate.PZB_System != ZUGBEEINFLUSSUNG.NONE)
				MoveLM_PZB_LZB();
		}
		
		private void MoveLM_INTEGRA()
		{
			int pos = 0;
			if (localstate.LM_INTEGRA_GELB != localstate.LM_INTEGRA_GELB2)
			{
				if (localstate.LM_INTEGRA_GELB)
				{
					FreeLM(INTEGRA_GELB);
					pos = 0;
					INTEGRA_GELB.Point1 = new Point(57+GetLocation(pos),263);
					INTEGRA_GELB.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
					LMs[pos] = INTEGRA_GELB;
				}
				else
				{
					FreeLM(INTEGRA_GELB);
				}
				localstate.LM_INTEGRA_GELB2 = localstate.LM_INTEGRA_GELB;
			}

			if (localstate.LM_INTEGRA_ROT != localstate.LM_INTEGRA_ROT2)
			{
				if (localstate.LM_INTEGRA_ROT)
				{
					FreeLM(INTEGRA_ROT);
					pos = 1;
					INTEGRA_ROT.Point1 = new Point(57+GetLocation(pos),263);
					INTEGRA_ROT.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
					LMs[pos] = INTEGRA_ROT;
				}
				else
				{
					FreeLM(INTEGRA_ROT);
				}
				localstate.LM_INTEGRA_ROT2 = localstate.LM_INTEGRA_ROT;
			}

			FreeLM(ZUB_BLAU);
			pos = 5;
			ZUB_BLAU.Point1 = new Point(57+GetLocation(pos),263);
			ZUB_BLAU.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
			LMs[pos] = ZUB_BLAU;
		}
		private void MoveLM_PZB_LZB()
		{
			//localstate.Zugart = "85";

			if (localstate.TrainType != TRAIN_TYPE.DBpzfa766_1)
			{
				if (localstate.LM_LZB_B || localstate.LM_GNT_B)
					LMs[0] = LZB_B;
				else
					LMs[0] = null;
				LMs[5] = PZB_AKTIV;
			}
			else
			{
				if (LMs[0] == LZB_B) LMs[0] = null;
				if (LMs[5] == PZB_AKTIV) LMs[5] = null;
			}

			// R+6 Point(286,263)				39
			// R+5 Point(248,263) == "LZB/PZB"	38
			// R+4 Point(210,263)				38	
			// R+3 Point(172,263)				38
			// R+2 Point(134,263)				38
			// R+1 Point(96, 263)				38
			// R   Point(57,263)  == LZB "B"	39

			int pos = 0;
			int zugart = 1; // Zugart
			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1) zugart = 0;

			if (localstate.Zugart == "95" || localstate.Zugart == "85") // OBERE ZUGART
			{
				if (localstate.LM_Zugart_O != localstate.LM_Zugart_O2)
				{
					if (localstate.LM_Zugart_O)
					{
						PZB_ZA_O.Point1 = new Point(57+GetLocation(zugart),263);
						PZB_ZA_O.Point2 = new Point(57+GetLocation(zugart)+GetWidth(zugart),263+47);
						LMs[zugart] = PZB_ZA_O;
					}
					else
					{
						LMs[zugart] = null;
					}
					localstate.LM_Zugart_O2 = localstate.LM_Zugart_O;
				}

				if (localstate.LM_Zugart_M != localstate.LM_Zugart_M2)
				{
					if (localstate.LM_Zugart_M)
					{
						FreeLM(PZB_ZA_M);
						// such n�chst freien
						pos = GetNextFree();
						PZB_ZA_M.Point1 = new Point(57+GetLocation(pos),263);
						PZB_ZA_M.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_ZA_M;
					}
					else
					{
						FreeLM(PZB_ZA_M);
					}
					localstate.LM_Zugart_M2 = localstate.LM_Zugart_M;
				}
			}
			else if (localstate.Zugart == "75" || localstate.Zugart == "70") // MITTLERE ZUGART
			{
				if (localstate.LM_Zugart_M != localstate.LM_Zugart_M2)
				{
					if (localstate.LM_Zugart_M)
					{
						PZB_ZA_M.Point1 = new Point(57+GetLocation(zugart),263);
						PZB_ZA_M.Point2 = new Point(57+GetLocation(zugart)+GetWidth(zugart),263+47);
						LMs[zugart] = PZB_ZA_M;
					}
					else
					{
						LMs[zugart] = null;
					}
					localstate.LM_Zugart_M2 = localstate.LM_Zugart_M;
				}

				if (localstate.LM_Zugart_O != localstate.LM_Zugart_O2)
				{
					if (localstate.LM_Zugart_O)
					{
						FreeLM(PZB_ZA_O);
						pos = GetNextFree();
						PZB_ZA_O.Point1 = new Point(57+GetLocation(pos),263);
						PZB_ZA_O.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_ZA_O;
					}
					else
					{
						FreeLM(PZB_ZA_O);
					}
					localstate.LM_Zugart_O2 = localstate.LM_Zugart_O;
				}
			}
			else if (localstate.Zugart == "60" || localstate.Zugart == "55") // UNTERE ZUGART
			{
				if (localstate.LM_Zugart_U)
				{
					PZB_ZA_U.Point1 = new Point(57+GetLocation(zugart),263);
					PZB_ZA_U.Point2 = new Point(57+GetLocation(zugart)+GetWidth(zugart),263+47);
					LMs[zugart] = PZB_ZA_U;
				}
				else
				{
					LMs[zugart] = null;
				}

				if (localstate.LM_Zugart_O != localstate.LM_Zugart_O2)
				{
					if (localstate.LM_Zugart_O)
					{
						FreeLM(PZB_ZA_O);
						pos = GetNextFree();
						PZB_ZA_O.Point1 = new Point(57+GetLocation(pos),263);
						PZB_ZA_O.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_ZA_O;
					}
					else
					{
						FreeLM(PZB_ZA_O);
					}
					localstate.LM_Zugart_O2 = localstate.LM_Zugart_O;
				}
				if (localstate.LM_Zugart_M != localstate.LM_Zugart_M2)
				{
					if (localstate.LM_Zugart_M)
					{
						// such n�chst freien
						FreeLM(PZB_ZA_M);
						pos = GetNextFree();
						PZB_ZA_M.Point1 = new Point(57+GetLocation(pos),263);
						PZB_ZA_M.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_ZA_M;
					}
					else
					{
						FreeLM(PZB_ZA_M);
					}
					localstate.LM_Zugart_M2 = localstate.LM_Zugart_M;
				}
			}

			if (!localstate.LM_LZB_S && !localstate.LM_LZB_�)
			{
				if (localstate.LM_1000Hz != localstate.LM_1000Hz2)
				{
					if (localstate.LM_1000Hz)
					{
						FreeLM(PZB_1000);
						pos = GetNextFree();
						PZB_1000.Point1 = new Point(57+GetLocation(pos),263);
						PZB_1000.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_1000;
					}
					else
					{
						FreeLM(PZB_1000);
					}
					localstate.LM_1000Hz2 = localstate.LM_1000Hz;
				}
			}

			if (!localstate.LM_LZB_S && !localstate.LM_LZB_�)
			{
				if (localstate.LM_500Hz != localstate.LM_500Hz2)
				{
					if (localstate.LM_500Hz)
					{
						FreeLM(PZB_500);
						pos = GetNextFree();
						PZB_500.Point1 = new Point(57+GetLocation(pos),263);
						PZB_500.Point2 = new Point(57+GetLocation(pos)+GetWidth(pos),263+47);
						LMs[pos] = PZB_500;
					}
					else
					{
						FreeLM(PZB_500);
					}
					localstate.LM_500Hz2 = localstate.LM_500Hz;
				}
			}


			if (!localstate.LM_LZB_�)
			{
				if (localstate.LM_Befehl != localstate.LM_Befehl2)
				{
					if (localstate.LM_Befehl)
					{
						FreeLM(PZB_Befehl);
						pos = GetNextFree();
						PZB_Befehl = new Square(this, new Point(57+GetLocation(pos),263), new Point(57+GetLocation(pos)+GetWidth(pos), 263+47), "Befehl", Color.WhiteSmoke, false, 0, false, "");
						LMs[pos] = PZB_Befehl;
					}
					else
					{
						FreeLM(PZB_Befehl);
					}
					localstate.LM_Befehl2 = localstate.LM_Befehl;
				}
			}

			if (localstate.LM_LZB_S != localstate.LM_LZB_S2)
			{
				if (localstate.LM_LZB_S)
				{
					FreeLM(LZB_S);
					pos = GetNextFree();
					LZB_S = new Square(this, new Point(57+GetLocation(pos),263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "S", Color.Red, false, 0, false, "");
					LMs[pos] = LZB_S;
				}
				else
				{
					FreeLM(LZB_S);
				}
				localstate.LM_LZB_S2 = localstate.LM_LZB_S;
			}

			if (localstate.LM_LZB_G != localstate.LM_LZB_G2)
			{
				if (localstate.LM_LZB_G)
				{
					FreeLM(LZB_G);
					pos = GetNextFree();
					LZB_G = new Square(this, new Point(57+GetLocation(pos), 263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "G", Color.Red, false, 0, false, "");
					LMs[pos] = LZB_G;
				}
				else
				{
					FreeLM(LZB_G);
				}
				localstate.LM_LZB_G2 = localstate.LM_LZB_G;
			}
			
			if (localstate.LM_LZB_H != localstate.LM_LZB_H2)
			{
				if (localstate.LM_LZB_H)
				{
					FreeLM(LZB_H);
					pos = GetNextFree();
					LZB_H = new Square(this, new Point(57+GetLocation(pos),263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "H", Color.Red, false, 0, false, "");
					LMs[pos] = LZB_H;
				}
				else
				{
					FreeLM(LZB_H);
				}
				localstate.LM_LZB_H2 = localstate.LM_LZB_H;
			}

			if (localstate.LM_LZB_ENDE != localstate.LM_LZB_ENDE2)
			{
				if (localstate.LM_LZB_ENDE)
				{
					FreeLM(LZB_Ende);
					pos = GetNextFree();
					LZB_Ende = new Square(this, new Point(57+GetLocation(pos),263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "LZB", Color.Yellow, false, 0, true, "Ende");
					LMs[pos] = LZB_Ende;
				}
				else
				{
					FreeLM(LZB_Ende);
				}
				localstate.LM_LZB_ENDE2 = localstate.LM_LZB_ENDE;
			}

			if (localstate.LM_GNT_� != localstate.LM_GNT_�2)
			{
				if (localstate.LM_GNT_�)
				{
					PZB_AKTIV.Text1 = "GNT";
					PZB_AKTIV.Text2 = "PZB";
					PZB_AKTIV.TwoLines = true;
				}
				else
				{
					PZB_AKTIV.Text1 = "PZB";
					PZB_AKTIV.TwoLines = false;
				}
				localstate.LM_GNT_�2 = localstate.LM_GNT_�;
			}

			if (localstate.LM_GNT_S != localstate.LM_GNT_S2)
			{
				if (localstate.LM_GNT_S)
				{
					FreeLM(LZB_S);
					pos = GetNextFree();
					LZB_S = new Square(this, new Point(57+GetLocation(pos),263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "S", Color.Red, false, 0, false, "");
					LMs[pos] = LZB_S;
				}
				else
				{
					FreeLM(LZB_S);
				}
				localstate.LM_GNT_S2 = localstate.LM_GNT_S;
			}

			if (localstate.LM_GNT_G != localstate.LM_GNT_G2)
			{
				if (localstate.LM_GNT_G)
				{
					FreeLM(LZB_G);
					pos = GetNextFree();
					LZB_G = new Square(this, new Point(57+GetLocation(pos), 263), new Point(57+GetLocation(pos)+GetWidth(pos),263+47), "G", Color.Red, false, 0, false, "");
					LMs[pos] = LZB_G;
				}
				else
				{
					FreeLM(LZB_G);
				}
				localstate.LM_GNT_G2 = localstate.LM_GNT_G;
			}
		}

		private void MoveTextZeile()
		{
			int pos = 0;
			if (!localstate.LM_GNT_�)
			{
				if (localstate.LM_LZB_S)
				{
					FreeTextZeile(LZB_S_TEXT);
					pos = GetNextFreeTextZeile();
					LZB_S_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "LZB/PZB-Zwangsbremsung", Color.Red, false, 0);
					LINEs[pos] = LZB_S_TEXT;
					return;
				}
				else
				{
					FreeTextZeile(LZB_S_TEXT);
				}

				if (localstate.LM_LZB_G)
				{
					FreeTextZeile(LZB_G_TEXT);
					pos = GetNextFreeTextZeile();
					LZB_G_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "Geschwindigkeit reduzieren", Color.WhiteSmoke, false, 0);					
					LINEs[pos] = LZB_G_TEXT;
				}
				else
				{
					FreeTextZeile(LZB_G_TEXT);
				}
			}
			else
			{
				if (localstate.LM_GNT_S)
				{
					FreeTextZeile(LZB_S_TEXT);
					pos = GetNextFreeTextZeile();
					LZB_S_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "GNT/PZB-Zwangsbremsung", Color.Red, false, 0);
					LINEs[pos] = LZB_S_TEXT;
				}
				else
				{
					FreeTextZeile(LZB_S_TEXT);
				}

				if (localstate.LM_GNT_G)
				{
					FreeTextZeile(LZB_G_TEXT);
					pos = GetNextFreeTextZeile();
					LZB_G_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "Geschwindigkeit reduzieren", Color.WhiteSmoke, false, 0);					
					LINEs[pos] = LZB_G_TEXT;
				}
				else
				{
					FreeTextZeile(LZB_G_TEXT);
				}
			}

			if (localstate.LM_1000Hz)
			{
				FreeTextZeile(PZB_1000_TEXT);
				pos = GetNextFreeTextZeile();
				PZB_1000_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "�berwachungsgeschwindigkeit: "+localstate.Zugart+"km/h", Color.Yellow, false, 0);
				LINEs[pos] = PZB_1000_TEXT;
			}
			else
			{
				FreeTextZeile(PZB_1000_TEXT);
			}

			if (localstate.LM_500Hz)
			{
				FreeTextZeile(PZB_500_TEXT);
				pos = GetNextFreeTextZeile();
				PZB_500_TEXT = new Line(this, new Point(57,311+GetLocationTextZeile(pos)), new Point(57+268,311+26+GetLocationTextZeile(pos)), "�berwachungsgeschwindigkeit: 45km/h", Color.Red, false, 0);
				LINEs[pos] = PZB_500_TEXT;
			}
			else
			{
				FreeTextZeile(PZB_500_TEXT);
			}

			
		}

		private void MoveZugkraft()
		{
			while (Math.Abs(localstate.Zugkraft_Thread - localstate.Zugkraft) > 15f)
			{
				if ((localstate.Zugkraft_Thread - localstate.Zugkraft) > 0) // pos
				{
					Monitor.Enter(localstate);
					localstate.Zugkraft_Thread -= 15f;
					Monitor.Exit(localstate);
				}
				else // neg
				{
					Monitor.Enter(localstate);
					localstate.Zugkraft_Thread += 15f;
					Monitor.Exit(localstate);
				}
				something_changed = true;
				Thread.Sleep(40);
			}
			localstate.Zugkraft_Thread = localstate.Zugkraft;
		}
		private void FreeLM(Square search)
		{
			if (search == null) return;

			for (int i = 0; i < 7; i++)
			{
				try
				{
					if (LMs[i] == null) continue;

					Square square = (Square)LMs[i];

					if (square.Text1 == search.Text1 && square.Text2 == search.Text2)
					{
						LMs[i] = null;
					}
				}
				catch (Exception) {continue;}
			}

			search = null;
		}
		private void FreeTextZeile(Line search)
		{
			if (search == null) return;

			for (int i = 0; i < 3; i++)
			{
				try
				{
					if (LINEs[i] == null) continue;

					Line line = (Line)LINEs[i];

					if (line.Text == search.Text)
					{
						LINEs[i] = null;
					}
				}
				catch (Exception) {continue;}
			}

			search = null;
		}
		private int GetNextFree()
		{
			int pos = 2;
			if (localstate.LM_LZB_�) pos = 1;
			if (localstate.TrainType == TRAIN_TYPE.DBpzfa766_1) pos = 1;
			if (localstate.PZB_System == ZUGBEEINFLUSSUNG.SIGNUM) pos = 0;
			for (int i = pos; i < 6; i++)
			{
				if (i==5 && localstate.TrainType != TRAIN_TYPE.DBpzfa766_1) continue;

				if (LMs[i] == null) return i;
			}
			return 6;
		}
		private int GetNextFreeTextZeile()
		{
			int pos = 0;
			for (int i = pos; i < 3; i++)
			{
				if (LINEs[i] == null) return i;
			}
			return 2;
		}
		private int GetWidth(int pos)
		{
			if (pos == 0 || pos == 6)
				return 39;
			else
				return 38;
		}
		private int GetLocation(int pos)
		{
			int loc = 0;
			for(int i = 0; i < pos; i++)
			{
				if (i == 0 || i == 6)
					loc += 39;
				else
					loc += 38;
			}
			return loc;
		}
		private int GetLocationTextZeile(int pos)
		{
			int loc = 0;
			for(int i = 0; i < pos; i++)
			{
				loc += 27;
			}
			return loc;
		}
		public void SetTrainType(string type)
		{
			switch(type)
			{
				case "BR146_1":
					localstate.TrainType = TRAIN_TYPE.BR146_1;
					break;
				case "BR185":
					localstate.TrainType = TRAIN_TYPE.BR185;
					break;
				case "BR189":
					localstate.TrainType = TRAIN_TYPE.BR189;
					break;
			}
		}

		private void timer_Doppelpunkt_Tick(object sender, System.EventArgs e)
		{
			if (!IsCONNECTED)
			{
				SetUhrDatum(DateTime.Now /*vtime.AddSeconds(1)*/);
			}
			InternalDrawDots = !InternalDrawDots;
			DrawDots = InternalDrawDots;
			
			something_changed = true;
		}

		private void timerDisableDots_Tick(object sender, System.EventArgs e)
		{
			timerDisableDots.Enabled = false;

			DrawDots = false;
		}
	}
}
