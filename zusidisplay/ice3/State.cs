using System;
using MMI.EBuLa.Tools;

namespace MMI.ICE3
{
	public enum CURRENT_DISPLAY
	{
		D2_BRH = 1, D2_HBL, D2_Hl_angl, D2_FspBr, 
		D2_Schalt, D2_Probe, D2_Zustand, D2_HL,
		D1_W, D1_System, D1_Abfert, D1_Zugb, D1_Grundb,
		D1_DFÜ, D1_VA, D1_FB, D1_Oberstr, D1_Schalt, 
		V_GREATER_0, V_EQUAL_0, ST, NONE
	}

	public enum TYPE
	{
		David1=1, David2
	}

	public enum ICE3TYPE
	{
		ICE403=1, ICE406, ICE411, ICE415, ICE415_2, ICE605, ICE605_2, NONE
	}

	public enum TÜREN
	{
		ZU_ABFAHRT=1, ZU, SCHLIESSEN, FAHRGÄSTE_I_O, AUF, FREIGEGEBEN
	}

	public enum WBBremse
	{
		BB_SB=1, AUS, SB
	}

	public class ICE3State : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY;
		public CURRENT_DISPLAY OLD_DISPLAY;
		public TYPE Type;		
		public ICE3TYPE ICEtype1;
		public ICE3TYPE ICEtype2;
		public int maxOberstrom = 900;
		public WBBremse wbBremse = WBBremse.AUS;
		public WBBremse wbBremse_erlaubt = WBBremse.BB_SB;

		public float Zusatzbremse = 0f;

		public float OberstromGrenze = 900f;
		public float OberstromToRender = 0f;
		public float Drehzahl = 0f;

		public new float Fahrstufe = 0f;
		public float E_Bremse = 0f;

		public TÜREN Türen = TÜREN.ZU;


		public StörungsManager störungmgr = new StörungsManager();


		public ICE3State()
		{	
			//störungmgr.Add(new Störung(ENUMStörung.S02_Trennschütz));
			störungmgr.Add(new Störung(ENUMStörung.S01_ZUSIKomm));			
		}
		
		public string Zugnummer = "12345";
	}
}
