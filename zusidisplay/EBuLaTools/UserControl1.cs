using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace MMI.EBuLa.Tools
{
	public class EBuLaTools : System.Windows.Forms.UserControl
	{
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.GroupBox gB_sound;
        private System.Windows.Forms.RadioButton rb_nosound;
        private System.Windows.Forms.RadioButton rb_apisound;
        private System.Windows.Forms.RadioButton rb_dxsound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_Inverse;
        private System.Windows.Forms.CheckBox cb_border;
        private System.Windows.Forms.GroupBox gb_multiwindow;
        private System.Windows.Forms.CheckBox cb_movewindow;
        private System.Windows.Forms.TextBox tb_height;
        private System.Windows.Forms.TextBox tb_width;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.GroupBox gb_Filesystem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_file;
        private System.Windows.Forms.TextBox tb_path;
        private System.Windows.Forms.FolderBrowserDialog BrowserDialog;
        private System.Windows.Forms.Button b_browse_path;
        private System.Windows.Forms.Button b_browse_file;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button b_cancel;

        private XMLLoader m_conf,backup_conf;
        private SystemTools.System sys = new SystemTools.System();
		private int oldSelectedIndex = -1;

        private System.Windows.Forms.GroupBox gb_Keys;
        private System.Windows.Forms.ListBox lb_Keys;
        private System.Windows.Forms.Button b_clear;
        private System.Windows.Forms.Timer key_timer;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tb_Port;
		private System.Windows.Forms.CheckBox cb_topmost;
		private System.Windows.Forms.CheckBox cb_focustozusi;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tb_Host;
		private System.Windows.Forms.ListBox lb_Windows;
		private System.Windows.Forms.CheckBox cb_deepSearch;
		private System.Windows.Forms.CheckBox l_doubleBuffer;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label l_Energie;
		private System.Windows.Forms.Button b_Energie;
		private System.Windows.Forms.CheckBox l_lowFPS;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TrackBar tB_FPS;
		private System.Windows.Forms.Label l_FPS;

        string[] keyarray;

		public EBuLaTools(ref XMLLoader Conf)
		{
			InitializeComponent();

            m_conf = Conf;
            backup_conf = m_conf;

            LoadConfiguration();
		}

        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			this.components = new System.ComponentModel.Container();
			this.gB_sound = new System.Windows.Forms.GroupBox();
			this.rb_dxsound = new System.Windows.Forms.RadioButton();
			this.rb_apisound = new System.Windows.Forms.RadioButton();
			this.rb_nosound = new System.Windows.Forms.RadioButton();
			this.gb_multiwindow = new System.Windows.Forms.GroupBox();
			this.lb_Windows = new System.Windows.Forms.ListBox();
			this.cb_focustozusi = new System.Windows.Forms.CheckBox();
			this.cb_topmost = new System.Windows.Forms.CheckBox();
			this.cb_border = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tb_height = new System.Windows.Forms.TextBox();
			this.tb_width = new System.Windows.Forms.TextBox();
			this.cb_movewindow = new System.Windows.Forms.CheckBox();
			this.cb_Inverse = new System.Windows.Forms.CheckBox();
			this.b_save = new System.Windows.Forms.Button();
			this.gb_Filesystem = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tb_Host = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tb_Port = new System.Windows.Forms.TextBox();
			this.b_browse_file = new System.Windows.Forms.Button();
			this.b_browse_path = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tb_file = new System.Windows.Forms.TextBox();
			this.tb_path = new System.Windows.Forms.TextBox();
			this.BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.b_cancel = new System.Windows.Forms.Button();
			this.gb_Keys = new System.Windows.Forms.GroupBox();
			this.b_clear = new System.Windows.Forms.Button();
			this.lb_Keys = new System.Windows.Forms.ListBox();
			this.key_timer = new System.Windows.Forms.Timer(this.components);
			this.cb_deepSearch = new System.Windows.Forms.CheckBox();
			this.l_doubleBuffer = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.l_Energie = new System.Windows.Forms.Label();
			this.b_Energie = new System.Windows.Forms.Button();
			this.l_lowFPS = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.l_FPS = new System.Windows.Forms.Label();
			this.tB_FPS = new System.Windows.Forms.TrackBar();
			this.gB_sound.SuspendLayout();
			this.gb_multiwindow.SuspendLayout();
			this.gb_Filesystem.SuspendLayout();
			this.gb_Keys.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tB_FPS)).BeginInit();
			this.SuspendLayout();
			// 
			// gB_sound
			// 
			this.gB_sound.Controls.Add(this.rb_dxsound);
			this.gB_sound.Controls.Add(this.rb_apisound);
			this.gB_sound.Controls.Add(this.rb_nosound);
			this.gB_sound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gB_sound.Location = new System.Drawing.Point(16, 24);
			this.gB_sound.Name = "gB_sound";
			this.gB_sound.Size = new System.Drawing.Size(232, 120);
			this.gB_sound.TabIndex = 0;
			this.gB_sound.TabStop = false;
			this.gB_sound.Text = "Soundsystem:";
			// 
			// rb_dxsound
			// 
			this.rb_dxsound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_dxsound.Location = new System.Drawing.Point(32, 80);
			this.rb_dxsound.Name = "rb_dxsound";
			this.rb_dxsound.TabIndex = 2;
			this.rb_dxsound.Text = "DirectX 9.0";
			this.rb_dxsound.CheckedChanged += new System.EventHandler(this.rb_dxsound_CheckedChanged);
			// 
			// rb_apisound
			// 
			this.rb_apisound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_apisound.Location = new System.Drawing.Point(32, 56);
			this.rb_apisound.Name = "rb_apisound";
			this.rb_apisound.TabIndex = 1;
			this.rb_apisound.Text = "Windows API";
			this.rb_apisound.CheckedChanged += new System.EventHandler(this.rb_apisound_CheckedChanged);
			// 
			// rb_nosound
			// 
			this.rb_nosound.Checked = true;
			this.rb_nosound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rb_nosound.Location = new System.Drawing.Point(32, 32);
			this.rb_nosound.Name = "rb_nosound";
			this.rb_nosound.TabIndex = 0;
			this.rb_nosound.TabStop = true;
			this.rb_nosound.Text = "kein Sound";
			this.rb_nosound.CheckedChanged += new System.EventHandler(this.rb_nosound_CheckedChanged);
			// 
			// gb_multiwindow
			// 
			this.gb_multiwindow.Controls.Add(this.lb_Windows);
			this.gb_multiwindow.Controls.Add(this.cb_focustozusi);
			this.gb_multiwindow.Controls.Add(this.cb_topmost);
			this.gb_multiwindow.Controls.Add(this.cb_border);
			this.gb_multiwindow.Controls.Add(this.label2);
			this.gb_multiwindow.Controls.Add(this.label1);
			this.gb_multiwindow.Controls.Add(this.tb_height);
			this.gb_multiwindow.Controls.Add(this.tb_width);
			this.gb_multiwindow.Controls.Add(this.cb_movewindow);
			this.gb_multiwindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gb_multiwindow.Location = new System.Drawing.Point(536, 24);
			this.gb_multiwindow.Name = "gb_multiwindow";
			this.gb_multiwindow.Size = new System.Drawing.Size(240, 440);
			this.gb_multiwindow.TabIndex = 4;
			this.gb_multiwindow.TabStop = false;
			this.gb_multiwindow.Text = "Mehrschirmbetrieb:";
			// 
			// lb_Windows
			// 
			this.lb_Windows.Location = new System.Drawing.Point(16, 32);
			this.lb_Windows.Name = "lb_Windows";
			this.lb_Windows.Size = new System.Drawing.Size(208, 147);
			this.lb_Windows.TabIndex = 0;
			this.lb_Windows.SelectedIndexChanged += new System.EventHandler(this.lb_Windows_SelectedIndexChanged);
			// 
			// cb_focustozusi
			// 
			this.cb_focustozusi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_focustozusi.Location = new System.Drawing.Point(24, 392);
			this.cb_focustozusi.Name = "cb_focustozusi";
			this.cb_focustozusi.Size = new System.Drawing.Size(200, 24);
			this.cb_focustozusi.TabIndex = 8;
			this.cb_focustozusi.Text = "Gib Fokus nach Klick zurück an Zusi";
			this.cb_focustozusi.CheckedChanged += new System.EventHandler(this.cb_focustozusi_CheckedChanged);
			// 
			// cb_topmost
			// 
			this.cb_topmost.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_topmost.Location = new System.Drawing.Point(24, 368);
			this.cb_topmost.Name = "cb_topmost";
			this.cb_topmost.Size = new System.Drawing.Size(184, 24);
			this.cb_topmost.TabIndex = 7;
			this.cb_topmost.Text = "Immer im Vordergrund";
			this.cb_topmost.CheckedChanged += new System.EventHandler(this.cb_topmost_CheckedChanged);
			// 
			// cb_border
			// 
			this.cb_border.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_border.Location = new System.Drawing.Point(24, 304);
			this.cb_border.Name = "cb_border";
			this.cb_border.TabIndex = 6;
			this.cb_border.Text = "Fensterrahmen";
			this.cb_border.CheckedChanged += new System.EventHandler(this.cb_border_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 272);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "vertikal:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 232);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "horizontal:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_height
			// 
			this.tb_height.Location = new System.Drawing.Point(96, 272);
			this.tb_height.Name = "tb_height";
			this.tb_height.TabIndex = 5;
			this.tb_height.Text = "0";
			this.tb_height.TextChanged += new System.EventHandler(this.tb_height_TextChanged);
			// 
			// tb_width
			// 
			this.tb_width.Location = new System.Drawing.Point(96, 232);
			this.tb_width.Name = "tb_width";
			this.tb_width.TabIndex = 3;
			this.tb_width.Text = "0";
			this.tb_width.TextChanged += new System.EventHandler(this.tb_width_TextChanged);
			// 
			// cb_movewindow
			// 
			this.cb_movewindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_movewindow.Location = new System.Drawing.Point(24, 200);
			this.cb_movewindow.Name = "cb_movewindow";
			this.cb_movewindow.Size = new System.Drawing.Size(176, 24);
			this.cb_movewindow.TabIndex = 1;
			this.cb_movewindow.Text = "Selektiertes Fenster verschieben";
			this.cb_movewindow.CheckedChanged += new System.EventHandler(this.cb_movewindow_CheckedChanged);
			// 
			// cb_Inverse
			// 
			this.cb_Inverse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_Inverse.Location = new System.Drawing.Point(40, 168);
			this.cb_Inverse.Name = "cb_Inverse";
			this.cb_Inverse.TabIndex = 1;
			this.cb_Inverse.Text = "Nachtmodus";
			this.cb_Inverse.CheckedChanged += new System.EventHandler(this.cb_Inverse_CheckedChanged);
			// 
			// b_save
			// 
			this.b_save.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.b_save.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_save.Location = new System.Drawing.Point(40, 560);
			this.b_save.Name = "b_save";
			this.b_save.TabIndex = 5;
			this.b_save.Text = "Speichern";
			this.b_save.Click += new System.EventHandler(this.b_save_Click);
			// 
			// gb_Filesystem
			// 
			this.gb_Filesystem.Controls.Add(this.label6);
			this.gb_Filesystem.Controls.Add(this.tb_Host);
			this.gb_Filesystem.Controls.Add(this.label5);
			this.gb_Filesystem.Controls.Add(this.tb_Port);
			this.gb_Filesystem.Controls.Add(this.b_browse_file);
			this.gb_Filesystem.Controls.Add(this.b_browse_path);
			this.gb_Filesystem.Controls.Add(this.label3);
			this.gb_Filesystem.Controls.Add(this.label4);
			this.gb_Filesystem.Controls.Add(this.tb_file);
			this.gb_Filesystem.Controls.Add(this.tb_path);
			this.gb_Filesystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gb_Filesystem.Location = new System.Drawing.Point(16, 216);
			this.gb_Filesystem.Name = "gb_Filesystem";
			this.gb_Filesystem.Size = new System.Drawing.Size(232, 184);
			this.gb_Filesystem.TabIndex = 2;
			this.gb_Filesystem.TabStop = false;
			this.gb_Filesystem.Text = "IOSystem:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 23);
			this.label6.TabIndex = 6;
			this.label6.Text = "Host:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_Host
			// 
			this.tb_Host.Location = new System.Drawing.Point(48, 112);
			this.tb_Host.Name = "tb_Host";
			this.tb_Host.Size = new System.Drawing.Size(136, 20);
			this.tb_Host.TabIndex = 7;
			this.tb_Host.Text = "";
			this.tb_Host.TextChanged += new System.EventHandler(this.tb_Host_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Port:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_Port
			// 
			this.tb_Port.Location = new System.Drawing.Point(48, 144);
			this.tb_Port.Name = "tb_Port";
			this.tb_Port.Size = new System.Drawing.Size(136, 20);
			this.tb_Port.TabIndex = 9;
			this.tb_Port.Text = "";
			this.tb_Port.TextChanged += new System.EventHandler(this.tb_Port_TextChanged);
			// 
			// b_browse_file
			// 
			this.b_browse_file.Enabled = false;
			this.b_browse_file.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_browse_file.Location = new System.Drawing.Point(192, 72);
			this.b_browse_file.Name = "b_browse_file";
			this.b_browse_file.Size = new System.Drawing.Size(24, 23);
			this.b_browse_file.TabIndex = 5;
			this.b_browse_file.Text = "...";
			this.b_browse_file.Visible = false;
			this.b_browse_file.Click += new System.EventHandler(this.b_browse_file_Click);
			// 
			// b_browse_path
			// 
			this.b_browse_path.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_browse_path.Location = new System.Drawing.Point(192, 32);
			this.b_browse_path.Name = "b_browse_path";
			this.b_browse_path.Size = new System.Drawing.Size(24, 23);
			this.b_browse_path.TabIndex = 2;
			this.b_browse_path.Text = "...";
			this.b_browse_path.Click += new System.EventHandler(this.b_browse_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Enabled = false;
			this.label3.Location = new System.Drawing.Point(4, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Datei:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label3.Visible = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Pfad:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_file
			// 
			this.tb_file.Enabled = false;
			this.tb_file.Location = new System.Drawing.Point(48, 72);
			this.tb_file.Name = "tb_file";
			this.tb_file.Size = new System.Drawing.Size(136, 20);
			this.tb_file.TabIndex = 4;
			this.tb_file.Text = "";
			this.tb_file.Visible = false;
			this.tb_file.TextChanged += new System.EventHandler(this.tb_file_TextChanged);
			// 
			// tb_path
			// 
			this.tb_path.Location = new System.Drawing.Point(48, 32);
			this.tb_path.Name = "tb_path";
			this.tb_path.Size = new System.Drawing.Size(136, 20);
			this.tb_path.TabIndex = 1;
			this.tb_path.Text = "";
			this.tb_path.TextChanged += new System.EventHandler(this.tb_path_TextChanged);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "*.zug";
			this.openFileDialog.Filter = "Zusi Zugdatei (*.zug)|*.zug|Alle Dateien (*.*)|*.*";
			// 
			// b_cancel
			// 
			this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_cancel.Location = new System.Drawing.Point(136, 560);
			this.b_cancel.Name = "b_cancel";
			this.b_cancel.TabIndex = 6;
			this.b_cancel.Text = "Abbrechen";
			this.b_cancel.Click += new System.EventHandler(this.b_cancel_Click);
			// 
			// gb_Keys
			// 
			this.gb_Keys.Controls.Add(this.b_clear);
			this.gb_Keys.Controls.Add(this.lb_Keys);
			this.gb_Keys.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gb_Keys.Location = new System.Drawing.Point(264, 24);
			this.gb_Keys.Name = "gb_Keys";
			this.gb_Keys.Size = new System.Drawing.Size(256, 344);
			this.gb_Keys.TabIndex = 3;
			this.gb_Keys.TabStop = false;
			this.gb_Keys.Text = "Tastenbelegung:";
			// 
			// b_clear
			// 
			this.b_clear.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_clear.Location = new System.Drawing.Point(88, 304);
			this.b_clear.Name = "b_clear";
			this.b_clear.TabIndex = 1;
			this.b_clear.Text = "löschen";
			this.b_clear.Click += new System.EventHandler(this.b_clear_Click);
			// 
			// lb_Keys
			// 
			this.lb_Keys.Location = new System.Drawing.Point(16, 32);
			this.lb_Keys.Name = "lb_Keys";
			this.lb_Keys.Size = new System.Drawing.Size(224, 251);
			this.lb_Keys.TabIndex = 0;
			this.lb_Keys.SelectedIndexChanged += new System.EventHandler(this.lb_Keys_SelectedIndexChanged);
			// 
			// key_timer
			// 
			this.key_timer.Interval = 10;
			this.key_timer.Tick += new System.EventHandler(this.key_timer_Tick);
			// 
			// cb_deepSearch
			// 
			this.cb_deepSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cb_deepSearch.Location = new System.Drawing.Point(40, 416);
			this.cb_deepSearch.Name = "cb_deepSearch";
			this.cb_deepSearch.Size = new System.Drawing.Size(464, 32);
			this.cb_deepSearch.TabIndex = 12;
			this.cb_deepSearch.Text = "Im EBuLa Zugauswahldialog auch den Abfahrt- und Zielbahnhof suchen";
			this.cb_deepSearch.Click += new System.EventHandler(this.cb_useDB_CheckedChanged);
			// 
			// l_doubleBuffer
			// 
			this.l_doubleBuffer.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.l_doubleBuffer.Location = new System.Drawing.Point(40, 448);
			this.l_doubleBuffer.Name = "l_doubleBuffer";
			this.l_doubleBuffer.Size = new System.Drawing.Size(472, 32);
			this.l_doubleBuffer.TabIndex = 13;
			this.l_doubleBuffer.Text = "Manuelles Doublebuffering verwenden (vor 1.6.2)";
			this.l_doubleBuffer.CheckedChanged += new System.EventHandler(this.l_doubleBuffer_CheckedChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.l_Energie);
			this.groupBox4.Controls.Add(this.b_Energie);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(672, 480);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(104, 100);
			this.groupBox4.TabIndex = 14;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Energiezähler:";
			// 
			// l_Energie
			// 
			this.l_Energie.Location = new System.Drawing.Point(8, 32);
			this.l_Energie.Name = "l_Energie";
			this.l_Energie.Size = new System.Drawing.Size(80, 23);
			this.l_Energie.TabIndex = 7;
			this.l_Energie.Text = "0 kWh";
			this.l_Energie.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// b_Energie
			// 
			this.b_Energie.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.b_Energie.Location = new System.Drawing.Point(16, 64);
			this.b_Energie.Name = "b_Energie";
			this.b_Energie.TabIndex = 6;
			this.b_Energie.Text = "Löschen";
			this.b_Energie.Click += new System.EventHandler(this.b_Energie_Click);
			// 
			// l_lowFPS
			// 
			this.l_lowFPS.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.l_lowFPS.Location = new System.Drawing.Point(40, 480);
			this.l_lowFPS.Name = "l_lowFPS";
			this.l_lowFPS.Size = new System.Drawing.Size(304, 32);
			this.l_lowFPS.TabIndex = 15;
			this.l_lowFPS.Text = "Niedrige Framerate in Diagnosedisplays (2 FPS)";
			this.l_lowFPS.CheckedChanged += new System.EventHandler(this.l_lowFPS_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.l_FPS);
			this.groupBox1.Controls.Add(this.tB_FPS);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(360, 480);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(304, 100);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Framerate:";
			// 
			// l_FPS
			// 
			this.l_FPS.Location = new System.Drawing.Point(16, 24);
			this.l_FPS.Name = "l_FPS";
			this.l_FPS.Size = new System.Drawing.Size(184, 23);
			this.l_FPS.TabIndex = 1;
			this.l_FPS.Text = "Aktuelle Framerate: 25 FPS";
			// 
			// tB_FPS
			// 
			this.tB_FPS.LargeChange = 10;
			this.tB_FPS.Location = new System.Drawing.Point(8, 48);
			this.tB_FPS.Maximum = 30;
			this.tB_FPS.Minimum = 2;
			this.tB_FPS.Name = "tB_FPS";
			this.tB_FPS.Size = new System.Drawing.Size(288, 45);
			this.tB_FPS.SmallChange = 2;
			this.tB_FPS.TabIndex = 0;
			this.tB_FPS.TickFrequency = 2;
			this.tB_FPS.Value = 25;
			this.tB_FPS.Scroll += new System.EventHandler(this.tB_FPS_Scroll);
			// 
			// EBuLaTools
			// 
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.l_lowFPS);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.l_doubleBuffer);
			this.Controls.Add(this.cb_deepSearch);
			this.Controls.Add(this.gb_Keys);
			this.Controls.Add(this.b_cancel);
			this.Controls.Add(this.gb_Filesystem);
			this.Controls.Add(this.b_save);
			this.Controls.Add(this.cb_Inverse);
			this.Controls.Add(this.gb_multiwindow);
			this.Controls.Add(this.gB_sound);
			this.Name = "EBuLaTools";
			this.Size = new System.Drawing.Size(800, 600);
			this.Load += new System.EventHandler(this.EBuLaTools_Load);
			this.gB_sound.ResumeLayout(false);
			this.gb_multiwindow.ResumeLayout(false);
			this.gb_Filesystem.ResumeLayout(false);
			this.gb_Keys.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tB_FPS)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

        public XMLLoader Conf
        {
            get{return m_conf;}
            set{m_conf = value;}
        }

        private void LoadConfiguration()
        {
            cb_Inverse.Checked = m_conf.Inverse;
			cb_deepSearch.Checked = m_conf.SearchForDepAndArr;
			cb_topmost.Checked = m_conf.TopMost;
			cb_focustozusi.Checked = m_conf.FocusToZusi;
			tb_Port.Text = m_conf.Port.ToString();
			tb_Host.Text = m_conf.Host;
			l_doubleBuffer.Checked = m_conf.DoubleBuffer;
			l_Energie.Text = Convert.ToInt32(m_conf.Energie / 1000d).ToString() + " kWh";
			l_lowFPS.Checked = m_conf.LowFPS;

			if (m_conf.FramesPerSecond > tB_FPS.Maximum)
				tB_FPS.Value = tB_FPS.Maximum;
			else if (m_conf.FramesPerSecond < tB_FPS.Minimum)
				tB_FPS.Value = tB_FPS.Minimum;
			else
				tB_FPS.Value = m_conf.FramesPerSecond;

			l_FPS.Text = "Aktuelle Framerate: "+tB_FPS.Value+" FPS";

			FillWindowList();

            SetMoveWindow(false);

            if (m_conf.Sound == 2)
            {
                rb_dxsound.Checked = true;
            }
            else if (m_conf.Sound == 1)
            {
                rb_apisound.Checked = true;
            }
            else
            {
                rb_nosound.Checked = true;
            }

            tb_path.Text = m_conf.Path;
            tb_file.Text = m_conf.File;

            FillKeyArray();

            LoadKeys();
            
        }

        private bool IsMoveWindow()
        {
            return  false; //TODO(m_conf.Width != 0 || m_conf.Height != 0);
        }

        private void SetMoveWindow(bool enabled)
        {
            if (enabled)
            {
                tb_width.Enabled = true;
                tb_height.Enabled = true;
                label1.Enabled = true;
                label2.Enabled = true;
            }
            else//disabled
            {
                tb_width.Enabled = false;
                tb_width.Text = "";
                tb_width_TextChanged(this, new System.EventArgs());
                tb_height.Text = "";
                tb_height_TextChanged(this, new System.EventArgs());
                tb_height.Enabled = false;
                label1.Enabled = false;
                label2.Enabled = false;
            }
        }

        private void b_load_Click(object sender, System.EventArgs e)
        {
            LoadConfiguration();
        }

        private void cb_movewindow_CheckedChanged(object sender, System.EventArgs e)
        {
            SetMoveWindow(cb_movewindow.Checked);
        }

        private void b_save_Click(object sender, System.EventArgs e)
        {
			if (lb_Windows.SelectedIndex < lb_Windows.Items.Count-1)
                lb_Windows.SelectedIndex++;
			else lb_Windows.SelectedIndex = 0;

			lb_Keys_SelectedIndexChanged(sender, e);
            m_conf.SaveFile();
            this.Dispose();
        }

        private void cb_Inverse_CheckedChanged(object sender, System.EventArgs e)
        {
            m_conf.Inverse = cb_Inverse.Checked;
        }

        private void cb_border_CheckedChanged(object sender, System.EventArgs e)
        {
            //TODOm_conf.Border = cb_border.Checked;
        }

        private void tb_width_TextChanged(object sender, System.EventArgs e)
        {
			/*
            try
            {
                if (tb_width.Text != "")
                {
					if (tb_width.Text[0].ToString() == "-")
					{
						if (tb_width.Text.Length > 1)
							m_conf.Width = Convert.ToInt32(tb_width.Text.Remove(0,1));
					}
					else
						m_conf.Width = Convert.ToInt32(tb_width.Text);
                }
                else
                {
                    m_conf.Width = 0;
                }
            }
            catch
            {
                MessageBox.Show("Fehlerhafte Eingabe!");
                tb_width.Text = tb_width.Text.Remove(tb_width.Text.Length-1,1);
            }
			*/
        }

        private void tb_height_TextChanged(object sender, System.EventArgs e)
        {
			/*
            try
            {
                if (tb_height.Text != "")
                {
					if (tb_height.Text[0].ToString() == "-")
					{
						if (tb_height.Text.Length > 1)
							m_conf.Height = Convert.ToInt32(tb_height.Text.Remove(0,1));
					}
					else
						m_conf.Height = Convert.ToInt32(tb_height.Text);

                }
                else
                {
                    m_conf.Height = 0;
                }
            }
            catch
            {
                MessageBox.Show("Fehlerhafte Eingabe!");
                tb_height.Text = tb_height.Text.Remove(tb_height.Text.Length-1,1);
            }
			*/
        }

        private void rb_nosound_CheckedChanged(object sender, System.EventArgs e)
        {
            SetSound();
        }

        private void SetSound()
        {
            if (rb_dxsound.Checked)
            {
                m_conf.Sound = 2;
            }
            else if (rb_apisound.Checked)
            {
                m_conf.Sound = 1;
            }
            else
            {
                m_conf.Sound = 0;
            }
        }

        private void rb_apisound_CheckedChanged(object sender, System.EventArgs e)
        {
            SetSound();
        }

        private void rb_dxsound_CheckedChanged(object sender, System.EventArgs e)
        {
            SetSound();        
        }

        private void b_browse_Click(object sender, System.EventArgs e)
        {
            BrowserDialog.ShowDialog();
            tb_path.Text = BrowserDialog.SelectedPath;
            m_conf.Path = tb_path.Text;
        }

        private void b_browse_file_Click(object sender, System.EventArgs e)
        {
            if (tb_path.Text != "") openFileDialog.InitialDirectory = tb_path.Text;
            openFileDialog.ShowDialog();
            tb_file.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            m_conf.File = tb_file.Text;
        }

        private void tb_path_TextChanged(object sender, System.EventArgs e)
        {
            m_conf.Path = tb_path.Text;
        }

        private void tb_file_TextChanged(object sender, System.EventArgs e)
        {
            m_conf.File = tb_file.Text;
        }

        private void b_cancel_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        void LoadKeys()
        {
            lb_Keys.BeginUpdate();
            lb_Keys.Items.Clear();
            lb_Keys.Items.Add("Taste 'E': "+ConvertStringToKeysString("key_E"));
            lb_Keys.Items.Add("Taste 'C': "+ConvertStringToKeysString("key_C"));
            lb_Keys.Items.Add("Taste 'Pfeil Links': "+ConvertStringToKeysString("key_Left"));
            lb_Keys.Items.Add("Taste 'Pfeil Rechts': "+ConvertStringToKeysString("key_Right"));
            lb_Keys.Items.Add("Taste 'Pfeil Hoch': "+ConvertStringToKeysString("key_Up"));
            lb_Keys.Items.Add("Taste 'Pfeil Runter': "+ConvertStringToKeysString("key_Down"));
            lb_Keys.Items.Add("Taste '1': "+ConvertStringToKeysString("key_1"));
            lb_Keys.Items.Add("Taste '2': "+ConvertStringToKeysString("key_2"));
            lb_Keys.Items.Add("Taste '3': "+ConvertStringToKeysString("key_3"));
            lb_Keys.Items.Add("Taste '4': "+ConvertStringToKeysString("key_4"));
            lb_Keys.Items.Add("Taste '5': "+ConvertStringToKeysString("key_5"));
            lb_Keys.Items.Add("Taste '6': "+ConvertStringToKeysString("key_6"));
            lb_Keys.Items.Add("Taste '7': "+ConvertStringToKeysString("key_7"));
            lb_Keys.Items.Add("Taste '8': "+ConvertStringToKeysString("key_8"));
            lb_Keys.Items.Add("Taste '9': "+ConvertStringToKeysString("key_9"));
            lb_Keys.Items.Add("Taste '0': "+ConvertStringToKeysString("key_0"));
            lb_Keys.Items.Add("Taste 'Invertieren': "+ConvertStringToKeysString("key_Invert"));
            lb_Keys.Items.Add("Taste 'Helligkeit': "+ConvertStringToKeysString("key_Brightness"));
            lb_Keys.Items.Add("Taste 'An/Aus': "+ConvertStringToKeysString("key_OnOff"));
            lb_Keys.EndUpdate();
        }

        string ConvertStringToKeysString(string key)
        {
            string keyname = m_conf.Key(key).ToString();
            if (keyname == "" || keyname == "-1")
            {
                return "(nicht zugewiesen)";
            }
            else
            {
                KeysConverter c = new KeysConverter();
                Keys e = (Keys)c.ConvertFromString(keyname);
                return e.ToString();
            }
        }

        void FillKeyArray()
        {
            keyarray = new string[19];
            keyarray[0] = "key_E";
            keyarray[1] = "key_C";
            keyarray[2] = "key_Left";
            keyarray[3] = "key_Right";
            keyarray[4] = "key_Up";
            keyarray[5] = "key_Down";
            keyarray[6] = "key_1";
            keyarray[7] = "key_2";
            keyarray[8] = "key_3";
            keyarray[9] = "key_4";
            keyarray[10] = "key_5";
            keyarray[11] = "key_6";
            keyarray[12] = "key_7";
            keyarray[13] = "key_8";
            keyarray[14] = "key_9";
            keyarray[15] = "key_0";
            keyarray[16] = "key_Invert";
            keyarray[17] = "key_Brightness";
            keyarray[18] = "key_OnOff";
        }

		void FillWindowList()
		{
			lb_Windows.Items.Clear();
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.EBuLa));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.MMI));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.DIAGNOSE));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.DAVID1));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.DAVID2));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.ICE3_1));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.ICE3_1));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.ET42X));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.VT612));
			lb_Windows.Items.Add(GetWindowListString(enumDisplay.Menu));
			if (oldSelectedIndex > -1) lb_Windows.SelectedIndex = oldSelectedIndex;
		}

		private string GetWindowListString(enumDisplay disp)
		{
			string retval = "";
			switch (disp)
			{
				case enumDisplay.Menu:
					retval = "Menü ( R="+m_conf.GetBorder(enumDisplay.Menu).ToString()+" / H="+m_conf.GetWidth(enumDisplay.Menu).ToString()+" / V="+m_conf.GetHeight(enumDisplay.Menu).ToString()+" )";
					break;
				case enumDisplay.EBuLa:
					retval = "EBuLa ( R="+m_conf.GetBorder(enumDisplay.EBuLa).ToString()+" / H="+m_conf.GetWidth(enumDisplay.EBuLa).ToString()+" / V="+m_conf.GetHeight(enumDisplay.EBuLa).ToString()+" )";
					break;
				case enumDisplay.MMI:
					retval = "MMI ( R="+m_conf.GetBorder(enumDisplay.MMI).ToString()+" / H="+m_conf.GetWidth(enumDisplay.MMI).ToString()+" / V="+m_conf.GetHeight(enumDisplay.MMI).ToString()+" )";
					break;
				case enumDisplay.DIAGNOSE:
					retval = "Diagnose ( R="+m_conf.GetBorder(enumDisplay.DIAGNOSE).ToString()+" / H="+m_conf.GetWidth(enumDisplay.DIAGNOSE).ToString()+" / V="+m_conf.GetHeight(enumDisplay.DIAGNOSE).ToString()+" )";
					break;
				case enumDisplay.DAVID1:
					retval = "DAVID(links) ( R="+m_conf.GetBorder(enumDisplay.DAVID1).ToString()+" / H="+m_conf.GetWidth(enumDisplay.DAVID1).ToString()+" / V="+m_conf.GetHeight(enumDisplay.DAVID1).ToString()+" )";
					break;
				case enumDisplay.DAVID2:
					retval = "DAVID(rechts) ( R="+m_conf.GetBorder(enumDisplay.DAVID2).ToString()+" / H="+m_conf.GetWidth(enumDisplay.DAVID2).ToString()+" / V="+m_conf.GetHeight(enumDisplay.DAVID2).ToString()+" )";
					break;
				case enumDisplay.ICE3_1:
					retval = "ICE3/T(D)(links) ( R="+m_conf.GetBorder(enumDisplay.ICE3_1).ToString()+" / H="+m_conf.GetWidth(enumDisplay.ICE3_1).ToString()+" / V="+m_conf.GetHeight(enumDisplay.ICE3_1).ToString()+" )";
					break;
				case enumDisplay.ICE3_2:
					retval = "ICE3/T(D)(rechts) ( R="+m_conf.GetBorder(enumDisplay.ICE3_2).ToString()+" / H="+m_conf.GetWidth(enumDisplay.ICE3_2).ToString()+" / V="+m_conf.GetHeight(enumDisplay.ICE3_2).ToString()+" )";
					break;
				case enumDisplay.ET42X:
					retval = "VT423-426 ( R="+m_conf.GetBorder(enumDisplay.ET42X).ToString()+" / H="+m_conf.GetWidth(enumDisplay.ET42X).ToString()+" / V="+m_conf.GetHeight(enumDisplay.ET42X).ToString()+" )";
					break;
				case enumDisplay.VT612:
					retval = "VT611/612 ( R="+m_conf.GetBorder(enumDisplay.VT612).ToString()+" / H="+m_conf.GetWidth(enumDisplay.VT612).ToString()+" / V="+m_conf.GetHeight(enumDisplay.VT612).ToString()+" )";
					break;
			}
			return retval;
		}

        private void b_clear_Click(object sender, System.EventArgs e)
        {
            if (lb_Keys.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte zuerst ein Taste markieren!");
            }
            else
            {
                m_conf.SetKey(keyarray[lb_Keys.SelectedIndex], -1);
                LoadKeys();
            }
        }

        private void lb_Keys_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lb_Keys.SelectedIndex == -1)
            {
                key_timer.Enabled = false;
                b_clear.Enabled = false;
            }
            else
            {
                b_clear.Enabled = true;
                key_timer.Enabled = true;
            }
        }

        private void key_timer_Tick(object sender, System.EventArgs e)
        {
            for(int i = 0; i < 251; i++)
            {
                if (CompKey(i) && (i != 1 && i != 2 && i != 4))
                {
                    key_timer.Enabled = false;
                    if (m_conf.IsInUse(i))
                    {
                        MessageBox.Show("Diese Taste ist bereits belegt!");
                        lb_Keys.SelectedIndex = -1;
                    }
                    else
                    {
                        m_conf.SetKey(keyarray[lb_Keys.SelectedIndex], i);
                        LoadKeys();
                    }
                    break;
                }
            }
        }

        private bool CompKey(long KCode)
        {
            long KCode2 = 0;
            /*if (KCode == 105) KCode2 = 33; // NUM 9
            if (KCode == 99) KCode2 = 34; // NUM 3
            if (KCode == 97) KCode2 = 35; // NUM 1            
            if (KCode == 103) KCode2 = 36; // NUM 7
            if (KCode == 100) KCode2 = 37; // NUM 4
            if (KCode == 104) KCode2 = 38; // NUM 8
            if (KCode == 102) KCode2 = 39; // NUM 6
            if (KCode == 98) KCode2 = 40; // NUM 2
            if (KCode == 96) KCode2 = 45; // NUM 0
            if (KCode == 110) KCode2 = 46; // NUM ,*/

            if ( (sys.GetKey(KCode) == -32767) || (sys.GetKey(KCode2) == -32767) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		private void cb_useDB_CheckedChanged(object sender, System.EventArgs e)
		{
			m_conf.SearchForDepAndArr = cb_deepSearch.Checked;
		}

		private void tb_Port_TextChanged(object sender, System.EventArgs e)
		{
			int port;
			try
			{
				port = Convert.ToInt32(tb_Port.Text);
			}
			catch (Exception)
			{
				MessageBox.Show("Sie müssen einen freien Port zwischen 1024 und 65536 angeben! (default:1435)");
				port = 1435;
				tb_Port.Text = "1435";
			}
			m_conf.Port = Convert.ToInt32(tb_Port.Text);
		}

		private void cb_topmost_CheckedChanged(object sender, System.EventArgs e)
		{
			m_conf.TopMost = cb_topmost.Checked;
		}

		private void cb_focustozusi_CheckedChanged(object sender, System.EventArgs e)
		{
			m_conf.FocusToZusi = cb_focustozusi.Checked;
		}

		private void tb_Host_TextChanged(object sender, System.EventArgs e)
		{
			m_conf.Host = tb_Host.Text;
		}

		private void EBuLaTools_Load(object sender, System.EventArgs e)
		{
		
		}

		private void lb_Windows_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((oldSelectedIndex == lb_Windows.SelectedIndex) || (lb_Windows.SelectedIndex < 0) ) return;
			
			if (oldSelectedIndex > -1)
			{
				SaveItemData(GetEnumFromIndex(oldSelectedIndex));
			}
			ShowItemData(GetEnumFromIndex(lb_Windows.SelectedIndex));

			oldSelectedIndex = lb_Windows.SelectedIndex;
			FillWindowList();
		}

		private void ShowItemData(enumDisplay display)
		{
			if (m_conf.GetWidth(display) == 0 && m_conf.GetHeight(display) == 0)
			{
				cb_movewindow.Checked = false;
				tb_width.Text = "0"; tb_height.Text = "0";
			}
			else
			{
				cb_movewindow.Checked = true;
				tb_width.Text = m_conf.GetWidth(display).ToString(); 
				tb_height.Text = m_conf.GetHeight(display).ToString(); 
			}

			cb_border.Checked = m_conf.GetBorder(display);
		}

		private void SaveItemData(enumDisplay display)
		{
			if (cb_movewindow.Checked)
			{
				if (tb_width.Text == "") tb_width.Text = "0";
				if (tb_height.Text == "") tb_height.Text = "0";
				m_conf.SetWidth(display, Convert.ToInt32(tb_width.Text));
				m_conf.SetHeight(display, Convert.ToInt32(tb_height.Text));
			}
			else
			{
				m_conf.SetWidth(display, 0);
				m_conf.SetHeight(display, 0);
			}
			m_conf.SetBorder(display, cb_border.Checked);
		}

		private enumDisplay GetEnumFromIndex(int index)
		{
			switch(index)
			{
				case 0:
					return enumDisplay.EBuLa;
				case 1:
					return enumDisplay.MMI;
				case 2:
					return enumDisplay.DIAGNOSE;
				case 3:
					return enumDisplay.DAVID1;	
				case 4:
					return enumDisplay.DAVID2;	
				case 5:
					return enumDisplay.ICE3_1;
				case 6:
					return enumDisplay.ICE3_2;
				case 7:
					return enumDisplay.ET42X;
				case 8:
					return enumDisplay.VT612;
				case 9:
					return enumDisplay.Menu;
				default:
					return enumDisplay.EBuLa;
			}
		}

		private void l_doubleBuffer_CheckedChanged(object sender, System.EventArgs e)
		{
			m_conf.DoubleBuffer = l_doubleBuffer.Checked;
		}

		private void b_Energie_Click(object sender, System.EventArgs e)
		{
			m_conf.Energie = 0d;
			l_Energie.Text = Convert.ToInt32(m_conf.Energie / 1000d).ToString() + " kWh";
		}

		private void l_lowFPS_CheckedChanged(object sender, System.EventArgs e)
		{
			m_conf.LowFPS = l_lowFPS.Checked;
		}

		private void tB_FPS_Scroll(object sender, System.EventArgs e)
		{
			m_conf.FramesPerSecond = tB_FPS.Value;
			l_FPS.Text = "Aktuelle Framerate: "+tB_FPS.Value+" FPS";
		}
    }
}
