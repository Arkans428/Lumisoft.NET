using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public delegate bool WMessage_EventHandler(object sender, ref Message m);

	/// <summary>
	/// Control type.
	/// </summary>
	internal enum ControlType
	{
		Edit,
		Button,
		Label,
		PictureBox,
	}

	/// <summary>
	/// Focused control(control which may recieve focus) base.
	/// </summary>
	[DesignerSerializer(typeof(WSerializer),typeof(CodeDomSerializer))]
	public class WFocusedCtrlBase : System.Windows.Forms.UserControl,IWSerializer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal ViewStyle   m_ViewStyle          = null; 
		internal bool        m_UseStaticViewStyle = true;		
		internal bool        m_DrawBorder         = true;
		internal bool        m_ReadOnly           = false;
		internal ControlType m_ControlType        = ControlType.Edit;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WFocusedCtrlBase()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			this.SetStyle(ControlStyles.Selectable,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint,true);

			m_ViewStyle = new ViewStyle();

			m_ViewStyle.StyleChanged += new ViewStyleChangedEventHandler(this.OnViewStyleChanged);
			ViewStyle.staticViewStyle.StyleChanged += new ViewStyleChangedEventHandler(this.OnViewStyleChanged);

			UseStaticViewStyle = true;
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
			// WFocusedCtrlBase
			// 
			this.Name = "WFocusedCtrlBase";
			this.Size = new System.Drawing.Size(150, 48);

		}
		#endregion


		#region Events handling
//**
		#region function ChildCtrlLostFocus
				
	//	private void ChildCtrlLostFocus(object sender,System.EventArgs e)
	//	{
	//		if(!this.Focused){
		//		this.OnLostFocus(e);
	//		}
	//	}
				
		#endregion

		#region function ChildCtrlMouseLeave
				
		private void ChildCtrlMouseLeave(object sender,System.EventArgs e)
		{
			this.Refresh();
		}
				
		#endregion

		#region function ChildCtrlMouseEnter

		private void ChildCtrlMouseEnter(object sender,System.EventArgs e)
		{
			this.Refresh();
		}

		#endregion


		#region function OnViewStyleChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnViewStyleChanged(object sender,ViewStyle_EventArgs e)
		{
			if(m_UseStaticViewStyle)
			{
				m_ViewStyle.CopyFrom(ViewStyle.staticViewStyle);
			}

			this.Invalidate(false);
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

			bool allowHot = (this.Enabled && !this.DesignMode) && !(this.IsMouseInControl && Control.MouseButtons == MouseButtons.Left && !this.ContainsFocus);
			bool hot = (this.IsMouseInControl || this.ContainsFocus) && allowHot;
			DrawControl(e.Graphics,hot);		
		}

		#endregion


		#region virtual DrawControl(2)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hot"></param>
		protected virtual void DrawControl(bool hot)
		{
			using(Graphics g = this.CreateGraphics()){
				DrawControl(g,hot);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="hot"></param>
		protected virtual void DrawControl(Graphics g,bool hot)
		{			
			if(m_DrawBorder){
				//----- Draw border around control -------------------------//
				Painter.DrawBorder(g,m_ViewStyle,this.ClientRectangle,this.ContainsFocus || this.IsMouseInControl);
				//-----------------------------------------------------------//
			}
		}

		#endregion
		
		#region virtual OnViewStyleChanged

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnViewStyleChanged(ViewStyle_EventArgs e)
		{			
			this.Invalidate(false);
		}

		#endregion


		#region overrides

		#region override OnControlAdded

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);

			e.Control.MouseEnter += new System.EventHandler(this.ChildCtrlMouseEnter);
			e.Control.MouseLeave += new System.EventHandler(this.ChildCtrlMouseLeave);
	//		e.Control.LostFocus += new System.EventHandler(this.ChildCtrlLostFocus);
		}

		#endregion

		#region function override OnMouseEnter

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		
			this.Refresh();			
		}

		#endregion

		#region function override OnMouseLeave

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		
			this.Refresh();			
		}

		#endregion


		#region function override OnGotFocus

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			if(this.DesignMode){
				return;
			}

			switch(m_ControlType)
			{
				case ControlType.Edit:
					this.BackColor = m_ViewStyle.EditFocusedColor;
					break;

				case ControlType.Button:
					this.BackColor = m_ViewStyle.ButtonHotColor;
					break;
			}

			this.Refresh();
		}

		#endregion

		#region function override OnLostFocus

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

		/*	if(this.DesignMode){
				return;
			}

			switch(m_ControlType)
			{
				case ControlType.Edit:
					this.BackColor = m_ViewStyle.EditColor;
					break;

				case ControlType.Button:
					this.BackColor = m_ViewStyle.ButtonColor;
					break;
			}*/

			this.Refresh();
		}

		#endregion

		#endregion


		#region virtual function ShouldSerialize

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public virtual bool ShouldSerialize(string propertyName)
		{
			return true;
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets if to use static viewStyle.
		/// </summary>
		[		
		Category("View style")
		]
		public virtual bool UseStaticViewStyle
		{
			get{ return m_UseStaticViewStyle; }

			set
			{
				m_UseStaticViewStyle = value; 
				if(value){
					ViewStyle.staticViewStyle.CopyTo(m_ViewStyle);
					m_ViewStyle.ReadOnly = true;
				}
				else{
					m_ViewStyle.ReadOnly = false;
				}
			}
		}

		/// <summary>
		/// Gets controls active viewStyle.
		/// </summary>
		[
		TypeConverter(typeof(ExpandableObjectConverter)),
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Category("View style")
		]
		public virtual ViewStyle ViewStyle
		{
			get{ return m_ViewStyle; }
		}

		/// <summary>
		/// Gets or sets if to draw border around control.
		/// </summary>
		[
		Browsable(false),
		]
		public bool DrawBorder
		{
			get{ return m_DrawBorder; }

			set{ 
				m_DrawBorder = value; 
				
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets if mouse cursor is in control area.
		/// </summary>
		[
		Browsable(false),
		]
		public bool IsMouseInControl
		{
			get{
				if(this.DesignMode){
					return false;
				}

				Point mPos  = Control.MousePosition;
				bool retVal = this.ClientRectangle.Contains(this.PointToClient(mPos));
				return retVal;
			}
		}

		#endregion

	}
}
