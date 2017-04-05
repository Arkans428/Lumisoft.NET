using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{	
	/// <summary>
	/// EditBox control.
	/// </summary>
	public class WEditBox : WFocusedCtrlBase
	{
		private WTextBoxBase m_pTextBox;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components = null;

		#region Events

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler           EnterKeyPressed = null;

		/// <summary>
		/// 
		/// </summary>
		public event WValidate_EventHandler Validate        = null;

		#endregion
		
		private int m_FlasCounter = 0;
	
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WEditBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			m_pTextBox.LostFocus += new System.EventHandler(this.m_pTextBox_OnLostFocus);
			m_pTextBox.GotFocus  += new System.EventHandler(this.m_pTextBox_OnGotFocus);
			m_pTextBox.KeyUp     += new KeyEventHandler(this.m_pTextBox_KeyUp);
			m_pTextBox.KeyPress  += new KeyPressEventHandler(this.m_pTextBox_KeyPress);
			m_pTextBox.KeyDown   += new KeyEventHandler(this.m_pTextBox_KeyDown);

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
			this.m_pTextBox.Size = new System.Drawing.Size(94, 13);
			this.m_pTextBox.TabIndex = 0;
			this.m_pTextBox.Text = "";
			this.m_pTextBox.TextChanged += new System.EventHandler(this.m_pTextBox_TextChanged);
			// 
			// timer1
			// 
			this.timer1.Interval = 150;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// WEditBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_pTextBox});
			this.Name = "WEditBox";
			this.Size = new System.Drawing.Size(100, 20);			
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling
				
		#region function m_pTextBox_OnGotFocus

		private void m_pTextBox_OnGotFocus(object sender, System.EventArgs e)
		{
			this.BackColor = m_ViewStyle.EditFocusedColor;
			this.OnGotFocus(e);
		}

		#endregion

		#region function m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);

		//	OnValidate();

		//	DrawControl(this.ContainsFocus);

			this.OnLostFocus(e);
		}

		#endregion

		#region function m_pTextBox_TextChanged

		private void m_pTextBox_TextChanged(object sender, System.EventArgs e)
		{
			base.OnTextChanged(new System.EventArgs());
		}

		#endregion


		#region fucntion m_pTextBox_KeyUp

		private void m_pTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		#endregion

		#region function m_pTextBox_KeyPress

		private void m_pTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		#endregion

		#region function m_pTextBox_KeyDown

		private void m_pTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		#endregion

		
		#region function timer1_Tick

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
					this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);
					break;
				
				case "EditReadOnlyColor":
					this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);
					break;
			
				case "EditDisabledColor":
					this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);
					break;
			}

			this.Invalidate(false);
		}
	
		#endregion

		#endregion

		
		#region function ProcessDialogKey

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
		{
			System.Windows.Forms.Keys key = keyData;
			if(key == System.Windows.Forms.Keys.Enter){
				this.OnEnterKeyPressed();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		#endregion


		#region override OnEnabledChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

	//		m_pTextBox.Enabled = this.Enabled;

			this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);
		}

		#endregion

		#region override function OnLayout

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLayout(LayoutEventArgs  e)
		{
			base.OnLayout(e);
			if(m_pTextBox == null){
				return;
			}

			if(this.Multiline || !this.AutoSize){
				m_pTextBox.Top    = 2;
				m_pTextBox.Height = this.Size.Height - 4;
				m_pTextBox.Width  = this.Size.Width  - 5;
			}
			else if(this.Size.Height > m_pTextBox.Height + 1){
				int yPos = (this.Size.Height - m_pTextBox.Height) / 2;
				m_pTextBox.Top = yPos;
	
				m_pTextBox.Width  = this.Size.Width  - 6;
			}			
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
					if(this.Size.Equals(new System.Drawing.Size(100, 20))){
						retVal = false;
					}
					break;

				case "ReadOnly":
					if(this.ReadOnly == false){
						retVal = false;
					}
					break;

				case "Multiline":
					if(this.Multiline == false){
						retVal = false;
					}
					break;

				case "PasswordChar":
					if(this.PasswordChar == '\0'){
						retVal = false;
					}
					break;

				case "ScrollBars":
					if(this.ScrollBars == ScrollBars.None){
						retVal = false;
					}
					break;

				case "TextAlign":
					if(this.TextAlign == HorizontalAlignment.Left){
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

				case "WordWrap":
					if(this.WordWrap == true){
						retVal = false;
					}
					break;

				case "DecMaxValue":
					if(this.DecMaxValue == 999999999M){
						retVal = false;
					}
					break;

				case "DecMinValue":
					if(this.DecMinValue == -999999999M){
						retVal = false;
					}
					break;
			}

			return retVal;
		}

		#endregion


		#region Public functions

		#region function FlashControl
		
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
		
		#endregion
		
		#region Properties Implementation
		
		#region Color stuff
		
		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public new Color BackColor
		{
			get{ return base.BackColor; }

			set{
				base.BackColor     = value;
				m_pTextBox.BackColor = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Color ForeColor
		{
			get{ return base.ForeColor; }

			set{
				base.ForeColor     = value;
				m_pTextBox.ForeColor = value;
			}
		}

		#endregion

		
		/// <summary>
		/// Gets or sets mask.
		/// </summary>
		public WEditBox_Mask Mask
		{
			get{ return m_pTextBox.Mask; }

			set{ m_pTextBox.Mask = value; }
		}

		/// <summary>
		/// Gets or sets number of decimal places.
		/// </summary>
		public int DecimalPlaces
		{
			get{ return m_pTextBox.DecimalPlaces; }

			set{ m_pTextBox.DecimalPlaces = value; }
		}

		/// <summary>
		/// Gets or sets maximum text length.
		/// </summary>
		public int MaxLength
		{
			get{ return m_pTextBox.MaxLength; }

			set{ m_pTextBox.MaxLength = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public decimal DecMinValue
		{
			get{ return m_pTextBox.DecMinValue; }

			set{ m_pTextBox.DecMinValue = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public decimal DecMaxValue
		{
			get{ return m_pTextBox.DecMaxValue; }

			set{ m_pTextBox.DecMaxValue = value; }
		}

		/// <summary>
		/// Gest or sets password char.
		/// </summary>
		public char PasswordChar
		{
			get{ return m_pTextBox.PasswordChar; }

			set { m_pTextBox.PasswordChar = value; }
		}

		/// <summary>
		/// Gets or sets text alignment.
		/// </summary>
		public HorizontalAlignment TextAlign
		{
			get{ return m_pTextBox.TextAlign; }

			set{ m_pTextBox.TextAlign = value; }
		}

		/// <summary>
		/// Gets or sets if editbox is multiline.
		/// </summary>
		public bool Multiline
		{
			get{ return m_pTextBox.Multiline; }

			set{ 
				m_pTextBox.Multiline = value; 
				m_pTextBox.AcceptsReturn = true;				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ScrollBars ScrollBars
		{
			get{ return m_pTextBox.ScrollBars; }

			set{ m_pTextBox.ScrollBars = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool WordWrap
		{
			get{ return m_pTextBox.WordWrap; }

			set{ m_pTextBox.WordWrap = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public new bool AutoSize
		{ 
			get{ return m_pTextBox.AutoSize; }

			set{ m_pTextBox.AutoSize = value; }
		}

		/// <summary>
		/// Gets or sets if control is readonly.
		/// </summary>
		public bool ReadOnly
		{
			get{ return m_ReadOnly; }

			set{ 
				m_ReadOnly = value;
				m_pTextBox.ReadOnly = value;

				this.BackColor = m_ViewStyle.GetEditColor(value,this.Enabled,this.ContainsFocus);
			}
		}

		/// <summary>
		/// Gets or sets text.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public override string Text
		{
			get{ return m_pTextBox.Text; }

			set{ m_pTextBox.Text = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Lines
		{
			get{ return m_pTextBox.Lines; }

			set{ m_pTextBox.Lines = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public decimal DecValue
		{
			get{ return m_pTextBox.DecValue; }

			set{ m_pTextBox.DecValue = value; }
		}

		/// <summary>
		/// 
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

		#endregion
	
		#region Events Implementation

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


		#region function OnValidate

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
