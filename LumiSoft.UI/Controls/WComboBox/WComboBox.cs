using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// ComboBox control.
	/// </summary>
	[DefaultEvent("SelectedIndexChanged"),]
	public class WComboBox : WButtonEdit
	{
		private System.ComponentModel.IContainer components = null;
		
		#region Events

		/// <summary>
		/// Occurs when the selected index changed.
		/// </summary>
		public event System.EventHandler SelectedIndexChanged = null;

		/// <summary>
		/// Occurs when the drop-down portion is shown.
		/// </summary>
		public event System.EventHandler DropDown = null;

		#endregion

		private WComboPopUp m_WComboPopUp   = null;
		private int         m_DropDownWidth = 100;
		private int         m_VisibleItems  = 10;
		private WComboItems m_WComboItems   = null;
		private WComboItem  m_SelectedItem  = null;
		private WComboItem  m_LoadedItem    = null;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WComboBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call	

			m_pTextBox.LostFocus += new System.EventHandler(this.m_pTextBox_OnLostFocus);
		//	m_pTextBox.TextChanged += new System.EventHandler(this.m_pTextBox_TextChanged);

			m_WComboItems = new WComboItems(this);

			m_DropDownWidth = this.Width;
			m_pTextBox.HideSelection = false;
		
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
			this.m_pTextBox.ProccessMessage += new LumiSoft.UI.Controls.WMessage_EventHandler(this.m_pTextBox_ProccessMessage);
			// 
			// WComboBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_pTextBox});
			this.Name = "WComboBox";			
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling

		#region function OnPopUp_SelectionChanged

		private void OnPopUp_SelectionChanged(object sender,WComboSelChanged_EventArgs e)
		{
			this.Text = e.Text;
			m_SelectedItem = e.Item;

			OnSelectedIndexChanged();
		}

		#endregion

		#region function OnPopUp_Closed

		private void OnPopUp_Closed(object sender,System.EventArgs e)
		{
			m_DroppedDown = false;
			m_WComboPopUp.Dispose();
			Invalidate(false);

			if(!this.ContainsFocus){
				this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
			}
		}

		#endregion


		#region function m_pTextBox_TextChanged

	//	private bool m_TextChange = false;
		private void m_pTextBox_TextChanged(object sender, System.EventArgs e)
		{
	//		if(!m_TextChange){
			//	m_TextChange = true;
	//			base.OnTextChanged(new System.EventArgs());
	//		//	m_TextChange = false;
	//		}
		}

		#endregion

		#region function m_pTextBox_ProccessMessage

		private bool m_pTextBox_ProccessMessage(object sender,ref System.Windows.Forms.Message m)
		{
			if(m_DroppedDown && m_WComboPopUp != null && IsNeeded(ref m)){
				// Forward message to PopUp Form
				m_WComboPopUp.PostMessage(ref m);
				return true;
			}

			return false;
		}

		#endregion

		#region function m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			if(m_DroppedDown && m_WComboPopUp != null&& !m_WComboPopUp.ClientRectangle.Contains(m_WComboPopUp.PointToClient(Control.MousePosition))){
				m_WComboPopUp.Close();
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
			if(this.EditStyle == EditStyle.ReadOnly || !this.Enabled){
				return;
			}
	
			if(m_DroppedDown){
				m_WComboPopUp.Close();
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
			if(m_DroppedDown){
				return;
			}	
		
			ShowPopUp();			
		}

		#endregion

		#region override OnSizeChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);

			if(this.DesignMode){
				m_DropDownWidth = this.Width;
			}
		}

		#endregion

		
		#region function ShowPopUp

		private void ShowPopUp()
		{
			Point pt = new Point(this.Left,this.Bottom + 1);
			m_WComboPopUp = new WComboPopUp(this,m_ViewStyle,m_WComboItems.ToArray(),m_VisibleItems,this.Text,m_DropDownWidth);
			m_WComboPopUp.Location = this.Parent.PointToScreen(pt);
			m_WComboPopUp.RightToLeft = this.RightToLeft;
			m_WComboPopUp.SelectionChanged += new SelectionChangedHandler(this.OnPopUp_SelectionChanged);
			m_WComboPopUp.Closed += new System.EventHandler(this.OnPopUp_Closed);
	        m_WComboPopUp.Show();

			m_WComboPopUp.m_Start = true;
			m_DroppedDown = true;

			OnDropDown();
		}

		#endregion


		#region function SelectItemByTag

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		public void SelectItemByTag(object tag)
		{
			if(tag == null){
				return;
			}

			int index = 0;
			foreach(WComboItem it in this.Items){
				if(it.Tag.ToString() == tag.ToString()){
					this.SelectedIndex = index;
				}

				index++;
			}
		}

		#endregion

		#region function SelectItemByText

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		public void SelectItemByText(string text)
		{			
			int index = 0;
			foreach(WComboItem it in this.Items){
				if(it.Text.ToLower() == text.ToLower()){
					this.SelectedIndex = index;
				}

				index++;
			}
		}

		#endregion

		#region function FindString

		/// <summary>
		/// Finds the first item in the ComboBox that starts with the specified string. The search is not case sensitive.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public int FindString(string s)
		{
			return FindString(s,-1);
		}

		/// <summary>
		/// Finds the first item after the given index which starts with the given string. The search is not case sensitive.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
		/// <returns></returns>
		public int FindString(string s,int startIndex)
		{
			if(startIndex == -1){
				startIndex = 0;
			}

			for(int i=startIndex;i<this.Items.Count;i++){
				WComboItem it = this.Items[i];
				if(it.Text.ToLower().StartsWith(s.ToLower())){
					return i;
				}
			}

			return -1;
		}

		#endregion

		#region function FindStringExact

		/// <summary>
		/// Finds the first item in the ComboBox that starts with the specified string. The search is not case sensitive.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public int FindStringExact(string s)
		{
			return FindString(s,-1);
		}

		/// <summary>
		/// Finds the first item after the given index which starts with the given string. The search is not case sensitive.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
		/// <returns></returns>
		public int FindStringExact(string s,int startIndex)
		{
			if(startIndex == -1){
				startIndex = 0;
			}

			for(int i=startIndex;i<this.Items.Count;i++){
				WComboItem it = this.Items[i];
				if(it.Text.ToLower() == s.ToLower()){
					return i;
				}
			}

			return -1;
		}

		#endregion

		
		#region function IsNeeded

		private bool IsNeeded(ref  System.Windows.Forms.Message m)
		{
			if(m.Msg == (int)Msgs.WM_CHAR){				
				return false;
			}

			if(m.Msg == (int)Msgs.WM_MOUSEWHEEL){
				return true;
			}

			if(m.Msg == (int)Msgs.WM_KEYUP || m.Msg == (int)Msgs.WM_KEYDOWN){
			//	MessageBox.Show(m.LParam.ToInt32().ToString() + ":" + m.WParam.ToInt32().ToString());

				Keys key = (Keys)m.WParam.ToInt32();
				if(key == Keys.Enter || key == Keys.Up || key == Keys.Down || key == Keys.PageUp || key == Keys.PageDown){
					return true;
				}
			}			

			return false;
		}

		#endregion

						
		#region Properties Implementation

		/// <summary>
		/// Gets or sets dropdown width.
		/// </summary>
		public int DropDownWidth
		{
			get{ return m_DropDownWidth; }

			set{ m_DropDownWidth = value; }
		}

		/// <summary>
		/// Gets or sets dropdown is shown.
		/// </summary>
		public bool DroppedDown
		{
			get{ return m_DroppedDown; }

			set{ 
				if(value && !m_DroppedDown){
					ShowPopUp();
				}
				else if(m_WComboPopUp != null){
					m_WComboPopUp.Close();
				}
			}
		}

		/// <summary>
		/// Gets or sets visible items count.
		/// </summary>
		public int VisibleItems
		{
			get{ return m_VisibleItems; }

			set{ m_VisibleItems = value; }
		}

		/// <summary>
		/// Gets items collection.
		/// </summary>
		public WComboItems Items
		{
			get{ return m_WComboItems; }
		}

		/// <summary>
		/// Gets selected item.
		/// </summary>
		public WComboItem SelectedItem
		{
			get{ return m_SelectedItem; }
		}

		/// <summary>
		/// Gets selected item's index.
		/// </summary>
		public int SelectedIndex
		{
			get{
				if(this.SelectedItem != null){
					return Items.IndexOf(this.SelectedItem); 
				}
				else{
					return -1;
				}
			}

			set{
				if(value > -1 && value < this.Items.Count){
					m_SelectedItem = this.Items[value];
					m_LoadedItem   = this.Items[value];
					this.Text = m_SelectedItem.Text;

					OnSelectedIndexChanged();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public new bool IsModified
		{
			get{
				if(!m_LoadedItem.Equals(m_SelectedItem) || base.IsModified){
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets or sets selection start.
		/// </summary>
		public int SelectionStart
		{
			get{ return m_pTextBox.SelectionStart; }

			set{ m_pTextBox.SelectionStart = value; }
		}

		/// <summary>
		/// Gets or sets selection length.
		/// </summary>
		public int SelectionLength
		{
			get{ return m_pTextBox.SelectionLength; }

			set{ m_pTextBox.SelectionLength = value; }
		}


	/*	/// <summary>
		/// Gets or sets selection length.
		/// </summary>
		public override string Text
		{
			get{ return m_pTextBox.Text; }

			set{
				if(!m_TextChange){
					m_TextChange = true;
					m_pTextBox.Text = value;
					m_TextChange = false;

			//		base.OnTextChanged(new System.EventArgs());
				}
				else{
					m_pTextBox.Text = value;
				}
			}
		}*/

		#endregion

		#region Events Implementation

		private void OnSelectedIndexChanged()
		{
			if(SelectedIndexChanged != null){
				SelectedIndexChanged(this,new System.EventArgs());
			}
		}

		private void OnDropDown()
		{
			if(this.DropDown != null){
				DropDown(this,new System.EventArgs());
			}
		}

		#endregion

	}
}
