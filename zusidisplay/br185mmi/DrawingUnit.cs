using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MMI.MMIBR185
{
	/// <summary>
	/// Zusammenfassung für DrawingUnit.
	/// </summary>
	public abstract class DrawingUnit
	{
		protected Color m_dark_color = Color.Black;
		protected SmoothingMode m_smoothing_mode = SmoothingMode.HighSpeed;
		protected const int m_thickness = 1;

		protected System.Windows.Forms.UserControl m_parent;

		protected Point m_upper_left;
		protected Point m_lower_right;
		
		protected string m_text_line;
		
		protected Color m_color;
		protected Color m_draw_color;

		protected bool m_is_flashing;
		protected uint m_flashing_speed;

		protected System.Windows.Forms.Timer m_timer;

		public DrawingUnit()
		{
			m_parent = null;
			m_upper_left = new Point(0,0);
			m_lower_right = new Point(0,0);
			m_text_line = "";
			m_color = Color.White;
			m_draw_color = m_color;
			m_is_flashing = false;
			m_flashing_speed = int.MaxValue;

			m_timer = new System.Windows.Forms.Timer();
			m_timer.Enabled = false;
			m_timer.Tick += new System.EventHandler(this.OnTimerTick);
		}

		public DrawingUnit(System.Windows.Forms.UserControl parent,Point ul, Point lr, string text, Color color, bool isFlashing, uint flashing_speed)
		{
			m_parent = parent;
			m_upper_left = ul;
			m_lower_right = lr;
			m_text_line = text;
			m_color = color;
			m_draw_color = m_color;
			m_is_flashing = isFlashing;
			m_flashing_speed = flashing_speed;

			m_timer = new System.Windows.Forms.Timer();
			m_timer.Enabled = false;
			if (m_is_flashing) 
			{
				m_timer.Interval = (int)m_flashing_speed;
			}
			else
			{
				m_timer.Interval = int.MaxValue;
			}
			m_timer.Tick += new System.EventHandler(this.OnTimerTick);
		}

		public abstract void Draw(ref Graphics pg);

		public void Flash(bool on)
		{
			m_timer.Enabled = on;
		}

		public void Clear(ref Graphics db_g)
		{
			if (m_parent != null)
			{
				// Graphics Object
				Graphics pg = db_g;
				//pg.SmoothingMode = m_smoothing_mode;

				// clear used area

				// Brush Object
				SolidBrush sb = new SolidBrush(m_dark_color);
			
				int width = Math.Abs(m_upper_left.X - m_lower_right.X);
				int height = Math.Abs(m_upper_left.Y - m_lower_right.Y);

				pg.FillRectangle(sb, m_upper_left.X, m_upper_left.Y, width, height);
			}
		}

		private void OnTimerTick(object sender, System.EventArgs e)
		{
			/*if (m_draw_color == m_color)
			{
				m_draw_color = m_dark_color;
			}
			else
			{
				m_draw_color = m_color;
			}

			Draw(null);*/
		}

	}
}
