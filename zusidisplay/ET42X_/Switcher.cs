using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MMI.EBuLa.Tools;

namespace MMI.ET42X
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
		private System.Windows.Forms.RadioButton rb_425_2;
		private System.Windows.Forms.RadioButton rb_424_2;
		private System.Windows.Forms.RadioButton rb_423_2;
		private System.Windows.Forms.RadioButton rb_426_2;
		private System.Windows.Forms.RadioButton rb_425_1;
		private System.Windows.Forms.RadioButton rb_424_1;
		private System.Windows.Forms.RadioButton rb_423_1;
		private System.Windows.Forms.RadioButton rb_426_1;
		private System.Windows.Forms.RadioButton rb_kein_3;
		private System.Windows.Forms.RadioButton rb_425_3;
		private System.Windows.Forms.RadioButton rb_424_3;
		private System.Windows.Forms.RadioButton rb_423_3;
		private System.Windows.Forms.RadioButton rb_426_3;

		MMI.ET42X.ET42XState m_state;

		public Switcher(ref MMI.ET42X.ET42XState s, bool topMost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			m_state = s;

			if (s.ET42Xtype1 == ET42XTYPE.ET423)
			{
				rb_423_1.Checked = true;
				rb_423_1_CheckedChanged(this, new EventArgs());
			}
			else if (s.ET42Xtype1 == ET42XTYPE.ET424)
			{
				rb_424_1.Checked = true;
				rb_424_1_CheckedChanged(this, new EventArgs());
			}
			else if (s.ET42Xtype1 == ET42XTYPE.ET425)
			{
				rb_425_1.Checked = true;
				rb_425_1_CheckedChanged(this, new EventArgs());
			}
			else if (s.ET42Xtype1 == ET42XTYPE.ET426)
			{
				rb_426_1.Checked = true;
				rb_425_1_CheckedChanged(this, new EventArgs());
			}

			if (s.ET42Xtype2 == ET42XTYPE.ET423)
				rb_423_2.Checked = true;
			else if (s.ET42Xtype2 == ET42XTYPE.ET424)
				rb_424_2.Checked = true;
			else if (s.ET42Xtype2 == ET42XTYPE.ET425)
				rb_425_2.Checked = true;
			else if (s.ET42Xtype2 == ET42XTYPE.ET426)
				rb_426_2.Checked = true;
			else
				rb_kein_2.Checked = true;

			if (s.ET42Xtype3 == ET42XTYPE.ET423)
				rb_423_3.Checked = true;
			else if (s.ET42Xtype3 == ET42XTYPE.ET424)
				rb_424_3.Checked = true;
			else if (s.ET42Xtype3 == ET42XTYPE.ET425)
				rb_425_3.Checked = true;
			else if (s.ET42Xtype3 == ET42XTYPE.ET426)
				rb_426_3.Checked = true;
			else
				rb_kein_3.Checked = true;

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
			this.rb_425_2 = new System.Windows.Forms.RadioButton();
			this.rb_424_2 = new System.Windows.Forms.RadioButton();
			this.rb_423_2 = new System.Windows.Forms.RadioButton();
			this.rb_426_2 = new System.Windows.Forms.RadioButton();
			this.rb_425_1 = new System.Windows.Forms.RadioButton();
			this.rb_424_1 = new System.Windows.Forms.RadioButton();
			this.rb_423_1 = new System.Windows.Forms.RadioButton();
			this.rb_426_1 = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.rb_kein_3 = new System.Windows.Forms.RadioButton();
			this.rb_425_3 = new System.Windows.Forms.RadioButton();
			this.rb_424_3 = new System.Windows.Forms.RadioButton();
			this.rb_423_3 = new System.Windows.Forms.RadioButton();
			this.rb_426_3 = new System.Windows.Forms.RadioButton();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// b_ok
			// 
			this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_ok.Location = new System.Drawing.Point(104, 264);
			this.b_ok.Name = "b_ok";
			this.b_ok.TabIndex = 2;
			this.b_ok.Text = "OK";
			this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(216, 264);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 3;
			this.b_cancel.Text = "Abbrechen";
			// 
			// cb_12std
			// 
			this.cb_12std.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_12std.Location = new System.Drawing.Point(16, 224);
			this.cb_12std.Name = "cb_12std";
			this.cb_12std.Size = new System.Drawing.Size(192, 24);
			this.cb_12std.TabIndex = 5;
			this.cb_12std.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rb_kein_2);
			this.groupBox3.Controls.Add(this.rb_425_2);
			this.groupBox3.Controls.Add(this.rb_424_2);
			this.groupBox3.Controls.Add(this.rb_423_2);
			this.groupBox3.Controls.Add(this.rb_426_2);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(136, 16);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(120, 200);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "2. Zugtyp:";
			// 
			// rb_kein_2
			// 
			this.rb_kein_2.Checked = true;
			this.rb_kein_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_2.Location = new System.Drawing.Point(16, 160);
			this.rb_kein_2.Name = "rb_kein_2";
			this.rb_kein_2.Size = new System.Drawing.Size(88, 24);
			this.rb_kein_2.TabIndex = 13;
			this.rb_kein_2.TabStop = true;
			this.rb_kein_2.Text = "<kein Zug>";
			// 
			// rb_425_2
			// 
			this.rb_425_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_425_2.Location = new System.Drawing.Point(16, 96);
			this.rb_425_2.Name = "rb_425_2";
			this.rb_425_2.Size = new System.Drawing.Size(88, 24);
			this.rb_425_2.TabIndex = 9;
			this.rb_425_2.Text = "ET 425";
			// 
			// rb_424_2
			// 
			this.rb_424_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_424_2.Location = new System.Drawing.Point(16, 64);
			this.rb_424_2.Name = "rb_424_2";
			this.rb_424_2.Size = new System.Drawing.Size(88, 24);
			this.rb_424_2.TabIndex = 8;
			this.rb_424_2.Text = "ET 424";
			// 
			// rb_423_2
			// 
			this.rb_423_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_423_2.Location = new System.Drawing.Point(16, 32);
			this.rb_423_2.Name = "rb_423_2";
			this.rb_423_2.Size = new System.Drawing.Size(88, 24);
			this.rb_423_2.TabIndex = 7;
			this.rb_423_2.Text = "ET 423";
			// 
			// rb_426_2
			// 
			this.rb_426_2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_426_2.Location = new System.Drawing.Point(16, 128);
			this.rb_426_2.Name = "rb_426_2";
			this.rb_426_2.Size = new System.Drawing.Size(88, 24);
			this.rb_426_2.TabIndex = 6;
			this.rb_426_2.Text = "ET 426";
			// 
			// rb_425_1
			// 
			this.rb_425_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_425_1.Location = new System.Drawing.Point(16, 96);
			this.rb_425_1.Name = "rb_425_1";
			this.rb_425_1.Size = new System.Drawing.Size(80, 24);
			this.rb_425_1.TabIndex = 4;
			this.rb_425_1.Text = "ET 425";
			this.rb_425_1.CheckedChanged += new System.EventHandler(this.rb_425_1_CheckedChanged);
			// 
			// rb_424_1
			// 
			this.rb_424_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_424_1.Location = new System.Drawing.Point(16, 64);
			this.rb_424_1.Name = "rb_424_1";
			this.rb_424_1.Size = new System.Drawing.Size(80, 24);
			this.rb_424_1.TabIndex = 3;
			this.rb_424_1.Text = "ET 424";
			this.rb_424_1.CheckedChanged += new System.EventHandler(this.rb_424_1_CheckedChanged);
			// 
			// rb_423_1
			// 
			this.rb_423_1.Checked = true;
			this.rb_423_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_423_1.Location = new System.Drawing.Point(16, 32);
			this.rb_423_1.Name = "rb_423_1";
			this.rb_423_1.Size = new System.Drawing.Size(80, 24);
			this.rb_423_1.TabIndex = 1;
			this.rb_423_1.TabStop = true;
			this.rb_423_1.Text = "ET 423";
			this.rb_423_1.CheckedChanged += new System.EventHandler(this.rb_423_1_CheckedChanged);
			// 
			// rb_426_1
			// 
			this.rb_426_1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_426_1.Location = new System.Drawing.Point(16, 128);
			this.rb_426_1.Name = "rb_426_1";
			this.rb_426_1.Size = new System.Drawing.Size(80, 24);
			this.rb_426_1.TabIndex = 0;
			this.rb_426_1.Text = "ET 426";
			this.rb_426_1.Click += new System.EventHandler(this.rb_425_1_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rb_425_1);
			this.groupBox1.Controls.Add(this.rb_424_1);
			this.groupBox1.Controls.Add(this.rb_423_1);
			this.groupBox1.Controls.Add(this.rb_426_1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(104, 200);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "1. Zugtyp:";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.rb_kein_3);
			this.groupBox4.Controls.Add(this.rb_425_3);
			this.groupBox4.Controls.Add(this.rb_424_3);
			this.groupBox4.Controls.Add(this.rb_423_3);
			this.groupBox4.Controls.Add(this.rb_426_3);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(264, 16);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(120, 200);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "3. Zugtyp:";
			// 
			// rb_kein_3
			// 
			this.rb_kein_3.Checked = true;
			this.rb_kein_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_kein_3.Location = new System.Drawing.Point(16, 160);
			this.rb_kein_3.Name = "rb_kein_3";
			this.rb_kein_3.Size = new System.Drawing.Size(88, 24);
			this.rb_kein_3.TabIndex = 13;
			this.rb_kein_3.TabStop = true;
			this.rb_kein_3.Text = "<kein Zug>";
			// 
			// rb_425_3
			// 
			this.rb_425_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_425_3.Location = new System.Drawing.Point(16, 96);
			this.rb_425_3.Name = "rb_425_3";
			this.rb_425_3.Size = new System.Drawing.Size(88, 24);
			this.rb_425_3.TabIndex = 9;
			this.rb_425_3.Text = "ET 425";
			// 
			// rb_424_3
			// 
			this.rb_424_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_424_3.Location = new System.Drawing.Point(16, 64);
			this.rb_424_3.Name = "rb_424_3";
			this.rb_424_3.Size = new System.Drawing.Size(88, 24);
			this.rb_424_3.TabIndex = 8;
			this.rb_424_3.Text = "ET 424";
			// 
			// rb_423_3
			// 
			this.rb_423_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_423_3.Location = new System.Drawing.Point(16, 32);
			this.rb_423_3.Name = "rb_423_3";
			this.rb_423_3.Size = new System.Drawing.Size(88, 24);
			this.rb_423_3.TabIndex = 7;
			this.rb_423_3.Text = "ET 423";
			// 
			// rb_426_3
			// 
			this.rb_426_3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_426_3.Location = new System.Drawing.Point(16, 128);
			this.rb_426_3.Name = "rb_426_3";
			this.rb_426_3.Size = new System.Drawing.Size(88, 24);
			this.rb_426_3.TabIndex = 6;
			this.rb_426_3.Text = "ET 426";
			// 
			// Switcher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(402, 303);
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
			this.ResumeLayout(false);

		}
		#endregion

		private void b_ok_Click(object sender, System.EventArgs e)
		{
			if (rb_423_1.Checked)
			{
				m_state.ET42Xtype1 = ET42XTYPE.ET423;
			}
			else if (rb_424_1.Checked)
			{
				m_state.ET42Xtype1 = ET42XTYPE.ET424;
			}
			else if (rb_425_1.Checked)
			{
				m_state.ET42Xtype1 = ET42XTYPE.ET425;
			}
			else if (rb_426_1.Checked)
			{
				m_state.ET42Xtype1 = ET42XTYPE.ET426;
			}
			
			if (rb_423_2.Checked)
			{
				m_state.ET42Xtype2 = ET42XTYPE.ET423;
			}
			else if (rb_424_2.Checked)
			{
				m_state.ET42Xtype2 = ET42XTYPE.ET424;
			}
			else if (rb_425_2.Checked)
			{
				m_state.ET42Xtype2 = ET42XTYPE.ET425;
			}
			else if (rb_426_2.Checked)
			{
				m_state.ET42Xtype2 = ET42XTYPE.ET426;
			}
			else
			{
				m_state.ET42Xtype2 = ET42XTYPE.NONE;
			}

			if (rb_423_3.Checked)
			{
				m_state.ET42Xtype3 = ET42XTYPE.ET423;
			}
			else if (rb_424_3.Checked)
			{
				m_state.ET42Xtype3 = ET42XTYPE.ET424;
			}
			else if (rb_425_3.Checked)
			{
				m_state.ET42Xtype3 = ET42XTYPE.ET425;
			}
			else if (rb_426_3.Checked)
			{
				m_state.ET42Xtype3 = ET42XTYPE.ET426;
			}
			else
			{
				m_state.ET42Xtype3 = ET42XTYPE.NONE;
			}

			m_state.addtionalhours = cb_12std.Checked;

		}

		private void SetET423(bool val)
		{
			rb_423_2.Enabled = val;
			rb_423_3.Enabled = val;
			if (!val && rb_423_2.Checked) rb_423_2.Checked = false;
			rb_423_3.Checked = rb_423_2.Checked;
		}

		private void SetET424(bool val)
		{
			rb_424_2.Enabled = val;
			rb_424_3.Enabled = val;
			if (!val && rb_424_2.Checked) rb_424_2.Checked = false;
			rb_424_3.Checked = rb_424_2.Checked;
		}
		
		private void SetET425(bool val)
		{
			rb_425_2.Enabled = val;
			rb_425_3.Enabled = val;
			if (!val && rb_425_2.Checked) rb_425_2.Checked = false;
			if (!val && rb_425_3.Checked) rb_425_3.Checked = false;
		}
		
		private void SetET426(bool val)
		{
			rb_426_2.Enabled = val;
			rb_426_3.Enabled = val;
			if (!val && rb_426_2.Checked) rb_426_2.Checked = false;
			if (!val && rb_426_3.Checked) rb_426_3.Checked = false;
		}

		private void rb_423_1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			SetET423(true);
			SetET424(false);
			SetET425(false);
			SetET426(false);
			this.ResumeLayout();
		}

		private void rb_424_1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			SetET423(false);
			SetET424(true);
			SetET425(false);
			SetET426(false);
			this.ResumeLayout();
		}

		private void rb_425_1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.SuspendLayout();
			SetET423(false);
			SetET424(false);
			SetET425(true);
			SetET426(true);
			this.ResumeLayout();
		}

	}
}
