using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	/// <summary>
	/// Zusammenfassung für DateTimeField.
	/// </summary>
	public class DateTimeField : System.Windows.Forms.Form, KeyHandlerInterface
	{
        Control control = null;
        DateTime date = new DateTime(0);
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dateP;
        private System.Windows.Forms.NumericUpDown Sec;
        private System.Windows.Forms.NumericUpDown Min;
        private System.Windows.Forms.NumericUpDown Std;
        public  System.Windows.Forms.CheckBox cbZusiZeit;
		public System.Windows.Forms.CheckBox cbAdditionHours;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DateTimeField(DateTime dt, ref Control c)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

            date = dt;

            control = c;
            dateP.Value = date;

			cbAdditionHours.Checked = c.addtionalhours;

            // Uhr
            Std.Value = date.Hour;
            Min.Value = date.Minute;
            Sec.Value = date.Second;

            if (control.timer_disabled)
            {
                cbZusiZeit.Checked = false;
                cbZusiZeit.Enabled = false;
                cbZusiZeit.Text += " (Kein Zusi gefunden!)";
            }
            else
            {
                cbZusiZeit.Checked = control.use_zusi_time;
            }

            if (control.inverse)
            {
                //switch to inverse
                this.BackColor = System.Drawing.Color.LightSlateGray;
                this.ForeColor = System.Drawing.Color.White;
            }

			this.TopMost = control.XMLConf.TopMost;

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
			this.dateP = new System.Windows.Forms.DateTimePicker();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Sec = new System.Windows.Forms.NumericUpDown();
			this.Min = new System.Windows.Forms.NumericUpDown();
			this.Std = new System.Windows.Forms.NumericUpDown();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.cbZusiZeit = new System.Windows.Forms.CheckBox();
			this.cbAdditionHours = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Sec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Min)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Std)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dateP);
			this.groupBox1.Enabled = false;
			this.groupBox1.Location = new System.Drawing.Point(24, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 80);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Datum:";
			// 
			// dateP
			// 
			this.dateP.Location = new System.Drawing.Point(24, 40);
			this.dateP.Name = "dateP";
			this.dateP.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.Sec);
			this.groupBox2.Controls.Add(this.Min);
			this.groupBox2.Controls.Add(this.Std);
			this.groupBox2.Enabled = false;
			this.groupBox2.Location = new System.Drawing.Point(24, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(248, 88);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Uhrzeit:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(160, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "Sekunde";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(96, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Minute";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Stunde";
			// 
			// Sec
			// 
			this.Sec.Location = new System.Drawing.Point(160, 48);
			this.Sec.Maximum = new System.Decimal(new int[] {
																59,
																0,
																0,
																0});
			this.Sec.Name = "Sec";
			this.Sec.Size = new System.Drawing.Size(40, 20);
			this.Sec.TabIndex = 6;
			// 
			// Min
			// 
			this.Min.Location = new System.Drawing.Point(96, 48);
			this.Min.Maximum = new System.Decimal(new int[] {
																59,
																0,
																0,
																0});
			this.Min.Name = "Min";
			this.Min.Size = new System.Drawing.Size(40, 20);
			this.Min.TabIndex = 5;
			// 
			// Std
			// 
			this.Std.Location = new System.Drawing.Point(32, 48);
			this.Std.Maximum = new System.Decimal(new int[] {
																23,
																0,
																0,
																0});
			this.Std.Name = "Std";
			this.Std.Size = new System.Drawing.Size(40, 20);
			this.Std.TabIndex = 4;
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(296, 184);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 32);
			this.button1.TabIndex = 6;
			this.button1.Text = "C";
			this.button1.Click += new System.EventHandler(this.B_C_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button2.Location = new System.Drawing.Point(296, 224);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(32, 32);
			this.button2.TabIndex = 7;
			this.button2.Text = "E";
			this.button2.Click += new System.EventHandler(this.B_E_Click);
			// 
			// cbZusiZeit
			// 
			this.cbZusiZeit.Checked = true;
			this.cbZusiZeit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbZusiZeit.Location = new System.Drawing.Point(24, 24);
			this.cbZusiZeit.Name = "cbZusiZeit";
			this.cbZusiZeit.Size = new System.Drawing.Size(304, 24);
			this.cbZusiZeit.TabIndex = 8;
			this.cbZusiZeit.Text = "Zusi Zeit (Registry) benutzen";
			this.cbZusiZeit.CheckedChanged += new System.EventHandler(this.cbZusiZeit_CheckedChanged);
			// 
			// cbAdditionHours
			// 
			this.cbAdditionHours.Checked = true;
			this.cbAdditionHours.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbAdditionHours.Location = new System.Drawing.Point(24, 48);
			this.cbAdditionHours.Name = "cbAdditionHours";
			this.cbAdditionHours.Size = new System.Drawing.Size(304, 24);
			this.cbAdditionHours.TabIndex = 9;
			this.cbAdditionHours.Text = "12 Std zur Zusi Zeit (TCP) addieren";
			// 
			// DateTimeField
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(354, 280);
			this.ControlBox = false;
			this.Controls.Add(this.cbAdditionHours);
			this.Controls.Add(this.cbZusiZeit);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "DateTimeField";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Datum/Uhrzeit einstellen";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Sec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Min)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Std)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

        public void B_E_Click(object sender, System.EventArgs e)
        {
            string s = dateP.Value.Date.ToShortDateString();
            s += " "+ Std.Value.ToString()+ ":"+Min.Value.ToString()+":"+Sec.Value.ToString();
            control.date_buffer = DateTime.Parse(s);

			control.addtionalhours = cbAdditionHours.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cbZusiZeit_CheckedChanged(object sender, System.EventArgs e)
        {
            control.use_zusi_time = cbZusiZeit.Checked;
            groupBox1.Enabled = !cbZusiZeit.Checked;
            groupBox2.Enabled = !cbZusiZeit.Checked;
        }

        public void B_C_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
	}
}
