using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	/// <summary>
	/// Zusammenfassung für ControlSwitch.
	/// </summary>
	public class ControlSwitch : System.Windows.Forms.Form, KeyHandlerInterface
	{
        Control c = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Page_Ahead;
        private System.Windows.Forms.RadioButton Page_Back;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox L_Versp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button B_E;
        private System.Windows.Forms.Button B_C;
        private System.Windows.Forms.RadioButton radioButton3;

		bool Ahead = false;
		bool Back = false;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ControlSwitch(Control control)
		{
			InitializeComponent();

            c = control;
            L_Versp.Text = c.verspaetung.ToString();


            if (c.timer_disabled)
            {
                radioButton1.Enabled = false;
                radioButton1.Text = "Zusi-km (Kein Zusi!)";
				radioButton3.Checked = c.move_via_time;
            }
            else
            {
                radioButton1.Checked = c.timer_on;
				radioButton3.Checked = c.move_via_time;
            }

            if (control.inverse)
            {
                //switch to inverse
                this.BackColor = System.Drawing.Color.LightSlateGray;
                this.ForeColor = System.Drawing.Color.White;
            }

			this.TopMost = c.XMLConf.TopMost;
			Page_Ahead.Focus();
			Ahead = true;
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
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Page_Back = new System.Windows.Forms.RadioButton();
			this.Page_Ahead = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.L_Versp = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.B_E = new System.Windows.Forms.Button();
			this.B_C = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "Wählen Sie die Verspätung und den Blätterungsmodus aus";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.Page_Back);
			this.groupBox1.Controls.Add(this.Page_Ahead);
			this.groupBox1.Location = new System.Drawing.Point(24, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 88);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Blättern:";
			// 
			// Page_Back
			// 
			this.Page_Back.Location = new System.Drawing.Point(16, 48);
			this.Page_Back.Name = "Page_Back";
			this.Page_Back.TabIndex = 1;
			this.Page_Back.Text = "Zurück";
			this.Page_Back.CheckedChanged += new System.EventHandler(this.Page_Back_CheckedChanged);
			// 
			// Page_Ahead
			// 
			this.Page_Ahead.Checked = true;
			this.Page_Ahead.Location = new System.Drawing.Point(16, 24);
			this.Page_Ahead.Name = "Page_Ahead";
			this.Page_Ahead.TabIndex = 0;
			this.Page_Ahead.TabStop = true;
			this.Page_Ahead.Text = "Vor";
			this.Page_Ahead.CheckedChanged += new System.EventHandler(this.Page_Ahead_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.L_Versp);
			this.groupBox2.Location = new System.Drawing.Point(24, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(176, 64);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Verspätung:";
			this.groupBox2.Enter += new System.EventHandler(this.groupBox3_Enter);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(88, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Minuten";
			// 
			// L_Versp
			// 
			this.L_Versp.Location = new System.Drawing.Point(16, 24);
			this.L_Versp.Name = "L_Versp";
			this.L_Versp.Size = new System.Drawing.Size(64, 20);
			this.L_Versp.TabIndex = 0;
			this.L_Versp.Text = "";
			this.L_Versp.TextChanged += new System.EventHandler(this.L_Versp_TextChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioButton3);
			this.groupBox3.Controls.Add(this.radioButton2);
			this.groupBox3.Controls.Add(this.radioButton1);
			this.groupBox3.Location = new System.Drawing.Point(24, 208);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(176, 100);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Blättermodus:";
			this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(16, 40);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.TabIndex = 1;
			this.radioButton3.Text = "Zeit";
			// 
			// radioButton2
			// 
			this.radioButton2.Checked = true;
			this.radioButton2.Location = new System.Drawing.Point(16, 64);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.TabIndex = 2;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "manuell";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(16, 16);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(152, 24);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.Text = "Zusi-km (Registry)";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 312);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(184, 40);
			this.label3.TabIndex = 4;
			this.label3.Text = "Bestätigen mit E, Abbrechen mit C und Auswahl mit Pfeiltasten";
			// 
			// B_E
			// 
			this.B_E.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.B_E.Location = new System.Drawing.Point(216, 272);
			this.B_E.Name = "B_E";
			this.B_E.Size = new System.Drawing.Size(32, 32);
			this.B_E.TabIndex = 6;
			this.B_E.Text = "E";
			this.B_E.Click += new System.EventHandler(this.B_E_Click);
			// 
			// B_C
			// 
			this.B_C.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.B_C.Location = new System.Drawing.Point(216, 88);
			this.B_C.Name = "B_C";
			this.B_C.Size = new System.Drawing.Size(32, 32);
			this.B_C.TabIndex = 5;
			this.B_C.Text = "C";
			this.B_C.Click += new System.EventHandler(this.B_C_Click);
			// 
			// ControlSwitch
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(266, 376);
			this.ControlBox = false;
			this.Controls.Add(this.B_C);
			this.Controls.Add(this.B_E);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ControlSwitch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Steuerungsart";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        public void B_E_Click(object sender, System.EventArgs e)
        {

            try
            {
				if (Ahead)
				{
					c.NextEntry();
				}
				else if (Back)
				{
					c.PrevEntry();
				}
				else
				{
					c.verspaetung = Convert.ToInt32(L_Versp.Text);

					c.timer_on = radioButton1.Checked;
					c.move_via_time = radioButton3.Checked;
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Falsche Eingabe");
            }
        }

        public void B_C_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void L_Versp_TextChanged(object sender, System.EventArgs e)
        {
			Ahead = false;
			Back = false;

            int i_e = L_Versp.Text.IndexOf("e");

            if (i_e != -1)
            {
                if (i_e == 0)
                {
                    L_Versp.Text = "";
                }
                else
                {
                    L_Versp.Text = L_Versp.Text.Remove(i_e,1);
                }
                B_E_Click(sender,e);
            }
        }

		private void Page_Ahead_CheckedChanged(object sender, System.EventArgs e)
		{
			Ahead = true;
			Back = false;
		}

		private void Page_Back_CheckedChanged(object sender, System.EventArgs e)
		{
			Ahead = false;
			Back = true;
		}

		private void groupBox3_Enter(object sender, System.EventArgs e)
		{
			Ahead = false;
			Back = false;
		}
	}
}
