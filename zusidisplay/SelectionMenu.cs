using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MMI
{
	public class SelectionMenu : System.Windows.Forms.UserControl
	{
        private Handler m_handler = null;
        private System.Windows.Forms.Button B_Quit;
        private System.Windows.Forms.Button B_EBuLaTools;
        private System.Windows.Forms.Label L_version;
		private System.Windows.Forms.Button b_Info;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label l_brand;
		private System.Windows.Forms.Label l_brand_version;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button B_EBuLa;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button B_David2;
		private System.Windows.Forms.Button B_David1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button B_Diagnose;
		private System.Windows.Forms.Button B_Mesa;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button B_VT612;
		private System.Windows.Forms.Button B_ET42X;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button B_ICE3_2;
		private System.Windows.Forms.Button B_ICE3_1;

		private bool m_topmost = false;

		public SelectionMenu(Handler h, bool topmost)
		{
			InitializeComponent();

            if (h == null)
            {
                MessageBox.Show("Fehler beim Erstellen des Handlers!");
                h = new Handler();
            }

			m_topmost = topmost;

			
			panel1.BackColor = Color.FromArgb(61,92,122);
			panel2.BackColor = Color.FromArgb(117,144,174);

            m_handler = h;
            L_version.Text = "Version: "+Application.ProductVersion;
			l_brand_version.Text = " " + GetVersion(Application.ProductVersion);
            //l_EBuLa_version.Text = "EBuLa Version: "+EBuLa.ControlContainer.Version;
			//l_Tools_Version.Text = "Tools Version: "+MMI.EBuLa.Tools.XMLLoader.ToolsVersion;

			if (!m_handler.ebula) B_EBuLa.Enabled = false;
			//if (!m_handler.ebulatools) B_EBuLa.Enabled = false;
			if (!m_handler.diagnose) B_Diagnose.Enabled = false;
			if (!m_handler.david)
			{
				B_David1.Enabled = false;
				B_David2.Enabled = false;
			}
			if (!m_handler.ice3)
			{
				B_ICE3_1.Enabled = false;
				B_ICE3_2.Enabled = false;
			}
			if (!m_handler.mmi) B_Mesa.Enabled = false;
			if (!m_handler.et42x) B_ET42X.Enabled = false;
			if (!m_handler.vt612) B_VT612.Enabled = false;

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
			this.B_Quit = new System.Windows.Forms.Button();
			this.B_EBuLaTools = new System.Windows.Forms.Button();
			this.L_version = new System.Windows.Forms.Label();
			this.b_Info = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.l_brand_version = new System.Windows.Forms.Label();
			this.l_brand = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.B_EBuLa = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.B_David2 = new System.Windows.Forms.Button();
			this.B_David1 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.B_Diagnose = new System.Windows.Forms.Button();
			this.B_Mesa = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.B_VT612 = new System.Windows.Forms.Button();
			this.B_ET42X = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.B_ICE3_2 = new System.Windows.Forms.Button();
			this.B_ICE3_1 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// B_Quit
			// 
			this.B_Quit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Quit.Location = new System.Drawing.Point(304, 528);
			this.B_Quit.Name = "B_Quit";
			this.B_Quit.Size = new System.Drawing.Size(184, 40);
			this.B_Quit.TabIndex = 2;
			this.B_Quit.Text = "Beenden";
			this.B_Quit.Click += new System.EventHandler(this.B_Quit_Click);
			// 
			// B_EBuLaTools
			// 
			this.B_EBuLaTools.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_EBuLaTools.Location = new System.Drawing.Point(400, 480);
			this.B_EBuLaTools.Name = "B_EBuLaTools";
			this.B_EBuLaTools.Size = new System.Drawing.Size(88, 40);
			this.B_EBuLaTools.TabIndex = 3;
			this.B_EBuLaTools.Text = "Einstellungen";
			this.B_EBuLaTools.Click += new System.EventHandler(this.B_EBuLaTools_Click);
			// 
			// L_version
			// 
			this.L_version.Location = new System.Drawing.Point(672, 576);
			this.L_version.Name = "L_version";
			this.L_version.Size = new System.Drawing.Size(128, 23);
			this.L_version.TabIndex = 4;
			this.L_version.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// b_Info
			// 
			this.b_Info.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_Info.Location = new System.Drawing.Point(304, 480);
			this.b_Info.Name = "b_Info";
			this.b_Info.Size = new System.Drawing.Size(88, 40);
			this.b_Info.TabIndex = 8;
			this.b_Info.Text = "Info";
			this.b_Info.Click += new System.EventHandler(this.b_Info_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.MidnightBlue;
			this.panel1.Controls.Add(this.l_brand_version);
			this.panel1.Controls.Add(this.l_brand);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(800, 80);
			this.panel1.TabIndex = 15;
			// 
			// l_brand_version
			// 
			this.l_brand_version.AutoSize = true;
			this.l_brand_version.BackColor = System.Drawing.Color.Transparent;
			this.l_brand_version.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_brand_version.ForeColor = System.Drawing.Color.White;
			this.l_brand_version.Location = new System.Drawing.Point(262, 26);
			this.l_brand_version.Name = "l_brand_version";
			this.l_brand_version.Size = new System.Drawing.Size(85, 42);
			this.l_brand_version.TabIndex = 18;
			this.l_brand_version.Text = "1.0.0";
			// 
			// l_brand
			// 
			this.l_brand.AutoSize = true;
			this.l_brand.BackColor = System.Drawing.Color.Transparent;
			this.l_brand.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.l_brand.ForeColor = System.Drawing.Color.White;
			this.l_brand.Location = new System.Drawing.Point(16, 8);
			this.l_brand.Name = "l_brand";
			this.l_brand.Size = new System.Drawing.Size(261, 61);
			this.l_brand.TabIndex = 17;
			this.l_brand.Text = "ZusiDisplay";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 80);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(800, 16);
			this.panel2.TabIndex = 16;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.B_EBuLa);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(304, 136);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(184, 88);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Fahplananzeige:";
			// 
			// B_EBuLa
			// 
			this.B_EBuLa.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_EBuLa.Location = new System.Drawing.Point(24, 24);
			this.B_EBuLa.Name = "B_EBuLa";
			this.B_EBuLa.Size = new System.Drawing.Size(136, 48);
			this.B_EBuLa.TabIndex = 1;
			this.B_EBuLa.Text = "EBuLa";
			this.B_EBuLa.Click += new System.EventHandler(this.B_EBuLa_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.B_David2);
			this.groupBox2.Controls.Add(this.B_David1);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(416, 256);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 88);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Diagnosedisplay ICE 1 und ICE 2:";
			// 
			// B_David2
			// 
			this.B_David2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_David2.Location = new System.Drawing.Point(176, 24);
			this.B_David2.Name = "B_David2";
			this.B_David2.Size = new System.Drawing.Size(144, 48);
			this.B_David2.TabIndex = 13;
			this.B_David2.Text = "DAVID (rechts)";
			this.B_David2.Click += new System.EventHandler(this.B_David2_Click);
			// 
			// B_David1
			// 
			this.B_David1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_David1.Location = new System.Drawing.Point(16, 24);
			this.B_David1.Name = "B_David1";
			this.B_David1.Size = new System.Drawing.Size(144, 48);
			this.B_David1.TabIndex = 12;
			this.B_David1.Text = "DAVID (links)";
			this.B_David1.Click += new System.EventHandler(this.B_David_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.B_Diagnose);
			this.groupBox3.Controls.Add(this.B_Mesa);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(40, 256);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(336, 88);
			this.groupBox3.TabIndex = 19;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Diagnosedisplay E-Lok:";
			// 
			// B_Diagnose
			// 
			this.B_Diagnose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Diagnose.Location = new System.Drawing.Point(16, 24);
			this.B_Diagnose.Name = "B_Diagnose";
			this.B_Diagnose.Size = new System.Drawing.Size(144, 48);
			this.B_Diagnose.TabIndex = 18;
			this.B_Diagnose.Text = "Diagnosedisplay";
			this.B_Diagnose.Click += new System.EventHandler(this.B_Diagnose_Click);
			// 
			// B_Mesa
			// 
			this.B_Mesa.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Mesa.Location = new System.Drawing.Point(176, 24);
			this.B_Mesa.Name = "B_Mesa";
			this.B_Mesa.Size = new System.Drawing.Size(144, 48);
			this.B_Mesa.TabIndex = 15;
			this.B_Mesa.Text = "MMI";
			this.B_Mesa.Click += new System.EventHandler(this.B_Mesa_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.B_VT612);
			this.groupBox4.Controls.Add(this.B_ET42X);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(40, 360);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(336, 88);
			this.groupBox4.TabIndex = 20;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Diagnosedisplay Triebwagen:";
			// 
			// B_VT612
			// 
			this.B_VT612.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_VT612.Location = new System.Drawing.Point(176, 24);
			this.B_VT612.Name = "B_VT612";
			this.B_VT612.Size = new System.Drawing.Size(144, 48);
			this.B_VT612.TabIndex = 19;
			this.B_VT612.Text = "VT611/612";
			this.B_VT612.Click += new System.EventHandler(this.B_VT612_Click);
			// 
			// B_ET42X
			// 
			this.B_ET42X.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ET42X.Location = new System.Drawing.Point(16, 24);
			this.B_ET42X.Name = "B_ET42X";
			this.B_ET42X.Size = new System.Drawing.Size(144, 48);
			this.B_ET42X.TabIndex = 18;
			this.B_ET42X.Text = "ET423-426";
			this.B_ET42X.Click += new System.EventHandler(this.B_ET42X_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.B_ICE3_2);
			this.groupBox5.Controls.Add(this.B_ICE3_1);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(416, 360);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(328, 88);
			this.groupBox5.TabIndex = 21;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Diagnosedisplay ICE 3 / T / TD:";
			// 
			// B_ICE3_2
			// 
			this.B_ICE3_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ICE3_2.Location = new System.Drawing.Point(172, 24);
			this.B_ICE3_2.Name = "B_ICE3_2";
			this.B_ICE3_2.Size = new System.Drawing.Size(144, 48);
			this.B_ICE3_2.TabIndex = 17;
			this.B_ICE3_2.Text = "Display (rechts)";
			this.B_ICE3_2.Click += new System.EventHandler(this.B_ICE3_2_Click);
			// 
			// B_ICE3_1
			// 
			this.B_ICE3_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ICE3_1.Location = new System.Drawing.Point(12, 24);
			this.B_ICE3_1.Name = "B_ICE3_1";
			this.B_ICE3_1.Size = new System.Drawing.Size(144, 48);
			this.B_ICE3_1.TabIndex = 16;
			this.B_ICE3_1.Text = "Display (links)";
			this.B_ICE3_1.Click += new System.EventHandler(this.B_ICE3_1_Click);
			// 
			// SelectionMenu
			// 
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.b_Info);
			this.Controls.Add(this.L_version);
			this.Controls.Add(this.B_EBuLaTools);
			this.Controls.Add(this.B_Quit);
			this.Name = "SelectionMenu";
			this.Size = new System.Drawing.Size(800, 600);
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        private void B_EBuLa_Click(object sender, System.EventArgs e)
        {
            m_handler.Load = Loadwhat.EBuLa;
            Dispose();
        }

        private void B_Quit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void B_Mesa_Click(object sender, System.EventArgs e)
        {
            m_handler.Load = Loadwhat.MMI;
            Dispose();
        }

        private void B_EBuLaTools_Click(object sender, System.EventArgs e)
        {
            m_handler.Load = Loadwhat.EBuLaTools;
            Dispose();
        }

		private void B_David_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.DAVID;
			Dispose();
		}

		private void b_Info_Click(object sender, System.EventArgs e)
		{
			(new AboutBox(Application.ProductVersion, m_topmost)).ShowDialog();
		}

		private void B_David2_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.DAVID2;
			Dispose();
		}

		private void B_ICE3_1_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.ICE3_1;
			Dispose();
		}

		private void B_ICE3_2_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.ICE3_2;
			Dispose();
		}

		private void B_ET42X_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.ET42X;
			Dispose();
		}

		private void B_VT612_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.VT612;
			Dispose();
		}

		private void B_Diagnose_Click(object sender, System.EventArgs e)
		{
			m_handler.Load = Loadwhat.DIAGNOSE;
			Dispose();		
		}

		private string GetVersion(string complete)
		{
			int count = 0;
			int pos = -1;

			foreach(char s in complete.ToCharArray())
			{
				pos++;
				string str = s.ToString();
				if (str == ".") count++;
				if (count == 3)
				{
					return complete.Substring(0, pos);
				}
			}
			return complete;
		}
	}
}
