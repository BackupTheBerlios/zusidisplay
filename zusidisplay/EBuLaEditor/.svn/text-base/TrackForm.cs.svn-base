using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MMI.EBuLaEditor
{
	/// <summary>
	/// Zusammenfassung für TrackForm.
	/// </summary>
	public class TrackForm : System.Windows.Forms.UserControl
	{
		public System.Windows.Forms.Label label5;
		public System.Windows.Forms.Label label2;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox tbName;
		public System.Windows.Forms.ComboBox cbTrack;
		public System.Windows.Forms.ComboBox cbGNT;
		public System.Windows.Forms.Label label6;
		public System.Windows.Forms.TextBox tbMbr;
		public System.Windows.Forms.Label label7;
		public System.Windows.Forms.Label lEntryCounter;
		public System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label label4;
		public System.Windows.Forms.Label label8;
		public System.Windows.Forms.Label label9;
		public System.Windows.Forms.CheckBox cbTunnel;
		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.RadioButton rbNone;
		public System.Windows.Forms.RadioButton rbSingle;
		public System.Windows.Forms.RadioButton rbDouble;
		public System.Windows.Forms.Button bUp;
		public System.Windows.Forms.Button bDown;
		public System.Windows.Forms.ListBox lbEntries;
		public System.Windows.Forms.TextBox tbDescription;
		public System.Windows.Forms.TextBox tbPos;
		public System.Windows.Forms.TextBox tbSpeed;
		public System.Windows.Forms.TextBox tbOps;
		public System.Windows.Forms.TextBox tbSig;
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.Button bAdd;
		public System.Windows.Forms.Button bDelete;
		public System.Windows.Forms.Label label10;
		public System.Windows.Forms.ComboBox cbOne_track;

		FormMain fm;

		public TrackForm(FormMain formm)
		{
			// Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			InitializeComponent();

			cbGNT.SelectedIndex = 0;
			cbTrack.SelectedIndex = 0;
			fm = formm;
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
			this.label5 = new System.Windows.Forms.Label();
			this.lEntryCounter = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.cbTrack = new System.Windows.Forms.ComboBox();
			this.cbGNT = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbMbr = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.tbPos = new System.Windows.Forms.TextBox();
			this.tbSpeed = new System.Windows.Forms.TextBox();
			this.tbOps = new System.Windows.Forms.TextBox();
			this.tbSig = new System.Windows.Forms.TextBox();
			this.cbTunnel = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbDouble = new System.Windows.Forms.RadioButton();
			this.rbSingle = new System.Windows.Forms.RadioButton();
			this.rbNone = new System.Windows.Forms.RadioButton();
			this.bUp = new System.Windows.Forms.Button();
			this.bDown = new System.Windows.Forms.Button();
			this.lbEntries = new System.Windows.Forms.ListBox();
			this.bAdd = new System.Windows.Forms.Button();
			this.bDelete = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.cbOne_track = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(304, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 15;
			this.label5.Text = "Geschwindigkeitsprofil:";
			this.label5.Click += new System.EventHandler(this.label5_Click);
			// 
			// lEntryCounter
			// 
			this.lEntryCounter.Location = new System.Drawing.Point(16, 152);
			this.lEntryCounter.Name = "lEntryCounter";
			this.lEntryCounter.Size = new System.Drawing.Size(100, 16);
			this.lEntryCounter.TabIndex = 19;
			this.lEntryCounter.Text = "Eintrag: 0 von 0";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(200, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "Gleisauswahl:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 14;
			this.label1.Text = "Streckenname:";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(16, 48);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(160, 20);
			this.tbName.TabIndex = 0;
			this.tbName.Text = "";
			this.tbName.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			// 
			// cbTrack
			// 
			this.cbTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTrack.Items.AddRange(new object[] {
														 "Regelgleis",
														 "Gegengleis"});
			this.cbTrack.Location = new System.Drawing.Point(200, 48);
			this.cbTrack.Name = "cbTrack";
			this.cbTrack.Size = new System.Drawing.Size(88, 21);
			this.cbTrack.TabIndex = 2;
			this.cbTrack.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			this.cbTrack.SelectedIndexChanged += new System.EventHandler(this.cbTrack_SelectedIndexChanged);
			// 
			// cbGNT
			// 
			this.cbGNT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGNT.Items.AddRange(new object[] {
													   "Normal",
													   "GNT"});
			this.cbGNT.Location = new System.Drawing.Point(312, 48);
			this.cbGNT.Name = "cbGNT";
			this.cbGNT.Size = new System.Drawing.Size(88, 21);
			this.cbGNT.TabIndex = 1;
			this.cbGNT.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			this.cbGNT.SelectedIndexChanged += new System.EventHandler(this.cbTrack_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(528, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(136, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Mindestbremshundertstel:";
			// 
			// tbMbr
			// 
			this.tbMbr.Location = new System.Drawing.Point(528, 48);
			this.tbMbr.Name = "tbMbr";
			this.tbMbr.Size = new System.Drawing.Size(80, 20);
			this.tbMbr.TabIndex = 3;
			this.tbMbr.Text = "";
			this.tbMbr.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 88);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(120, 16);
			this.label7.TabIndex = 18;
			this.label7.Text = "Streckenbeschreibung:";
			// 
			// tbDescription
			// 
			this.tbDescription.Location = new System.Drawing.Point(16, 104);
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.Size = new System.Drawing.Size(592, 20);
			this.tbDescription.TabIndex = 4;
			this.tbDescription.Text = "";
			this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			// 
			// tbPos
			// 
			this.tbPos.Location = new System.Drawing.Point(56, 200);
			this.tbPos.Name = "tbPos";
			this.tbPos.Size = new System.Drawing.Size(80, 20);
			this.tbPos.TabIndex = 5;
			this.tbPos.Text = "";
			this.tbPos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSig_KeyPress);
			// 
			// tbSpeed
			// 
			this.tbSpeed.Location = new System.Drawing.Point(176, 200);
			this.tbSpeed.Name = "tbSpeed";
			this.tbSpeed.Size = new System.Drawing.Size(80, 20);
			this.tbSpeed.TabIndex = 6;
			this.tbSpeed.Text = "";
			this.tbSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSig_KeyPress);
			// 
			// tbOps
			// 
			this.tbOps.Location = new System.Drawing.Point(296, 200);
			this.tbOps.Name = "tbOps";
			this.tbOps.Size = new System.Drawing.Size(192, 20);
			this.tbOps.TabIndex = 7;
			this.tbOps.Text = "";
			this.tbOps.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSig_KeyPress);
			// 
			// tbSig
			// 
			this.tbSig.Location = new System.Drawing.Point(528, 200);
			this.tbSig.Name = "tbSig";
			this.tbSig.Size = new System.Drawing.Size(80, 20);
			this.tbSig.TabIndex = 8;
			this.tbSig.Text = "";
			this.tbSig.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSig_KeyPress);
			// 
			// cbTunnel
			// 
			this.cbTunnel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbTunnel.Location = new System.Drawing.Point(296, 240);
			this.cbTunnel.Name = "cbTunnel";
			this.cbTunnel.TabIndex = 10;
			this.cbTunnel.Text = "Tunnel";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 20;
			this.label3.Text = "Position:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(176, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 21;
			this.label4.Text = "Geschwindigkeit:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(296, 184);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 16);
			this.label8.TabIndex = 22;
			this.label8.Text = "Name der Betriebsstelle:";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(528, 184);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 29;
			this.label9.Text = "Signalgeschw.:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbDouble);
			this.groupBox1.Controls.Add(this.rbSingle);
			this.groupBox1.Controls.Add(this.rbNone);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(56, 232);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sägezahnlinie:";
			// 
			// rbDouble
			// 
			this.rbDouble.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbDouble.Location = new System.Drawing.Point(24, 64);
			this.rbDouble.Name = "rbDouble";
			this.rbDouble.TabIndex = 2;
			this.rbDouble.Text = "doppelt";
			// 
			// rbSingle
			// 
			this.rbSingle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbSingle.Location = new System.Drawing.Point(24, 40);
			this.rbSingle.Name = "rbSingle";
			this.rbSingle.TabIndex = 1;
			this.rbSingle.Text = "einfach";
			// 
			// rbNone
			// 
			this.rbNone.Checked = true;
			this.rbNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbNone.Location = new System.Drawing.Point(24, 16);
			this.rbNone.Name = "rbNone";
			this.rbNone.TabIndex = 0;
			this.rbNone.TabStop = true;
			this.rbNone.Text = "keine";
			// 
			// bUp
			// 
			this.bUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bUp.Location = new System.Drawing.Point(648, 192);
			this.bUp.Name = "bUp";
			this.bUp.Size = new System.Drawing.Size(56, 48);
			this.bUp.TabIndex = 12;
			this.bUp.Text = "Nach oben";
			this.bUp.Click += new System.EventHandler(this.bUp_Click);
			// 
			// bDown
			// 
			this.bDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bDown.Location = new System.Drawing.Point(648, 248);
			this.bDown.Name = "bDown";
			this.bDown.Size = new System.Drawing.Size(56, 48);
			this.bDown.TabIndex = 13;
			this.bDown.Text = "Nach unten";
			this.bDown.Click += new System.EventHandler(this.bDown_Click);
			// 
			// lbEntries
			// 
			this.lbEntries.Location = new System.Drawing.Point(56, 352);
			this.lbEntries.Name = "lbEntries";
			this.lbEntries.Size = new System.Drawing.Size(552, 173);
			this.lbEntries.TabIndex = 11;
			this.lbEntries.SelectedIndexChanged += new System.EventHandler(this.lbEntries_SelectedIndexChanged);
			// 
			// bAdd
			// 
			this.bAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bAdd.Location = new System.Drawing.Point(440, 248);
			this.bAdd.Name = "bAdd";
			this.bAdd.Size = new System.Drawing.Size(80, 48);
			this.bAdd.TabIndex = 30;
			this.bAdd.Text = "Zeile einfügen";
			this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
			// 
			// bDelete
			// 
			this.bDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bDelete.Location = new System.Drawing.Point(528, 248);
			this.bDelete.Name = "bDelete";
			this.bDelete.Size = new System.Drawing.Size(80, 48);
			this.bDelete.TabIndex = 31;
			this.bDelete.Text = "Zeile löschen";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(424, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 33;
			this.label10.Text = "eingleisig:";
			// 
			// cbOne_track
			// 
			this.cbOne_track.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOne_track.Items.AddRange(new object[] {
															 "nein",
															 "ja"});
			this.cbOne_track.Location = new System.Drawing.Point(424, 48);
			this.cbOne_track.Name = "cbOne_track";
			this.cbOne_track.Size = new System.Drawing.Size(88, 21);
			this.cbOne_track.TabIndex = 32;
			this.cbOne_track.SelectedIndexChanged += new System.EventHandler(this.cbTrack_SelectedIndexChanged);
			// 
			// TrackForm
			// 
			this.Controls.Add(this.label10);
			this.Controls.Add(this.cbOne_track);
			this.Controls.Add(this.bDelete);
			this.Controls.Add(this.bAdd);
			this.Controls.Add(this.lbEntries);
			this.Controls.Add(this.bDown);
			this.Controls.Add(this.bUp);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbTunnel);
			this.Controls.Add(this.tbSig);
			this.Controls.Add(this.tbOps);
			this.Controls.Add(this.tbSpeed);
			this.Controls.Add(this.tbPos);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbMbr);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lEntryCounter);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.cbTrack);
			this.Controls.Add(this.cbGNT);
			this.Name = "TrackForm";
			this.Size = new System.Drawing.Size(800, 550);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void lbEntries_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fm.UpdateComponents();
		}

		private void bUp_Click(object sender, System.EventArgs e)
		{
			if (lbEntries.SelectedIndex > 1)
				lbEntries.SelectedIndex--;
			fm.UpdateComponents();
		}

		private void bDown_Click(object sender, System.EventArgs e)
		{
			if (lbEntries.SelectedIndex < lbEntries.Items.Count-1)
				lbEntries.SelectedIndex++;
			fm.UpdateComponents();
		}

		private void tbSig_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				bDown_Click(sender, e);
				tbPos.Focus();
			}
		}

		private void tbDescription_TextChanged(object sender, System.EventArgs e)
		{
			if (fm != null) fm.UpdateDesc();
		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			fm.AddLine();
		}

		private void bDelete_Click(object sender, System.EventArgs e)
		{
			fm.DeleteLine();
		}

		private void cbTrack_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (fm != null) fm.UpdateDesc();
		}

		private void label5_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
