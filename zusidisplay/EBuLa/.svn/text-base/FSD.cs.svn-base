using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MMI.EBuLa
{
	/// <summary>
	/// Zusammenfassung für FSD.
	/// </summary>
	public class FSD : System.Windows.Forms.UserControl
	{
        private Control control = null;
        private System.Windows.Forms.Label l_static_1;
        private System.Windows.Forms.Label l_static_2;
        private System.Windows.Forms.Label l_trainnumber;
        private System.Windows.Forms.Label l_traintype;
        private System.Windows.Forms.Label l_runningway;
        private System.Windows.Forms.Label l_discription;
        private System.Windows.Forms.Label l_baureihe;
        private System.Windows.Forms.Label l_vmax;
        private System.Windows.Forms.Label l_mbr;
        private System.Windows.Forms.Label l_date_valid_plan;
        private System.Windows.Forms.Label l_date_plan;
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FSD(Control c)
		{
			// Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			InitializeComponent();

            control = c;
            SetColor();
            l_vmax.Text = control.buffer_vmax + " km/h";
			l_mbr.Text = "Mbr "+control.Route.mbr;
			l_discription.Text = control.Route.Description;
			l_trainnumber.Text = control.Route.trainnumber;
			l_traintype.Text = control.Route.traintype;
			l_runningway.Text = control.Route.waypoints;
			l_date_plan.Text = control.Route.schedule_date;
			l_date_valid_plan.Text = control.Route.schedule_valid;
            l_baureihe.Text = "Tfz "+ control.Route.tfz;
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
            this.l_static_1 = new System.Windows.Forms.Label();
            this.l_static_2 = new System.Windows.Forms.Label();
            this.l_trainnumber = new System.Windows.Forms.Label();
            this.l_traintype = new System.Windows.Forms.Label();
            this.l_runningway = new System.Windows.Forms.Label();
            this.l_discription = new System.Windows.Forms.Label();
            this.l_baureihe = new System.Windows.Forms.Label();
            this.l_vmax = new System.Windows.Forms.Label();
            this.l_mbr = new System.Windows.Forms.Label();
            this.l_date_valid_plan = new System.Windows.Forms.Label();
            this.l_date_plan = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // l_static_1
            // 
            this.l_static_1.Location = new System.Drawing.Point(48, 8);
            this.l_static_1.Name = "l_static_1";
            this.l_static_1.Size = new System.Drawing.Size(80, 16);
            this.l_static_1.TabIndex = 1;
            this.l_static_1.Text = "Fahrplan vom";
            // 
            // l_static_2
            // 
            this.l_static_2.Location = new System.Drawing.Point(48, 24);
            this.l_static_2.Name = "l_static_2";
            this.l_static_2.Size = new System.Drawing.Size(128, 16);
            this.l_static_2.TabIndex = 2;
            this.l_static_2.Text = "Buchfahrplan gültig ab";
            // 
            // l_trainnumber
            // 
            this.l_trainnumber.Location = new System.Drawing.Point(48, 88);
            this.l_trainnumber.Name = "l_trainnumber";
            this.l_trainnumber.Size = new System.Drawing.Size(128, 16);
            this.l_trainnumber.TabIndex = 3;
            this.l_trainnumber.Text = "<Zugnummer>";
            // 
            // l_traintype
            // 
            this.l_traintype.Location = new System.Drawing.Point(208, 88);
            this.l_traintype.Name = "l_traintype";
            this.l_traintype.Size = new System.Drawing.Size(128, 16);
            this.l_traintype.TabIndex = 4;
            this.l_traintype.Text = "<Zuggattung>";
            // 
            // l_runningway
            // 
            this.l_runningway.Location = new System.Drawing.Point(48, 56);
            this.l_runningway.Name = "l_runningway";
            this.l_runningway.Size = new System.Drawing.Size(520, 16);
            this.l_runningway.TabIndex = 5;
            this.l_runningway.Text = "<Laufweg>";
            // 
            // l_discription
            // 
            this.l_discription.Location = new System.Drawing.Point(48, 112);
            this.l_discription.Name = "l_discription";
            this.l_discription.Size = new System.Drawing.Size(520, 40);
            this.l_discription.TabIndex = 6;
            this.l_discription.Text = "<Beschreibung>";
            // 
            // l_baureihe
            // 
            this.l_baureihe.Location = new System.Drawing.Point(48, 152);
            this.l_baureihe.Name = "l_baureihe";
            this.l_baureihe.Size = new System.Drawing.Size(128, 16);
            this.l_baureihe.TabIndex = 7;
            this.l_baureihe.Text = "Tfz <Nummer>";
            // 
            // l_vmax
            // 
            this.l_vmax.Location = new System.Drawing.Point(48, 176);
            this.l_vmax.Name = "l_vmax";
            this.l_vmax.Size = new System.Drawing.Size(128, 16);
            this.l_vmax.TabIndex = 8;
            this.l_vmax.Text = "<Vmax>";
            // 
            // l_mbr
            // 
            this.l_mbr.Location = new System.Drawing.Point(296, 152);
            this.l_mbr.Name = "l_mbr";
            this.l_mbr.Size = new System.Drawing.Size(128, 16);
            this.l_mbr.TabIndex = 9;
            this.l_mbr.Text = "Mbr <>";
            // 
            // l_date_valid_plan
            // 
            this.l_date_valid_plan.Location = new System.Drawing.Point(184, 24);
            this.l_date_valid_plan.Name = "l_date_valid_plan";
            this.l_date_valid_plan.Size = new System.Drawing.Size(128, 16);
            this.l_date_valid_plan.TabIndex = 10;
            this.l_date_valid_plan.Text = "<Datum>";
            // 
            // l_date_plan
            // 
            this.l_date_plan.Location = new System.Drawing.Point(184, 8);
            this.l_date_plan.Name = "l_date_plan";
            this.l_date_plan.Size = new System.Drawing.Size(128, 16);
            this.l_date_plan.TabIndex = 11;
            this.l_date_plan.Text = "<Datum>";
            // 
            // FSD
            // 
            this.Controls.Add(this.l_date_plan);
            this.Controls.Add(this.l_date_valid_plan);
            this.Controls.Add(this.l_mbr);
            this.Controls.Add(this.l_vmax);
            this.Controls.Add(this.l_baureihe);
            this.Controls.Add(this.l_discription);
            this.Controls.Add(this.l_runningway);
            this.Controls.Add(this.l_traintype);
            this.Controls.Add(this.l_trainnumber);
            this.Controls.Add(this.l_static_2);
            this.Controls.Add(this.l_static_1);
            this.Name = "FSD";
            this.Size = new System.Drawing.Size(630, 326);
            this.ResumeLayout(false);

        }
		#endregion

        public void SetColor()
        {
            if (control.inverse)
            {
                foreach(System.Windows.Forms.Control cont in this.Controls)
                {
                    if (cont.GetType().ToString() == "System.Windows.Forms.Label")
                    {
                        ((Label)cont).ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            else
            {
                foreach(System.Windows.Forms.Control cont in this.Controls)
                {
                    if (cont.GetType().ToString() == "System.Windows.Forms.Label")
                    {
                        ((Label)cont).ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }
	}
}
