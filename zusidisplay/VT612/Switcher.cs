using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.VT612
{
	/// <summary>
	/// Zusammenfassung für Switcher.
	/// </summary>
	public class Switcher : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button b_ok;
		private System.Windows.Forms.Button b_cancel;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox cb_12std;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rb_kein_2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton rb_kein_3;
		private System.Windows.Forms.RadioButton rb_612_2;
		private System.Windows.Forms.RadioButton rb_611_2;
		private System.Windows.Forms.RadioButton rb_612_1;
		private System.Windows.Forms.RadioButton rb_611_1;
		private System.Windows.Forms.RadioButton rb_612_3;
		private System.Windows.Forms.RadioButton rb_611_3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rb_kein_4;
		private System.Windows.Forms.RadioButton rb_612_4;
		private System.Windows.Forms.RadioButton rb_611_4;

		MMI.VT612.VT612State m_state;

		public Switcher(ref MMI.VT612.VT612State s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			if (s.VT612type1 == VT612TYPE.VT611)
			{
				rb_611_1.Checked = true;
				rb_611_1_CheckedChanged(this, new EventArgs());
			}
			else if (s.VT612type1 == VT612TYPE.VT612)
			{
				rb_612_1.Checked = true;
				rb_612_1_CheckedChanged(this, new EventArgs());
			}

			if (s.VT612type2 == VT612TYPE.VT611)
				rb_611_2.Checked = true;
			else if (s.VT612type2 == VT612TYPE.VT612)
				rb_612_2.Checked = true;
			else
				rb_kein_2.Checked = true;

			if (s.VT612type3 == VT612TYPE.VT611)
				rb_611_3.Checked = true;
			else if (s.VT612type3 == VT612TYPE.VT612)
				rb_612_3.Checked = true;
			else
				rb_kein_3.Checked = true;

			if (s.VT612type4 == VT612TYPE.VT611)
				rb_611_4.Checked = true;
			else if (s.VT612type4 == VT612TYPE.VT612)
				rb_612_4.Checked = true;
			else
				rb_kein_4.Checked = true;

			if (m_state.addtionalhours)
				cb_12std.Checked = true;

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
			this.b_ok = new System.Windows.Forms.Button();
			this.b_cancel = new System.Windows.Forms.Button();
			this.cb_12std = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rb_kein_2 = new System.Windows.Forms.RadioButton();
			this.rb_612_2 = new System.Windows.Forms.RadioButton();
			this.rb_611_2 = new System.Windows.Forms.RadioButton();
			this.rb_612_1 = new System.Windows.Forms.RadioButton();
			this.rb_611_1 = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.rb_kein_3 = new System.Windows.Forms.RadioButton();
			this.rb_612_3 = new System.Windows.Forms.RadioButton();
			this.rb_611_3 = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rb_kein_4 = new System.Windows.Forms.RadioButton();
			this.rb_612_4 = new System.Windows.Forms.RadioButton();
			this.rb_611_4 = new System.Windows.Forms.RadioButton();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(160, 200);
			this.b_ok.Name = "b_ok";
			this.b_ok.TabIndex = 2;
			this.b_ok.Text = "OK";
			this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(272, 200);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 3;
			this.b_cancel.Text = "Abbrechen";
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(16, 160);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(192, 24);
			this.cb_12std.TabIndex = 5;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rb_kein_2);
			this.groupBox3.Controls.Add(this.rb_612_2);
			this.groupBox3.Controls.Add(this.rb_611_2);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(136, 16);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(120, 136);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "2. Zugtyp:";
			// 
			// rb_kein_2
			// 
			this.rb_kein_2.Checked = true;
			this.rb_kein_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_2.Location = new System.Drawing.Point(16, 96);
			this.rb_kein_2.Name = "rb_kein_2";
			this.rb_kein_2.Size = new System.Drawing.Size(88, 24);
			this.rb_kein_2.TabIndex = 13;
			this.rb_kein_2.TabStop = true;
			this.rb_kein_2.Text = "<kein Zug>";
			// 
			// rb_612_2
			// 
			this.rb_612_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_612_2.Location = new System.Drawing.Point(16, 64);
			this.rb_612_2.Name = "rb_612_2";
			this.rb_612_2.Size = new System.Drawing.Size(88, 24);
			this.rb_612_2.TabIndex = 8;
			this.rb_612_2.Text = "VT 612";
			// 
			// rb_611_2
			// 
			this.rb_611_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_611_2.Location = new System.Drawing.Point(16, 32);
			this.rb_611_2.Name = "rb_611_2";
			this.rb_611_2.Size = new System.Drawing.Size(88, 24);
			this.rb_611_2.TabIndex = 7;
			this.rb_611_2.Text = "VT 611";
			// 
			// rb_612_1
			// 
			this.rb_612_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_612_1.Location = new System.Drawing.Point(16, 64);
			this.rb_612_1.Name = "rb_612_1";
			this.rb_612_1.Size = new System.Drawing.Size(80, 24);
			this.rb_612_1.TabIndex = 3;
			this.rb_612_1.Text = "VT 612";
			this.rb_612_1.CheckedChanged += new System.EventHandler(this.rb_612_1_CheckedChanged);
			// 
			// rb_611_1
			// 
			this.rb_611_1.Checked = true;
			this.rb_611_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_611_1.Location = new System.Drawing.Point(16, 32);
			this.rb_611_1.Name = "rb_611_1";
			this.rb_611_1.Size = new System.Drawing.Size(80, 24);
			this.rb_611_1.TabIndex = 1;
			this.rb_611_1.TabStop = true;
			this.rb_611_1.Text = "VT 611";
			this.rb_611_1.CheckedChanged += new System.EventHandler(this.rb_611_1_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_612_1);
			this.groupBox1.Controls.Add(this.rb_611_1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(104, 136);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "1. Zugtyp:";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.rb_kein_3);
			this.groupBox4.Controls.Add(this.rb_612_3);
			this.groupBox4.Controls.Add(this.rb_611_3);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(264, 16);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(120, 136);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "3. Zugtyp:";
			// 
			// rb_kein_3
			// 
			this.rb_kein_3.Checked = true;
			this.rb_kein_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_3.Location = new System.Drawing.Point(16, 96);
			this.rb_kein_3.Name = "rb_kein_3";
			this.rb_kein_3.Size = new System.Drawing.Size(88, 24);
			this.rb_kein_3.TabIndex = 13;
			this.rb_kein_3.TabStop = true;
			this.rb_kein_3.Text = "<kein Zug>";
			// 
			// rb_612_3
			// 
			this.rb_612_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_612_3.Location = new System.Drawing.Point(16, 64);
			this.rb_612_3.Name = "rb_612_3";
			this.rb_612_3.Size = new System.Drawing.Size(88, 24);
			this.rb_612_3.TabIndex = 8;
			this.rb_612_3.Text = "VT 612";
			// 
			// rb_611_3
			// 
			this.rb_611_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_611_3.Location = new System.Drawing.Point(16, 32);
			this.rb_611_3.Name = "rb_611_3";
			this.rb_611_3.Size = new System.Drawing.Size(88, 24);
			this.rb_611_3.TabIndex = 7;
			this.rb_611_3.Text = "VT 611";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rb_kein_4);
			this.groupBox2.Controls.Add(this.rb_612_4);
			this.groupBox2.Controls.Add(this.rb_611_4);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(400, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(120, 136);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "3. Zugtyp:";
			// 
			// rb_kein_4
			// 
			this.rb_kein_4.Checked = true;
			this.rb_kein_4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_4.Location = new System.Drawing.Point(16, 96);
			this.rb_kein_4.Name = "rb_kein_4";
			this.rb_kein_4.Size = new System.Drawing.Size(88, 24);
			this.rb_kein_4.TabIndex = 13;
			this.rb_kein_4.TabStop = true;
			this.rb_kein_4.Text = "<kein Zug>";
			// 
			// rb_612_4
			// 
			this.rb_612_4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_612_4.Location = new System.Drawing.Point(16, 64);
			this.rb_612_4.Name = "rb_612_4";
			this.rb_612_4.Size = new System.Drawing.Size(88, 24);
			this.rb_612_4.TabIndex = 8;
			this.rb_612_4.Text = "VT 612";
			// 
			// rb_611_4
			// 
			this.rb_611_4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_611_4.Location = new System.Drawing.Point(16, 32);
			this.rb_611_4.Name = "rb_611_4";
			this.rb_611_4.Size = new System.Drawing.Size(88, 24);
			this.rb_611_4.TabIndex = 7;
			this.rb_611_4.Text = "VT 611";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(538, 239);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox4);
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
			this.Text = "ET42X Wahl";
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_611_1.Checked)
			{
				m_state.VT612type1 = VT612TYPE.VT611;
			}
			else if (rb_612_1.Checked)
			{
				m_state.VT612type1 = VT612TYPE.VT612;
			}

			if (rb_611_2.Checked)
			{
				m_state.VT612type2 = VT612TYPE.VT611;
			}
			else if (rb_612_2.Checked)
			{
				m_state.VT612type2 = VT612TYPE.VT612;
			}
			else
			{
				m_state.VT612type3 = VT612TYPE.NONE;
			}

			if (rb_611_3.Checked)
			{
				m_state.VT612type3 = VT612TYPE.VT611;
			}
			else if (rb_612_3.Checked)
			{
				m_state.VT612type3 = VT612TYPE.VT612;
			}
			else
			{
				m_state.VT612type3 = VT612TYPE.NONE;
			}

			if (rb_611_4.Checked)
			{
				m_state.VT612type4 = VT612TYPE.VT611;
			}
			else if (rb_612_4.Checked)
			{
				m_state.VT612type4 = VT612TYPE.VT612;
			}
			else
			{
				m_state.VT612type4 = VT612TYPE.NONE;
			}                                       

			m_state.addtionalhours = cb_12std.Checked;
		}

		private void SetVT611(bool val)
		{
			rb_611_2.Enabled = val;
			rb_611_3.Enabled = val;
			rb_611_4.Enabled = val;
			if (!val && rb_611_2.Checked) rb_kein_2.Checked = true;
			if (!val && rb_611_3.Checked) rb_kein_3.Checked = true;
			if (!val && rb_611_4.Checked) rb_kein_4.Checked = true;
		}

		private void SetVT612(bool val)
		{
			rb_612_2.Enabled = val;
			rb_612_3.Enabled = val;
			rb_612_4.Enabled = val;
			if (!val && rb_612_2.Checked) rb_kein_2.Checked = true;
			if (!val && rb_612_3.Checked) rb_kein_3.Checked = true;
			if (!val && rb_612_4.Checked) rb_kein_4.Checked = true;
		}

		private void rb_611_1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			SetVT611(true);
			SetVT612(false);
			this.ResumeLayout();
		}

		private void rb_612_1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			SetVT611(false);
			SetVT612(true);
			this.ResumeLayout();
		}

	}
}
