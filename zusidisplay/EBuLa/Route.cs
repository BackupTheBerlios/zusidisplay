using System;
using System.Collections;

namespace MMI.EBuLa
{
	public class Route
	{
        ArrayList/*<Entry>*/ m_route = new ArrayList();

        ArrayList/*<>*/ m_timetable = new ArrayList();

        string m_name = "";

        string m_description = "";

        ulong m_offset = 0;

        long m_position = 0;

        string m_vmax = "";
		
		public bool   one_track = false;
		public string trainnumber = "";
		public string traintype = "";
		public string trackname = "";
		public string from = "";
		public string tfz = "";		
		public string waypoints = "";
		public string schedule_date = "";
		public string schedule_valid = "";		
		public string mbr = "";
		public string track_side = "right";
		public string track_gnt = "none";

		public Route(string name, string desc)
        {
			m_name = name;
            m_description = desc;
		}

        public string Name
        {
            get{ return m_name; }
            set{ m_name = value; }
        }

        public string Description
        {
            get{ return m_description; }
            set{ m_description = value; }
        }

        public ulong Offset
        {
            get{ return m_offset; }
            set{ m_offset = value; }
        }

        public long Position
        {
            get{ return m_position; }
            set{ m_position = value; }
        }

        public ArrayList Entrys
        {
            get { return m_route; }
            set { m_route = value; }
        }

        public string Vmax
        {
            get{ return m_vmax; }
            set{ m_vmax = value; }
        }
        

        public int SearchForSpeed(int pos, bool gnt)
        {
            pos = pos + (int)Offset - 1;
            if (pos > m_route.Count-1) return 0;
//            Console.WriteLine("LET'S GO! from: "+pos.ToString());
            for(int i = pos; i >= 0; i--)
            {
                Entry e = (Entry)m_route[i];
                
                if (gnt)
                {
                    if (e.m_gnt_speed != "")
                    {
                        Console.WriteLine("ENTRY OK: "+ e.m_gnt_speed);
                        return Convert.ToInt32(e.m_gnt_speed);
                    }
                }
                else
                {
                    if (e.m_speed!= "")
                    {
                        return Convert.ToInt32(e.m_speed);
                    }
                }
            }
            return 0;
        }

        public void setVmax()
        {
            int vmax = 0;
            foreach(Entry e in m_route)
            {
                try
                {
                    if (e.m_speed != "" && vmax < Convert.ToInt32(e.m_speed))
                    {
                        vmax = Convert.ToInt32(e.m_speed);
                    }
                }
                catch{}
            }
            m_vmax = vmax.ToString();
        }

	}
}
