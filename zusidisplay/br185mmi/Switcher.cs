using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.MMIBR185
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
		private System.Windows.Forms.RadioButton rb_189;
		private System.Windows.Forms.RadioButton rb_146_1;
		private System.Windows.Forms.RadioButton rb_766_1;
		private System.Windows.Forms.RadioButton rb_185;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rb_kein;
		private System.Windows.Forms.RadioButton rb_SAT;
		private System.Windows.Forms.CheckBox cb_12std;
		private System.Windows.Forms.CheckBox cb_Showclock;
		private System.Windows.Forms.Label l_trainControl;

		MMI.EBuLa.Tools.State m_state;

		public Switcher(ref MMI.EBuLa.Tools.State s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			if (s.TrainType == TRAIN_TYPE.BR146_1)
				rb_146_1.Checked = true;
			else if (s.TrainType == TRAIN_TYPE.DBpzfa766_1)
				rb_766_1.Checked = true;
			else if (s.TrainType == TRAIN_TYPE.BR185)
				rb_185.Checked = true;
			else if (s.TrainType == TRAIN_TYPE.BR189)
				rb_189.Checked = true;

			//if (s.Türschliesung == TÜRSCHlIESSUNG.TB0)
			//	rb_TB0.Checked = true;
			//else if (s.Türschliesung == TÜRSCHlIESSUNG.ICE)
			//	rb_ICE.Checked = true;
			if (s.Türschliesung == TÜRSCHlIESSUNG.SAT)
				rb_SAT.Checked = true;

			cb_12std.Checked = m_state.addtionalhours;
			cb_Showclock.Checked = m_state.SHOW_CLOCK;

			string zbf = Zugbeinflussung(s.PZB_System);
			if (s.LM_GNT_B || s.LM_GNT_Ü) zbf += " und ZUB 122";
			l_trainControl.Text = "Aktive Zugbeinflussung: "+zbf;

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
			this.rb_185 = new System.Windows.Forms.RadioButton();
			this.rb_766_1 = new System.Windows.Forms.RadioButton();
			this.rb_146_1 = new System.Windows.Forms.RadioButton();
			this.rb_189 = new System.Windows.Forms.RadioButton();
			this.b_ok = new System.Windows.Forms.Button();
			this.b_cancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rb_SAT = new System.Windows.Forms.RadioButton();
			this.rb_kein = new System.Windows.Forms.RadioButton();
			this.cb_12std = new System.Windows.Forms.CheckBox();
			this.cb_Showclock = new System.Windows.Forms.CheckBox();
			this.l_trainControl = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_185);
			this.groupBox1.Controls.Add(this.rb_766_1);
			this.groupBox1.Controls.Add(this.rb_146_1);
			this.groupBox1.Controls.Add(this.rb_189);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(256, 168);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bitte wählen Sie den gewünschten Loktyp aus:";
			// 
			// rb_185
			// 
			this.rb_185.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_185.Location = new System.Drawing.Point(16, 64);
			this.rb_185.Name = "rb_185";
			this.rb_185.Size = new System.Drawing.Size(168, 24);
			this.rb_185.TabIndex = 3;
			this.rb_185.Text = "BR 185 (Vmax=140 + AFB)";
			// 
			// rb_766_1
			// 
			this.rb_766_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_766_1.Location = new System.Drawing.Point(16, 128);
			this.rb_766_1.Name = "rb_766_1";
			this.rb_766_1.Size = new System.Drawing.Size(208, 24);
			this.rb_766_1.TabIndex = 2;
			this.rb_766_1.Text = "DBpzfa 766.1 (Vmax=160 + Fahrstufen)";
			// 
			// rb_146_1
			// 
			this.rb_146_1.Checked = true;
			this.rb_146_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_146_1.Location = new System.Drawing.Point(16, 32);
			this.rb_146_1.Name = "rb_146_1";
			this.rb_146_1.Size = new System.Drawing.Size(168, 24);
			this.rb_146_1.TabIndex = 1;
			this.rb_146_1.TabStop = true;
			this.rb_146_1.Text = "BR 146.1 (Vmax=160 + AFB)";
			// 
			// rb_189
			// 
			this.rb_189.Enabled = false;
			this.rb_189.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_189.Location = new System.Drawing.Point(16, 96);
			this.rb_189.Name = "rb_189";
			this.rb_189.Size = new System.Drawing.Size(176, 24);
			this.rb_189.TabIndex = 0;
			this.rb_189.Text = "BR 189";
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(240, 248);
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
			this.b_cancel.Location = new System.Drawing.Point(240, 32);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 3;
			this.b_cancel.Text = "Abbrechen";
			this.b_cancel.Visible = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rb_SAT);
			this.groupBox2.Controls.Add(this.rb_kein);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(296, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(256, 72);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Bitte wählen Sie die Türschließeinrichtung aus:";
			// 
			// rb_SAT
			// 
			this.rb_SAT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_SAT.Location = new System.Drawing.Point(144, 32);
			this.rb_SAT.Name = "rb_SAT";
			this.rb_SAT.Size = new System.Drawing.Size(80, 24);
			this.rb_SAT.TabIndex = 5;
			this.rb_SAT.Text = "SAT/TAV";
			// 
			// rb_kein
			// 
			this.rb_kein.Checked = true;
			this.rb_kein.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein.Location = new System.Drawing.Point(32, 32);
			this.rb_kein.Name = "rb_kein";
			this.rb_kein.Size = new System.Drawing.Size(80, 24);
			this.rb_kein.TabIndex = 2;
			this.rb_kein.TabStop = true;
			this.rb_kein.Text = "Keine / TB0";
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(296, 96);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(232, 24);
			this.cb_12std.TabIndex = 6;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// cb_Showclock
			// 
			this.cb_Showclock.Checked = true;
			this.cb_Showclock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cb_Showclock.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_Showclock.Location = new System.Drawing.Point(296, 120);
			this.cb_Showclock.Name = "cb_Showclock";
			this.cb_Showclock.Size = new System.Drawing.Size(232, 24);
			this.cb_Showclock.TabIndex = 7;
			this.cb_Showclock.Text = "Digitaluhr anzeigen";
			this.cb_Showclock.CheckedChanged += new System.EventHandler(this.cb_Showclock_CheckedChanged);
			// 
			// l_trainControl
			// 
			this.l_trainControl.Location = new System.Drawing.Point(16, 200);
			this.l_trainControl.Name = "l_trainControl";
			this.l_trainControl.Size = new System.Drawing.Size(520, 23);
			this.l_trainControl.TabIndex = 8;
			this.l_trainControl.Text = "Aktive Zugbeinflussung: (keine)";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(562, 280);
			this.Controls.Add(this.l_trainControl);
			this.Controls.Add(this.cb_Showclock);
			this.Controls.Add(this.cb_12std);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.b_cancel);
			this.Controls.Add(this.b_ok);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Switcher";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lok Wahl";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_146_1.Checked)
			{
				m_state.TrainType = TRAIN_TYPE.BR146_1;
			}
			else if(rb_766_1.Checked)
			{
				m_state.TrainType = TRAIN_TYPE.DBpzfa766_1;
			}
			else if (rb_185.Checked)
			{
				m_state.TrainType = TRAIN_TYPE.BR185;
			}
			else
			{
				m_state.TrainType = TRAIN_TYPE.BR189;
			}

			// Türschließeinrichtung
			if (rb_kein.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.KEINE;
			}
			//else if (rb_TB0.Checked)
			//{
			//	m_state.Türschliesung = TÜRSCHlIESSUNG.TB0;
			//}
			//else if (rb_ICE.Checked)
			//{
			//	m_state.Türschliesung = TÜRSCHlIESSUNG.ICE;
			//}
			else if (rb_SAT.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.SAT;
			}
			else
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.KEINE;
			}

			m_state.addtionalhours = cb_12std.Checked;
			m_state.SHOW_CLOCK = cb_Showclock.Checked;
		}

		private void cb_Showclock_CheckedChanged(object sender, System.EventArgs e)
		{
			cb_12std.Enabled = cb_Showclock.Checked;
		}

		private string Zugbeinflussung(ZUGBEEINFLUSSUNG zbf)
		{
			switch (zbf)
			{
				case ZUGBEEINFLUSSUNG.I54:
					return "Indusi 54";
				case ZUGBEEINFLUSSUNG.I60:
					return "Indusi 60";
				case ZUGBEEINFLUSSUNG.I60R:
					return "Indusi 60R";
				case ZUGBEEINFLUSSUNG.LZB80_I80:
					return "LZB 80 / I80";
				case ZUGBEEINFLUSSUNG.PZ80:
					return "PZ 80";
				case ZUGBEEINFLUSSUNG.PZ80R:
					return "PZ 80R";
				case ZUGBEEINFLUSSUNG.PZB90_15:
					return "PZB 90 Version 1.5";
				case ZUGBEEINFLUSSUNG.PZB90_16:
					return "PZB 90 Version 1.6";
				case ZUGBEEINFLUSSUNG.SIGNUM:
					return "SBB-SIGNUM INTEGRA";
				case ZUGBEEINFLUSSUNG.NONE:
					return "(keine)";
				default:
					return "(unbekannt)";
			}
		}
	}
}
