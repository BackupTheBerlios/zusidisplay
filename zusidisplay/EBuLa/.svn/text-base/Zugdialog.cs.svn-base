using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	public class Zugdialog : System.Windows.Forms.Form, KeyHandlerInterface
	{
        private Control m_c;
        private System.Windows.Forms.Label l_static1;
        private System.Windows.Forms.TextBox tb_Zugnummer;
        private System.Windows.Forms.Label l_static2;
        private System.Windows.Forms.CheckBox cb_Zusi;
        private System.Windows.Forms.TextBox tb_Zuggattung;

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button B_C;
		private System.Windows.Forms.CheckBox cb_TrackPath;
        private System.Windows.Forms.Button B_E;


		public Zugdialog(ref Control c)
		{
			InitializeComponent();

            m_c = c;

			if (m_c.timer_disabled)
			{
				cb_Zusi.Checked = false;
				cb_Zusi.Enabled = false;
				cb_Zusi.Text += " (Kein Zusi!)";
			}
            
			if (cb_Zusi.Checked && cb_Zusi.Enabled && m_c.searchInTrackPath)
			{
				cb_TrackPath.Checked = true;
			}
			else if (!cb_Zusi.Enabled)
			{
				cb_TrackPath.Checked = false;
				cb_TrackPath.Enabled = false;
			}

            if (c.inverse)
            {
                //switch to inverse
                this.BackColor = System.Drawing.Color.LightSlateGray;
                this.ForeColor = System.Drawing.Color.White;
            }

			this.TopMost = c.XMLConf.TopMost;
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

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.l_static1 = new System.Windows.Forms.Label();
			this.tb_Zugnummer = new System.Windows.Forms.TextBox();
			this.B_C = new System.Windows.Forms.Button();
			this.B_E = new System.Windows.Forms.Button();
			this.tb_Zuggattung = new System.Windows.Forms.TextBox();
			this.l_static2 = new System.Windows.Forms.Label();
			this.cb_Zusi = new System.Windows.Forms.CheckBox();
			this.cb_TrackPath = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// l_static1
			// 
			this.l_static1.Enabled = false;
			this.l_static1.Location = new System.Drawing.Point(6, 56);
			this.l_static1.Name = "l_static1";
			this.l_static1.Size = new System.Drawing.Size(200, 23);
			this.l_static1.TabIndex = 2;
			this.l_static1.Text = "Geben Sie die Zugnummer ein:";
			this.l_static1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tb_Zugnummer
			// 
			this.tb_Zugnummer.Enabled = false;
			this.tb_Zugnummer.Location = new System.Drawing.Point(208, 56);
			this.tb_Zugnummer.Name = "tb_Zugnummer";
			this.tb_Zugnummer.TabIndex = 3;
			this.tb_Zugnummer.Text = "";
			// 
			// B_C
			// 
			this.B_C.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.B_C.Location = new System.Drawing.Point(336, 16);
			this.B_C.Name = "B_C";
			this.B_C.Size = new System.Drawing.Size(32, 32);
			this.B_C.TabIndex = 5;
			this.B_C.Text = "C";
			this.B_C.Click += new System.EventHandler(this.B_C_Click);
			// 
			// B_E
			// 
			this.B_E.Location = new System.Drawing.Point(336, 88);
			this.B_E.Name = "B_E";
			this.B_E.Size = new System.Drawing.Size(32, 32);
			this.B_E.TabIndex = 6;
			this.B_E.Text = "E";
			this.B_E.Click += new System.EventHandler(this.B_E_Click);
			// 
			// tb_Zuggattung
			// 
			this.tb_Zuggattung.Enabled = false;
			this.tb_Zuggattung.Location = new System.Drawing.Point(208, 24);
			this.tb_Zuggattung.Name = "tb_Zuggattung";
			this.tb_Zuggattung.TabIndex = 1;
			this.tb_Zuggattung.Text = "";
			// 
			// l_static2
			// 
			this.l_static2.Enabled = false;
			this.l_static2.Location = new System.Drawing.Point(38, 24);
			this.l_static2.Name = "l_static2";
			this.l_static2.Size = new System.Drawing.Size(168, 23);
			this.l_static2.TabIndex = 0;
			this.l_static2.Text = "Geben Sie die Zuggattung ein:";
			this.l_static2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cb_Zusi
			// 
			this.cb_Zusi.Checked = true;
			this.cb_Zusi.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cb_Zusi.Location = new System.Drawing.Point(56, 96);
			this.cb_Zusi.Name = "cb_Zusi";
			this.cb_Zusi.Size = new System.Drawing.Size(256, 24);
			this.cb_Zusi.TabIndex = 4;
			this.cb_Zusi.Text = "Daten von Zusi übernehmen";
			this.cb_Zusi.CheckedChanged += new System.EventHandler(this.cb_Zusi_CheckedChanged);
			// 
			// cb_TrackPath
			// 
			this.cb_TrackPath.Location = new System.Drawing.Point(56, 120);
			this.cb_TrackPath.Name = "cb_TrackPath";
			this.cb_TrackPath.Size = new System.Drawing.Size(256, 24);
			this.cb_TrackPath.TabIndex = 7;
			this.cb_TrackPath.Text = "Daten im Streckenverzeichnis suchen";
			this.cb_TrackPath.CheckedChanged += new System.EventHandler(this.cb_TrackPath_CheckedChanged);
			// 
			// Zugdialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(426, 176);
			this.ControlBox = false;
			this.Controls.Add(this.cb_TrackPath);
			this.Controls.Add(this.cb_Zusi);
			this.Controls.Add(this.tb_Zuggattung);
			this.Controls.Add(this.tb_Zugnummer);
			this.Controls.Add(this.l_static2);
			this.Controls.Add(this.B_E);
			this.Controls.Add(this.B_C);
			this.Controls.Add(this.l_static1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Zugdialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EBuLa Zugauswahl";
			this.ResumeLayout(false);

		}
		#endregion

        private void cb_Zusi_CheckedChanged(object sender, System.EventArgs e)
        {
            l_static1.Enabled = !cb_Zusi.Checked; // nur Text
            l_static2.Enabled = !cb_Zusi.Checked; // nur Text
            tb_Zugnummer.Enabled = !cb_Zusi.Checked;
            tb_Zuggattung.Enabled = !cb_Zusi.Checked;
			cb_TrackPath.Enabled = cb_Zusi.Checked;

			if (cb_Zusi.Checked)
			{
				tb_Zuggattung.Text = "";
				tb_Zugnummer.Text = "";
				m_c.buffer_traintype = "";
				m_c.buffer_trainnumber = "";
			}
			else
			{
				cb_TrackPath.Checked = false;
			}
        }


        public void B_E_Click(object sender, System.EventArgs e)
        {
			if (!cb_Zusi.Checked /*&& (tb_Zugnummer.Text.EndsWith("e") || tb_Zuggattung.Text.EndsWith("e"))*/)
			{
				if (tb_Zugnummer.Text == "" && tb_Zuggattung.Text == "")
				{
					MessageBox.Show("Sie müssen Zuggattung und Zugnummer eingeben!");
					return;
				}
				else
				{
					m_c.buffer_trainnumber = tb_Zugnummer.Text;
					m_c.buffer_traintype = tb_Zuggattung.Text;
					this.DialogResult = DialogResult.OK;
					this.Dispose();
				}
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Dispose();
			}
        }

        public void B_C_Click(object sender, System.EventArgs e)
        {  
            if (cb_Zusi.Checked)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Dispose();
            }
        }

		private void cb_TrackPath_CheckedChanged(object sender, System.EventArgs e)
		{
			m_c.searchInTrackPath = cb_TrackPath.Checked;
		}
    }
}
