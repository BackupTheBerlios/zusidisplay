using System;
using MMI.EBuLa.Tools;

namespace MMI.DAVID
{
	public enum CURRENT_DISPLAY
	{
		D2_BRH = 1, D2_D, D2_Br, D2_Fbr, 
		D2_Fdyn, D2_B, D2_HBL, D2_HL,
		D1_W, D1_LZB, D1_Ub, D1_A, 
		D1_F, D1_I, D1_U, D1_G, V_GREATER_0, V_EQUAL_0, ST, NONE
	}

	public enum TYPE
	{
		David1=1, David2
	}

	public enum ICETYPE
	{
		ICE1=1, ICE2_ET, ICE2_DT
	}

	public class DavidState : MMI.EBuLa.Tools.State
	{
		public CURRENT_DISPLAY DISPLAY;
		public TYPE Type;		
		public ICETYPE ICEtype;
		public int maxOberstrom = 900;

		public int FahrstufenSchalter = 0;
		public int E_Bremse = 0;

		public St�rungsManager st�rungmgr = new St�rungsManager();


		public DavidState()
		{	
			//st�rungmgr.Add(new St�rung(ENUMSt�rung.S02_Trennsch�tz));
			st�rungmgr.Add(new St�rung(ENUMSt�rung.S01_ZUSIKomm));			
		}
	}
}
