using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Button control.
	/// </summary>
	[DefaultEvent("ButtonPressed"),]
	public class WButton : WFocusedCtrlBase
	{		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
		public event ButtonPressedEventHandler ButtonPressed = null;

		private string m_Text   = ""; // Holds property 'Text'.
		private string m_TextID = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
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
			// 
			// WButton
			// 
			this.Name = "WButton";
			this.Size = new System.Drawing.Size(80, 24);

		}
		#endregion


		#region function DrawControl

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="hot"></param>
		protected override void DrawControl(Graphics g,bool hot)
		{	
			DrawControl(g,hot,false);
		}

		private void DrawControl(Graphics g,bool hot,bool pressed)
		{
			g.Clear(m_ViewStyle.GetButtonColor(hot,pressed));
			g.DrawRectangle(new Pen(m_ViewStyle.GetBorderColor(hot),2),this.ClientRectangle);

			StringFormat format  = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment     = StringAlignment.Center;

			Rectangle txtRect = this.ClientRectangle;
			if(pressed){
				txtRect.Location = new Point(txtRect.Left+1,txtRect.Top+1);
			}

			if(this.Enabled){
				g.DrawString(m_Text,this.Font,new SolidBrush(Color.Black),txtRect,format);
			}
			else{
				g.DrawString(m_Text,this.Font,new SolidBrush(Color.FromArgb(128,128,128)),txtRect,format);
			}
		}

		#endregion


		#region override OnMouseDown

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if(this.Enabled && this.Focused){
				using(Graphics g = this.CreateGraphics()){
					DrawControl(g,true,true);
				}
			}
		}

		#endregion

		#region override OnMouseUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if(this.Enabled && this.Focused && this.IsMouseInControl){
				using(Graphics g = this.CreateGraphics()){
					DrawControl(g,this.Focused,false);
				}

				OnButtonClicked();
			}
			else{
				this.Invalidate(false);
			}
		}

		#endregion

		#region override OnKeyDown

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if(this.Enabled && (e.KeyData == Keys.Space || e.KeyData == Keys.Enter)){
				using(Graphics g = this.CreateGraphics()){
					DrawControl(g,true,true);
				}

				// Enter gives immedeate click
				if(e.KeyData == Keys.Enter){
					OnButtonClicked();
				}
			}
		}

		#endregion

		#region override OnKeyUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if(this.Enabled && (e.KeyData == Keys.Space || e.KeyData == Keys.Enter)){				
				using(Graphics g = this.CreateGraphics()){
					DrawControl(g,this.Focused,false);
				}			

				if(e.KeyData == Keys.Space){
					OnButtonClicked();
				}
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
				case "TextID":
					if(this.TextID.Length == 0){
						retVal = false;
					}
					break;

				case "UseStaticViewStyle":
					if(this.UseStaticViewStyle == false){ //**
						retVal = false;
					}
					break;

				case "Size":
					if(this.Size.Equals(new System.Drawing.Size(80, 24))){
						retVal = false;
					}
					break;
			}

			return retVal;
		}

		#endregion
        

		#region Properties Implementation

		/// <summary>
		/// Gets or sets button caption.
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public override string Text
		{
			get{ return m_Text; }

			set{ 
				m_Text = value; 
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets text ID. This is for multilingual support.
		/// For examle at runtime you can search with this textID right caption for item.
		/// </summary>
		public string TextID
		{
			get{ return m_TextID; }

			set{ m_TextID = value;	}
		}

		#endregion

		#region Events Implementation

		#region function OnButtonClicked

		/// <summary>
		/// 
		/// </summary>
		protected void OnButtonClicked()
		{
			if(this.ButtonPressed != null){
				this.ButtonPressed(this,new System.EventArgs());
			}
		}

		#endregion

		#endregion

	}
}
