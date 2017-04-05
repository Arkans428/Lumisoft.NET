using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WImageDropDown
{
	/// <summary>
	/// Summary description for WImageDropDownPopUp.
	/// </summary>
	public class WImageDropDownPopUp : LumiSoft.UI.Controls.WPopUpFormBase
	{
		private LumiSoft.UI.Controls.WPictureBox wPictureBox1;
		private LumiSoft.UI.Controls.WButton m_pLoad;
		private LumiSoft.UI.Controls.WButton m_pClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="img"></param>
		public WImageDropDownPopUp(Control parent,Image img) : base(parent)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

			wPictureBox1.Image = img;

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.wPictureBox1 = new LumiSoft.UI.Controls.WPictureBox();
			this.m_pLoad = new LumiSoft.UI.Controls.WButton();
			this.m_pClose = new LumiSoft.UI.Controls.WButton();
			((System.ComponentModel.ISupportInitialize)(this.m_pLoad)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_pClose)).BeginInit();
			this.SuspendLayout();
			// 
			// wPictureBox1
			// 
			this.wPictureBox1.Image = null;
			this.wPictureBox1.Name = "wPictureBox1";
			this.wPictureBox1.Size = new System.Drawing.Size(216, 144);
			this.wPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.wPictureBox1.TabIndex = 0;
			this.wPictureBox1.UseStaticViewStyle = false;
			this.wPictureBox1.ViewStyle.EditReadOnlyColor = System.Drawing.Color.White;
			// 
			// m_pLoad
			// 
			this.m_pLoad.Location = new System.Drawing.Point(16, 152);
			this.m_pLoad.Name = "m_pLoad";
			this.m_pLoad.Size = new System.Drawing.Size(56, 24);
			this.m_pLoad.TabIndex = 1;
			this.m_pLoad.Text = "Load";
			this.m_pLoad.UseStaticViewStyle = false;
			this.m_pLoad.ButtonPressed += new LumiSoft.UI.Controls.ButtonPressedEventHandler(this.m_pLoad_ButtonPressed);
			// 
			// m_pClose
			// 
			this.m_pClose.Location = new System.Drawing.Point(112, 152);
			this.m_pClose.Name = "m_pClose";
			this.m_pClose.Size = new System.Drawing.Size(48, 24);
			this.m_pClose.TabIndex = 2;
			this.m_pClose.Text = "Close";
			this.m_pClose.UseStaticViewStyle = false;
			this.m_pClose.ButtonPressed += new LumiSoft.UI.Controls.ButtonPressedEventHandler(this.m_pClose_ButtonPressed);
			// 
			// WImageDropDownPopUp
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(216, 184);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_pClose,
																		  this.m_pLoad,
																		  this.wPictureBox1});
			this.Name = "WImageDropDownPopUp";
			this.Text = "WImageDropDownPopUp";
			((System.ComponentModel.ISupportInitialize)(this.m_pLoad)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_pClose)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling

		#region function m_pLoad_ButtonPressed

		private void m_pLoad_ButtonPressed(object sender, System.EventArgs e)
		{
		
		}

		#endregion

		#region function m_pClose_ButtonPressed

		private void m_pClose_ButtonPressed(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion

		#endregion


        #region Properties Implementation

        /// <summary>
        /// Blocks from from activating when it is showed.
        /// </summary>
        protected override bool ShowWithoutActivation 
        {
          get{ return true; }
        }

        #endregion

    }
}
