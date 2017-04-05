using System;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Summary description for WScrollBar.
	/// </summary>
	public class WScrollBar : UserControl
	{
		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
		public event EventHandler PositionChanged = null;

		private ViewStyle m_ViewStyle       = null;
		private int       m_Minimum         = 0;
		private int       m_Maximum         = 0;
		private int       m_Position        = 0;
		private Point     m_MousePos        = new Point(-1,-1);
		private String    m_HittedObject    = "";
		private bool      m_Vertical        = true;
		private bool      m_Dragging        = false;
		private Rectangle m_pScrollPaneRect = new Rectangle(0,0,17,17);

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WScrollBar()
		{	
			m_ViewStyle = new ViewStyle();
			
			SetStyle(ControlStyles.Selectable,false);
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);
            SetStyle(ControlStyles.ResizeRedraw,true);
        }


        #region method OnPaint

        protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			PaintX(e.Graphics);
		}

		private void PaintX(Graphics g)
		{				
			// Fill backround
			g.FillRectangle(new SolidBrush(Color.FromArgb(242,242,242)),0,0,this.Width,this.Height);
			g.DrawRectangle(new Pen(Color.FromArgb(212,208,200)),0,0,this.Width - 1,this.Height - 1);
			
			// Draw scroll pane
			g.FillRectangle(new SolidBrush(m_ViewStyle.GetButtonColor(ScrollPaneRect.Contains(m_MousePos) || m_Dragging,Control.MouseButtons == MouseButtons.Left)),m_pScrollPaneRect.X,m_pScrollPaneRect.Y,m_pScrollPaneRect.Width,m_pScrollPaneRect.Height);
			g.DrawRectangle(new Pen(m_ViewStyle.GetBorderColor(ScrollPaneRect.Contains(m_MousePos) || m_Dragging)),m_pScrollPaneRect.X,m_pScrollPaneRect.Y,m_pScrollPaneRect.Width,m_pScrollPaneRect.Height);
				
			// Draw decrease button
			g.FillRectangle(new SolidBrush(m_ViewStyle.GetButtonColor(DecreaseButtonRect.Contains(m_MousePos) && !m_Dragging,Control.MouseButtons  == MouseButtons.Left)),DecreaseButtonRect);
			if(m_Vertical){
				LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Black,GetTriangleRect(DecreaseButtonRect),Direction.Up);
			}
			else{
				LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Black,GetTriangleRect(DecreaseButtonRect),Direction.Left);
			}	
			g.DrawRectangle(new Pen(m_ViewStyle.GetBorderColor(DecreaseButtonRect.Contains(m_MousePos) && !m_Dragging)),DecreaseButtonRect);
			
			// Draw increase button
			g.FillRectangle(new SolidBrush(m_ViewStyle.GetButtonColor(IncreaseButtonRect.Contains(m_MousePos) && !m_Dragging,Control.MouseButtons  == MouseButtons.Left)),IncreaseButtonRect);			
			if(m_Vertical){
				LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Black,GetTriangleRect(IncreaseButtonRect),Direction.Down);
			}
			else{
				LumiSoft.UI.Controls.Paint.DrawTriangle(g,Color.Black,GetTriangleRect(IncreaseButtonRect),Direction.Right);
			}	
			g.DrawRectangle(new Pen(m_ViewStyle.GetBorderColor(IncreaseButtonRect.Contains(m_MousePos) && !m_Dragging)),IncreaseButtonRect);
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

			if(m_Dragging){
				return;
			}
		
			m_MousePos       = new Point(-1,-1);
			m_HittedObject   = "";
			m_Dragging       = false;
		
			Invalidate();
        }

        #endregion

        #region override method OnMouseDown

        protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if(ScrollPaneRect.Contains(new Point(e.X,e.Y))){
				// See if we may begin dragging
				if(RaiseMayChangePosition()){
					m_Dragging = true;
				}
			}
	
			Invalidate();
        }

        #endregion

        #region override method OnMouseUp

        protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if(m_Dragging){
				this.Position = m_Position;
			}
		
			m_Dragging = false;
								
			Invalidate();
        }

        #endregion

        #region override method OnClick

        protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if(!RaiseMayChangePosition()){
				return;
			}
			
            Point mousePos = this.PointToClient(Control.MousePosition);
			if(this.DecreaseButtonRect.Contains(mousePos) && m_Position > m_Minimum){
				this.Position = (m_Position - 1);
				OnPositionChanged();
			}
			else if(this.IncreaseButtonRect.Contains(mousePos) && m_Position < m_Maximum){
				this.Position = (m_Position + 1);
				OnPositionChanged();
			}
            // Scroll pane clicked.
            else if(m_pScrollPaneRect.Contains(mousePos)){
            }
            // Page up or down.
            else{
                if(m_Vertical){
                    if(mousePos.Y < m_pScrollPaneRect.Top){
                        OnPageUp();
                    }
                    else{
                        OnPageDown();
                    }
                }
                else{
                    if(mousePos.X < m_pScrollPaneRect.Left){
                        OnPageUp();
                    }
                    else{
                        OnPageDown();
                    }
                }
            }           
        }

        #endregion

        #region override method OnMouseMove

        protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			// Begin dragging
			if(!m_Dragging && e.Button == MouseButtons.Left){
				 m_Dragging = true;
			}
		
			// There is active drag operation
			if(m_Dragging){
				int position = m_Position;

				if(m_Vertical){
					int y = (e.Y - m_MousePos.Y);
					if(m_pScrollPaneRect.Y + y >= DecreaseButtonRect.Bottom + 1 && m_pScrollPaneRect.Bottom + y + 1 <= IncreaseButtonRect.Y){
						m_pScrollPaneRect = new Rectangle(m_pScrollPaneRect.X,m_pScrollPaneRect.Y + y,m_pScrollPaneRect.Width,m_pScrollPaneRect.Height);
						
						if(m_pScrollPaneRect.Bottom >= this.Height - 19){
							m_Position = this.Maximum;
						}
						else{
							int scrollArea = Bounds.Height - ScrollPaneRect.Height - 34;
							float pos = (float)(m_pScrollPaneRect.Y - 17) / scrollArea;
							m_Position = (int)(this.Maximum * pos);
						}
						
						if(position != m_Position){
							OnPositionChanged();
						}
						
						this.Invalidate();
					}
				}
				else{
					int x = (e.X - m_MousePos.X);			
					if(m_pScrollPaneRect.X + x >= DecreaseButtonRect.Right + 1 && m_pScrollPaneRect.Right + x + 1 <= IncreaseButtonRect.X){
						m_pScrollPaneRect = new Rectangle(m_pScrollPaneRect.X + x,m_pScrollPaneRect.Y,m_pScrollPaneRect.Width,m_pScrollPaneRect.Height);
						
						if(m_pScrollPaneRect.Right >= this.Width - 19){
							m_Position = this.Maximum;
						}
						else{
							int scrollArea = Bounds.Width - ScrollPaneRect.Width - 34;
							float pos = (float)(m_pScrollPaneRect.X - 17) / (float)scrollArea;
							m_Position = (int)(this.Maximum * pos);
						}
						
						if(position != m_Position){
							OnPositionChanged();
						}
					
						this.Invalidate();
					}
				}				
			}	
			// Handle normal mouse move
			else{
				m_MousePos = new Point(e.X,e.Y);

				String hittedObject = "";
				if(this.DecreaseButtonRect.Contains(m_MousePos)){
					hittedObject = "DecreaseButton";
				}
				else if(this.IncreaseButtonRect.Contains(m_MousePos)){
					hittedObject = "IncreaseButton";
				}
				else if(ScrollPaneRect.Contains(m_MousePos)){
					hittedObject = "ScrollPane";
				}
				
				if(!m_HittedObject.Equals(hittedObject)){
					m_HittedObject = hittedObject;
					
					this.Invalidate();
				}
			}

			m_MousePos = new Point(e.X,e.Y);
        }

        #endregion

        #region method getTriangleRect

        private Rectangle getTriangleRect(Rectangle buttonRect)
		{
			if(m_Vertical){
				return new Rectangle(buttonRect.X + 4,buttonRect.Y + 7,buttonRect.Width - 7,buttonRect.Height - 12);
			}
			else{
				return new Rectangle(buttonRect.X + 7,buttonRect.Y + 4,buttonRect.Width - 12,buttonRect.Height - 7);
			}
        }

        #endregion

        #region method GetScrollPaneX

        /// <summary>
		/// Gets scroll pane start X position.
		/// </summary>
		/// <returns></returns>
		private int GetScrollPaneX()
		{
			if(m_Vertical){
				return 0;	
			}
			
			int x = 18;
			if(m_Position != 0){
				if(m_Position < this.Maximum){
					x += Convert.ToInt32(((this.Width - 36 - GetScrollPaneWidth()) / (float)this.Maximum) * (float)m_Position);
				}
				else{
					x = this.Width - 18 - GetScrollPaneWidth() - 1;
				}
			}
			
			return x;
        }

        #endregion

        #region method GetScrollPaneY

        /// <summary>
		/// Gets scroll pane start Y position.
		/// </summary>
		/// <returns></returns>
		private int GetScrollPaneY()
		{
			if(!m_Vertical){
				return 0;			
			}
			
			int y = 18;
			if(this.Maximum != 0){
				if(m_Position < this.Maximum){
					y += Convert.ToInt32(((this.Height - 36 - GetScrollPaneHeight()) / (float)this.Maximum) * (float)m_Position);
				}
				else{
					y = this.Height - 18 - GetScrollPaneHeight() - 1;
				}
			}
			
			return y;
        }

        #endregion

        #region method GetScrollPaneWidth

        /// <summary>
		/// Gets scroll pane width.
		/// </summary>
		/// <returns></returns>
		private int GetScrollPaneWidth()
		{
			if(m_Vertical){
				return Bounds.Width - 1;			
			}
			if(this.Maximum == 0){
				return this.Width - 34;
			}
			
			int width = (this.Width - 34) / this.Maximum;
			if(width < 12){
				width = 12;
			}
			
			return width;
        }

        #endregion

        #region method GetScrollPaneHeight

        /// <summary>
		/// Gets scroll pane height.
		/// </summary>
		/// <returns></returns>
		private int GetScrollPaneHeight()
		{
			if(!m_Vertical){
				return Bounds.Height - 1;			
			}
			if(this.Maximum == 0){
				return this.Height - 34;
			}
			
			int height = (this.Height - 34) / this.Maximum;
			if(height < 12){
				height = 12;
			}
			
			return height;
        }

        #endregion

        #region method GetTriangleRect

        private Rectangle GetTriangleRect(Rectangle buttonRect)
		{
			if(m_Vertical){
				return new Rectangle(buttonRect.X + 4,buttonRect.Y + 7,buttonRect.Width - 7,buttonRect.Height - 12);
			}
			else{
				return new Rectangle(buttonRect.X + 7,buttonRect.Y + 4,buttonRect.Width - 12,buttonRect.Height - 7);
			}
        }

        #endregion


        #region Properties Implementation

        private Rectangle ScrollPaneRect
		{
			get{ return m_pScrollPaneRect; }
		}
		
		private Rectangle DecreaseButtonRect
		{
			get{
				if(m_Vertical){
					return new Rectangle(0,0,this.Width - 1,17);
				}
				else{
					return new Rectangle(0,0,this.Height,this.Height - 1);			
				}
			}
		}
		
		private Rectangle IncreaseButtonRect
		{
			get{
				if(m_Vertical){
					return new Rectangle(0,this.Height - 18,this.Width - 1,17);
				}
				else{
					return new Rectangle(this.Width - 18,0,this.Height,this.Height - 1);
				}
			}
		}
	
		
	
		/// <summary>
		/// Gets or sets minimum value.
		/// </summary>
		public int Minimum
		{
			get{ return m_Minimum; }

			set{
				if(m_Minimum != value){
					m_Minimum = value;
				
					Position = 0;
				}
			}
		}
		
		
		/// <summary>
		/// Gets or sets maximum value.
		/// </summary>
		public int Maximum
		{
			get{ return m_Maximum;}

			set{
				if(m_Maximum != value){
					m_Maximum = value;
				
					if(Position > m_Maximum){
						Position = m_Maximum;
					}
					else{
						Position = Position;
					}
				}
			}
		}
		
		/// <summary>
		/// Gets scrollbar position.
		/// </summary>
		public int Position
		{
			get { return m_Position; }

			set{
				if(value <= m_Minimum){
					value = m_Minimum;
				}
				else if(value >= m_Maximum){
					value = m_Maximum;
				}
				m_Position = value;

				m_pScrollPaneRect = new Rectangle(GetScrollPaneX(),GetScrollPaneY(),GetScrollPaneWidth(),GetScrollPaneHeight());
				Invalidate();
			}
		}
		
		public bool Vertical
		{
			set{
				if(m_Vertical != value){
					m_Vertical = value;
				}
		   }
		}

		#endregion


		#region Events Implementation

		/// <summary>
		/// Raises PositionChanged event.
		/// </summary>
		protected void OnPositionChanged()
		{
			if(this.PositionChanged != null){
				this.PositionChanged(this,new System.EventArgs());
			}
		}

		/// <summary>
		/// Raises MayChangePosition event.
		/// </summary>
		/// <returns>Returns true if may change position or false if not.</returns>
		protected bool RaiseMayChangePosition()
		{
			return true;
		}

        /// <summary>
        /// Is raised when page up scroll needed.
        /// </summary>
        public event EventHandler PageUp = null;

        /// <summary>
        /// Raises <b>PageUp</b> event.
        /// </summary>
        protected void OnPageUp()
        {
            if(this.PageUp != null){
                this.PageUp(this,new EventArgs());
            }
        }

        /// <summary>
        /// Is raised when page down scroll needed.
        /// </summary>
        public event EventHandler PageDown = null;

        /// <summary>
        /// Raises <b>PageDown</b> event.
        /// </summary>
        protected void OnPageDown()
        {
            if(this.PageDown != null){
                this.PageDown(this,new EventArgs());
            }
        }

		#endregion

	}
}
