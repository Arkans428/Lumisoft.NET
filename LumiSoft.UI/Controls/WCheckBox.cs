using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Summary description for WCheckBox.
	/// </summary>
	[DefaultEvent("CheckedChanged"),]
	public class WCheckBox : WFocusedCtrlBase
	{
		/// <summary> 
		/// CheckBx control.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Events

		/// <summary>
		/// Occurs when the checked state changed.
		/// </summary>
		public event System.EventHandler CheckedChanged = null;

		#endregion

        private HorizontalAlignment m_HzAlignment = HorizontalAlignment.Left;
		//private LeftRight m_CheckAlign = LeftRight.Left;
		private Icon      m_Icon       = null;
		private bool      m_Checked    = false;
		private bool      m_LoadValue  = false;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WCheckBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

            m_ControlType = ControlType.Label;

			m_Icon = Core.LoadIcon("check.ico");
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
			// WCheckBox
			// 
			this.Name = "WCheckBox";
			this.Size = new System.Drawing.Size(30, 22);			

		}
		#endregion


		#region override function OnKeyUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if(!this.ReadOnly && e.KeyData == Keys.Space){
				if(this.Checked){
					m_Checked = false;
				}
				else{
					m_Checked = true;
				}
				this.Invalidate(false);

				OnCheckedChanged();
			}
		}

		#endregion

		#region override function OnMouseUp

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if(!this.ReadOnly){
				if(this.Checked){
					m_Checked = false;
				}
				else{
					m_Checked = true;
				}
				this.Refresh();

				OnCheckedChanged();
			}
		}

		#endregion


		#region method DrawControl

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="hot"></param>
		protected override void DrawControl(Graphics g,bool hot)
		{
			Pen pen = new Pen(m_ViewStyle.GetBorderColor(hot));
			
			Rectangle checkRect = GetCheckRect();

			if(this.Focused){
				g.FillRectangle(new SolidBrush(m_ViewStyle.EditFocusedColor),checkRect);
			}
			else{
				g.FillRectangle(new SolidBrush(m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled)),checkRect);
			}

			//---- Draw icon --------------------------------------------//
			if(m_Icon != null && m_Checked){

				//------ Adjust Icon sizes and location ----------------------------------//
				Rectangle drawRect = new Rectangle(checkRect.X + 2,checkRect.Y + 2,9,9);
				
				Painter.DrawIcon(g,m_Icon,drawRect,!this.Enabled,false);								
			}

			// Draw rect around control
			g.DrawRectangle(pen,checkRect);
		}		

		#endregion

        
		#region method GetCheckRect

		private Rectangle GetCheckRect()
		{
			Rectangle rect = new Rectangle(0,0,0,0);

            if(m_HzAlignment == HorizontalAlignment.Center){
                rect = new Rectangle((this.Width - 12)/2,(this.Height - 12)/2,12,12);
            }
            else if(m_HzAlignment == HorizontalAlignment.Left){
                rect = new Rectangle(0,(this.Height - 12)/2,12,12);
            }
            else if(m_HzAlignment == HorizontalAlignment.Right){
                rect = new Rectangle((this.Width - 9)/2,(this.Height - 9)/2,12,12);
            }
            					
			return rect;
		}

		#endregion
				
		
		#region Properties Implementation

		/// <summary>
		/// Gets or sets checked state.
		/// </summary>
		public bool Checked
		{
			get{ return m_Checked; }

			set{				
				m_Checked   = value;
				m_LoadValue = value;
				this.Invalidate(false);
				OnCheckedChanged();
			}
		}

		/// <summary>
		/// Gets or sets control to readonly.
		/// </summary>
		public bool ReadOnly
		{
			get{ return m_ReadOnly; }

			set{
				m_ReadOnly = value;
				this.Invalidate(false);
			}
		}

		/// <summary>
		/// Gets if value is modified.
		/// </summary>
		public bool IsModified
		{
			get{ return m_Checked != m_LoadValue; }
		}

        /// <summary>
        /// Gets or sets check box alignment.
        /// </summary>
        public HorizontalAlignment HzAlignment
        {
            get{ return m_HzAlignment; }

            set{ 
                m_HzAlignment = value;
                Invalidate();
            }
        }
		
		#endregion

		#region Events Implementation

		/// <summary>
		/// 
		/// </summary>
		protected void OnCheckedChanged()
		{
			if(this.CheckedChanged != null){
				this.CheckedChanged(this,new System.EventArgs());
			}
		}

		#endregion

	}
}
