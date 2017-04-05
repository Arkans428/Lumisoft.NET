using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WImageDropDown
{
	/// <summary>
	/// Summary description for WImageDropDown.
	/// </summary>
	public class WImageDropDown :  WButtonEdit
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private WImageDropDownPopUp m_WImageDropDownPopUp = null;
		private Size  m_DropDownSize;
		private Image m_DropDownImage                     = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WImageDropDown()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			m_pTextBox.LostFocus += new System.EventHandler(this.m_pTextBox_OnLostFocus);

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
			components = new System.ComponentModel.Container();
		}
		#endregion


		#region Events handling

		#region function OnPopUp_Closed

		private void OnPopUp_Closed(object sender,System.EventArgs e)
		{
			m_DroppedDown = false;
			m_WImageDropDownPopUp.Dispose();
			Invalidate(false);

			if(!this.ContainsFocus){
				this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
			}
		}

		#endregion
	
		#region function m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			if(m_DroppedDown && m_WImageDropDownPopUp != null&& !m_WImageDropDownPopUp.ClientRectangle.Contains(m_WImageDropDownPopUp.PointToClient(Control.MousePosition))){
				m_WImageDropDownPopUp.Close();
				m_DroppedDown = false;
			}
		}

		#endregion

		#endregion


		#region override OnButtonPressed
        
		/// <summary>
		/// 
		/// </summary>
		protected override void OnButtonPressed()
		{
			if(m_DroppedDown){
				return;
			}	
		
			ShowPopUp();			
		}

		#endregion


		#region function ShowPopUp

		private void ShowPopUp()
		{
			Point pt = new Point(this.Left,this.Bottom + 1);
			m_WImageDropDownPopUp = new WImageDropDownPopUp(this,m_DropDownImage);
			m_WImageDropDownPopUp.Location = this.Parent.PointToScreen(pt);
	//		m_WImageDropDownPopUp.SelectionChanged += new SelectionChangedHandler(this.OnPopUp_SelectionChanged);
			m_WImageDropDownPopUp.Closed += new System.EventHandler(this.OnPopUp_Closed);
	        m_WImageDropDownPopUp.Show();

			m_WImageDropDownPopUp.m_Start = true;
			m_DroppedDown = true;
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets dropdown prefered size.
		/// </summary>
		public Size DropDownPrefSize
		{
			get{ return m_DropDownSize; }

			set{ m_DropDownSize = value; }
		}

		/// <summary>
		/// Gets or sets dropdown image.
		/// </summary>
		public Image DropDownImage
		{
			get{ return m_DropDownImage; }

			set{m_DropDownImage = value; }
		}

		#endregion

	}
}
