using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net.Sockets;

using MMI.SystemTools;
using MMI.EBuLa.Tools;

namespace MMI.EBuLa
{
    /// <summary>
    /// Zusammenfassung für UserControl1.
    /// </summary>
    public class EbulaControl : System.Windows.Forms.UserControl
    {
		const bool USE_DOUBLE_BUFFER = false;
		const SmoothingMode SMOOTHING_MODE = SmoothingMode.AntiAlias;
		const TextRenderingHint TEXT_MODE = TextRenderingHint.AntiAliasGridFit;

		bool SomethingChanged = false;

		#region Members
        public System.DateTime vtime;
        private System.Timers.Timer vtimer;
        private System.DateTime vdifftime;
        public  int verspaetung;
        private FSD f = null;

		Color BLACK = Color.Black;
		Hashtable ht;

        private KeyHandler kh = new KeyHandler(null);

        private int firsttime = 3;

		private XMLLoader m_conf;

		public bool isGNT = true;

		private Bitmap m_backBuffer;
		private Graphics g;

		private bool fsdwason = false;

		private Network net;
		public Thread t;

        public Control control = null;
        private System.Timers.Timer timer_Uhr;
        private System.Windows.Forms.Label L_Uhr;
        private System.Windows.Forms.Label L_Zeit;
        private System.Windows.Forms.Label L_Zug;
        private System.Windows.Forms.Label L_Valid;
        private System.Windows.Forms.Label L_Speed_12;
        private System.Windows.Forms.Label L_Speed_11;
        private System.Windows.Forms.Label L_Speed_10;
        private System.Windows.Forms.Label L_Speed_09;
        private System.Windows.Forms.Label L_Speed_08;
        private System.Windows.Forms.Label L_Speed_07;
        private System.Windows.Forms.Label L_Speed_06;
        private System.Windows.Forms.Label L_Speed_05;
        private System.Windows.Forms.Label L_Speed_04;
        private System.Windows.Forms.Label L_Speed_03;
        private System.Windows.Forms.Label L_Speed_02;
        private System.Windows.Forms.Label L_Speed_01;
        private System.Windows.Forms.Label L_Pos_12;
        private System.Windows.Forms.Label L_Pos_11;
        private System.Windows.Forms.Label L_Pos_10;
        private System.Windows.Forms.Label L_Pos_09;
        private System.Windows.Forms.Label L_Pos_08;
        private System.Windows.Forms.Label L_Pos_07;
        private System.Windows.Forms.Label L_Pos_06;
        private System.Windows.Forms.Label L_Pos_05;
        private System.Windows.Forms.Label L_Pos_04;
        private System.Windows.Forms.Label L_Pos_03;
        private System.Windows.Forms.Label L_Pos_02;
        private System.Windows.Forms.Label L_Pos_01;
        private System.Windows.Forms.Label L_Ops_01;
        private System.Windows.Forms.Label L_Ops_02;
        private System.Windows.Forms.Label L_Ops_03;
        private System.Windows.Forms.Label L_Ops_04;
        private System.Windows.Forms.Label L_Ops_05;
        private System.Windows.Forms.Label L_Ops_06;
        private System.Windows.Forms.Label L_Ops_07;
        private System.Windows.Forms.Label L_Ops_08;
        private System.Windows.Forms.Label L_Ops_09;
        private System.Windows.Forms.Label L_Ops_10;
        private System.Windows.Forms.Label L_Ops_11;
        private System.Windows.Forms.Label L_Ops_12;
        private System.Windows.Forms.Label L_TOP;
        private System.Windows.Forms.Label L_DOWN;
        private System.Windows.Forms.Label L_AK_01;
        private System.Windows.Forms.Label L_AK_02;
        private System.Windows.Forms.Label L_AK_03;
        private System.Windows.Forms.Label L_AK_04;
        private System.Windows.Forms.Label L_AK_05;
        private System.Windows.Forms.Label L_AK_06;
        private System.Windows.Forms.Label L_AK_07;
        private System.Windows.Forms.Label L_AK_08;
        private System.Windows.Forms.Label L_AK_09;
        private System.Windows.Forms.Label L_AK_10;
        private System.Windows.Forms.Label L_AK_11;
        private System.Windows.Forms.Label L_AK_12;
        private System.Windows.Forms.Label L_A_01;
        private System.Windows.Forms.Label L_A_02;
        private System.Windows.Forms.Label L_A_03;
        private System.Windows.Forms.Label L_A_04;
        private System.Windows.Forms.Label L_A_05;
        private System.Windows.Forms.Label L_A_06;
        private System.Windows.Forms.Label L_A_07;
        private System.Windows.Forms.Label L_A_08;
        private System.Windows.Forms.Label L_A_09;
        private System.Windows.Forms.Label L_A_10;
        private System.Windows.Forms.Label L_A_11;
        private System.Windows.Forms.Label L_A_12;
        private System.Windows.Forms.Label L_D_12;
        private System.Windows.Forms.Label L_D_11;
        private System.Windows.Forms.Label L_D_10;
        private System.Windows.Forms.Label L_D_09;
        private System.Windows.Forms.Label L_D_08;
        private System.Windows.Forms.Label L_D_07;
        private System.Windows.Forms.Label L_D_06;
        private System.Windows.Forms.Label L_D_05;
        private System.Windows.Forms.Label L_D_04;
        private System.Windows.Forms.Label L_D_03;
        private System.Windows.Forms.Label L_D_02;
        private System.Windows.Forms.Label L_D_01;
        private System.Windows.Forms.Label L_DK_01;
        private System.Windows.Forms.Label L_DK_02;
        private System.Windows.Forms.Label L_DK_03;
        private System.Windows.Forms.Label L_DK_04;
        private System.Windows.Forms.Label L_DK_05;
        private System.Windows.Forms.Label L_DK_06;
        private System.Windows.Forms.Label L_DK_07;
        private System.Windows.Forms.Label L_DK_08;
        private System.Windows.Forms.Label L_DK_09;
        private System.Windows.Forms.Label L_DK_10;
        private System.Windows.Forms.Label L_DK_11;
        private System.Windows.Forms.Label L_DK_12;
        private System.Windows.Forms.Label L_OpsS_01;
        private System.Windows.Forms.Label L_OpsS_02;
        private System.Windows.Forms.Label L_OpsS_03;
        private System.Windows.Forms.Label L_OpsS_04;
        private System.Windows.Forms.Label L_OpsS_05;
        private System.Windows.Forms.Label L_OpsS_06;
        private System.Windows.Forms.Label L_OpsS_07;
        private System.Windows.Forms.Label L_OpsS_08;
        private System.Windows.Forms.Label L_OpsS_09;
        private System.Windows.Forms.Label L_OpsS_10;
        private System.Windows.Forms.Label L_OpsS_11;
        private System.Windows.Forms.Label L_OpsS_12;
        private System.Windows.Forms.PictureBox Qu;
        private System.Windows.Forms.Timer timer_Position;
        private System.Windows.Forms.PictureBox PB_12;
        private System.Windows.Forms.PictureBox PB_11;
        private System.Windows.Forms.PictureBox PB_10;
        private System.Windows.Forms.PictureBox PB_09;
        private System.Windows.Forms.PictureBox PB_08;
        private System.Windows.Forms.PictureBox PB_07;
        private System.Windows.Forms.PictureBox PB_06;
        private System.Windows.Forms.PictureBox PB_05;
        private System.Windows.Forms.PictureBox PB_04;
        private System.Windows.Forms.PictureBox PB_03;
        private System.Windows.Forms.PictureBox PB_02;
        private System.Windows.Forms.PictureBox PB_01;
        private System.Windows.Forms.PictureBox PL_04;
        private System.Windows.Forms.PictureBox PL_01;
        private System.Windows.Forms.PictureBox PL_02;
        private System.Windows.Forms.PictureBox PL_03;
        private System.Windows.Forms.PictureBox PL_05;
        private System.Windows.Forms.PictureBox PL_06;
        private System.Windows.Forms.PictureBox PL_07;
        private System.Windows.Forms.PictureBox PL_08;
        private System.Windows.Forms.PictureBox PL_09;
        private System.Windows.Forms.PictureBox PL_10;
        private System.Windows.Forms.PictureBox PL_11;
        private System.Windows.Forms.PictureBox Deko1;
        private System.Windows.Forms.PictureBox Deko2;
        private System.Windows.Forms.PictureBox DekoL1;
        private System.Windows.Forms.PictureBox DekoL2;
        private System.Windows.Forms.PictureBox DekoL4;
        private System.Windows.Forms.PictureBox DekoL3;
        private System.Windows.Forms.PictureBox DekoL2a;
        private System.Windows.Forms.PictureBox Deko3;
        private System.Windows.Forms.PictureBox DekoLL1;
        private System.Windows.Forms.PictureBox DekoLL2;
        private System.Windows.Forms.PictureBox DekoLL3;
        private System.Windows.Forms.Button B_G;
        private System.Windows.Forms.Button B_GNT;
        private System.Windows.Forms.Button B_Plan;
        private System.Windows.Forms.Button B_Zeit;
        private System.Windows.Forms.Button B_GW;
        private System.Windows.Forms.Button B_LW;
        private System.Windows.Forms.Button B_LaT;
        private System.Windows.Forms.Button B_LaD;
        private System.Windows.Forms.Button B_FSD;
        private System.Windows.Forms.Button B_Zug;
        private System.Windows.Forms.Panel PanelButton;
        private System.Windows.Forms.Panel PanelDown;
        private System.Windows.Forms.Label Down_RW;
        private System.Windows.Forms.Label Down_Verspaetung;
        private System.Windows.Forms.Label Down_Timer;
        private System.Windows.Forms.Label Down_Radio;
        private System.Windows.Forms.Label Down_Speed;
        private System.Windows.Forms.PictureBox DekoL5;
        private System.Windows.Forms.PictureBox pB_White_Up;
        private System.Windows.Forms.PictureBox pB_White_Down;
        private System.Windows.Forms.PictureBox DekoDown4;
        private System.Windows.Forms.PictureBox QuW;
        private System.Windows.Forms.Timer timer_Sync;
        private System.ComponentModel.IContainer components;
		private System.Windows.Forms.PictureBox J_01;
		private System.Windows.Forms.PictureBox J_02;
		private System.Windows.Forms.PictureBox J_03;
		private System.Windows.Forms.PictureBox J_04;
		private System.Windows.Forms.PictureBox J_05;
		private System.Windows.Forms.PictureBox J_06;
		private System.Windows.Forms.PictureBox J_09;
		private System.Windows.Forms.PictureBox J_08;
		private System.Windows.Forms.PictureBox J_07;
		private System.Windows.Forms.PictureBox J_10;
		private System.Windows.Forms.PictureBox J_11;
		private System.Windows.Forms.Timer timer_refresh;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label l_ESF_Pos;
		private System.Windows.Forms.Label l_ESF_Neg;
		private System.Windows.Forms.Label L_A_13;
		private System.Windows.Forms.Label L_D_13;
		private System.Windows.Forms.PictureBox PB_13;

        private MMI.EBuLa.Tools.SoundInterface Sound;

		#endregion

		public EbulaControl(MMI.EBuLa.Tools.XMLLoader XMLConf) : this(XMLConf, false)
		{
		}
		

        public EbulaControl(MMI.EBuLa.Tools.XMLLoader XMLConf, bool isDAVID)
        {
			//This turns off internal double buffering of all custom GDI+ drawing
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);

            // Dieser Aufruf ist für den Windows Form-Designer erforderlich.
            InitializeComponent();

            try
            {
                InitSpeedBar();

                // Setze Virtuelle Zeit auf Reale Zeit
                vtime = new DateTime(System.DateTime.Now.Ticks);

                // Zeitmesser (basiert auf RealZeit)
                vdifftime = new DateTime(System.DateTime.Now.Ticks);

                vtimer = new System.Timers.Timer();
                vtimer.Enabled = true;
                vtimer.Interval = 950;
                vtimer.SynchronizingObject = this;
                vtimer.Elapsed += new System.Timers.ElapsedEventHandler(this.vtimer_Elapsed);

				g = this.CreateGraphics();

                verspaetung = 0;
				B_GNT.Enabled = false;

                control = new Control(verspaetung, XMLConf);
				control.use_DB = XMLConf.UseDB;

                //control.LoadTrainDEBUG();
                control.Route = new Route("", "");
                control.Route.Position = -1;

				control.vtime = vtime;

				m_conf = XMLConf;

                if (XMLConf.Sound == 0)
                {
                    Sound = new NullSound();
                    control.sound = false;
                }
                else
                {
                    switch (XMLConf.Sound)
                    {
                        case 1:
                            Sound = new APISound();
                            break;
                        case 2:
                            Sound = new DxSound();
                            break;
                    }
                    control.sound = true;                    
                }
				
				net = new Network(isDAVID);
				if (net != null)
				{
					net.setControl(ref control);

					if (t == null)
					{
						t = new Thread(new ThreadStart(net.Connect));
						t.IsBackground = true;
						t.Start();
						Thread.Sleep(0);
					}
				}

				L_TOP.Location = new Point(260, L_TOP.Location.Y);
				L_DOWN.Location = new Point(260, L_DOWN.Location.Y);

				fsdwason = true;
				SetButtons(false);

				if (control.sound) Sound.PlaySound();

                UpdateControl();

                this.Focus();
            }
            catch (Exception e)
            {
               MessageBox.Show("Fehler beim Erstellen der EBuLa Anzeige! ("+e.Message.ToString()+")");
            }
            

        }

        protected override void Dispose( bool disposing )
        {
			if( disposing )
            {
                if( components != null )
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

		public void Terminate()
		{
			t.Abort();
			net.Dispose();
			t = null;
		}

        #region Vom Komponenten-Designer generierter Code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EbulaControl));
			this.L_Uhr = new System.Windows.Forms.Label();
			this.timer_Uhr = new System.Timers.Timer();
			this.L_Zeit = new System.Windows.Forms.Label();
			this.L_Zug = new System.Windows.Forms.Label();
			this.L_Valid = new System.Windows.Forms.Label();
			this.L_Pos_12 = new System.Windows.Forms.Label();
			this.L_Pos_11 = new System.Windows.Forms.Label();
			this.L_Pos_10 = new System.Windows.Forms.Label();
			this.L_Pos_09 = new System.Windows.Forms.Label();
			this.L_Pos_08 = new System.Windows.Forms.Label();
			this.L_Pos_07 = new System.Windows.Forms.Label();
			this.L_Pos_06 = new System.Windows.Forms.Label();
			this.L_Pos_05 = new System.Windows.Forms.Label();
			this.L_Pos_04 = new System.Windows.Forms.Label();
			this.L_Pos_03 = new System.Windows.Forms.Label();
			this.L_Pos_02 = new System.Windows.Forms.Label();
			this.L_Pos_01 = new System.Windows.Forms.Label();
			this.L_Speed_12 = new System.Windows.Forms.Label();
			this.L_Speed_11 = new System.Windows.Forms.Label();
			this.L_Speed_10 = new System.Windows.Forms.Label();
			this.L_Speed_09 = new System.Windows.Forms.Label();
			this.L_Speed_08 = new System.Windows.Forms.Label();
			this.L_Speed_07 = new System.Windows.Forms.Label();
			this.L_Speed_06 = new System.Windows.Forms.Label();
			this.L_Speed_05 = new System.Windows.Forms.Label();
			this.L_Speed_04 = new System.Windows.Forms.Label();
			this.L_Speed_03 = new System.Windows.Forms.Label();
			this.L_Speed_02 = new System.Windows.Forms.Label();
			this.L_Speed_01 = new System.Windows.Forms.Label();
			this.L_Ops_01 = new System.Windows.Forms.Label();
			this.L_Ops_02 = new System.Windows.Forms.Label();
			this.L_Ops_03 = new System.Windows.Forms.Label();
			this.L_Ops_04 = new System.Windows.Forms.Label();
			this.L_Ops_05 = new System.Windows.Forms.Label();
			this.L_Ops_06 = new System.Windows.Forms.Label();
			this.L_Ops_07 = new System.Windows.Forms.Label();
			this.L_Ops_08 = new System.Windows.Forms.Label();
			this.L_Ops_09 = new System.Windows.Forms.Label();
			this.L_Ops_10 = new System.Windows.Forms.Label();
			this.L_Ops_11 = new System.Windows.Forms.Label();
			this.L_Ops_12 = new System.Windows.Forms.Label();
			this.L_DOWN = new System.Windows.Forms.Label();
			this.L_TOP = new System.Windows.Forms.Label();
			this.L_A_01 = new System.Windows.Forms.Label();
			this.L_A_02 = new System.Windows.Forms.Label();
			this.L_A_03 = new System.Windows.Forms.Label();
			this.L_A_04 = new System.Windows.Forms.Label();
			this.L_A_05 = new System.Windows.Forms.Label();
			this.L_A_06 = new System.Windows.Forms.Label();
			this.L_A_07 = new System.Windows.Forms.Label();
			this.L_A_08 = new System.Windows.Forms.Label();
			this.L_A_09 = new System.Windows.Forms.Label();
			this.L_A_10 = new System.Windows.Forms.Label();
			this.L_A_11 = new System.Windows.Forms.Label();
			this.L_A_12 = new System.Windows.Forms.Label();
			this.L_D_12 = new System.Windows.Forms.Label();
			this.L_D_11 = new System.Windows.Forms.Label();
			this.L_D_10 = new System.Windows.Forms.Label();
			this.L_D_09 = new System.Windows.Forms.Label();
			this.L_D_08 = new System.Windows.Forms.Label();
			this.L_D_07 = new System.Windows.Forms.Label();
			this.L_D_06 = new System.Windows.Forms.Label();
			this.L_D_05 = new System.Windows.Forms.Label();
			this.L_D_04 = new System.Windows.Forms.Label();
			this.L_D_03 = new System.Windows.Forms.Label();
			this.L_D_02 = new System.Windows.Forms.Label();
			this.L_D_01 = new System.Windows.Forms.Label();
			this.L_AK_01 = new System.Windows.Forms.Label();
			this.L_AK_02 = new System.Windows.Forms.Label();
			this.L_AK_03 = new System.Windows.Forms.Label();
			this.L_AK_04 = new System.Windows.Forms.Label();
			this.L_AK_05 = new System.Windows.Forms.Label();
			this.L_AK_06 = new System.Windows.Forms.Label();
			this.L_AK_07 = new System.Windows.Forms.Label();
			this.L_AK_08 = new System.Windows.Forms.Label();
			this.L_AK_09 = new System.Windows.Forms.Label();
			this.L_AK_10 = new System.Windows.Forms.Label();
			this.L_AK_11 = new System.Windows.Forms.Label();
			this.L_AK_12 = new System.Windows.Forms.Label();
			this.L_DK_01 = new System.Windows.Forms.Label();
			this.L_DK_02 = new System.Windows.Forms.Label();
			this.L_DK_03 = new System.Windows.Forms.Label();
			this.L_DK_04 = new System.Windows.Forms.Label();
			this.L_DK_05 = new System.Windows.Forms.Label();
			this.L_DK_06 = new System.Windows.Forms.Label();
			this.L_DK_07 = new System.Windows.Forms.Label();
			this.L_DK_08 = new System.Windows.Forms.Label();
			this.L_DK_09 = new System.Windows.Forms.Label();
			this.L_DK_10 = new System.Windows.Forms.Label();
			this.L_DK_11 = new System.Windows.Forms.Label();
			this.L_DK_12 = new System.Windows.Forms.Label();
			this.L_OpsS_01 = new System.Windows.Forms.Label();
			this.L_OpsS_02 = new System.Windows.Forms.Label();
			this.L_OpsS_03 = new System.Windows.Forms.Label();
			this.L_OpsS_04 = new System.Windows.Forms.Label();
			this.L_OpsS_05 = new System.Windows.Forms.Label();
			this.L_OpsS_06 = new System.Windows.Forms.Label();
			this.L_OpsS_07 = new System.Windows.Forms.Label();
			this.L_OpsS_08 = new System.Windows.Forms.Label();
			this.L_OpsS_09 = new System.Windows.Forms.Label();
			this.L_OpsS_10 = new System.Windows.Forms.Label();
			this.L_OpsS_11 = new System.Windows.Forms.Label();
			this.L_OpsS_12 = new System.Windows.Forms.Label();
			this.Qu = new System.Windows.Forms.PictureBox();
			this.timer_Position = new System.Windows.Forms.Timer(this.components);
			this.PB_12 = new System.Windows.Forms.PictureBox();
			this.PB_11 = new System.Windows.Forms.PictureBox();
			this.PB_10 = new System.Windows.Forms.PictureBox();
			this.PB_09 = new System.Windows.Forms.PictureBox();
			this.PB_08 = new System.Windows.Forms.PictureBox();
			this.PB_07 = new System.Windows.Forms.PictureBox();
			this.PB_06 = new System.Windows.Forms.PictureBox();
			this.PB_05 = new System.Windows.Forms.PictureBox();
			this.PB_04 = new System.Windows.Forms.PictureBox();
			this.PB_03 = new System.Windows.Forms.PictureBox();
			this.PB_02 = new System.Windows.Forms.PictureBox();
			this.PB_01 = new System.Windows.Forms.PictureBox();
			this.PB_13 = new System.Windows.Forms.PictureBox();
			this.PL_04 = new System.Windows.Forms.PictureBox();
			this.PL_01 = new System.Windows.Forms.PictureBox();
			this.PL_02 = new System.Windows.Forms.PictureBox();
			this.PL_03 = new System.Windows.Forms.PictureBox();
			this.PL_05 = new System.Windows.Forms.PictureBox();
			this.PL_06 = new System.Windows.Forms.PictureBox();
			this.PL_07 = new System.Windows.Forms.PictureBox();
			this.PL_08 = new System.Windows.Forms.PictureBox();
			this.PL_09 = new System.Windows.Forms.PictureBox();
			this.PL_10 = new System.Windows.Forms.PictureBox();
			this.PL_11 = new System.Windows.Forms.PictureBox();
			this.Deko1 = new System.Windows.Forms.PictureBox();
			this.Deko2 = new System.Windows.Forms.PictureBox();
			this.DekoL1 = new System.Windows.Forms.PictureBox();
			this.DekoL2 = new System.Windows.Forms.PictureBox();
			this.DekoL4 = new System.Windows.Forms.PictureBox();
			this.DekoL3 = new System.Windows.Forms.PictureBox();
			this.DekoL2a = new System.Windows.Forms.PictureBox();
			this.Deko3 = new System.Windows.Forms.PictureBox();
			this.DekoLL1 = new System.Windows.Forms.PictureBox();
			this.DekoLL2 = new System.Windows.Forms.PictureBox();
			this.DekoLL3 = new System.Windows.Forms.PictureBox();
			this.PanelButton = new System.Windows.Forms.Panel();
			this.B_G = new System.Windows.Forms.Button();
			this.B_GNT = new System.Windows.Forms.Button();
			this.B_Plan = new System.Windows.Forms.Button();
			this.B_Zeit = new System.Windows.Forms.Button();
			this.B_GW = new System.Windows.Forms.Button();
			this.B_LW = new System.Windows.Forms.Button();
			this.B_LaT = new System.Windows.Forms.Button();
			this.B_LaD = new System.Windows.Forms.Button();
			this.B_FSD = new System.Windows.Forms.Button();
			this.B_Zug = new System.Windows.Forms.Button();
			this.PanelDown = new System.Windows.Forms.Panel();
			this.Down_RW = new System.Windows.Forms.Label();
			this.Down_Verspaetung = new System.Windows.Forms.Label();
			this.Down_Timer = new System.Windows.Forms.Label();
			this.Down_Radio = new System.Windows.Forms.Label();
			this.Down_Speed = new System.Windows.Forms.Label();
			this.DekoDown4 = new System.Windows.Forms.PictureBox();
			this.DekoL5 = new System.Windows.Forms.PictureBox();
			this.pB_White_Up = new System.Windows.Forms.PictureBox();
			this.pB_White_Down = new System.Windows.Forms.PictureBox();
			this.timer_Sync = new System.Windows.Forms.Timer(this.components);
			this.QuW = new System.Windows.Forms.PictureBox();
			this.J_01 = new System.Windows.Forms.PictureBox();
			this.J_02 = new System.Windows.Forms.PictureBox();
			this.J_03 = new System.Windows.Forms.PictureBox();
			this.J_04 = new System.Windows.Forms.PictureBox();
			this.J_05 = new System.Windows.Forms.PictureBox();
			this.J_06 = new System.Windows.Forms.PictureBox();
			this.J_10 = new System.Windows.Forms.PictureBox();
			this.J_09 = new System.Windows.Forms.PictureBox();
			this.J_08 = new System.Windows.Forms.PictureBox();
			this.J_07 = new System.Windows.Forms.PictureBox();
			this.J_11 = new System.Windows.Forms.PictureBox();
			this.timer_refresh = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.l_ESF_Pos = new System.Windows.Forms.Label();
			this.l_ESF_Neg = new System.Windows.Forms.Label();
			this.L_A_13 = new System.Windows.Forms.Label();
			this.L_D_13 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.timer_Uhr)).BeginInit();
			this.PanelButton.SuspendLayout();
			this.PanelDown.SuspendLayout();
			this.SuspendLayout();
			// 
			// L_Uhr
			// 
			this.L_Uhr.Font = new System.Drawing.Font("Arial", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Uhr.Location = new System.Drawing.Point(464, 0);
			this.L_Uhr.Name = "L_Uhr";
			this.L_Uhr.Size = new System.Drawing.Size(168, 64);
			this.L_Uhr.TabIndex = 11;
			this.L_Uhr.Text = "14:32:00";
			this.L_Uhr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// timer_Uhr
			// 
			this.timer_Uhr.Enabled = true;
			this.timer_Uhr.Interval = 990;
			this.timer_Uhr.SynchronizingObject = this;
			this.timer_Uhr.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Uhr_Elapsed);
			// 
			// L_Zeit
			// 
			this.L_Zeit.Font = new System.Drawing.Font("Arial", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Zeit.Location = new System.Drawing.Point(296, 0);
			this.L_Zeit.Name = "L_Zeit";
			this.L_Zeit.Size = new System.Drawing.Size(168, 64);
			this.L_Zeit.TabIndex = 12;
			this.L_Zeit.Text = "01.01.04";
			this.L_Zeit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// L_Zug
			// 
			this.L_Zug.Font = new System.Drawing.Font("Arial", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Zug.Location = new System.Drawing.Point(0, 0);
			this.L_Zug.Name = "L_Zug";
			this.L_Zug.Size = new System.Drawing.Size(128, 64);
			this.L_Zug.TabIndex = 13;
			this.L_Zug.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// L_Valid
			// 
			this.L_Valid.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Valid.Location = new System.Drawing.Point(136, 0);
			this.L_Valid.Name = "L_Valid";
			this.L_Valid.Size = new System.Drawing.Size(144, 64);
			this.L_Valid.TabIndex = 14;
			this.L_Valid.Text = "EBuLa-Karte gültig bis: 88.88.88";
			this.L_Valid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// L_Pos_12
			// 
			this.L_Pos_12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_12.Location = new System.Drawing.Point(128, 80);
			this.L_Pos_12.Name = "L_Pos_12";
			this.L_Pos_12.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_12.TabIndex = 15;
			this.L_Pos_12.Text = "L_Pos_12";
			this.L_Pos_12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_11
			// 
			this.L_Pos_11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_11.Location = new System.Drawing.Point(128, 104);
			this.L_Pos_11.Name = "L_Pos_11";
			this.L_Pos_11.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_11.TabIndex = 16;
			this.L_Pos_11.Text = "L_Pos_11";
			this.L_Pos_11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_10
			// 
			this.L_Pos_10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_10.Location = new System.Drawing.Point(128, 128);
			this.L_Pos_10.Name = "L_Pos_10";
			this.L_Pos_10.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_10.TabIndex = 44;
			this.L_Pos_10.Text = "L_Pos_10";
			this.L_Pos_10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_09
			// 
			this.L_Pos_09.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_09.Location = new System.Drawing.Point(128, 152);
			this.L_Pos_09.Name = "L_Pos_09";
			this.L_Pos_09.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_09.TabIndex = 43;
			this.L_Pos_09.Text = "L_Pos_09";
			this.L_Pos_09.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_08
			// 
			this.L_Pos_08.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_08.Location = new System.Drawing.Point(128, 176);
			this.L_Pos_08.Name = "L_Pos_08";
			this.L_Pos_08.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_08.TabIndex = 42;
			this.L_Pos_08.Text = "L_Pos_08";
			this.L_Pos_08.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_07
			// 
			this.L_Pos_07.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_07.Location = new System.Drawing.Point(128, 200);
			this.L_Pos_07.Name = "L_Pos_07";
			this.L_Pos_07.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_07.TabIndex = 41;
			this.L_Pos_07.Text = "L_Pos_07";
			this.L_Pos_07.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_06
			// 
			this.L_Pos_06.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_06.Location = new System.Drawing.Point(128, 224);
			this.L_Pos_06.Name = "L_Pos_06";
			this.L_Pos_06.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_06.TabIndex = 40;
			this.L_Pos_06.Text = "L_Pos_06";
			this.L_Pos_06.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_05
			// 
			this.L_Pos_05.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_05.Location = new System.Drawing.Point(128, 248);
			this.L_Pos_05.Name = "L_Pos_05";
			this.L_Pos_05.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_05.TabIndex = 39;
			this.L_Pos_05.Text = "L_Pos_05";
			this.L_Pos_05.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_04
			// 
			this.L_Pos_04.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_04.Location = new System.Drawing.Point(128, 272);
			this.L_Pos_04.Name = "L_Pos_04";
			this.L_Pos_04.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_04.TabIndex = 38;
			this.L_Pos_04.Text = "L_Pos_04";
			this.L_Pos_04.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_03
			// 
			this.L_Pos_03.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_03.Location = new System.Drawing.Point(128, 296);
			this.L_Pos_03.Name = "L_Pos_03";
			this.L_Pos_03.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_03.TabIndex = 24;
			this.L_Pos_03.Text = "L_Pos_03";
			this.L_Pos_03.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_02
			// 
			this.L_Pos_02.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_02.Location = new System.Drawing.Point(128, 320);
			this.L_Pos_02.Name = "L_Pos_02";
			this.L_Pos_02.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_02.TabIndex = 25;
			this.L_Pos_02.Text = "L_Pos_02";
			this.L_Pos_02.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Pos_01
			// 
			this.L_Pos_01.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Pos_01.Location = new System.Drawing.Point(128, 344);
			this.L_Pos_01.Name = "L_Pos_01";
			this.L_Pos_01.Size = new System.Drawing.Size(70, 18);
			this.L_Pos_01.TabIndex = 26;
			this.L_Pos_01.Text = "L_Pos_01";
			this.L_Pos_01.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_12
			// 
			this.L_Speed_12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_12.Location = new System.Drawing.Point(80, 80);
			this.L_Speed_12.Name = "L_Speed_12";
			this.L_Speed_12.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_12.TabIndex = 27;
			this.L_Speed_12.Text = "L_Speed_12";
			this.L_Speed_12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_11
			// 
			this.L_Speed_11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_11.Location = new System.Drawing.Point(80, 104);
			this.L_Speed_11.Name = "L_Speed_11";
			this.L_Speed_11.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_11.TabIndex = 28;
			this.L_Speed_11.Text = "L_S_11";
			this.L_Speed_11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_10
			// 
			this.L_Speed_10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_10.Location = new System.Drawing.Point(80, 128);
			this.L_Speed_10.Name = "L_Speed_10";
			this.L_Speed_10.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_10.TabIndex = 29;
			this.L_Speed_10.Text = "L_S_10";
			this.L_Speed_10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_09
			// 
			this.L_Speed_09.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_09.Location = new System.Drawing.Point(80, 152);
			this.L_Speed_09.Name = "L_Speed_09";
			this.L_Speed_09.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_09.TabIndex = 30;
			this.L_Speed_09.Text = "L_S_09";
			this.L_Speed_09.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_08
			// 
			this.L_Speed_08.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_08.Location = new System.Drawing.Point(80, 176);
			this.L_Speed_08.Name = "L_Speed_08";
			this.L_Speed_08.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_08.TabIndex = 31;
			this.L_Speed_08.Text = "L_S_08";
			this.L_Speed_08.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_07
			// 
			this.L_Speed_07.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_07.Location = new System.Drawing.Point(80, 200);
			this.L_Speed_07.Name = "L_Speed_07";
			this.L_Speed_07.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_07.TabIndex = 32;
			this.L_Speed_07.Text = "L_S_07";
			this.L_Speed_07.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_06
			// 
			this.L_Speed_06.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_06.Location = new System.Drawing.Point(80, 224);
			this.L_Speed_06.Name = "L_Speed_06";
			this.L_Speed_06.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_06.TabIndex = 33;
			this.L_Speed_06.Text = "L_S_06";
			this.L_Speed_06.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_05
			// 
			this.L_Speed_05.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_05.Location = new System.Drawing.Point(80, 248);
			this.L_Speed_05.Name = "L_Speed_05";
			this.L_Speed_05.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_05.TabIndex = 34;
			this.L_Speed_05.Text = "L_S_05";
			this.L_Speed_05.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_04
			// 
			this.L_Speed_04.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_04.Location = new System.Drawing.Point(80, 272);
			this.L_Speed_04.Name = "L_Speed_04";
			this.L_Speed_04.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_04.TabIndex = 35;
			this.L_Speed_04.Text = "L_S_04";
			this.L_Speed_04.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_03
			// 
			this.L_Speed_03.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_03.Location = new System.Drawing.Point(80, 296);
			this.L_Speed_03.Name = "L_Speed_03";
			this.L_Speed_03.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_03.TabIndex = 36;
			this.L_Speed_03.Text = "L_S_03";
			this.L_Speed_03.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_02
			// 
			this.L_Speed_02.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_02.Location = new System.Drawing.Point(80, 320);
			this.L_Speed_02.Name = "L_Speed_02";
			this.L_Speed_02.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_02.TabIndex = 37;
			this.L_Speed_02.Text = "L_S_02";
			this.L_Speed_02.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Speed_01
			// 
			this.L_Speed_01.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Speed_01.Location = new System.Drawing.Point(80, 344);
			this.L_Speed_01.Name = "L_Speed_01";
			this.L_Speed_01.Size = new System.Drawing.Size(32, 18);
			this.L_Speed_01.TabIndex = 28;
			this.L_Speed_01.Text = "L_S_01";
			this.L_Speed_01.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_Ops_01
			// 
			this.L_Ops_01.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_01.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_01.Location = new System.Drawing.Point(232, 344);
			this.L_Ops_01.Name = "L_Ops_01";
			this.L_Ops_01.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_01.TabIndex = 49;
			this.L_Ops_01.Text = "L_Ops_01";
			// 
			// L_Ops_02
			// 
			this.L_Ops_02.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_02.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_02.Location = new System.Drawing.Point(232, 320);
			this.L_Ops_02.Name = "L_Ops_02";
			this.L_Ops_02.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_02.TabIndex = 48;
			this.L_Ops_02.Text = "L_Ops_02";
			// 
			// L_Ops_03
			// 
			this.L_Ops_03.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_03.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_03.Location = new System.Drawing.Point(232, 296);
			this.L_Ops_03.Name = "L_Ops_03";
			this.L_Ops_03.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_03.TabIndex = 47;
			this.L_Ops_03.Text = "L_Ops_03";
			// 
			// L_Ops_04
			// 
			this.L_Ops_04.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_04.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_04.Location = new System.Drawing.Point(232, 272);
			this.L_Ops_04.Name = "L_Ops_04";
			this.L_Ops_04.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_04.TabIndex = 50;
			this.L_Ops_04.Text = "L_Ops_04";
			// 
			// L_Ops_05
			// 
			this.L_Ops_05.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_05.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_05.Location = new System.Drawing.Point(232, 248);
			this.L_Ops_05.Name = "L_Ops_05";
			this.L_Ops_05.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_05.TabIndex = 51;
			this.L_Ops_05.Text = "L_Ops_05";
			// 
			// L_Ops_06
			// 
			this.L_Ops_06.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_06.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_06.Location = new System.Drawing.Point(232, 224);
			this.L_Ops_06.Name = "L_Ops_06";
			this.L_Ops_06.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_06.TabIndex = 52;
			this.L_Ops_06.Text = "L_Ops_06";
			// 
			// L_Ops_07
			// 
			this.L_Ops_07.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_07.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_07.Location = new System.Drawing.Point(232, 200);
			this.L_Ops_07.Name = "L_Ops_07";
			this.L_Ops_07.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_07.TabIndex = 53;
			this.L_Ops_07.Text = "L_Ops_07";
			// 
			// L_Ops_08
			// 
			this.L_Ops_08.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_08.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_08.Location = new System.Drawing.Point(232, 176);
			this.L_Ops_08.Name = "L_Ops_08";
			this.L_Ops_08.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_08.TabIndex = 54;
			this.L_Ops_08.Text = "L_Ops_08";
			// 
			// L_Ops_09
			// 
			this.L_Ops_09.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_09.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_09.Location = new System.Drawing.Point(232, 152);
			this.L_Ops_09.Name = "L_Ops_09";
			this.L_Ops_09.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_09.TabIndex = 55;
			this.L_Ops_09.Text = "L_Ops_09";
			// 
			// L_Ops_10
			// 
			this.L_Ops_10.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_10.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_10.Location = new System.Drawing.Point(232, 128);
			this.L_Ops_10.Name = "L_Ops_10";
			this.L_Ops_10.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_10.TabIndex = 56;
			this.L_Ops_10.Text = "L_Ops_10";
			// 
			// L_Ops_11
			// 
			this.L_Ops_11.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_11.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_11.Location = new System.Drawing.Point(232, 104);
			this.L_Ops_11.Name = "L_Ops_11";
			this.L_Ops_11.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_11.TabIndex = 46;
			this.L_Ops_11.Text = "L_Ops_11";
			// 
			// L_Ops_12
			// 
			this.L_Ops_12.BackColor = System.Drawing.Color.Transparent;
			this.L_Ops_12.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_Ops_12.Location = new System.Drawing.Point(232, 80);
			this.L_Ops_12.Name = "L_Ops_12";
			this.L_Ops_12.Size = new System.Drawing.Size(184, 18);
			this.L_Ops_12.TabIndex = 45;
			this.L_Ops_12.Text = "L_Ops_12";
			// 
			// L_DOWN
			// 
			this.L_DOWN.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DOWN.Location = new System.Drawing.Point(232, 368);
			this.L_DOWN.Name = "L_DOWN";
			this.L_DOWN.Size = new System.Drawing.Size(165, 18);
			this.L_DOWN.TabIndex = 57;
			this.L_DOWN.Text = "L_DOWN";
			// 
			// L_TOP
			// 
			this.L_TOP.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_TOP.Location = new System.Drawing.Point(232, 56);
			this.L_TOP.Name = "L_TOP";
			this.L_TOP.Size = new System.Drawing.Size(232, 18);
			this.L_TOP.TabIndex = 58;
			this.L_TOP.Text = "L_TOP";
			// 
			// L_A_01
			// 
			this.L_A_01.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_01.Location = new System.Drawing.Point(480, 344);
			this.L_A_01.Name = "L_A_01";
			this.L_A_01.Size = new System.Drawing.Size(50, 18);
			this.L_A_01.TabIndex = 59;
			this.L_A_01.Text = "L_A_01";
			this.L_A_01.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_02
			// 
			this.L_A_02.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_02.Location = new System.Drawing.Point(480, 320);
			this.L_A_02.Name = "L_A_02";
			this.L_A_02.Size = new System.Drawing.Size(50, 18);
			this.L_A_02.TabIndex = 61;
			this.L_A_02.Text = "L_A_02";
			this.L_A_02.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_03
			// 
			this.L_A_03.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_03.Location = new System.Drawing.Point(480, 296);
			this.L_A_03.Name = "L_A_03";
			this.L_A_03.Size = new System.Drawing.Size(50, 18);
			this.L_A_03.TabIndex = 62;
			this.L_A_03.Text = "L_A_03";
			this.L_A_03.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_04
			// 
			this.L_A_04.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_04.Location = new System.Drawing.Point(480, 272);
			this.L_A_04.Name = "L_A_04";
			this.L_A_04.Size = new System.Drawing.Size(50, 18);
			this.L_A_04.TabIndex = 63;
			this.L_A_04.Text = "L_A_04";
			this.L_A_04.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_05
			// 
			this.L_A_05.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_05.Location = new System.Drawing.Point(480, 248);
			this.L_A_05.Name = "L_A_05";
			this.L_A_05.Size = new System.Drawing.Size(50, 18);
			this.L_A_05.TabIndex = 64;
			this.L_A_05.Text = "L_A_05";
			this.L_A_05.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_06
			// 
			this.L_A_06.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_06.Location = new System.Drawing.Point(480, 224);
			this.L_A_06.Name = "L_A_06";
			this.L_A_06.Size = new System.Drawing.Size(50, 18);
			this.L_A_06.TabIndex = 65;
			this.L_A_06.Text = "L_A_06";
			this.L_A_06.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_07
			// 
			this.L_A_07.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_07.Location = new System.Drawing.Point(480, 200);
			this.L_A_07.Name = "L_A_07";
			this.L_A_07.Size = new System.Drawing.Size(50, 18);
			this.L_A_07.TabIndex = 66;
			this.L_A_07.Text = "L_A_07";
			this.L_A_07.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_08
			// 
			this.L_A_08.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_08.Location = new System.Drawing.Point(480, 176);
			this.L_A_08.Name = "L_A_08";
			this.L_A_08.Size = new System.Drawing.Size(50, 18);
			this.L_A_08.TabIndex = 67;
			this.L_A_08.Text = "L_A_08";
			this.L_A_08.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_09
			// 
			this.L_A_09.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_09.Location = new System.Drawing.Point(480, 152);
			this.L_A_09.Name = "L_A_09";
			this.L_A_09.Size = new System.Drawing.Size(50, 18);
			this.L_A_09.TabIndex = 68;
			this.L_A_09.Text = "L_A_09";
			this.L_A_09.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_10
			// 
			this.L_A_10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_10.Location = new System.Drawing.Point(480, 128);
			this.L_A_10.Name = "L_A_10";
			this.L_A_10.Size = new System.Drawing.Size(50, 18);
			this.L_A_10.TabIndex = 69;
			this.L_A_10.Text = "L_A_10";
			this.L_A_10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_11
			// 
			this.L_A_11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_11.Location = new System.Drawing.Point(480, 104);
			this.L_A_11.Name = "L_A_11";
			this.L_A_11.Size = new System.Drawing.Size(50, 18);
			this.L_A_11.TabIndex = 70;
			this.L_A_11.Text = "L_A_11";
			this.L_A_11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_12
			// 
			this.L_A_12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_12.Location = new System.Drawing.Point(480, 80);
			this.L_A_12.Name = "L_A_12";
			this.L_A_12.Size = new System.Drawing.Size(50, 18);
			this.L_A_12.TabIndex = 60;
			this.L_A_12.Text = "L_A_12";
			this.L_A_12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_12
			// 
			this.L_D_12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_12.Location = new System.Drawing.Point(560, 80);
			this.L_D_12.Name = "L_D_12";
			this.L_D_12.Size = new System.Drawing.Size(50, 18);
			this.L_D_12.TabIndex = 71;
			this.L_D_12.Text = "L_D_12";
			this.L_D_12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_11
			// 
			this.L_D_11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_11.Location = new System.Drawing.Point(560, 104);
			this.L_D_11.Name = "L_D_11";
			this.L_D_11.Size = new System.Drawing.Size(50, 18);
			this.L_D_11.TabIndex = 73;
			this.L_D_11.Text = "L_D_11";
			this.L_D_11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_10
			// 
			this.L_D_10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_10.Location = new System.Drawing.Point(560, 128);
			this.L_D_10.Name = "L_D_10";
			this.L_D_10.Size = new System.Drawing.Size(50, 18);
			this.L_D_10.TabIndex = 74;
			this.L_D_10.Text = "L_D_10";
			this.L_D_10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_09
			// 
			this.L_D_09.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_09.Location = new System.Drawing.Point(560, 152);
			this.L_D_09.Name = "L_D_09";
			this.L_D_09.Size = new System.Drawing.Size(50, 18);
			this.L_D_09.TabIndex = 75;
			this.L_D_09.Text = "L_D_09";
			this.L_D_09.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_08
			// 
			this.L_D_08.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_08.Location = new System.Drawing.Point(560, 176);
			this.L_D_08.Name = "L_D_08";
			this.L_D_08.Size = new System.Drawing.Size(50, 18);
			this.L_D_08.TabIndex = 76;
			this.L_D_08.Text = "L_D_08";
			this.L_D_08.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_07
			// 
			this.L_D_07.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_07.Location = new System.Drawing.Point(560, 200);
			this.L_D_07.Name = "L_D_07";
			this.L_D_07.Size = new System.Drawing.Size(50, 18);
			this.L_D_07.TabIndex = 77;
			this.L_D_07.Text = "L_D_07";
			this.L_D_07.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_06
			// 
			this.L_D_06.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_06.Location = new System.Drawing.Point(560, 224);
			this.L_D_06.Name = "L_D_06";
			this.L_D_06.Size = new System.Drawing.Size(50, 18);
			this.L_D_06.TabIndex = 78;
			this.L_D_06.Text = "L_D_06";
			this.L_D_06.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_05
			// 
			this.L_D_05.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_05.Location = new System.Drawing.Point(560, 248);
			this.L_D_05.Name = "L_D_05";
			this.L_D_05.Size = new System.Drawing.Size(50, 18);
			this.L_D_05.TabIndex = 79;
			this.L_D_05.Text = "L_D_05";
			this.L_D_05.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_04
			// 
			this.L_D_04.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_04.Location = new System.Drawing.Point(560, 272);
			this.L_D_04.Name = "L_D_04";
			this.L_D_04.Size = new System.Drawing.Size(50, 18);
			this.L_D_04.TabIndex = 80;
			this.L_D_04.Text = "L_D_04";
			this.L_D_04.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_03
			// 
			this.L_D_03.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_03.Location = new System.Drawing.Point(560, 296);
			this.L_D_03.Name = "L_D_03";
			this.L_D_03.Size = new System.Drawing.Size(50, 18);
			this.L_D_03.TabIndex = 81;
			this.L_D_03.Text = "L_D_03";
			this.L_D_03.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_02
			// 
			this.L_D_02.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_02.Location = new System.Drawing.Point(560, 320);
			this.L_D_02.Name = "L_D_02";
			this.L_D_02.Size = new System.Drawing.Size(50, 18);
			this.L_D_02.TabIndex = 82;
			this.L_D_02.Text = "L_D_02";
			this.L_D_02.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_01
			// 
			this.L_D_01.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_01.Location = new System.Drawing.Point(560, 344);
			this.L_D_01.Name = "L_D_01";
			this.L_D_01.Size = new System.Drawing.Size(50, 18);
			this.L_D_01.TabIndex = 72;
			this.L_D_01.Text = "L_D_01";
			this.L_D_01.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_AK_01
			// 
			this.L_AK_01.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_01.Location = new System.Drawing.Point(528, 344);
			this.L_AK_01.Name = "L_AK_01";
			this.L_AK_01.Size = new System.Drawing.Size(20, 18);
			this.L_AK_01.TabIndex = 83;
			this.L_AK_01.Text = "L_AK_01";
			this.L_AK_01.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_02
			// 
			this.L_AK_02.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_02.Location = new System.Drawing.Point(528, 320);
			this.L_AK_02.Name = "L_AK_02";
			this.L_AK_02.Size = new System.Drawing.Size(20, 18);
			this.L_AK_02.TabIndex = 85;
			this.L_AK_02.Text = "L_AK_02";
			this.L_AK_02.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_03
			// 
			this.L_AK_03.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_03.Location = new System.Drawing.Point(528, 296);
			this.L_AK_03.Name = "L_AK_03";
			this.L_AK_03.Size = new System.Drawing.Size(20, 18);
			this.L_AK_03.TabIndex = 86;
			this.L_AK_03.Text = "L_AK_03";
			this.L_AK_03.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_04
			// 
			this.L_AK_04.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_04.Location = new System.Drawing.Point(528, 272);
			this.L_AK_04.Name = "L_AK_04";
			this.L_AK_04.Size = new System.Drawing.Size(20, 18);
			this.L_AK_04.TabIndex = 87;
			this.L_AK_04.Text = "L_AK_04";
			this.L_AK_04.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_05
			// 
			this.L_AK_05.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_05.Location = new System.Drawing.Point(528, 248);
			this.L_AK_05.Name = "L_AK_05";
			this.L_AK_05.Size = new System.Drawing.Size(20, 18);
			this.L_AK_05.TabIndex = 88;
			this.L_AK_05.Text = "L_AK_05";
			this.L_AK_05.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_06
			// 
			this.L_AK_06.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_06.Location = new System.Drawing.Point(528, 224);
			this.L_AK_06.Name = "L_AK_06";
			this.L_AK_06.Size = new System.Drawing.Size(20, 18);
			this.L_AK_06.TabIndex = 89;
			this.L_AK_06.Text = "L_AK_06";
			this.L_AK_06.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_07
			// 
			this.L_AK_07.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_07.Location = new System.Drawing.Point(528, 200);
			this.L_AK_07.Name = "L_AK_07";
			this.L_AK_07.Size = new System.Drawing.Size(20, 18);
			this.L_AK_07.TabIndex = 90;
			this.L_AK_07.Text = "L_AK_07";
			this.L_AK_07.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_08
			// 
			this.L_AK_08.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_08.Location = new System.Drawing.Point(528, 176);
			this.L_AK_08.Name = "L_AK_08";
			this.L_AK_08.Size = new System.Drawing.Size(20, 18);
			this.L_AK_08.TabIndex = 91;
			this.L_AK_08.Text = "L_AK_08";
			this.L_AK_08.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_09
			// 
			this.L_AK_09.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_09.Location = new System.Drawing.Point(528, 152);
			this.L_AK_09.Name = "L_AK_09";
			this.L_AK_09.Size = new System.Drawing.Size(20, 18);
			this.L_AK_09.TabIndex = 92;
			this.L_AK_09.Text = "L_AK_09";
			this.L_AK_09.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_10
			// 
			this.L_AK_10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_10.Location = new System.Drawing.Point(528, 128);
			this.L_AK_10.Name = "L_AK_10";
			this.L_AK_10.Size = new System.Drawing.Size(20, 18);
			this.L_AK_10.TabIndex = 93;
			this.L_AK_10.Text = "L_AK_10";
			this.L_AK_10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_11
			// 
			this.L_AK_11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_11.Location = new System.Drawing.Point(528, 104);
			this.L_AK_11.Name = "L_AK_11";
			this.L_AK_11.Size = new System.Drawing.Size(20, 18);
			this.L_AK_11.TabIndex = 94;
			this.L_AK_11.Text = "L_AK_11";
			this.L_AK_11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_AK_12
			// 
			this.L_AK_12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_AK_12.Location = new System.Drawing.Point(528, 80);
			this.L_AK_12.Name = "L_AK_12";
			this.L_AK_12.Size = new System.Drawing.Size(20, 18);
			this.L_AK_12.TabIndex = 84;
			this.L_AK_12.Text = "L_AK_12";
			this.L_AK_12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_01
			// 
			this.L_DK_01.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_01.Location = new System.Drawing.Point(608, 344);
			this.L_DK_01.Name = "L_DK_01";
			this.L_DK_01.Size = new System.Drawing.Size(20, 18);
			this.L_DK_01.TabIndex = 95;
			this.L_DK_01.Text = "L_DK_01";
			this.L_DK_01.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_02
			// 
			this.L_DK_02.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_02.Location = new System.Drawing.Point(608, 320);
			this.L_DK_02.Name = "L_DK_02";
			this.L_DK_02.Size = new System.Drawing.Size(20, 18);
			this.L_DK_02.TabIndex = 97;
			this.L_DK_02.Text = "L_DK_02";
			this.L_DK_02.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_03
			// 
			this.L_DK_03.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_03.Location = new System.Drawing.Point(608, 296);
			this.L_DK_03.Name = "L_DK_03";
			this.L_DK_03.Size = new System.Drawing.Size(20, 18);
			this.L_DK_03.TabIndex = 98;
			this.L_DK_03.Text = "L_DK_03";
			this.L_DK_03.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_04
			// 
			this.L_DK_04.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_04.Location = new System.Drawing.Point(608, 272);
			this.L_DK_04.Name = "L_DK_04";
			this.L_DK_04.Size = new System.Drawing.Size(20, 18);
			this.L_DK_04.TabIndex = 99;
			this.L_DK_04.Text = "L_DK_04";
			this.L_DK_04.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_05
			// 
			this.L_DK_05.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_05.Location = new System.Drawing.Point(608, 248);
			this.L_DK_05.Name = "L_DK_05";
			this.L_DK_05.Size = new System.Drawing.Size(20, 18);
			this.L_DK_05.TabIndex = 100;
			this.L_DK_05.Text = "L_DK_05";
			this.L_DK_05.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_06
			// 
			this.L_DK_06.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_06.Location = new System.Drawing.Point(608, 224);
			this.L_DK_06.Name = "L_DK_06";
			this.L_DK_06.Size = new System.Drawing.Size(20, 18);
			this.L_DK_06.TabIndex = 101;
			this.L_DK_06.Text = "L_DK_06";
			this.L_DK_06.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_07
			// 
			this.L_DK_07.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_07.Location = new System.Drawing.Point(608, 200);
			this.L_DK_07.Name = "L_DK_07";
			this.L_DK_07.Size = new System.Drawing.Size(20, 18);
			this.L_DK_07.TabIndex = 102;
			this.L_DK_07.Text = "L_DK_07";
			this.L_DK_07.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_08
			// 
			this.L_DK_08.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_08.Location = new System.Drawing.Point(608, 176);
			this.L_DK_08.Name = "L_DK_08";
			this.L_DK_08.Size = new System.Drawing.Size(20, 18);
			this.L_DK_08.TabIndex = 103;
			this.L_DK_08.Text = "L_DK_08";
			this.L_DK_08.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_09
			// 
			this.L_DK_09.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_09.Location = new System.Drawing.Point(608, 152);
			this.L_DK_09.Name = "L_DK_09";
			this.L_DK_09.Size = new System.Drawing.Size(20, 18);
			this.L_DK_09.TabIndex = 104;
			this.L_DK_09.Text = "L_DK_09";
			this.L_DK_09.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_10
			// 
			this.L_DK_10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_10.Location = new System.Drawing.Point(608, 128);
			this.L_DK_10.Name = "L_DK_10";
			this.L_DK_10.Size = new System.Drawing.Size(20, 18);
			this.L_DK_10.TabIndex = 105;
			this.L_DK_10.Text = "L_DK_10";
			this.L_DK_10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_11
			// 
			this.L_DK_11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_11.Location = new System.Drawing.Point(608, 104);
			this.L_DK_11.Name = "L_DK_11";
			this.L_DK_11.Size = new System.Drawing.Size(20, 18);
			this.L_DK_11.TabIndex = 106;
			this.L_DK_11.Text = "L_DK_11";
			this.L_DK_11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_DK_12
			// 
			this.L_DK_12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_DK_12.Location = new System.Drawing.Point(608, 80);
			this.L_DK_12.Name = "L_DK_12";
			this.L_DK_12.Size = new System.Drawing.Size(20, 18);
			this.L_DK_12.TabIndex = 96;
			this.L_DK_12.Text = "L_DK_12";
			this.L_DK_12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// L_OpsS_01
			// 
			this.L_OpsS_01.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_01.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_01.Location = new System.Drawing.Point(416, 344);
			this.L_OpsS_01.Name = "L_OpsS_01";
			this.L_OpsS_01.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_01.TabIndex = 107;
			this.L_OpsS_01.Text = "L_OpsS_01";
			// 
			// L_OpsS_02
			// 
			this.L_OpsS_02.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_02.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_02.Location = new System.Drawing.Point(416, 320);
			this.L_OpsS_02.Name = "L_OpsS_02";
			this.L_OpsS_02.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_02.TabIndex = 109;
			this.L_OpsS_02.Text = "L_OpsS_02";
			// 
			// L_OpsS_03
			// 
			this.L_OpsS_03.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_03.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_03.Location = new System.Drawing.Point(416, 296);
			this.L_OpsS_03.Name = "L_OpsS_03";
			this.L_OpsS_03.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_03.TabIndex = 110;
			this.L_OpsS_03.Text = "L_OpsS_03";
			// 
			// L_OpsS_04
			// 
			this.L_OpsS_04.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_04.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_04.Location = new System.Drawing.Point(416, 272);
			this.L_OpsS_04.Name = "L_OpsS_04";
			this.L_OpsS_04.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_04.TabIndex = 111;
			this.L_OpsS_04.Text = "L_OpsS_04";
			// 
			// L_OpsS_05
			// 
			this.L_OpsS_05.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_05.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_05.Location = new System.Drawing.Point(416, 248);
			this.L_OpsS_05.Name = "L_OpsS_05";
			this.L_OpsS_05.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_05.TabIndex = 112;
			this.L_OpsS_05.Text = "L_OpsS_05";
			// 
			// L_OpsS_06
			// 
			this.L_OpsS_06.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_06.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_06.Location = new System.Drawing.Point(416, 224);
			this.L_OpsS_06.Name = "L_OpsS_06";
			this.L_OpsS_06.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_06.TabIndex = 113;
			this.L_OpsS_06.Text = "L_OpsS_06";
			// 
			// L_OpsS_07
			// 
			this.L_OpsS_07.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_07.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_07.Location = new System.Drawing.Point(416, 200);
			this.L_OpsS_07.Name = "L_OpsS_07";
			this.L_OpsS_07.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_07.TabIndex = 114;
			this.L_OpsS_07.Text = "L_OpsS_07";
			// 
			// L_OpsS_08
			// 
			this.L_OpsS_08.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_08.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_08.Location = new System.Drawing.Point(416, 176);
			this.L_OpsS_08.Name = "L_OpsS_08";
			this.L_OpsS_08.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_08.TabIndex = 115;
			this.L_OpsS_08.Text = "L_OpsS_08";
			// 
			// L_OpsS_09
			// 
			this.L_OpsS_09.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_09.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_09.Location = new System.Drawing.Point(416, 152);
			this.L_OpsS_09.Name = "L_OpsS_09";
			this.L_OpsS_09.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_09.TabIndex = 116;
			this.L_OpsS_09.Text = "L_OpsS_09";
			// 
			// L_OpsS_10
			// 
			this.L_OpsS_10.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_10.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_10.Location = new System.Drawing.Point(416, 128);
			this.L_OpsS_10.Name = "L_OpsS_10";
			this.L_OpsS_10.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_10.TabIndex = 117;
			this.L_OpsS_10.Text = "L_OpsS_10";
			// 
			// L_OpsS_11
			// 
			this.L_OpsS_11.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_11.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_11.Location = new System.Drawing.Point(416, 104);
			this.L_OpsS_11.Name = "L_OpsS_11";
			this.L_OpsS_11.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_11.TabIndex = 118;
			this.L_OpsS_11.Text = "L_OpsS_11";
			// 
			// L_OpsS_12
			// 
			this.L_OpsS_12.BackColor = System.Drawing.Color.Transparent;
			this.L_OpsS_12.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_OpsS_12.Location = new System.Drawing.Point(416, 80);
			this.L_OpsS_12.Name = "L_OpsS_12";
			this.L_OpsS_12.Size = new System.Drawing.Size(35, 18);
			this.L_OpsS_12.TabIndex = 108;
			this.L_OpsS_12.Text = "L_OpsS_12";
			// 
			// Qu
			// 
			this.Qu.BackColor = System.Drawing.Color.Transparent;
			this.Qu.Image = ((System.Drawing.Image)(resources.GetObject("Qu.Image")));
			this.Qu.Location = new System.Drawing.Point(119, 78);
			this.Qu.Name = "Qu";
			this.Qu.Size = new System.Drawing.Size(20, 20);
			this.Qu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.Qu.TabIndex = 119;
			this.Qu.TabStop = false;
			// 
			// timer_Position
			// 
			this.timer_Position.Interval = 500;
			this.timer_Position.Tick += new System.EventHandler(this.timer_Position_Tick);
			// 
			// PB_12
			// 
			this.PB_12.BackColor = System.Drawing.Color.Transparent;
			this.PB_12.Image = ((System.Drawing.Image)(resources.GetObject("PB_12.Image")));
			this.PB_12.Location = new System.Drawing.Point(16, 78);
			this.PB_12.Name = "PB_12";
			this.PB_12.Size = new System.Drawing.Size(7, 26);
			this.PB_12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_12.TabIndex = 125;
			this.PB_12.TabStop = false;
			// 
			// PB_11
			// 
			this.PB_11.BackColor = System.Drawing.Color.Transparent;
			this.PB_11.Image = ((System.Drawing.Image)(resources.GetObject("PB_11.Image")));
			this.PB_11.Location = new System.Drawing.Point(16, 104);
			this.PB_11.Name = "PB_11";
			this.PB_11.Size = new System.Drawing.Size(7, 24);
			this.PB_11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_11.TabIndex = 126;
			this.PB_11.TabStop = false;
			// 
			// PB_10
			// 
			this.PB_10.BackColor = System.Drawing.Color.Transparent;
			this.PB_10.Image = ((System.Drawing.Image)(resources.GetObject("PB_10.Image")));
			this.PB_10.Location = new System.Drawing.Point(16, 128);
			this.PB_10.Name = "PB_10";
			this.PB_10.Size = new System.Drawing.Size(7, 24);
			this.PB_10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_10.TabIndex = 127;
			this.PB_10.TabStop = false;
			// 
			// PB_09
			// 
			this.PB_09.BackColor = System.Drawing.Color.Transparent;
			this.PB_09.Image = ((System.Drawing.Image)(resources.GetObject("PB_09.Image")));
			this.PB_09.Location = new System.Drawing.Point(16, 152);
			this.PB_09.Name = "PB_09";
			this.PB_09.Size = new System.Drawing.Size(7, 24);
			this.PB_09.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_09.TabIndex = 128;
			this.PB_09.TabStop = false;
			// 
			// PB_08
			// 
			this.PB_08.BackColor = System.Drawing.Color.Transparent;
			this.PB_08.Image = ((System.Drawing.Image)(resources.GetObject("PB_08.Image")));
			this.PB_08.Location = new System.Drawing.Point(16, 176);
			this.PB_08.Name = "PB_08";
			this.PB_08.Size = new System.Drawing.Size(7, 24);
			this.PB_08.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_08.TabIndex = 129;
			this.PB_08.TabStop = false;
			// 
			// PB_07
			// 
			this.PB_07.BackColor = System.Drawing.Color.Transparent;
			this.PB_07.Image = ((System.Drawing.Image)(resources.GetObject("PB_07.Image")));
			this.PB_07.Location = new System.Drawing.Point(16, 200);
			this.PB_07.Name = "PB_07";
			this.PB_07.Size = new System.Drawing.Size(7, 24);
			this.PB_07.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_07.TabIndex = 130;
			this.PB_07.TabStop = false;
			// 
			// PB_06
			// 
			this.PB_06.BackColor = System.Drawing.Color.Transparent;
			this.PB_06.Image = ((System.Drawing.Image)(resources.GetObject("PB_06.Image")));
			this.PB_06.Location = new System.Drawing.Point(16, 224);
			this.PB_06.Name = "PB_06";
			this.PB_06.Size = new System.Drawing.Size(7, 24);
			this.PB_06.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_06.TabIndex = 131;
			this.PB_06.TabStop = false;
			// 
			// PB_05
			// 
			this.PB_05.BackColor = System.Drawing.Color.Transparent;
			this.PB_05.Image = ((System.Drawing.Image)(resources.GetObject("PB_05.Image")));
			this.PB_05.Location = new System.Drawing.Point(16, 248);
			this.PB_05.Name = "PB_05";
			this.PB_05.Size = new System.Drawing.Size(7, 24);
			this.PB_05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_05.TabIndex = 132;
			this.PB_05.TabStop = false;
			// 
			// PB_04
			// 
			this.PB_04.BackColor = System.Drawing.Color.Transparent;
			this.PB_04.Image = ((System.Drawing.Image)(resources.GetObject("PB_04.Image")));
			this.PB_04.Location = new System.Drawing.Point(16, 272);
			this.PB_04.Name = "PB_04";
			this.PB_04.Size = new System.Drawing.Size(7, 24);
			this.PB_04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_04.TabIndex = 133;
			this.PB_04.TabStop = false;
			// 
			// PB_03
			// 
			this.PB_03.BackColor = System.Drawing.Color.Transparent;
			this.PB_03.Image = ((System.Drawing.Image)(resources.GetObject("PB_03.Image")));
			this.PB_03.Location = new System.Drawing.Point(16, 296);
			this.PB_03.Name = "PB_03";
			this.PB_03.Size = new System.Drawing.Size(7, 24);
			this.PB_03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_03.TabIndex = 134;
			this.PB_03.TabStop = false;
			// 
			// PB_02
			// 
			this.PB_02.BackColor = System.Drawing.Color.Transparent;
			this.PB_02.Image = ((System.Drawing.Image)(resources.GetObject("PB_02.Image")));
			this.PB_02.Location = new System.Drawing.Point(16, 320);
			this.PB_02.Name = "PB_02";
			this.PB_02.Size = new System.Drawing.Size(7, 24);
			this.PB_02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_02.TabIndex = 135;
			this.PB_02.TabStop = false;
			// 
			// PB_01
			// 
			this.PB_01.BackColor = System.Drawing.Color.Transparent;
			this.PB_01.Image = ((System.Drawing.Image)(resources.GetObject("PB_01.Image")));
			this.PB_01.Location = new System.Drawing.Point(16, 344);
			this.PB_01.Name = "PB_01";
			this.PB_01.Size = new System.Drawing.Size(7, 24);
			this.PB_01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_01.TabIndex = 136;
			this.PB_01.TabStop = false;
			// 
			// PB_13
			// 
			this.PB_13.BackColor = System.Drawing.Color.Transparent;
			this.PB_13.Image = ((System.Drawing.Image)(resources.GetObject("PB_13.Image")));
			this.PB_13.Location = new System.Drawing.Point(16, 53);
			this.PB_13.Name = "PB_13";
			this.PB_13.Size = new System.Drawing.Size(7, 27);
			this.PB_13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PB_13.TabIndex = 137;
			this.PB_13.TabStop = false;
			this.PB_13.Visible = false;
			// 
			// PL_04
			// 
			this.PL_04.Image = ((System.Drawing.Image)(resources.GetObject("PL_04.Image")));
			this.PL_04.Location = new System.Drawing.Point(0, 272);
			this.PL_04.Name = "PL_04";
			this.PL_04.Size = new System.Drawing.Size(128, 1);
			this.PL_04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_04.TabIndex = 138;
			this.PL_04.TabStop = false;
			this.PL_04.Visible = false;
			// 
			// PL_01
			// 
			this.PL_01.Image = ((System.Drawing.Image)(resources.GetObject("PL_01.Image")));
			this.PL_01.Location = new System.Drawing.Point(0, 344);
			this.PL_01.Name = "PL_01";
			this.PL_01.Size = new System.Drawing.Size(128, 1);
			this.PL_01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_01.TabIndex = 139;
			this.PL_01.TabStop = false;
			this.PL_01.Visible = false;
			// 
			// PL_02
			// 
			this.PL_02.Image = ((System.Drawing.Image)(resources.GetObject("PL_02.Image")));
			this.PL_02.Location = new System.Drawing.Point(0, 320);
			this.PL_02.Name = "PL_02";
			this.PL_02.Size = new System.Drawing.Size(128, 1);
			this.PL_02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_02.TabIndex = 140;
			this.PL_02.TabStop = false;
			this.PL_02.Visible = false;
			// 
			// PL_03
			// 
			this.PL_03.Image = ((System.Drawing.Image)(resources.GetObject("PL_03.Image")));
			this.PL_03.Location = new System.Drawing.Point(0, 296);
			this.PL_03.Name = "PL_03";
			this.PL_03.Size = new System.Drawing.Size(128, 1);
			this.PL_03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_03.TabIndex = 141;
			this.PL_03.TabStop = false;
			this.PL_03.Visible = false;
			// 
			// PL_05
			// 
			this.PL_05.Image = ((System.Drawing.Image)(resources.GetObject("PL_05.Image")));
			this.PL_05.Location = new System.Drawing.Point(0, 248);
			this.PL_05.Name = "PL_05";
			this.PL_05.Size = new System.Drawing.Size(128, 1);
			this.PL_05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_05.TabIndex = 142;
			this.PL_05.TabStop = false;
			this.PL_05.Visible = false;
			// 
			// PL_06
			// 
			this.PL_06.Image = ((System.Drawing.Image)(resources.GetObject("PL_06.Image")));
			this.PL_06.Location = new System.Drawing.Point(0, 224);
			this.PL_06.Name = "PL_06";
			this.PL_06.Size = new System.Drawing.Size(128, 1);
			this.PL_06.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_06.TabIndex = 143;
			this.PL_06.TabStop = false;
			this.PL_06.Visible = false;
			// 
			// PL_07
			// 
			this.PL_07.Image = ((System.Drawing.Image)(resources.GetObject("PL_07.Image")));
			this.PL_07.Location = new System.Drawing.Point(0, 200);
			this.PL_07.Name = "PL_07";
			this.PL_07.Size = new System.Drawing.Size(128, 1);
			this.PL_07.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_07.TabIndex = 144;
			this.PL_07.TabStop = false;
			this.PL_07.Visible = false;
			// 
			// PL_08
			// 
			this.PL_08.Image = ((System.Drawing.Image)(resources.GetObject("PL_08.Image")));
			this.PL_08.Location = new System.Drawing.Point(0, 176);
			this.PL_08.Name = "PL_08";
			this.PL_08.Size = new System.Drawing.Size(128, 1);
			this.PL_08.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_08.TabIndex = 145;
			this.PL_08.TabStop = false;
			this.PL_08.Visible = false;
			// 
			// PL_09
			// 
			this.PL_09.Image = ((System.Drawing.Image)(resources.GetObject("PL_09.Image")));
			this.PL_09.Location = new System.Drawing.Point(0, 152);
			this.PL_09.Name = "PL_09";
			this.PL_09.Size = new System.Drawing.Size(128, 1);
			this.PL_09.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_09.TabIndex = 146;
			this.PL_09.TabStop = false;
			this.PL_09.Visible = false;
			// 
			// PL_10
			// 
			this.PL_10.Image = ((System.Drawing.Image)(resources.GetObject("PL_10.Image")));
			this.PL_10.Location = new System.Drawing.Point(0, 128);
			this.PL_10.Name = "PL_10";
			this.PL_10.Size = new System.Drawing.Size(128, 1);
			this.PL_10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_10.TabIndex = 147;
			this.PL_10.TabStop = false;
			this.PL_10.Visible = false;
			// 
			// PL_11
			// 
			this.PL_11.Image = ((System.Drawing.Image)(resources.GetObject("PL_11.Image")));
			this.PL_11.Location = new System.Drawing.Point(0, 104);
			this.PL_11.Name = "PL_11";
			this.PL_11.Size = new System.Drawing.Size(128, 1);
			this.PL_11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PL_11.TabIndex = 148;
			this.PL_11.TabStop = false;
			this.PL_11.Visible = false;
			// 
			// Deko1
			// 
			this.Deko1.Image = ((System.Drawing.Image)(resources.GetObject("Deko1.Image")));
			this.Deko1.Location = new System.Drawing.Point(0, 53);
			this.Deko1.Name = "Deko1";
			this.Deko1.Size = new System.Drawing.Size(640, 2);
			this.Deko1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.Deko1.TabIndex = 150;
			this.Deko1.TabStop = false;
			// 
			// Deko2
			// 
			this.Deko2.Image = ((System.Drawing.Image)(resources.GetObject("Deko2.Image")));
			this.Deko2.Location = new System.Drawing.Point(-5, 74);
			this.Deko2.Name = "Deko2";
			this.Deko2.Size = new System.Drawing.Size(640, 2);
			this.Deko2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.Deko2.TabIndex = 151;
			this.Deko2.TabStop = false;
			// 
			// DekoL1
			// 
			this.DekoL1.Image = ((System.Drawing.Image)(resources.GetObject("DekoL1.Image")));
			this.DekoL1.Location = new System.Drawing.Point(128, 76);
			this.DekoL1.Name = "DekoL1";
			this.DekoL1.Size = new System.Drawing.Size(2, 290);
			this.DekoL1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL1.TabIndex = 152;
			this.DekoL1.TabStop = false;
			// 
			// DekoL2
			// 
			this.DekoL2.Image = ((System.Drawing.Image)(resources.GetObject("DekoL2.Image")));
			this.DekoL2.Location = new System.Drawing.Point(216, 76);
			this.DekoL2.Name = "DekoL2";
			this.DekoL2.Size = new System.Drawing.Size(1, 290);
			this.DekoL2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL2.TabIndex = 153;
			this.DekoL2.TabStop = false;
			// 
			// DekoL4
			// 
			this.DekoL4.Image = ((System.Drawing.Image)(resources.GetObject("DekoL4.Image")));
			this.DekoL4.Location = new System.Drawing.Point(552, 76);
			this.DekoL4.Name = "DekoL4";
			this.DekoL4.Size = new System.Drawing.Size(2, 290);
			this.DekoL4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL4.TabIndex = 154;
			this.DekoL4.TabStop = false;
			// 
			// DekoL3
			// 
			this.DekoL3.Image = ((System.Drawing.Image)(resources.GetObject("DekoL3.Image")));
			this.DekoL3.Location = new System.Drawing.Point(464, 76);
			this.DekoL3.Name = "DekoL3";
			this.DekoL3.Size = new System.Drawing.Size(2, 290);
			this.DekoL3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL3.TabIndex = 155;
			this.DekoL3.TabStop = false;
			// 
			// DekoL2a
			// 
			this.DekoL2a.Image = ((System.Drawing.Image)(resources.GetObject("DekoL2a.Image")));
			this.DekoL2a.Location = new System.Drawing.Point(224, 76);
			this.DekoL2a.Name = "DekoL2a";
			this.DekoL2a.Size = new System.Drawing.Size(3, 290);
			this.DekoL2a.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL2a.TabIndex = 156;
			this.DekoL2a.TabStop = false;
			// 
			// Deko3
			// 
			this.Deko3.Image = ((System.Drawing.Image)(resources.GetObject("Deko3.Image")));
			this.Deko3.Location = new System.Drawing.Point(0, 365);
			this.Deko3.Name = "Deko3";
			this.Deko3.Size = new System.Drawing.Size(640, 2);
			this.Deko3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.Deko3.TabIndex = 157;
			this.Deko3.TabStop = false;
			// 
			// DekoLL1
			// 
			this.DekoLL1.Image = ((System.Drawing.Image)(resources.GetObject("DekoLL1.Image")));
			this.DekoLL1.Location = new System.Drawing.Point(128, 0);
			this.DekoLL1.Name = "DekoLL1";
			this.DekoLL1.Size = new System.Drawing.Size(2, 54);
			this.DekoLL1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoLL1.TabIndex = 158;
			this.DekoLL1.TabStop = false;
			// 
			// DekoLL2
			// 
			this.DekoLL2.Image = ((System.Drawing.Image)(resources.GetObject("DekoLL2.Image")));
			this.DekoLL2.Location = new System.Drawing.Point(286, 0);
			this.DekoLL2.Name = "DekoLL2";
			this.DekoLL2.Size = new System.Drawing.Size(2, 54);
			this.DekoLL2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoLL2.TabIndex = 159;
			this.DekoLL2.TabStop = false;
			// 
			// DekoLL3
			// 
			this.DekoLL3.Image = ((System.Drawing.Image)(resources.GetObject("DekoLL3.Image")));
			this.DekoLL3.Location = new System.Drawing.Point(464, 0);
			this.DekoLL3.Name = "DekoLL3";
			this.DekoLL3.Size = new System.Drawing.Size(2, 54);
			this.DekoLL3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoLL3.TabIndex = 160;
			this.DekoLL3.TabStop = false;
			// 
			// PanelButton
			// 
			this.PanelButton.BackColor = System.Drawing.Color.LightGray;
			this.PanelButton.Controls.Add(this.B_G);
			this.PanelButton.Controls.Add(this.B_GNT);
			this.PanelButton.Controls.Add(this.B_Plan);
			this.PanelButton.Controls.Add(this.B_Zeit);
			this.PanelButton.Controls.Add(this.B_GW);
			this.PanelButton.Controls.Add(this.B_LW);
			this.PanelButton.Controls.Add(this.B_LaT);
			this.PanelButton.Controls.Add(this.B_LaD);
			this.PanelButton.Controls.Add(this.B_FSD);
			this.PanelButton.Controls.Add(this.B_Zug);
			this.PanelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.PanelButton.Location = new System.Drawing.Point(0, 412);
			this.PanelButton.Name = "PanelButton";
			this.PanelButton.Size = new System.Drawing.Size(630, 48);
			this.PanelButton.TabIndex = 161;
			// 
			// B_G
			// 
			this.B_G.Location = new System.Drawing.Point(575, 3);
			this.B_G.Name = "B_G";
			this.B_G.Size = new System.Drawing.Size(52, 42);
			this.B_G.TabIndex = 19;
			this.B_G.TabStop = false;
			this.B_G.Text = "G";
			this.B_G.Click += new System.EventHandler(this.B_G_Click);
			// 
			// B_GNT
			// 
			this.B_GNT.Location = new System.Drawing.Point(513, 3);
			this.B_GNT.Name = "B_GNT";
			this.B_GNT.Size = new System.Drawing.Size(52, 42);
			this.B_GNT.TabIndex = 18;
			this.B_GNT.TabStop = false;
			this.B_GNT.Text = "GNT";
			this.B_GNT.Visible = false;
			this.B_GNT.Click += new System.EventHandler(this.B_GNT_Click);
			// 
			// B_Plan
			// 
			this.B_Plan.Location = new System.Drawing.Point(449, 3);
			this.B_Plan.Name = "B_Plan";
			this.B_Plan.Size = new System.Drawing.Size(52, 42);
			this.B_Plan.TabIndex = 17;
			this.B_Plan.TabStop = false;
			this.B_Plan.Text = "Plan";
			this.B_Plan.Visible = false;
			this.B_Plan.Click += new System.EventHandler(this.B_Plan_Click);
			// 
			// B_Zeit
			// 
			this.B_Zeit.Location = new System.Drawing.Point(385, 3);
			this.B_Zeit.Name = "B_Zeit";
			this.B_Zeit.Size = new System.Drawing.Size(52, 42);
			this.B_Zeit.TabIndex = 16;
			this.B_Zeit.TabStop = false;
			this.B_Zeit.Text = "Zeit";
			this.B_Zeit.Click += new System.EventHandler(this.B_Zeit_Click);
			// 
			// B_GW
			// 
			this.B_GW.Location = new System.Drawing.Point(322, 3);
			this.B_GW.Name = "B_GW";
			this.B_GW.Size = new System.Drawing.Size(52, 42);
			this.B_GW.TabIndex = 15;
			this.B_GW.TabStop = false;
			this.B_GW.Text = "GW";
			this.B_GW.Visible = false;
			this.B_GW.Click += new System.EventHandler(this.B_GW_Click);
			// 
			// B_LW
			// 
			this.B_LW.Location = new System.Drawing.Point(257, 3);
			this.B_LW.Name = "B_LW";
			this.B_LW.Size = new System.Drawing.Size(52, 42);
			this.B_LW.TabIndex = 14;
			this.B_LW.TabStop = false;
			this.B_LW.Text = "LW";
			this.B_LW.Visible = false;
			this.B_LW.Click += new System.EventHandler(this.B_LW_Click);
			// 
			// B_LaT
			// 
			this.B_LaT.Location = new System.Drawing.Point(193, 3);
			this.B_LaT.Name = "B_LaT";
			this.B_LaT.Size = new System.Drawing.Size(52, 42);
			this.B_LaT.TabIndex = 13;
			this.B_LaT.TabStop = false;
			this.B_LaT.Text = "LaT";
			this.B_LaT.Visible = false;
			this.B_LaT.Click += new System.EventHandler(this.B_LaT_Click);
			// 
			// B_LaD
			// 
			this.B_LaD.Location = new System.Drawing.Point(129, 3);
			this.B_LaD.Name = "B_LaD";
			this.B_LaD.Size = new System.Drawing.Size(52, 42);
			this.B_LaD.TabIndex = 12;
			this.B_LaD.TabStop = false;
			this.B_LaD.Text = "LaD";
			this.B_LaD.Visible = false;
			this.B_LaD.Click += new System.EventHandler(this.B_LaD_Click);
			// 
			// B_FSD
			// 
			this.B_FSD.Location = new System.Drawing.Point(65, 3);
			this.B_FSD.Name = "B_FSD";
			this.B_FSD.Size = new System.Drawing.Size(52, 42);
			this.B_FSD.TabIndex = 11;
			this.B_FSD.TabStop = false;
			this.B_FSD.Text = "FSD";
			this.B_FSD.Click += new System.EventHandler(this.B_FSD_Click);
			// 
			// B_Zug
			// 
			this.B_Zug.Location = new System.Drawing.Point(4, 3);
			this.B_Zug.Name = "B_Zug";
			this.B_Zug.Size = new System.Drawing.Size(52, 42);
			this.B_Zug.TabIndex = 10;
			this.B_Zug.TabStop = false;
			this.B_Zug.Text = "Zug";
			this.B_Zug.Click += new System.EventHandler(this.B_Zug_Click);
			// 
			// PanelDown
			// 
			this.PanelDown.BackColor = System.Drawing.Color.OldLace;
			this.PanelDown.Controls.Add(this.Down_RW);
			this.PanelDown.Controls.Add(this.Down_Verspaetung);
			this.PanelDown.Controls.Add(this.Down_Timer);
			this.PanelDown.Controls.Add(this.Down_Radio);
			this.PanelDown.Controls.Add(this.Down_Speed);
			this.PanelDown.Location = new System.Drawing.Point(0, 387);
			this.PanelDown.Name = "PanelDown";
			this.PanelDown.Size = new System.Drawing.Size(416, 25);
			this.PanelDown.TabIndex = 162;
			// 
			// Down_RW
			// 
			this.Down_RW.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Down_RW.Location = new System.Drawing.Point(192, 6);
			this.Down_RW.Name = "Down_RW";
			this.Down_RW.Size = new System.Drawing.Size(48, 18);
			this.Down_RW.TabIndex = 129;
			this.Down_RW.Text = "RW/r";
			this.Down_RW.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// Down_Verspaetung
			// 
			this.Down_Verspaetung.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Down_Verspaetung.Location = new System.Drawing.Point(344, 6);
			this.Down_Verspaetung.Name = "Down_Verspaetung";
			this.Down_Verspaetung.Size = new System.Drawing.Size(72, 18);
			this.Down_Verspaetung.TabIndex = 128;
			this.Down_Verspaetung.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// Down_Timer
			// 
			this.Down_Timer.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Down_Timer.Location = new System.Drawing.Point(6, 6);
			this.Down_Timer.Name = "Down_Timer";
			this.Down_Timer.Size = new System.Drawing.Size(76, 18);
			this.Down_Timer.TabIndex = 127;
			// 
			// Down_Radio
			// 
			this.Down_Radio.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Down_Radio.Location = new System.Drawing.Point(264, 6);
			this.Down_Radio.Name = "Down_Radio";
			this.Down_Radio.Size = new System.Drawing.Size(72, 18);
			this.Down_Radio.TabIndex = 126;
			this.Down_Radio.Text = "- ZF -";
			this.Down_Radio.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// Down_Speed
			// 
			this.Down_Speed.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Down_Speed.Location = new System.Drawing.Point(80, 6);
			this.Down_Speed.Name = "Down_Speed";
			this.Down_Speed.Size = new System.Drawing.Size(48, 18);
			this.Down_Speed.TabIndex = 125;
			this.Down_Speed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// DekoDown4
			// 
			this.DekoDown4.Image = ((System.Drawing.Image)(resources.GetObject("DekoDown4.Image")));
			this.DekoDown4.Location = new System.Drawing.Point(-8, 385);
			this.DekoDown4.Name = "DekoDown4";
			this.DekoDown4.Size = new System.Drawing.Size(640, 2);
			this.DekoDown4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoDown4.TabIndex = 163;
			this.DekoDown4.TabStop = false;
			// 
			// DekoL5
			// 
			this.DekoL5.Image = ((System.Drawing.Image)(resources.GetObject("DekoL5.Image")));
			this.DekoL5.Location = new System.Drawing.Point(416, 385);
			this.DekoL5.Name = "DekoL5";
			this.DekoL5.Size = new System.Drawing.Size(2, 27);
			this.DekoL5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.DekoL5.TabIndex = 164;
			this.DekoL5.TabStop = false;
			// 
			// pB_White_Up
			// 
			this.pB_White_Up.Image = ((System.Drawing.Image)(resources.GetObject("pB_White_Up.Image")));
			this.pB_White_Up.Location = new System.Drawing.Point(432, 208);
			this.pB_White_Up.Name = "pB_White_Up";
			this.pB_White_Up.Size = new System.Drawing.Size(100, 16);
			this.pB_White_Up.TabIndex = 165;
			this.pB_White_Up.TabStop = false;
			this.pB_White_Up.Visible = false;
			// 
			// pB_White_Down
			// 
			this.pB_White_Down.Image = ((System.Drawing.Image)(resources.GetObject("pB_White_Down.Image")));
			this.pB_White_Down.Location = new System.Drawing.Point(352, 168);
			this.pB_White_Down.Name = "pB_White_Down";
			this.pB_White_Down.Size = new System.Drawing.Size(15, 98);
			this.pB_White_Down.TabIndex = 166;
			this.pB_White_Down.TabStop = false;
			this.pB_White_Down.Visible = false;
			// 
			// timer_Sync
			// 
			this.timer_Sync.Enabled = true;
			this.timer_Sync.Interval = 1;
			this.timer_Sync.Tick += new System.EventHandler(this.timer_SyncUhr_Tick);
			// 
			// QuW
			// 
			this.QuW.BackColor = System.Drawing.Color.Transparent;
			this.QuW.Image = ((System.Drawing.Image)(resources.GetObject("QuW.Image")));
			this.QuW.Location = new System.Drawing.Point(344, 136);
			this.QuW.Name = "QuW";
			this.QuW.Size = new System.Drawing.Size(20, 20);
			this.QuW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.QuW.TabIndex = 167;
			this.QuW.TabStop = false;
			this.QuW.Visible = false;
			// 
			// J_01
			// 
			this.J_01.Image = ((System.Drawing.Image)(resources.GetObject("J_01.Image")));
			this.J_01.Location = new System.Drawing.Point(128, 341);
			this.J_01.Name = "J_01";
			this.J_01.Size = new System.Drawing.Size(70, 1);
			this.J_01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_01.TabIndex = 168;
			this.J_01.TabStop = false;
			this.J_01.Visible = false;
			// 
			// J_02
			// 
			this.J_02.Image = ((System.Drawing.Image)(resources.GetObject("J_02.Image")));
			this.J_02.Location = new System.Drawing.Point(128, 317);
			this.J_02.Name = "J_02";
			this.J_02.Size = new System.Drawing.Size(70, 1);
			this.J_02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_02.TabIndex = 169;
			this.J_02.TabStop = false;
			this.J_02.Visible = false;
			// 
			// J_03
			// 
			this.J_03.Image = ((System.Drawing.Image)(resources.GetObject("J_03.Image")));
			this.J_03.Location = new System.Drawing.Point(128, 293);
			this.J_03.Name = "J_03";
			this.J_03.Size = new System.Drawing.Size(70, 1);
			this.J_03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_03.TabIndex = 170;
			this.J_03.TabStop = false;
			this.J_03.Visible = false;
			// 
			// J_04
			// 
			this.J_04.Image = ((System.Drawing.Image)(resources.GetObject("J_04.Image")));
			this.J_04.Location = new System.Drawing.Point(128, 269);
			this.J_04.Name = "J_04";
			this.J_04.Size = new System.Drawing.Size(70, 1);
			this.J_04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_04.TabIndex = 171;
			this.J_04.TabStop = false;
			this.J_04.Visible = false;
			// 
			// J_05
			// 
			this.J_05.Image = ((System.Drawing.Image)(resources.GetObject("J_05.Image")));
			this.J_05.Location = new System.Drawing.Point(128, 245);
			this.J_05.Name = "J_05";
			this.J_05.Size = new System.Drawing.Size(70, 1);
			this.J_05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_05.TabIndex = 172;
			this.J_05.TabStop = false;
			this.J_05.Visible = false;
			// 
			// J_06
			// 
			this.J_06.Image = ((System.Drawing.Image)(resources.GetObject("J_06.Image")));
			this.J_06.Location = new System.Drawing.Point(128, 221);
			this.J_06.Name = "J_06";
			this.J_06.Size = new System.Drawing.Size(70, 1);
			this.J_06.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_06.TabIndex = 173;
			this.J_06.TabStop = false;
			this.J_06.Visible = false;
			// 
			// J_10
			// 
			this.J_10.Image = ((System.Drawing.Image)(resources.GetObject("J_10.Image")));
			this.J_10.Location = new System.Drawing.Point(128, 125);
			this.J_10.Name = "J_10";
			this.J_10.Size = new System.Drawing.Size(70, 1);
			this.J_10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_10.TabIndex = 174;
			this.J_10.TabStop = false;
			this.J_10.Visible = false;
			// 
			// J_09
			// 
			this.J_09.Image = ((System.Drawing.Image)(resources.GetObject("J_09.Image")));
			this.J_09.Location = new System.Drawing.Point(128, 149);
			this.J_09.Name = "J_09";
			this.J_09.Size = new System.Drawing.Size(70, 1);
			this.J_09.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_09.TabIndex = 175;
			this.J_09.TabStop = false;
			this.J_09.Visible = false;
			// 
			// J_08
			// 
			this.J_08.Image = ((System.Drawing.Image)(resources.GetObject("J_08.Image")));
			this.J_08.Location = new System.Drawing.Point(128, 173);
			this.J_08.Name = "J_08";
			this.J_08.Size = new System.Drawing.Size(70, 1);
			this.J_08.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_08.TabIndex = 176;
			this.J_08.TabStop = false;
			this.J_08.Visible = false;
			// 
			// J_07
			// 
			this.J_07.Image = ((System.Drawing.Image)(resources.GetObject("J_07.Image")));
			this.J_07.Location = new System.Drawing.Point(128, 197);
			this.J_07.Name = "J_07";
			this.J_07.Size = new System.Drawing.Size(70, 1);
			this.J_07.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_07.TabIndex = 177;
			this.J_07.TabStop = false;
			this.J_07.Visible = false;
			// 
			// J_11
			// 
			this.J_11.Image = ((System.Drawing.Image)(resources.GetObject("J_11.Image")));
			this.J_11.Location = new System.Drawing.Point(128, 101);
			this.J_11.Name = "J_11";
			this.J_11.Size = new System.Drawing.Size(70, 1);
			this.J_11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.J_11.TabIndex = 178;
			this.J_11.TabStop = false;
			this.J_11.Visible = false;
			// 
			// timer_refresh
			// 
			this.timer_refresh.Enabled = true;
			this.timer_refresh.Tick += new System.EventHandler(this.timer_refresh_Tick);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(424, 392);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 18);
			this.label1.TabIndex = 179;
			this.label1.Text = "ESF";
			// 
			// l_ESF_Pos
			// 
			this.l_ESF_Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_ESF_Pos.Location = new System.Drawing.Point(456, 392);
			this.l_ESF_Pos.Name = "l_ESF_Pos";
			this.l_ESF_Pos.Size = new System.Drawing.Size(88, 18);
			this.l_ESF_Pos.TabIndex = 180;
			this.l_ESF_Pos.Text = "+ 0 kWh";
			this.l_ESF_Pos.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// l_ESF_Neg
			// 
			this.l_ESF_Neg.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_ESF_Neg.Location = new System.Drawing.Point(544, 392);
			this.l_ESF_Neg.Name = "l_ESF_Neg";
			this.l_ESF_Neg.Size = new System.Drawing.Size(72, 18);
			this.l_ESF_Neg.TabIndex = 181;
			this.l_ESF_Neg.Text = "- 0 kWh";
			this.l_ESF_Neg.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_A_13
			// 
			this.L_A_13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_A_13.Location = new System.Drawing.Point(480, 56);
			this.L_A_13.Name = "L_A_13";
			this.L_A_13.Size = new System.Drawing.Size(50, 18);
			this.L_A_13.TabIndex = 182;
			this.L_A_13.Text = "L_A_13";
			this.L_A_13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// L_D_13
			// 
			this.L_D_13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.L_D_13.Location = new System.Drawing.Point(560, 56);
			this.L_D_13.Name = "L_D_13";
			this.L_D_13.Size = new System.Drawing.Size(50, 18);
			this.L_D_13.TabIndex = 183;
			this.L_D_13.Text = "L_D_13";
			this.L_D_13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// EbulaControl
			// 
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.L_D_13);
			this.Controls.Add(this.L_A_13);
			this.Controls.Add(this.l_ESF_Neg);
			this.Controls.Add(this.l_ESF_Pos);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.J_11);
			this.Controls.Add(this.J_07);
			this.Controls.Add(this.J_08);
			this.Controls.Add(this.J_09);
			this.Controls.Add(this.J_10);
			this.Controls.Add(this.J_06);
			this.Controls.Add(this.J_05);
			this.Controls.Add(this.J_04);
			this.Controls.Add(this.J_03);
			this.Controls.Add(this.J_02);
			this.Controls.Add(this.J_01);
			this.Controls.Add(this.QuW);
			this.Controls.Add(this.pB_White_Down);
			this.Controls.Add(this.pB_White_Up);
			this.Controls.Add(this.DekoL5);
			this.Controls.Add(this.DekoDown4);
			this.Controls.Add(this.PanelDown);
			this.Controls.Add(this.PanelButton);
			this.Controls.Add(this.DekoLL3);
			this.Controls.Add(this.DekoLL2);
			this.Controls.Add(this.DekoLL1);
			this.Controls.Add(this.Deko3);
			this.Controls.Add(this.DekoL3);
			this.Controls.Add(this.DekoL4);
			this.Controls.Add(this.DekoL1);
			this.Controls.Add(this.Deko2);
			this.Controls.Add(this.Deko1);
			this.Controls.Add(this.PL_11);
			this.Controls.Add(this.PL_10);
			this.Controls.Add(this.PL_09);
			this.Controls.Add(this.PL_08);
			this.Controls.Add(this.PL_07);
			this.Controls.Add(this.PL_06);
			this.Controls.Add(this.PL_05);
			this.Controls.Add(this.PL_03);
			this.Controls.Add(this.PL_02);
			this.Controls.Add(this.PL_01);
			this.Controls.Add(this.PL_04);
			this.Controls.Add(this.PB_13);
			this.Controls.Add(this.PB_01);
			this.Controls.Add(this.PB_02);
			this.Controls.Add(this.PB_03);
			this.Controls.Add(this.PB_04);
			this.Controls.Add(this.PB_05);
			this.Controls.Add(this.PB_06);
			this.Controls.Add(this.PB_07);
			this.Controls.Add(this.PB_08);
			this.Controls.Add(this.PB_09);
			this.Controls.Add(this.PB_10);
			this.Controls.Add(this.PB_11);
			this.Controls.Add(this.PB_12);
			this.Controls.Add(this.Qu);
			this.Controls.Add(this.L_OpsS_01);
			this.Controls.Add(this.L_OpsS_02);
			this.Controls.Add(this.L_OpsS_03);
			this.Controls.Add(this.L_OpsS_04);
			this.Controls.Add(this.L_OpsS_05);
			this.Controls.Add(this.L_OpsS_06);
			this.Controls.Add(this.L_OpsS_07);
			this.Controls.Add(this.L_OpsS_08);
			this.Controls.Add(this.L_OpsS_09);
			this.Controls.Add(this.L_OpsS_10);
			this.Controls.Add(this.L_OpsS_11);
			this.Controls.Add(this.L_OpsS_12);
			this.Controls.Add(this.L_DK_01);
			this.Controls.Add(this.L_DK_02);
			this.Controls.Add(this.L_DK_03);
			this.Controls.Add(this.L_DK_04);
			this.Controls.Add(this.L_DK_05);
			this.Controls.Add(this.L_DK_06);
			this.Controls.Add(this.L_DK_07);
			this.Controls.Add(this.L_DK_08);
			this.Controls.Add(this.L_DK_09);
			this.Controls.Add(this.L_DK_10);
			this.Controls.Add(this.L_DK_11);
			this.Controls.Add(this.L_DK_12);
			this.Controls.Add(this.L_AK_01);
			this.Controls.Add(this.L_AK_02);
			this.Controls.Add(this.L_AK_03);
			this.Controls.Add(this.L_AK_04);
			this.Controls.Add(this.L_AK_05);
			this.Controls.Add(this.L_AK_06);
			this.Controls.Add(this.L_AK_07);
			this.Controls.Add(this.L_AK_08);
			this.Controls.Add(this.L_AK_09);
			this.Controls.Add(this.L_AK_10);
			this.Controls.Add(this.L_AK_11);
			this.Controls.Add(this.L_AK_12);
			this.Controls.Add(this.L_D_12);
			this.Controls.Add(this.L_D_11);
			this.Controls.Add(this.L_D_10);
			this.Controls.Add(this.L_D_09);
			this.Controls.Add(this.L_D_08);
			this.Controls.Add(this.L_D_07);
			this.Controls.Add(this.L_D_06);
			this.Controls.Add(this.L_D_05);
			this.Controls.Add(this.L_D_04);
			this.Controls.Add(this.L_D_03);
			this.Controls.Add(this.L_D_02);
			this.Controls.Add(this.L_D_01);
			this.Controls.Add(this.L_A_01);
			this.Controls.Add(this.L_A_02);
			this.Controls.Add(this.L_A_03);
			this.Controls.Add(this.L_A_04);
			this.Controls.Add(this.L_A_05);
			this.Controls.Add(this.L_A_06);
			this.Controls.Add(this.L_A_07);
			this.Controls.Add(this.L_A_08);
			this.Controls.Add(this.L_A_09);
			this.Controls.Add(this.L_A_10);
			this.Controls.Add(this.L_A_11);
			this.Controls.Add(this.L_A_12);
			this.Controls.Add(this.L_TOP);
			this.Controls.Add(this.L_DOWN);
			this.Controls.Add(this.L_Ops_01);
			this.Controls.Add(this.L_Ops_02);
			this.Controls.Add(this.L_Ops_03);
			this.Controls.Add(this.L_Ops_04);
			this.Controls.Add(this.L_Ops_05);
			this.Controls.Add(this.L_Ops_06);
			this.Controls.Add(this.L_Ops_07);
			this.Controls.Add(this.L_Ops_08);
			this.Controls.Add(this.L_Ops_09);
			this.Controls.Add(this.L_Ops_10);
			this.Controls.Add(this.L_Ops_11);
			this.Controls.Add(this.L_Ops_12);
			this.Controls.Add(this.L_Speed_12);
			this.Controls.Add(this.L_Speed_11);
			this.Controls.Add(this.L_Speed_10);
			this.Controls.Add(this.L_Speed_09);
			this.Controls.Add(this.L_Speed_08);
			this.Controls.Add(this.L_Speed_07);
			this.Controls.Add(this.L_Speed_06);
			this.Controls.Add(this.L_Speed_05);
			this.Controls.Add(this.L_Speed_04);
			this.Controls.Add(this.L_Speed_03);
			this.Controls.Add(this.L_Speed_02);
			this.Controls.Add(this.L_Speed_01);
			this.Controls.Add(this.L_Pos_01);
			this.Controls.Add(this.L_Pos_02);
			this.Controls.Add(this.L_Pos_03);
			this.Controls.Add(this.L_Pos_04);
			this.Controls.Add(this.L_Pos_05);
			this.Controls.Add(this.L_Pos_06);
			this.Controls.Add(this.L_Pos_07);
			this.Controls.Add(this.L_Pos_08);
			this.Controls.Add(this.L_Pos_09);
			this.Controls.Add(this.L_Pos_10);
			this.Controls.Add(this.L_Pos_11);
			this.Controls.Add(this.L_Pos_12);
			this.Controls.Add(this.L_Valid);
			this.Controls.Add(this.L_Zug);
			this.Controls.Add(this.L_Zeit);
			this.Controls.Add(this.L_Uhr);
			this.Controls.Add(this.DekoL2a);
			this.Controls.Add(this.DekoL2);
			this.Name = "EbulaControl";
			this.Size = new System.Drawing.Size(630, 460);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.EbulaControl_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EbulaControl_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.timer_Uhr)).EndInit();
			this.PanelButton.ResumeLayout(false);
			this.PanelDown.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        public void OnKeyDownOwn(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.EbulaControl_KeyDown(sender, e);
        }

        private void B_Zug_Click(object sender, System.EventArgs e)
        {
            timer_Sync.Interval = 1;

            SetButtons(true);

			control.XMLConf.UseDB = true;

			if (control.XMLConf.UseDB)
			{
				Zugauswahl z = new Zugauswahl(ref control, L_TOP);
				//kh.Form = z;
				z.ShowDialog();

				//kh.Form = null;

				SetButtons(false);

				if (z.DialogResult == DialogResult.Cancel)
				{
					control.buffer_trainnumber = "";
					control.buffer_traintype = "";
					return;
				}
			}
			else
			{
				Zugdialog z = new Zugdialog(ref control);
				//kh.Form = z;
				z.ShowDialog();

				//kh.Form = null;

				SetButtons(false);

				if (z.DialogResult == DialogResult.Cancel)
				{
					control.buffer_trainnumber = "";
					control.buffer_traintype = "";
					return;
				}
			}
            control.LoadTimeTableFromZusi();

            UpdateControl();
			
			if (control.RouteHasGNT()) 
				B_GNT.Enabled = true;
			else
				B_GNT.Enabled = false;

			SetButtons(false);
           
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();

        }

        private void timer_Uhr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            String h_zero = ""; String m_zero = ""; String s_zero = "";

            if (control.vtime.TimeOfDay.Hours < 10) { h_zero = ""; }
            if (control.vtime.TimeOfDay.Minutes < 10) { m_zero = "0"; }
            if (control.vtime.TimeOfDay.Seconds < 10) { s_zero = "0"; }

            L_Uhr.Text = h_zero + control.vtime.TimeOfDay.Hours.ToString()+ ":" +
                m_zero + control.vtime.TimeOfDay.Minutes.ToString() + ":" + 
                s_zero + control.vtime.TimeOfDay.Seconds.ToString(); 

            String mo_zero = ""; String d_zero = "";
            if (control.vtime.Month < 10) { mo_zero = "0"; }
            if (control.vtime.Day < 10) { d_zero = "0"; }

            L_Zeit.Text = d_zero + control.vtime.Day.ToString()+ "." +
                mo_zero + control.vtime.Month.ToString() + "." + 
                control.vtime.Year.ToString()[2] + control.vtime.Year.ToString()[3];

            // Valid

            L_Valid.Text = "EBuLa-Karte gültig bis: "+control.vtime.AddDays(1).Date.ToShortDateString();

            if (control.Route != null) 
            {
                string help = control.Route.Name;
                if (control.Route.Name.Length == 6)
                {
                    help = control.Route.Name.Remove(2,control.Route.Name.Length-3);
                }
                if (control.Route.Name.Length == 8)
                {
                    help = control.Route.Name.Remove(3,control.Route.Name.Length-4);
                }
                if (control.Route.Name.Length == 10)
                {
                    help = control.Route.Name.Remove(4,control.Route.Name.Length-5);
                }
                if (control.Route.Name.Length == 12)
                {
                    help = control.Route.Name.Remove(5,control.Route.Name.Length-6);
                }
                L_Zug.Text = help;
				//TODO
				if (control.use_DB)
					L_Zug.Text = control.buffer_trainnumber;
            }

			LoadESF();
        }

        private void vtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan diff = DateTime.Now - vdifftime;
            control.vtime = control.vtime.AddTicks(diff.Ticks);
            vdifftime = DateTime.Now;
        }

        public void OnKeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (System.Windows.Forms.Keys.NumPad1) : 
                    B_Zug_Click(this, new System.EventArgs());
                    break;
            }
        }

        private void UpdateControl()
        { 
			SomethingChanged = true;
            try
            {
				if (control.Route == null)
				{
					return;
				}

				ht = new Hashtable();

                this.SuspendLayout();

                int qu_pos = 342 - ( ((int)control.Route.Position - (int)control.Route.Offset) * 24);

                SetQu(qu_pos);

                // down verspätung

                if (verspaetung > 0)
                {
                    Down_Verspaetung.Text = "+" + verspaetung.ToString();
                }
                else
                {
                    Down_Verspaetung.Text = verspaetung.ToString();
                }
                Down_Verspaetung.Text += " min";

                // down timer
				if (control.timer_on) 
				{
					Down_Timer.Text = "T(km)";
				}
				else if (control.move_via_time)
				{
					Down_Timer.Text = "T(Uhr)";
				}
				else
                {
                    Down_Timer.Text = "man";
                }

				if (control.gnt) Down_Timer.Text += " GNT";

				if (control.left) Down_RW.Text = "RW/l";
				else Down_RW.Text = "RW/r";

                // DOWN SPEED
                for (int i = (int)control.Route.Position; i >= 0; i--)
                {
                    Entry e = (Entry)control.Route.Entrys[i];
					string check = "";
					if (control.gnt)
					{
						check = e.m_gnt_speed;
					}
					else
						check = e.m_speed;
						
					if (check == "") 
					{
						Down_Speed.Text = "";
						continue;
					}
					else
					{
						if (control.gnt)
                            Down_Speed.Text = e.m_gnt_speed;
						else
							Down_Speed.Text = e.m_speed;
						break;
					}
                }

                // DOWN RADIO
                for (int i = (int)control.Route.Position; i >= 0; i--)
                {
                    Entry e = (Entry)control.Route.Entrys[i];
                    if (e.m_type == EntryType.RADIO_MARKER)
                    {
                        Down_Radio.Text = e.m_ops_name;
                        break;
                    }
                    else
                    {
                        Down_Radio.Text = "- ZF -";
                    }
                }

                int speed = 0;

                foreach (System.Windows.Forms.Control cc in this.Controls)
                {
                    if (cc.Name.StartsWith("PB_"))
                    {
                        PictureBox pb = (PictureBox)cc;
                        int pos = Convert.ToInt32(cc.Name.Remove(0,3));
                        speed = control.Route.SearchForSpeed(pos, control.gnt);
                        double s = speed/2.2;
                        speed = (int)s;
                        pb.Location = new Point( speed , cc.Location.Y-firsttime);
						if (s > 0d && cc.Name.IndexOf("13") < 0) 
							pb.Visible = true;
						else
							pb.Visible = false;
                    }
                    if (cc.Name.StartsWith("PL"))
                    {
                        cc.Location = new Point(cc.Location.X , cc.Location.Y-firsttime);
                    }

                    if (cc.Name.StartsWith("L_Pos_"))
                    {
                        // POSITION           
                        int pos = System.Convert.ToInt32(cc.Name.Substring(6));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
                    
                        l.Text = ((Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset]).m_position;

						l.Font = new Font(l.Font.FontFamily, l.Font.Size, FontStyle.Regular);
                    }
					if (cc.Name.StartsWith("J_"))
					{
						// JUMP
						int pos = System.Convert.ToInt32(cc.Name.Substring(2));
						PictureBox l = (PictureBox)cc;
						if (pos > control.Route.Entrys.Count)
						{
							l.Visible = false;
							continue;
						}
                    
						if ( (pos+(int)control.Route.Offset >= 0) && (pos+(int)control.Route.Offset < (int)control.Route.Entrys.Count)  && ((Entry)control.Route.Entrys[pos+(int)control.Route.Offset]).isJump)
						{
							l.Visible = true;
						}
						else
						{
							l.Visible = false;
						}
					}
                    if (cc.Name.StartsWith("L_Speed_"))
                    {
                        // SPEED
                        int pos = System.Convert.ToInt32(cc.Name.Substring(8));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
                        if (control.gnt)
                        {
                            l.Text = ((Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset]).m_gnt_speed;
                        }
                        else
                        {
                            l.Text = ((Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset]).m_speed;
                        }
                    }
                    if (cc.Name.StartsWith("L_Ops_"))
                    {
                        // OPS           
                        int pos = System.Convert.ToInt32(cc.Name.Substring(6));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
						l.Location = new Point(260, l.Location.Y);

                        Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
                        if (e.m_type == EntryType.RADIO_MARKER || e.m_type == EntryType.RADIO_MARKER_ENDING || e.m_type == EntryType.GNT_BEGINNING || e.m_type == EntryType.GNT_ENDING || e.m_type == EntryType.LZB_BEGINNING || e.m_type == EntryType.LZB || e.m_type == EntryType.LZB_ENDING || e.m_type == EntryType.VERKUERTZT)
                        { 
							if (e.m_type != EntryType.RADIO_MARKER && e.m_type != EntryType.RADIO_MARKER_ENDING && e.m_type != EntryType.VERKUERTZT)
							{
								l.Text = "";
								l.Size = new Size(0, l.Height);
							}
							ht.Add(pos, e.m_type);
                        }
                        else 
                        { 
							l.Size = new Size(184, l.Height);
							if (e == control.Marker)
								l.Font = new Font(l.Font.FontFamily, l.Font.Size, FontStyle.Bold);
							else
								l.Font = new Font(l.Font.FontFamily, l.Font.Size, FontStyle.Regular);
                        }

						l.Text = e.m_ops_name;
						if (l.Text.Length > 0)
						{
							if (l.Text[l.Text.Length-1] == '|')
							{
								l.Text = l.Text.Remove(l.Text.Length-1,1);
							}
							l.Text = RemoveDrehung(l.Text);
						}
                    }
                    if (cc.Name.StartsWith("L_OpsS_"))
                    {
                        // OPS_SPEED
                        int pos = System.Convert.ToInt32(cc.Name.Substring(7));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
                        Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
                        if (e == control.Marker || e.m_type == EntryType.RADIO_MARKER)
                        { 
                            l.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                            //l.Font.Bold = true;
                        }
                        else 
                        { 
                            l.Font = new System.Drawing.Font("Zusi standard", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                            //l.Font.Bold = false; 
                        }
                        l.Text = e.m_ops_speed;
                    }
                    if (cc.Name.StartsWith("L_TOP"))
                    {
                        L_TOP.Text = "";
                        int offset2 = 0;
                        do
                        {
                            if (control.Route.Entrys.Count >=  (13 + (int)control.Route.Offset + offset2))
                            {
                                String s = ((Entry)control.Route.Entrys[12+(int)control.Route.Offset+offset2]).m_ops_name;
                                if (s == "")
                                {
                                    offset2++;
                                }
                                else
                                {
                                    L_TOP.Text = RemoveDrehung(s);
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        while(true);
                    }
                    if (cc.Name.StartsWith("L_DOWN"))
                    {
                        L_DOWN.Text = "";
                        int offset3 = 0;
                        do
                        {
                            if ((int)control.Route.Offset-offset3-1 > 0)
                            {
                                String s = ((Entry)control.Route.Entrys[(int)control.Route.Offset-offset3-1]).m_ops_name;
                                if (s == "" && (int)control.Route.Offset-offset3 > 1)
                                {
                                    offset3++;
                                }
                                else
                                {
                                    L_DOWN.Text = RemoveDrehung(s);
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        while(true);
                    }
                    if (cc.Name.StartsWith("L_A_"))
                    {
                        // ETA
                        int pos = System.Convert.ToInt32(cc.Name.Substring(4));
                        Label l = (Label)cc;
						l.Text = "";
                        if (pos > control.Route.Entrys.Count)
                        {
                            //l.Text = "";
                            continue;
                        }
						try
						{
							Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
							l.Text = e.m_eta;
						}
						catch(Exception){}                        
                    }
                    if (cc.Name.StartsWith("L_AK_"))
                    {
                        // ETA-APPENDIX
                        int pos = System.Convert.ToInt32(cc.Name.Substring(5));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
                        Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
                        if (e.m_eta_app != new char())
                        {
                            l.Text = "."+e.m_eta_app;
                        }
                        else
                        {
                            l.Text = "";
                        }
                    }
                    if (cc.Name.StartsWith("L_D_"))
                    {
                        // ETD
                        int pos = System.Convert.ToInt32(cc.Name.Substring(4));
                        Label l = (Label)cc;
						l.Text = "";
                        if (pos > control.Route.Entrys.Count)
                        {
                            //l.Text = "";
                            continue;
                        }
						try
						{
							Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
							l.Text = e.m_etd;
						}
						catch(Exception){}
                    }
                    if (cc.Name.StartsWith("L_DK_"))
                    {
                        // ETD-APPENDIX
                        int pos = System.Convert.ToInt32(cc.Name.Substring(5));
                        Label l = (Label)cc;
                        if (pos > control.Route.Entrys.Count)
                        {
                            l.Text = "";
                            continue;
                        }
                        Entry e = (Entry)control.Route.Entrys[pos-1+(int)control.Route.Offset];
                        if (e.m_etd_app != new char())
                        {
                            l.Text = "."+e.m_etd_app;
                        }
                        else
                        {
                            l.Text = "";
                        }
                    
                    }
                }

                //LINE
                // 01 <-> 02
                if ( PB_01.Location.X != PB_02.Location.X)
                {
                    // we need a Line 
                    PL_01.Visible = true;
                }
                else
                {
                    PL_01.Visible = false;
                }
                // 02 <-> 03
                if ( PB_02.Location.X != PB_03.Location.X)
                {
                    // we need a Line 
                    PL_02.Visible = true;
                }
                else
                {
                    PL_02.Visible = false;
                }
                // 03 <-> 04
                if ( PB_03.Location.X != PB_04.Location.X)
                {
                    // we need a Line 
                    PL_03.Visible = true;
                }
                else
                {
                    PL_03.Visible = false;
                }
                // 04 <-> 05
                if ( PB_04.Location.X != PB_05.Location.X)
                {
                    // we need a Line 
                    PL_04.Visible = true;
                }
                else
                {
                    PL_04.Visible = false;
                }
                // 05 <-> 06
                if ( PB_05.Location.X != PB_06.Location.X)
                {
                    // we need a Line 
                    PL_05.Visible = true;
                }
                else
                {
                    PL_05.Visible = false;
                }
                // 06 <-> 07
                if ( PB_06.Location.X != PB_07.Location.X)
                {
                    // we need a Line 
                    PL_06.Visible = true;
                }
                else
                {
                    PL_06.Visible = false;
                }
                // 07 <-> 08
                if ( PB_07.Location.X != PB_08.Location.X)
                {
                    // we need a Line 
                    PL_07.Visible = true;
                }
                else
                {
                    PL_07.Visible = false;
                }
                // 08 <-> 09
                if ( PB_08.Location.X != PB_09.Location.X)
                {
                    // we need a Line 
                    PL_08.Visible = true;
                }
                else
                {
                    PL_08.Visible = false;
                }
                // 09 <-> 10
                if ( PB_09.Location.X != PB_10.Location.X)
                {
                    // we need a Line 
                    PL_09.Visible = true;
                }
                else
                {
                    PL_09.Visible = false;
                }
                // 10 <-> 11
                if ( PB_10.Location.X != PB_11.Location.X)
                {
                    // we need a Line 
                    PL_10.Visible = true;
                }
                else
                {
                    PL_10.Visible = false;
                }
                // 11 <-> 12
                if ( PB_11.Location.X != PB_12.Location.X)
                {
                    // we need a Line 
                    PL_11.Visible = true;
                }
                else
                {
                    PL_11.Visible = false;
                }

				int newsize = 1; 
				if (control.Route.one_track) newsize = 0;

				if (control.left)
				{
					DekoL2.Size = new Size(3, DekoL2.Size.Height);
					DekoL2a.Size = new Size(newsize, DekoL2a.Size.Height);
				}
				else
				{
					DekoL2.Size = new Size(newsize, DekoL2.Size.Height);
					DekoL2a.Size = new Size(3, DekoL2a.Size.Height);
				}

				// Streckenskizze verstecken wenn kein plan geladen
				if (control.Route == null || (control.Route != null && control.Route.Entrys.Count < 1))
				{
					DekoL2.Visible = false;
					DekoL2a.Visible = false;
					Down_Radio.Visible = false;
					Down_RW.Visible = false;
					Down_Speed.Visible = false;
					Down_Timer.Visible = false;
					Down_Verspaetung.Visible = false;
				}
				else
				{
					DekoL2.Visible = true;
					DekoL2a.Visible = true;
					Down_Radio.Visible = true;
					Down_RW.Visible = true;
					Down_Speed.Visible = true;
					Down_Timer.Visible = true;
					Down_Verspaetung.Visible = true;
				}

				// alte Geschwindigkeit anzeigen
				if (L_Speed_01.Text == "" && control.Route.Offset > 0 && control.Route.Entrys.Count > 0)
				{
					for (int i = (int)control.Route.Offset; i >= 0; i--)
					{
						Entry e = (Entry)control.Route.Entrys[i];
						string check = "";
						if (control.gnt)
						{
							check = e.m_gnt_speed;
						}
						else
							check = e.m_speed;

						if (check == "") 
						{
							L_Speed_01.Text = "";
							L_Speed_01.BackColor = L_Speed_02.BackColor;
							continue;
						}
						else
						{
							L_Speed_01.Text = check;
							if (control.inverse)
							{
								L_Speed_01.BackColor = Color.SteelBlue;
							}
							else
							{
								L_Speed_01.BackColor = Color.Yellow;
							}
							break;
						}
					}
				}
				else
				{
					if (L_Speed_01.BackColor == Color.SteelBlue || L_Speed_01.BackColor == Color.Yellow)
					{
						L_Speed_01.BackColor = L_Speed_02.BackColor;
					}
				}

				this.ResumeLayout();
                firsttime = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Update des Formulars! ("+e.Message.ToString()+")");
            }
        }

		private void DrawSpecial()
		{
			
		}

        private void B_Zeit_Click(object sender, System.EventArgs e)
        {
            ControlSwitch cw = new ControlSwitch(control);
			cw.Location = new Point(m_conf.Position.X+430, m_conf.Position.Y+130);
            SetButtons(true);
            kh.Form = cw;
			DialogResult result;
            do
			{
				result = cw.ShowDialog();
				UpdateControl();
			}
			while (result == DialogResult.Retry);
            kh.Form = null;
            SetButtons(false);
            if (cw.DialogResult == DialogResult.OK)
            {
                verspaetung = control.verspaetung;
            }
            timer_Position.Enabled = control.timer_on;
            UpdateControl();

            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();            
        }

        private void B_Plan_Click(object sender, System.EventArgs e)
        {
            B_FSD.Visible = true;
            B_Plan.Visible = false;
            this.Controls.Remove(f);
            f.Dispose();
            f = null;

            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void B_GW_Click(object sender, System.EventArgs e)
        {
			if (control.Route.Entrys.Count < 1)
				return;

			control.left = !control.left;
           
			control.LoadTimeTableFromZusi();

			if (control.Route == null)
			{
				// did not work
				control.left = !control.left;
				SetButtons(false);
				MessageBox.Show("Es existiert kein Fahrplan für das Gegengleis!");
				return;
			}

			SetButtons(false);

			UpdateControl();

			if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void B_G_Click(object sender, System.EventArgs e)
        {
            DateTimeField dt = new DateTimeField(control.vtime, ref control);
            SetButtons(true);
            kh.Form = dt;
            dt.ShowDialog();
            kh.Form = null;
            SetButtons(false);
            if (dt.DialogResult == DialogResult.OK && !control.use_zusi_time)
            {
                control.vtime = control.date_buffer;
            }

            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void B_GNT_Click(object sender, System.EventArgs e)
        {
			if (control != null && control.Route != null && control.Route.Entrys != null && control.Route.Entrys.Count < 1)
				return;

			control.gnt = !control.gnt;

			string old_bs = "";
			try
			{
				old_bs = ((Entry)control.Route.Entrys[(int)control.Route.Position]).m_ops_name;
			}
			catch (Exception) {}
           
			control.LoadTimeTableFromZusi();

			if (control.Route == null)
			{
				// did not work
				control.gnt = !control.gnt;
				SetButtons(false);
				MessageBox.Show("Es existiert kein GNT Fahrplan!");
				return;
			}

			if (old_bs != "")
			{
				control.ResetMarker(old_bs);
			}

			
			SetButtons(false);

            UpdateControl();

            if (control.sound) Sound.PlaySound();
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void EbulaControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    if ((int)control.Route.Offset < (control.Route.Entrys.Count-12))
                    {
                        control.Route.Offset++;
                        UpdateControl();
                    }
                    break;
                case Keys.PageDown:
                    if (control.Route.Offset > 0)
                    {
                        control.Route.Offset--;
                        UpdateControl();
                    }
                    break;
            }
        }

        private void B_G_Enter(object sender, System.EventArgs e)
        {
            L_Speed_01.Focus();
        }

        private void B_FSD_Click(object sender, System.EventArgs e)
        {
            f = new FSD(control);
            f.Location = new Point(0,58);
            f.Parent = this;
            f.BackColor = this.BackColor;
            f.ForeColor = this.ForeColor;
            this.Controls.Add(f);
            f.BringToFront();
            B_Plan.Visible = true;
            B_FSD.Visible = false;

            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void B_LaD_Click(object sender, System.EventArgs e)
        {
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        private void B_LaT_Click(object sender, System.EventArgs e)
        {
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

		public void Button_Up_Pressed(object sender, System.EventArgs e)
		{
			if (control.sound) Sound.PlaySound();

			if (control.move_via_time || control.timer_on)
			{
				control.NextPage();
			}
			else
			{
				control.NextEntry(false);
			}
			UpdateControl();
			if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        public void Button_Down_Pressed(object sender, System.EventArgs e)
        {
			if (control.sound) Sound.PlaySound();

			if (control.move_via_time || control.timer_on)
			{
				control.PrevPage();
			}
			else
			{
				control.PrevEntry();
			}
			
			UpdateControl();
			if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();        
        }

        public void Button_Right_Pressed(object sender, System.EventArgs e)
        {
                    
        }

        public void Button_Left_Pressed(object sender, System.EventArgs e)
        {
        }

        public void Button_E_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            if (kh.Form == null)
            {
                if (control.Route != null && (control.Marker == null || control.Route.Position == -1) && control.Route.Entrys.Count > 0)
                {
                    control.Marker = (Entry)control.Route.Entrys[0];
                    control.Route.Position = 0;
                    UpdateControl();
                }
                if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
            }
            else // kh.Form != null
            {
                kh.Button_E_Clicked(sender,e);
            }
        }

        public void Button_C_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlayErrorSound();
            if (kh.Form == null)
            {
                control.Marker = null;
                control.Route.Position = -1;
                UpdateControl();
                if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
            }
            else // kh.Form != null
            {
                kh.Button_C_Clicked(sender,e);
            }
        }

        public void Button_Inverse_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            Inverse();
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();
        }

        public void Button_0_Pressed(object sender, System.EventArgs e)
        {            
            if (control.sound) Sound.PlaySound();
            if (!control.keysarenumbers && B_G.Visible && B_G.Text == "G") B_G_Click(sender, e);
        }
        
        public void Button_1_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            if (!control.keysarenumbers && B_Zug.Visible && B_Zug.Text == "Zug") B_Zug_Click(sender, e);
        }
        
        public void Button_2_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            if (!control.keysarenumbers && B_FSD.Visible && B_FSD.Text == "FSD") B_FSD_Click(sender, e);
        }
        
        public void Button_3_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();

        }
        
        public void Button_4_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();

        }
        
        public void Button_5_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();

        }
        
        public void Button_6_Pressed(object sender, System.EventArgs e)
        {
			if (!B_GW.Enabled) return;
            if (control.sound) Sound.PlaySound();
			if (!control.keysarenumbers && B_GW.Visible && (B_GW.Text == "GW" || B_GW.Text == "<GW>")) B_GW_Click(sender ,e);
        }
        
        public void Button_7_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            if (!control.keysarenumbers && B_Zeit.Visible && B_Zeit.Text == "Zeit") B_Zeit_Click(sender, e);
        }
        
        public void Button_8_Pressed(object sender, System.EventArgs e)
        {
            if (control.sound) Sound.PlaySound();
            if (!control.keysarenumbers && B_Plan.Visible && B_Plan.Text == "Plan") B_Plan_Click(sender, e);
        }
        
        public void Button_9_Pressed(object sender, System.EventArgs e)
        {
			if (!B_GNT.Enabled) return;
            if (control.sound) Sound.PlaySound();
			if (!control.keysarenumbers && B_GNT.Visible && (B_GNT.Text == "GNT" || B_GNT.Text == "<GNT>")) B_GNT_Click(sender ,e);
        }


        public string Version()
        {
            return Application.ProductVersion;
        }

        public void SetQu(int position)
        {
            if (position > 342 || position < 78)
            {
                Qu.Visible = false;
                return;
            }
            Qu.Visible = true;
            Qu.Location = new Point(Qu.Location.X, position);
        }

        public void QuDown()
        {
            if (Qu.Location.Y - 24 < 80)
            {
                Qu.Visible = false;
            }
            else
            {
                Qu.Visible = true;
                Qu.Location = new Point (Qu.Location.X,  Qu.Location.Y - 24);
            }
        }

        public void QuUp()
        {
            if (Qu.Location.Y + 24 > 344)
            {
                Qu.Visible = false;
            }
            else
            {
                Qu.Visible = true;
                Qu.Location = new Point (Qu.Location.X,  Qu.Location.Y + 24);
            }
        }

        private void timer_Position_Tick(object sender, System.EventArgs e)
        {
            timer_Position.Enabled = false;
            /* OFFLINE
            control.MoveViaTime(vtime, verspaetung);

            while(control.Route.Position - (long)control.Route.Offset > 12 )
            {
                control.NextPage();
                UpdateControl();
            }
            UpdateControl();
            */
        }

        private void InitSpeedBar()
        {
            PB_01.Location = new Point(0, PB_01.Location.Y);
            PB_02.Location = new Point(0, PB_02.Location.Y);
            PB_03.Location = new Point(0, PB_03.Location.Y);
            PB_04.Location = new Point(0, PB_04.Location.Y);
            PB_05.Location = new Point(0, PB_05.Location.Y);
            PB_06.Location = new Point(0, PB_06.Location.Y);
            PB_07.Location = new Point(0, PB_07.Location.Y);
            PB_08.Location = new Point(0, PB_08.Location.Y);
            PB_09.Location = new Point(0, PB_09.Location.Y);
            PB_10.Location = new Point(0, PB_10.Location.Y);
            PB_11.Location = new Point(0, PB_11.Location.Y);
            PB_12.Location = new Point(0, PB_12.Location.Y);
            PB_13.Location = new Point(0, PB_13.Location.Y);
        }

        private void SetButtons(bool numbers)
        {
			if (!control.XMLConf.UseDB)
			{
				B_GW.Enabled = false;
				//B_GNT.Enabled = false;
			}
			if (control.Route != null && control.Route.one_track) B_GW.Enabled = false;

			control.keysarenumbers = numbers;
            if (numbers)
            {
				fsdwason = B_FSD.Visible;
                B_Zug.Text = "1";
                B_FSD.Text = "2";
                B_LaD.Text = "3";
                B_LaT.Text = "4";
                B_LW.Text = "5";
                B_GW.Text = "6";
                B_Zeit.Text = "7";
                B_Plan.Text = "8";
                B_GNT.Text = "9";
                B_G.Text = "0";

                B_Zug.Visible = true;
                B_FSD.Visible = true;
                B_LaD.Visible = true;
                B_LaT.Visible = true;
                B_LW.Visible = true;
                B_GW.Visible = true;
                B_Zeit.Visible = true;
                B_Plan.Visible = true;
                B_GNT.Visible = true;
                B_G.Visible = true;
            }
            else
            {
                B_Zug.Text = "Zug";
                B_FSD.Text = "FSD";
                B_LaD.Text = "LaD";
                B_LaT.Text = "LaT";
                B_LW.Text = "LW";
				B_GW.Text = "GW";
                B_Zeit.Text = "Zeit";
                B_Plan.Text = "Plan";
				B_GNT.Text = "GNT";
                B_G.Text = "G";

                B_Zug.Visible = true;
                B_FSD.Visible = fsdwason;
                B_LaD.Visible = false;
                B_LaT.Visible = false;
                B_LW.Visible = false;
                B_GW.Visible = B_GW.Enabled;
                B_Zeit.Visible = true;
                B_Plan.Visible = !fsdwason;
                B_GNT.Visible = B_GNT.Enabled;
                B_G.Visible = true;
            }

			

        }

        private void timer_SyncUhr_Tick(object sender, System.EventArgs e)
        {
            RegistryKey rk = null;
			try
			{
				if (control.timer_disabled && !control.move_via_time)
				{
					// Timer deaktiviert
					if (L_TOP.Text == "Kein Zusi gefunden! stand-alone!")
						L_TOP.Text = "";
					//timer_Sync.Enabled = false;
					return;
				}
			}
			catch(Exception)
			{
				//timer_Sync.Enabled = false;
				if (L_TOP.Text == "Kein Zusi gefunden! stand-alone!")
					L_TOP.Text = "";
				System.Windows.Forms.MessageBox.Show("Control nicht da!");
				return;
			}

			try
            {
				rk = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Zusi");
                if (rk.OpenSubKey("Zusi").GetValue("ZugSteht").ToString() == "1" && timer_Sync.Interval == 1)
                {
                    // Zug steht => kein Aktualisierung
                    timer_Sync.Interval = 10000;
                    return;
                }
            }
            catch (Exception)
            {
				if (!control.move_via_time)
				{
					// Zusi nicht lokal vorhanden
					timer_Sync.Interval = 1000;
					if (control != null)
					{
						control.use_zusi_time = false;
						control.timer_on = false;
						control.timer_disabled = true;
					}
					L_TOP.Text = "Kein Zusi gefunden! stand-alone!";
					return;
				}
            }


            timer_Sync.Interval = 1000;

			if (control.use_zusi_time)
			{
				// UHR
				double time = 0d;
				if (rk != null) 
				{
					try
					{
						if (rk.OpenSubKey("Zusi").GetValue("ZugSteht").ToString() == "0" && !control.use_network)
						{
							time = Convert.ToDouble(rk.OpenSubKey("Zusi").GetValue("SimZeit").ToString());
							control.vtime = control.ConvertToDateTime(time);
							control.vtime = control.vtime.AddSeconds(2);
						}
					}
					catch (Exception exc)
					{
						timer_Sync.Enabled = false;
						System.Windows.Forms.MessageBox.Show("Zusi SimZeit Konvertierung fehlgeschlagen! ("+exc.Message.ToString()+")");
					}
				}
				else
				{
					timer_Sync.Enabled = false;
					System.Windows.Forms.MessageBox.Show("Zusi nicht gefunden!");
					return;
				}
                
			}

			if (control.move_via_time)
			{
				control.MoveViaTime(control.vtime, control.verspaetung);
				UpdateControl();
			}

			else if (control.timer_on)
			{
				if (control.Route.Position == -1)
				{
					return;
				}
				// BLOCK
				//string block = rk.OpenSubKey("Zusi").GetValue("Block").ToString();
				string km = "";
				try
				{
					km = rk.OpenSubKey("Zusi").GetValue("km").ToString();
				}
				catch (Exception exc)
				{
					timer_Sync.Enabled = false;
					System.Windows.Forms.MessageBox.Show("Zusi km Konvertierung fehlgeschlagen! ("+exc.Message.ToString()+")");
				}
				km = km.Substring(0,4);
				double d_km = 0d;
				if (km != "")
				{
					d_km = Convert.ToDouble(km);
				}
                
                
				for(int i = (int)control.Route.Position; i < control.Route.Entrys.Count; i++)
				{
					if (((Entry)control.Route.Entrys[i]).m_position == "")
					{
						continue;
					}

					double pos = 0;

					try { pos = Convert.ToDouble(((Entry)control.Route.Entrys[i]).m_position); }
					catch {}
                    
					double next_pos = double.MaxValue;
                    
					int k = 1;

					while ((i+k) < control.Route.Entrys.Count)
					{
						if ( ((Entry)control.Route.Entrys[i+k]).m_position == "") 
						{
							k++;
							continue;
						}
						try {next_pos = Convert.ToDouble(((Entry)control.Route.Entrys[i+k]).m_position);}
						catch{}
						break;
					}

					bool aufsteigend = true;
					if (pos > next_pos) aufsteigend = false;

					//MessageBox.Show("POS: "+pos.ToString()+"  NEXTPOS: "+next_pos.ToString());
                    
					if ( (pos <= d_km && next_pos > d_km) || (pos >= d_km && next_pos < d_km) || (pos >= d_km && control.Route.Position == 0 && aufsteigend) || (pos <= d_km && control.Route.Position == 0 && !aufsteigend) )
					{
						control.Route.Position = i;
						control.Marker = (Entry)control.Route.Entrys[i];
						// now 11

						int counter = 0;

						while ((control.Route.Position - (int)control.Route.Offset - 10) > 0)
						{
							if (counter > 10)
							{
								//MessageBox.Show("Next Page lief mehr als 10 mal?");
								break;
							}
							control.NextPage();
							counter++;
						}
						UpdateControl();
						break;
					}
				}
			}
            if (rk != null) rk.Close();
        }

        private void B_LW_Click(object sender, System.EventArgs e)
        {
            if (control.XMLConf.FocusToZusi) control.SetFocusToZusi();        
        }

		private string RemoveDrehung(string text)
		{
			int ind = text.IndexOf("##DREHUNG##");
			if (ind >= 0)
			{
				return text.Remove(ind, text.Length - ind);
			}
			else
			{
				return text;
			}
		}

        public void Inverse()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EbulaControl));
            // TODO
            if (this.BackColor == System.Drawing.Color.Black)
            {
                // switch to daylight
                control.inverse = false;

				this.BackColor = System.Drawing.Color.WhiteSmoke;
				
                PanelButton.BackColor = System.Drawing.Color.LightGray;
                PanelDown.BackColor = System.Drawing.Color.OldLace;

                if (f != null)
                {
                    f.BackColor = this.BackColor;
                    f.ForeColor = this.ForeColor;
                    f.SetColor();
                    f.BringToFront();
                }
				
				BLACK = Color.Black;

                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    c.ForeColor = System.Drawing.Color.Black;
					if (c.Name.StartsWith("DekoL"))
					{
						((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("DekoL3.Image")));
						continue;
					}
					else if (c.Name.StartsWith("PB"))
					{
						((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("PB_11.Image")));
						continue;
					}
					else if (c.Name.StartsWith("Deko") || c.Name.StartsWith("PL") || c.Name.StartsWith("J"))
					{
						((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("Deko3.Image")));
					}
					else if (c.Name.StartsWith("Qu"))
					{
						((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("Qu.Image")));
					}
                }
            }
            else
            {
                // switch to nightlight

                control.inverse = true;

				this.BackColor = System.Drawing.Color.Black;
                PanelButton.BackColor = System.Drawing.Color.LightSlateGray;
                PanelDown.BackColor = System.Drawing.Color.SteelBlue;

                if (f != null)
                {
                    f.BackColor = this.BackColor;
                    f.ForeColor = this.ForeColor;
                    f.SetColor();
                    f.BringToFront();
                }

				BLACK = Color.WhiteSmoke;

                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    c.ForeColor = System.Drawing.Color.WhiteSmoke;
                    if (c.Name.StartsWith("DekoL")  || c.Name.StartsWith("PB"))
                    {
                        ((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("pB_White_Down.Image")));
                        continue;
                    }
                    if (c.Name.StartsWith("Deko") || c.Name.StartsWith("PL") || c.Name.StartsWith("J"))
                    {
                        ((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("pB_White_Up.Image")));
                    }
                    if (c.Name.StartsWith("Qu"))
                    {
                        ((PictureBox)c).Image = ((System.Drawing.Image)(resources.GetObject("QuW.Image")));
                    }
                }
			}
			if (L_Speed_01.BackColor != L_Speed_03.BackColor)
			{
				if (control.inverse)
					L_Speed_01.BackColor = Color.SteelBlue;
				else
					L_Speed_01.BackColor = Color.Yellow;
						
			}
		}

		private void EbulaControl_Paint(object sender, System.Windows.Forms.PaintEventArgs ea)
		{
			if (m_backBuffer == null)
			{
				m_backBuffer= new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			}

			if (USE_DOUBLE_BUFFER)
				g = Graphics.FromImage(m_backBuffer);
			else
				g = ea.Graphics;
			
			g.Clear(this.BackColor);

			//Paint your graphis on g here

			Pen p = new Pen(BLACK, 2);

			Font f = new Font("Arial", 4, GraphicsUnit.Millimeter);

			foreach(DictionaryEntry e in ht)
			{
				EntryType type = (EntryType)e.Value;
				int pos = (int)e.Key;
				string s = "";

				switch(type)
				{
					case EntryType.GNT_BEGINNING:
						s = "GNT";
						break;
					case EntryType.GNT_ENDING:
						s = "GNT";
						break;
					case EntryType.LZB_BEGINNING:
						s = "LZB";
						break;
					case EntryType.LZB_ENDING:
						s = "LZB";
						break;
					case EntryType.RADIO_MARKER:
						s = "";
						break;
					case EntryType.RADIO_MARKER_ENDING:
						s = "";
						break;
					case EntryType.VERKUERTZT:
						s = "";
						break;
				}

				if (s != "")
				{
					Font f2 = new Font("Arial", 3, GraphicsUnit.Millimeter);
					g.DrawRectangle(p, 230, 80+(12-pos)*24, 28, 18);
					g.DrawString(s, f2, new SolidBrush(BLACK), 232, 82+(12-pos)*24); 

					if (type == EntryType.GNT_ENDING || type == EntryType.LZB_ENDING)
					{
						g.DrawLine(p, 230, 80+(12-pos)*24+18, 230+28, 80+(12-pos)*24);
					}
				}
				if (type == EntryType.VERKUERTZT)
				{
					Font f3 = new Font("Zusi standard", 14F, System.Drawing.GraphicsUnit.Point);
					g.DrawString("º", f3, new SolidBrush(BLACK), 230, 78+(12-pos)*24); 
				}
				else if (type == EntryType.RADIO_MARKER || type == EntryType.RADIO_MARKER_ENDING)
				{
					g.FillEllipse(new SolidBrush(BLACK), 232-2, 80+(12-pos)*24-3, 7, 9);
					g.FillEllipse(new SolidBrush(BLACK), 232+5, 80+(12-pos)*24+6, 9, 7);
					g.DrawLine(new Pen(BLACK, 4), 232-1, 80+(12-pos)*24, 232+6, 80+(12-pos)*24-3+6+7);
					if (type == EntryType.RADIO_MARKER_ENDING)
					{
						g.DrawLine(p, 232-2, 80+(12-pos)*24+6+7, 232+5+7, 80+(12-pos)*24-1);
					}
				}

			}

			if (control.Route.Entrys.Count > 0)
			{
				//Zacken

				Pen pen = new Pen(BLACK, 2);
				int maxpos = 12;
				if (control.Route.Entrys.Count < 12) maxpos = control.Route.Entrys.Count;

				for (int i = (int)control.Route.Offset; i < (int)control.Route.Offset+maxpos; i++)
				{
					int s = ((Entry)control.Route.Entrys[i]).zack;
					if ( s < 1) continue;
					int width = 6;
					int x = 451+5;
					int i_pos = i - (int)control.Route.Offset;
					int y = 80+(12-i_pos)*24 - 28;
					Point[] pp = {new Point(x+width, y), new Point(x, y+6), new Point(x+width, y+12), new Point(x, y+18), new Point(x+width, y+24)};
					g.DrawLines(pen, pp);

					if ( s < 2) continue;
					x = x-4;
					Point[] pp2 = {new Point(x+width, y), new Point(x, y+6), new Point(x+width, y+12), new Point(x, y+18), new Point(x+width, y+24)};
					g.DrawLines(pen, pp2);

				}
			}

			l_ESF_Pos.Text = "+ " + Math.Round(m_conf.Energie/1000f,0).ToString() + " kWh";


			g.SmoothingMode = SMOOTHING_MODE;
			g.TextRenderingHint = TEXT_MODE;

			if (USE_DOUBLE_BUFFER)
			{
				//Copy the back buffer to the screen
				this.CreateGraphics().DrawImageUnscaled(m_backBuffer,0,0);
			}
		}

		private void timer_refresh_Tick(object sender, System.EventArgs e)
		{
			if (!SomethingChanged) return;

			SomethingChanged = false;

			this.Refresh();
		}
		private void LoadESF()
		{
			m_conf.FastReadFile();
			SomethingChanged = true;
		}
    }
}
