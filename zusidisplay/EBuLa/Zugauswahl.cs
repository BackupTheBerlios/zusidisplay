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

		bool nonNumEntered = false;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label H_Text2;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label H_Text3;
        private System.Windows.Forms.Label Zugwahl_lang;
        private System.Windows.Forms.Label H_Text1;
        private System.Windows.Forms.Label H_CD;
		private System.Windows.Forms.Button b_E;
		private System.Windows.Forms.Button b_C;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label H_Version;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Zugauswahl(ref Control control, Label l_top)
		{
			InitializeComponent();

			//
			// INIT
			//
            m_control = control;
            Zugwahl_lang.Text = "";

			ArrayList trains = null;

			/*
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
			*/

			// DEBUG
			string oldtext = l_top.Text;
			Application.DoEvents();
			control.ParseDirectory(l_top);
			Application.DoEvents();
			l_top.Text = oldtext;

			if (control.train_list == null)
				return;

			
			listBox.Sorted = true;

			if (control.inverse)
			{
				//switch to inverse
				this.BackColor = System.Drawing.Color.LightSlateGray;
				this.ForeColor = System.Drawing.Color.White;
				listBox.BackColor = System.Drawing.Color.LightSlateGray;
				listBox.ForeColor = System.Drawing.Color.White;
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
			this.b_E = new System.Windows.Forms.Button();
			this.b_C = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.H_Version = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// H_Text2
			// 
			this.H_Text2.BackColor = System.Drawing.Color.Transparent;
			this.H_Text2.Location = new System.Drawing.Point(88, 16);
			this.H_Text2.Name = "H_Text2";
			this.H_Text2.Size = new System.Drawing.Size(168, 23);
			this.H_Text2.TabIndex = 0;
			this.H_Text2.Text = "Geben Sie die Zugnummer ein:";
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(248, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(104, 23);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
			// 
			// listBox
			// 
			this.listBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.listBox.Location = new System.Drawing.Point(16, 56);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(480, 95);
			this.listBox.TabIndex = 3;
			this.listBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBox_KeyUp);
			this.listBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseUp);
			// 
			// H_Text3
			// 
			this.H_Text3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.H_Text3.Location = new System.Drawing.Point(208, 160);
			this.H_Text3.Name = "H_Text3";
			this.H_Text3.TabIndex = 4;
			this.H_Text3.Text = "Ausgewählter Zug:";
			this.H_Text3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Zugwahl_lang
			// 
			this.Zugwahl_lang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Zugwahl_lang.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Zugwahl_lang.Location = new System.Drawing.Point(32, 184);
			this.Zugwahl_lang.Name = "Zugwahl_lang";
			this.Zugwahl_lang.Size = new System.Drawing.Size(456, 32);
			this.Zugwahl_lang.TabIndex = 5;
			// 
			// H_Text1
			// 
			this.H_Text1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.H_Text1.Location = new System.Drawing.Point(32, 224);
			this.H_Text1.Name = "H_Text1";
			this.H_Text1.Size = new System.Drawing.Size(504, 16);
			this.H_Text1.TabIndex = 6;
			this.H_Text1.Text = "Bitte Zugnummer eingben, bestätigen mit E, korrigieren mit C";
			// 
			// H_CD
			// 
			this.H_CD.Location = new System.Drawing.Point(248, 264);
			this.H_CD.Name = "H_CD";
			this.H_CD.Size = new System.Drawing.Size(72, 23);
			this.H_CD.TabIndex = 10;
			this.H_CD.Text = "CD 04/05";
			// 
			// b_E
			// 
			this.b_E.Location = new System.Drawing.Point(504, 184);
			this.b_E.Name = "b_E";
			this.b_E.Size = new System.Drawing.Size(32, 32);
			this.b_E.TabIndex = 8;
			this.b_E.Text = "E";
			this.b_E.Click += new System.EventHandler(this.b_E_Click);
			// 
			// b_C
			// 
			this.b_C.Location = new System.Drawing.Point(504, 56);
			this.b_C.Name = "b_C";
			this.b_C.Size = new System.Drawing.Size(32, 32);
			this.b_C.TabIndex = 7;
			this.b_C.Text = "C";
			this.b_C.Click += new System.EventHandler(this.b_C_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(16, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Bitte wählen Sie den gewünschten Zug:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 264);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 23);
			this.label2.TabIndex = 9;
			this.label2.Text = "ZusiDisplay";
			// 
			// H_Version
			// 
			this.H_Version.Location = new System.Drawing.Point(400, 264);
			this.H_Version.Name = "H_Version";
			this.H_Version.Size = new System.Drawing.Size(152, 23);
			this.H_Version.TabIndex = 11;
			this.H_Version.Text = "EBuLa Version 7.2.0";
			this.H_Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// Zugauswahl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 280);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
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
			{
				TrainInfo ti = (TrainInfo)m_control.train_list[listBox.SelectedItem.ToString().Replace("(","").Replace(")","")];
				Zugwahl_lang.Text = listBox.SelectedItem.ToString();
				if (ti.Departure != "")	Zugwahl_lang.Text += " von \""+ti.Departure + "\"";
				if (ti.Arrival != "")	Zugwahl_lang.Text += " nach \""+ti.Arrival + "\"";
				Zugwahl_lang.Text +=" ("+ti.File+")";
				H_Text1.Text = "Bitte Zuglauf auswählen, Abbruch mit C, zum Regelzug mit E";
			}
			else
			{
				H_Text1.Text = "Bitte Zugnummer eingben, bestätigen mit E, korrigieren mit C";
			}
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
			nonNumEntered = false;

            if (e.KeyCode == Keys.E)
            {
				nonNumEntered = true;
				AcceptAndClose();    				
            }
		
			if (e.KeyCode == Keys.C)
			{
				nonNumEntered = true;
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}

			if (e.KeyValue < 48 || (e.KeyValue > 57 && e.KeyValue < 96 ) || e.KeyValue > 105)
			{
				nonNumEntered = true;
			}

			if (e.KeyCode == Keys.Back)
			{
				nonNumEntered = false;
				//textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length-1, 1);
			}

		}

		private void UpdateList()
		{
			listBox.Items.Clear();
			if (textBox1.Text == "") return;

			//listBox.BeginUpdate();
			foreach(DictionaryEntry de in m_control.train_list)
			{		
				TrainInfo ti = (TrainInfo)de.Value;
				if (ti.Number.StartsWith(textBox1.Text))
					listBox.Items.Add(ti.Number+" ("+ti.Type+")");
			}
			//listBox.ResumeLayout();
			listBox.Update();
		}

		private void SetTrainTandN()
		{
			/*
			string text = Zugwahl_lang.Text;

			int space = text.IndexOf(" ", 0);
			int brack = text.IndexOf("]", 0);
			
            string number = text.Substring(0, space);

			string type = text.Substring(space+2, brack-(space+2));

			*/
			TrainInfo ti = (TrainInfo)m_control.train_list[listBox.SelectedItem.ToString().Replace("(","").Replace(")","")];

			m_control.buffer_trainnumber = ti.Number;
			m_control.buffer_traintype = ti.Type;
			m_control.current_Train = ti;
			//m_control.buffer_trainschedule = schedule;
		}

		private void AcceptAndClose()
		{
			if (listBox.SelectedIndex > -1)
			{
				SetTrainTandN();
				//m_control.searchInTrackPath = true;
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

		private void H_Version_Click(object sender, System.EventArgs e)
		{
		
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (nonNumEntered)
			{
				e.Handled = true;
			}
		}

		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			UpdateList();
		}
	}
}
