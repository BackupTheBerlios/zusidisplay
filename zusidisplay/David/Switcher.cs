using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.DAVID
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
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rb_kein;
		private System.Windows.Forms.RadioButton rb_ICE;
		private System.Windows.Forms.RadioButton rb_TB0;
		private System.Windows.Forms.RadioButton rb_SAT;
		private System.Windows.Forms.RadioButton rb_ICE1;
		private System.Windows.Forms.RadioButton rb_ICE2_DT;
		private System.Windows.Forms.RadioButton rb_ICE2_ET_TK;
		private System.Windows.Forms.RadioButton rb_ICE2_ET_STWG;
		private System.Windows.Forms.CheckBox cb_12std;

		MMI.DAVID.DavidState m_state;

		public Switcher(ref MMI.DAVID.DavidState s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			if (s.ICEtype == ICETYPE.ICE1)
				rb_ICE1.Checked = true;
			else if (s.ICEtype == ICETYPE.ICE2_ET)
				rb_ICE2_ET_TK.Checked = true;
			else if (s.ICEtype == ICETYPE.ICE2_DT)
				rb_ICE2_DT.Checked = true;

/*			if (s.Türschliesung == TÜRSCHlIESSUNG.TB0)
				rb_TB0.Checked = true;
			else if (s.Türschliesung == TÜRSCHlIESSUNG.ICE)
				rb_ICE.Checked = true;
			else if (s.Türschliesung == TÜRSCHlIESSUNG.SAT)
				rb_SAT.Checked = true;*/

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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rb_ICE2_ET_STWG = new System.Windows.Forms.RadioButton();
			this.rb_ICE2_ET_TK = new System.Windows.Forms.RadioButton();
			this.rb_ICE1 = new System.Windows.Forms.RadioButton();
			this.rb_ICE2_DT = new System.Windows.Forms.RadioButton();
			this.b_ok = new System.Windows.Forms.Button();
			this.b_cancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rb_SAT = new System.Windows.Forms.RadioButton();
			this.rb_TB0 = new System.Windows.Forms.RadioButton();
			this.rb_ICE = new System.Windows.Forms.RadioButton();
			this.rb_kein = new System.Windows.Forms.RadioButton();
			this.cb_12std = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_ICE2_ET_STWG);
			this.groupBox1.Controls.Add(this.rb_ICE2_ET_TK);
			this.groupBox1.Controls.Add(this.rb_ICE1);
			this.groupBox1.Controls.Add(this.rb_ICE2_DT);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(256, 168);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bitte wählen Sie den gewünschten Zugtyp aus:";
			// 
			// rb_ICE2_ET_STWG
			// 
			this.rb_ICE2_ET_STWG.Enabled = false;
			this.rb_ICE2_ET_STWG.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_ICE2_ET_STWG.Location = new System.Drawing.Point(16, 96);
			this.rb_ICE2_ET_STWG.Name = "rb_ICE2_ET_STWG";
			this.rb_ICE2_ET_STWG.Size = new System.Drawing.Size(168, 24);
			this.rb_ICE2_ET_STWG.TabIndex = 4;
			this.rb_ICE2_ET_STWG.Text = "ICE 2 (Stwg+6Wg+Tk)";
			// 
			// rb_ICE2_ET_TK
			// 
			this.rb_ICE2_ET_TK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_ICE2_ET_TK.Location = new System.Drawing.Point(16, 64);
			this.rb_ICE2_ET_TK.Name = "rb_ICE2_ET_TK";
			this.rb_ICE2_ET_TK.Size = new System.Drawing.Size(168, 24);
			this.rb_ICE2_ET_TK.TabIndex = 3;
			this.rb_ICE2_ET_TK.Text = "ICE 2 (Tk+6Wg+Stwg)";
			// 
			// rb_ICE1
			// 
			this.rb_ICE1.Checked = true;
			this.rb_ICE1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_ICE1.Location = new System.Drawing.Point(16, 32);
			this.rb_ICE1.Name = "rb_ICE1";
			this.rb_ICE1.Size = new System.Drawing.Size(168, 24);
			this.rb_ICE1.TabIndex = 1;
			this.rb_ICE1.TabStop = true;
			this.rb_ICE1.Text = "ICE 1 (Tk1+12Wg+Tk2)";
			// 
			// rb_ICE2_DT
			// 
			this.rb_ICE2_DT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_ICE2_DT.Location = new System.Drawing.Point(16, 128);
			this.rb_ICE2_DT.Name = "rb_ICE2_DT";
			this.rb_ICE2_DT.Size = new System.Drawing.Size(232, 24);
			this.rb_ICE2_DT.TabIndex = 0;
			this.rb_ICE2_DT.Text = "ICE 2 (Tk1+6Wg+Stwg1+Stwg2+6Wg+Tk2)";
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(48, 344);
			this.b_ok.Name = "b_ok";
			this.b_ok.TabIndex = 2;
			this.b_ok.Text = "OK";
			this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(152, 344);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 3;
			this.b_cancel.Text = "Abbrechen";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rb_SAT);
			this.groupBox2.Controls.Add(this.rb_TB0);
			this.groupBox2.Controls.Add(this.rb_ICE);
			this.groupBox2.Controls.Add(this.rb_kein);
			this.groupBox2.Enabled = false;
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(16, 232);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(256, 96);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Bitte wählen Sie die Türschließeinrichtung aus:";
			this.groupBox2.Visible = false;
			// 
			// rb_SAT
			// 
			this.rb_SAT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_SAT.Location = new System.Drawing.Point(144, 56);
			this.rb_SAT.Name = "rb_SAT";
			this.rb_SAT.Size = new System.Drawing.Size(80, 24);
			this.rb_SAT.TabIndex = 5;
			this.rb_SAT.Text = "SAT/TAV";
			// 
			// rb_TB0
			// 
			this.rb_TB0.Enabled = false;
			this.rb_TB0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_TB0.Location = new System.Drawing.Point(32, 56);
			this.rb_TB0.Name = "rb_TB0";
			this.rb_TB0.Size = new System.Drawing.Size(80, 24);
			this.rb_TB0.TabIndex = 4;
			this.rb_TB0.Text = "TB0";
			// 
			// rb_ICE
			// 
			this.rb_ICE.Enabled = false;
			this.rb_ICE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_ICE.Location = new System.Drawing.Point(144, 32);
			this.rb_ICE.Name = "rb_ICE";
			this.rb_ICE.Size = new System.Drawing.Size(80, 24);
			this.rb_ICE.TabIndex = 3;
			this.rb_ICE.Text = "ICE";
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
			this.rb_kein.Text = "Keine";
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(32, 192);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(232, 24);
			this.cb_12std.TabIndex = 5;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 384);
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
			this.Text = "ICE Wahl";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_ICE1.Checked)
			{
				m_state.ICEtype = ICETYPE.ICE1;
			}
			else if(rb_ICE2_ET_TK.Checked)
			{
				m_state.ICEtype = ICETYPE.ICE2_ET;
			}
			else if (rb_ICE2_DT.Checked)
			{
				m_state.ICEtype = ICETYPE.ICE2_DT;
			}

			m_state.addtionalhours = cb_12std.Checked;

			/*
			// Türschließeinrichtung
			if (rb_kein.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.KEINE;
			}
			else if (rb_TB0.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.TB0;
			}
			else if (rb_ICE.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.ICE;
			}
			else if (rb_SAT.Checked)
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.SAT;
			}
			else
			{
				m_state.Türschliesung = TÜRSCHlIESSUNG.KEINE;
			}  */
		}
	}
}
