using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using MMI.EBuLa.Tools;

namespace MMI.ICE3
{
	public class ControlContainer : System.Windows.Forms.UserControl
	{
		const string BACKGROUND_IMAGE = @".\Pictures\ice3.jpg";

        public ICE3Control m_widget = null;
		public MMI.EBuLa.EbulaControl m_widget_ebula = null;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel P_Display;
        private System.Windows.Forms.Timer timer_keys;

		private string pushed_button = "";
                
        private SystemTools.System sys = new SystemTools.System();

        private MMI.EBuLa.Tools.XMLLoader m_XMLConf = null;

		Network net;
		private System.Windows.Forms.PictureBox pB_EBuLa;
		Thread t;

		public ControlContainer(MMI.EBuLa.Tools.XMLLoader XMLConf, int side)
		{
			InitializeComponent();

            m_XMLConf = XMLConf;

            try
            {
                //m_parent = gotParent;
                m_widget = new ICE3Control(XMLConf, this);
                P_Display.Controls.Add(m_widget);
                if (m_XMLConf.Inverse) 
                {
                    m_widget.Inverse = true;
                }
                else 
                {
					m_widget.Inverse = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Erstellen des Widgets! ("+e.Message+")");
            }

			try
			{
				pB_EBuLa.Image = new Bitmap(BACKGROUND_IMAGE);
			}
			catch(Exception)
			{
				MessageBox.Show("Fehler! Das Hintergrundbild \""+BACKGROUND_IMAGE+"\" konnte nicht geladen werden!");
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
				t.IsBackground = true;
				t.Priority = ThreadPriority.Lowest;
				t.Start();
				Thread.Sleep(1);
			}

			/*
			MMI.EBuLa.Network ebula_net = null;
			m_widget_ebula = new MMI.EBuLa.EbulaControl(m_XMLConf, ref ebula_net);
			m_widget_ebula.Visible = false;
			P_Display.Controls.Add(m_widget_ebula);
			/*if (m_XMLConf.Inverse) 
			{
				m_widget_ebula.Inverse();
			}*/

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
			m_widget = null;
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
			this.P_Display = new System.Windows.Forms.Panel();
			this.timer_keys = new System.Windows.Forms.Timer(this.components);
			this.pB_EBuLa = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// P_Display
			// 
			this.P_Display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.P_Display.Location = new System.Drawing.Point(69, 73);
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
			// pB_EBuLa
			// 
			this.pB_EBuLa.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pB_EBuLa.Location = new System.Drawing.Point(0, 0);
			this.pB_EBuLa.Name = "pB_EBuLa";
			this.pB_EBuLa.Size = new System.Drawing.Size(800, 600);
			this.pB_EBuLa.TabIndex = 0;
			this.pB_EBuLa.TabStop = false;
			this.pB_EBuLa.Click += new System.EventHandler(this.pB_EBuLa_Click);
			this.pB_EBuLa.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pB_EBuLa_MouseDown);
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
			
			if (pushed_button == "")
			{			
				if (CompKey(m_XMLConf.Key("key_E"))) pushed_button = "key_E";

				else if (CompKey(m_XMLConf.Key("key_C"))) pushed_button = "key_C";
            
				else if (CompKey(m_XMLConf.Key("key_Right"))) pushed_button = "key_Right";
            
				else if (CompKey(m_XMLConf.Key("key_Left"))) pushed_button = "key_Left";
            
				else if (CompKey(m_XMLConf.Key("key_Up"))) pushed_button = "key_Up";
            
				else if (CompKey(m_XMLConf.Key("key_Down"))) pushed_button = "key_Down";

				else if (CompKey(m_XMLConf.Key("key_OnOff"))) pushed_button = "key_OnOff";

				else if (CompKey(m_XMLConf.Key("key_Invert"))) pushed_button = "key_Invert";

				else if (CompKey(m_XMLConf.Key("key_Brightness"))) pushed_button = "key_Brightness";

				else if (CompKey(m_XMLConf.Key("key_1"))) pushed_button = "key_1";

				else if (CompKey(m_XMLConf.Key("key_2"))) pushed_button = "key_2";

				else if (CompKey(m_XMLConf.Key("key_3"))) pushed_button = "key_3";

				else if (CompKey(m_XMLConf.Key("key_4"))) pushed_button = "key_4";

				else if (CompKey(m_XMLConf.Key("key_5"))) pushed_button = "key_5";

				else if (CompKey(m_XMLConf.Key("key_6"))) pushed_button = "key_6";
            
				else if (CompKey(m_XMLConf.Key("key_7"))) pushed_button = "key_7";
            
				else if (CompKey(m_XMLConf.Key("key_8"))) pushed_button = "key_8";
            
				else if (CompKey(m_XMLConf.Key("key_9"))) pushed_button = "key_9";
            
				else if (CompKey(m_XMLConf.Key("key_0"))) pushed_button = "key_0";
			}
			else
			{
				if (ButtonPressed())
					return;
				
				switch (pushed_button)
				{
					case "key_E":
						m_widget.Button_E_Pressed(this, ea);
						break;
					case "key_C":
						m_widget.Button_C_Pressed(this, ea);
						break;
					case "key_Right":
						//m_widget.Button__Pressed(this, ea);
						break;
					case "key_Left":
						//m_widget.Button_Left_Pressed(this, ea);
						break;
					case "key_Up":
						m_widget.Button_Up_Pressed(this, ea);
						break;
					case "key_Down":
						m_widget.Button_Down_Pressed(this, ea);
						break;
					case "key_OnOff":
						//m_widget.Button_OnOff_Pressed(this, ea);
						break;
					case "key_Invert":
						m_widget.Button_Inverse_Pressed(this, ea);
						break;
					case "key_Brightness":
						//m_widget.Button_E_Pressed(this, ea);
						break;
					case "key_1":
						m_widget.Button_1_Pressed(this, ea);
						break;
					case "key_2":
						m_widget.Button_2_Pressed(this, ea);
						break;
					case "key_3":
						m_widget.Button_3_Pressed(this, ea);
						break;
					case "key_4":
						m_widget.Button_4_Pressed(this, ea);
						break;
					case "key_5":
						m_widget.Button_5_Pressed(this, ea);
						break;
					case "key_6":
						m_widget.Button_6_Pressed(this, ea);
						break;
					case "key_7":
						m_widget.Button_7_Pressed(this, ea);
						break;
					case "key_8":
						m_widget.Button_8_Pressed(this, ea);
						break;
					case "key_9":
						m_widget.Button_9_Pressed(this, ea);
						break;
					case "key_0":
						m_widget.Button_0_Pressed(this, ea);						
						break;
				}
				pushed_button = "";
			}

		}

        /*private void timer_keys_Tick(object sender, System.EventArgs e)
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
        }*/

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

		private bool ButtonPressed()
		{
			return CompKey(m_XMLConf.Key("key_Invert"));/*(CompKey(m_XMLConf.Key("key_E"))) || (CompKey(m_XMLConf.Key("key_C"))) || (CompKey(m_XMLConf.Key("key_Right"))) ||
            
			(CompKey(m_XMLConf.Key("key_Left"))) || (CompKey(m_XMLConf.Key("key_Up"))) || (CompKey(m_XMLConf.Key("key_Down"))) ||
				
			(CompKey(m_XMLConf.Key("key_OnOff"))) || (CompKey(m_XMLConf.Key("key_Invert"))) || (CompKey(m_XMLConf.Key("key_Brightness"))) ||

			(CompKey(m_XMLConf.Key("key_1"))) || (CompKey(m_XMLConf.Key("key_2"))) || (CompKey(m_XMLConf.Key("key_3"))) ||

			(CompKey(m_XMLConf.Key("key_4"))) || (CompKey(m_XMLConf.Key("key_5"))) || (CompKey(m_XMLConf.Key("key_6"))) ||
            
			(CompKey(m_XMLConf.Key("key_7"))) || (CompKey(m_XMLConf.Key("key_8"))) || (CompKey(m_XMLConf.Key("key_9"))) ||
            
			(CompKey(m_XMLConf.Key("key_0")));*/
		}
		private void ControlContainer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			m_widget.ICE3Control_Paint(sender, e);
		}

		private void pB_EBuLa_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Y > 544 && e.Y < 588)	 // untere buttons
			{
				if (e.X > 92 && e.X < 137) // 1
				{
					if (m_widget.Visible)
						m_widget.Button_1_Pressed(sender, e);
					else
						m_widget_ebula.Button_1_Pressed(sender, e);
				}
				else if (e.X > 150 && e.X < 196) // 2
				{
					if (m_widget.Visible)
						m_widget.Button_2_Pressed(sender, e);
					else
						m_widget_ebula.Button_2_Pressed(sender, e);
				}
				else if (e.X > 212 && e.X < 256) // 3
				{
					if (m_widget.Visible)
						m_widget.Button_3_Pressed(sender, e);
					else
						m_widget_ebula.Button_3_Pressed(sender, e);
				}
				else if (e.X > 272 && e.X < 315) // 4
				{
					if (m_widget.Visible)
						m_widget.Button_4_Pressed(sender, e);
					else
						m_widget_ebula.Button_4_Pressed(sender, e);
				}
				else if (e.X > 332 && e.X < 375) // 5
				{
					if (m_widget.Visible)
						m_widget.Button_5_Pressed(sender, e);
					else
						m_widget_ebula.Button_5_Pressed(sender, e);
				}
				else if (e.X > 391 && e.X < 434) // 6
				{
					if (m_widget.Visible)
						m_widget.Button_6_Pressed(sender, e);
					else
						m_widget_ebula.Button_6_Pressed(sender, e);
				}
				else if (e.X > 450 && e.X < 495) // 7
				{
					if (m_widget.Visible)
						m_widget.Button_7_Pressed(sender, e);
					else
						m_widget_ebula.Button_7_Pressed(sender, e);
				}
				else if (e.X > 510 && e.X < 555) // 8
				{
					if (m_widget.Visible)
						m_widget.Button_8_Pressed(sender, e);
					else
						m_widget_ebula.Button_8_Pressed(sender, e);
				}
				else if (e.X > 571 && e.X < 614) // 9
				{
					if (m_widget.Visible)
						m_widget.Button_9_Pressed(sender, e);
					else
						m_widget_ebula.Button_9_Pressed(sender, e);
				}
				else if (e.X > 629 && e.X < 673) // 0
				{
					if (m_widget.Visible)
						m_widget.Button_0_Pressed(sender, e);
					else
						m_widget_ebula.Button_0_Pressed(sender, e);
				}
			}
			else if (e.Y > 13 && e.Y < 55)	 // obere buttons
			{
				if (e.X > 95 && e.X < 140) // aus
				{
					this.Dispose();
				}
				else if (e.X > 155 && e.X < 200) // sonne
				{

				}
				else if (e.X > 214 && e.X < 258) // mond
				{
					// Umschaltung ICE <-> EBuLa
					m_widget.Inverse = !m_widget.Inverse;
					m_widget.something_changed = true;
				}
				else if (e.X > 273 && e.X < 318) // S
				{
					
				}
				else if (e.X > 332 && e.X < 375) // i
				{ 
					// Umschaltung ICE Typ
					m_widget.Button_SW_Pressed(sender, e);					
				}
				else if (e.X > 391 && e.X < 494) // störung
				{
					m_widget.Button_ST_Pressed(sender, e);
				}
				else if (e.X > 510 && e.X < 552) // v>0
				{
					m_widget.Button_V_GR_0_Pressed(sender, e);
				}
				else if (e.X > 568 && e.X < 611) // v=0
				{
					m_widget.Button_V_EQ_0_Pressed(sender, e);
				}
				else if (e.X > 627 && e.X < 670) // UD
				{
					if (m_widget.Visible)
					{
						//SwitchBackground();
						m_widget.Button_UD_Pressed(sender, e);
					}
				}
			}
			else if (e.X > 718 && e.X < 764)
			{
				// seitliche Reihe
				if (e.Y > 283 && e.Y < 331)
				{
					if (m_widget.Visible)
						m_widget.Button_Up_Pressed(sender, e);
					else
						m_widget_ebula.Button_Up_Pressed(sender, e);

				}
				else if (e.Y > 331 && e.Y < 383)
				{
					if (m_widget.Visible)
						m_widget.Button_Down_Pressed(sender, e);
					else
						m_widget_ebula.Button_Down_Pressed(sender, e);

				}
				else if (e.Y > 397 && e.Y < 440)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ControlContainer));

			if (!m_widget.IsDavid1)
			{
				this.pB_EBuLa.Image = null;
				this.pB_EBuLa.Image = ((System.Drawing.Image)(resources.GetObject("pB_EBuLa.Image")));
			}
			else
			{
				this.pB_EBuLa.Image = null;										//
				this.pB_EBuLa.Image = ((System.Drawing.Image)(resources.GetObject("pB_David1.Image")));
			}
		}

		private void SwitchEBuLA_DAVID()
		{ 
			return;
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

		private void pB_EBuLa_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
