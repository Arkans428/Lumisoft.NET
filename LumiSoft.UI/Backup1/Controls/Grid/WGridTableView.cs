using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using LumiSoft.UI.Controls;
using LumiSoft.UI.Controls.Grid.Editors;

namespace LumiSoft.UI.Controls.Grid
{
	/// <summary>
	/// Provides tabular data view for WGridControl.
	/// </summary>
	public class WGridTableView
	{		
		private WGridControl   m_pGrid                   = null;
		private WGridTableView m_pDetailView             = null;
		private WGridTableView m_pParentView             = null;
		private _RowInfo       m_pParentRow              = null;
		private int            m_FirstVisibleRow         = 0;
		private int            m_LastVisibleRow          = -1;
		private WGridColumn    m_pActiveColumn           = null;
		private _RowInfo       m_pActiveRow              = null;
		private object[]       m_ActiveRowValues         = null;
		private WScrollBar     m_pVtScrollBar            = null;
		private WScrollBar     m_pHzScrollBar            = null;
		private WDragWindow    m_pDragWindow             = null;
		private WGridColumn    m_pDragColumn             = null;
        private string         m_Name                    = "";
		private bool           m_AllowEdit               = false;
		private bool           m_AllowSort               = true;
		private bool           m_AllowAppendNewRow       = true;
		private bool           m_AllowMultiSelect        = false;
		private bool           m_AllowReorderColumns     = true;
		private bool           m_EnterKeyDoubleClicks    = false;
		private bool           m_ShowColumns             = true;
		private bool           m_ShowRowIndicators       = true;
		private bool           m_ShowFooter              = true;
		private bool           m_ShowMaximizeForMainView = false;
		private bool           m_ShowCurrentCell         = true;
		private bool           m_ShowVerticalGridlines   = true;
		private bool           m_ShowHorizontalGridlines = true;
		private WGridColumns   m_pColumns                = null;
		private Rectangle      m_pBounds                 = new Rectangle(-1,-1,0,0);
		private WBaseEditor    m_pActiveEditor           = null;
		private object         m_pTag                    = null;	
		private List<_RowInfo> m_pRowInfos               = null;
		private List<_RowInfo> m_pSelectedRows           = null;		
		private DataView       m_pDataSource             = null;
		private DataRelation   m_pRelation               = null;
		private int            m_ColumnsStartX           = 0;
		private bool           m_ViewMaximized           = false;
        private int            m_SkipDataSourceEvents    = 0;     // REMOVE ME: 27.11.2010 ??
        
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="grid">Owner grid control.</param>
		public WGridTableView(WGridControl grid) : this("",grid)
		{			
        }

        /// <summary>
		/// Default constructor.
		/// </summary>
        /// <param name="name">View name.</param>
		/// <param name="grid">Owner grid control.</param>
		public WGridTableView(string name,WGridControl grid)
		{
            m_Name          = name;
            m_pGrid         = grid;
			m_pColumns      = new WGridColumns(this);
			m_pRowInfos     = new List<_RowInfo>();
			m_pSelectedRows = new List<_RowInfo>();
			m_pBounds       = new Rectangle(-1,-1,0,0);

			m_pVtScrollBar = new WScrollBar();
			m_pVtScrollBar.Visible = false;
			m_pVtScrollBar.PositionChanged += new EventHandler(this.OnViewScrolled);
            m_pVtScrollBar.PageUp += new EventHandler(m_pVtScrollBar_PageUp);
            m_pVtScrollBar.PageDown += new EventHandler(m_pVtScrollBar_PageDown);
			m_pVtScrollBar.Vertical = true;

			m_pHzScrollBar = new WScrollBar();		
			m_pHzScrollBar.Visible = false;
			m_pHzScrollBar.PositionChanged += new EventHandler(this.OnViewScrolled);
            m_pHzScrollBar.PageUp += new EventHandler(m_pHzScrollBar_PageLeft);
            m_pHzScrollBar.PageDown += new EventHandler(m_pHzScrollBar_PageRight);
			m_pHzScrollBar.Vertical = false;

			m_pGrid.Controls.Add(m_pVtScrollBar);
			m_pGrid.Controls.Add(m_pHzScrollBar);

            m_pColumns.CountChanged += new EventHandler(m_pColumns_CountChanged);
        }
                
                
        #region method Dispose

        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{            
			m_pGrid.Controls.Remove(m_pVtScrollBar);
			m_pGrid.Controls.Remove(m_pHzScrollBar);

            // Remove old dataview chaged listener.
            if(m_pDataSource != null){  
                m_pDataSource.ListChanged -= new System.ComponentModel.ListChangedEventHandler(m_pDataSource_ListChanged);
            }

            m_pGrid = null;
            m_pDetailView = null;
            m_pParentView = null;
            m_pParentRow = null;
            m_pDataSource = null;
        }

        #endregion


        #region method m_pColumns_CountChanged

        /// <summary>
        /// Is called when column count has changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pColumns_CountChanged(object sender,EventArgs e)
        {
            Calculate(true);
        }

        #endregion
                
        #region method m_pDataSource_ListChanged

        /// <summary>
        /// This method is called when DataSource(DataView) values has changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pDataSource_ListChanged(object sender,System.ComponentModel.ListChangedEventArgs e)
        {   
            // REMOVE ME: 29.11.2010
            //if(m_SkipDataSourceEvents > 0){
            //    return;
            //}

            if(e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded){
                if(m_pDataSource[e.NewIndex].IsNew){
                    return;
                }

                bool existingRow = false;
                foreach(_RowInfo rowInfo in m_pRowInfos){
                    if(rowInfo.RowSource == m_pDataSource[e.NewIndex]){
                        existingRow = true;

                        break;
                    }
                }
                if(!existingRow){
                    m_pRowInfos.Insert(e.NewIndex,new _RowInfo(this,m_pDataSource[e.NewIndex]));

                    Calculate(true);
                }
            }
            else if(e.ListChangedType == System.ComponentModel.ListChangedType.ItemChanged){
                Repaint();

                // REMOVE ME: 29.11.2010
                // Update active editor value, if there is active editor.
                //if(m_pActiveColumn != null && m_pActiveEditor != null){
                //    m_pActiveEditor.Value = m_pActiveRow.RowSource[m_pActiveColumn.MappingName];
                //}
            }
            else if(e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted){
            }
            else{  
                OnDataViewChanged();
            }
        }

        #endregion

        #region method m_pVtScrollBar_PageUp

        private void m_pVtScrollBar_PageUp(object sender,EventArgs e)
        {
            PageUp();
        }

        #endregion

        #region method m_pVtScrollBar_PageDown

        private void m_pVtScrollBar_PageDown(object sender,EventArgs e)
        {
            PageDown();
        }

        #endregion

        #region method m_pHzScrollBar_PageLeft

        private void m_pHzScrollBar_PageLeft(object sender,EventArgs e)
        {
            OnPageLeft();
        }

        #endregion

        #region method m_pHzScrollBar_PageRight

        private void m_pHzScrollBar_PageRight(object sender,EventArgs e)
        {
            OnPageRight();
        }

        #endregion
                

        #region method Clone

        /// <summary>
		/// Clones grid view.
		/// </summary>
		/// <returns>Returnes cloned grid view.</returns>
		public WGridTableView Clone()
		{
			WGridTableView view = new WGridTableView(m_pGrid);
            view.AllowAppendNewRow = this.AllowAppendNewRow;
            view.AllowEdit = this.AllowEdit;
            view.AllowMultiSelect = this.AllowMultiSelect;
            view.AllowReorderColumns = this.AllowReorderColumns;
            view.AllowSort = this.AllowSort;
            view.EnterKeyDoubleClicks = this.EnterKeyDoubleClicks;
            view.ShowColumns = this.ShowColumns;
            view.ShowCurrentCell = this.ShowCurrentCell;
            view.ShowFooter = this.ShowFooter;
            view.ShowHorizontalGridlines = this.ShowHorizontalGridlines;
            view.ShowMaximizeForMainView = this.ShowMaximizeForMainView;
            view.ShowRowIndicators = this.ShowRowIndicators;
            view.ShowVerticalGridlines = this.ShowVerticalGridlines;
            view.Tag = this.Tag;
            				
			foreach(WGridColumn columnSource in this.Columns){
				WGridColumn columnNew = view.Columns.AddColumn(columnSource.Text);
                columnNew.AllowEdit = columnSource.AllowEdit;
                columnNew.AllowResize = columnSource.AllowResize;
                columnNew.AllowSort = columnSource.AllowSort;                
				columnNew.CellsTextAlign = columnSource.CellsTextAlign;
				columnNew.CellTextFormat = columnSource.CellTextFormat;
                columnNew.Editor = columnSource.Editor;
                columnNew.Font = columnSource.Font;
                columnNew.FooterText = columnSource.FooterText;
                columnNew.FooterTextAlign = columnSource.FooterTextAlign;
                columnNew.FooterType = columnSource.FooterType;
                columnNew.MappingName = columnSource.MappingName;
                columnNew.Tag = columnSource.Tag;
                columnNew.Text = columnSource.Text;
                columnNew.TextAlign = columnSource.TextAlign;
                columnNew.TextID = columnSource.TextID;
                columnNew.Width = columnSource.Width;
                columnNew.Visible = columnSource.Visible;	
			}
			
			if(m_pDataSource != null){
				view.DataSource = new DataView(m_pDataSource.Table);
			}
			
			return view;
        }

        #endregion

        #region method OnViewScrolled

        private void OnViewScrolled(Object sender,System.EventArgs e)
		{
			OnViewScrolled();
        }
               
        private void OnViewScrolled()
		{
			if(IsActiveRowModified()){
				return;
			}
			HideEditor();
	
			m_FirstVisibleRow = m_pVtScrollBar.Position;
            Calculate(false);
            m_pGrid.Repaint(true);
        }

        #endregion

        #region method OnDataViewChanged

        /// <summary>
        /// Is called when DataView sorting,filtering,... changed.
        /// </summary>
	    private void OnDataViewChanged()
	    {
            m_SkipDataSourceEvents++;

            DataRelation relation = m_pRelation;
            this.DataSource = m_pDataSource;
            this.DataRelation = relation;

            m_SkipDataSourceEvents--;
        }

        #endregion

        #region method Paint

        /// <summary>
		/// Paints table view.
		/// </summary>
		/// <param name="g"></param>
		internal void Paint(Graphics g)
		{
			g.SetClip(this.Bounds);
						
			// Fill backround
			g.FillRectangle(new SolidBrush(Color.White),this.Bounds);

			if(ShowColumns){
				PaintHeader(g);
			}
			if(ShowFooter){
				PaintFoother(g);
			}
         							
			// Paint visible rows
			if(m_pRowInfos.Count > 0){
                g.SetClip(RowsRect);

				for(int i=m_FirstVisibleRow;i<=m_LastVisibleRow;i++){			
					PaintRow(g,m_pRowInfos[i]);
				}

                g.SetClip(this.Bounds);
			}			

			// Paint view border
			g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),Bounds.X,Bounds.Y,Bounds.Width - 1,Bounds.Height - 1);
        }

        #endregion

        #region method PaintHeader

        /// <summary>
		/// Paints view header.
		/// </summary>
		/// <param name="g"></param>
		private void PaintHeader(Graphics g)
		{
			//--- Paint unused area of columns  ------------------------------------------------//
			g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),Bounds.Left,Bounds.Top,ColumnsRect.Width,ColumnsRect.Height);
			//--------------------------------------------------------------------------------------//
			
			// If child grid draw maximize/minimize button
			if(m_pParentView != null || m_ShowMaximizeForMainView){
				if(m_ViewMaximized){
					// Draw minimize X
					g.DrawLine(new Pen(Color.Red),Bounds.X + 6,Bounds.Y + 6,Bounds.X + 14,Bounds.Y + 14);
					g.DrawLine(new Pen(Color.Red),Bounds.X + 6,Bounds.Y + 14,Bounds.X + 14,Bounds.Y + 6);
				}
				else{
					// Draw maximize magnify glass
					g.FillEllipse(new SolidBrush(Color.White),Bounds.X + 4,Bounds.Y + 4,9,9);
					g.DrawEllipse(new Pen(Color.Black),Bounds.X + 4,Bounds.Y + 4,9,9);
					g.DrawLine(new Pen(Color.Black),Bounds.X + 12,Bounds.Y + 12,Bounds.X + 12 + 4,Bounds.Y + 12 + 4);
				}
			}
			//--------------------------------------------------------------------------------------//

			String sort = "";
			if(m_pDataSource != null){
				sort = m_pDataSource.Sort.ToLower();
			}
			String[] sortList = sort.Split(',');
					
			g.IntersectClip(this.ColumnsRect);	
			foreach(WGridColumn col in this.Columns.VisibleColumns){				
				// Paint only visible columns
				if(col.Bounds.Right > -1 &&  col.Bounds.Left < RowsRect.Right){
					// Fill backround
					g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),col.Bounds.X,col.Bounds.Y,col.Bounds.Width,col.Bounds.Height);
					
					//--- Paint sorting arrow ----------------------------------------------------//
					for(int c=0;c<sortList.Length;c++){
						if(sortList[c].Split(' ')[0].Equals(col.MappingName.ToLower())){
							if(sortList[c].IndexOf("desc") > -1){
								LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Gray,new Rectangle((int)col.Bounds.Right - 13,col.Bounds.Y + 7,9,7),Direction.Down);
							}
							else{
								LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Gray,new Rectangle((int)col.Bounds.Right - 13,col.Bounds.Y + 7,9,7),Direction.Up);
							}
							break;
						}					
					}
					//---------------------------------------------------------------------------//
					
					// Draw text
					LumiSoft.UI.Controls.Paint.DrawText(g,Color.Black,col.Font,col.Text,col.Bounds,HorizontalAlignment.Center);
		
					// Paint border around column
					PaintRaisedBorder(g,new Rectangle(col.Bounds.Left,col.Bounds.Top,col.Bounds.Width,col.Bounds.Height + 1));
				}
			}		
			g.SetClip(this.Bounds);
        }

        #endregion

        #region method PaintFoother

        /// <summary>
		/// Paints view foother.
		/// </summary>
		/// <param name="g"></param>
		private void PaintFoother(Graphics g)
		{
			// Fill backround
			g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),this.FooterRect);
					
			foreach(WGridColumn col in this.Columns.VisibleColumns){				
				// Paint only visible columns
				if(col.Bounds.Right > -1 && col.Bounds.Left < RowsRect.Right){
					String footerText = "";
					switch(col.FooterType)
					{
						case WGridViewFooterType.Text:
							footerText = col.FooterText;
							break;
						case WGridViewFooterType.Count:
							if(m_pDataSource != null){
								footerText = col.FooterText + m_pDataSource.Count.ToString();
							}
							else{
								footerText = col.FooterText + "0";
							}
							break;
						case WGridViewFooterType.Sum:
							decimal sum = 0;
							if(m_pDataSource != null){
								for(int v=0;v<m_pDataSource.Count;v++){
									DataRowView dr = m_pDataSource[v];
									
									try{
										sum += decimal.Parse(dr[col.MappingName].ToString());
									}
									catch{
									}
								}
							}
							
							// Decimal places formating
							if(!string.IsNullOrEmpty(col.CellTextFormat)){
								try{									
									footerText = sum.ToString(col.CellTextFormat);
								}
								catch{					
								}
							}
							else{			
								footerText = col.FooterText + sum;
							}
							break;
					}

					// Paint column foother only it's available
					if(footerText.Length > 0){
						// Draw text
						LumiSoft.UI.Controls.Paint.DrawText(g,Color.Black,col.Font,footerText,new Rectangle(col.Bounds.Left + 4,FooterRect.Y + 3,col.Bounds.Width - 8,FooterRect.Height - 4),col.FooterTextAlign);
					
						// Draw border
						g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),col.Bounds.X + 2,FooterRect.Y + 2,col.Bounds.Width - 4,FooterRect.Height - 4);
					}
				}
			}
			
			// Draw border around foother area
			g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),FooterRect.X,FooterRect.Y,FooterRect.Width,FooterRect.Height);
        }

        #endregion

        #region method PaintRow

        /// <summary>
		/// Paints specified grid row.
		/// </summary>
		/// <param name="g">Graphichs where to draw row.</param>
		/// <param name="rowInfo">Row which to draw.</param>
		private void PaintRow(Graphics g,_RowInfo rowInfo)
		{
			bool activeRow = m_pActiveRow != null && rowInfo.RowSource.GetHashCode() == m_pActiveRow.RowSource.GetHashCode();
			bool selected  = m_pSelectedRows.Contains(rowInfo);			
			foreach(WGridColumn col in this.Columns.VisibleColumns){
				// Paint only visible cells
				if(col.Bounds.Right > -1 && col.Bounds.Left < RowsRect.Right){
					bool cellSelected = false;
					// Active cell or not selected row
					if(!selected || (activeRow && m_pActiveColumn != null && col.Equals(m_pActiveColumn) && ShowCurrentCell)){
						// Rows with index 1,3,5, ....					
						int rowIndex = m_FirstVisibleRow + m_pRowInfos.IndexOf(rowInfo);
						if((double)rowIndex / 2 != Math.Floor((double)rowIndex / 2)){
							g.FillRectangle(new SolidBrush(Color.FromArgb(245,245,245)),col.Bounds.Left,rowInfo.getY,col.Bounds.Width,rowInfo.RowHeight);							
						}
						// Rows with index 0,2,4, ....
						else{					
							g.FillRectangle(new SolidBrush(Color.White),col.Bounds.Left,rowInfo.getY,col.Bounds.Width,rowInfo.RowHeight);	
						}										
					}
					// Selected row
					else{
						g.FillRectangle(new SolidBrush(Color.FromArgb(64,56,181)),col.Bounds.Left,rowInfo.getY,col.Bounds.Width,rowInfo.RowHeight);
						
						cellSelected = true;
					}
				
					// Ask column editor to paint itself
					col.Editor.PaintDefault(this,g,rowInfo.GetValue(col),new Rectangle(col.Bounds.Left + 2,rowInfo.getY,col.Bounds.Width - 4,rowInfo.RowHeight),cellSelected,col);
					
					//---- Draw cell gridlines
					if(ShowVerticalGridlines){
						// Cell leftside vertical border
						g.DrawLine(new Pen(Color.FromArgb(128,128,128)),col.Bounds.X,rowInfo.getY,col.Bounds.X,rowInfo.getY + rowInfo.RowHeight);
						// Cell rightside vertical border
						g.DrawLine(new Pen(Color.FromArgb(128,128,128)),col.Bounds.X + col.Bounds.Width,rowInfo.getY,col.Bounds.X + col.Bounds.Width,rowInfo.getY + rowInfo.RowHeight);
					}
					if(ShowHorizontalGridlines){
						// Cell top horizontal border
						g.DrawLine(new Pen(Color.FromArgb(128,128,128)),col.Bounds.X,rowInfo.getY,col.Bounds.X + col.Bounds.Width,rowInfo.getY );
						// Cell bottom horizontal border
						g.DrawLine(new Pen(Color.FromArgb(128,128,128)),col.Bounds.X,rowInfo.getY +  + rowInfo.RowHeight,col.Bounds.X + col.Bounds.Width,rowInfo.getY + rowInfo.RowHeight);
					}
					//--------------------------------
				}				
			}

			// Repaint active cell border
			if(ShowCurrentCell && activeRow && m_pActiveColumn != null){
				g.DrawRectangle(new Pen(Color.Black),m_pActiveColumn.Bounds.X + 1,rowInfo.getY + 1,m_pActiveColumn.Bounds.Width - 2,rowInfo.RowHeight - 2);
			}			
			
			//--- Draw row idicator column ----------------------------------------------------------------------//
			if(ShowRowIndicators){
				// Fill backround
				g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),Bounds.X,rowInfo.Bounds.Y,20,rowInfo.Bounds.Height);
				
				// Draw border
				PaintRaisedBorder(g,new Rectangle(Bounds.X,rowInfo.Bounds.Y,20,rowInfo.Bounds.Height));

				// Paint active row arrow
				if(activeRow){
					LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Black,new Rectangle(Bounds.X + 8,rowInfo.Bounds.Y + 5,5,9),Direction.Right);
				}
			}
			//----------------------------------------------------------------------------------------------------//

			//--- Draw master detail column ---------------------------------------------------------//
			if(MasterDetail){
				// Fill backround
				g.FillRectangle(new SolidBrush(Color.White),MasterDetailRect.X,rowInfo.Bounds.Y,20,rowInfo.Bounds.Height);
						
				if(rowInfo.DetailView == null){
					// Paint +
					g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),MasterDetailRect.X + 5,rowInfo.Bounds.Y + 5,10,10);
					g.DrawLine(new Pen(Color.FromArgb(128,128,128)),MasterDetailRect.X + 8,rowInfo.Bounds.Y + 10,MasterDetailRect.X + 12,rowInfo.Bounds.Top + 10);
					g.DrawLine(new Pen(Color.FromArgb(128,128,128)),MasterDetailRect.X + 10,rowInfo.Bounds.Y + 8,MasterDetailRect.X + 10,rowInfo.Bounds.Top + 12);
				}
				else{
					// Paint -
					g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),MasterDetailRect.X + 5,rowInfo.Bounds.Y + 5,10,10);
					g.DrawLine(new Pen(Color.FromArgb(128,128,128)),MasterDetailRect.X + 8,rowInfo.Bounds.Y + 10,MasterDetailRect.X + 12,rowInfo.Bounds.Top + 10);
				}
					
				// Paint border
				PaintRaisedBorder(g,new Rectangle(MasterDetailRect.X,rowInfo.Bounds.Y,20,rowInfo.Bounds.Height));
			}
			//----------------------------------------------------------------------------------------//
					
			// Paint sub grid
			if(rowInfo.DetailView != null){
				RectangleF clip = g.ClipBounds;

				rowInfo.DetailView.Paint(g);

				// Restore original clip
				g.SetClip(clip);
			}
        }

        #endregion

        #region method RepaintRow

        /// <summary>
		/// Repaints specified row.
		/// </summary>
		/// <param name="rowInfo"></param>
		private void RepaintRow(_RowInfo rowInfo)
		{			
			Graphics g = m_pGrid.GetPaintGraphics();
			if(g != null){
				// Allow to paint only visible rows
				if(m_pRowInfos.IndexOf(rowInfo) >= m_FirstVisibleRow && m_pRowInfos.IndexOf(rowInfo) <= m_LastVisibleRow){
					g.SetClip(this.RowsRect);

					PaintRow(g,rowInfo);			
					m_pGrid.Invalidate(new Rectangle(this.Bounds.X,rowInfo.getY,this.Bounds.Width,rowInfo.RowHeight));
				}
			}
        }

        #endregion

        #region method Repaint

        /// <summary>
		/// Repaints table view.
		/// </summary>
		public void Repaint()
		{                        
			Graphics g = m_pGrid.GetPaintGraphics();
			if(g != null){
				Paint(g);
                m_pGrid.Invalidate();
			}
        }

        #endregion

        #region method PaintRaisedBorder

        /// <summary>
		/// Paints border with raised effect.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="rect"></param>
		private void PaintRaisedBorder(Graphics g,Rectangle rect)
		{
			g.DrawRectangle(new Pen(Color.FromArgb(128,128,128)),rect);
			g.DrawLine(new Pen(Color.White),rect.X + 1,rect.Y + 1,rect.X + rect.Width - 2,rect.Y + 1);
			g.DrawLine(new Pen(Color.White),rect.X + 1,rect.Y + 1,rect.X + 1,rect.Y + rect.Height - 2);
        }

        #endregion



        //--- Process key events --------------------------------------------------//

        #region method Process_keyTyped

        internal void Process_keyTyped(KeyPressEventArgs e)
		{
        }

        #endregion

        #region method Process_keyPressed

        internal void Process_keyPressed(KeyEventArgs e)
		{
			//--- Process keyboard events ------------------------------------------------------//
			if(e.KeyCode == Keys.Enter){			
				if(m_EnterKeyDoubleClicks){
					if(m_pActiveRow != null){
						m_pGrid.RaiseOnRowDoubleClicked(m_pActiveRow.RowSource);
					}
				}
				else{
					MoveNext();
				}
			}
			else if(e.KeyCode == Keys.Down){
				// If row contains sub grid, move active row to sub grid first visible row
				if(m_pActiveRow != null && m_pActiveRow.DetailView != null){
					m_pActiveRow.DetailView.SelectFirstVisibleRow();
				}
				else{
					// If sub grid and active row is last row, move active row to parent gird next row.
					if(m_pActiveRow != null && m_pParentView != null && m_pRowInfos.IndexOf(m_pActiveRow) == m_pRowInfos.Count - 1){
						// If view maximized, don't allow move to parent grid view 
						if(!m_ViewMaximized){
							m_pParentView.SelectNextRow(m_pParentRow);
						}					
					}
					else{
						SelectNextRow();
					}
				}
			}
			else if(e.KeyCode == Keys.Up){
				// If sub grid and active row is first row, move active row to parent grid parent row.
				if(m_pRowInfos.IndexOf(m_pActiveRow) == 0 && m_pParentView != null && m_pParentRow != null){
					// If view maximized, don't allow move to parent grid view 
					if(!m_ViewMaximized){
						m_pParentView.SelectRow(m_pParentRow,null,false);
					}				
				}
				else{
					if(m_pRowInfos.IndexOf(m_pActiveRow) > 0){
						_RowInfo row = m_pRowInfos[m_pRowInfos.IndexOf(m_pActiveRow) - 1];
						// If row contains sub grid, move active row to sub grid last visible row
						if(row.DetailView != null){
							if((m_pRowInfos.IndexOf(m_pActiveRow) - 1 < m_FirstVisibleRow)){
								SelectPreviousRow();
							}
							row.DetailView.SelectLastVisibleRow();		
						}
						else{
							SelectPreviousRow();
						}
					}
				}
			}
			else if(e.KeyCode == Keys.Right){
				if(m_pColumns.IndexOf(m_pActiveColumn) < m_pColumns.Count - 1){
					SelectRow(m_pActiveRow,m_pColumns[m_pColumns.IndexOf(m_pActiveColumn) + 1],false);
				}
			}
			else if(e.KeyCode == Keys.Left){
				if(m_pColumns.IndexOf(m_pActiveColumn) > 0){
					SelectRow(m_pActiveRow,m_pColumns[m_pColumns.IndexOf(m_pActiveColumn) - 1],false);
				}
			}
			else if(e.KeyCode == Keys.PageUp){
				PageUp();
			}
			else if(e.KeyCode == Keys.PageDown){
				PageDown();
			}
			else if(e.Control && e.KeyCode == Keys.Delete){
                if(this.AllowEdit){
				    DeleteActiveRow();
                }
			}
            else if(e.Control && e.KeyCode == Keys.A){
                if(m_pRowInfos.Count > 0){
                    SelectRows(m_pRowInfos[0],m_pRowInfos[m_pRowInfos.Count - 1],false);
                    this.Repaint();
                }
            }            
            else if(e.Control && e.Shift && e.KeyCode == Keys.C){
                System.Text.StringBuilder retVal = new System.Text.StringBuilder();
                foreach(_RowInfo rowInfo in m_pSelectedRows){
                    for(int i=0;i<this.Columns.Count;i++){
                        if(i > 0){
                            retVal.Append("\t");
                        }
                        retVal.Append("\"" + rowInfo.RowSource[this.Columns[i].MappingName].ToString() + "\"");
                    }
                    retVal.Append("\r\n");
                }
                Clipboard.SetText(retVal.ToString());
            }
            else if(e.Control && e.KeyCode == Keys.C){
                if(m_pActiveRow != null){
                    Clipboard.SetText(m_pActiveRow.RowSource[m_pActiveColumn.MappingName].ToString());
                }
            }
        }


        #endregion

        #region method Process_keyReleased

        internal void Process_keyReleased(KeyEventArgs e)
		{
        }

        #endregion


        #region method Process_mousePressed

        internal void Process_mousePressed(MouseEventArgs e)
		{
			if(!m_pGrid.Focused){
				m_pGrid.Focus();			
			}
			
			_RowInfo hittedRow = GetRowFromPoint(new Point(e.X,e.Y));
			if(hittedRow == null){
				return;
			}
			
			//--- See if master detail expand/collapse clicked ----------------------------------------------------//
			if(MasterDetail && (e.X > MasterDetailRect.X && e.X < m_ColumnsStartX)){
				ToggleRow(hittedRow);
			}
			//------------------------------------------------------------------------------------------------------//
			
			
			WGridColumn hittedCellColumn = null;
			foreach(WGridColumn col in this.Columns.VisibleColumns){
				if(e.X > col.Bounds.X && e.X < col.Bounds.Right){
					hittedCellColumn = col;
					break;
				}
			}
					
			//--- Handle row selection ------------------------//
			if(m_AllowMultiSelect){
				if(Control.ModifierKeys == Keys.Shift){
					if(m_pActiveRow != null){
						SelectRows(m_pActiveRow,hittedRow);
					}
					else{
						SelectRows(hittedRow,hittedRow);
					}
					
					this.Repaint();
					return;
				}
				else if(Control.ModifierKeys == Keys.Control){
					ToggleRowSelection(hittedRow);
					RepaintRow(hittedRow);
					return;
				}
			}
			//-------------------------------------------------//
			
			SelectRow(hittedRow,hittedCellColumn,true);
        }

        #endregion

        #region method Process_mouseReleased

        internal void Process_mouseReleased(MouseEventArgs e)
		{
			//--- There is active drag operation, end it. ---------------------//
			if(m_pDragWindow != null){
				Object[]    tag         = (Object[])m_pDragWindow.Tag;
				WGridColumn col         = (WGridColumn)tag[0];
				bool        resize_drag = (bool)tag[1];
				
				if(resize_drag){
					int x    = m_pGrid.Parent.PointToScreen(m_pGrid.Location).X + e.X;
					int colX = m_pGrid.Parent.PointToScreen(m_pGrid.Location).X + col.Bounds.X;
					int width = x - colX;
					if(width > 0){
						col.Width = width;
						Calculate(true);
						
						UpdateActiveEditor();
					}
				}
				else{
					// Column dragged to right
					if(col.Bounds.Right < e.X){
						// Find first column which contains mouse position
						for(int i=col.Index;i<this.Columns.Count;i++){
							WGridColumn nextColumn = (WGridColumn)this.Columns[i];
							if(nextColumn.Visible && nextColumn.Bounds.Contains(new Point(e.X,e.Y))){
								col.Index = nextColumn.Index;
								break;
							}
						}
					}
					// Column dragged to left
					else if(col.Bounds.X > e.X){
						// Find first column which contains mouse position
						for(int i=col.Index;i>-1;i--){
							WGridColumn nextColumn = (WGridColumn)this.Columns[i];						
							if(nextColumn.Visible && nextColumn.Bounds.Contains(new Point(e.X,e.Y))){
								col.Index = nextColumn.Index;
								break;
							}
						}
					}
				}
				
				m_pDragWindow.Dispose();
				m_pDragWindow = null;
				m_pDragColumn = null;
			//	m_pGrid.m_pDragView = null;
				
				this.Repaint();
				
				return;
			}
			else{
				// See if maximaze/minimaze child grid clicked
				if(e.X < m_ColumnsStartX && e.Y < this.Bounds.Y + 20){				
					if(m_pParentView != null){
						if(m_ViewMaximized){
							m_ViewMaximized = false;
							m_pGrid.OnViewMinimized(this);
							m_pParentView.Calculate(true);
						}
						else{							
							SelectRow((_RowInfo)m_pRowInfos[0],m_pColumns[0],true);
							m_ViewMaximized = true;
							m_pGrid.OnViewMaximized(this);						
							this.setBounds(m_pParentView.Bounds);
							Calculate(true);
						}
					}
					else if(m_ShowMaximizeForMainView){
						m_ViewMaximized = !m_ViewMaximized;
						
						m_pGrid.RaiseOnMaxMinButtonPressed(m_ViewMaximized);
						Calculate(true);
					}
				}
			}
			//-----------------------------------------------------------------//
        }

        #endregion

        #region method Process_mouseClicked

        internal void Process_mouseClicked(MouseEventArgs e) 
		{
			if(e.Clicks == 2){
				_RowInfo hittedRow = GetRowFromPoint(new Point(e.X,e.Y));
				if(hittedRow != null){
					m_pGrid.RaiseOnRowDoubleClicked(hittedRow.RowSource);
				}
			}
			// See if column sorting
			else if(e.Button == MouseButtons.Left && AllowSort){
                // Don't sort if no data.
                if(m_pDataSource == null || m_pDataSource.Count == 0){
                    return;
                }
				// Don't allow sorting if active row modified
				if(m_pGrid.FocusedView.IsActiveRowModified()){
					return;
				}
				m_pGrid.FocusedView.HideEditor();

				WGridColumn mouseCol = GetColumnFromPoint(new Point(e.X,e.Y));
				if(mouseCol != null && m_pDataSource != null && mouseCol.AllowSort){
                    string sort = m_pDataSource.Sort;	

                    // Multi-column sorting.
                    if(Control.ModifierKeys == Keys.Control){
                        // Clicked coulmn is in sorting expression, reverse sorting.
                        if(sort.IndexOf(mouseCol.MappingName) > -1){
                            if(sort.IndexOf(mouseCol.MappingName + " ASC") > -1){
                                sort = sort.Replace(mouseCol.MappingName + " ASC",mouseCol.MappingName + " DESC");
                            }
                            else{
                                sort = sort.Replace(mouseCol.MappingName + " DESC",mouseCol.MappingName + " ASC");
                            }
                        }
                        // Add new column to sorting expression.
                        else{
                            if(sort.Length > 0){
                                sort += ",";
                            }
                            sort += mouseCol.MappingName + " ASC";
                        }
                    }
                    // Single column sorting.
                    else{
                        if(sort.StartsWith(mouseCol.MappingName + " ASC")){
						    sort = mouseCol.MappingName + " DESC";
					    }
					    else{
						    sort = mouseCol.MappingName + " ASC";
    					}
                    }

                    m_pDataSource.Sort = sort;
				}
			}
        }

        #endregion

        #region method Process_mouseEntered

        internal void Process_mouseEntered(MouseEventArgs e)
		{
        }

        #endregion

        #region method Process_mouseExited

        internal void Process_mouseExited(MouseEventArgs e)
		{
        }

        #endregion

        #region method Process_mouseWheelMoved

        internal void Process_mouseWheelMoved(MouseEventArgs e) 
		{
			if(IsActiveRowModified()){
				return;
			}
			
			if(Control.ModifierKeys == Keys.Shift){
				if(e.Delta > 0){
					if(m_pHzScrollBar.Position > m_pHzScrollBar.Minimum){
						m_pHzScrollBar.Position = m_pHzScrollBar.Position - 5;
						OnViewScrolled(); 
					}
				}
				else{
					if(m_pHzScrollBar.Position < m_pHzScrollBar.Maximum){
						m_pHzScrollBar.Position = m_pHzScrollBar.Position + 5;
						OnViewScrolled();
					}
				}
			}
			else{
				if(e.Delta > 0){
					if(m_pVtScrollBar.Position > m_pVtScrollBar.Minimum){
						m_pVtScrollBar.Position = m_pVtScrollBar.Position -1;
						m_FirstVisibleRow = m_pVtScrollBar.Position;
						OnViewScrolled();
					}
				}
				else{
					if(m_pVtScrollBar.Position < m_pVtScrollBar.Maximum){
						m_pVtScrollBar.Position = m_pVtScrollBar.Position + 1;
						m_FirstVisibleRow = m_pVtScrollBar.Position;
						OnViewScrolled();
					}
				}
			}
        }

        #endregion

        #region method Process_mouseMoved

        internal void Process_mouseMoved(MouseEventArgs e)
		{
			// There is active drag operation, we don't need that event.
			if(m_pDragWindow != null){			
				return;
			}
			
			//--- Resize cursor refreshing ---------------------------------------//	
			m_pGrid.Cursor = Cursors.Default;
			WGridColumn mouseCol = GetColumnFromPoint(new Point(e.X,e.Y));
			if(mouseCol != null){
				if(e.X > mouseCol.Bounds.Right - 10 && e.X < mouseCol.Bounds.Right - 2){
					m_pGrid.Cursor = Cursors.SizeWE;
				}		
			}
			//--------------------------------------------------------------------//
        }

        #endregion

        #region method Process_mouseDragged
                
		internal void Process_mouseDragged(MouseEventArgs e)
		{	
			if(m_pDragWindow == null){
				WGridColumn mouseCol = GetColumnFromPoint(new Point(e.X,e.Y));
			
				//--- Column resize --------------------------------------------//
				int x = m_pGrid.Parent.PointToScreen(m_pGrid.Location).X + e.X;
				int y = m_pGrid.Parent.PointToScreen(m_pGrid.Location).Y + ColumnsRect.Y;		
				if(mouseCol != null && e.X > mouseCol.Bounds.Right - 10 && e.X <= mouseCol.Bounds.Right){
					m_pDragWindow = new WDragWindow(m_pGrid);
					m_pDragWindow.Tag = new Object[]{mouseCol,true};
					m_pDragWindow.Show(x,y,1,this.Bounds.Height - m_pHzScrollBar.Bounds.Height,Color.Black);
					
					m_pGrid.SetFocusedView(this);
				}			
				//--- If column drag (reorder)
				else if(AllowReorderColumns && mouseCol != null && e.X >= mouseCol.Bounds.X + 10){
					m_pDragColumn = mouseCol;
					
					m_pDragWindow = new WDragWindow(m_pGrid);
					m_pDragWindow.Tag = new Object[]{mouseCol,false};
								
					//----------- Draw column to image
					Bitmap image = new Bitmap(mouseCol.Bounds.Width,mouseCol.Bounds.Height);
					Graphics g = Graphics.FromImage(image);
					
					// Fill backround
					g.FillRectangle(new SolidBrush(Color.FromArgb(212,208,200)),0,0,mouseCol.Bounds.Width,mouseCol.Bounds.Height);
								
					// Paint text
					LumiSoft.UI.Controls.Paint.DrawText(g,Color.Black,mouseCol.Font,mouseCol.Text,new Rectangle(0,0,mouseCol.Bounds.Width,mouseCol.Bounds.Height),HorizontalAlignment.Center);

					// Paint border around column
					PaintRaisedBorder(g,new Rectangle(0,0,mouseCol.Bounds.Width,mouseCol.Bounds.Height));
					
					g.Dispose();
					//----------------------------------
					
					m_pDragWindow.Show(x,y - mouseCol.Bounds.Height,mouseCol.Bounds.Width,mouseCol.Bounds.Height,image);
					
					m_pGrid.SetFocusedView(this);
				}
			}
		
			// There is current drag operation, update drag window location.
			if(m_pDragWindow != null){
				m_pDragWindow.UpdatePosition(m_pGrid.Parent.PointToScreen(m_pGrid.Location).X + e.X);
			
				WGridColumn mouseCol = GetColumnFromPoint(new Point(e.X,e.Y));
									
				// Draw column reorder destination column hot rect
				Object[] tag = (Object[])m_pDragWindow.Tag;
			
				// Restore default grid header
				Graphics g = m_pGrid.GetPaintGraphics();
				PaintHeader(g);
				m_pGrid.Invalidate(this.ColumnsRect);
			
				if(mouseCol != null && !mouseCol.Equals(m_pDragColumn) && !((bool)tag[1])){
					// Paint new destination column
					g.DrawRectangle(new Pen(Color.Green),mouseCol.Bounds.X,mouseCol.Bounds.Y,mouseCol.Bounds.Width - 1,mouseCol.Bounds.Height - 1);
					m_pGrid.Invalidate(this.ColumnsRect);								
				}
			}
        }

        #endregion

        //-------------------------------------------------------------------------//


        #region method OnColumnChanged

        /// <summary>
		/// Is called when column porperty changed.
		/// </summary>
		/// <param name="column"></param>
		internal void OnColumnChanged(WGridColumn column)
		{
			HideEditor();
			
			Calculate(true);
        }

        #endregion

        #region method Calculate

        /// <summary>
		/// Calculates table view draw info.
		/// </summary>
		/// <param name="repaint"></param>
		internal void Calculate(bool repaint)
		{	
            if(!m_pGrid.m_pActiveViews.Contains(this)){
                return;
            }
	
			int rowIndicatorsWidth = 20;
			if(!ShowRowIndicators){
				rowIndicatorsWidth = 0;
			}
			int masterDetailWidth = 20;
			if(!MasterDetail){
				masterDetailWidth = 0;
			}
			m_ColumnsStartX   = Bounds.X + rowIndicatorsWidth + masterDetailWidth;
			int colsHeight    = CalculateColumnsHeight();
			int footherHeight = FooterRect.Height;
			if(!ShowFooter){
				footherHeight = 0;
			}
			
			// Hide all this View sub grids
            foreach(WGridTableView view in m_pGrid.m_pActiveViews){
                if(view.m_pParentView == this){
					view.setBounds(new Rectangle(-1,-1,0,0));
					view.HideScrollBars();
				}
            }

			//--- Calculate columns info --------------------------------------//
			int colsWidth  = 0;		
			int colX       = m_ColumnsStartX;
			//--- Calculate columns total width -------//
			foreach(WGridColumn col in this.Columns){			
				if(col.Visible){				
					colsWidth += col.Width;			
				}			
			}
			//-----------------------------------------//
			
			bool hzScrollbarVisible = m_ColumnsStartX + colsWidth + 20 > Bounds.Right;
			
			//--- Calculate columns bounds -------------//
			foreach(WGridColumn col in this.Columns){		
				if(col.Visible){
					if(hzScrollbarVisible){
						col.setBounds(new Rectangle(colX - m_pHzScrollBar.Position,Bounds.Y,col.Width,colsHeight));
					}
					else{
						col.setBounds(new Rectangle(colX,Bounds.Y,col.Width,colsHeight));
					}
					colX += col.Width;			
				}			
			}
			//------------------------------------------------------------------//
							
			//--- Calculate rows info ---------------------------------------//
			bool vtScrollbarVisible = m_FirstVisibleRow != 0;
			int rowStartY = Bounds.Top + colsHeight;	
			int rowMaxAllowedY = Bounds.Bottom - footherHeight;
			if(hzScrollbarVisible){
				rowMaxAllowedY -= m_pHzScrollBar.Bounds.Height;
			}
			for(int i=m_FirstVisibleRow;i<m_pRowInfos.Count;i++){
				_RowInfo rowInfo = m_pRowInfos[i];
						
				int rowHeight = CalculateRowHeight(rowInfo);
				// Row is over visble area
				if(rowStartY > rowMaxAllowedY){
					vtScrollbarVisible = true;
					break;
				}
				else{				
					rowInfo.RowHeight = rowHeight;
					rowInfo.Bounds = new Rectangle(m_ColumnsStartX,rowStartY,colX - m_ColumnsStartX,rowInfo.RowHeight);
				
					// Update sub grid size,location
					if(rowInfo.DetailView != null){
						int maxHeight = RowsRect.Bottom - rowStartY - rowInfo.RowHeight;
						rowInfo.DetailView.setBounds(new Rectangle(rowInfo.Bounds.X,rowInfo.Bounds.Y + rowInfo.RowHeight,Bounds.Width - rowInfo.Bounds.X,maxHeight));
						rowInfo.DetailView.AutoHeight();
						rowInfo.Bounds = new Rectangle(rowInfo.Bounds.X,rowInfo.Bounds.Y,rowInfo.Bounds.Width,rowInfo.RowHeight + rowInfo.DetailView.Bounds.Height);
					}				
				}
				m_LastVisibleRow = i;
				rowStartY += rowInfo.Bounds.Height;
			}
			// See if last visible row partially visible
			if(m_pRowInfos.Count > 0 && !vtScrollbarVisible){
				if(rowStartY > rowMaxAllowedY){
					vtScrollbarVisible = true;
				}
			}

			// If vertical scrollbar visible, we need to update row and open View width
			if(vtScrollbarVisible){
				for(int i=m_FirstVisibleRow;i<=m_LastVisibleRow;i++){
					_RowInfo rowInfo = m_pRowInfos[i];
					
					Rectangle r = rowInfo.Bounds;
					rowInfo.Bounds = new Rectangle(r.X,r.Y,r.Width - 18,r.Height);
					if(rowInfo.DetailView != null){
						r = rowInfo.DetailView.Bounds;
						rowInfo.DetailView.setBounds(new Rectangle(r.X,r.Y,r.Width - 18,r.Height));						
					}
				}
			}
			//--------------------------------------------------------------------------//
					
			//--- Set scrollbars -------------------------------------------------------//
			if(!hzScrollbarVisible){
				m_pHzScrollBar.Visible = false;
				m_pHzScrollBar.Bounds = new Rectangle(-1,-1,0,0);
				m_pHzScrollBar.Position = 0;
                m_pHzScrollBar.Maximum = 0;
			}
			else{			
				if(vtScrollbarVisible){
					m_pHzScrollBar.Bounds = new Rectangle(this.Bounds.X,this.Bounds.Bottom - 17,(int)this.Bounds.Width - 17,17);
				}
				else{
					m_pHzScrollBar.Bounds = new Rectangle(this.Bounds.X,this.Bounds.Bottom - 17,(int)this.Bounds.Width,17);
				}
				m_pHzScrollBar.Maximum = m_ColumnsStartX + colsWidth - this.Bounds.Width + 20;
				m_pHzScrollBar.Visible = true;
			}		
			if(!vtScrollbarVisible){
				m_pVtScrollBar.Visible = false;
				m_pVtScrollBar.Bounds = new Rectangle(-1,-1,0,0);
				m_pVtScrollBar.Position = 0;
                m_pVtScrollBar.Maximum = 0;
			}
			else{	
				if(hzScrollbarVisible){
					m_pVtScrollBar.Bounds = new Rectangle(this.Bounds.Right - 18,this.Bounds.Y,18,this.Bounds.Height - 18);
				}
				else{
					m_pVtScrollBar.Bounds = new Rectangle(this.Bounds.Right - 18,this.Bounds.Top,18,this.Bounds.Height);
				}
				m_pVtScrollBar.Maximum = m_pDataSource.Count - 1;
				m_pVtScrollBar.Visible = true;
			}
			//--------------------------------------------------------------------------//
			
			if(repaint){
				Repaint();
			}
        }

        #endregion

        #region method HideScrollBars

        internal void HideScrollBars()
		{
			m_pHzScrollBar.Visible = false;
			m_pVtScrollBar.Visible = false;
        }

        #endregion

        #region method AutoHeight

        internal void AutoHeight()
		{
			if(m_pRowInfos.Count == 0){
				return;
			}
		
            if(m_LastVisibleRow > -1 && (m_pRowInfos.Count - 1 == m_LastVisibleRow)){
                if((m_pRowInfos[m_LastVisibleRow].Bounds.Bottom + 17) < this.RowsRect.Bottom){
                    this.setBounds(new Rectangle(m_pBounds.X,m_pBounds.Y,m_pBounds.Width,m_pBounds.Height - (this.RowsRect.Bottom - (m_pRowInfos[m_LastVisibleRow].Bounds.Bottom + 17))));
                }
            }
            /*
			_RowInfo rowInfo = m_pRowInfos[m_pRowInfos.Count - 1];
			if(m_FirstVisibleRow == 0 && m_pRowInfos.Count - 1 == m_LastVisibleRow){
				int height = RowsRect.Bottom - rowInfo.Bounds.Y - rowInfo.RowHeight;
				if(height > 0 && height < m_pBounds.Height){
                    // Leave always 17 pixels for HZ scrollbar.
					this.setBounds(new Rectangle(m_pBounds.X,m_pBounds.Y,m_pBounds.Width,m_pBounds.Height - height + 2 + 17));
				}
			}*/
        }

        #endregion

        #region method CalculateColumnsHeight

        /// <summary>
		/// Calculats columns height.
		/// </summary>
		/// <returns></returns>
		private int CalculateColumnsHeight()
		{
			if(ShowColumns){
				return 20;
			}
			else{
				return 0;
			}
        }

        #endregion

        #region method CalculateRowHeight

        /// <summary>
		/// Calculates row height.
		/// </summary>
		/// <param name="_RowInfo"></param>
		/// <returns></returns>
		private int CalculateRowHeight(_RowInfo _RowInfo)
		{
		//	Graphics g = m_pGrid.();
		//	if(g == null){
		//		return 19;
		//	}
			
			int rowHeight = 19 + ((int)m_pGrid.ViewStyle.GridCellsFont.Size - 8);
		/*	foreach(Column col in this.Columns){			
				int h = col.Editor.CalculateHeight(g,_RowInfo.RowSource[col.MappingName],col);
				if(h > rowHeight){
					// TODO: don't allow bigger than grid
					rowHeight = h;
				}
			}*/
			
			return rowHeight;
        }

        #endregion

        #region method ClearSelection

        /// <summary>
		/// Clears view selected rows.
		/// </summary>
		public void ClearSelection()
		{
			if(m_pActiveRow != null){
				_RowInfo activeRow = m_pActiveRow;

				m_pActiveRow = null;
				RepaintRow(activeRow);
			}
			
			_RowInfo[] selectedRows = m_pSelectedRows.ToArray();
			m_pSelectedRows.Clear();
			
            foreach(_RowInfo row in selectedRows){
                RepaintRow(row);
            }
        }

        #endregion

        #region method SelectRow

        /// <summary>
		/// Activates specified row. If row isn't visible, it will be scrolled to visible area.
		/// </summary>
		/// <param name="_RowInfo"></param>
		/// <param name="col"></param>
		/// <param name="clearSelection"></param>
		private void SelectRow(_RowInfo _RowInfo,WGridColumn col,bool clearSelection)
		{
			// Row or column didn't changed
			if(m_pActiveRow == _RowInfo && m_pActiveColumn == col){
				ShowEditor(col);
				return;
			}
			// New row different from active row, but active row modified and we can't allow to chage row. 
			else if(m_pActiveRow != _RowInfo && IsActiveRowModified()){
				return;
			}
			
			HideEditor();
			
			// Focused view changed
			if(m_pGrid.FocusedView != null && !m_pGrid.FocusedView.Equals(this)){
				m_pGrid.FocusedView.HideEditor();
				m_pGrid.FocusedView.ClearSelection();
			}
			
            _RowInfo oldRow = m_pActiveRow;

			// Clear all selected rows
			if(clearSelection){
				ClearSelection();
			}
												
			if(col != null){
				m_pActiveColumn = col;
			}
						
			m_pGrid.SetFocusedView(this);
			m_pActiveRow = _RowInfo;
			if(oldRow != m_pActiveRow){
				m_ActiveRowValues = (Object[])_RowInfo.RowSource.Row.ItemArray.Clone();
			}
			// add active row selected rows list
			if(!m_pSelectedRows.Contains(m_pActiveRow)){
				m_pSelectedRows.Add(m_pActiveRow);
			}
			
			bool repaintNeeded = false;
			//--- Scroll if needed ----------------------------------------------//
			if(m_FirstVisibleRow > m_pRowInfos.IndexOf(m_pActiveRow)){
				m_pVtScrollBar.Position = m_pRowInfos.IndexOf(m_pActiveRow);
				m_FirstVisibleRow = m_pVtScrollBar.Position;
				Calculate(false);
				
				repaintNeeded = true;
			}
			else if(m_LastVisibleRow > -1 && m_LastVisibleRow < m_pRowInfos.IndexOf(m_pActiveRow)){
                int counter = 0;
			    while(m_LastVisibleRow < m_pRowInfos.IndexOf(m_pActiveRow) && counter < m_pRowInfos.Count){
				    m_pVtScrollBar.Position = m_pVtScrollBar.Position + 1;
				    m_FirstVisibleRow = m_pVtScrollBar.Position;
				    Calculate(false);
				    counter++;
			    }
			    repaintNeeded = true;
			}
			//--------------------------------------------------------------------//
	
			// Row isn't visible, make it fully visible
			if(m_LastVisibleRow > -1 && RowsRect.Bottom < m_pActiveRow.Bounds.Bottom){                
				int counter = 0;
				while(RowsRect.Bottom < m_pActiveRow.Bounds.Bottom && counter < m_pRowInfos.Count){ 
					m_pVtScrollBar.Position = m_pVtScrollBar.Position + 1;
					m_FirstVisibleRow = m_pVtScrollBar.Position;
					Calculate(false);
					counter++;                    
				}
				repaintNeeded = true;
			}
			
			// Column isn't fully visible, make it
			if(col != null){
				if(col.Bounds.Right > this.Bounds.Right - m_pVtScrollBar.Width){
					m_pHzScrollBar.Position = m_pHzScrollBar.Position + col.Bounds.Right - this.Bounds.Right + m_pVtScrollBar.Width + 10;
					Calculate(false);
					repaintNeeded = true;
				}
				if(col.Bounds.Left < m_ColumnsStartX){
					m_pHzScrollBar.Position = m_pHzScrollBar.Position - (m_ColumnsStartX - col.Bounds.Left);
					Calculate(false);
					repaintNeeded = true;
				}
			}

			if(oldRow != m_pActiveRow){
				m_pGrid.RaiseOnActiveRowChanged(m_pActiveRow.RowSource);
			}
			
			// We need to repaint all
			if(repaintNeeded){
				Repaint();
			}
			// Repaint old and new active row 
			else{
				if(oldRow != null){
					RepaintRow(oldRow);
				}
				RepaintRow(m_pActiveRow);
			}
			
			if(col != null){
				ShowEditor(col);
			}
        }

        #endregion

        #region method SelectFirstVisibleRow

        /// <summary>
		/// Selects first visible row.
		/// </summary>
		public void SelectFirstVisibleRow()
		{
            if(m_pRowInfos == null || m_pRowInfos.Count == 0){
                return;
            }

			SelectRow(m_pRowInfos[m_FirstVisibleRow],m_pActiveColumn,true);
        }

        #endregion

        #region method SelectLastVisibleRow

        /// <summary>
		/// Selects last visible row.
		/// </summary>
		public void SelectLastVisibleRow()
		{
            if(m_pRowInfos == null || m_pRowInfos.Count == 0){
                return;
            }

			_RowInfo lastVisbleRow = null;
			for(int i=m_FirstVisibleRow;i<=m_LastVisibleRow;i++){
				_RowInfo _RowInfo = m_pRowInfos[i];
				if(_RowInfo.Bounds.Bottom <= RowsRect.Bottom){
					lastVisbleRow = _RowInfo;
				}
				else{
					break;
				}
			}
		
			SelectRow(lastVisbleRow,m_pActiveColumn,true);
        }

        #endregion

        #region method SelectLastRow

        /// <summary>
        /// Selects last row in view.
        /// </summary>
        public void SelectLastRow()
        {
            if(m_pRowInfos.Count > 0){
				SelectRow(m_pRowInfos[m_pRowInfos.Count - 1],m_pActiveColumn,true);
                Repaint();
			}
        }

        #endregion

        #region method SelectNextRow

        /// <summary>
		/// Selects next row. If next row isn't visible, then scrolls and makes that row visible.
		/// </summary>
		private void SelectNextRow()
		{
			SelectNextRow(m_pActiveRow);
        }

        /// <summary>
		/// Selects next row from specified row. If next row isn't visible, then scrolls and makes that row visible.
		/// </summary>
		/// <param name="row"></param>
		private void SelectNextRow(_RowInfo row)
		{
			if(m_pRowInfos.IndexOf(row) < m_pRowInfos.Count - 1){
				SelectRow(m_pRowInfos[m_pRowInfos.IndexOf(row) + 1],m_pActiveColumn,true);
			}
        }

        #endregion

        #region method SelectPreviousRow

        /// <summary>
		/// Selects previous row. If next previous isn't visible, then scrolls and makes that row visible.
		/// </summary>
		private void SelectPreviousRow()
		{
			if(m_pRowInfos.IndexOf(m_pActiveRow) > 0){			
				SelectRow((_RowInfo)m_pRowInfos[m_pRowInfos.IndexOf(m_pActiveRow) - 1],m_pActiveColumn,true);
			}
        }

        #endregion

        #region method SelectRows

        /// <summary>
		/// Selects specified range of rows.
		/// </summary>
		/// <param name="startRow">Selection start.</param>
		/// <param name="endRow">Selection end.</param>
		private void SelectRows(_RowInfo startRow,_RowInfo endRow)
		{
            SelectRows(startRow,endRow,true);
        }

        /// <summary>
		/// Selects specified range of rows.
		/// </summary>
		/// <param name="startRow">Selection start.</param>
		/// <param name="endRow">Selection end.</param>
        /// <param name="activateEndRow">Specifies if <b>endRow</b> is activated.</param>
		private void SelectRows(_RowInfo startRow,_RowInfo endRow,bool activateEndRow)
		{
			m_pSelectedRows.Clear();
			
			if(m_pRowInfos.IndexOf(startRow) == m_pRowInfos.IndexOf(endRow)){
				m_pSelectedRows.Add(startRow);
			}
			if(m_pRowInfos.IndexOf(startRow) < m_pRowInfos.IndexOf(endRow)){
				for(int i=m_pRowInfos.IndexOf(startRow);i<=m_pRowInfos.IndexOf(endRow);i++){
					m_pSelectedRows.Add((_RowInfo)m_pRowInfos[i]);
				}
			}
			else{
				for(int i=m_pRowInfos.IndexOf(endRow);i<=m_pRowInfos.IndexOf(startRow);i++){
					m_pSelectedRows.Add((_RowInfo)m_pRowInfos[i]);
				}
			}
			
            if(activateEndRow){
			    SelectRow(endRow,m_pActiveColumn,false);
            }
        }

        #endregion

        #region method ToggleRowSelection

        /// <summary>
		/// Toggles row selection. If row is selected then unselects it, if row is unselected then selects it.
		/// </summary>
		/// <param name="_RowInfo"></param>
		/// <returns></returns>
		private bool ToggleRowSelection(_RowInfo _RowInfo)
		{
			if(m_pSelectedRows.Contains(_RowInfo)){
				m_pSelectedRows.Remove(_RowInfo);
			
				if(_RowInfo.Equals(m_pActiveRow)){
					m_pActiveRow = null;
				}
			
				return false;
			}
			else{							
				SelectRow(_RowInfo,m_pActiveColumn,false);
			
				return true;
			}
        }

        #endregion

        #region method SelectFirstEditableColumn

        /// <summary>
		/// Selects first editable column in active row.
		/// </summary>
		public void SelectFirstEditableColumn()
		{
			HideEditor();
			m_pActiveColumn = null;
			MoveNext();
        }

        #endregion

        #region method GetColumnFromPoint

        /// <summary>
		/// Gets column from specified point or null if there isn't any column at that point.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		private WGridColumn GetColumnFromPoint(Point pt)
		{
			foreach(WGridColumn col in this.Columns){
				if(col.Visible && col.Bounds.Contains(pt)){
					return col;
				}
			}
	
			return null;
        }

        #endregion

        #region method GetRowFromPoint

        /// <summary>
		/// Gets row from specified point or null if there isn't any row at that point.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		private _RowInfo GetRowFromPoint(Point pt)
		{
			// Find hitted row		
			for(int i=m_FirstVisibleRow;i<=m_LastVisibleRow;i++){
				_RowInfo _RowInfo = (_RowInfo)m_pRowInfos[i];
				
				if(this.RowsRect.Contains(pt) && pt.Y >= _RowInfo.Bounds.Y && pt.Y <= _RowInfo.Bounds.Bottom){
					return _RowInfo;
				}
			}
			
			return null;
        }

        #endregion

        #region method ShowEditor

        /// <summary>
		/// Acivates specified column editor.
		/// </summary>
		public void ShowEditor()
		{
            ShowEditor(m_pActiveColumn);
        }

        /// <summary>
		/// Acivates specified column editor.
		/// </summary>
		/// <param name="col"></param>
		public void ShowEditor(WGridColumn col)
		{
			HideEditor();
		
			// If grid readonly or column isn't visible, don't activate editor.
			if(m_AllowEdit && col != null && col.Visible && col.AllowEdit && m_pActiveRow != null){
				m_pActiveEditor = col.Editor;
				UpdateActiveEditor();
				m_pGrid.Controls.Add(m_pActiveEditor);
				m_pActiveEditor.StartEdit(this,m_pActiveRow.GetValue(col).ToString());
			
				RepaintRow(m_pActiveRow);
			}
        }

        #endregion

        #region method HideEditor

        /// <summary>
		/// Hides active editor, if exists.
		/// </summary>
		private void HideEditor()
		{
			// Hide active editor, if any
			if(m_pActiveEditor != null){
                m_SkipDataSourceEvents++;
                							
				// Store editor value to DataRowView.
                if(m_pActiveEditor.IsModified){
				    m_pActiveRow.RowSource[m_pActiveColumn.MappingName] = m_pActiveEditor.EditValue;
                }
									
				m_pGrid.Controls.Remove(m_pActiveEditor);
				m_pGrid.Focus();
				m_pActiveEditor = null;

                m_SkipDataSourceEvents--;
			}
        }

        #endregion

        #region method RefreshActiveEditorValue

        /// <summary>
        /// Refreshed active editor value from datasource.
        /// </summary>
        public void RefreshActiveEditorValue()
        {
            if(m_pActiveEditor != null){
                m_pActiveEditor.Value = m_pActiveRow.RowSource[m_pActiveColumn.MappingName];
            }
        }

        #endregion

        #region method UpdateActiveEditor

        /// <summary>
		/// Updates active editor size and location.
		/// </summary>
		private void UpdateActiveEditor()
		{
			WGridColumn col = m_pActiveColumn;
			if(m_pActiveEditor != null){
				// Part of Column is bigger or out of view
				if(col.Bounds.Right > Bounds.Right - m_pVtScrollBar.Width){
					m_pActiveEditor.Bounds = new Rectangle(col.Bounds.Left + 2,m_pActiveRow.getY + 2,Bounds.Right - m_pVtScrollBar.Width - col.Bounds.Left - 2,m_pActiveRow.RowHeight - 3);
				}
				else{
					m_pActiveEditor.Bounds = new Rectangle(col.Bounds.Left + 2,m_pActiveRow.getY + 2,col.Bounds.Width - 3,m_pActiveRow.RowHeight - 3);
				}
				m_pActiveEditor.Focus();
			}
        }

        #endregion

        #region method IsActiveRowModified

        /// <summary>
		/// Gets if active row is modified.
		/// </summary>
		/// <returns>If some of the active row values modified, returns true. If no active row, returns false.</returns>
		private bool IsActiveRowModified()
		{
            if(!m_AllowEdit){
                return false;
            }

			if(m_ActiveRowValues != null && m_pActiveRow != null){			
				for(int i=0;i<m_ActiveRowValues.Length;i++){
                    if(!object.Equals(m_ActiveRowValues[i],m_pActiveRow.RowSource[i])){                        
                        return true;
                    }
				}
			}
			if(m_pActiveEditor != null && m_pActiveEditor.IsModified){
				return true;
			}
			
			return false;
        }

        #endregion

        #region method DeleteActiveRow

        /// <summary>
		/// Deletes active row.
		/// </summary>
		public void DeleteActiveRow()
		{
			if(m_pActiveRow != null && !m_pActiveRow.RowSource.IsNew){
                m_SkipDataSourceEvents++;

                HideEditor();

				// Raise delete row event and see if allowed or canceled
				if(m_pGrid.RaiseDeleteRow(m_pActiveRow.RowSource)){
					_RowInfo newActiveRow = null;
					int index = m_pRowInfos.IndexOf(m_pActiveRow);			
					if(index < m_pRowInfos.Count){
						newActiveRow = m_pRowInfos[index + 1];
					}
					else if(index > 0){
						newActiveRow = m_pRowInfos[index - 1];
					}			
			
					m_pRowInfos.Remove(m_pActiveRow);
					m_pActiveRow.RowSource.Delete();
                    m_pActiveRow.RowSource.EndEdit();
					m_pActiveRow = null;
                    m_pSelectedRows.Clear();
		
                    // Workaround: if to delete last row from dataview, it deletes also newitem row.
                    if(m_pDataSource.Count == 0 && m_pRowInfos.Count == 1){                        
                        m_pRowInfos.Clear();
                        AddNewItemRow();
                        newActiveRow = m_pRowInfos[0];
                    }
            	   
				    Calculate(true);
				    SelectRow(newActiveRow,m_pActiveColumn,true);                
				}

                ShowEditor();

                m_SkipDataSourceEvents--;
			}
        }

        #endregion

        #region method MoveNext

        /// <summary>
		/// Moves current column to next column or if last column, then moves to next row.
		/// </summary>
		public void MoveNext()
		{            
            m_SkipDataSourceEvents++;
            try{  
                // No rows.
                if(m_pRowInfos.Count == 0){
                    return;
                }
			    // No columns, we can't move
			    if(m_pColumns.Count == 0){
				    return;
			    }

			    // There is no active row, make first row as active.
			    if(m_pActiveRow == null && Columns.VisibleColumns.Length > 0){
				    SelectRow(m_pRowInfos[0],Columns.VisibleColumns[0],true);
				    return;
			    }

			    int activeColIndex = -1;
			    if(m_pActiveColumn != null){
				    activeColIndex = m_pColumns.IndexOf(m_pActiveColumn);
			    }

			    // If view is editable call validate cell value event.
			    if(m_pActiveEditor != null && !m_pGrid.RaiseValidateCellValue(m_pActiveEditor)){
				    return;
			    }
			    HideEditor();
					
			    bool moveNextRow = false;						
			    // Move next column.
			    if(activeColIndex < m_pColumns.Count - 1){
				    WGridColumn column = m_pColumns[activeColIndex + 1];
					
				    // If column not visible or view is editable and column isn't, move next editable column or row.
				    if(!column.Visible || (AllowEdit && !column.AllowEdit)){
					    moveNextRow = true;
						
					    // Find next visible and editable column, if exists.
					    while(m_pColumns.IndexOf(column) < m_pColumns.Count - 1){
						    column = m_pColumns[m_pColumns.IndexOf(column) + 1];						
						    if((column.Visible && !AllowEdit) || (column.Visible && column.AllowEdit)){
							    moveNextRow = false;
							    break;
						    }
					    }					
				    }
					
				    if(!moveNextRow){
					    SelectRow(m_pActiveRow,column,false);
				    }
			    }
			    // No next editable column.
			    else{
				    moveNextRow = true;
			    }
				
			    // Move next row
			    if(moveNextRow){
				    // Update or Add new row
				    if(this.AllowEdit){										
					    if(m_pActiveRow.RowSource.IsNew){
						    if(m_pGrid.RaiseAddNewRow(m_pActiveRow.RowSource)){
							    m_pActiveRow.RowSource.EndEdit();
								
							    // Append new item row
							    AddNewItemRow();
							    this.Calculate(true);

                                m_pActiveColumn = null;
							    m_ActiveRowValues = null;                            
                                SelectRow(m_pRowInfos[m_pRowInfos.Count - 1],null,true);
                                // Move to first visible column of if ediatable then to first ediatable column.
                                MoveNext();
						    }
                            // Add row canceled
                            else{
                                ShowEditor();
                                return;
                            }
					    }
					    else{
						    // Call update only if active row is modified.
						    if(this.IsActiveRowModified()){
							    // Update row canceled
							    if(!m_pGrid.RaiseUpdateRow(m_pActiveRow.RowSource)){
                                    ShowEditor();
								    return;
							    }
						    }

    						m_pActiveColumn = null;
	    					m_ActiveRowValues = null;
		    				// Move to first visible column of if ediatable then to first ediatable column
			    			MoveNext();
				    		SelectNextRow();
			                this.Repaint();
					    }
				    }
				    else{
					    m_pActiveColumn = null;
					    m_ActiveRowValues = null;
					    // Move to first visible column of if ediatable then to first ediatable column
					    MoveNext();
					    SelectNextRow();					
				    }	
			    }
            }
            finally{
                m_SkipDataSourceEvents--;
            }
        }

        #endregion

        #region method ExpandRow

        /// <summary>
		/// Expands detail view.
		/// </summary>
		/// <param name="rowInfo">Row to expand.</param>
		internal void ExpandRow(_RowInfo rowInfo)
		{            
			if(this.MasterDetail && rowInfo.DetailView == null){
				WGridTableView detailView = m_pDetailView.Clone();
				detailView.m_pParentView = this;
				detailView.m_pParentRow = rowInfo;
				detailView.DataSource = rowInfo.RowSource.CreateChildView(m_pRelation.RelationName);
				rowInfo.DetailView = detailView;
                m_pGrid.m_pActiveViews.Add(detailView);
                detailView.Calculate(false);
                				
				this.Calculate(true);
                
                        
                if(detailView.Bounds.Height < 60){
                    m_FirstVisibleRow += m_pRowInfos.IndexOf(rowInfo) > (m_FirstVisibleRow + 3) ? 3 : 1;
                    this.Calculate(true);
                }
			}
        }

        #endregion

        #region method CollapseRow

        /// <summary>
		/// Collapses detail view.
		/// </summary>
		/// <param name="rowInfo">Row to collapse.</param>
		internal void CollapseRow(_RowInfo rowInfo)
		{
			if(rowInfo.DetailView != null){
				m_pGrid.SetFocusedView(this);
				m_pGrid.m_pActiveViews.Remove(rowInfo.DetailView);
				rowInfo.DetailView.Dispose();
				rowInfo.DetailView = null;
				rowInfo.Bounds = new Rectangle(rowInfo.Bounds.X,rowInfo.Bounds.Y,rowInfo.Bounds.Width,rowInfo.RowHeight);
				m_pGrid.Focus();

				this.Calculate(true);
			}
        }

        #endregion

        #region method ToggleRow

        /// <summary>
		/// Toggles detail view. If view is open then closes it, if view is closed then opens it.
		/// </summary>
		/// <param name="rowInfo"></param>
		internal void ToggleRow(_RowInfo rowInfo)
		{
			if(rowInfo.DetailView == null){
				ExpandRow(rowInfo);
			}
			else{
				CollapseRow(rowInfo);
			}
        }

        #endregion

        #region method UndoRow

        /// <summary>
		/// Restores active row default values.
		/// </summary>
		public void UndoRow()
		{
            m_SkipDataSourceEvents++;

			if(m_pActiveRow != null){
				m_pActiveRow.RowSource.Row.ItemArray = m_ActiveRowValues;

				if(m_pActiveEditor != null){
					m_pActiveEditor.Value = m_pActiveRow.RowSource[m_pActiveColumn.MappingName];
				}

				SelectFirstEditableColumn();

                m_ActiveRowValues = m_pActiveRow.RowSource.Row.ItemArray;
			}

            m_SkipDataSourceEvents--;
        }

        #endregion

        #region method AddNewItemRow

        /// <summary>
		/// Shows new item row if allowed.
		/// </summary>
		private void AddNewItemRow()
		{
            m_SkipDataSourceEvents++;

			// Append new item row
			if(AllowAppendNewRow){
		        DataRowView dr = m_pDataSource.AddNew();
                //dr.EndEdit();

                m_pGrid.RaiseNewItemRowAdded(dr);

				// This is child grid, we need to fill relation column
				if(m_pParentRow != null && m_pRelation != null){
				    dr[m_pRelation.ChildColumns[0].ColumnName] = m_pParentRow.RowSource[m_pRelation.ParentColumns[0].ColumnName];
				}

				m_pRowInfos.Add(new _RowInfo(this,dr));
            }

            m_SkipDataSourceEvents--;
        }

        #endregion

        #region method PageUp

        /// <summary>
        /// Scrolls page up in view. If there is no pages up, nothing is done.
        /// </summary>
        public void PageUp()
        {
            if((m_FirstVisibleRow - (m_LastVisibleRow - m_FirstVisibleRow)) > 0){
			    SelectRow(m_pRowInfos[(m_FirstVisibleRow - (m_LastVisibleRow - m_FirstVisibleRow))],m_pActiveColumn,true);
		    }
			else{
			    SelectRow(m_pRowInfos[0],m_pActiveColumn,true);
		    }
        }

        #endregion

        #region method PageDown

        /// <summary>
        /// Scrolls page down in view. If there is no pages down, nothing is done.
        /// </summary>
        public void PageDown()
        {
            if(m_LastVisibleRow + (m_LastVisibleRow - m_FirstVisibleRow - 1) < m_pRowInfos.Count - 1){
			    SelectRow(m_pRowInfos[m_LastVisibleRow + (m_LastVisibleRow - m_FirstVisibleRow - 1)],m_pActiveColumn,true);
			}
			else{
				SelectRow(m_pRowInfos[m_pRowInfos.Count - 1],m_pActiveColumn,true);
			}
        }

        #endregion

        #region method OnPageLeft

        /// <summary>
        /// Scrolls page left in view. If there is no pages left, nothing is done.
        /// </summary>
        public void OnPageLeft()
        {
            if(m_pHzScrollBar.Visible){
                m_pHzScrollBar.Position = Math.Max(0,m_pHzScrollBar.Position - this.RowsRect.Width);
                OnViewScrolled();
            }
        }

        #endregion

        #region method OnPageRight

        /// <summary>
        /// Scrolls page right in view. If there is no pages right, nothing is done.
        /// </summary>
        public void OnPageRight()
        {
            if(m_pHzScrollBar.Visible){
                m_pHzScrollBar.Position = Math.Max(m_pHzScrollBar.Position,m_pHzScrollBar.Position + this.RowsRect.Width);
                OnViewScrolled();
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets view name.
        /// </summary>
        public string Name
        {
            get{ return m_Name; }
        }

        /// <summary>
		/// Gets or sets grid data source.
		/// </summary>
		public DataView DataSource
		{
			get{ return m_pDataSource; }

			set{
				HideEditor();
				m_pRowInfos.Clear();
				m_pSelectedRows.Clear();
				m_FirstVisibleRow = 0;
				m_LastVisibleRow  = -1;
				m_pVtScrollBar.Position = 0;
				m_pVtScrollBar.Maximum = 0;
				m_pHzScrollBar.Position = 0;
				m_pHzScrollBar.Maximum = 0;
				m_pActiveRow = null;
				m_pActiveColumn = null;
				m_pRelation = null;
				
				// Remove old dataview chaged listener.
                if(m_pDataSource != null){
                    m_pDataSource.ListChanged -= new System.ComponentModel.ListChangedEventHandler(m_pDataSource_ListChanged);
                }
	
				// If no data source, create new empty placeholder table
				if(value == null){
					try{
						DataSet ds = new DataSet();
						m_pDataSource = ds.Tables.Add("empty").DefaultView;
					}
					catch{				
					}
				}
                else{
                    m_pDataSource = value;
                }
                                                
                // Add new dataview changed listener.
                m_pDataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(m_pDataSource_ListChanged);
                                				
				// See if there is any datarelation for this table
		/*		DataRelationCollection relations = m_pDataSource.Table.DataSet.Relations;
				for(int i=0;i<relations.Count;i++){
					DataRelation relation = relations[i];
					if(relation.ParentColumns[0].Table.Equals(m_pDataSource.Table)){
						m_pRelation = relation;
						m_MasterDetail = true;
						break;
					}
				}*/
				
				// Construct row info holder.
                DataRowView drNewItemRow = null;
                foreach(DataRowView dr in m_pDataSource){
                    if(dr.IsNew){
                        drNewItemRow = dr;
                    }
                    else{
                        m_pRowInfos.Add(new _RowInfo(this,dr));
                    }
                }
                // Set new-item row as last one.
                if(drNewItemRow != null){
                    m_pRowInfos.Add(new _RowInfo(this,drNewItemRow));
                }
				        
                // Append new item row
                if(value != null && this.AllowEdit && this.AllowAppendNewRow && drNewItemRow == null){
                    AddNewItemRow();
				}
				// ??? Shows new item row for outputs
				//if(value != null && this.AllowAppendNewRow && drNewItemRow == null){
				//	AddNewItemRow();
				//}
				
				Calculate(false);
                
				if(m_pParentView == null){		
					MoveNext();
				}

                m_pGrid.Repaint(false);                
			}
		}        
                
		/// <summary>
		/// Gets or sets detail view of this view.
		/// </summary>
		public WGridTableView DetailView
		{
			get{ return m_pDetailView; }

			set{
				if(m_pDetailView != value){
					m_pDetailView = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets detail view datarelation.
		/// </summary>
		public DataRelation DataRelation
		{
			get{ return m_pRelation; }

			set{
				if(m_pRelation != value){
					m_pRelation = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets is view is master/detail view.
		/// </summary>
		public bool MasterDetail
		{
			get{ return (m_pRelation != null && m_pRelation != null); }
		}

		/// <summary>
		/// Gets or sets if data is editable.
		/// </summary>
		public bool AllowEdit
		{
			get{ return m_AllowEdit; }

			set{
				if(m_AllowEdit != value){
					m_AllowEdit = value;

					if(m_pDataSource == null){
						return;
					}

                    if(!m_AllowEdit){
                        HideEditor();
                    }
                                        
					if(!m_AllowEdit){
						HideEditor();
						
						// Hide NewItem row
						foreach(DataRowView dr in m_pDataSource){
							if(dr.IsNew){
								// Delete this row from _RowInfos
								for(int r=0;r<m_pRowInfos.Count;r++){
									_RowInfo rowInfo = (_RowInfo)m_pRowInfos[r];
									if(rowInfo.RowSource.Equals(dr)){
										m_pRowInfos.Remove(rowInfo);
										break;
									}
								}
								
								dr.Delete();
								break;
							}
						}
					}
					else if(AllowAppendNewRow){
						// Show NewItem row
						AddNewItemRow();
					}
                     
                    m_pActiveRow    = null;
					m_pActiveColumn = null;
					
					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if view allows user to add new rows.
		/// </summary>
		public bool AllowAppendNewRow
		{
			get{ return m_AllowAppendNewRow; }

			set{
				if(m_AllowAppendNewRow != value){
					m_AllowAppendNewRow = value;

                    // No data, do nothing.
                    if(m_pDataSource == null){
                    }
				    else if(m_AllowAppendNewRow){
						// Show NewItem row
						AddNewItemRow();
					}
					else{
						HideEditor();
						
						// Hide NewItem row
						foreach(DataRowView dr in m_pDataSource){
							if(dr.IsNew){
								// Delete this row from _RowInfos
								for(int r=0;r<m_pRowInfos.Count;r++){
									_RowInfo rowInfo = m_pRowInfos[r];
									if(rowInfo.RowSource.Equals(dr)){
										m_pRowInfos.Remove(rowInfo);
										break;
									}
								}
									
								dr.Delete();
								break;
							}
						}
					}

					Calculate(true);

					if(AllowEdit){
						if(m_AllowAppendNewRow){
							// Show NewItem row
							AddNewItemRow();
						}
						else{
							HideEditor();
						
							// Hide NewItem row
							foreach(DataRowView dr in m_pDataSource){
								if(dr.IsNew){
									// Delete this row from _RowInfos
									for(int r=0;r<m_pRowInfos.Count;r++){
										_RowInfo rowInfo = (_RowInfo)m_pRowInfos[r];
										if(rowInfo.RowSource.Equals(dr)){
											m_pRowInfos.Remove(rowInfo);
											break;
										}
									}
									
									dr.Delete();
									break;
								}
							}
						}

						Calculate(true);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets if view is sortable.
		/// </summary>
		public bool AllowSort
		{
			get{ return m_AllowSort; }

			set{
				if(m_AllowSort != value){
					m_AllowSort = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if multiple selection is allowed.
		/// </summary>
		public bool AllowMultiSelect
		{
			get{ return m_AllowMultiSelect; }

			set{
				if(m_AllowMultiSelect != value){
					m_AllowMultiSelect = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if columns reordering is allowed.
		/// </summary>
		public bool AllowReorderColumns
		{
			get{ return m_AllowReorderColumns; }

			set{
				if(m_AllowReorderColumns != value){
					m_AllowReorderColumns = value;
				}
			}
		}
				
		/// <summary>
		/// Gets or sets if enter key simulates mouse double click or moves next.
		/// </summary>
		public bool EnterKeyDoubleClicks
		{
			get{ return m_EnterKeyDoubleClicks; }

			set{
				if(m_EnterKeyDoubleClicks != value){
					m_EnterKeyDoubleClicks = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if maximize/minimaize button is shown to Main grid view.
		/// NOTE: This property works for Main View only.
		/// </summary>
		public bool ShowMaximizeForMainView
		{
			get{ return m_ShowMaximizeForMainView; }

			set{
				if(m_ShowMaximizeForMainView != value){
					m_ShowMaximizeForMainView = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if columns are visible.
		/// </summary>
		public bool ShowColumns
		{
			get{ return m_ShowColumns; }

			set{
				if(m_ShowColumns != value){
					m_ShowColumns = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if row indicators are visible.
		/// </summary>
		public bool ShowRowIndicators
		{
			get{ return m_ShowRowIndicators; }

			set{
				if(m_ShowRowIndicators != value){
					m_ShowRowIndicators = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if footer is visible.
		/// </summary>
		public bool ShowFooter
		{
			get{ return m_ShowFooter; }

			set{
				if(m_ShowFooter != value){
					m_ShowFooter = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if current cell is shown.
		/// NOTE: This property works only if View is not editable, otherwise it has no effect.
		/// </summary>
		public bool ShowCurrentCell
		{
			get{ return m_ShowCurrentCell; }

			set{
				if(m_ShowCurrentCell != value){
					m_ShowCurrentCell = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if vertical grid lines are shwon.
		/// </summary>
		public bool ShowVerticalGridlines
		{
			get{ return m_ShowVerticalGridlines; }

			set{
				if(m_ShowVerticalGridlines != value){
					m_ShowVerticalGridlines = value;

					Calculate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets if horizontal grid lines are shwon.
		/// </summary>
		public bool ShowHorizontalGridlines
		{
			get{ return m_ShowHorizontalGridlines; }

			set{
				if(m_ShowHorizontalGridlines != value){
					m_ShowHorizontalGridlines = value;

					Calculate(true);
				}
			}
		}
		
		/// <summary>
		/// Gets columns collection.
		/// </summary>
		public WGridColumns Columns
		{
			get{ return m_pColumns; }
		}

		/// <summary>
		/// Gets table view bounding rectangle.
		/// </summary>
		public Rectangle Bounds
		{
			get{ return m_pBounds; }
		}
		internal void setBounds(Rectangle value)
		{
			if(m_pBounds != value){
				m_pBounds = value;
				this.Calculate(false);
			}
		}

		/// <summary>
		/// Gets currently active editor or null if no active editor.
		/// </summary>
		public WBaseEditor ActiveEditor
		{
			get{ return m_pActiveEditor; }
		}

		/// <summary>
		/// Gets active cell column or null if no current cell.
		/// </summary>
		/// <returns></returns>
		public WGridColumn ActiveColumn
		{
			get{ return m_pActiveColumn; }
		}

		/// <summary>
		/// Returns currently selected rows.
		/// </summary>
		public DataRowView[] SelectedRows
		{
			get{
				DataRowView[] retVal = new DataRowView[m_pSelectedRows.Count];
				for(int i=0;i<m_pSelectedRows.Count;i++){
					retVal[i] = m_pSelectedRows[i].RowSource;
				}
				
				return retVal;
			}
		}

		/// <summary>
		/// Gets or tag (user data).
		/// </summary>
		public object Tag
		{
			get{ return m_pTag; }

			set{
				if(m_pTag != value){
					m_pTag = value;
				}
			}
		}

		/// <summary>
		/// Gets view owner grid control.
		/// </summary>
		public WGridControl Grid
		{
			get{ return m_pGrid; }
		}


        private Rectangle MasterDetailRect
		{
			get{
				if(MasterDetail){
					if(ShowRowIndicators){
						return new Rectangle(Bounds.Left + 20,Bounds.Top,20,Bounds.Height);
					}
					else{
						return new Rectangle(Bounds.Left,Bounds.Top,20,Bounds.Height);
					}
				}
				else{
					return new Rectangle(-1,-1,0,0);
				}
			}
		}
		
		private Rectangle ColumnsRect
		{	
			get{
				if(ShowColumns){
					return new Rectangle(m_ColumnsStartX,Bounds.Top,this.Bounds.Width - m_pVtScrollBar.Bounds.Width,20);
				}
				else{
					return new Rectangle(-1,-1,0,0);
				}
			}
		}
		
		private Rectangle FooterRect
		{
			get{
				if(ShowFooter){
					return new Rectangle(Bounds.Left,Bounds.Bottom - 21 - m_pHzScrollBar.Bounds.Height,Bounds.Width - m_pVtScrollBar.Bounds.Width,20);
				}
				else{
					return new Rectangle(-1,-1,0,0);
				}
			}
		}
		
		private Rectangle RowsRect
		{
			get{
				return new Rectangle(Bounds.Left,Bounds.Top + ColumnsRect.Height,Bounds.Width - m_pVtScrollBar.Bounds.Width - 1,Bounds.Height - ColumnsRect.Height - FooterRect.Height - m_pHzScrollBar.Bounds.Height - 1);
			}
		}

		/// <summary>
		/// Returns components owned by this view.
		/// </summary>
		internal ArrayList Components
		{
			get{ 
				ArrayList retVal = new ArrayList();
				retVal.Add(m_pHzScrollBar);
				retVal.Add(m_pVtScrollBar);

				return retVal; 
			}
		}

		#endregion

	}
}
