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

        private System.Windows.Forms.Button B_EBuLa;
        private System.Windows.Forms.Button B_Mesa;
        private System.Windows.Forms.Button B_Quit;
        private System.Windows.Forms.Button B_EBuLaTools;
        private System.Windows.Forms.Label L_version;
		private System.Windows.Forms.Button b_Info;
		private System.Windows.Forms.Button B_David1;
		private System.Windows.Forms.Button B_David2;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button B_ICE3_2;
		private System.Windows.Forms.Button B_ICE3_1;
		private System.Windows.Forms.Button B_ET42X;
		private System.Windows.Forms.Button B_VT612;
		private System.Windows.Forms.Button B_Diagnose;

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

            m_handler = h;
            L_version.Text = "Version: "+Application.ProductVersion;
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
			this.B_EBuLa = new System.Windows.Forms.Button();
			this.B_Mesa = new System.Windows.Forms.Button();
			this.B_Quit = new System.Windows.Forms.Button();
			this.B_EBuLaTools = new System.Windows.Forms.Button();
			this.L_version = new System.Windows.Forms.Label();
			this.B_David1 = new System.Windows.Forms.Button();
			this.b_Info = new System.Windows.Forms.Button();
			this.B_David2 = new System.Windows.Forms.Button();
			this.B_ICE3_2 = new System.Windows.Forms.Button();
			this.B_ICE3_1 = new System.Windows.Forms.Button();
			this.B_ET42X = new System.Windows.Forms.Button();
			this.B_VT612 = new System.Windows.Forms.Button();
			this.B_Diagnose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// B_EBuLa
			// 
			this.B_EBuLa.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_EBuLa.Location = new System.Drawing.Point(168, 24);
			this.B_EBuLa.Name = "B_EBuLa";
			this.B_EBuLa.Size = new System.Drawing.Size(456, 88);
			this.B_EBuLa.TabIndex = 0;
			this.B_EBuLa.Text = "EBuLa";
			this.B_EBuLa.Click += new System.EventHandler(this.B_EBuLa_Click);
			// 
			// B_Mesa
			// 
			this.B_Mesa.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Mesa.Location = new System.Drawing.Point(168, 120);
			this.B_Mesa.Name = "B_Mesa";
			this.B_Mesa.Size = new System.Drawing.Size(216, 80);
			this.B_Mesa.TabIndex = 1;
			this.B_Mesa.Text = "MMI";
			this.B_Mesa.Click += new System.EventHandler(this.B_Mesa_Click);
			// 
			// B_Quit
			// 
			this.B_Quit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Quit.Location = new System.Drawing.Point(168, 528);
			this.B_Quit.Name = "B_Quit";
			this.B_Quit.Size = new System.Drawing.Size(456, 56);
			this.B_Quit.TabIndex = 2;
			this.B_Quit.Text = "Beenden";
			this.B_Quit.Click += new System.EventHandler(this.B_Quit_Click);
			// 
			// B_EBuLaTools
			// 
			this.B_EBuLaTools.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_EBuLaTools.Location = new System.Drawing.Point(168, 472);
			this.B_EBuLaTools.Name = "B_EBuLaTools";
			this.B_EBuLaTools.Size = new System.Drawing.Size(216, 48);
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
			// B_David1
			// 
			this.B_David1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_David1.Location = new System.Drawing.Point(168, 296);
			this.B_David1.Name = "B_David1";
			this.B_David1.Size = new System.Drawing.Size(216, 80);
			this.B_David1.TabIndex = 7;
			this.B_David1.Text = "DAVID 1";
			this.B_David1.Click += new System.EventHandler(this.B_David_Click);
			// 
			// b_Info
			// 
			this.b_Info.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_Info.Location = new System.Drawing.Point(408, 472);
			this.b_Info.Name = "b_Info";
			this.b_Info.Size = new System.Drawing.Size(216, 48);
			this.b_Info.TabIndex = 8;
			this.b_Info.Text = "Info";
			this.b_Info.Click += new System.EventHandler(this.b_Info_Click);
			// 
			// B_David2
			// 
			this.B_David2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_David2.Location = new System.Drawing.Point(408, 296);
			this.B_David2.Name = "B_David2";
			this.B_David2.Size = new System.Drawing.Size(216, 80);
			this.B_David2.TabIndex = 9;
			this.B_David2.Text = "DAVID 2";
			this.B_David2.Click += new System.EventHandler(this.B_David2_Click);
			// 
			// B_ICE3_2
			// 
			this.B_ICE3_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ICE3_2.Location = new System.Drawing.Point(408, 384);
			this.B_ICE3_2.Name = "B_ICE3_2";
			this.B_ICE3_2.Size = new System.Drawing.Size(216, 80);
			this.B_ICE3_2.TabIndex = 11;
			this.B_ICE3_2.Text = "ICE-3/T(D)  Display 2";
			this.B_ICE3_2.Click += new System.EventHandler(this.B_ICE3_2_Click);
			// 
			// B_ICE3_1
			// 
			this.B_ICE3_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ICE3_1.Location = new System.Drawing.Point(168, 384);
			this.B_ICE3_1.Name = "B_ICE3_1";
			this.B_ICE3_1.Size = new System.Drawing.Size(216, 80);
			this.B_ICE3_1.TabIndex = 10;
			this.B_ICE3_1.Text = "ICE-3/T(D)  Display 1";
			this.B_ICE3_1.Click += new System.EventHandler(this.B_ICE3_1_Click);
			// 
			// B_ET42X
			// 
			this.B_ET42X.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_ET42X.Location = new System.Drawing.Point(168, 208);
			this.B_ET42X.Name = "B_ET42X";
			this.B_ET42X.Size = new System.Drawing.Size(216, 80);
			this.B_ET42X.TabIndex = 12;
			this.B_ET42X.Text = "ET42X Display";
			this.B_ET42X.Click += new System.EventHandler(this.B_ET42X_Click);
			// 
			// B_VT612
			// 
			this.B_VT612.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_VT612.Location = new System.Drawing.Point(408, 208);
			this.B_VT612.Name = "B_VT612";
			this.B_VT612.Size = new System.Drawing.Size(216, 80);
			this.B_VT612.TabIndex = 13;
			this.B_VT612.Text = "VT611/612 Display";
			this.B_VT612.Click += new System.EventHandler(this.B_VT612_Click);
			// 
			// B_Diagnose
			// 
			this.B_Diagnose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.B_Diagnose.Location = new System.Drawing.Point(408, 120);
			this.B_Diagnose.Name = "B_Diagnose";
			this.B_Diagnose.Size = new System.Drawing.Size(216, 80);
			this.B_Diagnose.TabIndex = 14;
			this.B_Diagnose.Text = "Diagnose Display";
			this.B_Diagnose.Click += new System.EventHandler(this.B_Diagnose_Click);
			// 
			// SelectionMenu
			// 
			this.Controls.Add(this.B_Diagnose);
			this.Controls.Add(this.B_VT612);
			this.Controls.Add(this.B_ET42X);
			this.Controls.Add(this.B_ICE3_2);
			this.Controls.Add(this.B_ICE3_1);
			this.Controls.Add(this.B_David2);
			this.Controls.Add(this.b_Info);
			this.Controls.Add(this.B_David1);
			this.Controls.Add(this.L_version);
			this.Controls.Add(this.B_EBuLaTools);
			this.Controls.Add(this.B_Quit);
			this.Controls.Add(this.B_Mesa);
			this.Controls.Add(this.B_EBuLa);
			this.Name = "SelectionMenu";
			this.Size = new System.Drawing.Size(800, 600);
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
	}
}
