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

using MMI.EBuLa.Tools;


namespace MMI.DIAGNOSE
{	
	public class DIAGNOSEControl : System.Windows.Forms.UserControl
	{
		const bool USE_DOUBLE_BUFFER = true;
		const int ZUGKRAFT = 96;
		const int BREMSKRAFT = 120;
		const string DSK_SPERRCODE = "1234";

		#region Declarations
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		int stoerung_counter = 0;
		bool inverse_display = false;

		private Bitmap m_backBuffer;
		private Graphics g;

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
		private System.Windows.Forms.Timer timer_eingabe;

		bool on = true;

		#endregion
		public DIAGNOSEControl(MMI.EBuLa.Tools.XMLLoader conf, ref MMI.MMIBR185.BR185Control mmi)
		{
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
			something_changed = true;


			// NOTBREMSE und E-BREMSE fehlen

			vtime = DateTime.Now;

			zugkraft_thread = new Thread(new ThreadStart(MoveZugkraft));

			int interval = Convert.ToInt32(Math.Round((1d/(double)conf.FramesPerSecond)*1000d));
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
			this.timer_eingabe = new System.Windows.Forms.Timer(this.components);
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
			this.timer1.Interval = 1;
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
			this.l_Button2.Location = new System.Drawing.Point(6, 8);
			this.l_Button2.Name = "l_Button2";
			this.l_Button2.Size = new System.Drawing.Size(48, 48);
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
			// timer_eingabe
			// 
			this.timer_eingabe.Enabled = true;
			this.timer_eingabe.Interval = 1000;
			this.timer_eingabe.Tick += new System.EventHandler(this.timer_eingabe_Tick);
			// 
			// DIAGNOSEControl
			// 
			this.BackColor = System.Drawing.Color.LightGray;
			this.Controls.Add(this.p_UpperLine);
			this.Controls.Add(this.p_Buttons);
			this.Name = "DIAGNOSEControl";
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
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					localstate.ZugnummerTemp = "";
					localstate.marker_changed = false;
					localstate.TfnummerTemp = "";
					localstate.marker_changed2 = false;
					localstate.marker = 0;
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.DSK:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
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
					localstate.DISPLAY = CURRENT_DISPLAY.Zugbesy;
					break;
				case CURRENT_DISPLAY.Z_BR:
					if (IsBombardier())
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
				case CURRENT_DISPLAY.Zugbesy:
					localstate.DISPLAY = CURRENT_DISPLAY.DSK;
					break;
				case CURRENT_DISPLAY.G:
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
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zugbesy:
					localstate.DISPLAY = CURRENT_DISPLAY.G;
					break;
				case CURRENT_DISPLAY.Z_BR:
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
									if (IsBombardier())
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
									if (IsBombardier())
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
			if (IsBombardier())
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
			// Stift
			Pen pen_dg = new Pen(DARK, 1);
			Pen pen_ws = new Pen(BRIGHT, 1);

			pg.DrawLine(pen_ws, x, y, x+width, y);
			pg.DrawLine(pen_ws, x, y, x,       y+height);

			pg.DrawLine(pen_dg, x+width, y,        x+width, y+height);
			pg.DrawLine(pen_dg, x,       y+height, x+width, y+height);
		}

		private void DrawFrameSunken(ref Graphics pg, int x, int y, int width, int height)
		{
			// Stift
			Pen pen_dg = new Pen(DARK, 1);
			Pen pen_ws = new Pen(BRIGHT, 2);

			pg.DrawLine(pen_dg, x, y, x+width, y);
			pg.DrawLine(pen_dg, x, y, x,       y+height);

			pg.DrawLine(pen_ws, x+width, y,        x+width, y+height);
			pg.DrawLine(pen_ws, x,       y+height, x+width, y+height);
		}
		private void DrawFrameSunkenSmall(ref Graphics pg, int x, int y, int width, int height)
		{
			// Stift
			Pen pen_dg = new Pen(DARK, 1);
			Pen pen_ws = new Pen(BRIGHT, 1);

			pg.DrawLine(pen_dg, x, y, x+width, y);
			pg.DrawLine(pen_dg, x, y, x,       y+height);

			pg.DrawLine(pen_ws, x+width, y,        x+width, y+height);
			pg.DrawLine(pen_ws, x,       y+height, x+width, y+height);
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
		#endregion
		public void DrawZugkraft(ref Graphics pg)
		{
			// Rahmen
			DrawFrame(ref pg, 508, 281, 120, 38);

			float zugkraft = localstate.ZugkraftGesammt;

			zugkraft = ReduziereZugkraft(localstate.ZugkraftGesammt, 1) * 2f;

			/*
			if (localstate.VT612type2 != VT612TYPE.NONE)
				zugkraft += ReduziereZugkraft(localstate.ZugkraftGesammt, 2) * 2f;

			if (localstate.VT612type3 != VT612TYPE.NONE)
				zugkraft += ReduziereZugkraft(localstate.ZugkraftGesammt, 3) * 2f;
				*/

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

		public void DrawUhr(ref Graphics pg)
		{
			if (localstate.DISPLAY == CURRENT_DISPLAY.G || localstate.DISPLAY == CURRENT_DISPLAY.Zugbesy || localstate.DISPLAY == CURRENT_DISPLAY.Z_BR)
			{
				// Uhr analog
				// Rahmen
				DrawFrame(ref pg, 468, 30, 160, 160);

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

		public void DrawSpannung(ref Graphics pg)
		{
			if (IsBombardier()) DrawSpannungBombardier(ref pg);
			else DrawSpannungSiemens(ref pg);
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
			f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);
			pg.DrawString("Spannung", f, new SolidBrush(BLACK), 500, 230);
			pg.DrawString("Oberstrom", f, new SolidBrush(BLACK), 500, 255);
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
			if (IsBombardier())
				DrawOberstromBombardier(ref pg);
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
				
				Störung st = localstate.störungmgr.Current;

				s = "St. in "+st.Name;
				pg.DrawString(s, f, new SolidBrush(StTextColor), 462, 352);
				if (IsSiemens()) DrawStatusStörRahmen(ref pg, true, false);
			}
			else if (localstate.störungmgr.GetPassives().Count > 1 || INIT)
			{
				if (STÖRUNG_BG == Color.Gold)
				{
					if (inverse_display)
						pg.FillRectangle(new SolidBrush(MMI_ORANGE), 438, 338, 190, 50);
					else
						pg.FillRectangle(new SolidBrush(STÖRUNG_BG), 438, 338, 190, 50);
				}				
				s = "          St";
				pg.DrawString(s, f, new SolidBrush(StTextColor), 462, 352);
				if (IsSiemens()) DrawStatusStörRahmen(ref pg, true, false);
			}
				

			pg.SmoothingMode = SMOOTHING_MODE;

		}
		public void DrawStatus(ref Graphics pg)
		{
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
					pg.FillRectangle(new SolidBrush(Color.Gold), 196, 338, 242, 50);
					s = s.Substring(1, s.Length-1);
					pg.DrawString(s, f, new SolidBrush(StöTextColor), 208, 352);
				}
				else
				{
					pg.FillRectangle(new SolidBrush(STATUS_BLUE), 196, 338, 242, 50);
					if (s.Substring(0,1) == "!") s = s.Substring(1, s.Length-1);
					pg.DrawString(s, f, new SolidBrush(StaTextColor), 208, 352);
				}
				
				

				if (IsSiemens()) DrawStatusStörRahmen(ref pg, false, true);
			}

			pg.SmoothingMode = SMOOTHING_MODE;
		}
		public void DrawStatusStörRahmen(ref Graphics pg, bool störung, bool status)
		{
			//Stör
			if (störung) DrawFrame(ref pg, 438, 338, 190, 50);

			//Status
			if (status) DrawFrame(ref pg, 196, 338, 242, 50);

			if (IsBombardier())
			{	
				//kleine Felder
				DrawFrame(ref pg, 145, 338, 50, 50);
				DrawFrame(ref pg, 94, 338, 50, 50);

				// großer Rahmen
				DrawFrame(ref pg, 1, 335, 629, 56);
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
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f));
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft));
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
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
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
					iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 6.25f);
					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
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

			if (height > 250) height = 250;

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
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f));
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft));
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
					iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 5.208333f);
					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					iheight = Convert.ToInt32(localstate.E_Bremse * 39.0625f);
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
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * 4f * localstate.traction));
				b = new SolidBrush(MMI_BLUE);
			}
			else
			{
				zk = Convert.ToInt32(Math.Abs(localstate.Zugkraft * localstate.traction));
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
					iheight = Convert.ToInt32(localstate.FahrstufenSchalter * 6.6796875f);
					b = new SolidBrush(MMI_BLUE);
				}
				else
				{
					iheight = Convert.ToInt32(localstate.E_Bremse * 44.53125f);
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
				pg.DrawLine(Pens.WhiteSmoke, plus_s_1, plus_s_2);
			}
		}
		private void DrawST(ref Graphics pg)
		{
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
			if (localstate.DISPLAY == CURRENT_DISPLAY.G || localstate.DISPLAY == CURRENT_DISPLAY.Zugbesy || localstate.DISPLAY == CURRENT_DISPLAY.Z_BR)
			{
				// Datum oben rechts
				DrawFrame(ref pg, 468, 1, 160, 30);

				string s = "";

				s = DayOfWeekGerman(vtime.DayOfWeek).Substring(0, 2) + ", ";	
				if (vtime.DayOfYear < 10)
					s += "0"+vtime.DayOfYear.ToString() + ".";
				else
					s += vtime.DayOfYear.ToString() + ".";
				if (vtime.Month < 10)
					s += "0"+vtime.Month.ToString() + ".";
				else
					s += vtime.Month.ToString() + ".";
				s += vtime.Year.ToString();

				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

				pg.DrawString(s, f, new SolidBrush(BLACK), 498, 8);
			}
			else
			{
				// Datum oben links
				DrawFrame(ref pg, 1, 1, 120, 38);

				string s = "";

				s = DayOfWeekGerman(vtime.DayOfWeek).Substring(0, 2) + ", ";	
				if (vtime.DayOfYear < 10)
					s += "0"+vtime.DayOfYear.ToString() + ".";
				else
					s += vtime.DayOfYear.ToString() + ".";
				if (vtime.Month < 10)
					s += "0"+vtime.Month.ToString() + ".";
				else
					s += vtime.Month.ToString() + ".";
				s += vtime.Year.ToString();

				Font f = new Font("Arial", 4, FontStyle.Bold, GraphicsUnit.Millimeter);

				pg.DrawString(s, f, new SolidBrush(BLACK), 10, 12);
			}

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
			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.G:
					if (IsBombardier())
                        l_Button1.Text = "An-triebe";
					else
						l_Button1.Text = "";
					l_Button2.Text = "W";
					l_Button3.Text = "";
					l_Button4.Text = "Zug- Besy";
					l_Button5.Text = "";
					if (IsMMI())
                        l_Button6.Text = "Ein Displ.";
					else
						l_Button6.Text = "";
					l_Button7.Text = "Z / Br";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "";
					break;
				case CURRENT_DISPLAY.Z_BR:
					if (IsBombardier())
						l_Button1.Text = "An-triebe";
					else
						l_Button1.Text = "";
					l_Button2.Text = "W";
					l_Button3.Text = "";
					if (IsBombardier())
						l_Button4.Text = "Zug- Besy";
					else
						l_Button4.Text = "";
					l_Button5.Text = "";
					if (IsMMI())
						l_Button6.Text = "Ein Displ.";
					else
						l_Button6.Text = "";
					l_Button7.Text = "";
					l_Button8.Text = "";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.Zugbesy:
					l_Button1.Text = "ZDE";
					l_Button2.Text = "Zug-/ Tf-Nr.";
					l_Button3.Text = "Lok-DE";
					l_Button4.Text = "";
					l_Button5.Text = "";
					l_Button6.Text = "";
					l_Button7.Text = "DSK";
					if (IsBombardier())
						l_Button8.Text = "Prü-fen";
					else
						l_Button8.Text = "Prü-lauf";
					l_Button9.Text = "";
					l_Button0.Text = "G";
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					l_Button1.Text = "1";
					l_Button2.Text = "2";
					l_Button3.Text = "3";
					l_Button4.Text = "4";
					l_Button5.Text = "5";
					l_Button6.Text = "6";
					l_Button7.Text = "7";
					l_Button8.Text = "8";
					l_Button9.Text = "9";
					l_Button0.Text = "0";
					break;
				case CURRENT_DISPLAY.DSK:
					l_Button1.Text = "1";
					l_Button2.Text = "2";
					l_Button3.Text = "3";
					l_Button4.Text = "4";
					l_Button5.Text = "5";
					l_Button6.Text = "6";
					l_Button7.Text = "7";
					l_Button8.Text = "8";
					l_Button9.Text = "9";
					l_Button0.Text = "0";
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

			if (IsBombardier())
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
					pg.DrawString("DSK Kurzzeitspeicher geperrt", f, new SolidBrush(BLACK), 60, 310);
			}

		}
		public void DrawSeitlicheSoftkeys(ref Graphics pg)
		{
			// Seitliche Soft-Keys
			DrawFrame(ref pg, 518, 43, 110, 48);
			DrawFrame(ref pg, 518, 105, 110, 50);
			DrawFrame(ref pg, 518, 170, 110, 50);
			DrawFrame(ref pg, 518, 235, 110, 112);

			Font f = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

			switch(localstate.DISPLAY)
			{
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					if (IsBombardier())
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
					if (IsBombardier())
					{
						pg.DrawString("Grundbild", f, new SolidBrush(BLACK), 525, 56);
						pg.DrawString("zurück", f, new SolidBrush(BLACK), 537, 120);
						pg.DrawString("weiter", f, new SolidBrush(BLACK), 540, 185);

						pg.DrawString("Kurzzeit-", f, new SolidBrush(BLACK), 538, 264);
						pg.DrawString("speicher", f, new SolidBrush(BLACK), 538, 282);
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
						pg.DrawString("sperren", f, new SolidBrush(BLACK), 538, 299);
					}
					break;
			}
		}
		public void DrawZugTfNummer(ref Graphics pg)
		{
			DrawSeitlicheSoftkeys(ref pg);

			// großer Rahmen
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
						if (IsBombardier())
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
					else if (!localstate.marker_changed || localstate.marker > 5)
						if (IsBombardier())
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
						if (IsBombardier())
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Black, 110+i*48, y +19);
						else
							pg.DrawString(nr.Substring(i, 1), f, Brushes.Blue, 110+i*48, y +19);
					else if (!localstate.marker_changed2 || localstate.marker-6 > 5)
						if (IsBombardier())
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
						if (IsBombardier())
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
					if (IsBombardier())	DrawStatusStörRahmen(ref g, true, true);
					break;
				case CURRENT_DISPLAY.Z_BR:
					DrawZug(ref g);
					DrawZugkraftBalken(ref g);
					DrawOberstrom(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier())	DrawStatusStörRahmen(ref g, true, true);
					break;
				case CURRENT_DISPLAY.Zugbesy:
					DrawZug(ref g);
					DrawZugbeeinflussungssysteme(ref g);
					DrawStörung(ref g);
					DrawStatus(ref g);
					if (IsBombardier())	DrawStatusStörRahmen(ref g, true, true);					
					break;
				case CURRENT_DISPLAY.Zug_Tf_Nr:
					DrawTitle(ref g);
					DrawZugTfNummer(ref g);
					break;
				case CURRENT_DISPLAY.DSK:
					DrawTitle(ref g);
					DrawDSK(ref g);
					break;
				default:
					DrawTitle(ref g);
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
		private string DayOfWeekGerman(DayOfWeek dayofweek)
		{
			switch (dayofweek)
			{
				case DayOfWeek.Monday:
					return "Montag";
				case DayOfWeek.Tuesday:
					return "Dienstag";
				case DayOfWeek.Wednesday:
					return "Mittwoch";
				case DayOfWeek.Thursday:
					return "Donnerstag";
				case DayOfWeek.Friday:
					return "Freitag";
				case DayOfWeek.Saturday:
					return "Samstag";
				case DayOfWeek.Sunday:
					return "Sonntag";
			}
			return "";

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
					if (IsBombardier())
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
		public bool IsBombardier()
		{
			return (localstate.type == TRAIN_TYPE.BR101 || localstate.type == TRAIN_TYPE.BR145 || localstate.type == TRAIN_TYPE.BR146 || localstate.type == TRAIN_TYPE.BR146_1 || localstate.type == TRAIN_TYPE.BR185);
		}
		public bool IsSiemens()
		{
			return !IsBombardier();
		}
		public bool IsMMI()
		{
			return (localstate.type == TRAIN_TYPE.BR185 || localstate.type == TRAIN_TYPE.BR146_1 || localstate.type == TRAIN_TYPE.BR189);
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
	}
}
