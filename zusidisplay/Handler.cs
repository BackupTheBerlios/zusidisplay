using System;

namespace MMI
{
    public enum Loadwhat
    {
        EBuLa=1, EBuLaTools, MMI, DAVID, DAVID2, ICE3_1, ICE3_2, ET42X, VT612, DIAGNOSE, Menu, FIS_TERM
    }

	public class Handler
	{
        // members
        Loadwhat m_load;

        public Loadwhat Load
        {
            get
            {
                return m_load;
            }
            set
            {
                m_load = value;
            }
        }

		public bool ebula = true;
		public bool ebulatools = true;
		public bool mmi = true;
		public bool david = true;
		public bool ice3 = true;
		public bool et42x = true;
		public bool vt612 = true;
		public bool diagnose = true;

	}
}
