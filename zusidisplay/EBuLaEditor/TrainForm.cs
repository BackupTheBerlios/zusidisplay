using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MMI.EBuLaEditor
{
	/// <summary>
	/// Zusammenfassung für TrainForm.
	/// </summary>
	public class TrainForm : System.Windows.Forms.UserControl
	{
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox tbName;
		public System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox tbTrainNumber;
		public System.Windows.Forms.Label label3;
		public System.Windows.Forms.TextBox tbTrainType;
		public System.Windows.Forms.Label label4;
		public System.Windows.Forms.Label label5;
		public System.Windows.Forms.Label label6;
		public System.Windows.Forms.Label label7;
		public System.Windows.Forms.Label label8;
		public System.Windows.Forms.Label label9;
		public System.Windows.Forms.Label label10;
		public System.Windows.Forms.TextBox tbBegin;
		public System.Windows.Forms.TextBox tbValidSince;
		public System.Windows.Forms.TextBox tbValidSpan;
		public System.Windows.Forms.TextBox tbWaypoints;
		public System.Windows.Forms.TextBox tbVmax;
		public System.Windows.Forms.TextBox tbTfz;
		public System.Windows.Forms.TextBox tbDays;
		private System.Windows.Forms.Button bBrowse;
		public System.Windows.Forms.ListBox lbEntries;
		public System.Windows.Forms.Label label11;
		public System.Windows.Forms.Label label12;
		public System.Windows.Forms.Label label13;
		public System.Windows.Forms.Button bDown;
		public System.Windows.Forms.Button bUp;
		public System.Windows.Forms.TextBox tbArr;
		public System.Windows.Forms.TextBox tbDep;
		public System.Windows.Forms.TextBox tbOpsName;
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.CheckBox cb_LastEntry;
		private FormMain form;

		public TrainForm(FormMain fm)
		{
			// Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			InitializeComponent();

			form = fm;

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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbTrainNumber = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbTrainType = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbBegin = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbValidSince = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbValidSpan = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbWaypoints = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbVmax = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.tbTfz = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tbDays = new System.Windows.Forms.TextBox();
			this.bBrowse = new System.Windows.Forms.Button();
			this.lbEntries = new System.Windows.Forms.ListBox();
			this.label11 = new System.Windows.Forms.Label();
			this.tbArr = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.tbDep = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tbOpsName = new System.Windows.Forms.TextBox();
			this.bDown = new System.Windows.Forms.Button();
			this.bUp = new System.Windows.Forms.Button();
			this.cb_LastEntry = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 16;
			this.label1.Text = "Streckenname:";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(16, 48);
			this.tbName.Name = "tbName";
			this.tbName.ReadOnly = true;
			this.tbName.Size = new System.Drawing.Size(160, 20);
			this.tbName.TabIndex = 15;
			this.tbName.Text = "";
			this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(432, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 18;
			this.label2.Text = "Zugnummer:";
			// 
			// tbTrainNumber
			// 
			this.tbTrainNumber.Location = new System.Drawing.Point(432, 48);
			this.tbTrainNumber.Name = "tbTrainNumber";
			this.tbTrainNumber.Size = new System.Drawing.Size(112, 20);
			this.tbTrainNumber.TabIndex = 17;
			this.tbTrainNumber.Text = "";
			this.tbTrainNumber.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(280, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 20;
			this.label3.Text = "Zugart:";
			// 
			// tbTrainType
			// 
			this.tbTrainType.Location = new System.Drawing.Point(280, 48);
			this.tbTrainType.Name = "tbTrainType";
			this.tbTrainType.Size = new System.Drawing.Size(104, 20);
			this.tbTrainType.TabIndex = 19;
			this.tbTrainType.Text = "";
			this.tbTrainType.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 22;
			this.label4.Text = "Begin des Laufwegs:";
			// 
			// tbBegin
			// 
			this.tbBegin.Location = new System.Drawing.Point(16, 104);
			this.tbBegin.Name = "tbBegin";
			this.tbBegin.Size = new System.Drawing.Size(160, 20);
			this.tbBegin.TabIndex = 21;
			this.tbBegin.Text = "";
			this.tbBegin.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(224, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(176, 16);
			this.label5.TabIndex = 24;
			this.label5.Text = "Gültigkeitsbeginn des Fahrplans:";
			// 
			// tbValidSince
			// 
			this.tbValidSince.Location = new System.Drawing.Point(224, 104);
			this.tbValidSince.Name = "tbValidSince";
			this.tbValidSince.Size = new System.Drawing.Size(160, 20);
			this.tbValidSince.TabIndex = 23;
			this.tbValidSince.Text = "";
			this.tbValidSince.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(432, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(192, 16);
			this.label6.TabIndex = 26;
			this.label6.Text = "Gültigkeitszeitraum des Fahrplans:";
			// 
			// tbValidSpan
			// 
			this.tbValidSpan.Location = new System.Drawing.Point(432, 104);
			this.tbValidSpan.Name = "tbValidSpan";
			this.tbValidSpan.Size = new System.Drawing.Size(160, 20);
			this.tbValidSpan.TabIndex = 25;
			this.tbValidSpan.Text = "";
			this.tbValidSpan.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(120, 16);
			this.label7.TabIndex = 28;
			this.label7.Text = "Laufwegs:";
			// 
			// tbWaypoints
			// 
			this.tbWaypoints.Location = new System.Drawing.Point(16, 160);
			this.tbWaypoints.Name = "tbWaypoints";
			this.tbWaypoints.Size = new System.Drawing.Size(272, 20);
			this.tbWaypoints.TabIndex = 27;
			this.tbWaypoints.Text = "";
			this.tbWaypoints.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(480, 144);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 16);
			this.label8.TabIndex = 32;
			this.label8.Text = "Höchstgeschwindigkeit:";
			// 
			// tbVmax
			// 
			this.tbVmax.Location = new System.Drawing.Point(480, 160);
			this.tbVmax.Name = "tbVmax";
			this.tbVmax.Size = new System.Drawing.Size(112, 20);
			this.tbVmax.TabIndex = 31;
			this.tbVmax.Text = "";
			this.tbVmax.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(336, 144);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 30;
			this.label9.Text = "Triebfahrzeug:";
			// 
			// tbTfz
			// 
			this.tbTfz.Location = new System.Drawing.Point(336, 160);
			this.tbTfz.Name = "tbTfz";
			this.tbTfz.Size = new System.Drawing.Size(104, 20);
			this.tbTfz.TabIndex = 29;
			this.tbTfz.Text = "";
			this.tbTfz.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 200);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 16);
			this.label10.TabIndex = 34;
			this.label10.Text = "Fahrplantage:";
			// 
			// tbDays
			// 
			this.tbDays.Location = new System.Drawing.Point(16, 216);
			this.tbDays.Name = "tbDays";
			this.tbDays.Size = new System.Drawing.Size(576, 20);
			this.tbDays.TabIndex = 33;
			this.tbDays.Text = "";
			this.tbDays.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// bBrowse
			// 
			this.bBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bBrowse.Location = new System.Drawing.Point(192, 48);
			this.bBrowse.Name = "bBrowse";
			this.bBrowse.Size = new System.Drawing.Size(24, 24);
			this.bBrowse.TabIndex = 35;
			this.bBrowse.Text = "...";
			this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// lbEntries
			// 
			this.lbEntries.Location = new System.Drawing.Point(16, 368);
			this.lbEntries.Name = "lbEntries";
			this.lbEntries.Size = new System.Drawing.Size(576, 173);
			this.lbEntries.TabIndex = 36;
			this.lbEntries.SelectedIndexChanged += new System.EventHandler(this.lbEntries_SelectedIndexChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(328, 272);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(100, 16);
			this.label11.TabIndex = 42;
			this.label11.Text = "Ankunft:";
			// 
			// tbArr
			// 
			this.tbArr.Location = new System.Drawing.Point(328, 288);
			this.tbArr.Name = "tbArr";
			this.tbArr.Size = new System.Drawing.Size(104, 20);
			this.tbArr.TabIndex = 41;
			this.tbArr.Text = "";
			this.tbArr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbArr_KeyPress);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(480, 272);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(100, 16);
			this.label12.TabIndex = 40;
			this.label12.Text = "Abfahrt:";
			// 
			// tbDep
			// 
			this.tbDep.Location = new System.Drawing.Point(480, 288);
			this.tbDep.Name = "tbDep";
			this.tbDep.Size = new System.Drawing.Size(112, 20);
			this.tbDep.TabIndex = 39;
			this.tbDep.Text = "";
			this.tbDep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbArr_KeyPress);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(16, 272);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(152, 16);
			this.label13.TabIndex = 38;
			this.label13.Text = "Name der Betriebsstelle:";
			// 
			// tbOpsName
			// 
			this.tbOpsName.Location = new System.Drawing.Point(16, 288);
			this.tbOpsName.Name = "tbOpsName";
			this.tbOpsName.ReadOnly = true;
			this.tbOpsName.Size = new System.Drawing.Size(256, 20);
			this.tbOpsName.TabIndex = 37;
			this.tbOpsName.Text = "";
			// 
			// bDown
			// 
			this.bDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bDown.Location = new System.Drawing.Point(640, 344);
			this.bDown.Name = "bDown";
			this.bDown.Size = new System.Drawing.Size(56, 48);
			this.bDown.TabIndex = 44;
			this.bDown.Text = "Nach unten";
			this.bDown.Click += new System.EventHandler(this.bDown_Click);
			// 
			// bUp
			// 
			this.bUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bUp.Location = new System.Drawing.Point(640, 288);
			this.bUp.Name = "bUp";
			this.bUp.Size = new System.Drawing.Size(56, 48);
			this.bUp.TabIndex = 43;
			this.bUp.Text = "Nach oben";
			this.bUp.Click += new System.EventHandler(this.bUp_Click);
			// 
			// cb_LastEntry
			// 
			this.cb_LastEntry.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_LastEntry.Location = new System.Drawing.Point(16, 328);
			this.cb_LastEntry.Name = "cb_LastEntry";
			this.cb_LastEntry.Size = new System.Drawing.Size(144, 24);
			this.cb_LastEntry.TabIndex = 45;
			this.cb_LastEntry.Text = "Letzte Betriebsstelle";
			// 
			// TrainForm
			// 
			this.Controls.Add(this.cb_LastEntry);
			this.Controls.Add(this.bDown);
			this.Controls.Add(this.bUp);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.tbArr);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.tbDep);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.tbOpsName);
			this.Controls.Add(this.lbEntries);
			this.Controls.Add(this.bBrowse);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbDays);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.tbVmax);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.tbTfz);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tbWaypoints);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbValidSpan);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tbValidSince);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbBegin);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbTrainType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbTrainNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbName);
			this.Name = "TrainForm";
			this.Size = new System.Drawing.Size(800, 550);
			this.ResumeLayout(false);

		}
		#endregion

		private void lbEntries_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			form.UpdateComponents();
		}

		private void bUp_Click(object sender, System.EventArgs e)
		{
			if (lbEntries.SelectedIndex > 0)
				lbEntries.SelectedIndex--;
		}

		private void bDown_Click(object sender, System.EventArgs e)
		{
			if (lbEntries.SelectedIndex < lbEntries.Items.Count - 1)
				lbEntries.SelectedIndex++;
		}

		public void bBrowse_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dialog;
			string dir = "";
			do
			{
				dialog = new OpenFileDialog();
				dialog.Filter = "Geschwindigkeitshefte (track_*.xml)|track_*.xml|Alle Dateien (*.*)|*.*";
				dialog.RestoreDirectory = false;
			
				if (!form.GetDataControl().isNewFile)
				{
					dir = System.IO.Path.GetDirectoryName(form.GetDataControl().filename);
					dir = System.IO.Directory.GetParent(dir).FullName;
					dialog.InitialDirectory = dir;
				}
				DialogResult result = dialog.ShowDialog();

				if (result == DialogResult.Cancel) return;

				if (System.IO.Path.GetDirectoryName(dialog.FileName) != dir && !(form.GetDataControl().isNewFile))
				{
					System.Windows.Forms.MessageBox.Show("FALSCHER ORDNER!");
				}
			}
			while(System.IO.Path.GetDirectoryName(dialog.FileName) != dir && !(form.GetDataControl().isNewFile));

			string file = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);

			form.SetTrackName(RemoveEBuLaStrings(file), dialog.FileName);
		}

		private void tbArr_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				bDown_Click(sender, e);
			}
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			if (form != null) form.UpdateDesc();
		}

		private string RemoveEBuLaStrings(string file)
		{
			int index = file.IndexOf("track_");
			if (index >= 0) file = file.Remove(index, 6);

			index = file.IndexOf("_left");
			if (index >= 0) file = file.Remove(index, 5);

			index = file.IndexOf("_right");
			if (index >= 0) file = file.Remove(index, 6);
			
			index = file.IndexOf("_gnt");
			if (index >= 0) file = file.Remove(index, 4);

			return file;
		}
	}
}
