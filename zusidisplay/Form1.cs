using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Reflection;

// use my ebula control
using MMI.EBuLa;
using MMI.EBuLa.Tools;
using MMI.MMIBR185;
using System.Threading;


namespace MMI
{
	public class MainForm : System.Windows.Forms.Form
	{

        private string conffile = System.IO.Path.GetDirectoryName(Application.ExecutablePath)+"\\configuration.xml";

		const string EBuLaFile		= "ebula.dll";
		const string EBuLaToolsFile = "ebulatools.dll";
		const string MMIFile		= "br185mmi.dll";
		const string DAVIDFile		= "david.dll";
		const string ICE3File		= "ice3.dll";
		const string ET42XFile		= "et42x.dll";
		const string DIAGNOSEFile	= "diagnose.dll";
		const string VT612File		= "vt612.dll";

        // my own members
        private MMI.EBuLa.ControlContainer m_widget = null; // this is EBuLa
        private EBuLaTools m_tools = null;
        private MMIBR185.ControlContainer m_br185 = null;
		private MMI.ET42X.ControlContainer m_et42x = null;
		private MMI.DIAGNOSE.ControlContainer m_diagnose = null;
		private MMI.VT612.ControlContainer m_vt612 = null;
		private MMI.DAVID.ControlContainer m_david = null;
		private MMI.ICE3.ControlContainer m_ice3 = null;
        private SelectionMenu m_menu = null;
        private Handler m_handler = null;
		public MMI.EBuLa.Network net = null;

        private XMLLoader XmlConf = null;

        private System.Windows.Forms.Timer timer_control;
        private System.ComponentModel.IContainer components;

		public MainForm()
		{
			InitializeComponent();

            LoadConf();

            Init();

			this.Focus();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer_control = new System.Windows.Forms.Timer(this.components);
			// 
			// timer_control
			// 
			this.timer_control.Enabled = true;
			this.timer_control.Tick += new System.EventHandler(this.timer_control_Tick);
			// 
			// MainForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "ZusiDisplay";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.Resize += new System.EventHandler(this.MainForm_Resize);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			//Application.EnableVisualStyles();
			Application.Run(new MainForm());
        }

        private void timer_control_Tick(object sender, System.EventArgs e)
        {
			GC.Collect();
            try
            {
                if (m_widget != null && m_widget.IsDisposed)
                {
                    // show main selection
					m_widget.Terminate();
                    m_widget = null;
                    m_handler.Load = Loadwhat.Menu;
                    m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
                    this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
                }
                if (m_br185 != null && m_br185.IsDisposed)
                {
                    // show main selection
					//this.Controls.Remove(m_br185);
					m_br185.Terminate();
                    m_br185 = null;
                    m_handler.Load = Loadwhat.Menu;
                    m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
                    this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
                }   
				if (m_david != null && m_david.IsDisposed)
				{
					// show main selection
					//this.Controls.Remove(m_br185);
					m_david.Terminate();
					m_david = null;
					m_handler.Load = Loadwhat.Menu;
					m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
					this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
				}
				if (m_ice3 != null && m_ice3.IsDisposed)
				{
					// show main selection
					//this.Controls.Remove(m_br185);
					m_ice3.Terminate();
					m_ice3 = null;
					m_handler.Load = Loadwhat.Menu;
					m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
					this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
				}
				if (m_et42x != null && m_et42x.IsDisposed)
				{
					// show main selection
					m_et42x.Terminate();
					m_et42x = null;
					m_handler.Load = Loadwhat.Menu;
					m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
					this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
				}
				if (m_vt612 != null && m_vt612.IsDisposed)
				{
					// show main selection
					m_vt612.Terminate();
					m_vt612 = null;
					m_handler.Load = Loadwhat.Menu;
					m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
					this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
				}
				if (m_diagnose != null && m_diagnose.IsDisposed)
				{
					// show main selection
					m_diagnose.Terminate();
					m_diagnose = null;
					m_handler.Load = Loadwhat.Menu;
					m_menu = new SelectionMenu(m_handler, XmlConf.TopMost);
					this.Controls.Add(m_menu);
					this.Text = "ZusiDisplay";
				}
				if (m_tools != null && m_tools.IsDisposed)
                {
                    // show main selection

					m_tools = null;

                    LoadConf();
                    Init();

					this.Text = "ZusiDisplay";

                    /*
                    m_handler.Load = Loadwhat.Menu;
                    m_menu = new SelectionMenu(m_handler);
                    this.Controls.Add(m_menu);*/
                }
                if (m_menu != null && m_menu.IsDisposed)
                {
                    m_menu = null;
                    switch (m_handler.Load)
                    {
                        case Loadwhat.EBuLa:
                            m_widget = new EBuLa.ControlContainer(XmlConf);
                            this.Controls.Add(m_widget);
                            m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> EBuLa";
                            break;
                        case Loadwhat.EBuLaTools:
                            m_tools = new EBuLaTools(ref XmlConf);
                            this.Controls.Add(m_tools);
                            m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> Einstellungen";
                            break;
                        case Loadwhat.MMI:
							//MMI.BR185_MMI.Network net = new MMI.BR185_MMI.Network();
                            m_br185 = new MMIBR185.ControlContainer(XmlConf);
                            this.Controls.Add(m_br185);
                            m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> MMI";
                            break;
						case Loadwhat.DAVID:
							m_david = new MMI.DAVID.ControlContainer(XmlConf, 1);
							m_david.m_widget.localstate.Type = MMI.DAVID.TYPE.David1;
							m_david.m_widget.olddisplay = DAVID.CURRENT_DISPLAY.D2_HL;
							m_david.m_widget.Button_0_Pressed(this, e);
							m_david.SwitchBackground();
							this.Controls.Add(m_david);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> DAVID(links)";
							break;
						case Loadwhat.DAVID2:
							m_david = new MMI.DAVID.ControlContainer(XmlConf, 2);
							m_david.m_widget.localstate.Type = MMI.DAVID.TYPE.David2;
							m_david.m_widget.something_changed = true;
							this.Controls.Add(m_david);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> DAVID(rechts)";
							break;
						case Loadwhat.ICE3_1:
							m_ice3 = new MMI.ICE3.ControlContainer(XmlConf, 1);
							m_ice3.m_widget.localstate.Type = MMI.ICE3.TYPE.David1;
							m_ice3.m_widget.olddisplay = ICE3.CURRENT_DISPLAY.D2_Zustand;
							m_ice3.m_widget.localstate.DISPLAY = ICE3.CURRENT_DISPLAY.D1_Grundb;
							m_ice3.m_widget.something_changed = true;
							this.Controls.Add(m_ice3);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> ICE3/T(D)(links)";
							break;
						case Loadwhat.ICE3_2:
							m_ice3 = new MMI.ICE3.ControlContainer(XmlConf, 2);
							m_ice3.m_widget.olddisplay = ICE3.CURRENT_DISPLAY.D1_Grundb;
							m_ice3.m_widget.localstate.Type = MMI.ICE3.TYPE.David2;
							m_ice3.m_widget.localstate.DISPLAY = ICE3.CURRENT_DISPLAY.D2_Zustand;
							m_ice3.m_widget.something_changed = true;
							this.Controls.Add(m_ice3);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> ICE3/T(D)(rechts)";
							break;
						case Loadwhat.ET42X:
							m_et42x = new MMI.ET42X.ControlContainer(XmlConf);
							this.Controls.Add(m_et42x);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> ET42x";
							break;
						case Loadwhat.VT612:
							m_vt612 = new MMI.VT612.ControlContainer(XmlConf);
							this.Controls.Add(m_vt612);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> VT611/612";
							break;
						case Loadwhat.DIAGNOSE:
							m_diagnose = new MMI.DIAGNOSE.ControlContainer(XmlConf);
							this.Controls.Add(m_diagnose);
							m_handler.Load = Loadwhat.Menu;
							this.Text = "ZusiDisplay -> DIAGNOSE";
							break;
					}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Wechseln des Menüs! ("+ex.Message.ToString()+")");
                timer_control.Enabled = false;
            }
        }

        public void LoadConf()
        {
            try
            {
                XmlConf = new XMLLoader(conffile);
                /*if (XmlConf.Version < 13)
                {
                    MessageBox.Show("Ihre configuration.xml ist veraltet (Version: "+(XmlConf.Version/10d).ToString()+"). Bitte auf neue Version umstellen!");
                }*/
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Lesen der Konfigurationsdatei! ("+e.Message.ToString()+")");
            }
        }

        public void Init()
        {
            try
            {
                // initialize controls
                m_handler = new Handler();
                m_widget = null;//new ControlContainer(m_inverse);
                m_tools = null;//new EBuLaTools();
                m_menu   = new SelectionMenu(m_handler, XmlConf.TopMost);
                //m_mesa = new MesaController(); 

                // show menu
                this.Controls.Add(m_menu);

                this.Location = new Point(XmlConf.Width,XmlConf.Height);
                if (XmlConf.Border) 
                {
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                }

                this.TopMost = XmlConf.TopMost;


				// Check Modules
				if (!System.IO.File.Exists(@".\"+EBuLaFile))		m_handler.ebula = false;
				if (!System.IO.File.Exists(@".\"+EBuLaToolsFile))	m_handler.ebulatools = false;
				if (!System.IO.File.Exists(@".\"+MMIFile))			m_handler.mmi = false;
				if (!System.IO.File.Exists(@".\"+ICE3File))			m_handler.ice3 = false;
				if (!System.IO.File.Exists(@".\"+DAVIDFile))		m_handler.david = false;
				if (!System.IO.File.Exists(@".\"+DIAGNOSEFile))		m_handler.diagnose = false;
				if (!System.IO.File.Exists(@".\"+ET42XFile))		m_handler.et42x = false;
				if (!System.IO.File.Exists(@".\"+VT612File))		m_handler.vt612 = false;
				
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Erstellen des Fensters! ("+e.Message.ToString()+")");
            }
        }

		private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		}

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
			foreach(UserControl c in this.Controls)
			{
				if (c == null) continue;

				c.Size = new Size(this.Width, this.Height);
			}
		}

    }
}
