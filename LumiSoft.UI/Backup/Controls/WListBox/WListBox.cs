using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Summary description for WListBox.
	/// </summary>
	public class WListBox : WFocusedCtrlBase
	{
		private System.Windows.Forms.ListBox listBox1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WListBox()
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.IntegralHeight = false;
			this.listBox1.Location = new System.Drawing.Point(1, 1);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(148, 148);
			this.listBox1.TabIndex = 0;
			// 
			// WListBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listBox1});
			this.Name = "WListBox";
			this.ResumeLayout(false);

		}
		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets items collection.
		/// </summary>
		public ListBox.ObjectCollection Items
		{
			get{ return listBox1.Items; }
		}

		/// <summary>
		/// Gets or sets items height.
		/// </summary>
		public int ItemHeight
		{
			get{ return listBox1.ItemHeight; }

			set{ listBox1.ItemHeight = value; }
		}

		/// <summary>
		/// Gets or sets selected item.
		/// </summary>
		public object SelectedItem
		{
			get{ return listBox1.SelectedItem; }

			set{ listBox1.SelectedItem = value; }
		}

		#endregion

	}
}
