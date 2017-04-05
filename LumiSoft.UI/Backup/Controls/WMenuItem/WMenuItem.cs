using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    ///    Summary description for WMenuItem.
    /// </summary>
    public class WMenuItem : MenuItem
    {
		private Color HotTrackColor;
		private Brush HotTrackBrush       = null;
		private Color SelectedColor;
		private Brush SelectedBrush       = null;
		private Color SelectedBorderColor;
		private Pen SelectedBorderPen     = null;
		private Color GrayColor;
		private Brush GrayBrush           = null;
		private Pen GrayPen               = null;
		private Color SelectedShadowColor;
		
		private Font Font = null;
		private Bitmap Picture, GrayPicture;
		private StringFormat Format;
		private bool DisplayIconFrame = true;
		private bool ChildDisplayIconFrame = true;


		/// <summary>
		/// 
		/// </summary>
        public WMenuItem() 
        {	
			this.Font = new Font("Microsoft Sans Serif", 8);
			this.Format = new StringFormat();
			this.OwnerDraw = true;

			HotTrackColor       = Color.FromArgb(225, 219, 207);
			HotTrackBrush       = new SolidBrush(HotTrackColor);
			SelectedColor       = Color.FromArgb(187, 202, 191);
			SelectedBrush       = new SolidBrush(SelectedColor);
			SelectedBorderColor = Color.FromArgb(0, 128, 128);
			SelectedBorderPen   = new Pen(SelectedBorderColor);
			GrayColor           = Color.FromArgb(220, 212, 198);
			GrayBrush           = new SolidBrush(GrayColor);
			GrayPen             = new Pen(GrayColor);
			SelectedShadowColor = Color.FromArgb(121, 147, 147);
        }

		
		#region function Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{			
			base.Dispose( disposing );
		}

		#endregion
			

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDrawItem(DrawItemEventArgs e) 
		{
			base.OnDrawItem(e);

			if(Parent is MainMenu) 
			{		
				if((e.State & DrawItemState.HotLight) != 0){	
					e.Graphics.FillRectangle(SelectedBrush, e.Bounds);
					e.Graphics.DrawRectangle(SelectedBorderPen, e.Bounds.Left,e.Bounds.Y,e.Bounds.Width - 2,e.Bounds.Height - 1);
				} 
				else if((e.State & DrawItemState.Selected) != 0) 
				{
					e.Graphics.FillRectangle(SelectedBrush, e.Bounds);
				
					e.Graphics.DrawLine(new Pen(Color.Gray),e.Bounds.Left+1,e.Bounds.Top,e.Bounds.Right-1,e.Bounds.Top);
					e.Graphics.DrawLine(new Pen(Color.Gray),e.Bounds.Left+1,e.Bounds.Top,e.Bounds.Left+1,e.Bounds.Bottom-1);
					e.Graphics.DrawLine(new Pen(Color.DarkGray),e.Bounds.Right-1,e.Bounds.Top,e.Bounds.Right-1,e.Bounds.Bottom-1);

				}			
				else{
					e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
				}

				e.Graphics.DrawString(Text,this.Font,((e.State & DrawItemState.Inactive) != 0 ? Brushes.Gray : Brushes.Black), new PointF(e.Bounds.Left + 4, e.Bounds.Top + 4), Format);
			
			}   // Draw Context Menu Items -------------------------------------------------------
			else 
			{
				// draw Item's selected state
				if((e.State & DrawItemState.Selected) != 0) 
				{
					if(this.Enabled){					
						e.Graphics.FillRectangle(SelectedBrush, e.Bounds);
						e.Graphics.DrawRectangle(SelectedBorderPen, e.Bounds.Left,e.Bounds.Y,e.Bounds.Width - 2,e.Bounds.Height - 1);
					}
				} 
				else 
				{
					e.Graphics.FillRectangle(Brushes.White, e.Bounds);
					if(ShouldDisplayIconFrame){
						e.Graphics.FillRectangle(GrayBrush, new Rectangle(e.Bounds.Left, e.Bounds.Top, 24, 24));
					}
				}

				// draw item's image
				if(ShouldDisplayIconFrame && Picture != null) 
				{				
					if(this.Enabled){
						if((e.State & DrawItemState.Selected) != 0){
							e.Graphics.DrawImage(GrayPicture, e.Bounds.Left + 5, e.Bounds.Top + 5);
							e.Graphics.DrawImage(Picture, e.Bounds.Left + 3, e.Bounds.Top + 3);
						}
						else{
							e.Graphics.DrawImage(Picture,e.Bounds.Left + 4,e.Bounds.Top + 4);
						}
					}
					else{
						ControlPaint.DrawImageDisabled(e.Graphics,Picture,e.Bounds.Left + 4, e.Bounds.Top + 4,Color.Transparent);
					}				
				}
				
				// If not separator
				if(this.Text != "-") 
				{
					Rectangle DisplayRect = new Rectangle(e.Bounds.Left + (ShouldDisplayIconFrame ? 24 : 0) + 6, e.Bounds.Top + 6, e.Bounds.Width - (ShouldDisplayIconFrame ? 24 : 0) - 18, e.Bounds.Bottom - 6);
					Format.Alignment = StringAlignment.Near;

					if(this.Enabled){
						e.Graphics.DrawString(Text,this.Font, Brushes.Black, DisplayRect, Format);
					}
					else{
						e.Graphics.DrawString(Text,this.Font, Brushes.Gray, DisplayRect, Format);
					}
					

					if(Shortcut != Shortcut.None){
						Format.Alignment = StringAlignment.Far;
						e.Graphics.DrawString(ShortcutString,this.Font,Brushes.Black, DisplayRect, Format);
					}
				} 
				else
					e.Graphics.DrawLine(Pens.Gray, e.Bounds.Left + (ShouldDisplayIconFrame ? 24 : 0) + 6, e.Bounds.Top + 1, e.Bounds.Right, e.Bounds.Top + 1);
			
			}
		
		}

		#region function DrawParentItem

		private void DrawParentItem(Graphics gEx,DrawItemEventArgs e)
		{
			int width  = e.Bounds.Width;
			int height = e.Bounds.Height;

			// Draw HotLight
			if((e.State & DrawItemState.HotLight) != 0) 
			{	
				gEx.FillRectangle(SelectedBrush, gEx.ClipBounds);
				gEx.DrawRectangle(SelectedBorderPen,0,1,width - 1,height - 2);
			} 
			else
			{
				if((e.State & DrawItemState.Selected) != 0) 
				{
					gEx.FillRectangle(SelectedBrush, gEx.ClipBounds);
				
					gEx.DrawLine(new Pen(Color.Gray),1,0,width-1,0);
					gEx.DrawLine(new Pen(Color.Gray),1,0,1,height-1);
					gEx.DrawLine(new Pen(Color.DarkGray),width-1,0,width-1,height-1);

				}			
				else 
				{
					gEx.FillRectangle(SystemBrushes.Control, gEx.ClipBounds);
				}
			}

			// Draw text
			gEx.DrawString(Text, e.Font, ((e.State & DrawItemState.Inactive) != 0 ? Brushes.Gray : Brushes.Black), new PointF(5,3), Format);
			
		}

		#endregion

		#region function Draw_Item

		private void Draw_Item(Graphics gEx,DrawItemEventArgs e)
		{
			int width  = e.Bounds.Width;
			int height = e.Bounds.Height;

			bool enabled  = this.Enabled;
			bool selected = (e.State & DrawItemState.Selected) != 0;

			// Draw Enabled state
			if(enabled)
			{
				// Draw Selected state
				if(selected)
				{
					gEx.FillRectangle(SelectedBrush, gEx.ClipBounds);
					gEx.DrawRectangle(SelectedBorderPen, 0,0,width - 2,height - 1);

					// draw item's image
					if(ShouldDisplayIconFrame && Picture != null) 
					{					
						gEx.DrawImage(GrayPicture,5, 5);
						gEx.DrawImage(Picture, 3, 3);
					}
				}
				else // Draw Normal state
				{
					gEx.FillRectangle(Brushes.White, gEx.ClipBounds);

					if(ShouldDisplayIconFrame){ 
						gEx.FillRectangle(GrayBrush, new Rectangle(0,0, 24, 24));
					
						// draw item's image
						if(Picture != null){			
							gEx.DrawImage(Picture, 4, 4);
						}
					}					
				}
				
				// Draw Text
				if(this.Text != "-") 
				{
					Rectangle DisplayRect = new Rectangle((ShouldDisplayIconFrame ? 24 : 0) + 6, 6, width - (ShouldDisplayIconFrame ? 24 : 0) - 18, height - 6);
					Format.Alignment = StringAlignment.Near;

					gEx.DrawString(Text, e.Font, Brushes.Black, DisplayRect, Format);
				
					if(Shortcut != Shortcut.None) 
					{
						Format.Alignment = StringAlignment.Far;
						gEx.DrawString(ShortcutString, e.Font, Brushes.Black, DisplayRect, Format);
					}
				} 
				else{// Draw Separator
					gEx.DrawLine(Pens.Gray, (ShouldDisplayIconFrame ? 24 : 0) + 6, 1, width, 1);
				}
			
			}
			else // Draw Disabled state
			{
				// Draw Selected state (Item Disabled)
				if(selected)
				{
					gEx.FillRectangle(SelectedBrush, gEx.ClipBounds);
					gEx.DrawRectangle(new Pen(Color.Red), 0,0,width - 2,height - 1);
				}
				else
				{
					gEx.FillRectangle(Brushes.White, gEx.ClipBounds);

					if(ShouldDisplayIconFrame){ 
						gEx.FillRectangle(GrayBrush, new Rectangle(0,0, 24, 24));					
					}
				}

				if(ShouldDisplayIconFrame && Picture != null){ 							
					gEx.DrawImage(GrayPicture, 4, 4);					
				}

				Rectangle DisplayRect = new Rectangle((ShouldDisplayIconFrame ? 24 : 0) + 6, 6, width - (ShouldDisplayIconFrame ? 24 : 0) - 18, height - 6);
				Format.Alignment = StringAlignment.Near;

				gEx.DrawString(Text, e.Font, Brushes.Gray, DisplayRect, Format);
			}	
		}

		#endregion


		#region function OnMeasureItem

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMeasureItem(MeasureItemEventArgs e) 
		{
		//	base.OnMeasureItem(e);

			// If not separator
			if(this.Text != "-") 
			{
				Format.HotkeyPrefix = HotkeyPrefix.Hide;
				e.ItemWidth =  (int)e.Graphics.MeasureString(Text,Font).Width;
				if(Shortcut != Shortcut.None){
					e.ItemWidth += (int)e.Graphics.MeasureString(ShortcutString,Font).Width;
				}

				if(!(this.Parent is MainMenu)){

					if(!this.IsParent){
						e.ItemWidth += 28;
						if(ShouldDisplayIconFrame){
							e.ItemWidth += 24;
						}
					}
					else{
						if(!(this.Parent is MainMenu)){
							e.ItemWidth += 45;
						}
					}
				}				
				e.ItemHeight = (int)e.Graphics.MeasureString(Text,Font).Height + 11;
			} 
			else{ 
				e.ItemHeight = 3;
			}

			base.OnMeasureItem(e);
						
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// 
		/// </summary>
		protected String ShortcutString 
		{
			get {
				Keys k = (Keys)Shortcut;
				return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(k);				
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public bool ShouldDisplayIconFrame 
		{
			get 
			{
				if(Parent is WMenuItem) 
				{
					if(!((WMenuItem)Parent).ChildDisplayIconFrame)
						return false;
					else
						return DisplayIconFrame;
				} 
				else
					return DisplayIconFrame;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Category("Misc"),
		Description("MenuItem Icon"),
		DefaultValue(null),
		]
		public Bitmap ItemBmp
		{
			get{
				return Picture;
			}

			set{			

				if(value != null)
				{
					Picture = value;
					GrayPicture = new Bitmap(Picture);
					for(int y = 0; y < GrayPicture.Height; y++)
						for(int x = 0; x < GrayPicture.Width; x++)
							if(GrayPicture.GetPixel(x, y) != GrayPicture.GetPixel(0, 0))
								GrayPicture.SetPixel(x, y, SelectedShadowColor);						
				
					}						
								
				}
		}

		#endregion


		#region function Copy

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public WMenuItem Copy()
		{
			WMenuItem retVal = new WMenuItem();
			retVal.BarBreak  = this.BarBreak;
			retVal.Break     = this.Break;
			retVal.Checked   = this.Checked;
			retVal.Enabled   = this.Enabled;
		//	retVal.IsParent  = this.IsParent;
			retVal.Index     = this.Index;
			retVal.ItemBmp   = this.ItemBmp;
			retVal.OwnerDraw = this.OwnerDraw;
			retVal.Tag       = this.Tag;
			retVal.Text      = this.Text;
			retVal.Visible   = this.Visible;

			return retVal;
		}

		#endregion
		

    }
}
