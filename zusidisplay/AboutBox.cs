using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMI
{
	/// <summary>
	/// Zusammenfassung für AboutBox.
	/// </summary>
	public class AboutBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label l_version;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label l_NET_version;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutBox(string version, bool topmost)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
			l_version.Text = "Version: "+version;
			l_NET_version.Text = ".NET Version: "+Environment.Version.Major+"."+Environment.Version.Minor+"."+Environment.Version.Build;
			if (Environment.Version.Revision > 2031) l_NET_version.Text += " SP1";
			this.TopMost = topmost;

			this.linkLabel1.Text = "http://zusidisplay.berlios.de";
			this.linkLabel1.Links[0].LinkData = "Register";
			this.linkLabel1.Links.Add(0, 29, "http://zusidisplay.berlios.de");

			this.linkLabel2.Text = "haupert@babylon2k.de";
			this.linkLabel2.Links[0].LinkData = "Register";
			this.linkLabel2.Links.Add(0, 20, "mailto:haupert@babylon2k.de");

			
		    this.linkLabel3.Text = "Diese Programm ist durch die LGPL geschützt";
			this.linkLabel3.Links[0].LinkData = "Register";
			this.linkLabel3.Links.Add(28, 5, ".\\lgpl.txt");
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutBox));
			this.label1 = new System.Windows.Forms.Label();
			this.l_version = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel();
			this.label14 = new System.Windows.Forms.Label();
			this.l_NET_version = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Zusi Display";
			// 
			// l_version
			// 
			this.l_version.Location = new System.Drawing.Point(24, 48);
			this.l_version.Name = "l_version";
			this.l_version.Size = new System.Drawing.Size(152, 16);
			this.l_version.TabIndex = 1;
			this.l_version.Text = "Version: xxxx";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(24, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 144);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Folgende Module sind installiert:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(184, 16);
			this.label7.TabIndex = 11;
			this.label7.Text = "FT95 FIS Terminal";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(16, 96);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(192, 16);
			this.label13.TabIndex = 10;
			this.label13.Text = "ET423-426 Diagnosedisplay";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(208, 96);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(184, 16);
			this.label12.TabIndex = 9;
			this.label12.Text = "VT611/612 Diagnosedisplay (alpha)";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(208, 48);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(176, 16);
			this.label11.TabIndex = 8;
			this.label11.Text = "Neubau E-Lok Diagnosedisplay";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(208, 72);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(176, 16);
			this.label9.TabIndex = 7;
			this.label9.Text = "ICE3/T Diagnosedisplay";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(208, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(176, 16);
			this.label5.TabIndex = 6;
			this.label5.Text = "EBuLa Tools";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(176, 16);
			this.label4.TabIndex = 5;
			this.label4.Text = "DAVID Diagnosedisplay";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "MMI Display für BR146.1/185";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "EBuLa Display";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(184, 440);
			this.button1.Name = "button1";
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(48, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Autor: Jens Haupert";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(176, 72);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(128, 16);
			this.label10.TabIndex = 8;
			this.label10.Text = "© 2004-2005";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(48, 184);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(352, 16);
			this.label15.TabIndex = 10;
			this.label15.Text = "solange es nicht verändert und der Autor genannt wird.";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(48, 216);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(295, 16);
			this.label16.TabIndex = 11;
			this.label16.Text = "Gefundene Fehler, Verbesserungen und";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label17.Location = new System.Drawing.Point(48, 232);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(295, 16);
			this.label17.TabIndex = 12;
			this.label17.Text = "Anmerkungen sind stehts wilkommen.";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(48, 104);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(54, 17);
			this.linkLabel1.TabIndex = 14;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "linkLabel1";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(48, 120);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(54, 17);
			this.linkLabel2.TabIndex = 15;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "linkLabel2";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// linkLabel3
			// 
			this.linkLabel3.AutoSize = true;
			this.linkLabel3.Location = new System.Drawing.Point(48, 152);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size(54, 17);
			this.linkLabel3.TabIndex = 16;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "linkLabel3";
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label14.Location = new System.Drawing.Point(48, 168);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(360, 16);
			this.label14.TabIndex = 9;
			this.label14.Text = "und darf beliebig oft kopiert werden,";
			this.label14.Click += new System.EventHandler(this.label14_Click);
			// 
			// l_NET_version
			// 
			this.l_NET_version.Location = new System.Drawing.Point(176, 48);
			this.l_NET_version.Name = "l_NET_version";
			this.l_NET_version.Size = new System.Drawing.Size(240, 16);
			this.l_NET_version.TabIndex = 17;
			this.l_NET_version.Text = ".NET Version: xxxx";
			// 
			// AboutBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(450, 480);
			this.Controls.Add(this.l_NET_version);
			this.Controls.Add(this.linkLabel3);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.l_version);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Info zu MMI";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void label7_Click(object sender, System.EventArgs e)
		{
		}

		private void label14_Click(object sender, System.EventArgs e)
		{
		
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			// Determine which link was clicked within the LinkLabel.
			this.linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;

			// Display the appropriate link based on the value of the 
			// LinkData property of the Link object.
			string target = e.Link.LinkData as string;

			// If the value looks like a URL, navigate to it.
			// Otherwise, display it in a message box.
			if(null != target && target.StartsWith("http"))
			{
				System.Diagnostics.Process.Start(target);
			}
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			// Determine which link was clicked within the LinkLabel.
			this.linkLabel2.Links[linkLabel2.Links.IndexOf(e.Link)].Visited = true;

			// Display the appropriate link based on the value of the 
			// LinkData property of the Link object.
			string target = e.Link.LinkData as string;

			// If the value looks like a URL, navigate to it.
			// Otherwise, display it in a message box.
			if(null != target && target.StartsWith("mailto"))
			{
				System.Diagnostics.Process.Start(target);
			}
		}

		private void linkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			// Determine which link was clicked within the LinkLabel.
			this.linkLabel3.Links[linkLabel3.Links.IndexOf(e.Link)].Visited = true;

			// Display the appropriate link based on the value of the 
			// LinkData property of the Link object.
			string target = e.Link.LinkData as string;

			// If the value looks like a URL, navigate to it.
			// Otherwise, display it in a message box.
			if(null != target /*&& target.StartsWith("http")*/)
			{
				System.Diagnostics.Process.Start(target);
			}
		
		}
	}
}
