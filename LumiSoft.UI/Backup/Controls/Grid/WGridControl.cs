using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid
{
	#region Event handlers declarations

	/// <summary>
	/// 
	/// </summary>
	public delegate void DataRowViewHandler(DataRowView drV);

	/// <summary>
	/// 
	/// </summary>
	public delegate bool CancelableDataRowViewHandler(DataRowView drV);

	#endregion

	/// <summary>
	/// Grid control.
	/// </summary>
	public class WGridControl : UserControl
	{
		#region Events declarations

		/// <summary>
		/// Invoked when row double clicked.
		/// </summary>
		public event DataRowViewHandler RowDoubleClicked = null;

		/// <summary>
		/// Invoked when active row changed.
		/// </summary>
		public event DataRowViewHandler ActiveRowChanged = null;

		/// <summary>
		/// Invoked when grid needs to add new row.
		/// </summary>
		public event CancelableDataRowViewHandler AddNewRow = null;

		/// <summary>
		/// Invoked when grid needs to delete row.
		/// </summary>
		public event CancelableDataRowViewHandler DeleteRow = null;

		/// <summary>
		/// Invoked when grid needs to update row.
		/// </summary>
		public event CancelableDataRowViewHandler UpdateRow = null;

		/// <summary>
		/// Invoked when grid needs to store cell value.
		/// </summary>
		public event EventHandler ValidateCellValue = null;

		/// <summary>
		/// Invoked when Main Grid View minimize/maximize button pressed.
		/// </summary>
		public event EventHandler MaxMinButtonPressed = null;

		#endregion

		private   WText                m_pWText                  = null;
		private   WGridTableView       m_pMainView               = null; 
	    internal  WGridTableView       m_pFocusedView            = null;
		private   WGridTableView       m_pMouseView              = null;
		private   WGridTableView       m_pDragView               = null;
        private   WGridTableView       m_pMaximizedView          = null;
	    internal  List<WGridTableView> m_pActiveViews            = null;
		private   ArrayList            m_pMaximizeHiddenControls = null;
		private   Bitmap               m_pPaintBuffer            = null;
		private   Graphics             m_pPaintGraphics          = null;
        private   bool                 m_IsRePaintQueued         = false;
        private   Timer                m_pDelayedPainterTimer    = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WGridControl()
		{
			m_pFocusedView = new WGridTableView(this);		
			m_pMainView = m_pFocusedView;
		
			m_pActiveViews = new List<WGridTableView>();
			m_pActiveViews.Add(m_pFocusedView);
	
			m_pMaximizeHiddenControls = new ArrayList();

			SetStyle(ControlStyles.Selectable | ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw,true);
            
            m_pDelayedPainterTimer = new Timer();
            m_pDelayedPainterTimer.Interval = 40;
            m_pDelayedPainterTimer.Tick += new EventHandler(m_pDelayedPainterTimer_Tick);
        }
                
        #region method Dispose

        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();

			if(m_pPaintGraphics != null){
				m_pPaintGraphics.Dispose();
				m_pPaintGraphics = null;
			}
			if(m_pPaintBuffer != null){
				m_pPaintBuffer.Dispose();
				m_pPaintBuffer = null;
			}

			if(m_pWText != null){
                m_pWText.LanguageChanged -= new EventHandler(m_pWText_LanguageChanged);
				m_pWText = null;		
			}
        }

        #endregion


        #region method m_pDelayedPainterTimer_Tick

        private void m_pDelayedPainterTimer_Tick(object sender,EventArgs e)
        {
            m_pDelayedPainterTimer.Enabled = false;

            try{
                Repaint(false);
            }
            catch{
            }

            m_IsRePaintQueued = false;
        }

        #endregion

        #region method m_pWText_LanguageChanged

        private void m_pWText_LanguageChanged(object sender, EventArgs e)
        {
            Repaint(false);
        }

        #endregion


        #region override method OnSizeChanged

        /// <summary>
        /// This method is called by base class when this control size has changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            
            // Control size chaged so offscreen paint buffer is with invalid size, 
            // just dispose it, paint method will re create it when needed.
            if(m_pPaintGraphics != null){
                m_pPaintGraphics.Dispose();
                m_pPaintGraphics = null;
            }
            if(m_pPaintBuffer != null){
                m_pPaintBuffer.Dispose();
                m_pPaintBuffer = null;
            }

            if(m_pMainView != null){
				m_pMainView.setBounds(new Rectangle(0,0,this.Bounds.Width,this.Bounds.Height));
			}
        }

        #endregion

        #region override method OnPaint

        /// <summary>
		/// Default paint override.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// All painting is done to offscreen image, we just need to paint
			// offscreen image to Graphics.
            
            if(m_pPaintBuffer == null){
                Repaint(false);
            }
	
			// Draw offscreen image
			e.Graphics.DrawImage(m_pPaintBuffer,0,0);
        }

        #endregion

        #region method Repaint

        /// <summary>
        /// Repaints grid.
        /// </summary>
        /// <param name="delay">Specifies if repaint is delayed.</param>
        internal void Repaint(bool delay)
        {           
            if(delay){
                // We have queued reapint already.
                if(m_IsRePaintQueued){
                    return;
                }
                m_IsRePaintQueued = true;
                m_pDelayedPainterTimer.Enabled = true;
            }
            else{
                // Stop delayed painting, if any queued.
                m_pDelayedPainterTimer.Enabled = false;

                // All painting is done to offscreen image, we just need to paint
			    // offscreen image to Graphics.
                                
                Graphics g2 = GetPaintGraphics();
                								
			    // Fill default grid backround
			    g2.Clear(Color.FromArgb(212,208,200));
    			
                if(m_pMaximizedView != null){
                    m_pMaximizedView.Paint(g2);
                }
			    else if(m_pMainView != null){
				    m_pMainView.Paint(g2);
			    }

			    // Force to redraw
                Invalidate();
            }
        }

        #endregion

        #region method GetPaintGraphics

        /// <summary>
		/// Gets Graphics object what must be used for painting.
		/// </summary>
		/// <returns></returns>
		internal protected Graphics GetPaintGraphics()
		{
            if(m_pPaintBuffer == null){
                m_pPaintBuffer = new Bitmap(this.Bounds.Width,this.Bounds.Height);
            }
            
			if(m_pPaintGraphics == null){
				m_pPaintGraphics = Graphics.FromImage(m_pPaintBuffer);
			}

			return m_pPaintGraphics;
        }

        #endregion


        //--- Forward key events to focused view, if they aren't handled by user ----//

        #region override method OnKeyPress

        protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			if(m_pFocusedView != null){
				m_pFocusedView.Process_keyTyped(e);
			}
        }

        #endregion

        #region override method OnKeyDown

        protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);

			if(m_pFocusedView != null){
				m_pFocusedView.Process_keyPressed(e);
			}
        }

        #endregion

        #region override method OnKeyUp

        protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if(m_pFocusedView != null){
				m_pFocusedView.Process_keyReleased(e);
			}
        }

        #endregion

        #region method override ProcessDialogKey

        protected override bool ProcessDialogKey(Keys keyData)
		{
			if(keyData == Keys.Enter){
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Escape){
				OnKeyUp(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Up){
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Down){
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Left){
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Right){
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}

			return base.ProcessDialogKey(keyData);
        }

        #endregion

        //----------------------------------------------------------------------------//


        //--- Forward mouse events to view under mouse cursor ------------------------//

        #region override method OnMouseDown

        protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			WGridTableView view = this.GetMouseInTableView(new Point(e.X,e.Y));
			if(view != null){
				view.Process_mousePressed(e);
			}
        }

        #endregion

        #region override method OnMouseUp

        protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if(m_pDragView != null){
				m_pDragView.Process_mouseReleased(e);
				m_pDragView = null;
			}
			else{
				WGridTableView view = this.GetMouseInTableView(new Point(e.X,e.Y));
				if(view != null){
					view.Process_mouseReleased(e);
				}
			}
        }

        #endregion

        #region override method OnClick

        protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if(m_pDragView == null){
				int x = this.PointToClient(Control.MousePosition).X;
				int y = this.PointToClient(Control.MousePosition).Y;

				WGridTableView view = this.GetMouseInTableView(new Point(x,y));
				if(view != null){
					view.Process_mouseClicked(new MouseEventArgs(MouseButtons.Left,1,x,y,0));
				}
			}
        }

        #endregion

        #region override method OnDoubleClick

        protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);

			int x = this.PointToClient(Control.MousePosition).X;
			int y = this.PointToClient(Control.MousePosition).Y;

			WGridTableView view = this.GetMouseInTableView(new Point(x,y));
			if(view != null){
				view.Process_mouseClicked(new MouseEventArgs(MouseButtons.Left,2,x,y,0));
			}
        }

        #endregion

        #region override method OnMouseEnter

        protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
        }

        #endregion

        #region override method OnMouseLeave

        protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

		//	for(int i=0;i<m_pActiveViews.Count;i++){
		//		WGridTableView view = (WGridTableView)m_pActiveViews[i];
		//		view.Process_mouseExited(e);
		//	}
        }

        #endregion

        #region override method OnMouseWheel

        protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			// Mouse wheel events must be forwarded to keyboard focused view !!!
			if(m_pFocusedView != null){
				m_pFocusedView.Process_mouseWheelMoved(e);
			}
        }

        #endregion

        #region override method OnMouseMove

        protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if(m_pDragView == null && e.Button == MouseButtons.Left){
				m_pDragView = this.GetMouseInTableView(new Point(e.X,e.Y));
			}
			if(m_pDragView != null){
				m_pDragView.Process_mouseDragged(e);
			}
			else if(e.Button == MouseButtons.None){
				WGridTableView view = this.GetMouseInTableView(new Point(e.X,e.Y));
				if(view != null){
					view.Process_mouseMoved(e);
				}
			}
        }

        #endregion


     /*	protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);*/
//Console.WriteLine("drag");
		/*	if(m_pDragView == null){
				m_pDragView = this.GetMouseInTableView(new Point(drgevent.X,drgevent.Y));
			}
			else{
				m_pDragView.Process_mouseDragged(drgevent);
			}*/
	//	}
        //--------------------------------------------------------------------------------//


        #region method GetMouseInTableView

        /// <summary>
		/// Gets view what is at specified point.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		private WGridTableView GetMouseInTableView(Point pt)
		{
			// Focused view is in edit mode, skip all mouse events
		//	if(m_pFocusedView != null && m_pFocusedView.ActiveEditor != null){
		//		return null;
		//	}

			for(int i=m_pActiveViews.Count - 1;i>-1;i--){
				WGridTableView view = (WGridTableView)m_pActiveViews[i];
				
				if(view.Bounds.Contains(pt)){
					if(m_pMouseView != view){
						// Mouse in view changed, send Exit note.
						if(m_pMouseView != null){
							m_pMouseView.Process_mouseExited(null);
						}
						m_pMouseView = view;
					}				
					return view;
				}
			}
			
			return m_pFocusedView;
        }

        #endregion

        #region method OnViewMaximized

        /// <summary>
		/// Is called when specified view maximized.
		/// </summary>
		/// <param name="view"></param>
		internal protected void OnViewMaximized(WGridTableView view)
		{
			// Hide all grid components which owner isn't maximized view
			ArrayList viewC = view.Components;
			foreach(Control comp in this.Controls){
				if(comp.Visible){
					if(!viewC.Contains(comp)){
						comp.Visible = false;
						m_pMaximizeHiddenControls.Add(comp);
					}
				}
			}

            m_pMaximizedView = view;
        }

        #endregion

        #region method OnViewMinimized

        /// <summary>
		/// Is called when specified view minimized.
		/// </summary>
		/// <param name="view"></param>
		internal protected void OnViewMinimized(WGridTableView view)
		{
			// Show all grid components which was hidden for maximized view
			for(int i=0;i<m_pMaximizeHiddenControls.Count;i++){
				Control c = (Control)m_pMaximizeHiddenControls[i];
				c.Visible = true;
			}

            m_pMaximizedView = null;
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets grid main view.
		/// </summary>
		public WGridTableView MainView
		{
			get{ return m_pMainView; }
		}

		/// <summary>
		/// Gets keyboard focused view.
		/// </summary>
		public WGridTableView FocusedView
		{
			get{ return m_pFocusedView; }
		}

		/// <summary>
		/// Gets active views (Main view + all open child views).
		/// </summary>
		public WGridTableView[] ActiveViews
		{
			get{ return m_pActiveViews.ToArray(); }
		}
        
        /// <summary>
        /// Gets or sets grid texts provider.
        /// </summary>
		public WText WText
		{
			get{ return m_pWText; }

            set{ 
                m_pWText = value;

                if(m_pWText != null){
                    m_pWText.LanguageChanged += new EventHandler(m_pWText_LanguageChanged);
                }
            }
		}
                		
		#endregion

		#region Events Implementation

		/// <summary>
		/// Raises row double clicked event.
		/// </summary>
		/// <param name="dr"></param>
		internal protected void RaiseOnRowDoubleClicked(DataRowView dr)
		{
			if(this.RowDoubleClicked != null){
				this.RowDoubleClicked(dr);
			}
		}

		/// <summary>
		/// Raises active row changed event.
		/// </summary>
		/// <param name="dr"></param>
		internal protected void RaiseOnActiveRowChanged(DataRowView dr)
		{
			if(this.ActiveRowChanged != null){
				this.ActiveRowChanged(dr);
			}
		}

		/// <summary>
		/// Raises add row event.
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		internal protected bool RaiseAddNewRow(DataRowView dr)
		{
			if(this.AddNewRow != null){
				return this.AddNewRow(dr);
			}

			return true;
		}

		/// <summary>
		/// Raises delete row event.
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		internal protected bool RaiseDeleteRow(DataRowView dr)
		{
			if(this.DeleteRow != null){
				return this.DeleteRow(dr);
			}

			return true;
		}

		/// <summary>
		/// Raises update row event.
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		internal protected bool RaiseUpdateRow(DataRowView dr)
		{
			if(this.UpdateRow != null){
				return this.UpdateRow(dr);
			}

			return true;
		}

		/// <summary>
		/// Raises validates cell value event.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="dr"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		internal protected bool RaiseValidateCellValue(WGridColumn column,DataRowView dr,Object value)
		{
			return true;
		}

		/// <summary>
		/// Raises OnMaxMinButton pressed event.
		/// </summary>
		/// <param name="maximized"></param>
		internal protected void RaiseOnMaxMinButtonPressed(bool maximized)
		{
		//	if(this.RowDoubleClicked != null){
		//		this.RowDoubleClicked(dr);
		//	}
		}

		#endregion

	}
}
