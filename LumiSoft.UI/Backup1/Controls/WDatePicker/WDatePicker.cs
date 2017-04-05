using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WDatePicker
{
	/// <summary>
	/// DateTime picker control.
	/// </summary>
	[DefaultEvent("DateChanged"),]
	public class WDatePicker : WButtonEdit
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Events

		/// <summary>
		/// Occurs when date has changed.
		/// </summary>
		public event System.EventHandler DateChanged = null;

		#endregion

		private WDatePickerPopUp m_WDatePickerPopUp = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WDatePicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			m_pTextBox.LostFocus += new System.EventHandler(this.m_pTextBox_OnLostFocus);

			this.Mask = WEditBox_Mask.Date;
			this.Value = DateTime.Today;
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
			this.SuspendLayout();
			// 
			// m_pTextBox
			// 
			this.m_pTextBox.Size = new System.Drawing.Size(63, 13);
			this.m_pTextBox.ProccessMessage += new LumiSoft.UI.Controls.WMessage_EventHandler(this.m_pTextBox_ProccessMessage);
			// 
			// WDatePicker
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_pTextBox});
			this.Name = "WDatePicker";
			this.Size = new System.Drawing.Size(88, 20);			
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling

		#region method OnPopUp_SelectionChanged

		private void OnPopUp_SelectionChanged(object sender,WDateChanged_EventArgs e)
		{
			this.Value = e.Date;			
		}

		#endregion

		#region method OnPopUp_Closed

		private void OnPopUp_Closed(object sender,System.EventArgs e)
		{
			m_DroppedDown = false;
			m_WDatePickerPopUp.Dispose();
			Invalidate(false);

			if(!this.ContainsFocus){
				this.BackColor = m_ViewStyle.EditColor;
			}
		}

		#endregion


		#region method m_pTextBox_ProccessMessage

		private bool m_pTextBox_ProccessMessage(object sender,ref System.Windows.Forms.Message m)
		{
			if(m_DroppedDown && m_WDatePickerPopUp != null && IsNeeded(ref m)){
				
				// Forward message to PopUp Form
				m_WDatePickerPopUp.PostMessage(ref m);
				return true;
			}

			return false;
		}

		#endregion

		#region method m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			if(m_DroppedDown && m_WDatePickerPopUp != null && !m_WDatePickerPopUp.ClientRectangle.Contains(m_WDatePickerPopUp.PointToClient(Control.MousePosition))){
				m_WDatePickerPopUp.Close();
				m_DroppedDown = false;
			}
		}

		#endregion

		#endregion


        #region override method ProcessDialogKey

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
		{
            if(m_DroppedDown){
                m_WDatePickerPopUp.RaiseSelectionChanged();
				m_WDatePickerPopUp.Close();

                return true;
            }
            else{
                return base.ProcessDialogKey(keyData);
            }
        }

        #endregion


        #region override OnButtonPressed

        /// <summary>
		/// 
		/// </summary>
		protected override void OnButtonPressed()
		{
			if(this.EditStyle == EditStyle.ReadOnly || !this.Enabled){
				return;
			}
	
			if(m_DroppedDown){
				m_WDatePickerPopUp.Close();
			}
			else{		
				ShowPopUp();
			}		
		}

		#endregion

		#region override OnPlusKeyPressed
        
		/// <summary>
		/// 
		/// </summary>
		protected override void OnPlusKeyPressed()
		{
			if(m_DroppedDown || this.EditStyle == EditStyle.ReadOnly){
				return;
			}	
		
			ShowPopUp();			
		}

		#endregion


		#region function ShouldSerialize
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public override bool ShouldSerialize(string propertyName)
		{
			bool retVal = true;

			switch(propertyName)
			{				
				case "UseStaticViewStyle":
					if(this.UseStaticViewStyle == false){ //***
						retVal = false;
					}
					break;

				case "Size":
					if(this.Size.Equals(new System.Drawing.Size(88, 20))){
						retVal = false;
					}
					break;

				case "ButtonWidth":
					if(this.ButtonWidth == 18){
						retVal = false;
					}
					break;

				case "EditStyle":
					if(this.EditStyle == EditStyle.Editable){
						retVal = false;
					}
					break;

				case "ReadOnly":					
						retVal = false; // Never serialize
					break;

				case "AcceptsPlussKey":
					if(this.AcceptsPlusKey == true){
						retVal = false;
					}
					break;

				case "ButtonIcon":
					if(this.ButtonIcon == null || Core.CompareIcons(this.ButtonIcon,Core.LoadIcon("down.ico"))){
						retVal = false;
					}
					break;

				case "Value":
					if(Value.Equals(DateTime.Today)){
						retVal = false;
					}
					break;

				case "Text":
					retVal = false;
					break;

				case "BackColor":
					retVal = false;
					break;

				case "MaxLength":
					retVal = false;
					break;

				case "Mask":
					retVal = false;
					break;

				case "DecimalPlaces":
					retVal = false;
					break;
			}

			return retVal;
		}

		#endregion


		#region method ShowPopUp

		private void ShowPopUp()
		{
			Point pt = this.Parent.PointToScreen(new Point(this.Left,this.Bottom + 1));
			m_WDatePickerPopUp = new WDatePickerPopUp(this,m_ViewStyle,this.Value);
			m_WDatePickerPopUp.SelectionChanged += new DateSelectionChangedHandler(this.OnPopUp_SelectionChanged);
			m_WDatePickerPopUp.Closed += new System.EventHandler(this.OnPopUp_Closed);
	
			Rectangle screenRect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			if(screenRect.Bottom < pt.Y + m_WDatePickerPopUp.Height){
				pt.Y = pt.Y - m_WDatePickerPopUp.Height - this.Height - 1;
			}

			if(screenRect.Right < pt.X + m_WDatePickerPopUp.Width){
				pt.X = screenRect.Right - m_WDatePickerPopUp.Width - 2;
			}

			m_WDatePickerPopUp.Location = pt;
            m_WDatePickerPopUp.Show();

			m_WDatePickerPopUp.m_Start = true;
			m_DroppedDown = true;
		}

		#endregion

		
		#region method IsNeeded

		private bool IsNeeded(ref  System.Windows.Forms.Message m)
		{
			if(m.Msg == (int)Msgs.WM_MOUSEWHEEL){
				return true;
			}

			if(m.Msg == (int)Msgs.WM_KEYUP || m.Msg == (int)Msgs.WM_KEYDOWN){
				return true;
			}

			if(m.Msg == (int)Msgs.WM_CHAR){
				return true;
			}

			return false;
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets value.
		/// </summary>
		public DateTime Value
		{
			get{ return m_pTextBox.DateValue; }

			set{
				m_pTextBox.DateValue = value;

				OnDateChanged();
			}
		}

        /// <summary>
        /// Gets or sets if empty dates(DateTime.MinValue) are allowed.
        /// </summary>
        public bool AllowEmptyDate
        {
            get{ return m_pTextBox.AllowEmptyDate; }

            set{ m_pTextBox.AllowEmptyDate = value; }
        }

		#endregion

		#region Events Implementation

		/// <summary>
		/// 
		/// </summary>
		protected void OnDateChanged()
		{
			if(this.DateChanged != null){
				this.DateChanged(this,new System.EventArgs());
			}
		}

		#endregion
		
	}
}
