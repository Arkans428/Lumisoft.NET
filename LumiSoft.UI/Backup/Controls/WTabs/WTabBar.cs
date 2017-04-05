using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WTabs
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void TabChanged_EventHandler(object sender,TabChanged_EventArgs e);

	/// <summary>
	/// Summary description for WTabBar.
	/// </summary>
	public class WTabBar : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
		public event TabChanged_EventHandler TabChanged = null;

		private Tabs      m_pTabs            = null;
		private Tab       m_pSelectedTab     = null;
		private int       m_FVisibleTabIndex = 0;
		private Icon      m_iFirst           = null;
		private Icon      m_iPrev            = null;
		private Icon      m_iNext            = null;
		private Icon      m_iLast            = null;
		private ImageList m_ImgList          = null;
		private object    m_ActiveObj        = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WTabBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			SetStyle(ControlStyles.ResizeRedraw,true);
			SetStyle(ControlStyles.DoubleBuffer  | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);
		
			m_pTabs = new Tabs(this);
			
			m_iFirst = Core.LoadIcon("first.ico");
			m_iPrev  = Core.LoadIcon("prev.ico");
			m_iNext  = Core.LoadIcon("next.ico");
			m_iLast  = Core.LoadIcon("last.ico");

			this.Cursor = Cursors.Hand;
		}

		#region function Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			// 
			// WTabBar
			// 
			this.Name = "WTabBar";
			this.Size = new System.Drawing.Size(150, 32);

		}
		#endregion


		#region function OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			CalcDrawInfo();

			DrawTabs(e.Graphics);
			DrawButtons(e.Graphics);
		}

		#endregion

		#region function DrawTabs

		private void DrawTabs(Graphics g)
		{				
			g.Clear(Color.FromArgb(247, 243, 233));	
			g.DrawRectangle(new Pen(Color.Black),0,0,this.Width-1,this.Height-2);
			g.SetClip(new Rectangle(0,0,this.Width-this.BtnsRect.Width,this.Height));

			foreach(Tab tab in m_pTabs){
				Point mPos = this.PointToClient(Control.MousePosition);
				DrawTab(g,tab,!this.BtnsRect.Contains(mPos) && tab.Bounds.Contains(mPos),tab.Equals(m_pSelectedTab));
			}

			g.ResetClip();
		}

		#endregion

		#region function DrawTab

		private void DrawTab(Graphics g,Tab tab,bool hot,bool selected)
		{
			if(tab.Bounds.Width == 0){
				return;
			}

			//--- Fill selected tab backround
			if(selected){				
				g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Control)),tab.Bounds.Left-1,4,tab.Bounds.Width,this.Height);
			}

			//--- Draw caption
			StringFormat sFormat  = new StringFormat();
			sFormat.Alignment     = System.Drawing.StringAlignment.Center;
			sFormat.LineAlignment = System.Drawing.StringAlignment.Center;
			if(hot){
				if(tab.Enabled){
					g.DrawString(tab.Caption,this.Font,new SolidBrush(Color.Blue),tab.TextRect,sFormat);
				}
				else{
					g.DrawString(tab.Caption,this.Font,new SolidBrush(Color.Red),tab.TextRect,sFormat);
				}
			}
			else{
				if(selected){ 
					g.DrawString(tab.Caption,this.Font,new SolidBrush(Color.Black),tab.TextRect,sFormat);					
				}
				else{
					g.DrawString(tab.Caption,this.Font,new SolidBrush(Color.Gray),tab.TextRect,sFormat);					
				}
			}
			//------------------------------------------------------------------------------------------//

			//---- Draw right side tab border
			if(selected){
				g.DrawLine(new Pen(Color.Black),tab.Bounds.Right-1,4,tab.Bounds.Right-1,this.Height-3);							
			}
			else{
				g.DrawLine(new Pen(Color.Gray),tab.Bounds.Right-1,4,tab.Bounds.Right-1,this.Height-6);
			}

			//---- Draw item image 			
			if(m_ImgList != null && tab.ImageIndex > -1 && tab.ImageIndex < m_ImgList.Images.Count){
				if(tab.Enabled){
					g.DrawImage(m_ImgList.Images[tab.ImageIndex],tab.ImageRect);
				}
				else{
					ControlPaint.DrawImageDisabled(g,m_ImgList.Images[tab.ImageIndex],tab.ImageRect.X,tab.ImageRect.Y,Color.Transparent);
				}				
			}
		}

		#endregion

		#region function DrawButtons

		private void DrawButtons(Graphics g)
		{
			g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),this.BtnsRect.Left,this.BtnsRect.Top-1,this.BtnsRect.Width,this.BtnsRect.Height+1);
			
			Point mPos = this.PointToClient(Control.MousePosition);
			
			//------------- Draw hot -----------------------------------------------------------------------------------//
			if(this.FirstBtnRect.Contains(mPos)){
				g.DrawLine(new Pen(Color.White),this.FirstBtnRect.Left-1,1,this.FirstBtnRect.Left-1,this.Height-4);
				g.DrawLine(new Pen(Color.White),this.FirstBtnRect.Left-1,1,this.FirstBtnRect.Right-1,1);
				g.DrawLine(new Pen(Color.DarkGray),this.FirstBtnRect.Right-1,1,this.FirstBtnRect.Right-1,this.Height-4);
				g.DrawLine(new Pen(Color.DarkGray),this.FirstBtnRect.Left,this.Height-3,this.FirstBtnRect.Right-1,this.Height-3);
			}
		
			if(this.PrevBtnRect.Contains(mPos)){
				g.DrawLine(new Pen(Color.White),this.PrevBtnRect.Left-1,1,this.PrevBtnRect.Left-1,this.Height-4);
				g.DrawLine(new Pen(Color.White),this.PrevBtnRect.Left-1,1,this.PrevBtnRect.Right-1,1);
				g.DrawLine(new Pen(Color.DarkGray),this.PrevBtnRect.Right-1,1,this.PrevBtnRect.Right-1,this.Height-4);
				g.DrawLine(new Pen(Color.DarkGray),this.PrevBtnRect.Left,this.Height-3,this.PrevBtnRect.Right-1,this.Height-3);
			}

			if(this.NextBtnRect.Contains(mPos)){
				g.DrawLine(new Pen(Color.White),this.NextBtnRect.Left-1,1,this.NextBtnRect.Left-1,this.Height-4);
				g.DrawLine(new Pen(Color.White),this.NextBtnRect.Left-1,1,this.NextBtnRect.Right-1,1);
				g.DrawLine(new Pen(Color.DarkGray),this.NextBtnRect.Right-1,1,this.NextBtnRect.Right-1,this.Height-4);
				g.DrawLine(new Pen(Color.DarkGray),this.NextBtnRect.Left,this.Height-3,this.NextBtnRect.Right-1,this.Height-3);
			}

			if(this.LastBtnRect.Contains(mPos)){
				g.DrawLine(new Pen(Color.White),this.LastBtnRect.Left-1,1,this.LastBtnRect.Left-1,this.Height-4);
				g.DrawLine(new Pen(Color.White),this.LastBtnRect.Left-1,1,this.LastBtnRect.Right-1,1);
				g.DrawLine(new Pen(Color.DarkGray),this.LastBtnRect.Right-1,1,this.LastBtnRect.Right-1,this.Height-4);
				g.DrawLine(new Pen(Color.DarkGray),this.LastBtnRect.Left,this.Height-3,this.LastBtnRect.Right-1,this.Height-3);
			}
			//------------------------------------------------------------------------------------------------------------//
			
			if(IsFirstBtnEnabled){ 
				Painter.DrawIcon(g,m_iFirst,this.FirstBtnRect,false,false);
			}
			else{
				Painter.DrawIcon(g,m_iFirst,this.FirstBtnRect,true,false);
			}

			if(IsPrevBtnEnabled){
				Painter.DrawIcon(g,m_iPrev ,this.PrevBtnRect,false,false);
			}
			else{
				Painter.DrawIcon(g,m_iPrev ,this.PrevBtnRect,true,false);
			}

			if(IsNextBtnEnabled){
				Painter.DrawIcon(g,m_iNext ,this.NextBtnRect,false,false);
			}
			else{
				Painter.DrawIcon(g,m_iNext ,this.NextBtnRect,true,false);
			}

			if(IsLastBtnEnabled){
				Painter.DrawIcon(g,m_iLast ,this.LastBtnRect,false,false);
			}
			else{
				Painter.DrawIcon(g,m_iLast ,this.LastBtnRect,true,false);
			}

			//---- Draw white line at the right edge
			g.DrawLine(new Pen(Color.White),this.Right-1,0,this.Right-1,this.Height);
		}

		#endregion


		#region function CalcDrawInfo

		internal void CalcDrawInfo()
		{			
			using(Graphics g = this.CreateGraphics()){

				int tRight = 5;
                
				//--- loop through all tabs ------//
				for(int i=0;i<m_pTabs.Count;i++){
					Tab tab = this.Tabs[i];

					if(i >= m_FVisibleTabIndex){
						int width = CalcTabWidth(g,tab) + 10;
						tab.Bounds = new Rectangle(tRight,1,width,this.Height-1);
						tRight += width;
					}
					else{
						tab.Bounds = new Rectangle(-1,-1,0,0);
					}
				}
			}
		}

		#endregion

		#region function CalcTabWidth

		private int CalcTabWidth(Graphics g,Tab tab)
		{
			int size = (int)g.MeasureString(tab.Caption,this.Font,this.ClientSize.Width).Width;
			if(tab.ImageIndex > -1){
				size += m_ImgList.ImageSize.Width + 4;
			}
			return size;
		}

		#endregion


		#region function OnMouseUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);

			Point pt = new Point(e.X,e.Y);

			//--- Check if some scroll button clicked
			if(this.BtnsRect.Contains(pt)){
				if(this.IsFirstBtnEnabled && this.FirstBtnRect.Contains(pt)){
					m_FVisibleTabIndex = 0;
					this.Invalidate();
				}

				if(this.IsPrevBtnEnabled && this.PrevBtnRect.Contains(pt)){
					m_FVisibleTabIndex--;
					this.Invalidate();
				}

				if(this.IsNextBtnEnabled && this.NextBtnRect.Contains(pt)){
					m_FVisibleTabIndex++;
					this.Invalidate();
				}

				if(this.IsLastBtnEnabled && this.LastBtnRect.Contains(pt)){
					Tab lastTab = this.Tabs[this.Tabs.Count-1];
					while(m_FVisibleTabIndex < this.Tabs.Count-1){
						m_FVisibleTabIndex++;
						CalcDrawInfo();

						if(lastTab.Bounds.Right < this.BtnsRect.Left){
							CalcDrawInfo();
							break;
						}
					}
					this.Invalidate();
				}

				return;
			}

			//--- Check if some tab hitted
			foreach(Tab tab in m_pTabs){
				if(tab.Bounds.Contains(pt) && tab.Enabled && !tab.Equals(this.SelectedTab)){
					this.SelectedTab = tab;
					MakeTabVisible(tab);
					break;
				}
			}
		}

		#endregion

		#region function OnMouseMove

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);

			object activeItem = null;
			
			// Buttons
			if(this.BtnsRect.Contains(this.PointToClient(Control.MousePosition))){
				activeItem = null; // Buttons must always invalidated
			}
			else{
				// Tab
				Tab t = GetMouseTab();
				if(t != null){
					activeItem = t;
				}
				// TabControl
				else{
					activeItem = "tabcontrol";
				}
			}

			if(activeItem == null || !activeItem.Equals(m_ActiveObj)){
				m_ActiveObj = activeItem;
				this.Invalidate();
			}
		}

		#endregion

		#region function OnMouseLeave

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			m_ActiveObj = null;
			this.Invalidate();
		}

		#endregion


		#region function GetMouseTab

		/// <summary>
		/// Gets tab which is mouse cursor.
		/// </summary>
		private Tab GetMouseTab()
		{
			//--- loop through all tabs ------//
			for(int i=0;i<m_pTabs.Count;i++){
				Tab tab = this.Tabs[i];

				if(tab.Bounds.Contains(this.PointToClient(Control.MousePosition))){
					return tab;
				}
			}

			return null;
		}

		#endregion

		#region function MakeTabVisible

		/// <summary>
		/// Makses specified tab visible (scrolls to visible area).
		/// </summary>
		/// <param name="tab"></param>
		public void MakeTabVisible(Tab tab)
		{
			if(tab.Bounds.Left < 0 || tab.Bounds.Right > this.BtnsRect.Left){
				while(m_FVisibleTabIndex < this.Tabs.Count-1){
					m_FVisibleTabIndex++;
					CalcDrawInfo();

					if(tab.Bounds.Right < this.BtnsRect.Left){
						CalcDrawInfo();
						break;
					}
				}
				this.Invalidate();
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Tabs collection.
		/// </summary>
		public Tabs Tabs
		{
			get{ return m_pTabs; }
		}

		/// <summary>
		/// Gets or sets selected tab.
		/// </summary>
		public Tab SelectedTab
		{
			get{ return m_pSelectedTab;}

			set{ 
				OnSelectedTabChanged(value,m_pSelectedTab);
				m_pSelectedTab = value;
				this.Invalidate();				
			}
		}

		/// <summary>
		/// Gets or sets imagelist.
		/// </summary>
		public ImageList ImageList
		{
			get{ return m_ImgList; }

			set{ m_ImgList = value; }
		}

		#region Internal Properties

		internal Rectangle BtnsRect
		{
			get{ return new Rectangle(this.Width-68,2,67,this.Height-4); }
		}

		internal Rectangle FirstBtnRect
		{
			get{ return new Rectangle(this.Width-67,5,16,16); }
		}

		internal Rectangle PrevBtnRect
		{
			get{ return new Rectangle(this.Width-51,5,16,16); }
		}

		internal Rectangle NextBtnRect
		{
			get{ return new Rectangle(this.Width-34,5,16,16); }
		}

		internal Rectangle LastBtnRect
		{
			get{ return new Rectangle(this.Width-17,5,16,16); }
		}

		internal bool IsFirstBtnEnabled
		{
			get{
				if(m_pTabs.Count > 0){
					Tab t = this.Tabs[0];
					if(t.Bounds.Left < 1){
						return true;
					}
				}
				
				return false; 
			}
		}

		internal bool IsPrevBtnEnabled
		{
			get{ 
				if(m_pTabs.Count > 0){
					Tab t = this.Tabs[0];
					if(t.Bounds.Left > 5 || m_FVisibleTabIndex > 0){ // 5 is left for first tab
						return true;
					}
				}

				return false; 
			}
		}

		internal bool IsNextBtnEnabled
		{
			get{
				if(m_pTabs.Count > 0){
					Tab t = this.Tabs[this.Tabs.Count-1];
					if(t.Bounds.Right > this.BtnsRect.Left){
						return true;
					}
				}

				return false;
			}
		}

		internal bool IsLastBtnEnabled
		{
			get{
				if(m_pTabs.Count > 0){
					Tab t = this.Tabs[this.Tabs.Count-1];
					if(t.Bounds.Right > this.BtnsRect.Left){
						return true;
					}
				}

				return false;
			}
		}

		#endregion

		#endregion

		#region Events Implementation

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnSelectedTabChanged(Tab newTab,Tab oldTab)
		{
			if(this.TabChanged != null){
				this.TabChanged(this,new TabChanged_EventArgs(newTab,oldTab));
			}
		}

		#endregion

	}
}
