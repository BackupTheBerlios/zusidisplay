using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MMI.EBuLaEditor
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem mI_File_Open;
		private System.Windows.Forms.MenuItem mI_File_Save;
		private System.Windows.Forms.MenuItem mI_File_SaveAs;
		private System.Windows.Forms.MenuItem mI_File_Exit;
		private System.Windows.Forms.MenuItem mI_Help_About;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private DataControl dc;
		private TrackForm tf;
		private System.Windows.Forms.MenuItem mI_File_NewTrack;
		private System.Windows.Forms.MenuItem mI_File_NewTrain;
		private TrainForm trf;

		public FormMain()
		{
			InitializeComponent();

			dc = new DataControl();
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mI_File_NewTrack = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mI_File_Open = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.mI_File_Save = new System.Windows.Forms.MenuItem();
			this.mI_File_SaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.mI_File_Exit = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mI_Help_About = new System.Windows.Forms.MenuItem();
			this.mI_File_NewTrain = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.menuItem10});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mI_File_NewTrack,
																					  this.mI_File_NewTrain,
																					  this.menuItem7,
																					  this.mI_File_Open,
																					  this.menuItem9,
																					  this.mI_File_Save,
																					  this.mI_File_SaveAs,
																					  this.menuItem8,
																					  this.mI_File_Exit});
			this.menuItem1.Text = "Datei";
			// 
			// mI_File_NewTrack
			// 
			this.mI_File_NewTrack.Index = 0;
			this.mI_File_NewTrack.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mI_File_NewTrack.Text = "Neue Strecke";
			this.mI_File_NewTrack.Click += new System.EventHandler(this.mI_File_New_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 2;
			this.menuItem7.Text = "-";
			// 
			// mI_File_Open
			// 
			this.mI_File_Open.Index = 3;
			this.mI_File_Open.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mI_File_Open.Text = "Öffnen...";
			this.mI_File_Open.Click += new System.EventHandler(this.mI_File_Open_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 4;
			this.menuItem9.Text = "-";
			// 
			// mI_File_Save
			// 
			this.mI_File_Save.Enabled = false;
			this.mI_File_Save.Index = 5;
			this.mI_File_Save.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mI_File_Save.Text = "Speichern";
			this.mI_File_Save.Click += new System.EventHandler(this.mI_File_Save_Click);
			// 
			// mI_File_SaveAs
			// 
			this.mI_File_SaveAs.Enabled = false;
			this.mI_File_SaveAs.Index = 6;
			this.mI_File_SaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.mI_File_SaveAs.Text = "Speichern unter...";
			this.mI_File_SaveAs.Click += new System.EventHandler(this.mI_File_SaveAs_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 7;
			this.menuItem8.Text = "-";
			// 
			// mI_File_Exit
			// 
			this.mI_File_Exit.Index = 8;
			this.mI_File_Exit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.mI_File_Exit.Text = "Beenden";
			this.mI_File_Exit.Click += new System.EventHandler(this.mI_File_Exit_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mI_Help_About});
			this.menuItem10.Text = "?";
			// 
			// mI_Help_About
			// 
			this.mI_Help_About.Index = 0;
			this.mI_Help_About.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.mI_Help_About.Text = "Über...";
			// 
			// mI_File_NewTrain
			// 
			this.mI_File_NewTrain.Index = 1;
			this.mI_File_NewTrain.Text = "Neuer Zug";
			this.mI_File_NewTrain.Click += new System.EventHandler(this.mI_File_NewTrain_Click);
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 567);
			this.Menu = this.mainMenu;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EBuLa Editor";

		}
		#endregion

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new FormMain());
		}

		private void mI_File_Exit_Click(object sender, System.EventArgs e)
		{
			if (dc.isChanged)
			{
				string text;
				if (dc.isTrack) text = "das Geschwindigkeitsheft";
				else text = "den Fahrplan";

				DialogResult result = MessageBox.Show("Möchten Sie "+text+" jetzt speichern?", "Noch Speichern?", MessageBoxButtons.YesNoCancel);

				if (result == DialogResult.Cancel)
					return;
				else if (result == DialogResult.Yes)
					mI_File_Save_Click(sender, e);
			}
			this.Close();			
		}

		private void mI_File_New_Click(object sender, System.EventArgs e)
		{
			// NEW TRACK
			dc.isNewFile = true;

			dc.route = new EBuLa.Route("","");
			EBuLa.Entry entry = new EBuLa.Entry(EBuLa.EntryType.OPS_MARKER, "", "", "", "", "", "", new char(), "", new char());
			dc.route.Entrys.Add(entry);
			tf = new TrackForm(this);
			this.Controls.Add(tf);

			dc.OWN_FILL = true;
			dc.AddEntriesToTrack(ref tf);
			dc.OWN_FILL = false;

			mI_File_Save.Enabled = true;
			mI_File_SaveAs.Enabled = true;
		}

		private void mI_File_Open_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Alle EBuLa Dateien (track_*.xml;train_*.xml)|track_*.xml;train_*.xml|Geschwindigkeitshefte (track_*.xml)|track_*.xml|Fahrpläne (train_*.xml)|train_*.xml|Alle Dateien (*.*)|*.*";
			dialog.RestoreDirectory = true;
			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.Cancel) return;

			if (tf != null) this.Controls.Remove(tf);
			else if (trf != null) this.Controls.Remove(trf);
			tf = null; trf = null;
			dc.route = null;

			string filename = dialog.FileName;

			string h = System.IO.Path.GetFileNameWithoutExtension(filename);
			dc.isNewFile = false;
			if (h.IndexOf("track_") >= 0)
			{
				// it is a track
				dc.ReadTrack(filename);
				tf = new TrackForm(this);
				this.Controls.Add(tf);

				dc.OWN_FILL = true;
                dc.AddEntriesToTrack(ref tf);
				dc.OWN_FILL = false;
			}
			else if (h.IndexOf("train_") >= 0)
			{
				// it is a train
				dc.ReadTrain(filename);

				trf = new TrainForm(this);
				this.Controls.Add(trf);
				
				dc.OWN_FILL = true;
				dc.AddEntriesToTrain(ref trf);
				dc.OWN_FILL = false;
			}
			else
			{
				MessageBox.Show("Die Datei beginnt nicht mit 'track_' oder 'train_'!");
				return;
			}
            mI_File_Save.Enabled = true;
			mI_File_SaveAs.Enabled = true;
		}

		private void mI_File_Save_Click(object sender, System.EventArgs e)
		{
			if (dc.isNewFile)
			{
				dc.filename = "";
				mI_File_SaveAs_Click(sender, e);
			}
			else
			{
				if (tf != null)
					MMI.EBuLa.XMLReader.SaveRouteToTrackFile(dc.route, dc.filename);
				else 
					MMI.EBuLa.XMLReader.SaveRouteToTrainFile(dc.route, dc.filename);
			}
		}

		private void mI_File_SaveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			if (tf != null)
                dialog.Filter = "Geschwindigkeitshefte (track_*.xml)|track_*.xml|Fahrpläne (train_*.xml)|train_*.xml|Alle Dateien (*.*)|*.*";
			else
				dialog.Filter = "Fahrpläne (train_*.xml)|train_*.xml|Geschwindigkeitshefte (track_*.xml)|track_*.xml|Alle Dateien (*.*)|*.*";
			dialog.RestoreDirectory = true;
			dialog.FileName = System.IO.Path.GetFileName(dc.filename);
			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.Cancel) return;

			if (tf != null)
				MMI.EBuLa.XMLReader.SaveRouteToTrackFile(dc.route, dialog.FileName);
			else
				MMI.EBuLa.XMLReader.SaveRouteToTrainFile(dc.route, dialog.FileName);
		}

		public void UpdateComponents()
		{
			if (dc.OWN_FILL) return;
			if (tf != null)
                dc.UpdateTrackEntries(ref tf);
			else
				dc.UpdateTrainEntries(ref trf);
		}
		
		public void UpdateDesc()
		{
			if (tf != null)
                dc.UpdateDesc(tf);
			else
				dc.UpdateDesc(trf);
		}

		public void AddLine()
		{
			dc.AddLine(ref tf);
		}

		public void DeleteLine()
		{
			dc.DeleteLine(ref tf);
		}

		public DataControl GetDataControl()
		{
			return dc;
		}
		public void SetTrackName(string name, string filename)
		{
			dc.ReadTrack(filename);
			dc.OWN_FILL = true;
			dc.AddEntriesToTrain(ref trf);
			dc.OWN_FILL = false;
			dc.route.trackname = name;
			trf.tbName.Text = name;
		}

		private void mI_File_NewTrain_Click(object sender, System.EventArgs e)
		{
			dc.isNewFile = true;

			trf = new TrainForm(this);
			this.Controls.Add(trf);

			dc.OWN_FILL = true;
			trf.bBrowse_Click(this, new EventArgs());
			dc.OWN_FILL = false;

			mI_File_Save.Enabled = true;
			mI_File_SaveAs.Enabled = true;

		}
	}
}
