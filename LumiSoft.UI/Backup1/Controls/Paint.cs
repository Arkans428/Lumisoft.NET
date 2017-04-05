using System;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Paing methods.
	/// </summary>
	public class Paint
    {
        #region method DrawText

        /// <summary>
        /// Draws specified text.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="rect"></param>
        /// <param name="hzAlign"></param>
        public static void DrawText(Graphics g,Color color,Font font,String text,Rectangle rect,HorizontalAlignment hzAlign)
		{
			StringFormat format  = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags   = StringFormatFlags.LineLimit;

			switch(hzAlign){
				case HorizontalAlignment.Center:
					format.Alignment = StringAlignment.Center;
					break;
				case HorizontalAlignment.Left:
					format.Alignment = StringAlignment.Near;
					break;
				case HorizontalAlignment.Right:
					format.Alignment = StringAlignment.Far;
					break;
			}
			
			g.DrawString(text,font,new SolidBrush(color),rect,format);
        }

        #endregion

        #region method DrawTriangle

        /// <summary>
        /// Draws triangle to specified rectangle.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        /// <param name="direction"></param>
        public static void DrawTriangle(Graphics g,Color color,Rectangle rect,Direction direction)
		{			
			int x = rect.X;
			int y = rect.Y;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
			switch(direction)
			{
				case Direction.Up:					
					g.FillPolygon(new SolidBrush(color),new Point[]{new Point(x,y + rect.Height),new Point(x + (rect.Width / 2),y),new Point(x + rect.Width,y + rect.Height)});
					break;
					
				case Direction.Down:					
					g.FillPolygon(new SolidBrush(color),new Point[]{new Point(x,y),new Point(x + (rect.Width / 2),y + rect.Height),new Point(x + rect.Width,y)});
					break;
					
				case Direction.Left:					
					g.FillPolygon(new SolidBrush(color),new Point[]{new Point(x,y + (rect.Height / 2)),new Point(x + rect.Width,y),new Point(x + rect.Width,y + rect.Height)});
					break;
					
				case Direction.Right:					
					g.FillPolygon(new SolidBrush(color),new Point[]{new Point(x,y),new Point(x + rect.Width,y + (rect.Height / 2)),new Point(x,y + rect.Height)});				
					break;
			}
			
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        #endregion
    }
}
