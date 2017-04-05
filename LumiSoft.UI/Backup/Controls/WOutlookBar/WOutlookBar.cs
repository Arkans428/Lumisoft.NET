using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace LumiSoft.UI.Controls.WOutlookBar
{	
	/// <summary>
	/// 
	/// </summary>
	public delegate void ItemClickedEventHandler(object sender,ItemClicked_EventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public delegate void BarClickedEventHandler(object sender,BarClicked_EventArgs e);

	/// <summary>
	/// OutlookBar control.
	/// </summary>
	[DefaultEvent("ItemClicked"),]
	public class WOutlookBar : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public event ItemClickedEventHandler ItemClicked = null;

		/// <summary>
		/// 
		/// </summary>
		public event BarClickedEventHandler BarClicked = null;

		private bool      m_UseStaticViewStyle   = true;
		private int       m_ActiveBarIndex       = -1;
		private int       m_DefaultTextSpacing   = 3;
		private ImageList m_ImageList            = null;
		private ImageList m_ImageListSmall       = null;
		private bool	  m_AllowItemsStuck      = true;
		private Bars      m_pBars                = null;
		private Icon      m_UpButtonIcon         = null;
		private Icon      m_DownButtonIcon       = null;
		private ViewStyle m_ViewStyle            = null;
		private HitInfo   m_LastHitInfo          = null;   // Holds last hitted object info.
		private Item      m_StuckenItem          = null;   // Holds referance to Stucken item.
		private bool      m_BeginUpdate          = false;  // Holds beginupdate falg.	
		private Rectangle m_ActiveBarClientRect;
        
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WOutlookBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			SetStyle(ControlStyles.ResizeRedraw,true);
			SetStyle(ControlStyles.DoubleBuffer  | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);
		
			m_pBars     = new Bars(this);						
			m_ViewStyle = new ViewStyle();

			m_ViewStyle.StyleChanged += new ViewStyleChangedEventHandler(this.OnViewStyleChanged);
			ViewStyle.staticViewStyle.StyleChanged += new ViewStyleChangedEventHandler(this.OnStaticViewStyleChanged);

			m_UpButtonIcon   = Core.LoadIcon("up.ico");
			m_DownButtonIcon = Core.LoadIcon("down.ico");

		}

		#region function Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				ViewStyle.staticViewStyle.StyleChanged -= new ViewStyleChangedEventHandler(this.OnStaticViewStyleChanged);

				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion


		#region Events handling

		#region function OnViewStyleChanged(2)

		private void OnStaticViewStyleChanged(object sender,ViewStyle_EventArgs e)
		{	
			if(m_UseStaticViewStyle){
				m_ViewStyle.CopyFrom(ViewStyle.staticViewStyle);
			}
		}

		private void OnViewStyleChanged(object sender,ViewStyle_EventArgs e)
		{
		//	if(m_Initing){
		//		return;
		//	}			
			this.UpdateAll();

     //		OnViewStyleChanged(e);
		}

		#endregion

		#endregion


		#region Drawing stuff

		#region function OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);

			if(m_BeginUpdate){
				return;
			}

			e.Graphics.Clear(m_ViewStyle.BarClientAreaColor);

			if(this.Bars.Count > 0){
				DrawBars(e.Graphics);				
				DrawVisibleBarItems(e.Graphics);
				DrawScrollButtons(e.Graphics);
			}
		}

		#endregion

		#region function DrawBars

		private void DrawBars(Graphics g)
		{			
			foreach(Bar bar in m_pBars){

				Point mPt = this.PointToClient(Control.MousePosition);								
				DrawBar(g,bar,bar.BarRect.Contains(mPt),Control.MouseButtons == MouseButtons.Left);
			}
		}

		#endregion

		#region function DrawBar

		private void DrawBar(Graphics g,Bar bar,bool hot,bool pressed)
		{
			//---- Draw bar
			if(hot){
				g.FillRectangle(new SolidBrush(pressed ? m_ViewStyle.BarPressedColor : m_ViewStyle.BarHotColor),bar.BarRect);
				g.DrawRectangle(new Pen(m_ViewStyle.BarHotBorderColor),bar.BarRect);
			}
			else{
				g.FillRectangle(new SolidBrush(m_ViewStyle.BarColor),bar.BarRect);
				g.DrawRectangle(new Pen(m_ViewStyle.BarBorderColor),bar.BarRect);
			}

			//---- Draw bar caption -----------------------//		
			g.DrawString(bar.Caption,bar.Font,new SolidBrush(hot ? m_ViewStyle.BarHotTextColor : m_ViewStyle.BarTextColor),bar.BarRect,bar.BarStringFormat);
			//----------------------------------------------//
		}

		#endregion

		#region function DrawVisibleBarItems

		private void DrawVisibleBarItems(Graphics g)
		{			
			Bar bar = this.ActiveBar;
			if(bar != null){            
				g.SetClip(bar.BarClientRect);

				for(int i=bar.FirstVisibleIndex;i<bar.Items.Count;i++){
					Item item = bar.Items[i];
					Point mPt = this.PointToClient(Control.MousePosition);
					bool hot  = item.Bounds.Contains(mPt) && bar.IsItemVisible(item) && !this.UpScrollBtnRect.Contains(mPt)&& !this.DownScrollBtnRect.Contains(mPt);				    
					DrawItem(g,item,bar,hot,Control.MouseButtons == MouseButtons.Left);
				}

				g.ResetClip();
			}
		}

		#endregion

		#region function DrawItem

		private void DrawItem(Graphics g,Item item,Bar bar,bool hot,bool pressed)
		{
			SolidBrush textBrush = new SolidBrush(hot ? m_ViewStyle.BarItemHotTextColor : m_ViewStyle.BarItemTextColor);
			ItemsStyle itemStyle = bar.ItemsStyleCurrent;

			if(hot || item.Equals(m_StuckenItem)){
				Rectangle iRect = new Rectangle(item.Bounds.Location,item.Bounds.Size);												
				//--- If item style is IconSelect
				if(itemStyle == ItemsStyle.IconSelect){
					iRect = new Rectangle(((this.Width-32)/2)-1,item.Bounds.Y+2,34,34);
				}

				//--- if not stucken(selected) item
				if(!item.Equals(m_StuckenItem)){
					g.FillRectangle(new SolidBrush(pressed ? m_ViewStyle.BarItemPressedColor : m_ViewStyle.BarItemHotColor),iRect);
				}
				else{ //---- If stucken(selected) item
					g.FillRectangle(new SolidBrush(m_ViewStyle.BarItemSelectedColor),iRect);
					textBrush = new SolidBrush(m_ViewStyle.BarItemSelectedTextColor);
				}

				// Draw border
				g.DrawRectangle(new Pen(m_ViewStyle.BarItemBorderHotColor),iRect);
			}
			else{
				g.FillRectangle(new SolidBrush(m_ViewStyle.BarClientAreaColor),item.Bounds.X,item.Bounds.Y,item.Bounds.Width+1,item.Bounds.Height+1);
			}
			
			//---- Draw item image 
			Image image = item.Image;
			if(image != null){
				g.DrawImage(image,item.ImageRect);
			}

			//---- Draw item text 
			g.DrawString(item.Caption,bar.ItemsFont,textBrush,item.TextRect,bar.ItemsStringFormat);
		}

		#endregion

		#region function DrawScrollButtons

		private void DrawScrollButtons(Graphics g)
		{
			Point mPt = this.PointToClient(Control.MousePosition);

			//--- up arrow
			if(this.IsUpScrollBtnVisible){
				Rectangle upRect = this.UpScrollBtnRect;
				bool hot = upRect.Contains(mPt);
				Painter.DrawButton(g,ViewStyle.staticViewStyle,upRect,hot,hot,Control.MouseButtons == MouseButtons.Left && hot);

				Rectangle imgRect = new Rectangle(upRect.X,upRect.Y,13,15);
				Painter.DrawIcon(g,m_UpButtonIcon,imgRect,false,false);
			}
			
			if(this.IsDownScrollBtnVisible){
				//--- down arrow
				Rectangle downRect = this.DownScrollBtnRect;
				bool hot = downRect.Contains(mPt);
				Painter.DrawButton(g,ViewStyle.staticViewStyle,downRect,hot,hot,Control.MouseButtons == MouseButtons.Left && hot);

				Rectangle imgRect = new Rectangle(downRect.X,downRect.Y,13,15);
				Painter.DrawIcon(g,m_DownButtonIcon,imgRect,false,false);
			}
		}

		#endregion

		#region function DrawObject

		private void DrawObject(HitInfo hitInfo,bool unDrawOld)
		{
			using(Graphics g = this.CreateGraphics()){
				//--- Draw old object as normal
				if(unDrawOld && m_LastHitInfo != null){
					if(m_LastHitInfo.HittedObject == HittedObject.Item){
						g.SetClip(this.ActiveBar.BarClientRect);
						DrawItem(g,m_LastHitInfo.HittedItem,this.ActiveBar,false,false);
						g.ResetClip();
					}

					if(m_LastHitInfo.HittedObject == HittedObject.Bar){
						DrawBar(g,m_LastHitInfo.HittedBar,false,false);
					}
				}
				
				if(hitInfo.HittedObject == HittedObject.Item){
					g.SetClip(this.ActiveBar.BarClientRect);
					DrawItem(g,hitInfo.HittedItem,this.ActiveBar,true,Control.MouseButtons == MouseButtons.Left);
					g.ResetClip();					
				}

				if(hitInfo.HittedObject == HittedObject.Bar){
					DrawBar(g,hitInfo.HittedBar,true,Control.MouseButtons == MouseButtons.Left);
				}

				DrawScrollButtons(g);						
			}
		}

		#endregion

		#endregion
        

		#region function CalculateBarInfo

		/// <summary>
		/// Calculates bars and active bar's items location,sizes,... .
		/// </summary>
		private void CalculateBarInfo()
		{
			int barTop = 1;

			using(Graphics g = this.CreateGraphics()){

				//--- Calculate visible Bar's client rectangle
				int visibleBarClientHeight = this.ClientSize.Height - (CalculateBarsHeight(g)) - 2 - 1;

				//--- loop through all bars ------//
				for(int i=0;i<m_pBars.Count;i++){
					Bar bar = this.Bars[i];
					bar.BarClientRect = new Rectangle(0,0,0,0);

					//--- Calculate text rect Height
					SizeF bSize       = g.MeasureString(bar.Caption,bar.Font,this.ClientSize.Width-2);
					int barTextHeight = (int)(Math.Ceiling(bSize.Height));

					int barHeight = barTextHeight + m_DefaultTextSpacing*2;
				
					//--- If upper bars ---------
					if(i < m_ActiveBarIndex + 1){
						bar.BarRect = new Rectangle(1,barTop,this.ClientSize.Width-3,barHeight);
					
						//--- If active bar
						if(i == m_ActiveBarIndex){
							bar.BarClientRect = new Rectangle(1,bar.BarRect.Bottom,this.Width-2,visibleBarClientHeight);
							m_ActiveBarClientRect = new Rectangle(1,bar.BarRect.Bottom,this.Width-2,visibleBarClientHeight);

							int top = bar.BarRect.Bottom + 3;

							//--- Calculate items rect --------------------------//
							for(int it=bar.FirstVisibleIndex;it<bar.Items.Count;it++){
								Item item = bar.Items[it];

								int itemWidth  = this.Width-3;
								int itemHeight = CalculateItemHeight(g,item);
								item.Bounds = new Rectangle(1,top,itemWidth,itemHeight);
								top += itemHeight + 1;
							}
							//---------------------------------------------------//
						}
					}
					//--- If lower bars
					else{
						bar.BarRect = new Rectangle(1,barTop + visibleBarClientHeight,this.Width-3,barHeight);
					}

					barTop += barHeight + 1;
				}
				//--------------------------------//
			}
		}

		#endregion

		#region function CalculateBarsHeight

		/// <summary>
		/// Calculates height which is neede for bars.
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		private int CalculateBarsHeight(Graphics g)
		{
			int retVal = 0;
			foreach(Bar bar in this.Bars){
				SizeF bSize       = g.MeasureString(bar.Caption,bar.Font,this.ClientSize.Width-2);
				int barTextHeight = (int)(Math.Ceiling(bSize.Height));
				retVal = retVal + barTextHeight + m_DefaultTextSpacing*2 + 1;
			}

			retVal--;

			return retVal;
		}

		#endregion

		#region function CalculateItemHeight

		private int CalculateItemHeight(Graphics g,Item item)
		{
			int retVal = 0;
			SizeF iSize;

			switch(item.Bar.ItemsStyleCurrent)
			{
				case ItemsStyle.SmallIcon:
					iSize = g.MeasureString(item.Caption,item.Bar.ItemsFont,this.Width-18);
					retVal = (int)(Math.Ceiling(iSize.Height)) + 8; // spacing
					break;

				default:
					iSize = g.MeasureString(item.Caption,item.Bar.ItemsFont,this.Width-3);
					retVal = (int)(Math.Ceiling(iSize.Height)) + 43; // Icon + spacing
					break;
			}

			return retVal;
		}

		#endregion

	
		#region override function OnMouseUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if(e.Button != MouseButtons.Left){
				return;
			}

			HitInfo hitInfo = new HitInfo(new Point(e.X,e.Y),this);
					
			//--- If hitted bar is different than activeBar, set hittedBar as activeBar.
			if(hitInfo.HittedObject == HittedObject.Bar && hitInfo.HittedBar.Index != m_ActiveBarIndex){
				m_ActiveBarIndex = hitInfo.HittedBar.Index;
				this.UpdateAll();
				
				// Raise bar clicked event
				OnBarClicked(hitInfo.HittedBar);
				return;
			}

			//--- If item clicked
			if(hitInfo.HittedObject == HittedObject.Item){
				if(hitInfo.HittedItem.AllowStuck){
					if(!hitInfo.HittedItem.Equals(m_StuckenItem)){
						Item oldItem  = m_StuckenItem;
						m_StuckenItem = hitInfo.HittedItem;

						//--- Redraw old stucken item					
						if(oldItem != null && oldItem.Bar.IsItemVisible(oldItem)){
							using(Graphics g = this.CreateGraphics()){
								g.SetClip(this.ActiveBar.BarClientRect);
								DrawItem(g,oldItem,oldItem.Bar,false,false);
							}
						}

						// Raise item clicked event
						OnItemClicked(hitInfo.HittedItem);
					}
				}
				else{
					// Raise item clicked event
					OnItemClicked(hitInfo.HittedItem);
					this.Invalidate();
				}
			}

			//--- If up scroll button clicked
			Bar activeBar = this.ActiveBar;
			if(hitInfo.HittedObject == HittedObject.UpScrollButton){
				activeBar.FirstVisibleIndex--;
				this.UpdateAll();
				return;
			}

			//--- If down scroll button clicked
			if(hitInfo.HittedObject == HittedObject.DownScrollButton){
				activeBar.FirstVisibleIndex++;
				this.UpdateAll();
				return;
			}

			// By default, redraw last hitted object
			DrawObject(hitInfo,false);							
		}

		#endregion

		#region override function OnMouseDown

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if(m_LastHitInfo != null){		
				DrawObject(m_LastHitInfo,false);
			}
		}

		#endregion

		#region override function OnMouseMove

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			HitInfo hitInfo = new HitInfo(new Point(e.X,e.Y),this);
						
			if(!hitInfo.Compare(m_LastHitInfo)){					
				DrawObject(hitInfo,true);
				m_LastHitInfo = hitInfo;
			}			
		}

		#endregion

		#region override function OnMouseWheel

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			
			if(this.ActiveBar != null){
				if(e.Delta > 0 && this.ActiveBar.FirstVisibleIndex > 0){
					this.ActiveBar.FirstVisibleIndex--;
					this.UpdateAll();
				}

				if(e.Delta < 0 && !this.ActiveBar.IsLastVisible){
					this.ActiveBar.FirstVisibleIndex++;
					this.UpdateAll();
				}
			}
		}

		#endregion

		#region override function OnMouseLeave

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);

			this.Invalidate();			
			m_LastHitInfo = null;		
		}

		#endregion

		#region override function OnSizeChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdateAll();
		}

		#endregion

		
		#region function UpdateAll

		/// <summary>
		/// Calculates bar info and redraws all.
		/// </summary>
		internal void UpdateAll()
		{
			if(this.Bars.Count > 0 && m_ActiveBarIndex == -1){
				m_ActiveBarIndex = 0;
			}

			if(!m_BeginUpdate){
				CalculateBarInfo();

				// Update hitInfo
				Point mPt = this.PointToClient(Control.MousePosition);
				m_LastHitInfo = new HitInfo(new Point(mPt.X,mPt.Y),this);

				this.Invalidate();								
			}
		}

		#endregion


		#region function BeginUpdate / EndUpdate

		/// <summary>
		/// 
		/// </summary>
		public void BeginUpdate()
		{
			m_BeginUpdate = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void EndUpdate()
		{
			m_BeginUpdate = false;
			this.UpdateAll();
		}

		#endregion


		#region function GetHittedObject

		/// <summary>
		/// Gets currently hitted object.
		/// </summary>
		/// <returns></returns>
		public HitInfo GetHittedObject()
		{			
			return new HitInfo(this.PointToClient(Control.MousePosition),this);
		}

		#endregion
		

		#region Properties Implementation

		/// <summary>
		/// Gets or sets if to use static viewStyle.
		/// </summary>
		[		
		Category("View style")
		]
		public virtual bool UseStaticViewStyle
		{
			get{ return m_UseStaticViewStyle; }

			set
			{
				m_UseStaticViewStyle = value; 

				if(value){
					ViewStyle.staticViewStyle.CopyTo(m_ViewStyle);
					m_ViewStyle.ReadOnly = true;
				}
				else{
					m_ViewStyle.ReadOnly = false;
				}
			}
		}

		/// <summary>
		/// Gets controls active viewStyle.
		/// </summary>
		[
		TypeConverter(typeof(ExpandableObjectConverter)),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		]
		public ViewStyle ViewStyle
		{
			get{ return m_ViewStyle; }
		}

		/// <summary>
		/// Gets or sets items imagelist.
		/// </summary>
		public ImageList ImageList
		{
			get{ return m_ImageList; }

			set{ 
				m_ImageList = value;
				if(this.Bars.Count > 0){
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets items small imagelist.
		/// </summary>
		public ImageList ImageListSmall
		{
			get{ return m_ImageListSmall; }

			set{ 
				m_ImageListSmall = value;
				if(this.Bars.Count > 0){
					this.Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Gets bars collection.
		/// </summary>
		public Bars Bars
		{
			get{ return m_pBars; }
		}

		/// <summary>
		/// Gets or sets active bar.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Bar ActiveBar
		{
			get{
				if(m_ActiveBarIndex > -1){
					return this.Bars[m_ActiveBarIndex];
				}
				else{
					return null;
				}
			}

			set{
				if(value != null){
					m_ActiveBarIndex = value.Index;
					this.UpdateAll();
				}
				else{
					m_ActiveBarIndex = -1;
					this.UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets if items can stuck.
		/// </summary>
		public bool AllowItemsStuck
		{
			get{ return m_AllowItemsStuck; }

			set{ m_AllowItemsStuck = value; }
		}

		/// <summary>
		/// Gets currently stucken item.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Item StuckenItem
		{
			get{ return m_StuckenItem; }

			set{
				m_StuckenItem = value;
				if(m_StuckenItem != null && m_StuckenItem.AllowStuck){
					this.ActiveBar = m_StuckenItem.Bar;					

					// Raise event
					OnItemClicked(m_StuckenItem);
				}
				else{
					m_StuckenItem = null;
				}

				this.Invalidate(false);
			}
		}


		#region Internal Properties

		internal Rectangle UpScrollBtnRect
		{
			get{
				if(this.IsUpScrollBtnVisible){
					Rectangle upRect = new Rectangle(this.ActiveBar.BarClientRect.Right-19,this.ActiveBar.BarClientRect.Top+6,14,14);				
					return upRect; 
				}
				else{
					return new Rectangle(0,0,0,0);
				}
			}
		}

		internal Rectangle DownScrollBtnRect
		{
			get{
				if(this.IsDownScrollBtnVisible){
					Rectangle downRect = new Rectangle(this.ActiveBar.BarClientRect.Right-19,this.ActiveBar.BarClientRect.Bottom-19,14,14);
					return downRect; 
				}
				else{
					return new Rectangle(0,0,0,0);
				}
			}
		}

		internal bool IsUpScrollBtnVisible
		{
			get{
				if(this.ActiveBar != null && this.ActiveBar.FirstVisibleIndex > 0){
					return true; 
				}
				else{
					return false;
				}
			}
		}

		internal bool IsDownScrollBtnVisible
		{
			get{
				if(this.ActiveBar != null){
					return !this.ActiveBar.IsLastVisible;
				}
				else{
					return false;
				}
			}				
		}

		internal Rectangle ActiveBarClientRect
		{
			get{ return m_ActiveBarClientRect; }
		}

		#endregion
		
		#endregion

		#region Events Implementation

		#region function OnItemClicked

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		protected void OnItemClicked(Item item)
		{
			if(this.ItemClicked != null){
				ItemClicked_EventArgs oArgs = new ItemClicked_EventArgs(item);
				this.ItemClicked(this,oArgs);
			}
		}

		#endregion

		#region function OnBarClicked

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bar"></param>
		protected void OnBarClicked(Bar bar)
		{
			if(this.BarClicked != null){
				BarClicked_EventArgs oArgs = new BarClicked_EventArgs(bar);
				this.BarClicked(this,oArgs);
			}
		}

		#endregion

		#endregion

	}
}
