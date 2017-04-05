using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Specifies edit style.
	/// </summary>
	public enum EditStyle
	{
		/// <summary>
		/// Read only, text can't be changed and button can't be clicked.
		/// </summary>
		ReadOnly = 0,

		/// <summary>
		/// Selectable, text can't be changed, button can be pressed.
		/// </summary>
		Selectable = 1,

		/// <summary>
		/// Editable, text can be changed, button can be pressed.
		/// </summary>
		Editable = 2,
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void ButtonPressedEventHandler(object sender,System.EventArgs e);
    
	/// <summary>
	/// Button edit control(TextBox + Button).
	/// </summary>
	[DefaultEvent("ButtonPressed"),]
	public class WButtonEdit : WFocusedCtrlBase
	{
		/// <summary>
		/// 
		/// </summary>
		protected WTextBoxBase m_pTextBox;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		#region Events

		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
	    public event ButtonPressedEventHandler ButtonPressed   = null;

		/// <summary>
		/// Occurs when the 'Enter' key is pressed.
		/// </summary>
		public event ButtonPressedEventHandler EnterKeyPressed = null;

		/// <summary>
		/// Occurs when the '+' key in numeric pad is pressed.
		/// </summary>
		public event ButtonPressedEventHandler PlusKeyPressed  = null;

		/// <summary>
		/// 
		/// </summary>
		public event WValidate_EventHandler    Validate        = null;

		#endregion

		private int       m_ButtonWidth     = 18;    // Holds 
		private Icon      m_ButtonIcon      = null;
		private bool      m_AcceptsPlussKey = true;		
		private EditStyle m_EditStyle       = EditStyle.Editable;
		private int       m_FlasCounter     = 0;
		
		/// <summary>
		/// 
		/// </summary>
		protected bool m_DroppedDown            = false;
				
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WButtonEdit()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
					
			m_pTextBox.LostFocus   += new System.EventHandler(this.m_pTextBox_OnLostFocus);
			m_pTextBox.GotFocus    += new System.EventHandler(this.m_pTextBox_OnGotFocus);
			m_pTextBox.KeyUp       += new KeyEventHandler(this.m_pTextBox_KeyUp);
			m_pTextBox.KeyPress    += new KeyPressEventHandler(this.m_pTextBox_KeyPress);
			m_pTextBox.KeyDown     += new KeyEventHandler(this.m_pTextBox_KeyDown);
			m_pTextBox.TextChanged += new System.EventHandler(this.m_pTextBox_TextChanged);

			m_ButtonIcon = Core.LoadIcon("down.ico");

			this.BackColor = Color.White;
		}

		#region function Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
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
			this.components = new System.ComponentModel.Container();
			this.m_pTextBox = new LumiSoft.UI.Controls.WTextBoxBase();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// m_pTextBox
			// 
			this.m_pTextBox.Anchor = (System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.m_pTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_pTextBox.DecimalPlaces = 2;
			this.m_pTextBox.DecMaxValue = 999999999;
			this.m_pTextBox.DecMinValue = -999999999;
			this.m_pTextBox.Location = new System.Drawing.Point(3, 2);
			this.m_pTextBox.Mask = LumiSoft.UI.Controls.WEditBox_Mask.Text;
			this.m_pTextBox.Name = "m_pTextBox";
			this.m_pTextBox.Size = new System.Drawing.Size(86, 13);
			this.m_pTextBox.TabIndex = 0;
			this.m_pTextBox.Text = "";
			
			// 
			// timer1
			// 
			this.timer1.Interval = 150;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// WButtonEdit
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_pTextBox});
			this.Name = "WButtonEdit";
			this.Size = new System.Drawing.Size(118, 20);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WButtonEdit_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WButtonEdit_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling
						
		#region method WButtonEdit_MouseMove

		private void WButtonEdit_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.Invalidate(false);
		}

		#endregion


		#region method OnMouseUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{	
			base.OnMouseUp(e);

			if(e.Button == MouseButtons.Left && IsMouseInButtonRect()){
				OnButtonPressed();
			}
		}

		#endregion

		#region method WButtonEdit_MouseDown

		private void WButtonEdit_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(IsMouseInButtonRect()){
				this.Invalidate(false);
			}
		}

		#endregion


		#region method m_pTextBox_OnGotFocus

		private void m_pTextBox_OnGotFocus(object sender, System.EventArgs e)
		{
			this.BackColor = m_ViewStyle.EditFocusedColor;		
		//	this.OnValidate();

			this.OnGotFocus(e);
		}

		#endregion

		#region method m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			if(!m_DroppedDown){
				this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
			}
			
			this.OnLostFocus(e);
		}

		#endregion

		#region method m_pTextBox_TextChanged

		private void m_pTextBox_TextChanged(object sender, System.EventArgs e)
		{
			this.OnTextChanged(new System.EventArgs());
		}

		#endregion


		#region method m_pTextBox_KeyUp

		private void m_pTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		#endregion

		#region method m_pTextBox_KeyPress

		private void m_pTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		#endregion

		#region method m_pTextBox_KeyDown

		private void m_pTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		#endregion

				
		#region method timer1_Tick

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if(m_pTextBox.BackColor == this.BackColor){
				m_pTextBox.BackColor = m_ViewStyle.FlashColor;
			}
			else{
				m_pTextBox.BackColor = this.BackColor;
			}
			
			m_FlasCounter++;

			if(m_FlasCounter > 8){
				m_pTextBox.BackColor = this.BackColor;
				timer1.Enabled = false;
			}
		}

		#endregion


		#region function OnViewStyleChanged
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnViewStyleChanged(ViewStyle_EventArgs e)
		{
			switch(e.PropertyName)
			{
				case "EditColor":
					this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
					break;
				
				case "EditReadOnlyColor":
					this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
					break;
			
				case "EditDisabledColor":
					this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
					break;
			}

			this.Invalidate(false);
		}
	
		#endregion

		#endregion

				
		#region method DrawControl
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="hot"></param>
		protected override void DrawControl(Graphics g,bool hot)
		{
			Rectangle rectButton = GetButtonRect();
			
			//----- Draw border around control -------------------------//
			bool border_hot = hot;
			Painter.DrawBorder(g,m_ViewStyle,this.ClientRectangle,border_hot);
			//-----------------------------------------------------------//

			//----- Draw button ----------------------------------------------------//
			bool btn_hot     = (IsMouseInButtonRect() && hot) || m_DroppedDown;
			bool btn_pressed = IsMouseInButtonRect() && Control.MouseButtons == MouseButtons.Left && hot;

			Painter.DrawButton(g,m_ViewStyle,rectButton,border_hot,btn_hot,btn_pressed);
			//----- End of button drawing ------------------------------------------//
			
			//---- Draw icon --------------------------------------------//
			if(m_ButtonIcon != null){				
				Rectangle rectI  = new Rectangle(rectButton.Left+1,rectButton.Top,rectButton.Width-2,rectButton.Height-2);
				bool      grayed = !this.Enabled || (this.EditStyle == EditStyle.ReadOnly);
				Painter.DrawIcon(g,m_ButtonIcon,rectI,grayed,btn_pressed);
			}
			//-------------------------------------------------------------//			
		}

		#endregion

	
		#region override method ProcessDialogKey

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
		{
            bool retVal = base.ProcessDialogKey(keyData);

			Keys key = keyData;
			if(key == System.Windows.Forms.Keys.Enter){
				this.OnEnterKeyPressed();
                retVal = true;
			}
			else if(key == Keys.Add){
				OnPlusKeyPressed();
                retVal = true;
			}
			else if(keyData == Keys.F2 || keyData == Keys.F3){
				OnPlusKeyPressed();
			}

			return retVal;
		}

		#endregion

			
		#region function IsMouseInButtonRect

		/// <summary>
		/// Checks if mouse is in button part of control.
		/// </summary>
		/// <returns>Returns true if mouse is in button part of control.</returns>
		protected bool IsMouseInButtonRect()
		{
			Rectangle rectButton = GetButtonRect();
			Point mPos = Control.MousePosition;
			if(rectButton.Contains(this.PointToClient(mPos))){
				return true;
			}
			else{
				return false;
			}
		}

		#endregion

		#region fucntion GetButtonRect
	
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Rectangle GetButtonRect()
		{
			Rectangle rectButton = new Rectangle(this.Width - m_ButtonWidth,1,m_ButtonWidth - 1,this.Height - 2);
			return rectButton;
		}

		#endregion


		#region override OnEnabledChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			base.OnEnabledChanged(e);

			this.BackColor = m_ViewStyle.GetEditColor(this.EditStyle != EditStyle.Editable,this.Enabled);
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
					if(this.Size.Equals(new System.Drawing.Size(118, 20))){
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

				case "AcceptsPlusKey":
					if(this.AcceptsPlusKey == true){
						retVal = false;
					}
					break;

				case "ButtonIcon":
					if(this.ButtonIcon == null || Core.CompareIcons(this.ButtonIcon,Core.LoadIcon("down.ico"))){
						retVal = false;
					}
					break;

				case "Mask":
					if(this.Mask == WEditBox_Mask.Text){
						retVal = false;
					}
					break;

				case "MaxLength":
					if(this.MaxLength == 32767){
						retVal = false;
					}
					break;

				case "DecimalPlaces":
					if(this.DecimalPlaces == 2){
						retVal = false;
					}
					break;


				case "BackColor":
					retVal = false;
					break;
			}

			return retVal;
		}

		#endregion
		
		
		#region Public Functions

        #region method SelectAll

        /// <summary>
        /// Selects all text in edit area.
        /// </summary>
        public void SelectAll()
        {
            m_pTextBox.SelectAll();
        }

        #endregion

        /// <summary>
		/// 
		/// </summary>
		public void FlashControl()
		{
			if(!timer1.Enabled){
				m_FlasCounter  = 0;
				timer1.Enabled = true;
			}
		}

		#endregion

		#region Properties Implementation

		#region Color stuff

		/// <summary>
		/// 
		/// </summary>
		public override Color ForeColor
		{
			get{ return base.ForeColor; }

			set{ 
				base.ForeColor       = value;
				m_pTextBox.ForeColor = value;
			}
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		public override Color BackColor
		{
			get{ return base.BackColor; }

			set{
				base.BackColor       = value;
				m_pTextBox.BackColor = value;
				Invalidate(false);
			}
		}

		
		#endregion

				
		/// <summary>
		/// Gets or sets size of control.
		/// </summary>
		public new Size Size
		{
			get{ return base.Size; }

			set{
				if(value.Height > m_pTextBox.Height + 1){					
					base.Size = value;

					int yPos = (value.Height - m_pTextBox.Height) / 2;
					m_pTextBox.Top = yPos;					
				}				
			}
		}

		/// <summary>
		/// Gets or sets button size.
		/// </summary>
		public int ButtonWidth
		{
			get{ return m_ButtonWidth; }

			set{				
				m_ButtonWidth    = value;
				m_pTextBox.Width = this.Width - m_ButtonWidth - m_pTextBox.Left - 3;
				this.Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets button's icon.
		/// </summary>
		public Icon ButtonIcon
		{
			get{ 
				// If default icon, return null - otherwise designer serialises to resx. ???
				if(Core.CompareIcons(m_ButtonIcon,Core.LoadIcon("down.ico"))){
					return null; 
				}
				else{
					return m_ButtonIcon;
				}
			}

			set{ m_ButtonIcon = value; }
		}

		/// <summary>
		/// True, if value is modified.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool IsModified
		{
			get{ return m_pTextBox.Modified; }

			set{ m_pTextBox.Modified = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AcceptsPlusKey
		{
			get{ return m_AcceptsPlussKey; }

			set{ m_AcceptsPlussKey = value; }
		}
        		
		/// <summary>
		/// 
		/// </summary>
		[Obsolete("Use EditStyle instead !!!"),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ReadOnly
		{
			get{ return this.EditStyle == EditStyle.ReadOnly; }

			set{
				if(value){
					this.EditStyle = EditStyle.Selectable;
				}
				else{
					this.EditStyle = EditStyle.Editable;
				}
			}
		}

		/// <summary>
		/// Gets or sets edit style.
		/// </summary>
		public EditStyle EditStyle
		{
			get{ return m_EditStyle; }

			set{ 
				m_EditStyle = value; 

				if(m_EditStyle == EditStyle.ReadOnly || m_EditStyle == EditStyle.Selectable){
					this.BackColor = m_ViewStyle.EditReadOnlyColor;
					m_pTextBox.ReadOnly = true;
				}
				else{
					if(this.ContainsFocus){
						this.BackColor = m_ViewStyle.EditFocusedColor;
					}
					else{
						this.BackColor = m_ViewStyle.EditColor;
					}

					m_pTextBox.ReadOnly = false;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets maximum text length
		/// </summary>
		public int MaxLength
		{
			get{ return m_pTextBox.MaxLength; }

			set{ m_pTextBox.MaxLength = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public override string Text
		{
			get{ return m_pTextBox.Text; }

			set{
				m_pTextBox.Text = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public DateTime DateValue
		{
			get{ return m_pTextBox.DateValue; }

			set{ m_pTextBox.DateValue = value; }
		}

        /// <summary>
		/// 
		/// </summary>
		public decimal DecValue
		{
			get{ return m_pTextBox.DecValue; }

			set{ m_pTextBox.DecValue = value; }
		}
		
		/// <summary>
		/// Gets or sets mask.
		/// </summary>
		public WEditBox_Mask Mask
		{
			get{ return m_pTextBox.Mask; }

			set{
				m_pTextBox.Mask = value;
			}
		}

		/// <summary>
		/// Gets or decimal places.
		/// </summary>
		public int DecimalPlaces
		{
			get{ return m_pTextBox.DecimalPlaces; }

			set{
				m_pTextBox.DecimalPlaces = value;
			}
		}

        /// <summary>
        /// Gets or sets text alignment.
        /// </summary>
        public HorizontalAlignment TextAlign
        {
            get{ return m_pTextBox.TextAlign; }

            set{ m_pTextBox.TextAlign = value; }
        }

		#endregion

		#region Events Implementation

		#region function OnButtonPressed

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnButtonPressed() 
		{
			// Raises the ladu change event; 	
			System.EventArgs oArg = new System.EventArgs();

			if(this.ButtonPressed != null && this.EditStyle != EditStyle.ReadOnly && this.Enabled){
				this.ButtonPressed(this, oArg);
			}
		}

		#endregion

		#region function OnEnterKeyPressed

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnEnterKeyPressed() 
		{
			// Raises the ladu change event; 	
			System.EventArgs oArg = new System.EventArgs();

			if(this.EnterKeyPressed != null){
				this.EnterKeyPressed(this, oArg);
			}
		}

		#endregion

		#region function OnPlusKeyPressed

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnPlusKeyPressed() 
		{				
			System.EventArgs oArg = new System.EventArgs();

			// Raise event
			if(this.PlusKeyPressed != null && this.EditStyle != EditStyle.ReadOnly && this.Enabled){
				this.PlusKeyPressed(this, oArg);
			}			
		}

		#endregion


		#region method OnValidate

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnValidate() 
		{	
			// Raises the Validate change event; 	
			WValidate_EventArgs oArg = new WValidate_EventArgs(this.Name,this.Text);

			if(this.Validate != null){
				this.Validate(this, oArg);
			}
			
			//---- If validation failed ----//
			if(!oArg.IsValid){
				if(oArg.FlashControl){
					this.FlashControl();
				}

				if(!oArg.AllowMoveFocus){
					this.Focus();
				}
			}
			//------------------------------//						
		}

		#endregion
															
		#endregion
				
	}
}
