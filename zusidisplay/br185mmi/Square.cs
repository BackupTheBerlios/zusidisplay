using System;
using System.Drawing;

namespace MMI.MMIBR185
{
	/// <summary>
	/// Zusammenfassung für Square.
	/// </summary>
	public class Square : DrawingUnit
	{
		bool m_two_lines;

		string m_text_line_2;


		public Square() : base()
		{
			m_two_lines = false;
			m_text_line_2 = "";
		}

		public Square(System.Windows.Forms.UserControl parent, Point ul, Point lr, string text, Color color, bool isFlashing, uint flashing_speed, bool two_lines, string text2) : 
			base(parent, ul, lr, text, color, isFlashing, flashing_speed)
		{
			m_two_lines = two_lines;
			if (m_two_lines) 
			{ 
				m_text_line_2 = text2; 
			}
			else 
			{ 
				m_text_line_2 = ""; 
			}
		}

		public override void Draw(ref Graphics pg)
		{
			if (m_parent != null)
			{

				DrawSquare(ref pg);

				DrawText(ref pg);
			}
		}

		public string Text1
		{
			set{m_text_line = value;}
			get{return m_text_line;}
		}
		public string Text2
		{
			set{m_text_line_2 = value;}
			get{return m_text_line_2;}
		}
		public Point Point1
		{
			get
			{
				return m_upper_left;
			}
			set
			{   
				m_upper_left = value;
			}
		}

		public Point Point2
		{
			get
			{
				return m_lower_right;
			}
			set
			{   
				m_lower_right = value;
			}
		}

		private void DrawText(ref Graphics old_g)
		{
			Graphics pg = old_g;
			Brush textColor;
			if (m_draw_color == Color.FromArgb(0,128,255) || m_draw_color == Color.Black || m_draw_color == Color.DarkSlateGray)
			{
				textColor = Brushes.White;
			}
			else
			{
				textColor = Brushes.Black;
			}

			if (!m_two_lines)
			{
				Font f = new Font("Tahoma", 5.5f, FontStyle.Regular, GraphicsUnit.Millimeter);
				//pg.FillRectangle(sb, 0, 264, 45, 45);
				//pg.DrawString(m_text_line, f, textColor, 12f, 272f);

				if (m_text_line == "TAV")
				{ 
					DrawTAV(ref old_g);
				}
				else if (m_text_line.Length == 1)
				{
					f = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Millimeter);
					if (m_text_line == "T")
					{
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X+2, m_upper_left.Y+1);					}
					else
					{
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X, m_upper_left.Y+1);
					}
				}
				else if (m_text_line.Length == 2)
				{
					if (m_text_line == "85" || m_text_line == "70" || m_text_line == "55" || m_text_line == "95" || m_text_line == "75" || m_text_line == "60")
					{
						f = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Millimeter);
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X-5, m_upper_left.Y+4);
					}
					else
					{
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X+4, m_upper_left.Y+10);
					}
				}
				else 
				{	
					if (m_text_line == "PZB")
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X-2, m_upper_left.Y+10);
					else if (m_text_line == "Befehl")
					{
						f = new Font("Tahoma", 8f);
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X+3, m_upper_left.Y+10);
						pg.DrawString("40", f, textColor, m_upper_left.X+12, m_upper_left.Y+28);
					}
					else if (m_text_line == "Sifa")
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X+18, m_upper_left.Y+10);
					else
						pg.DrawString(m_text_line, f, textColor, m_upper_left.X-1, m_upper_left.Y+10);
				}
			}
			else //two_lines
			{
				Font f = new Font("Tahoma", 4, FontStyle.Regular, GraphicsUnit.Millimeter);

				if (m_text_line.Length > 3)
					pg.DrawString(m_text_line, f , textColor, m_upper_left.X+1, m_upper_left.Y+6);
				else
				{
					if (m_text_line == "NBÜ")
						pg.DrawString(m_text_line, f , textColor, m_upper_left.X+3, m_upper_left.Y+6);
					else 
						pg.DrawString(m_text_line, f , textColor, m_upper_left.X+6, m_upper_left.Y+6);
				}

						if (m_text_line_2.Length > 2)
                    pg.DrawString(m_text_line_2, f , textColor, m_upper_left.X+2, m_upper_left.Y+24);
				else
					pg.DrawString(m_text_line_2, f , textColor, m_upper_left.X+9, m_upper_left.Y+24);
			}

		}

		private void DrawSquare(ref Graphics old_g)
		{
			Graphics pg = old_g;
			// Brush Object
			SolidBrush sb = new SolidBrush(m_draw_color);

			int width = Math.Abs(m_upper_left.X - m_lower_right.X);
			int height = Math.Abs(m_upper_left.Y - m_lower_right.Y);

			if (m_text_line != "TAV")
			{
				pg.FillRectangle(sb, m_upper_left.X, m_upper_left.Y+m_thickness, width, height-m_thickness);
			}
			else
			{
				pg.FillRectangle(Brushes.White, m_upper_left.X, m_upper_left.Y+m_thickness, width, height-m_thickness);
			}
		}

		private void DrawTAV(ref Graphics pg)
		{ 
			Font f = new Font("Tahoma", 5.5f, FontStyle.Bold, GraphicsUnit.Millimeter);
			Brush textColor = Brushes.DarkGreen;
			Pen p = new Pen(textColor, 5);
			pg.DrawString("T", f, textColor, m_upper_left.X+10, m_upper_left.Y+12);
			int radius = 30;
			int x = m_upper_left.X;
			int y = m_upper_left.Y;

			pg.DrawEllipse(p, x+4, y+9, radius, radius);
		}


	}
}
