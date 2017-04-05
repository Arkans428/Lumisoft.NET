using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void DateSelectionChangedHandler(object sender,WDateChanged_EventArgs e);

	#region public class WDateChanged_EventArgs

	/// <summary>
	/// 
	/// </summary>
	public class WDateChanged_EventArgs
	{
		private DateTime m_SelectedDate;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedDate"></param>
		public WDateChanged_EventArgs(DateTime selectedDate)
		{
			m_SelectedDate = selectedDate;
		}

		#region Properties Implementation
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime Date
		{
			get{ return m_SelectedDate; }
		}

		#endregion

	}

	#endregion

	/// <summary>
	/// Summary description for WDatePickerPopUp.
	/// </summary>
	public class WDatePickerPopUp : LumiSoft.UI.Controls.WPopUpFormBase
	{
		private WMonthCalendar monthCalendar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public event DateSelectionChangedHandler SelectionChanged = null;

		private ViewStyle m_pViewStyle = null;

		/// <summary>
		/// 
		/// </summary>
		public WDatePickerPopUp(Control parent,ViewStyle viewStyle,DateTime date) : base(parent)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

            if(date == DateTime.MinValue){
                date = DateTime.Today;
            }

			m_pViewStyle = viewStyle;
			monthCalendar1.SetDate(date);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.monthCalendar1 = new LumiSoft.UI.Controls.WMonthCalendar();
			this.SuspendLayout();
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(1, 1);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.TabIndex = 0;
			this.monthCalendar1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.monthCalendar1_KeyUp);
			this.monthCalendar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.monthCalendar1_MouseUp);
			// 
			// WDatePickerPopUp
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(194, 157);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.monthCalendar1});
			this.Name = "WDatePickerPopUp";
			this.Text = "WDatePickerPopUp";
			this.VisibleChanged += new System.EventHandler(this.WDatePickerPopUp_VisibleChanged);
			this.ResumeLayout(false);

		}
		#endregion

		
		#region Events handling

		private void monthCalendar1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.MonthCalendar.HitTestInfo hTest = monthCalendar1.HitTest(this.PointToClient(Control.MousePosition));
			System.Windows.Forms.MonthCalendar.HitArea hArea = hTest.HitArea;

			if(hArea == System.Windows.Forms.MonthCalendar.HitArea.Date){
				monthCalendar1.SelectionStart = hTest.Time;
				OnSelectionChanged();

				this.Close();
			}
		}

		private void WDatePickerPopUp_VisibleChanged(object sender, System.EventArgs e)
		{
			if(this.Visible){
				this.Width  = monthCalendar1.Width + 2;
				this.Height = monthCalendar1.Height + 2;
			}
		}

		#region function monthCalendar1_KeyUp

		private void monthCalendar1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Escape){
				this.Close();
			}
		}

		#endregion

		#endregion


		#region function OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle rect = new Rectangle(this.ClientRectangle.Location,new Size(this.ClientRectangle.Width - 1,this.ClientRectangle.Height - 1));
			Pen pen = new Pen(m_pViewStyle.BorderHotColor);

			e.Graphics.DrawRectangle(pen,rect);
		}

		#endregion

		#region function PostMessage

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		public override void PostMessage(ref Message m)
		{
			Message msg = new Message();
			msg.HWnd    = monthCalendar1.Handle;
			msg.LParam  = m.LParam;
			msg.Msg     = m.Msg;
			msg.Result  = m.Result;
			msg.WParam  = m.WParam;

			// Forward message to ListBox
			monthCalendar1.PostMessage(ref msg);
		}

		#endregion


		#region function RaiseSelectionChanged

		/// <summary>
		/// 
		/// </summary>
		public void RaiseSelectionChanged()
		{
			OnSelectionChanged();
		}

		#endregion
		
		#region Events Implementation

		/// <summary>
		/// 
		/// </summary>
		protected void OnSelectionChanged()
		{
			if(this.SelectionChanged != null){
				this.SelectionChanged(this,new WDateChanged_EventArgs(monthCalendar1.SelectionStart));
			}
		}

		#endregion


        #region Properties Implementation

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
