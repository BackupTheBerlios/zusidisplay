using System;
using MMI.EBuLa.Tools;

namespace MMI.ET42X
{
	public enum CURRENT_DISPLAY
	{
		G=1, Mängel, ZUG, Zugbesy, B, VA, Spg, FIS, FIS_HK, FIS_ROUTE, S, V_GREATER_0, V_EQUAL_0, ST, NONE, OBERSTROM, Info, Besetz, Vist, DSK, Zug_Tf_Nr
	}

	public enum ET42XTYPE
	{
		ET423=1, ET424, ET425, ET426, NONE
	}       	

	public enum TÜREN
	{
		ZU_ABFAHRT=1, ZU, SCHLIESSEN, FAHRGÄSTE_I_O, AUF, FREIGEGEBEN
	}

	public class ET42XState : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY = CURRENT_DISPLAY.G;
		public CURRENT_DISPLAY OLD_DISPLAY = CURRENT_DISPLAY.NONE;
		public ET42XTYPE ET42Xtype1 = ET42XTYPE.ET425;
		public ET42XTYPE ET42Xtype2 = ET42XTYPE.ET425;
		public ET42XTYPE ET42Xtype3 = ET42XTYPE.ET425;

		public FIS_TYPE FISType = FIS_TYPE.GPS_FIS;

		public float Zusatzbremse = 0f;

		public float OberstromGrenze = 1000f;
		public float OberstromToRender = 0f;
		public float Drehzahl = 0f;

		public new float Fahrstufe = 0f;
		public int FahrstufenSchalter = 10;
		public float E_Bremse = 0f;

		public TÜREN Türen = TÜREN.ZU;

		public bool DSK_Gesperrt = false;
		public string DSK_BUFFER = "    ";

		public bool Kuehlung = true;


		public StörungsManager störungmgr = new StörungsManager();


		public ET42XState()
		{	
			//störungmgr.Add(new Störung(ENUMStörung.S02_Trennschütz));
			störungmgr.Add(new Störung(ENUMStörung.S01_ZUSIKomm));			
		}

		public FIS fis = new FIS();
		
		public string Zugnummer = "<INIT>";
		public string ZugnummerTemp = "";
		public string Tfnummer = "<INIT>";
		public string TfnummerTemp = "";

		public int marker = 0;
		public bool marker_changed = false;
		public bool marker_changed2 = false;
	}

}
