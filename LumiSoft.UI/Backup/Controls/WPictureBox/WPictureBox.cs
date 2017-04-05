using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// PictureBox control.
	/// </summary>
	public class WPictureBox : WFocusedCtrlBase
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WPictureBox() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			// Set control type, needed for ViewStyle coloring.
			m_ControlType = ControlType.PictureBox;
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pictureBox1.Location = new System.Drawing.Point(1, 1);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(98, 98);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// WPictureBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pictureBox1});
			this.Name = "WPictureBox";
			this.Size = new System.Drawing.Size(100, 100);			
			this.ResumeLayout(false);

		}
		#endregion

								
		#region Properties Implementation

		/// <summary>
		/// 
		/// </summary>
		public Image Image
		{
			get{ return pictureBox1.Image; }

			set{ 
				pictureBox1.Image = value; 

				if(pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize){
					base.Size = new Size(pictureBox1.Width + 2,pictureBox1.Height + 2);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public PictureBoxSizeMode SizeMode
		{
			get{ return pictureBox1.SizeMode; }

			set{ 
				pictureBox1.SizeMode = value; 

				if(value == PictureBoxSizeMode.AutoSize){
					base.Size = new Size(pictureBox1.Width + 2,pictureBox1.Height + 2);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public new Size Size
		{
			get{ return base.Size; }

			set{
				if(pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize){
					base.Size = new Size(pictureBox1.Width + 2,pictureBox1.Height + 2);
				}
				else{                    
					base.Size = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public new int Width
		{
			get{ return base.Width; }

			set{
				if(pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize){
					base.Width = pictureBox1.Width + 2;
				}
				else{
					base.Width = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public new int Height
		{
			get{ return base.Height; }

			set{
				if(pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize){
					base.Height = pictureBox1.Height + 2;
				}
				else{
					base.Height = value;
				}
			}
		}

		#endregion

	}
}
