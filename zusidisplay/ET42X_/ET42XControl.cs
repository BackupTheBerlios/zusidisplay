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


namespace MMI.ET42X
{	   
	public class ET42XControl : System.Windows.Forms.UserControl
	{
		const bool USE_DOUBLE_BUFFER = true;

		#region Declarations
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		bool inverse_display = false;

		private Bitmap m_backBuffer;
		private Graphics g;

		private DateTime vtime;

		public bool GLOBAL_SOUND = true;
		private bool CONNECTED = false;
		int stoerung_counter = 0;
		SoundInterface Sound;

		Color STÖRUNG_BG = Color.Gold, STÖRUNG_FG = Color.Black;
		Color BRIGHT = Color.WhiteSmoke; 
		Color DARK =  Color.DarkGray;
		Color BLACK = Color.Black;

		private Thread zugkraft_thread;
		Hashtable ht = new Hashtable(), ht2 = new Hashtable();
		ISphere uhrsec, uhrmin, uhrstd, uhrInnengr, uhrInnenkl, uhrAußen, uhrrest;
		Point center;

		public Network m_net;

		/*bool first_time_tacho = true;
		bool first_time_zugkraft = true;
		bool first_time_kombi = true;*/
		bool something_changed = true;
		Color MMI_BLUE = Color.FromArgb(0,128,255);
		Color MMI_ORANGE = Color.FromArgb(179, 177, 142);

		public MMI.EBuLa.Tools.XMLLoader m_conf;
		private const SmoothingMode SMOOTHING_MODE = SmoothingMode.AntiAlias;
		private const TextRenderingHint TEXT_MODE = TextRenderingHint.AntiAliasGridFit;
		float old_valu = 0f;

		//DBGraphics graph_main;

		DateTime lastTime = new DateTime(0);
		public MMI.ET42X.ET42XState localstate;		

		private System.Windows.Forms.Panel p_Buttons;
		private System.Windows.Forms.Panel p_Button1;
		private System.Windows.Forms.Panel p_UpperLine;
		private System.Windows.Forms.Label l_Button1;
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
		private System.Windows.Forms.Panel p_Button7;
		private System.Windows.Forms.Label l_Button7;
		private System.Windows.Forms.Panel p_Button8;
		private System.Windows.Forms.Label l_Button8;
		private System.Windows.Forms.Panel p_Button9;
		private System.Windows.Forms.Label l_Button9;
		private System.Windows.Forms.Panel p_Button0;
		private System.Windows.Forms.Label l_Button0;
		private System.Windows.Forms.Timer timer_st;
		private System.Windows.Forms.Panel p_extra;
		private System.Windows.Forms.Panel p_Light8;
		private System.Windows.Forms.Panel p_Light9;
		private System.Windows.Forms.Panel p_Dark3;
		private System.Windows.Forms.Panel p_Light7;
		private System.Windows.Forms.Panel p_Dark7;
		private System.Windows.Forms.Panel p_Light6;
		private System.Windows.Forms.Panel p_Dark1;
		private System.Windows.Forms.Panel p_Light5;
		private System.Windows.Forms.Panel p_Dark9;
		private System.Windows.Forms.Panel p_Light4;
		private System.Windows.Forms.Panel p_Dark8;
		private System.Windows.Forms.Panel p_Light3;
		private System.Windows.Forms.Panel p_Dark6;
		private System.Windows.Forms.Panel p_Light2;
		private System.Windows.Forms.Panel p_Dark5;
		private System.Windows.Forms.Panel p_Dark4;
		private System.Windows.Forms.Panel p_Light10;
		private System.Windows.Forms.Panel p_Dark2;
		private System.Windows.Forms.Panel p_Light1;

		bool on = true;

		#endregion
		public ET42XControl(MMI.EBuLa.Tools.XMLLoader conf, ref Network net)
		{
			InitializeComponent();

			m_net = net;

			uhrAußen = new ISphere(508+60, 41+60, 57);
			uhrInnengr = new ISphere(508+60, 41+60, 45);			
			uhrInnenkl = new ISphere(508+60, 41+60, 52);			
			uhrsec = new ISphere(508+60, 41+60, 43);
			uhrmin = new ISphere(508+60, 41+60, 43);
			uhrstd = new ISphere(508+60, 41+60, 30);
			uhrrest = new ISphere(508+60, 41+60, 7);
			center = new Point(508+60, 41+60);

			m_conf = conf;

			localstate = new MMI.ET42X.ET42XState();

			// NOTBREMSE und E-BREMSE fehlen

			SetButtons();

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

			vtime = DateTime.Now;

			zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));

			int interval = Convert.ToInt32(Math.Round((1d/(double)conf.FramesPerSecond)*1000d));
			timer1.Interval = interval;
			timer1.Enabled = true;

			Button_SW_Pressed(this, new EventArgs());						

			if (localstate.ET42Xtype1 == ET42XTYPE.ET423)
				localstate.FISType = FIS_TYPE.FIS;
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
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.p_Buttons = new System.Windows.Forms.Panel();
			this.p_extra = new System.Windows.Forms.Panel();
			this.p_Light8 = new System.Windows.Forms.Panel();
			this.p_Button0 = new System.Windows.Forms.Panel();
			this.p_Light9 = new System.Windows.Forms.Panel();
			this.l_Button0 = new System.Windows.Forms.Label();
			this.p_Dark3 = new System.Windows.Forms.Panel();
			this.p_Button9 = new System.Windows.Forms.Panel();
			this.p_Light7 = new System.Windows.Forms.Panel();
			this.l_Button9 = new System.Windows.Forms.Label();
			this.p_Dark7 = new System.Windows.Forms.Panel();
			this.p_Button8 = new System.Windows.Forms.Panel();
			this.p_Light6 = new System.Windows.Forms.Panel();
			this.l_Button8 = new System.Windows.Forms.Label();
			this.p_Dark1 = new System.Windows.Forms.Panel();
			this.p_Button7 = new System.Windows.Forms.Panel();
			this.p_Light5 = new System.Windows.Forms.Panel();
			this.l_Button7 = new System.Windows.Forms.Label();
			this.p_Dark9 = new System.Windows.Forms.Panel();
			this.p_Button6 = new System.Windows.Forms.Panel();
			this.p_Light4 = new System.Windows.Forms.Panel();
			this.l_Button6 = new System.Windows.Forms.Label();
			this.p_Dark8 = new System.Windows.Forms.Panel();
			this.p_Button5 = new System.Windows.Forms.Panel();
			this.p_Light3 = new System.Windows.Forms.Panel();
			this.l_Button5 = new System.Windows.Forms.Label();
			this.p_Dark6 = new System.Windows.Forms.Panel();
			this.p_Button4 = new System.Windows.Forms.Panel();
			this.p_Light2 = new System.Windows.Forms.Panel();
			this.l_Button4 = new System.Windows.Forms.Label();
			this.p_Dark5 = new System.Windows.Forms.Panel();
			this.p_Button3 = new System.Windows.Forms.Panel();
			this.p_Light1 = new System.Windows.Forms.Panel();
			this.l_Button3 = new System.Windows.Forms.Label();
			this.p_Dark4 = new System.Windows.Forms.Panel();
			this.p_Button2 = new System.Windows.Forms.Panel();
			this.p_Light10 = new System.Windows.Forms.Panel();
			this.l_Button2 = new System.Windows.Forms.Label();
			this.p_Dark2 = new System.Windows.Forms.Panel();
			this.p_Button1 = new System.Windows.Forms.Panel();
			this.l_Button1 = new System.Windows.Forms.Label();
			this.p_UpperLine = new System.Windows.Forms.Panel();
			this.timer_st = new System.Windows.Forms.Timer(this.components);
			this.p_Buttons.SuspendLayout();
			this.p_extra.SuspendLayout();
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
			this.p_Buttons.Controls.Add(this.p_extra);
			this.p_Buttons.Controls.Add(this.p_Button0);
			this.p_Buttons.Controls.Add(this.p_Dark3);
			this.p_Buttons.Controls.Add(this.p_Button9);
			this.p_Buttons.Controls.Add(this.p_Dark7);
			this.p_Buttons.Controls.Add(this.p_Button8);
			this.p_Buttons.Controls.Add(this.p_Dark1);
			this.p_Buttons.Controls.Add(this.p_Button7);
			this.p_Buttons.Controls.Add(this.p_Dark9);
			this.p_Buttons.Controls.Add(this.p_Button6);
			this.p_Buttons.Controls.Add(this.p_Dark8);
			this.p_Buttons.Controls.Add(this.p_Button5);
			this.p_Buttons.Controls.Add(this.p_Dark6);
			this.p_Buttons.Controls.Add(this.p_Button4);
			this.p_Buttons.Controls.Add(this.p_Dark5);
			this.p_Buttons.Controls.Add(this.p_Button3);
			this.p_Buttons.Controls.Add(this.p_Dark4);
			this.p_Buttons.Controls.Add(this.p_Button2);
			this.p_Buttons.Controls.Add(this.p_Dark2);
			this.p_Buttons.Controls.Add(this.p_Button1);
			this.p_Buttons.Location = new System.Drawing.Point(0, 395);
			this.p_Buttons.Name = "p_Buttons";
			this.p_Buttons.Size = new System.Drawing.Size(632, 64);
			this.p_Buttons.TabIndex = 0;
			// 
			// p_extra
			// 
			this.p_extra.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_extra.Controls.Add(this.p_Light8);
			this.p_extra.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_extra.Location = new System.Drawing.Point(626, 0);
			this.p_extra.Name = "p_extra";
			this.p_extra.Size = new System.Drawing.Size(3, 64);
			this.p_extra.TabIndex = 19;
			// 
			// p_Light8
			// 
			this.p_Light8.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light8.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light8.Location = new System.Drawing.Point(0, 0);
			this.p_Light8.Name = "p_Light8";
			this.p_Light8.Size = new System.Drawing.Size(2, 64);
			this.p_Light8.TabIndex = 5;
			// 
			// p_Button0
			// 
			this.p_Button0.BackColor = System.Drawing.Color.LightGray;
			this.p_Button0.Controls.Add(this.p_Light9);
			this.p_Button0.Controls.Add(this.l_Button0);
			this.p_Button0.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button0.Location = new System.Drawing.Point(566, 0);
			this.p_Button0.Name = "p_Button0";
			this.p_Button0.Size = new System.Drawing.Size(60, 64);
			this.p_Button0.TabIndex = 18;
			this.p_Button0.Click += new System.EventHandler(this.Button_0_Pressed);
			// 
			// p_Light9
			// 
			this.p_Light9.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light9.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light9.Location = new System.Drawing.Point(0, 0);
			this.p_Light9.Name = "p_Light9";
			this.p_Light9.Size = new System.Drawing.Size(2, 64);
			this.p_Light9.TabIndex = 5;
			// 
			// l_Button0
			// 
			this.l_Button0.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button0.Location = new System.Drawing.Point(6, 8);
			this.l_Button0.Name = "l_Button0";
			this.l_Button0.Size = new System.Drawing.Size(48, 48);
			this.l_Button0.TabIndex = 3;
			this.l_Button0.Text = "Button Null";
			this.l_Button0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.l_Button0.Click += new System.EventHandler(this.Button_0_Pressed);
			// 
			// p_Dark3
			// 
			this.p_Dark3.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark3.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark3.Location = new System.Drawing.Point(565, 0);
			this.p_Dark3.Name = "p_Dark3";
			this.p_Dark3.Size = new System.Drawing.Size(1, 64);
			this.p_Dark3.TabIndex = 17;
			// 
			// p_Button9
			// 
			this.p_Button9.BackColor = System.Drawing.Color.LightGray;
			this.p_Button9.Controls.Add(this.p_Light7);
			this.p_Button9.Controls.Add(this.l_Button9);
			this.p_Button9.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button9.Location = new System.Drawing.Point(503, 0);
			this.p_Button9.Name = "p_Button9";
			this.p_Button9.Size = new System.Drawing.Size(62, 64);
			this.p_Button9.TabIndex = 16;
			// 
			// p_Light7
			// 
			this.p_Light7.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light7.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light7.Location = new System.Drawing.Point(0, 0);
			this.p_Light7.Name = "p_Light7";
			this.p_Light7.Size = new System.Drawing.Size(2, 64);
			this.p_Light7.TabIndex = 5;
			// 
			// l_Button9
			// 
			this.l_Button9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button9.Location = new System.Drawing.Point(6, 8);
			this.l_Button9.Name = "l_Button9";
			this.l_Button9.Size = new System.Drawing.Size(48, 48);
			this.l_Button9.TabIndex = 3;
			this.l_Button9.Text = "Button Neun";
			this.l_Button9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark7
			// 
			this.p_Dark7.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark7.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark7.Location = new System.Drawing.Point(502, 0);
			this.p_Dark7.Name = "p_Dark7";
			this.p_Dark7.Size = new System.Drawing.Size(1, 64);
			this.p_Dark7.TabIndex = 15;
			// 
			// p_Button8
			// 
			this.p_Button8.BackColor = System.Drawing.Color.LightGray;
			this.p_Button8.Controls.Add(this.p_Light6);
			this.p_Button8.Controls.Add(this.l_Button8);
			this.p_Button8.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button8.Location = new System.Drawing.Point(440, 0);
			this.p_Button8.Name = "p_Button8";
			this.p_Button8.Size = new System.Drawing.Size(62, 64);
			this.p_Button8.TabIndex = 14;
			// 
			// p_Light6
			// 
			this.p_Light6.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light6.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light6.Location = new System.Drawing.Point(0, 0);
			this.p_Light6.Name = "p_Light6";
			this.p_Light6.Size = new System.Drawing.Size(2, 64);
			this.p_Light6.TabIndex = 5;
			// 
			// l_Button8
			// 
			this.l_Button8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button8.Location = new System.Drawing.Point(6, 8);
			this.l_Button8.Name = "l_Button8";
			this.l_Button8.Size = new System.Drawing.Size(48, 48);
			this.l_Button8.TabIndex = 3;
			this.l_Button8.Text = "Button Acht";
			this.l_Button8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark1
			// 
			this.p_Dark1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark1.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark1.Location = new System.Drawing.Point(439, 0);
			this.p_Dark1.Name = "p_Dark1";
			this.p_Dark1.Size = new System.Drawing.Size(1, 64);
			this.p_Dark1.TabIndex = 13;
			// 
			// p_Button7
			// 
			this.p_Button7.BackColor = System.Drawing.Color.LightGray;
			this.p_Button7.Controls.Add(this.p_Light5);
			this.p_Button7.Controls.Add(this.l_Button7);
			this.p_Button7.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button7.Location = new System.Drawing.Point(377, 0);
			this.p_Button7.Name = "p_Button7";
			this.p_Button7.Size = new System.Drawing.Size(62, 64);
			this.p_Button7.TabIndex = 12;
			// 
			// p_Light5
			// 
			this.p_Light5.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light5.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light5.Location = new System.Drawing.Point(0, 0);
			this.p_Light5.Name = "p_Light5";
			this.p_Light5.Size = new System.Drawing.Size(2, 64);
			this.p_Light5.TabIndex = 5;
			// 
			// l_Button7
			// 
			this.l_Button7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button7.Location = new System.Drawing.Point(6, 8);
			this.l_Button7.Name = "l_Button7";
			this.l_Button7.Size = new System.Drawing.Size(48, 48);
			this.l_Button7.TabIndex = 3;
			this.l_Button7.Text = "Button Sieben";
			this.l_Button7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark9
			// 
			this.p_Dark9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark9.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark9.Location = new System.Drawing.Point(376, 0);
			this.p_Dark9.Name = "p_Dark9";
			this.p_Dark9.Size = new System.Drawing.Size(1, 64);
			this.p_Dark9.TabIndex = 11;
			// 
			// p_Button6
			// 
			this.p_Button6.BackColor = System.Drawing.Color.LightGray;
			this.p_Button6.Controls.Add(this.p_Light4);
			this.p_Button6.Controls.Add(this.l_Button6);
			this.p_Button6.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button6.Location = new System.Drawing.Point(314, 0);
			this.p_Button6.Name = "p_Button6";
			this.p_Button6.Size = new System.Drawing.Size(62, 64);
			this.p_Button6.TabIndex = 10;
			this.p_Button6.Click += new System.EventHandler(this.Button_6_Pressed);
			// 
			// p_Light4
			// 
			this.p_Light4.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light4.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light4.Location = new System.Drawing.Point(0, 0);
			this.p_Light4.Name = "p_Light4";
			this.p_Light4.Size = new System.Drawing.Size(2, 64);
			this.p_Light4.TabIndex = 5;
			// 
			// l_Button6
			// 
			this.l_Button6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button6.Location = new System.Drawing.Point(8, 8);
			this.l_Button6.Name = "l_Button6";
			this.l_Button6.Size = new System.Drawing.Size(50, 48);
			this.l_Button6.TabIndex = 3;
			this.l_Button6.Text = "Button Sechs";
			this.l_Button6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.l_Button6.Click += new System.EventHandler(this.Button_6_Pressed);
			// 
			// p_Dark8
			// 
			this.p_Dark8.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark8.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark8.Location = new System.Drawing.Point(313, 0);
			this.p_Dark8.Name = "p_Dark8";
			this.p_Dark8.Size = new System.Drawing.Size(1, 64);
			this.p_Dark8.TabIndex = 9;
			// 
			// p_Button5
			// 
			this.p_Button5.BackColor = System.Drawing.Color.LightGray;
			this.p_Button5.Controls.Add(this.p_Light3);
			this.p_Button5.Controls.Add(this.l_Button5);
			this.p_Button5.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button5.Location = new System.Drawing.Point(251, 0);
			this.p_Button5.Name = "p_Button5";
			this.p_Button5.Size = new System.Drawing.Size(62, 64);
			this.p_Button5.TabIndex = 8;
			// 
			// p_Light3
			// 
			this.p_Light3.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light3.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light3.Location = new System.Drawing.Point(0, 0);
			this.p_Light3.Name = "p_Light3";
			this.p_Light3.Size = new System.Drawing.Size(2, 64);
			this.p_Light3.TabIndex = 5;
			// 
			// l_Button5
			// 
			this.l_Button5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button5.Location = new System.Drawing.Point(6, 8);
			this.l_Button5.Name = "l_Button5";
			this.l_Button5.Size = new System.Drawing.Size(48, 48);
			this.l_Button5.TabIndex = 3;
			this.l_Button5.Text = "Button Fünf";
			this.l_Button5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark6
			// 
			this.p_Dark6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark6.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark6.Location = new System.Drawing.Point(250, 0);
			this.p_Dark6.Name = "p_Dark6";
			this.p_Dark6.Size = new System.Drawing.Size(1, 64);
			this.p_Dark6.TabIndex = 7;
			// 
			// p_Button4
			// 
			this.p_Button4.BackColor = System.Drawing.Color.LightGray;
			this.p_Button4.Controls.Add(this.p_Light2);
			this.p_Button4.Controls.Add(this.l_Button4);
			this.p_Button4.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button4.Location = new System.Drawing.Point(188, 0);
			this.p_Button4.Name = "p_Button4";
			this.p_Button4.Size = new System.Drawing.Size(62, 64);
			this.p_Button4.TabIndex = 6;
			// 
			// p_Light2
			// 
			this.p_Light2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light2.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light2.Location = new System.Drawing.Point(0, 0);
			this.p_Light2.Name = "p_Light2";
			this.p_Light2.Size = new System.Drawing.Size(2, 64);
			this.p_Light2.TabIndex = 5;
			// 
			// l_Button4
			// 
			this.l_Button4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button4.Location = new System.Drawing.Point(6, 8);
			this.l_Button4.Name = "l_Button4";
			this.l_Button4.Size = new System.Drawing.Size(48, 48);
			this.l_Button4.TabIndex = 3;
			this.l_Button4.Text = "Button Vier";
			this.l_Button4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark5
			// 
			this.p_Dark5.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark5.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark5.Location = new System.Drawing.Point(187, 0);
			this.p_Dark5.Name = "p_Dark5";
			this.p_Dark5.Size = new System.Drawing.Size(1, 64);
			this.p_Dark5.TabIndex = 5;
			// 
			// p_Button3
			// 
			this.p_Button3.BackColor = System.Drawing.Color.LightGray;
			this.p_Button3.Controls.Add(this.p_Light1);
			this.p_Button3.Controls.Add(this.l_Button3);
			this.p_Button3.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button3.Location = new System.Drawing.Point(126, 0);
			this.p_Button3.Name = "p_Button3";
			this.p_Button3.Size = new System.Drawing.Size(61, 64);
			this.p_Button3.TabIndex = 4;
			// 
			// p_Light1
			// 
			this.p_Light1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light1.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light1.Location = new System.Drawing.Point(0, 0);
			this.p_Light1.Name = "p_Light1";
			this.p_Light1.Size = new System.Drawing.Size(2, 64);
			this.p_Light1.TabIndex = 5;
			// 
			// l_Button3
			// 
			this.l_Button3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button3.Location = new System.Drawing.Point(8, 8);
			this.l_Button3.Name = "l_Button3";
			this.l_Button3.Size = new System.Drawing.Size(48, 48);
			this.l_Button3.TabIndex = 3;
			this.l_Button3.Text = "Button Drei";
			this.l_Button3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark4
			// 
			this.p_Dark4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark4.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark4.Location = new System.Drawing.Point(125, 0);
			this.p_Dark4.Name = "p_Dark4";
			this.p_Dark4.Size = new System.Drawing.Size(1, 64);
			this.p_Dark4.TabIndex = 3;
			// 
			// p_Button2
			// 
			this.p_Button2.BackColor = System.Drawing.Color.LightGray;
			this.p_Button2.Controls.Add(this.p_Light10);
			this.p_Button2.Controls.Add(this.l_Button2);
			this.p_Button2.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Button2.Location = new System.Drawing.Point(63, 0);
			this.p_Button2.Name = "p_Button2";
			this.p_Button2.Size = new System.Drawing.Size(62, 64);
			this.p_Button2.TabIndex = 2;
			// 
			// p_Light10
			// 
			this.p_Light10.BackColor = System.Drawing.SystemColors.ControlLight;
			this.p_Light10.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Light10.Location = new System.Drawing.Point(0, 0);
			this.p_Light10.Name = "p_Light10";
			this.p_Light10.Size = new System.Drawing.Size(2, 64);
			this.p_Light10.TabIndex = 4;
			// 
			// l_Button2
			// 
			this.l_Button2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button2.Location = new System.Drawing.Point(5, 8);
			this.l_Button2.Name = "l_Button2";
			this.l_Button2.Size = new System.Drawing.Size(50, 48);
			this.l_Button2.TabIndex = 3;
			this.l_Button2.Text = "Button Zwei";
			this.l_Button2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// p_Dark2
			// 
			this.p_Dark2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.p_Dark2.Dock = System.Windows.Forms.DockStyle.Left;
			this.p_Dark2.Location = new System.Drawing.Point(62, 0);
			this.p_Dark2.Name = "p_Dark2";
			this.p_Dark2.Size = new System.Drawing.Size(1, 64);
			this.p_Dark2.TabIndex = 1;
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
			this.l_Button1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_Button1.Location = new System.Drawing.Point(6, 8);
			this.l_Button1.Name = "l_Button1";
			this.l_Button1.Size = new System.Drawing.Size(48, 48);
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
			// timer_st
			// 
			this.timer_st.Enabled = true;
			this.timer_st.Interval = 1000;
			this.timer_st.Tick += new System.EventHandler(this.timer_st_Tick);
			// 
			// ET42XControl
			// 
			this.BackColor = System.Drawing.Color.LightGray;
			this.Controls.Add(this.p_UpperLine);
			this.Controls.Add(this.p_Buttons);
			this.Name = "ET42XControl";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BR185Control_Paint);
			this.p_Buttons.ResumeLayout(false);
			this.p_extra.ResumeLayout(false);
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

		public bool addtionalhours
		{
			get{return localstate.addtionalhours;}
			set{localstate.addtionalhours = value;}
		}
		#region Buttons
		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS_ROUTE:
					localstate.fis.NextEntry();
					break;
				case CURRENT_DISPLAY.FIS_HK:
					Send(FIS_DATA.Next_Entry);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_Down_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS_ROUTE:
					localstate.fis.PrevEntry();
					break;
				case CURRENT_DISPLAY.FIS_HK:
					Send(FIS_DATA.Prev_Entry);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_E_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS_ROUTE:
					localstate.fis.NextFISPos((IFISNetwork)m_net);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_C_Pressed(object sender, System.EventArgs e)
		{
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_Inverse_Pressed(object sender, System.EventArgs e)
		{
			Inverse();
			something_changed = true;
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
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.OBERSTROM:
					localstate.OberstromGrenze = 1000f;
					break;
				case CURRENT_DISPLAY.Info:
					localstate.DISPLAY = CURRENT_DISPLAY.Besetz;
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_2_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.OBERSTROM:
					localstate.OberstromGrenze = 900f;
					break;
				case CURRENT_DISPLAY.Info:
					localstate.DISPLAY = CURRENT_DISPLAY.Vist;
					break;
				case CURRENT_DISPLAY.FIS:
					if (localstate.FISType == FIS_TYPE.FIS)
					{
						localstate.fis.position = FIS_POS.LINIE;
						localstate.DISPLAY = CURRENT_DISPLAY.FIS_ROUTE;
					}
					//else
						//localstate.DISPLAY = CURRENT_DISPLAY.FIS_ZUGNR;
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_3_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.S:
					localstate.Kuehlung = !localstate.Kuehlung;
					break;
				case CURRENT_DISPLAY.OBERSTROM:
					localstate.OberstromGrenze = 600f;
					break;
				case CURRENT_DISPLAY.FIS:
					if (localstate.fis.Status) localstate.DISPLAY = CURRENT_DISPLAY.FIS_HK;
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_4_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					//localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					break;
				case CURRENT_DISPLAY.S:
					localstate.DISPLAY = CURRENT_DISPLAY.OBERSTROM;
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_5_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					localstate.DISPLAY = CURRENT_DISPLAY.B;
					break;
				case CURRENT_DISPLAY.FIS:
					if (localstate.FISType != FIS_TYPE.FIS)
					{
						localstate.fis.position = FIS_POS.LINIE;
						localstate.DISPLAY = CURRENT_DISPLAY.FIS_ROUTE;
					}
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					localstate.fis.Delete();
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
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
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					localstate.DISPLAY = CURRENT_DISPLAY.Spg;
					break;
				case CURRENT_DISPLAY.FIS:
//OLD					if (localstate.fis.ShowEntry()) localstate.fis.FISAnAus();
					if (l_Button7.Text.IndexOf("FIS") > -1)
						Send(FIS_DATA.FIS_Aus);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_8_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					localstate.DISPLAY = CURRENT_DISPLAY.FIS;
					Send(FIS_DATA.Status);
					Send(FIS_DATA.GPS_Status);
					Send(FIS_DATA.Naechster_Halt);
					Send(FIS_DATA.Route_Data);
					break;
				case CURRENT_DISPLAY.FIS:
					Send(FIS_DATA.FIS_Start);
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					Send(FIS_DATA.FIS_Start);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_9_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					localstate.DISPLAY = CURRENT_DISPLAY.S;
					break;
				case CURRENT_DISPLAY.FIS:
					if (l_Button9.Text.IndexOf("FIS") > -1)
						Send(FIS_DATA.FIS_Ausloesen);
					break;
			}
			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_0_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.FIS_HK:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.S:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.OBERSTROM:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Spg:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Info:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Vist:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Besetz:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.B:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.ST:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.V_EQUAL_0:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.V_GREATER_0:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
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
			if (localstate.ET42Xtype1 == ET42XTYPE.ET423 || localstate.ET42Xtype1 == ET42XTYPE.ET424 || localstate.ET42Xtype1 == ET42XTYPE.ET426)
				localstate.FISType = FIS_TYPE.FIS;
			else
				localstate.FISType = FIS_TYPE.GPS_FIS;

			something_changed = true;
			if (m_conf.FocusToZusi) Stuff.SetFocusToZusi();
		}

		public void Button_I_Pressed(object sender, System.EventArgs e)
		{
			switch(localstate.DISPLAY)
			{
				default:
					localstate.DISPLAY = CURRENT_DISPLAY.Info;
					break;
			}
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
				}
				else
				{
					if (CONNECTED != value)
					{
						localstate.störungmgr.Add(new Störung(ENUMStörung.S01_ZUSIKomm));
						if (m_conf.Sound != 0 && GLOBAL_SOUND)
						{
							switch (m_conf.Sound)
							{
								case 1:
									Sound.PlayMalfunctionBombardierSound();
									Sound.PlayMalfunctionBombardierSound();
									Sound.PlayMalfunctionBombardierSound();
									break;
								case 2:
									Sound.PlayMalfunctionBombardierSound();
									Sound.PlayMalfunctionBombardierSound();
									Sound.PlayMalfunctionBombardierSound();
									break;
							}
						}
					}
				}
				CONNECTED = value;
				something_changed = true;

			}
		}

		private void Send(FIS_DATA type)
		{
			if (m_net != null) m_net.SendData(type, "");
		}

		private void DrawFrame(ref Graphics pg, int x, int y, int width, int height)
		{
			// old Style
			DrawFrameRaised(ref pg, x, y, width, height);
			DrawFrameSunken(ref pg, x+2, y+2, width-4, height-4);

			// new Style
			//DrawFrameSunken(ref pg, x, y, width, height);
			//DrawFrameRaised(ref pg, x+2, y+2, width-3, height-3);
		}

		private void DrawFrameNewStyle(ref Graphics pg, int x, int y, int width, int height)
		{
			// new Style
			DrawFrameSunken(ref pg, x, y, width, height);
			DrawFrameRaised(ref pg, x+2, y+2, width-3, height-3);
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
		public void SetFIS_Status(bool state)
		{
			localstate.fis.Status = state;
			something_changed = true;
		}
		public void SetFIS_GPSStatus(bool state)
		{
			localstate.fis.GPSStatus = state;
			something_changed = true;
		}
		public void SetFIS_NaechsterHalt(string data)
		{
			localstate.fis.NaechsterHalt = data;
			something_changed = true;
		}

		public void SetFIS_Netz(string data)
		{
			localstate.fis.CreateNetz(data);
		}

		public void SetFIS_Linie(string data)
		{
			localstate.fis.SetLinie(data);
		}

		public void SetFIS_Start(string data)
		{
			localstate.fis.CreateStart(data);
		}

		public void SetFIS_Ziel(string data)
		{
			localstate.fis.CreateZiel(data);
		}

		public void SetFIS_NoMoreData()
		{
			localstate.fis.NoMoreData();
		}

		public void SetFIS_NoMoreZiel()
		{
			localstate.fis.NoMoreZiel();
		}

		public void SetFIS_NoMoreStart()
		{
			localstate.fis.NoMoreStart();
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
		public void SetFahrstufenSchalter(float valu)
		{
			localstate.FahrstufenSchalter = Convert.ToInt32(valu);
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
			if (localstate.SHOW_CLOCK) something_changed = true;
		}

		public void DrawZugkraft(ref Graphics pg)
		{
			// Rahmen
			DrawFrame(ref pg, 508, 281, 120, 44);

			float zugkraft = localstate.ZugkraftGesammt;

			zugkraft = ReduziereZugkraft(localstate.ZugkraftGesammt, 1) * 2f;

			if (localstate.ET42Xtype2 != ET42XTYPE.NONE)
				zugkraft += ReduziereZugkraft(localstate.ZugkraftGesammt, 2) * 2f;

			if (localstate.ET42Xtype3 != ET42XTYPE.NONE)
				zugkraft += ReduziereZugkraft(localstate.ZugkraftGesammt, 3) * 2f;

			if (zugkraft < 0) zugkraft *= -1;

			string s = Convert.ToInt32(zugkraft).ToString();

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Gesamtkraft", f, new SolidBrush(BLACK), 510, 256);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("kN", f, new SolidBrush(BLACK), 590, 293);
			if (zugkraft >= 100)
				pg.DrawString(s, f, new SolidBrush(BLACK), 564, 293);
			else if (zugkraft >= 10)
				pg.DrawString(s, f, new SolidBrush(BLACK), 572, 293);
			else
				pg.DrawString(s, f, new SolidBrush(BLACK), 580, 293);
		}

		public void DrawUhr(ref Graphics pg)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.G)
			{
				// Uhr analog
				ArrayList uhrAußenList = uhrAußen.PointList();
				ArrayList uhrInnenklList = uhrInnenkl.PointList();
				ArrayList uhrInnengrList = uhrInnengr.PointList();
				ArrayList uhrminList = uhrmin.PointList();
				ArrayList uhrstdList = uhrstd.PointList();
				ArrayList uhrsecList = uhrsec.PointList();
				ArrayList uhrrestList = uhrrest.PointList();

				// Rahmen
				DrawFrame(ref pg, 508, 41, 120, 120);

				string s = localstate.ZugkraftGesammt.ToString() + " kN";

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
			else
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

		}

		public void DrawStörung(ref Graphics pg)
		{
			string s = "";
			Pen p_dg_3 = new Pen(new SolidBrush(DARK), 2);
			Pen p_dg_1 = new Pen(new SolidBrush(DARK), 1);
			Pen p_ws_3 = new Pen(new SolidBrush(BRIGHT), 2);
			Pen p_ws_1 = new Pen(new SolidBrush(BRIGHT), 1);

			Font f = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Millimeter);

			Color StTextColor = BLACK;
			if (inverse_display) StTextColor = MMI_BLUE;


			if (localstate.störungmgr.Current.Type != ENUMStörung.NONE)
			{
				if (STÖRUNG_BG == Color.Gold)
				{
					pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438, 338, 190, 50);
				}
				DrawFrame(ref pg, 438, 338, 190, 50);

				Störung st = localstate.störungmgr.Current;

				s = "St. in "+st.Name;
				pg.DrawString(s, f, new SolidBrush(StTextColor), 462, 352);
			}
			else if (localstate.störungmgr.GetPassives().Count > 1)
			{
				if (STÖRUNG_BG == Color.Gold)
				{
					if (inverse_display)
						pg.FillRectangle(new SolidBrush(MMI_ORANGE), 438, 338, 190, 50);
					else
						pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438, 338, 190, 50);
				}
				DrawFrame(ref pg, 438, 338, 190, 50);

				s = "          St";
				pg.DrawString(s, f, new SolidBrush(StTextColor), 462, 352);
			}


			/*
			f = new Font("Arial", 13, FontStyle.Regular, GraphicsUnit.Point);
			pg.DrawString(s, f, new SolidBrush(STÖRUNG_FG), 490, 410);
			*/
			pg.SmoothingMode = SMOOTHING_MODE;

		}
		public void DrawZugbeeinflussungssysteme(ref Graphics pg)
		{
			// Rahmen
			DrawFrame(ref pg, 5, 5, 420, 240);
			DrawFrame(ref pg, 5, 250, 420, 50);
			DrawFrame(ref pg, 5, 305, 420, 30);

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

			pg.DrawString("Eingestellte Daten", f, new SolidBrush(BLACK), 110, 185);
			if (localstate.LM_LZB_Ü)
				pg.DrawString("LZB-Betrieb", f, new SolidBrush(BLACK), 140, 265);
			else
				pg.DrawString("Kein LZB-Betrieb", f, new SolidBrush(BLACK), 110, 265);
			if (localstate.DSK_Gesperrt)
				pg.DrawString("DSK Kurzzeitspeicher geperrt", f, new SolidBrush(BLACK), 60, 310);
		}

		public void DrawZug(ref Graphics pg)
		{
			// Zugnummer
			DrawFrame(ref pg, 468, 189, 160, 30);

			string help = localstate.Zugnummer;
			int length = help.Length;

			for (int i = 0; i < length; i++)
			{
				if (help[0] != '0') break;
				help = help.Remove(0,1);
			}

			string s = "Zug " + help;

			if (localstate.Zugnummer == "<INIT>") return;

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString(s, f, new SolidBrush(BLACK), 510, 196);
		}
		public void UpdateScreen()
		{
			if (!something_changed)
				return;

			something_changed = false;

			BR185Control_Paint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0,0,this.Width, this.Height)));

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
			if (localstate.DISPLAY == CURRENT_DISPLAY.G)
			{
				// Datum oben rechts
				DrawFrame(ref pg, 508, 1, 120, 38);

				string s = MMI.EBuLa.Tools.Misc.getDateString(vtime);

				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

				pg.DrawString(s, f, new SolidBrush(BLACK), 517, 12);
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
		public void DrawFISLinie(ref Graphics pg)
		{
			// Rahmen oben
			DrawFrame(ref pg, 1, 1, 505, 38);

			string s = localstate.fis.Linie + " / "+ localstate.fis.Ziel + " / " + localstate.fis.NaechsterHalt;

			if (!localstate.fis.ShowEntry()) s = "";

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString(s, f, new SolidBrush(BLACK), 8, 12);

		}
		public void SetButtons()
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					l_Button1.Text = "";
					l_Button2.Text = "";//"W";
					l_Button3.Text = "";//"ZUG";
					l_Button4.Text = "";//"Zug-Besy";
					l_Button5.Text = "B";//"B";
					l_Button6.Text = "";//"V/A";
					l_Button7.Text = "Spg";//"Spg";
					l_Button8.Text = "FIS";
					l_Button9.Text = "S";//"S";
					l_Button0.Text = "";
					break;
				case CURRENT_DISPLAY.S:
					l_Button1.Text = "";//"An- trieb";
					l_Button2.Text = "";//"Zug- Absp.";
					if (localstate.Kuehlung)
						l_Button3.Text = "Kühl Aus";
					else
						l_Button3.Text = "Kühl Ein";
					l_Button4.Text = "Ober- strom";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";//"Ver- sch.";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.FIS:
					l_Button1.Text = "";
					if (localstate.FISType == FIS_TYPE.FIS)
                        l_Button2.Text = "Rout. Eing.";
					else
						l_Button2.Text = "Flüg. Zugnr"; 						
					if (localstate.fis.Status)
                        l_Button3.Text = "Halt. Korr.";
					else
						l_Button3.Text = "";						
					l_Button4.Text = "";//"Zug verl.";
					if (localstate.FISType == FIS_TYPE.FIS)
						l_Button5.Text = "";
					else
						l_Button5.Text = "Rout. Eing.";
					l_Button6.Text = "";
					if (localstate.fis.Status)
					{
						l_Button7.Text = "FIS aus";
						l_Button8.Text = "FIS Start";
						l_Button9.Text = "FIS ausl.";
					}
					else
					{
						l_Button7.Text = "";
						l_Button8.Text = "";
						l_Button9.Text = "";
					}
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					l_Button1.Text = "";
					l_Button2.Text = "";
					l_Button3.Text = "";
					l_Button4.Text = "";
					l_Button5.Text = "lösch";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "FIS Start";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.OBERSTROM:
					l_Button1.Text = "1000";
					l_Button2.Text = "900";
					l_Button3.Text = "600";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.Info:
					l_Button1.Text = "Bes.- Grad";
					l_Button2.Text = "V ist";
					l_Button3.Text = "";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.Besetz:
					l_Button1.Text = "";
					l_Button2.Text = "";
					l_Button3.Text = "";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.Vist:
					l_Button1.Text = "";
					l_Button2.Text = "";
					l_Button3.Text = "";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				default:
					l_Button1.Text = "";
					l_Button2.Text = "";
					l_Button3.Text = "";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
			}
		}	

		public void DrawKasten(ref Graphics pg, int tz)
		{
			Color color = MMI_BLUE;
			float zugkraft = localstate.ZugkraftGesammt;

			zugkraft = ReduziereZugkraft(zugkraft, tz);

			if (zugkraft < 0)
			{
				color = MMI_ORANGE;
				zugkraft *= -1;
			}

			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 46, 105, 205);

			int height = Convert.ToInt32(zugkraft * 1.5d);
			if (height > 150) height = 150;

			pg.FillRectangle(new SolidBrush(color), 08+(tz-1)*110+tz, 155 - height + 74, 30, height);
			pg.FillRectangle(new SolidBrush(color), 68+(tz-1)*110+tz, 155 - height + 74, 30, height);

			DrawFrameSunkenSmall(ref pg, 08+(tz-1)*110+tz, 79, 30, 150);
			DrawFrameSunkenSmall(ref pg, 68+(tz-1)*110+tz, 79, 30, 150);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("kN", f, new SolidBrush(BLACK), 40+(tz-1)*110+tz, 52);

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);

			for (int i = 0; i < 110; i+=10)
			{           
                if (i <= 0)
                    pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 54+(tz-1)*110+tz, 66 + (155 - i * 1.5f));
				else if (i < 100)
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 48+(tz-1)*110+tz, 66 + (155 - i * 1.5f));
				else
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 42+(tz-1)*110+tz, 66 + (155 - i * 1.5f));

				pg.DrawLine(new Pen(BLACK), 62+(tz-1)*110+tz, 74 + (155 - i * 1.5f), 62+(tz-1)*110+tz+5, 74 + (155 - i * 1.5f));
			}

			// Zugteil			
			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 253, 105, 30);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Zug "+tz.ToString(), f, new SolidBrush(BLACK), 28+(tz-1)*110+tz, 258);


			// Zugnummer

			string zug = "";

			switch (tz)
			{
				case 1:
					zug = TrainType(localstate.ET42Xtype1);
					break;
				case 2:
					zug = TrainType(localstate.ET42Xtype2);
					break;
				case 3:
					zug = TrainType(localstate.ET42Xtype3);
					break;					
			}

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 285, 52, 30);
			pg.DrawString(zug+" 00"+tz.ToString(), f, new SolidBrush(BLACK), 6+(tz-1)*110+tz, 294);
			
			DrawFrameNewStyle(ref pg, 54+(tz-1)*110+tz, 285, 52, 30);
			pg.DrawString(zug+" 50"+tz.ToString(), f, new SolidBrush(BLACK), 60+(tz-1)*110+tz, 294);

			//Pfeil
			int iheight = 0; Brush b;
			float factor = 1f;
            
            ET42XTYPE current_type;
			if (tz == 1)
				current_type = localstate.ET42Xtype1;
			else if (tz == 2)
				current_type = localstate.ET42Xtype2;
			else current_type = localstate.ET42Xtype3;

			if (localstate.FahrstufenSchalter >= 10)
			{
				b = new SolidBrush(MMI_BLUE);
				if (current_type == ET42XTYPE.ET423)
					factor = 150f / 1000f * 74f;
				else if (current_type == ET42XTYPE.ET424 || current_type == ET42XTYPE.ET425)
					factor = 150f / 1000f * 68f;
				else factor = 150f / 1000f * 35f;

				iheight = Convert.ToInt32((localstate.FahrstufenSchalter-10) * factor);
			}
			else
			{
				b = new SolidBrush(MMI_ORANGE);
				if (current_type == ET42XTYPE.ET423)
					factor = 150f / 1000f * 65f;
				else if (current_type == ET42XTYPE.ET424 || current_type == ET42XTYPE.ET425)
					factor = 150f / 1000f * 65f;
				else factor = 150f / 1000f * 45f;

				iheight = Convert.ToInt32((10-localstate.FahrstufenSchalter) * factor);
			}

			Point[] p = {new Point(08+7+(tz-1)*110+tz,155 - iheight + 74), new Point(08+7+(tz-1)*110+tz-11,155 - iheight + 74+4), new Point(08+7+(tz-1)*110+tz-7,155 - iheight + 74), new Point(08+7+(tz-1)*110+tz-11,155 - iheight + 74-4)};
			Point[] p2 = {new Point(68+7+(tz-1)*110+tz,155 - iheight + 74), new Point(68+7+(tz-1)*110+tz-11,155 - iheight + 74+4), new Point(68+7+(tz-1)*110+tz-7,155 - iheight + 74), new Point(68+7+(tz-1)*110+tz-11,155 - iheight + 74-4)};
			pg.FillPolygon(b, p);
			pg.FillPolygon(b, p2);
			pg.DrawPolygon(new Pen(BRIGHT), p);
			pg.DrawPolygon(new Pen(BRIGHT), p2);
			
		}
		public void DrawKastenBesetztgrad(ref Graphics pg, int tz)
		{
			Color color = MMI_BLUE;
			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 46, 105, 225);

			int height = 107;
			if (height > 150) height = 150;

			pg.FillRectangle(new SolidBrush(color), 08+(tz-1)*110+tz, 155 - height + 74, 30, height);
			pg.FillRectangle(new SolidBrush(color), 68+(tz-1)*110+tz, 155 - height + 74, 30, height);

			DrawFrameSunkenSmall(ref pg, 08+(tz-1)*110+tz, 79, 30, 150);
			DrawFrameSunkenSmall(ref pg, 68+(tz-1)*110+tz, 79, 30, 150);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("%", f, new SolidBrush(BLACK), 43+(tz-1)*110+tz, 52);

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);

			for (int i = 0; i < 150; i+=10)
			{   
				pg.DrawLine(new Pen(BLACK), 62+(tz-1)*110+tz, 74 + (155 - i * 1.071f), 62+(tz-1)*110+tz+5, 74 + (155 - i * 1.071f));

				if (Math.IEEERemainder(i, 20) != 0) continue;

				if (i <= 0)
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 54+(tz-1)*110+tz, 66 + (155 - i * 1.071f));
				else if (i < 100)
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 48+(tz-1)*110+tz, 66 + (155 - i * 1.071f));
				else
					pg.DrawString(i.ToString(), f, new SolidBrush(BLACK), 42+(tz-1)*110+tz, 66 + (155 - i * 1.071f));
			}

			
			// Zugnummer
			string zug = "";

			switch (tz)
			{
				case 1:
					zug = TrainType(localstate.ET42Xtype1);
					break;
				case 2:
					zug = TrainType(localstate.ET42Xtype2);
					break;
				case 3:
					zug = TrainType(localstate.ET42Xtype3);
					break;					
			}

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 275, 52, 30);
			pg.DrawString(zug+" 00"+tz.ToString(), f, new SolidBrush(BLACK), 6+(tz-1)*110+tz, 284);
			
			DrawFrameNewStyle(ref pg, 54+(tz-1)*110+tz, 275, 52, 30);
			pg.DrawString(zug+" 50"+tz.ToString(), f, new SolidBrush(BLACK), 60+(tz-1)*110+tz, 284);

			// Mittelwert
			DrawFrame(ref pg, 508, 281, 120, 44);

			string s = "100";

			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("Mittelwert", f, new SolidBrush(BLACK), 515, 256);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("%", f, new SolidBrush(BLACK), 590, 293);
			//if (zugkraft >= 100)
				pg.DrawString(s, f, new SolidBrush(BLACK), 564, 293);
			/*else if (zugkraft >= 10)
				pg.DrawString(s, f, new SolidBrush(BLACK), 572, 293);
			else
				pg.DrawString(s, f, new SolidBrush(BLACK), 580, 293);			*/
		}
		public void DrawKastenSpannung(ref Graphics pg, int tz)
		{
			Color color = MMI_BLUE;
			float spannung = localstate.Spannung;

			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 46, 105, 205);

			int height = Convert.ToInt32((spannung-9) * 15d / 1.1d);
			if (height > 150) height = 150;

			pg.FillRectangle(new SolidBrush(color), 35+(tz-1)*110+tz, 155 - height + 74, 37, height);

			if (localstate.Spannung > 1f)
				pg.FillRectangle(new SolidBrush(color), 35+(tz-1)*110+tz, 77+155, 37, 14);

			DrawFrameSunkenSmall(ref pg, 35+(tz-1)*110+tz, 79, 37, 150);
			DrawFrameSunkenSmall(ref pg, 35+(tz-1)*110+tz, 77+155, 37, 14);

			Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

			pg.DrawString("kV", f, new SolidBrush(BLACK), 10+(tz-1)*110+tz, 52);

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);

			for (int i = 0; i < 120; i+=10)
			{           
				if (i == 10 || i == 60 || i == 110)
					pg.DrawString((i/10f + 9f).ToString(), f, new SolidBrush(BLACK), 15+(tz-1)*110+tz, 67 + (155 - i * 1.364f));

				pg.DrawLine(new Pen(BLACK), 30+(tz-1)*110+tz, 74 + (155 - i * 1.364f), 30+(tz-1)*110+tz+5, 74 + (155 - i * 1.364f));
			}

			// 0
			pg.DrawString("0", f, new SolidBrush(BLACK), 22+(tz-1)*110+tz, 77 + 155);
			pg.DrawLine(new Pen(BLACK), 30+(tz-1)*110+tz, 84 + 155, 30+(tz-1)*110+tz+5, 84 + 155);

			ET42XTYPE current_type;
			if (tz == 1)
				current_type = localstate.ET42Xtype1;
			else if (tz == 2)
				current_type = localstate.ET42Xtype2;
			else current_type = localstate.ET42Xtype3;

			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 253, 52, 50);
			DrawFrameNewStyle(ref pg, 54+(tz-1)*110+tz, 253, 52, 50);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("HS 1", f, new SolidBrush(BLACK), 08+(tz-1)*110+tz, 258);
			if (current_type != ET42XTYPE.ET426)
				pg.DrawString("HS 2", f, new SolidBrush(BLACK), 62+(tz-1)*110+tz, 258);

			if (localstate.LM_HS)
			{
				pg.DrawString("AUS", f, new SolidBrush(BLACK), 10+(tz-1)*110+tz, 280);
				if (current_type != ET42XTYPE.ET426)
					pg.DrawString("AUS", f, new SolidBrush(BLACK), 64+(tz-1)*110+tz, 280);
			}
			else
			{
				pg.DrawString("EIN", f, new SolidBrush(BLACK), 11+(tz-1)*110+tz, 280);
				if (current_type != ET42XTYPE.ET426)
					pg.DrawString("EIN", f, new SolidBrush(BLACK), 65+(tz-1)*110+tz, 280);
			}


			// Zugnummer

			string zug = "";

			switch (tz)
			{
				case 1:
					zug = TrainType(localstate.ET42Xtype1);
					break;
				case 2:
					zug = TrainType(localstate.ET42Xtype2);
					break;
				case 3:
					zug = TrainType(localstate.ET42Xtype3);
					break;					
			}

			f = new Font("Arial", 3, FontStyle.Bold, GraphicsUnit.Millimeter);
			DrawFrameNewStyle(ref pg, 1+(tz-1)*110+tz, 305, 52, 20);
			pg.DrawString(zug+" 00"+tz.ToString(), f, new SolidBrush(BLACK), 6+(tz-1)*110+tz, 309);
			
			DrawFrameNewStyle(ref pg, 54+(tz-1)*110+tz, 305, 52, 20);
			pg.DrawString(zug+" 50"+tz.ToString(), f, new SolidBrush(BLACK), 60+(tz-1)*110+tz, 309);
			
		}
		public void DrawTitle(ref Graphics pg)
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					DrawFISLinie(ref pg);
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

		public void DrawFIS(ref Graphics pg)
		{
			switch(localstate.FISType)
			{
				case FIS_TYPE.FIS:
					DrawFIS_OhneGPS(ref pg);
					break;
				case FIS_TYPE.GPS_FIS:
					DrawFIS_MitGPS(ref pg);
					break;
				case FIS_TYPE.GPS_ZN_FIS:
					break;
			}
		}

		public void DrawFIS_OhneGPS(ref Graphics pg)
		{
			// Eingestellte Routen
			DrawFrame(ref pg, 1, 41, 315, 200);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Eingestellte Routen:", f, new SolidBrush(BLACK), 5, 65-19);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				pg.DrawString("Zug "+i.ToString(), f, new SolidBrush(BLACK), 10, 70-32+i*42);
				if (localstate.fis.ShowEntry()) 
					pg.DrawString(localstate.fis.Linie+" / "+localstate.fis.Ziel, f, new SolidBrush(BLACK), 100, 70-32+i*42);
				else
					pg.DrawString("                 ---", f, new SolidBrush(BLACK), 100, 70-32+i*42);

				DrawFrameSunkenSmall(ref pg, 72, 57-24+i*42, 230, 25);
			}

			// Nächster Halt
			DrawFrame(ref pg, 318, 41, 210, 99);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Nächster Halt:", f, new SolidBrush(BLACK), 363, 55);
			DrawFrameSunkenSmall(ref pg, 330, 90, 190, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.fis.NaechsterHalt == "")
				pg.DrawString("             ---", f, new SolidBrush(BLACK), 350, 98);
			else
				pg.DrawString(localstate.fis.NaechsterHalt, f, new SolidBrush(BLACK), 350, 98);



			// Rahmen FIS Status
			DrawFrame(ref pg, 318, 142, 210, 99);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("FIS Status:", f, new SolidBrush(BLACK), 375, 159);
			DrawFrameSunkenSmall(ref pg, 330, 194, 190, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.fis.Status)
                pg.DrawString("EIN", f, new SolidBrush(BLACK), 410, 202);
			else
				pg.DrawString("AUS", f, new SolidBrush(BLACK), 410, 202);
		}

		public void DrawFIS_MitGPS(ref Graphics pg)
		{
			// Eingestellte Routen
			DrawFrame(ref pg, 1, 41, 315, 200);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Eingestellte Routen:", f, new SolidBrush(BLACK), 5, 65-19);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				pg.DrawString("Zug "+i.ToString(), f, new SolidBrush(BLACK), 10, 70-32+i*42);
				if (localstate.fis.ShowEntry()) 
					pg.DrawString(localstate.fis.Linie+" / "+localstate.fis.Ziel, f, new SolidBrush(BLACK), 100, 70-32+i*42);
				else
					pg.DrawString("                 ---", f, new SolidBrush(BLACK), 100, 70-32+i*42);
				DrawFrameSunkenSmall(ref pg, 72, 57-24+i*42, 230, 25);
			}

			// Eingestellte Flügelzugnummern
			DrawFrame(ref pg, 1+316, 41, 311, 200);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Eingestellte Flügelzugnummern:", f, new SolidBrush(BLACK), 5+316, 65-19);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				pg.DrawString("Zug "+i.ToString(), f, new SolidBrush(BLACK), 10+316, 70-32+i*42);
				if (localstate.fis.Zugnummer == "")
                    pg.DrawString("                 ---", f, new SolidBrush(BLACK), 100+316, 70-32+i*42);
				else
					pg.DrawString(localstate.fis.Zugnummer, f, new SolidBrush(BLACK), 100+316, 70-32+i*42);
				DrawFrameSunkenSmall(ref pg, 72+316, 57-24+i*42, 230, 25);
			}

			// Nächster Halt
			DrawFrame(ref pg, 320-319, 60-60+243, 225, 89);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Nächster Halt:", f, new SolidBrush(BLACK), 360-316, 75-65+243);
			DrawFrameSunkenSmall(ref pg, 330-319, 107-65+243, 205, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.fis.NaechsterHalt == "")
				pg.DrawString("             ---", f, new SolidBrush(BLACK), 367-319, 114-65+243);
			else
				pg.DrawString(localstate.fis.NaechsterHalt, f, new SolidBrush(BLACK), 367-319, 114-65+243);



			// Rahmen FIS Status
			DrawFrame(ref pg, 335-108, 164-164+243, 225, 89);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("FIS Status:", f, new SolidBrush(BLACK), 378-95, 175-164+243);
			DrawFrameSunkenSmall(ref pg, 345-108, 206-164+243, 205, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.fis.Status)
                pg.DrawString("EIN", f, new SolidBrush(BLACK), 425-101, 213-164+243);
			else
				pg.DrawString("AUS", f, new SolidBrush(BLACK), 425-101, 213-164+243);


			// Rahmen GPS-Empfang
			DrawFrame(ref pg, 335-108+226, 164-164+243, 175, 89);
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("GPS-Empfang:", f, new SolidBrush(BLACK), 365-95+201, 175-164+243);
			DrawFrameSunkenSmall(ref pg, 345-108+226, 206-164+243, 155, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.fis.GPSStatus)
                pg.DrawString("JA", f, new SolidBrush(BLACK), 425-101+201, 213-164+243);
			else
				pg.DrawString("NEIN", f, new SolidBrush(BLACK), 420-101+201, 213-164+243);
		}

		public void DrawFIS_HK(ref Graphics pg)
		{
			// Nächster Halt
			int y = 90, x = -20;
			DrawFrame(ref pg, 318+x, 41+y, 210, 99);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Nächster Halt:", f, new SolidBrush(BLACK), 363+x, 55+y);
			pg.FillRectangle(new SolidBrush(Color.Gold) ,331+x, 91+y, 188, 28);
			DrawFrameSunkenSmall(ref pg, 330+x, 90+y, 190, 30);
			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString(localstate.fis.NaechsterHalt, f, new SolidBrush(Color.Black), 350+x, 98+y);

			DrawSeitlicheSoftkeys(ref pg);
		}

		public void DrawSeitlicheSoftkeys(ref Graphics pg)
		{
			// Seitliche Soft-Keys
			if (localstate.DISPLAY != CURRENT_DISPLAY.FIS_HK && localstate.DISPLAY != CURRENT_DISPLAY.FIS_ROUTE)
				DrawFrame(ref pg, 518, 43, 110, 48);
			DrawFrame(ref pg, 518, 105, 110, 50);
			DrawFrame(ref pg, 518, 170, 110, 50);
			DrawFrame(ref pg, 518, 235, 110, 112);

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS_HK:
					pg.DrawString("weiter", f, new SolidBrush(BLACK), 533, 121);

					pg.DrawString("zurück", f, new SolidBrush(BLACK), 533, 186);

					pg.DrawString("Über-", f, new SolidBrush(BLACK), 530, 282);
					pg.DrawString("nehmen", f, new SolidBrush(BLACK), 530, 303);
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					pg.DrawString("weiter", f, new SolidBrush(BLACK), 533, 121);

					pg.DrawString("zurück", f, new SolidBrush(BLACK), 533, 186);

					pg.DrawString("Über-", f, new SolidBrush(BLACK), 530, 282);
					pg.DrawString("nehmen", f, new SolidBrush(BLACK), 530, 303);
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
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
					break;
				case CURRENT_DISPLAY.DSK:
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
					break;
			}
		}
		
		public void DrawFIS_Route(ref Graphics pg)
		{
			// Eingestellte Routen
			DrawFrame(ref pg, 1, 41, 315, 200);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Aktuelle Routen:", f, new SolidBrush(BLACK), 5, 65-19);

			f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				pg.FillRectangle(new SolidBrush(Color.Gold), 72, 57-24+i*42, 230, 25);
				pg.DrawString("Zug "+i.ToString(), f, new SolidBrush(BLACK), 10, 70-32+i*42);
				if (localstate.fis.ShowEntry())
                    pg.DrawString(localstate.fis.Linie + " / " + localstate.fis.Ziel, f, new SolidBrush(Color.Black), 100, 70-32+i*42);
				else
					pg.DrawString("", f, new SolidBrush(Color.Black), 100, 70-32+i*42);
				DrawFrameSunkenSmall(ref pg, 72, 57-24+i*42, 230, 25);
			}

			DrawFrame(ref pg, 1, 243, 315, 145);
			
			if (localstate.fis.position == FIS_POS.LINIE)
				pg.FillRectangle(new SolidBrush(Color.Gold), 72, 217+42, 230, 25);
			else if (localstate.fis.position == FIS_POS.START)
				pg.FillRectangle(new SolidBrush(Color.Gold), 72, 217+2*42, 230, 25);
			else if (localstate.fis.position == FIS_POS.ZIEL)
				pg.FillRectangle(new SolidBrush(Color.Gold), 72, 217+3*42, 230, 25);

			DrawFrameSunkenSmall(ref pg, 72, 217+42, 230, 25);
			DrawFrameSunkenSmall(ref pg, 72, 217+2*42, 230, 25);
			DrawFrameSunkenSmall(ref pg, 72, 217+3*42, 230, 25);

			string linie = "(" + localstate.fis.GetNetz() + ") / " + localstate.fis.GetLinie();
			string start = localstate.fis.GetStart();
			string ziel = localstate.fis.GetZiel();
            
			pg.DrawString("Linie", f, new SolidBrush(BLACK), 10, 217+42+3);
			pg.DrawString("Start", f, new SolidBrush(BLACK), 10, 217+2*42+3);
			pg.DrawString("Ziel", f, new SolidBrush(BLACK), 10, 217+3*42+3);
			pg.DrawString(linie, f, new SolidBrush(Color.Black), 100, 217+42+3);
			pg.DrawString(start, f, new SolidBrush(Color.Black), 100, 217+42*2+3);
			pg.DrawString(ziel, f, new SolidBrush(Color.Black), 100, 217+42*3+3);

			DrawSeitlicheSoftkeys(ref pg);
		}

		public void DrawSchaltfunktionen(ref Graphics pg)
		{
			DrawFrame(ref pg, 0, 42, 315, 286); // Antrieb
			DrawFrame(ref pg, 318, 42, 230, 160); // Absperrungen
			DrawFrame(ref pg, 318, 205, 230, 60); // Kuehlung
			DrawFrame(ref pg, 318, 268, 230, 60); // Oberstrom


			// Antrieb
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			// Zug 1
			pg.DrawString("Antriebsanlage 1.1", f, new SolidBrush(BLACK), 16, 60);
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60, 41, 25, DARK, BRIGHT);
			f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64);

			if (localstate.ET42Xtype1 != ET42XTYPE.ET426)
			{
				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Antriebsanlage 1.2", f, new SolidBrush(BLACK), 16, 60+30);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60+30, 41, 25, DARK, BRIGHT);
				f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);			
				pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64+30);
			}
                                                                           	
			if (localstate.ET42Xtype2 != ET42XTYPE.NONE)
			{
				// Zug 2
				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Antriebsanlage 2.1", f, new SolidBrush(BLACK), 16, 60+60);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60+60, 41, 25, DARK, BRIGHT);
				
				f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);			
				pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64+60);

				if (localstate.ET42Xtype2 != ET42XTYPE.ET426)
				{
					f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString("Antriebsanlage 2.2", f, new SolidBrush(BLACK), 16, 60+90);
					MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60+90, 41, 25, DARK, BRIGHT);
					f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);			
					pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64+90);
				}
			}

			if (localstate.ET42Xtype3 != ET42XTYPE.NONE)
			{
				// Zug 3
				f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
				pg.DrawString("Antriebsanlage 3.1", f, new SolidBrush(BLACK), 16, 60+120);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60+120, 41, 25, DARK, BRIGHT);

				f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);			
				pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64+120);
				
				if (localstate.ET42Xtype3 != ET42XTYPE.ET426)
				{
					f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
					pg.DrawString("Antriebsanlage 3.2", f, new SolidBrush(BLACK), 16, 60+150);
					MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 240, 60+150, 41, 25, DARK, BRIGHT);

					f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);			
					pg.DrawString("Ein", f, new SolidBrush(BLACK), 245, 64+150);
				}
			}

			// bspdarstellung

			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("eingr.", f, new SolidBrush(BLACK), 6, 233+63);
			pg.DrawString("ausgr.", f, new SolidBrush(BLACK), 106, 233+63);
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 65, 233+63, 25, 25, DARK, BRIGHT);
			pg.FillRectangle(new SolidBrush(Color.Gold), 169, 233+63, 25, 25);
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 170, 233+63, 25, 25, DARK, BRIGHT);



			// Absperrungen
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Absperrungen im Zug", f, new SolidBrush(BLACK), 326, 48);

			if (localstate.ET42Xtype1 != ET42XTYPE.ET426)
			{
				// Zug 1
				pg.DrawString("Zug 1", f, new SolidBrush(BLACK), 326, 74);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 390, 74, 140, 25, DARK, BRIGHT);
			}

			if (localstate.ET42Xtype2 != ET42XTYPE.NONE && localstate.ET42Xtype2 != ET42XTYPE.ET426)
			{
				// Zug 2
				pg.DrawString("Zug 2", f, new SolidBrush(BLACK), 326, 74+30);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 390, 74+30, 140, 25, DARK, BRIGHT);
			}

			if (localstate.ET42Xtype3 != ET42XTYPE.NONE && localstate.ET42Xtype3 != ET42XTYPE.ET426)
			{
				// Zug 3
				pg.DrawString("Zug 3", f, new SolidBrush(BLACK), 326, 74+60);
				MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 390, 74+60, 140, 25, DARK, BRIGHT);
			}

			// Kuehlung
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Status Kühlung", f, new SolidBrush(BLACK), 326, 210);
			pg.DrawString("Fahrgastraum:", f, new SolidBrush(BLACK), 326, 235);

			f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.Kuehlung)
				pg.DrawString("Ein", f, new SolidBrush(BLACK), 495, 237);
			else
				pg.DrawString("Aus", f, new SolidBrush(BLACK), 495, 237);
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 491, 233, 41, 25, DARK, BRIGHT);

			// Oberstrom
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Oberstromgrenze", f, new SolidBrush(BLACK), 326, 210+63);

			f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString(localstate.OberstromGrenze.ToString()+" A", f, new SolidBrush(BLACK), 473, 237+63);
			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 471, 233+63, 61, 25, DARK, BRIGHT);
		}
		public void DrawEingabeOberstrom(ref Graphics pg)
		{
			DrawFrame(ref pg, 0, 42, 315, 100);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Oberstrombegrenzung", f, new SolidBrush(BLACK), 20, 60);

			f = new Font("Arial", 4.5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (localstate.OberstromGrenze < 1000f)
                pg.DrawString(localstate.OberstromGrenze.ToString()+" A", f, new SolidBrush(BLACK), 195, 105);
			else
				pg.DrawString(localstate.OberstromGrenze.ToString()+" A", f, new SolidBrush(BLACK), 190, 105);

			MMI.EBuLa.Tools.DisplayDraw.DrawFrameSunkenSmall(ref pg, 175, 100, 90, 25, DARK, BRIGHT);
		}
		public void DrawSpannung(ref Graphics pg)
		{
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				DrawKastenSpannung(ref g, i); 
			}
		}
		public void DrawInfo(ref Graphics pg)
		{
		}
		
		public void DrawBesetzungsgrad(ref Graphics pg)
		{
			for (int i = 1; i <= GetTrainCount(); i++)
			{
				DrawKastenBesetztgrad(ref g, i); 
			}
		}

		public void DrawVist(ref Graphics pg)
		{
			DrawFrame(ref pg, 0, 40, 628, 295);

			DrawFrame(ref pg, 80, 120, 420, 120);
			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (!inverse_display)
                pg.FillRectangle(new SolidBrush(this.BackColor), 90, 110, 160, 20);
			else
				pg.FillRectangle(new SolidBrush(Color.Black), 90, 110, 160, 20);

			pg.DrawString("Geschwindigkeit", f, new SolidBrush(BLACK), 90, 110);

			f = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Millimeter);
			if (Convert.ToInt32(localstate.Geschwindigkeit) >= 100)
				pg.DrawString(Convert.ToInt32(localstate.Geschwindigkeit).ToString(), f, new SolidBrush(BLACK), 165, 140);
			else if (Convert.ToInt32(localstate.Geschwindigkeit) >= 10)
				pg.DrawString(Convert.ToInt32(localstate.Geschwindigkeit).ToString(), f, new SolidBrush(BLACK), 205, 140);
			else
				pg.DrawString(Convert.ToInt32(localstate.Geschwindigkeit).ToString(), f, new SolidBrush(BLACK), 240, 140);

			pg.DrawString("km/h", f, new SolidBrush(BLACK), 300, 140);
			
		}

		public void DrawBremszustand(ref Graphics pg)
		{
			DrawFrame(ref pg, 33, 78, 564, 244); // 560x240

			DrawFrameNewStyle(ref pg, 35, 80, 159, 89);
			DrawFrameNewStyle(ref pg, 35, 170, 159, 29);
			DrawFrameNewStyle(ref pg, 35, 200, 159, 29);
			DrawFrameNewStyle(ref pg, 35, 230, 159, 29);
			DrawFrameNewStyle(ref pg, 35, 260, 159, 29);
			DrawFrameNewStyle(ref pg, 35, 290, 560, 29);

			for (int i = 0; i < GetTrainCount(); i++)
			{
				ET42XTYPE current_type;
				if (i == 0)
					current_type = localstate.ET42Xtype1;
				else if (i == 1)
					current_type = localstate.ET42Xtype2;
				else current_type = localstate.ET42Xtype3;

				// Kasten
				DrawFrameNewStyle(ref pg, 35+i*100+160, 80, 99, 59);

				DrawFrameNewStyle(ref pg, 35+i*100+160, 140, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+25, 140, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+75, 140, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+50, 140, 24, 29);

				DrawFrameNewStyle(ref pg, 35+i*100+160, 170, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+25, 170, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+50, 170, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+75, 170, 24, 29);

				DrawFrameNewStyle(ref pg, 35+i*100+160, 200, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+25, 200, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+50, 200, 24, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160+75, 200, 24, 29);

				DrawFrameNewStyle(ref pg, 35+i*100+160, 230, 99, 29);
				DrawFrameNewStyle(ref pg, 35+i*100+160, 260, 99, 29);


				// Schrift
				pg.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
				Font f = new Font("Arial", 4.4f, FontStyle.Bold, GraphicsUnit.Millimeter);

				pg.DrawString("aktueller", f, new SolidBrush(BLACK), 45, 114);
				pg.DrawString("Zustand", f, new SolidBrush(BLACK), 45, 134);

				pg.DrawString("Zug "+(i+1).ToString(), f, new SolidBrush(BLACK), 35+i*100+166+20, 88);
				pg.DrawString("TD", f, new SolidBrush(BLACK), 35+i*100+166+32, 114);

				pg.DrawString("Bremsenprüfung", f, new SolidBrush(BLACK), 40, 176);
				pg.DrawString("Geleitschutz", f, new SolidBrush(BLACK), 40, 206);
				pg.DrawString("Kompressor", f, new SolidBrush(BLACK), 40, 236);
				pg.DrawString("HBL-Leitung", f, new SolidBrush(BLACK), 40, 266);
				pg.DrawString("Ergebnis der aktuellen Bremshundertstel-Berechnung: BRH = "+localstate.Brh+" %", f, new SolidBrush(BLACK), 40, 296);

				pg.DrawString("1", f, new SolidBrush(BLACK), 35+i*100+166, 146);
				pg.DrawString("2", f, new SolidBrush(BLACK), 35+i*100+166+25, 146);
				if (current_type != ET42XTYPE.ET426)
				{
					pg.DrawString("3", f, new SolidBrush(BLACK), 35+i*100+166+50, 146);
					pg.DrawString("4", f, new SolidBrush(BLACK), 35+i*100+166+75, 146);
				}


			}

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

			if (inverse_display)
                g.Clear(Color.Black);
			else
				g.Clear(Color.LightGray);

			SetButtons();

			DrawDatum(ref g);
			DrawUhr(ref g);

			DrawTitle(ref g);

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					DrawZugkraft(ref g);

					for (int i = 1; i <= GetTrainCount(); i++)
					{
						DrawKasten(ref g, i); 
					}
					
					DrawStörung(ref g);
					break;
				case CURRENT_DISPLAY.S:
					DrawSchaltfunktionen(ref g);
					break;
				case CURRENT_DISPLAY.OBERSTROM:
					DrawEingabeOberstrom(ref g);
					break;
				case CURRENT_DISPLAY.Spg:
					DrawSpannung(ref g);
					break;
				case CURRENT_DISPLAY.Info:
					DrawInfo(ref g);
					break;
				case CURRENT_DISPLAY.Vist:
					DrawVist(ref g);
					DrawStörung(ref g);
					break;
				case CURRENT_DISPLAY.Besetz:
					DrawBesetzungsgrad(ref g);
					break;
				case CURRENT_DISPLAY.B:
					DrawBremszustand(ref g);
					break;
				case CURRENT_DISPLAY.Zugbesy:
					DrawZug(ref g);
					DrawZugbeeinflussungssysteme(ref g);
					DrawStörung(ref g);
					//DrawStatus(ref g);
					break;
				case CURRENT_DISPLAY.FIS:
					DrawFIS(ref g);
					break;
				case CURRENT_DISPLAY.FIS_HK:
					DrawFIS_HK(ref g);
					break;
				case CURRENT_DISPLAY.FIS_ROUTE:
					DrawFIS_Route(ref g);
					break;
			}

			if (USE_DOUBLE_BUFFER)
			{
				//g.Dispose();

				//Copy the back buffer to the screen
				e.Graphics.DrawImageUnscaled(m_backBuffer,0,0);
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
				if (stoerung_counter == 30 && GLOBAL_SOUND)
				{
					stoerung_counter = 0;
					Sound.PlayMalfunctionBombardierSound();
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

		private string TrainType(ET42XTYPE type)
		{
			switch (type)
			{
				case ET42XTYPE.ET423:
					return "423";
				case ET42XTYPE.ET424:
					return "424";
				case ET42XTYPE.ET425:
					return "425";
				case ET42XTYPE.ET426:
					return "426";
			}
			return "";
		}
		private string TitleString()
		{
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.FIS:
					return "                            FIS Übersicht";
				case CURRENT_DISPLAY.FIS_HK:
					return "                      Haltestellen Korrektur";
				case CURRENT_DISPLAY.FIS_ROUTE:
					return "                             Routeneingabe";
				case CURRENT_DISPLAY.S:
					return "                 Übersicht Schaltfunktionen";
				case CURRENT_DISPLAY.OBERSTROM:
					return "               Eingabe Oberstrombegrenzung";
				case CURRENT_DISPLAY.Spg:
					return "                       Spannungsanzeige";
				case CURRENT_DISPLAY.Info:
					return "                                Info";
				case CURRENT_DISPLAY.Vist:
					return "                    Geschwindigkeit Ersatzbild";
				case CURRENT_DISPLAY.Besetz:
					return "                        Besetzungsgrad";
				case CURRENT_DISPLAY.B:
					return "                        Bremsenzustand";
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


            
			something_changed = true;
			inverse_display = !inverse_display;
		}
		public int GetTrainCount()
		{
			int i = 1;
			if (localstate.ET42Xtype2 != ET42XTYPE.NONE)
				i++; 
			if (localstate.ET42Xtype3 != ET42XTYPE.NONE)
				i++;
            return i;
		}
		public float ReduziereZugkraft(float zugkraft, int tz)
		{
			switch(tz)
			{
/*				case 1:
					if (localstate.ET42Xtype1 == ET42XTYPE.ET426)
						return zugkraft;
					break;*/
				case 2:
                    if (localstate.ET42Xtype1 == ET42XTYPE.ET426 && localstate.ET42Xtype2 != ET42XTYPE.ET426)
						if (zugkraft < 0)
							return zugkraft * 2f * (65f/90f);
						else
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
			return zugkraft;
		}
	}
}
