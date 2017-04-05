using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]	
	public enum TextStyle
	{
		/// <summary>
		/// 
		/// </summary>
		Text3D = 0,

		/// <summary>
		/// 
		/// </summary>
		Shadow = 1,
	}

	/// <summary>
	/// Label control.
	/// </summary>
	public class WLabel : WFocusedCtrlBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private HorizontalAlignment m_TextAlignment      = HorizontalAlignment.Center;
		private string              m_Text               = "";
		private string              m_TextID             = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WLabel() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			SetStyle(ControlStyles.Selectable,false);
			m_DrawBorder = false;

			// Set control type, needed for ViewStyle coloring.
			m_ControlType = ControlType.Label;
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
			// WLabel
			// 
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WLabel";
			this.Size = new System.Drawing.Size(150, 24);

		}
		#endregion


		#region function OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			StringFormat format  = new StringFormat();
			format.LineAlignment = StringAlignment.Center;

			if(m_TextAlignment == HorizontalAlignment.Left){
				format.Alignment = StringAlignment.Near;
			}

			if(m_TextAlignment == HorizontalAlignment.Center){
				format.Alignment = StringAlignment.Center;
			}

			if(m_TextAlignment == HorizontalAlignment.Right){
				format.Alignment = StringAlignment.Far;
			}

			Rectangle txtRect = this.ClientRectangle;

	//		if(m_Shadow){
				// Draw Shadow
				Rectangle sRect = new Rectangle(new Point(txtRect.X+2,txtRect.Y+2),txtRect.Size);
				e.Graphics.DrawString(this.Text,this.Font,new SolidBrush(m_ViewStyle.TextShadowColor),sRect,format);
	//		}

	//		if(m_Effect3D){
				// Draw 3d effect
				Rectangle dRect = new Rectangle(new Point(txtRect.X+1,txtRect.Y+1),txtRect.Size);
				e.Graphics.DrawString(this.Text,this.Font,new SolidBrush(m_ViewStyle.Text3DColor),dRect,format);
	//		}

			// Draw normal text
			e.Graphics.DrawString(m_Text,this.Font,new SolidBrush(m_ViewStyle.TextColor),txtRect,format);
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

				case "DrawBorder":
					if(this.DrawBorder == false){
						retVal = false;
					}
					break;

				case "UseStaticViewStyle":
					if(this.UseStaticViewStyle == true){
						retVal = false;
					}
					break;

				case "Font":
					if(this.Font.Equals(new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,((byte)(0))))){
						retVal = false;
					}
					break;

				case "TextAlign":
					if(this.TextAlign == System.Windows.Forms.HorizontalAlignment.Center){
						retVal = false;
					}
					break;

				case "Size":
					if(this.Size.Equals(new System.Drawing.Size(150, 24))){
						retVal = false;
					}
					break;
			}

			return retVal;
		}

		#endregion

		
		#region Properties Implementation
				
		/// <summary>
		/// Gets or sets text.
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
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string TextID
		{
			get{ return m_TextID; }

			set{ m_TextID = value;	}
		}

		/// <summary>
		/// Gets text alignment.
		/// </summary>
		public HorizontalAlignment TextAlign
		{
			get{ return m_TextAlignment; }

			set{
				m_TextAlignment = value;
				this.Invalidate();
			}
		}

		#endregion

	}
}
