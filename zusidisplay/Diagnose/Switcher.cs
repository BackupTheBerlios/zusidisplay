using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.DIAGNOSE
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
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rb_double;
		private System.Windows.Forms.RadioButton rb_4times;
		private System.Windows.Forms.RadioButton rb_single;
		private System.Windows.Forms.RadioButton rb_3times;
		private System.Windows.Forms.RadioButton rb_101;
		private System.Windows.Forms.RadioButton rb_185;
		private System.Windows.Forms.RadioButton rb_145;
		private System.Windows.Forms.RadioButton rb_146;
		private System.Windows.Forms.RadioButton rb_182;
		private System.Windows.Forms.RadioButton rb_152;
		private System.Windows.Forms.RadioButton rb_189;
		private System.Windows.Forms.RadioButton rb_er20;
		private System.Windows.Forms.RadioButton rb_146_1;
		private System.Windows.Forms.RadioButton rb_101MET;

		MMI.DIAGNOSE.DIAGNOSEState m_state;

		public Switcher(ref MMI.DIAGNOSE.DIAGNOSEState s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			switch(m_state.type)
			{
				case TRAIN_TYPE.BR101:
					rb_101.Checked = true;
					break;
				case TRAIN_TYPE.BR101_MET:
					rb_101MET.Checked = true;
					break;
				case TRAIN_TYPE.BR145:
					rb_145.Checked = true;
					break;
				case TRAIN_TYPE.BR146:
					rb_146.Checked = true;
					break;
				case TRAIN_TYPE.BR146_1:
					rb_146_1.Checked = true;
					break;
				case TRAIN_TYPE.BR152:
					rb_152.Checked = true;
					break;
				case TRAIN_TYPE.BR182:
					rb_182.Checked = true;
					break;
				case TRAIN_TYPE.BR185:
					rb_185.Checked = true;
					break;
				case TRAIN_TYPE.BR189:
					rb_189.Checked = true;
					break;
				case TRAIN_TYPE.ER20:
					rb_er20.Checked = true;
					break;
			}

			switch(m_state.traction)
			{
				case 1:
					rb_single.Checked = true;
					break;
				case 2:
					rb_double.Checked = true;
					break;
				case 3:
					rb_3times.Checked = true;
					break;
				case 4:
					rb_4times.Checked = true;
					break;
			}
			
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
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.rb_101MET = new System.Windows.Forms.RadioButton();
			this.rb_182 = new System.Windows.Forms.RadioButton();
			this.rb_152 = new System.Windows.Forms.RadioButton();
			this.rb_189 = new System.Windows.Forms.RadioButton();
			this.rb_er20 = new System.Windows.Forms.RadioButton();
			this.rb_101 = new System.Windows.Forms.RadioButton();
			this.rb_185 = new System.Windows.Forms.RadioButton();
			this.rb_145 = new System.Windows.Forms.RadioButton();
			this.rb_146 = new System.Windows.Forms.RadioButton();
			this.rb_146_1 = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rb_double = new System.Windows.Forms.RadioButton();
			this.rb_4times = new System.Windows.Forms.RadioButton();
			this.rb_single = new System.Windows.Forms.RadioButton();
			this.rb_3times = new System.Windows.Forms.RadioButton();
			this.groupBox5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(104, 448);
			this.b_ok.Name = "b_ok";
			this.b_ok.TabIndex = 3;
			this.b_ok.Text = "OK";
			this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(216, 448);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 4;
			this.b_cancel.Text = "Abbrechen";
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(16, 408);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(192, 24);
			this.cb_12std.TabIndex = 2;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.rb_101MET);
			this.groupBox5.Controls.Add(this.rb_182);
			this.groupBox5.Controls.Add(this.rb_152);
			this.groupBox5.Controls.Add(this.rb_189);
			this.groupBox5.Controls.Add(this.rb_er20);
			this.groupBox5.Controls.Add(this.rb_101);
			this.groupBox5.Controls.Add(this.rb_185);
			this.groupBox5.Controls.Add(this.rb_145);
			this.groupBox5.Controls.Add(this.rb_146);
			this.groupBox5.Controls.Add(this.rb_146_1);
			this.groupBox5.Controls.Add(this.groupBox3);
			this.groupBox5.Controls.Add(this.groupBox2);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(16, 16);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(248, 376);
			this.groupBox5.TabIndex = 0;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Bitte wählen Sie den gewünschten Loktyp aus:";
			// 
			// rb_101MET
			// 
			this.rb_101MET.Enabled = false;
			this.rb_101MET.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_101MET.Location = new System.Drawing.Point(128, 56);
			this.rb_101MET.Name = "rb_101MET";
			this.rb_101MET.Size = new System.Drawing.Size(88, 24);
			this.rb_101MET.TabIndex = 15;
			this.rb_101MET.Text = "BR 101 (MET)";
			// 
			// rb_182
			// 
			this.rb_182.Enabled = false;
			this.rb_182.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_182.Location = new System.Drawing.Point(40, 256);
			this.rb_182.Name = "rb_182";
			this.rb_182.Size = new System.Drawing.Size(168, 24);
			this.rb_182.TabIndex = 7;
			this.rb_182.Text = "BR 182 / ES 64U2";
			// 
			// rb_152
			// 
			this.rb_152.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_152.Location = new System.Drawing.Point(40, 224);
			this.rb_152.Name = "rb_152";
			this.rb_152.Size = new System.Drawing.Size(168, 24);
			this.rb_152.TabIndex = 6;
			this.rb_152.Text = "BR 152 / ES 64F2";
			// 
			// rb_189
			// 
			this.rb_189.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_189.Location = new System.Drawing.Point(40, 288);
			this.rb_189.Name = "rb_189";
			this.rb_189.Size = new System.Drawing.Size(176, 24);
			this.rb_189.TabIndex = 8;
			this.rb_189.Text = "BR 189 / ES 64F4";
			// 
			// rb_er20
			// 
			this.rb_er20.Enabled = false;
			this.rb_er20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_er20.Location = new System.Drawing.Point(40, 320);
			this.rb_er20.Name = "rb_er20";
			this.rb_er20.Size = new System.Drawing.Size(176, 24);
			this.rb_er20.TabIndex = 9;
			this.rb_er20.Text = "ER 20";
			// 
			// rb_101
			// 
			this.rb_101.Checked = true;
			this.rb_101.Enabled = false;
			this.rb_101.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_101.Location = new System.Drawing.Point(40, 56);
			this.rb_101.Name = "rb_101";
			this.rb_101.Size = new System.Drawing.Size(72, 24);
			this.rb_101.TabIndex = 1;
			this.rb_101.TabStop = true;
			this.rb_101.Text = "BR 101";
			// 
			// rb_185
			// 
			this.rb_185.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_185.Location = new System.Drawing.Point(40, 152);
			this.rb_185.Name = "rb_185";
			this.rb_185.Size = new System.Drawing.Size(168, 24);
			this.rb_185.TabIndex = 4;
			this.rb_185.Text = "BR 185";
			// 
			// rb_145
			// 
			this.rb_145.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_145.Location = new System.Drawing.Point(40, 88);
			this.rb_145.Name = "rb_145";
			this.rb_145.Size = new System.Drawing.Size(168, 24);
			this.rb_145.TabIndex = 2;
			this.rb_145.Text = "BR 145";
			// 
			// rb_146
			// 
			this.rb_146.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_146.Location = new System.Drawing.Point(40, 120);
			this.rb_146.Name = "rb_146";
			this.rb_146.Size = new System.Drawing.Size(80, 24);
			this.rb_146.TabIndex = 3;
			this.rb_146.Text = "BR 146";
			// 
			// rb_146_1
			// 
			this.rb_146_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_146_1.Location = new System.Drawing.Point(128, 120);
			this.rb_146_1.Name = "rb_146_1";
			this.rb_146_1.Size = new System.Drawing.Size(80, 24);
			this.rb_146_1.TabIndex = 11;
			this.rb_146_1.Text = "BR 146.1";
			// 
			// groupBox3
			// 
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(24, 200);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(196, 160);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Siemens:";
			// 
			// groupBox2
			// 
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(24, 32);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 152);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "ADtranz/Bombardier:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_double);
			this.groupBox1.Controls.Add(this.rb_4times);
			this.groupBox1.Controls.Add(this.rb_single);
			this.groupBox1.Controls.Add(this.rb_3times);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(280, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(104, 168);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Traktionsart:";
			// 
			// rb_double
			// 
			this.rb_double.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_double.Location = new System.Drawing.Point(16, 64);
			this.rb_double.Name = "rb_double";
			this.rb_double.Size = new System.Drawing.Size(80, 24);
			this.rb_double.TabIndex = 1;
			this.rb_double.Text = "doppel";
			// 
			// rb_4times
			// 
			this.rb_4times.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_4times.Location = new System.Drawing.Point(16, 128);
			this.rb_4times.Name = "rb_4times";
			this.rb_4times.Size = new System.Drawing.Size(80, 24);
			this.rb_4times.TabIndex = 3;
			this.rb_4times.Text = "4-fach";
			// 
			// rb_single
			// 
			this.rb_single.Checked = true;
			this.rb_single.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_single.Location = new System.Drawing.Point(16, 32);
			this.rb_single.Name = "rb_single";
			this.rb_single.Size = new System.Drawing.Size(80, 24);
			this.rb_single.TabIndex = 0;
			this.rb_single.TabStop = true;
			this.rb_single.Text = "einfach";
			// 
			// rb_3times
			// 
			this.rb_3times.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_3times.Location = new System.Drawing.Point(16, 96);
			this.rb_3times.Name = "rb_3times";
			this.rb_3times.Size = new System.Drawing.Size(80, 24);
			this.rb_3times.TabIndex = 2;
			this.rb_3times.Text = "3-fach";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(402, 487);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.cb_12std);
			this.Controls.Add(this.b_cancel);
			this.Controls.Add(this.b_ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Switcher";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Triebfahrzeug Wahl";
			this.groupBox5.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_101.Checked)
			{
				m_state.type = TRAIN_TYPE.BR101;
			}
			if (rb_101MET.Checked)
			{
				m_state.type = TRAIN_TYPE.BR101_MET;
			}
			else if (rb_145.Checked)
			{
				m_state.type = TRAIN_TYPE.BR145;
			}
			else if (rb_146.Checked)
			{
				m_state.type = TRAIN_TYPE.BR146;
			}
			else if (rb_146_1.Checked)
			{
				m_state.type = TRAIN_TYPE.BR146_1;
			}
			else if (rb_152.Checked)
			{
				m_state.type = TRAIN_TYPE.BR152;
			}
			else if (rb_182.Checked)
			{
				m_state.type = TRAIN_TYPE.BR182;
			}
			else if (rb_185.Checked)
			{
				m_state.type = TRAIN_TYPE.BR185;
			}
			else if (rb_189.Checked)
			{
				m_state.type = TRAIN_TYPE.BR189;
			}
			else if (rb_er20.Checked)
			{
				m_state.type = TRAIN_TYPE.ER20;
			}

			if (rb_single.Checked)
				m_state.traction = 1;
			else if (rb_double.Checked)
				m_state.traction = 2;
			else if (rb_3times.Checked)
				m_state.traction = 3;
			else if (rb_4times.Checked)
				m_state.traction = 4;
            
			m_state.addtionalhours = cb_12std.Checked;
		}
	}
}
