using System;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Drag popup window.
	/// </summary>
	public class WDragWindow : System.Windows.Forms.Form
	{
		private Bitmap  m_pImage = null;
		private Color   m_pColor = Color.Black;
		private object  m_pTag   = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="parent"></param>
		public WDragWindow(Control parent)
		{	
			this.MinimumSize = new Size(1,1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;	
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
					
			if(m_pImage != null){
				e.Graphics.DrawImage(m_pImage,0,0);
			}
			else{				
				e.Graphics.FillRectangle(new SolidBrush(m_pColor),0,0,this.Width,this.Height);
			}
		}

		protected override void WndProc(ref Message m)
		{				
			if(m.Msg == (int)Msgs.WM_MOUSEACTIVATE){
				// Do not allow then mouse down to activate the window, but eat 
				// the message as we still want the mouse down for processing
				m.Result = (IntPtr)MouseActivateFlags.MA_NOACTIVATE;
				return;
			}

			base.WndProc(ref m);			
		}

		protected override CreateParams CreateParams 
		{
			get{
				// Extend the CreateParams property 
				CreateParams cp = base.CreateParams;

				// Update the window Style.
				cp.ExStyle |= (int)WindowExStyles.WS_EX_TOPMOST; 

				return cp;
			}
		}

        #region method Show

		/// <summary>
		/// Shows drag window with specified location,size and image.
		/// </summary>
		/// <param name="x">X position in screen cordinates.</param>
		/// <param name="y">Y position in screen cordinates.</param>
		/// <param name="width">Window width.</param>
		/// <param name="height">Window height.</param>
		/// <param name="image">Image to display in window.</param>
		public void Show(int x,int y,int width,int height,Bitmap image)
		{
			m_pImage = image;
				
			this.Bounds = new Rectangle(x,y,width,height);
		    this.Show();	

			this.Width = width;
        }
                
        /// <summary>
		/// Shows drag window with specified location,size and with specified bacround color.
		/// </summary>
		/// <param name="x">X position in screen cordinates.</param>
		/// <param name="y">Y position in screen cordinates.</param>
		/// <param name="width">Window width.</param>
		/// <param name="height">Window height.</param>
		/// <param name="color">Window backround color.</param>
		public void Show(int x,int y,int width,int height,Color color)
		{
			m_pColor = color;
		
			this.Bounds = new Rectangle(x,y,width,height);
			this.Show();
            this.Width = width;
	
            Timer timer = new Timer();
            timer.Tick += new EventHandler(delegate(object s,EventArgs e){
                if(Control.MouseButtons == MouseButtons.None){
                    if(this.Visible){
                        this.Visible = false;
                    }
                    timer.Enabled = false;
                }
            });	
		    timer.Enabled = true;
        }

        #endregion

        #region method UpdatePosition

        /// <summary>
		/// Updates drag window location.
		/// </summary>
		/// <param name="x">X position in screen cordinates.</param>
		public void UpdatePosition(int x)
		{			
			this.Location = new Point(x,this.Location.Y);
        }
                
        /// <summary>
		/// Updates drag window location.
		/// </summary>
		/// <param name="x">X position in screen cordinates.</param>
		/// <param name="y">Y position in screen cordinates.</param>
		public void UpdatePosition(int x, int y)
		{
			this.Location = new Point(x,y);
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets or sets tag.
		/// </summary>
		public object Tag
		{
			get{ return m_pTag; }

			set{ m_pTag = value; }
        }

        /// <summary>
        /// Blocks from from activating when it is showed.
        /// </summary>
        protected override bool ShowWithoutActivation 
        {
          get{ return true; }
        }

        #endregion

    }
}
