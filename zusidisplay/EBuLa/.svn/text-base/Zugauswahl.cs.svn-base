using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	/// <summary>
	/// Zusammenfassung für Zugauswahl.
	/// </summary>
	public class Zugauswahl : System.Windows.Forms.Form
	{
        private Control m_control;
		private string schedule = "";

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label H_Text2;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label H_Text3;
        private System.Windows.Forms.Label Zugwahl_lang;
        private System.Windows.Forms.Label H_Text1;
        private System.Windows.Forms.Label H_CD;
        private System.Windows.Forms.Label H_Version;
		private System.Windows.Forms.Button b_E;
		private System.Windows.Forms.Button b_C;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Zugauswahl(ref Control control)
		{
			InitializeComponent();

			//
			// INIT
			//
            m_control = control;
            Zugwahl_lang.Text = "";

			ArrayList trains = null;
			if (control.timer_disabled)
			{
				//stand alone
				OpenFileDialog f = new OpenFileDialog();
				f.InitialDirectory = control.XMLConf.Path+ @"\Strecken";
				f.Filter = "Zusi Fahrpläne (*.fpl)|*.fpl|Alle Dateien (*.*)|*.*" ;
				f.RestoreDirectory = true;

				if(f.ShowDialog() == DialogResult.OK)
				{
					schedule = System.IO.Path.GetFileNameWithoutExtension(f.FileName);
					string path = System.IO.Path.GetDirectoryName(f.FileName);
					control.buffer_trainpath = path;
					m_control.buffer_trainschedule = f.FileName;

					trains = XMLReader.ReadTrainlistFromDB(schedule, path , ref control);
				}
			}
			else
			{
				control.buffer_trainpath = control.getTrackPath();
				schedule = getSchedule(control.getTrainName());
				trains = XMLReader.ReadTrainlistFromDB(schedule, control.getTrackPath() , ref control);
			}
			
			if (trains == null)
				return;

			listBox.BeginUpdate();
			foreach(string s in trains)
			{
				listBox.Items.Add(s);
			}
			listBox.ResumeLayout();

			if (control.inverse)
			{
				//switch to inverse
				this.BackColor = System.Drawing.Color.LightSlateGray;
				this.ForeColor = System.Drawing.Color.White;
				listBox.BackColor = System.Drawing.Color.LightSlateGray;
			}

			this.TopMost = control.XMLConf.TopMost;
		}

		private string getSchedule(string s)
		{
			return m_control.getSchedule(s);
			/*string h = System.IO.Path.GetFileNameWithoutExtension(s);
			int offset = h.IndexOf("_",0);
			h = h.Substring(0, offset);
			rrturn h;*/
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
			this.H_Text2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.listBox = new System.Windows.Forms.ListBox();
			this.H_Text3 = new System.Windows.Forms.Label();
			this.Zugwahl_lang = new System.Windows.Forms.Label();
			this.H_Text1 = new System.Windows.Forms.Label();
			this.H_CD = new System.Windows.Forms.Label();
			this.H_Version = new System.Windows.Forms.Label();
			this.b_E = new System.Windows.Forms.Button();
			this.b_C = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// H_Text2
			// 
			this.H_Text2.BackColor = System.Drawing.Color.Transparent;
			this.H_Text2.Location = new System.Drawing.Point(88, 32);
			this.H_Text2.Name = "H_Text2";
			this.H_Text2.Size = new System.Drawing.Size(168, 23);
			this.H_Text2.TabIndex = 0;
			this.H_Text2.Text = "Geben Sie die Zugnummer ein:";
			this.H_Text2.Visible = false;
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(248, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(104, 23);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.Visible = false;
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			// 
			// listBox
			// 
			this.listBox.Location = new System.Drawing.Point(16, 64);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(480, 173);
			this.listBox.TabIndex = 2;
			this.listBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBox_KeyUp);
			this.listBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseUp);
			// 
			// H_Text3
			// 
			this.H_Text3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.H_Text3.Location = new System.Drawing.Point(208, 248);
			this.H_Text3.Name = "H_Text3";
			this.H_Text3.TabIndex = 3;
			this.H_Text3.Text = "Ausgewählter Zug:";
			this.H_Text3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Zugwahl_lang
			// 
			this.Zugwahl_lang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Zugwahl_lang.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Zugwahl_lang.Location = new System.Drawing.Point(32, 272);
			this.Zugwahl_lang.Name = "Zugwahl_lang";
			this.Zugwahl_lang.Size = new System.Drawing.Size(456, 40);
			this.Zugwahl_lang.TabIndex = 4;
			// 
			// H_Text1
			// 
			this.H_Text1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.H_Text1.Location = new System.Drawing.Point(32, 320);
			this.H_Text1.Name = "H_Text1";
			this.H_Text1.Size = new System.Drawing.Size(504, 24);
			this.H_Text1.TabIndex = 5;
			this.H_Text1.Text = "Bitte Zugnummer eingben, bestätigen mit E, korrigieren mit C";
			// 
			// H_CD
			// 
			this.H_CD.Location = new System.Drawing.Point(8, 368);
			this.H_CD.Name = "H_CD";
			this.H_CD.Size = new System.Drawing.Size(72, 23);
			this.H_CD.TabIndex = 6;
			this.H_CD.Text = "CD 02/04";
			// 
			// H_Version
			// 
			this.H_Version.Location = new System.Drawing.Point(400, 368);
			this.H_Version.Name = "H_Version";
			this.H_Version.Size = new System.Drawing.Size(152, 23);
			this.H_Version.TabIndex = 7;
			this.H_Version.Text = "EBuLa Version 5.2.0";
			this.H_Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// b_E
			// 
			this.b_E.Location = new System.Drawing.Point(504, 280);
			this.b_E.Name = "b_E";
			this.b_E.Size = new System.Drawing.Size(32, 32);
			this.b_E.TabIndex = 8;
			this.b_E.Text = "E";
			this.b_E.Click += new System.EventHandler(this.b_E_Click);
			// 
			// b_C
			// 
			this.b_C.Location = new System.Drawing.Point(504, 64);
			this.b_C.Name = "b_C";
			this.b_C.Size = new System.Drawing.Size(32, 32);
			this.b_C.TabIndex = 9;
			this.b_C.Text = "C";
			this.b_C.Click += new System.EventHandler(this.b_C_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.TabIndex = 10;
			this.label1.Text = "Bitte wählen Sie den gewünschten Zug:";
			// 
			// Zugauswahl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 392);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.b_C);
			this.Controls.Add(this.b_E);
			this.Controls.Add(this.H_Version);
			this.Controls.Add(this.H_CD);
			this.Controls.Add(this.H_Text1);
			this.Controls.Add(this.Zugwahl_lang);
			this.Controls.Add(this.H_Text3);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.H_Text2);
			this.Controls.Add(this.listBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Zugauswahl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EBuLa Zugauswahl";
			this.ResumeLayout(false);

		}
		#endregion

        private void listBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            listBox_KeyUp(sender, new System.Windows.Forms.KeyEventArgs(System.Windows.Forms.Keys.A));
        }

        private void listBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
			if (listBox.SelectedItem != null)
				Zugwahl_lang.Text = listBox.SelectedItem.ToString();
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E)
            {
				AcceptAndClose();
            }
		
			if (e.KeyCode == Keys.C)
			{
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
		}

		private void SetTrainTandN()
		{
			string text = Zugwahl_lang.Text;

			int space = text.IndexOf(" ", 0);
			int brack = text.IndexOf("]", 0);
			
            string number = text.Substring(0, space);

			string type = text.Substring(space+2, brack-(space+2));

			m_control.buffer_trainnumber = number;
			m_control.buffer_traintype = type;
			m_control.buffer_trainschedule = schedule;
		}

		private void AcceptAndClose()
		{
			if (Zugwahl_lang.Text != "")
			{
				SetTrainTandN();
				m_control.searchInTrackPath = true;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("Bitte Zug auswählen!");
				return;
			}
		}

		private void b_E_Click(object sender, System.EventArgs e)
		{
			AcceptAndClose();
		}

		private void b_C_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
