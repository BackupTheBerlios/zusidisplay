using System;
using MMI.EBuLa.Tools;

namespace MMI.VT612
{
	public enum CURRENT_DISPLAY
	{
		G=1, M�ngel, ZUG, Zugbesy, B, VA, Spg, FIS, S, V_GREATER_0, V_EQUAL_0, ST, NONE
	}

	public enum VT612TYPE
	{
		VT612=1, VT611, NONE
	}

	public enum T�REN
	{
		ZU_ABFAHRT=1, ZU, SCHLIESSEN, FAHRG�STE_I_O, AUF, FREIGEGEBEN
	}

	public class VT612State : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY = CURRENT_DISPLAY.G;
		public CURRENT_DISPLAY OLD_DISPLAY = CURRENT_DISPLAY.NONE;
		public VT612TYPE VT612type1 = VT612TYPE.VT612;
		public VT612TYPE VT612type2 = VT612TYPE.NONE;
		public VT612TYPE VT612type3 = VT612TYPE.NONE;
		public VT612TYPE VT612type4 = VT612TYPE.NONE;

		public float Zusatzbremse = 0f;

		public float OberstromGrenze = 900f;
		public float OberstromToRender = 0f;
		public float Drehzahl = 0f;

		public new float Fahrstufe = 0f;
		public int FahrstufenSchalter = 15;
		public float E_Bremse = 0f;

		public T�REN T�ren = T�REN.ZU;


		public St�rungsManager st�rungmgr = new St�rungsManager();


		public VT612State()
		{	
			//st�rungmgr.Add(new St�rung(ENUMSt�rung.S02_Trennsch�tz));
			st�rungmgr.Add(new St�rung(ENUMSt�rung.S01_ZUSIKomm));			
		}
		
		public string Zugnummer = "12345";
	}

}
