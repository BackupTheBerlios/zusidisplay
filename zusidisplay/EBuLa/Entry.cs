using System;

namespace MMI.EBuLa
{
	public class Entry
	{

        #region Membervariables

        public EntryType m_type = EntryType.OPS_MARKER;
        public string m_position = "0,0";
        public string m_speed = "0";
        public string m_gnt_speed = "";
        public string m_ops_name = "OPS";
        public string m_ops_speed = "";
        public string m_eta = "00:00"; // estimated time of arrival
        public char m_eta_app = '0';
        public string m_etd = "00:00"; // estimated time of departure
        public char m_etd_app = '0';
        public bool inverse = false;
        public int zack = 0;
		public bool tunnel = false;
		public bool isJump = false;
		public bool isLast = false;

        #endregion

		public Entry(EntryType type, string position, string speed, string gntspeed, string ops_name, string ops_speed, string eta, char eta_app, string etd, char etd_app)
		{
            m_type = type;
            m_position = position;
            m_speed = speed;
            m_gnt_speed = gntspeed;
            m_ops_name = ops_name;
            m_ops_speed = ops_speed;
            m_eta = eta;
            m_eta_app = eta_app;
            m_etd = etd;
            m_etd_app = etd_app;
		}

        public bool IsNotEmpty
        {
            get
            {
                return (!((m_position == "") && (m_speed == "") && (m_gnt_speed == "") && (m_ops_name == "") && (m_ops_speed == "") && (m_eta == "") && (m_etd == "")));
                }
        }


	}
}
