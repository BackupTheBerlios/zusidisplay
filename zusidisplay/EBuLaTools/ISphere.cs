using System;
using System.Drawing;

namespace MMI.EBuLa.Tools
{
	/// <summary>
	/// Zusammenfassung für ISphere.
	/// </summary>
	public class ISphere
	{
		int x, y, z;
        int m_radius;

        public ISphere(int x, int y, int radius)
        {
			this.x = x;
			this.y = y;
            m_radius = radius;
        }

        public int Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }

		public System.Collections.ArrayList PointList()
		{
			// this gives a "light" sphere
			// because we also nead a "heavy" one with 360 points
			// for the windrose
			return PointList(true);
		}

        public System.Collections.ArrayList PointList(bool realSphere)
        {
            System.Collections.ArrayList points = new System.Collections.ArrayList();

            Point p = new Point(0,0);

            // ang = 0°
            p.X = (x + this.Radius);
            p.Y = y;
			points.Add(p);

            // 0° < ang < 90°
			for(int ang = 1; ang < 90; ang++)
            {
                double angl = ang * (Math.PI / 180); // Convert to "Bogenmaß"
                double h = this.Radius * Math.Sin(angl);
                double z = this.Radius * Math.Sqrt( 1d - Math.Pow(Math.Sin(angl),2d) );
                p = new Point(0,0);
                p.X = Convert.ToInt32(x + z);
                p.Y = Convert.ToInt32(y + h);
                points.Add(p);
            }
                                
            // ang = 90°
            p.X = Convert.ToInt32(x);
            p.Y = Convert.ToInt32(y + this.Radius);
			points.Add(p);

            // 90° < ang < 180°
            for(int ang = 91; ang < 180; ang++)
            {
                double angl = ang * (Math.PI / 180); // Convert to "Bogenmaß"
                double h = this.Radius * Math.Sin(angl);
                double z = this.Radius * Math.Sqrt( 1d - Math.Pow(Math.Sin(angl),2d) );
                p = new Point(0,0);
                p.X = Convert.ToInt32(x - z);
                p.Y = Convert.ToInt32(y + h);
                points.Add(p);
            }

            // ang = 180°
            p.X = Convert.ToInt32(x - this.Radius);
            p.Y = Convert.ToInt32(y);
			points.Add(p);

            // 180° < ang < 270°
            for(int ang = 1; ang < 90; ang++)
            {
                double angl = ang * (Math.PI / 180); // Convert to "Bogenmaß"
                double h = this.Radius * Math.Sin(angl);
                double z = this.Radius * Math.Sqrt( 1d - Math.Pow(Math.Sin(angl),2d) );
                p = new Point(0,0);
                p.X = Convert.ToInt32(x - z);
                p.Y = Convert.ToInt32(y - h);
                points.Add(p);
            }

            // ang = 270°
            p.X = Convert.ToInt32(x);
            p.Y = Convert.ToInt32(y - this.Radius);
			points.Add(p);

            // 270° < ang < 360°
            for(int ang = 91; ang < 180; ang++)
            {
                double angl = ang * (Math.PI / 180); // Convert to "Bogenmaß"
                double h = this.Radius * Math.Sin(angl);
                double z = this.Radius * Math.Sqrt( 1d - Math.Pow(Math.Sin(angl),2d) );
                p = new Point(0,0);
                p.X = Convert.ToInt32(x + z);
                p.Y = Convert.ToInt32(y - h);
                points.Add(p);
            }

			// ang = 360° = 0°
			p.X = Convert.ToInt32(x + this.Radius);
			p.Y = Convert.ToInt32(y);
			points.Add(p);

            return points;
        }
    }
}
