using System;
using MMI.EBuLa.Tools;

namespace MMI.DIAGNOSE
{
	public enum CURRENT_DISPLAY
	{
		G=1, Zug_Tf_Nr, Zugbesy, DSK, Z_BR, FIS, S, V_GREATER_0, V_EQUAL_0, ST, NONE
	}

	public enum TRAIN_TYPE
	{
		BR101=1, BR101_MET, BR145, BR146, BR146_1, BR152, BR182, BR185, BR189, ER20
	}

	public enum T�REN
	{
		ZU_ABFAHRT=1, ZU, SCHLIESSEN, FAHRG�STE_I_O, AUF, FREIGEGEBEN
	}

	public class DIAGNOSEState : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY = CURRENT_DISPLAY.G;
		public CURRENT_DISPLAY OLD_DISPLAY = CURRENT_DISPLAY.NONE;
		public TRAIN_TYPE type = TRAIN_TYPE.BR145;
		public int traction = 1;

		public float Zusatzbremse = 0f;

		public float OberstromGrenze = 900f;
		public float OberstromToRender = 0f;
		public float Drehzahl = 0f;

		public new float Fahrstufe = 0f;
		public int FahrstufenSchalter = 0;
		public float E_Bremse = 0f;

		public T�REN T�ren = T�REN.ZU;

		public bool DSK_Gesperrt = false;
		public string DSK_BUFFER = "    ";


		public St�rungsManager st�rungmgr = new St�rungsManager();


		public DIAGNOSEState()
		{	
			//st�rungmgr.Add(new St�rung(ENUMSt�rung.S02_Trennsch�tz));
			st�rungmgr.Add(new St�rung(ENUMSt�rung.S01_ZUSIKomm));			
		}
		
		public string Zugnummer = "<INIT>";
		public string ZugnummerTemp = "";
		public string Tfnummer = "<INIT>";
		public string TfnummerTemp = "";

		public int marker = 0;
		public bool marker_changed = false;
		public bool marker_changed2 = false;
	}

}
