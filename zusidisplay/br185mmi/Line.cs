using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MMI.MMIBR185
{
	/// <summary>
	/// Zusammenfassung für Line.
	/// </summary>
	public class Line : DrawingUnit
	{
		public Line() : base() {}

		public Line(System.Windows.Forms.UserControl parent,Point ul, Point lr, string text, Color color, bool isFlashing, uint flashing_speed) : 
			base(parent, ul, lr, text, color, isFlashing, flashing_speed) {}
        
		public override void Draw(ref Graphics db_g)
		{
			if (m_parent != null)
			{
				// Graphics Object
				Graphics pg = db_g;
				//pg.SmoothingMode = m_smoothing_mode;

				DrawSquare(pg);

				DrawText(pg);
			}
		}

		private void DrawText(Graphics pg)
		{
			Brush textColor;
			if (m_draw_color == Color.FromArgb(58,41,121) || m_draw_color == Color.Black || m_draw_color == Color.FromArgb(0,128,255))
			{
				textColor = Brushes.White;
			}
			else
			{
				textColor = Brushes.Black;
			}

			Font f = new Font("Tahoma", 4, FontStyle.Regular, GraphicsUnit.Millimeter);
			//pg.FillRectangle(sb, 0, 264, 45, 45);
			//pg.DrawString(m_text_line, f, textColor, 12f, 272f);
			pg.DrawString(m_text_line, f, textColor, m_upper_left.X+3, m_upper_left.Y+4);
		}

		private void DrawSquare(Graphics pg)
		{
			// Brush Object
			SolidBrush sb = new SolidBrush(m_draw_color);

			int width = Math.Abs(m_upper_left.X - m_lower_right.X);
			int height = Math.Abs(m_upper_left.Y - m_lower_right.Y);

			pg.FillRectangle(sb, m_upper_left.X, m_upper_left.Y, width, height);
		}

		public string Text
		{
			get{return m_text_line;}
			set{m_text_line = value;}
		}


	}
}
