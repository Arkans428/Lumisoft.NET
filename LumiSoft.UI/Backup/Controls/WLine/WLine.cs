using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WLine
{
	/// <summary>
	/// Line control.
	/// </summary>
	public class WLine : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Color m_LineColor = Color.Black;
		private int   m_Angle     = 180;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WLine()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			SetStyle(ControlStyles.ResizeRedraw,true);
	//		SetStyle(ControlStyles.Opaque,true);
			SetStyle(ControlStyles.DoubleBuffer  | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);

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


		#region function OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawLine(new Pen(m_LineColor),0,this.Height/2,this.Width,this.Height/2);
		}

		#endregion


		#region Properties Implementaion

		// LineApperance(3D,Flat),VertAlign

		/// <summary>
		/// Gets or sets line color.
		/// </summary>
		public Color LineColor
		{
			get{ return m_LineColor; }

			set{ m_LineColor = value; }
		}

		/// <summary>
		/// Gets or sets line angle.
		/// </summary>
		public int Angle
		{
			get{ return m_Angle; }

			set{ m_Angle = value; }
		}
		
		#endregion

	}
}
