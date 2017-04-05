using System;
using System.Drawing;
using System.Windows.Forms;
using LumiSoft.UI.Controls;

namespace LumiSoft.UI
{
	/// <summary>
	/// Contains common functions for painting.
	/// </summary>
	public class Painter
	{
		/// <summary>
		/// 
		/// </summary>
		public Painter()
		{			
		}


		#region function DrawButton

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="viewStyle"></param>
		/// <param name="buttonRect"></param>
		/// <param name="border_hot"></param>
		/// <param name="btn_hot"></param>
		/// <param name="btn_pressed"></param>
		public static void DrawButton(Graphics g,ViewStyle viewStyle,Rectangle buttonRect,bool border_hot,bool btn_hot,bool btn_pressed)
		{
			if(btn_hot){
				if(btn_pressed){
					g.FillRectangle(new SolidBrush(viewStyle.ButtonPressedColor),buttonRect);
				}
				else{
					g.FillRectangle(new SolidBrush(viewStyle.ButtonHotColor),buttonRect);
				}
			}
			else{
				g.FillRectangle(new SolidBrush(viewStyle.ButtonColor),buttonRect);
			}

			//----- Draw border around button ----------------------------//
			// Append borders to button
			buttonRect = new Rectangle(buttonRect.X-1,buttonRect.Y-1,buttonRect.Width+1,buttonRect.Height+1);
			if(border_hot || btn_hot || btn_pressed){
				g.DrawRectangle(new Pen(viewStyle.BorderHotColor),buttonRect);
			}
			else{
				g.DrawRectangle(new Pen(viewStyle.BorderColor),buttonRect);
			}
		}

		#endregion

		#region function DrawBorder

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="viewStyle"></param>
		/// <param name="controlRect"></param>
		/// <param name="hot"></param>
		public static void DrawBorder(Graphics g,ViewStyle viewStyle,Rectangle controlRect,bool hot)
		{
			controlRect = new Rectangle(controlRect.X,controlRect.Y,controlRect.Width - 1,controlRect.Height - 1);

			if(hot){
				g.DrawRectangle(new Pen(viewStyle.BorderHotColor),controlRect);
			}
			else{
				g.DrawRectangle(new Pen(viewStyle.BorderColor),controlRect);
			}
		}

		#endregion

		#region function DrawArea

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="drawRect"></param>
		/// <param name="borderColor"></param>
		/// <param name="fillColor"></param>
		public static void DrawArea(Graphics g,Rectangle drawRect,Color borderColor,Color fillColor)
		{
		}

		#endregion

		#region function DrawIcon

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="icon"></param>
		/// <param name="drawRect"></param>
		/// <param name="grayed"></param>
		/// <param name="pushed"></param>
		public static void DrawIcon(Graphics g,Icon icon,Rectangle drawRect,bool grayed,bool pushed)
		{
			// If Graphics or Icon isn't valid,
			// just skip this function.
			if(g == null || icon == null){
				return;
			}

			// Icon pushed state, update icon location.
			if(pushed){
				drawRect.Location = new Point(drawRect.X + 1,drawRect.Y + 1);
			}

			//----- Draw Icon ---
			if(grayed){
				// Draw grayed icon
				Size s = new Size(drawRect.Size.Width-1,drawRect.Size.Height-1);
				ControlPaint.DrawImageDisabled(g,new Bitmap(icon.ToBitmap(),s),drawRect.X,drawRect.Y,Color.Transparent);
			}
			else{
				// Draw normal icon
				g.DrawIcon(icon,drawRect);
			}
		}

		#endregion

	}
}
