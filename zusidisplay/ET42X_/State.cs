using System;
using MMI.EBuLa.Tools;

namespace MMI.ET42X
{
	public enum CURRENT_DISPLAY
	{
		G=1, M�ngel, ZUG, Zugbesy, B, VA, Spg, FIS, S, V_GREATER_0, V_EQUAL_0, ST, NONE
	}

	public enum ET42XTYPE
	{
		ET423=1, ET424, ET425, ET426, NONE
	}

	public enum T�REN
	{
		ZU_ABFAHRT=1, ZU, SCHLIESSEN, FAHRG�STE_I_O, AUF, FREIGEGEBEN
	}

	public class ET42XState : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY = CURRENT_DISPLAY.G;
		public CURRENT_DISPLAY OLD_DISPLAY = CURRENT_DISPLAY.NONE;
		public ET42XTYPE ET42Xtype1 = ET42XTYPE.ET423;
		public ET42XTYPE ET42Xtype2 = ET42XTYPE.NONE;
		public ET42XTYPE ET42Xtype3 = ET42XTYPE.NONE;

		public float Zusatzbremse = 0f;

		public float OberstromGrenze = 900f;
		public float OberstromToRender = 0f;
		public float Drehzahl = 0f;

		public new float Fahrstufe = 0f;
		public float E_Bremse = 0f;

		public T�REN T�ren = T�REN.ZU;


		public St�rungsManager st�rungmgr = new St�rungsManager();


		public ET42XState()
		{	
			//st�rungmgr.Add(new St�rung(ENUMSt�rung.S02_Trennsch�tz));
			st�rungmgr.Add(new St�rung(ENUMSt�rung.S01_ZUSIKomm));			
		}
		
		public string Zugnummer = "12345";
	}

}
