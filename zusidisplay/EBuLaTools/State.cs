using System;

namespace MMI.EBuLa.Tools
{
	public enum TRAIN_TYPE 
	{
		BR185=1, BR146_1, BR189, DBpzfa766_1
	};

	public enum NETZ
	{
		DB=1, SNCF, ÖBB, SBB
	};

	public enum BREMSSTELLUNG
	{
		G=1, P, R, R_Mg, P_Mg
	}

	public enum ZUGBEEINFLUSSUNG
	{
		I54=1, I60, I60R, PZB90_15, PZB90_16, PZ80, PZ80R, LZB80_I80, SIGNUM
	}

	public enum TÜRSCHlIESSUNG
	{
		KEINE=1, TB0, ICE, SAT, TAV
	}

	public class State
	{
		#region Members

		public string Zugart = "";

		public TRAIN_TYPE TrainType = TRAIN_TYPE.BR146_1;

		public NETZ Netz = NETZ.DB;

		public bool Ein_Display = false;

		public bool LM_Zugart_O = false;
		public bool LM_Zugart_M = false;
		public bool LM_Zugart_U = true;

		public bool LM_LZB_Ü = false;
		public bool LM_LZB_B = true;
		public bool LM_LZB_S = false;
		public bool LM_LZB_G = false;
		public bool LM_LZB_H = false;
		public bool LM_LZB_ENDE = false;

		public bool LM_PZB_AKTIV = false;

		public bool LM_1000Hz = false;
		public bool LM_500Hz = false;
		public bool LM_Befehl = false;

		public bool LM_Sifa = false;
		public bool LM_HS = true;
		public bool LM_TÜR = false;
		public bool LM_NBÜ_EP = true;

		public bool LM_AFB = false;

		public bool LM_INTEGRA_GELB = false;
		public bool LM_INTEGRA_ROT = false;

		public bool addtionalhours = false;
		public bool SHOW_CLOCK = true;

		public bool Reisezug = false;
		public ZUGBEEINFLUSSUNG PZB_System = ZUGBEEINFLUSSUNG.I54;
		public float Brh = 0f;
		public BREMSSTELLUNG Bremsstellung = BREMSSTELLUNG.G;

		public TÜRSCHlIESSUNG Türschliesung = TÜRSCHlIESSUNG.SAT;

		public float Geschwindigkeit = 0f;
		public float AFB_LZB_SollGeschwindigkeit = 0f;
		public float AFB_SollGeschwindigkeit = 0f;
		public float LZB_SollGeschwindigkeit = 0f;
		public float LZB_ZielGeschwindigkeit = 0f;
		public int LZB_ZielWeg = 0;
		public float Zugkraft = 0f;
		public float ZugkraftGesammt = 0f;

		public float Zugkraft_Thread = 0f;
		public float ZugkraftGesammt_Thread = 0f;

		public int Fahrstufe = 0;
		public float Spannung = 0f;
		public float Oberstrom = 0f;

		public float HL_Druck = 0f;
		public float HBL_Druck = 0f;
		public float C_Druck = 0f;

		public int Richtungsschalter = 0;

		public bool LM_MG = false;



		public bool LM_Zugart_O2 = false;
		public bool LM_Zugart_M2 = false;
		public bool LM_Zugart_U2 = true;

		public bool LM_LZB_Ü2 = false;
		public bool LM_LZB_B2 = true;
		public bool LM_LZB_S2 = false;
		public bool LM_LZB_G2 = false;
		public bool LM_LZB_H2 = false;
		public bool LM_LZB_ENDE2 = false;

		public bool LM_PZB_AKTIV2 = false;

		public bool LM_1000Hz2 = false;
		public bool LM_500Hz2 = false;
		public bool LM_Befehl2 = false;

		#endregion

		public void ResetBackups()
		{
			LM_Zugart_O2 = false;
			LM_Zugart_M2 = false;
			LM_Zugart_U2 = true;

			LM_LZB_Ü2 = false;
			LM_LZB_B2 = true;
			LM_LZB_S2 = false;
			LM_LZB_G2 = false;
			LM_LZB_H2 = false;
			LM_LZB_ENDE2 = false;

			LM_PZB_AKTIV2 = false;

			LM_1000Hz2 = false;
			LM_500Hz2 = false;
			LM_Befehl2 = false;
		}
	}
}
