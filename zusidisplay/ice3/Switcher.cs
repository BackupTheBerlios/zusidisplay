using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.ICE3
{
	/// <summary>
	/// Zusammenfassung für Switcher.
	/// </summary>
	public class Switcher : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button b_ok;
		private System.Windows.Forms.Button b_cancel;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox cb_12std;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rb_411_1;
		private System.Windows.Forms.RadioButton rb_406_1;
		private System.Windows.Forms.RadioButton rb_403_1;
		private System.Windows.Forms.RadioButton rb_415_1;
		private System.Windows.Forms.RadioButton rb_605_1;
		private System.Windows.Forms.RadioButton rb_605_2;
		private System.Windows.Forms.RadioButton rb_411_2;
		private System.Windows.Forms.RadioButton rb_406_2;
		private System.Windows.Forms.RadioButton rb_403_2;
		private System.Windows.Forms.RadioButton rb_415_2;
		private System.Windows.Forms.RadioButton rb_605_605_2;
		private System.Windows.Forms.RadioButton rb_kein_2;
		private System.Windows.Forms.RadioButton rb_415_415_2;
		private System.Windows.Forms.RadioButton rb_605_605_1;
		private System.Windows.Forms.RadioButton rb_415_415_1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox tB_ZugNr;

		MMI.ICE3.ICE3State m_state;

		public Switcher(ref MMI.ICE3.ICE3State s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			if (s.ICEtype1 == ICE3TYPE.ICE403)
				rb_403_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE406)
				rb_406_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE411)
				rb_411_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE415)
				rb_415_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE415_2)
				rb_415_415_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE605)
				rb_605_1.Checked = true;
			else if (s.ICEtype1 == ICE3TYPE.ICE605_2)
				rb_605_605_1.Checked = true;

			if (s.ICEtype2 == ICE3TYPE.ICE403)
				rb_403_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE406)
				rb_406_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE411)
				rb_411_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE415)
				rb_415_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE415_2)
				rb_415_415_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE605)
				rb_605_2.Checked = true;
			else if (s.ICEtype2 == ICE3TYPE.ICE605_2)
				rb_605_605_2.Checked = true;

			if (m_state.addtionalhours)
				cb_12std.Checked = true;

			tB_ZugNr.Text = s.Zugnummer;

			this.TopMost = topMost;
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rb_415_415_1 = new System.Windows.Forms.RadioButton();
			this.rb_605_605_1 = new System.Windows.Forms.RadioButton();
			this.rb_605_1 = new System.Windows.Forms.RadioButton();
			this.rb_411_1 = new System.Windows.Forms.RadioButton();
			this.rb_406_1 = new System.Windows.Forms.RadioButton();
			this.rb_403_1 = new System.Windows.Forms.RadioButton();
			this.rb_415_1 = new System.Windows.Forms.RadioButton();
			this.b_ok = new System.Windows.Forms.Button();
			this.b_cancel = new System.Windows.Forms.Button();
			this.cb_12std = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rb_415_415_2 = new System.Windows.Forms.RadioButton();
			this.rb_kein_2 = new System.Windows.Forms.RadioButton();
			this.rb_605_605_2 = new System.Windows.Forms.RadioButton();
			this.rb_605_2 = new System.Windows.Forms.RadioButton();
			this.rb_411_2 = new System.Windows.Forms.RadioButton();
			this.rb_406_2 = new System.Windows.Forms.RadioButton();
			this.rb_403_2 = new System.Windows.Forms.RadioButton();
			this.rb_415_2 = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tB_ZugNr = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_415_415_1);
			this.groupBox1.Controls.Add(this.rb_605_605_1);
			this.groupBox1.Controls.Add(this.rb_605_1);
			this.groupBox1.Controls.Add(this.rb_411_1);
			this.groupBox1.Controls.Add(this.rb_406_1);
			this.groupBox1.Controls.Add(this.rb_403_1);
			this.groupBox1.Controls.Add(this.rb_415_1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 256);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bitte wählen Sie den führenden Zugtyp aus:";
			// 
			// rb_415_415_1
			// 
			this.rb_415_415_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_415_415_1.Location = new System.Drawing.Point(16, 160);
			this.rb_415_415_1.Name = "rb_415_415_1";
			this.rb_415_415_1.Size = new System.Drawing.Size(168, 24);
			this.rb_415_415_1.TabIndex = 15;
			this.rb_415_415_1.Text = "ICE-T 5-teilig (2xBR415)";
			this.rb_415_415_1.CheckedChanged += new System.EventHandler(this.rb_415_415_1_CheckedChanged);
			// 
			// rb_605_605_1
			// 
			this.rb_605_605_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_605_605_1.Location = new System.Drawing.Point(16, 224);
			this.rb_605_605_1.Name = "rb_605_605_1";
			this.rb_605_605_1.Size = new System.Drawing.Size(168, 24);
			this.rb_605_605_1.TabIndex = 13;
			this.rb_605_605_1.Text = "ICE-TD (2xBR605)";
			this.rb_605_605_1.CheckedChanged += new System.EventHandler(this.rb_605_605_1_CheckedChanged);
			// 
			// rb_605_1
			// 
			this.rb_605_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_605_1.Location = new System.Drawing.Point(16, 192);
			this.rb_605_1.Name = "rb_605_1";
			this.rb_605_1.Size = new System.Drawing.Size(168, 24);
			this.rb_605_1.TabIndex = 5;
			this.rb_605_1.Text = "ICE-TD (BR605)";
			this.rb_605_1.Click += new System.EventHandler(this.rb_605_1_CheckedChanged);
			// 
			// rb_411_1
			// 
			this.rb_411_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_411_1.Location = new System.Drawing.Point(16, 96);
			this.rb_411_1.Name = "rb_411_1";
			this.rb_411_1.Size = new System.Drawing.Size(168, 24);
			this.rb_411_1.TabIndex = 4;
			this.rb_411_1.Text = "ICE-T 7-teilig (BR411)";
			this.rb_411_1.Click += new System.EventHandler(this.rb_403_1_CheckedChanged);
			// 
			// rb_406_1
			// 
			this.rb_406_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_406_1.Location = new System.Drawing.Point(16, 64);
			this.rb_406_1.Name = "rb_406_1";
			this.rb_406_1.Size = new System.Drawing.Size(168, 24);
			this.rb_406_1.TabIndex = 3;
			this.rb_406_1.Text = "ICE 3M (BR406)";
			this.rb_406_1.Click += new System.EventHandler(this.rb_403_1_CheckedChanged);
			// 
			// rb_403_1
			// 
			this.rb_403_1.Checked = true;
			this.rb_403_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_403_1.Location = new System.Drawing.Point(16, 32);
			this.rb_403_1.Name = "rb_403_1";
			this.rb_403_1.Size = new System.Drawing.Size(168, 24);
			this.rb_403_1.TabIndex = 1;
			this.rb_403_1.TabStop = true;
			this.rb_403_1.Text = "ICE 3 (BR403)";
			this.rb_403_1.CheckedChanged += new System.EventHandler(this.rb_403_1_CheckedChanged);
			// 
			// rb_415_1
			// 
			this.rb_415_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_415_1.Location = new System.Drawing.Point(16, 128);
			this.rb_415_1.Name = "rb_415_1";
			this.rb_415_1.Size = new System.Drawing.Size(168, 24);
			this.rb_415_1.TabIndex = 0;
			this.rb_415_1.Text = "ICE-T 5-teilig (BR415)";
			this.rb_415_1.Click += new System.EventHandler(this.rb_403_1_CheckedChanged);
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(88, 344);
			this.b_ok.Name = "b_ok";
			this.b_ok.TabIndex = 2;
			this.b_ok.Text = "OK";
			this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.Enabled = false;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(192, 296);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 3;
			this.b_cancel.Text = "Abbrechen";
			this.b_cancel.Visible = false;
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(24, 288);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(192, 24);
			this.cb_12std.TabIndex = 5;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rb_415_415_2);
			this.groupBox3.Controls.Add(this.rb_kein_2);
			this.groupBox3.Controls.Add(this.rb_605_605_2);
			this.groupBox3.Controls.Add(this.rb_605_2);
			this.groupBox3.Controls.Add(this.rb_411_2);
			this.groupBox3.Controls.Add(this.rb_406_2);
			this.groupBox3.Controls.Add(this.rb_403_2);
			this.groupBox3.Controls.Add(this.rb_415_2);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(256, 16);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(232, 288);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Bitte wählen Sie den folgenden Zugtyp aus:";
			// 
			// rb_415_415_2
			// 
			this.rb_415_415_2.Enabled = false;
			this.rb_415_415_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_415_415_2.Location = new System.Drawing.Point(16, 160);
			this.rb_415_415_2.Name = "rb_415_415_2";
			this.rb_415_415_2.Size = new System.Drawing.Size(168, 24);
			this.rb_415_415_2.TabIndex = 14;
			this.rb_415_415_2.Text = "ICE-T 5-teilig (2xBR415)";
			// 
			// rb_kein_2
			// 
			this.rb_kein_2.Checked = true;
			this.rb_kein_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_2.Location = new System.Drawing.Point(16, 256);
			this.rb_kein_2.Name = "rb_kein_2";
			this.rb_kein_2.Size = new System.Drawing.Size(168, 24);
			this.rb_kein_2.TabIndex = 13;
			this.rb_kein_2.TabStop = true;
			this.rb_kein_2.Text = "<kein Zug>";
			// 
			// rb_605_605_2
			// 
			this.rb_605_605_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_605_605_2.Location = new System.Drawing.Point(16, 224);
			this.rb_605_605_2.Name = "rb_605_605_2";
			this.rb_605_605_2.Size = new System.Drawing.Size(168, 24);
			this.rb_605_605_2.TabIndex = 12;
			this.rb_605_605_2.Text = "ICE-TD (2xBR605)";
			// 
			// rb_605_2
			// 
			this.rb_605_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_605_2.Location = new System.Drawing.Point(16, 192);
			this.rb_605_2.Name = "rb_605_2";
			this.rb_605_2.Size = new System.Drawing.Size(168, 24);
			this.rb_605_2.TabIndex = 10;
			this.rb_605_2.Text = "ICE-TD (BR605)";
			// 
			// rb_411_2
			// 
			this.rb_411_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_411_2.Location = new System.Drawing.Point(16, 96);
			this.rb_411_2.Name = "rb_411_2";
			this.rb_411_2.Size = new System.Drawing.Size(168, 24);
			this.rb_411_2.TabIndex = 9;
			this.rb_411_2.Text = "ICE-T 7-teilig (BR411)";
			// 
			// rb_406_2
			// 
			this.rb_406_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_406_2.Location = new System.Drawing.Point(16, 64);
			this.rb_406_2.Name = "rb_406_2";
			this.rb_406_2.Size = new System.Drawing.Size(168, 24);
			this.rb_406_2.TabIndex = 8;
			this.rb_406_2.Text = "ICE 3M (BR406)";
			// 
			// rb_403_2
			// 
			this.rb_403_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_403_2.Location = new System.Drawing.Point(16, 32);
			this.rb_403_2.Name = "rb_403_2";
			this.rb_403_2.Size = new System.Drawing.Size(168, 24);
			this.rb_403_2.TabIndex = 7;
			this.rb_403_2.Text = "ICE 3 (BR403)";
			// 
			// rb_415_2
			// 
			this.rb_415_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_415_2.Location = new System.Drawing.Point(16, 128);
			this.rb_415_2.Name = "rb_415_2";
			this.rb_415_2.Size = new System.Drawing.Size(168, 24);
			this.rb_415_2.TabIndex = 6;
			this.rb_415_2.Text = "ICE-T 5-teilig (BR415)";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tB_ZugNr);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(256, 312);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(232, 64);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Zugnummer:";
			// 
			// tB_ZugNr
			// 
			this.tB_ZugNr.Location = new System.Drawing.Point(24, 32);
			this.tB_ZugNr.Name = "tB_ZugNr";
			this.tB_ZugNr.TabIndex = 0;
			this.tB_ZugNr.Text = "";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(506, 384);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.cb_12std);
			this.Controls.Add(this.b_cancel);
			this.Controls.Add(this.b_ok);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Switcher";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ICE Wahl";
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_403_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE403;
			}
			else if(rb_406_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE406;
			}
			else if (rb_411_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE411;
			}
			else if (rb_415_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE415;
			}
			else if (rb_415_415_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE415_2;
			}
			else if (rb_605_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE605;
			}
			else if (rb_605_605_1.Checked)
			{
				m_state.ICEtype1 = ICE3TYPE.ICE605_2;
			}
			
			if (rb_403_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE403;
			}
			else if(rb_406_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE406;
			}
			else if (rb_411_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE411;
			}
			else if (rb_415_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE415;
			}
			else if (rb_415_415_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE415_2;
			}
			else if (rb_605_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE605;
			}
			else if (rb_605_605_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.ICE605_2;
			}
			else if (rb_kein_2.Checked)
			{
				m_state.ICEtype2 = ICE3TYPE.NONE;
			}

			m_state.addtionalhours = cb_12std.Checked;

			m_state.Zugnummer = tB_ZugNr.Text;
		}

		private void ResetAll()
		{
			rb_403_2.Enabled = true;
			rb_406_2.Enabled = true;
			rb_411_2.Enabled = true;
			rb_415_2.Enabled = true;
			rb_415_415_2.Enabled = false;
			rb_605_2.Enabled = true;
			rb_605_605_2.Enabled = true;
			rb_kein_2.Enabled = true;
		}

		private void rb_403_1_CheckedChanged(object sender, System.EventArgs e)
		{
			ResetAll();
		}

		private void rb_415_415_1_CheckedChanged(object sender, System.EventArgs e)
		{
			rb_403_2.Enabled = false;
			rb_406_2.Enabled = false;
			rb_411_2.Enabled = false;
			rb_415_2.Enabled = true;
			rb_415_415_2.Enabled = false;
			rb_605_2.Enabled = true;
			rb_605_605_2.Enabled = false;
			rb_kein_2.Enabled = false;

			rb_415_2.Checked = true;
		}

		private void rb_605_605_1_CheckedChanged(object sender, System.EventArgs e)
		{
			rb_415_415_2.Enabled = false;
			rb_605_605_2.Enabled = false;
			rb_kein_2.Enabled = false;

			rb_605_2.Checked = true;
		}

		private void rb_605_1_CheckedChanged(object sender, System.EventArgs e)
		{
			rb_403_2.Enabled = true;
			rb_406_2.Enabled = true;
			rb_411_2.Enabled = true;
			rb_415_2.Enabled = true;
			rb_605_2.Enabled = true;
			rb_605_605_2.Enabled = true;
			rb_kein_2.Enabled = true;
			rb_415_415_2.Enabled = true;
		}
	}
}
