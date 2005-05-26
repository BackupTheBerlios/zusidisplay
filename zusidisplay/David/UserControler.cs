using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using MMI.EBuLa.Tools;

namespace MMI.DAVID
{
	public class ControlContainer : System.Windows.Forms.UserControl
	{
		const string BACKGROUND_IMAGE_ADTRANZ = @".\Pictures\adtranz_ice.jpg";
		const string BACKGROUND_IMAGE_ABB     = @".\Pictures\abb.jpg";

        public DavidControl m_widget = null;
		public MMI.EBuLa.EbulaControl m_widget_ebula = null;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.PictureBox pB_EBuLa;
        private System.Windows.Forms.Panel P_Display;
        private System.Windows.Forms.Timer timer_keys;
        
        private SystemTools.System sys = new SystemTools.System();

        private MMI.EBuLa.Tools.XMLLoader m_XMLConf = null;

		Network net;
		Thread t;

		public ControlContainer(MMI.EBuLa.Tools.XMLLoader XMLConf, int side)
		{
			InitializeComponent();

            m_XMLConf = XMLConf;

            try
            {
                //m_parent = gotParent;
                m_widget = new DavidControl(XMLConf, this);
                P_Display.Controls.Add(m_widget);
                if (m_XMLConf.Inverse) 
                {
                    m_widget.Inverse();
                }
                else 
                {
                    m_widget.Inverse();
                    m_widget.Inverse();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Erstellen des Widgets! ("+e.Message+")");
            }
             			
			try
			{
				SwitchBackground();
			}
			catch(Exception)
			{
				MessageBox.Show("Fehler! Das Hintergrundbild \""+BACKGROUND_IMAGE_ABB+" oder "+BACKGROUND_IMAGE_ADTRANZ+"\" konnte nicht geladen werden!");
			}
			
			if (side == 1)
				net = new Network(ref m_widget, "(links)");
            else if (side == 2)
				net = new Network(ref m_widget, "(rechts)");
			else 
				return;
				

			if (t == null)
			{
				t = new Thread(new ThreadStart(net.Connect));
				t.Priority = m_XMLConf.thread_prio;
				t.IsBackground = true;
				t.Start();
				Thread.Sleep(0);
			}

			if (side == 2)
			{
				m_widget_ebula = new MMI.EBuLa.EbulaControl(m_XMLConf, true);
				m_widget_ebula.Visible = false;
				P_Display.Controls.Add(m_widget_ebula);
				if (m_XMLConf.Inverse) 
				{
					m_widget_ebula.Inverse();
				}
			}
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

		public void Terminate()
		{
			m_widget.Dispose();
			if (m_widget_ebula != null)
			{
				m_widget_ebula.Terminate();
				if (m_widget_ebula.t != null)
					m_widget_ebula.t.Abort();
				m_widget_ebula.Dispose();
			}
			m_widget = null;
			m_widget_ebula = null;
			t.Abort();
			t = null;
			net = null;
			GC.Collect();
		}

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pB_EBuLa = new System.Windows.Forms.PictureBox();
			this.P_Display = new System.Windows.Forms.Panel();
			this.timer_keys = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// pB_EBuLa
			// 
			this.pB_EBuLa.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pB_EBuLa.Location = new System.Drawing.Point(0, 0);
			this.pB_EBuLa.Name = "pB_EBuLa";
			this.pB_EBuLa.Size = new System.Drawing.Size(800, 600);
			this.pB_EBuLa.TabIndex = 0;
			this.pB_EBuLa.TabStop = false;
			this.pB_EBuLa.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pB_EBuLa_MouseDown);
			// 
			// P_Display
			// 
			this.P_Display.Location = new System.Drawing.Point(94, 72);
			this.P_Display.Name = "P_Display";
			this.P_Display.Size = new System.Drawing.Size(630, 460);
			this.P_Display.TabIndex = 8;
			// 
			// timer_keys
			// 
			this.timer_keys.Enabled = true;
			this.timer_keys.Interval = 10;
			this.timer_keys.Tick += new System.EventHandler(this.timer_keys_Tick);
			// 
			// ControlContainer
			// 
			this.Controls.Add(this.P_Display);
			this.Controls.Add(this.pB_EBuLa);
			this.Name = "ControlContainer";
			this.Size = new System.Drawing.Size(800, 600);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ControlContainer_Paint);
			this.ResumeLayout(false);

		}
		#endregion

        public string Version()
        {
            return m_widget.Version();
        }

        private void B_Up_Click(object sender, System.EventArgs e)
        {
            m_widget.Button_Up_Pressed(sender, e);
        }

        private void B_Down_Click(object sender, System.EventArgs e)
        {
            m_widget.Button_Down_Pressed(sender, e);
        }

        private void B_E_Click(object sender, System.EventArgs e)
        {
            m_widget.Button_E_Pressed(sender, e);
        }

        private void B_C_Click(object sender, System.EventArgs e)
        {
            m_widget.Button_C_Pressed(sender, e);
        }

        private void B_Inverse_Click(object sender, System.EventArgs e)
        {
            m_widget.Button_Inverse_Pressed(sender, e);
        }

        private void B_Brightness_Click(object sender, System.EventArgs e)
        {
        
        }

        private void B_Off_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }


        private void timer_keys_Tick(object sender, System.EventArgs e)
        {
			EventArgs ea = new EventArgs();

            if (CompKey(m_XMLConf.Key("key_E"))) B_E_Click(this, ea);

            if (CompKey(m_XMLConf.Key("key_C"))) B_C_Click(this, ea);
            
            //if (CompKey(m_XMLConf.Key("key_Right"))) B_Right_Click(this, ea);
            
            //if (CompKey(m_XMLConf.Key("key_Left"))) B_Left_Click(this, ea);
            
            if (CompKey(m_XMLConf.Key("key_Up"))) B_Up_Click(this, ea);
            
            if (CompKey(m_XMLConf.Key("key_Down"))) B_Down_Click(this, ea);

            if (CompKey(m_XMLConf.Key("key_OnOff"))) B_Off_Click(this, ea);

            if (CompKey(m_XMLConf.Key("key_Invert"))) B_Inverse_Click(this, ea);

            if (CompKey(m_XMLConf.Key("key_Brightness"))) B_Brightness_Click(this, ea);

            if (CompKey(m_XMLConf.Key("key_1"))) m_widget.Button_1_Pressed(sender, e);

            if (CompKey(m_XMLConf.Key("key_2"))) m_widget.Button_2_Pressed(sender, e);

            if (CompKey(m_XMLConf.Key("key_3"))) m_widget.Button_3_Pressed(sender, e);

            if (CompKey(m_XMLConf.Key("key_4"))) m_widget.Button_4_Pressed(sender, e);

            if (CompKey(m_XMLConf.Key("key_5"))) m_widget.Button_5_Pressed(sender, e);

            if (CompKey(m_XMLConf.Key("key_6"))) m_widget.Button_6_Pressed(sender, e);
            
            if (CompKey(m_XMLConf.Key("key_7"))) m_widget.Button_7_Pressed(sender, e);
            
            if (CompKey(m_XMLConf.Key("key_8"))) m_widget.Button_8_Pressed(sender, e);
            
            if (CompKey(m_XMLConf.Key("key_9"))) m_widget.Button_9_Pressed(sender, e);
            
            if (CompKey(m_XMLConf.Key("key_0"))) m_widget.Button_0_Pressed(sender, e);
        }

        private bool CompKey(long KCode)
        {
            long KCode2 = 0;
            /*if (KCode == 105) KCode2 = 33; // NUM 9
            if (KCode == 99) KCode2 = 34; // NUM 3
            if (KCode == 97) KCode2 = 35; // NUM 1            
            if (KCode == 103) KCode2 = 36; // NUM 7
            if (KCode == 100) KCode2 = 37; // NUM 4
            if (KCode == 104) KCode2 = 38; // NUM 8
            if (KCode == 102) KCode2 = 39; // NUM 6
            if (KCode == 98) KCode2 = 40; // NUM 2
            if (KCode == 96) KCode2 = 45; // NUM 0
            if (KCode == 110) KCode2 = 46; // NUM ,*/

            if ( (sys.GetKey(KCode) == -32767) || (sys.GetKey(KCode2) == -32767) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		private void ControlContainer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			m_widget.DavidControl_Paint(sender, e);
		}

		private void pB_EBuLa_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Y > 540 && e.Y < 592)	 // untere buttons
			{
				if (e.X > 86 && e.X < 138) // DS
				{
					if (m_widget.Visible)
						m_widget.Button_1_Pressed(sender, e);
					else
						m_widget_ebula.Button_1_Pressed(sender, e);
				}
				else if (e.X > 150 && e.X < 205) // nix oder W
				{
					if (m_widget.Visible)
						m_widget.Button_2_Pressed(sender, e);
					else
						m_widget_ebula.Button_2_Pressed(sender, e);
				}
				else if (e.X > 216 && e.X < 270) // BRH oder nix
				{
					if (m_widget.Visible)
						m_widget.Button_3_Pressed(sender, e);
					else
						m_widget_ebula.Button_3_Pressed(sender, e);
				}
				else if (e.X > 282 && e.X < 336) // D oder LZB
				{
					if (m_widget.Visible)
						m_widget.Button_4_Pressed(sender, e);
					else
						m_widget_ebula.Button_4_Pressed(sender, e);
				}
				else if (e.X > 348 && e.X < 402) // Br oder UB
				{
					if (m_widget.Visible)
						m_widget.Button_5_Pressed(sender, e);
					else
						m_widget_ebula.Button_5_Pressed(sender, e);
				}
				else if (e.X > 413 && e.X < 467) // Fbr oder A
				{
					if (m_widget.Visible)
						m_widget.Button_6_Pressed(sender, e);
					else
						m_widget_ebula.Button_6_Pressed(sender, e);
				}
				else if (e.X > 480 && e.X < 535) // Fdyn oder F
				{
					if (m_widget.Visible)
						m_widget.Button_7_Pressed(sender, e);
					else
						m_widget_ebula.Button_7_Pressed(sender, e);
				}
				else if (e.X > 544 && e.X < 599) // B oder I
				{
					if (m_widget.Visible)
						m_widget.Button_8_Pressed(sender, e);
					else
						m_widget_ebula.Button_8_Pressed(sender, e);
				}
				else if (e.X > 610 && e.X < 664) // HBL oder U
				{
					if (m_widget.Visible)
						m_widget.Button_9_Pressed(sender, e);
					else
						m_widget_ebula.Button_9_Pressed(sender, e);
				}
				else if (e.X > 676 && e.X < 730) // HL oder G
				{
					if (m_widget.Visible)
						m_widget.Button_0_Pressed(sender, e);
					else
						m_widget_ebula.Button_0_Pressed(sender, e);
				}
			}
			else if (e.Y > 10 && e.Y < 60)	 // obere buttons
			{
				if (e.X > 88 && e.X < 138) // aus
				{
					this.Dispose();
				}
				else if (e.X > 150 && e.X < 205) // nix
				{
					// Umschaltung ICE <-> EBuLa
					SwitchEBuLA_DAVID();
				}
				else if (e.X > 216 && e.X < 270) // i
				{
					// Umschaltung ICE Typ
					m_widget.Button_SW_Pressed(sender, e);

					
				}
				else if (e.X > 282 && e.X < 402) // St
				{
					m_widget.Button_ST_Pressed(sender, e);
				}
				else if (e.X > 413 && e.X < 467) // V>0
				{
					m_widget.Button_V_GR_0_Pressed(sender, e);
				}
				else if (e.X > 480 && e.X < 535) // V=0
				{
					m_widget.Button_V_EQ_0_Pressed(sender, e);
				}
				else if (e.X > 554 && e.X < 599) // Sonne
				{
					
				}
				else if (e.X > 610 && e.X < 664) // Mond
				{
					if (m_widget.Visible)
						m_widget.Button_Inverse_Pressed(sender, e);
					else
						m_widget_ebula.Button_Inverse_Pressed(sender, e);
					
				}
				else if (e.X > 676 && e.X < 730) // UD
				{
					if (m_widget.Visible)
					{
						//SwitchBackground();
						m_widget.Button_UD_Pressed(sender, e);
					}
				}
			}
			else if (e.X > 740 && e.X < 790)
			{
				// seitliche Reihe
				if (e.Y > 180 && e.Y < 228)
				{
					if (m_widget.Visible)
						m_widget.Button_Up_Pressed(sender, e);
					else
						m_widget_ebula.Button_Up_Pressed(sender, e);

				}
				else if (e.Y > 242 && e.Y < 294)
				{
					if (m_widget.Visible)
						m_widget.Button_Down_Pressed(sender, e);
					else
						m_widget_ebula.Button_Down_Pressed(sender, e);

				}
				else if (e.Y > 306 && e.Y < 422)
				{
					if (m_widget.Visible)
						m_widget.Button_E_Pressed(sender, e);
					else
						m_widget_ebula.Button_E_Pressed(sender, e);
				}
			}

		}

		public void SwitchBackground()
		{
			if (!m_widget.IsDavid1)
			{
				if (System.IO.File.Exists(BACKGROUND_IMAGE_ADTRANZ))
				{
					// change to siemens
					pB_EBuLa.Image = new Bitmap(BACKGROUND_IMAGE_ADTRANZ);
				}
			}
			else
			{
				if (System.IO.File.Exists(BACKGROUND_IMAGE_ABB))
				{
					// change to bombardier
					pB_EBuLa.Image = new Bitmap(BACKGROUND_IMAGE_ABB);
				}
			}
		}

		private void SwitchEBuLA_DAVID()
		{ 
			if (m_widget.localstate.Type == TYPE.David2 && !m_widget.ShowKeys)
			{
				if (m_widget.Visible)
				{
					m_widget.Visible = false;
					m_widget_ebula.Visible = true;
				}
				else
				{
					m_widget_ebula.Visible = false;
					m_widget.Visible = true;				
				}
				if (m_XMLConf.FocusToZusi) Stuff.SetFocusToZusi();
			}
		}

		public void SetUhrzeitEbula(DateTime time)
		{
			if (m_widget_ebula != null)
			{
				m_widget_ebula.control.vtime = m_widget.vtime;
				//m_widget_ebula.control.use_zusi_time = false;
				//m_widget_ebula.control.timer_on = false;
				//m_widget_ebula.control.timer_disabled = true;
			}
		}
	}
}
