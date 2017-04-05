using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.WTabs
{
	/// <summary>
	/// Tab control.
	/// </summary>
	public class WTab : System.Windows.Forms.UserControl
	{
		private LumiSoft.UI.Controls.WTabs.WTabBar wTabBar1;
		private System.Windows.Forms.Panel panel1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WTab()
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
			if(disposing)
			{
				// Dispose all forms
				foreach(Tab tab in wTabBar1.Tabs){
					if(tab.iTag != null){
						object[] tag = (object[])tab.iTag;
						if(tag[0] != null){
							((Control)tag[0]).Dispose();
						}
					}
				}

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
			this.wTabBar1 = new LumiSoft.UI.Controls.WTabs.WTabBar();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// wTabBar1
			// 
			this.wTabBar1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.wTabBar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.wTabBar1.Name = "wTabBar1";
			this.wTabBar1.SelectedTab = null;
			this.wTabBar1.Size = new System.Drawing.Size(272, 24);
			this.wTabBar1.TabIndex = 0;
			this.wTabBar1.TabChanged += new TabChanged_EventHandler(this.wTabBar1_TabChanged);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(272, 192);
			this.panel1.TabIndex = 1;
			// 
			// WTab
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.wTabBar1});
			this.Name = "WTab";
			this.Size = new System.Drawing.Size(272, 216);
			this.ResumeLayout(false);

		}
		#endregion


		#region function wTabBar1_TabChanged

		private void wTabBar1_TabChanged(object sender,TabChanged_EventArgs e)
		{			
			if(e.OldTab != null && e.OldTab.iTag != null){
				Form oForm = (Form)((object[])(e.OldTab.iTag))[0];
				oForm.Visible = false;
			}

			if(e.NewTab != null && e.NewTab.iTag != null){
				Form nForm = (Form)((object[])(e.NewTab.iTag))[0];
				nForm.Visible = true;
			}			
		}

		#endregion


		#region function AddTab

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabPage"></param>
		/// <param name="tabText"></param>
		/// <returns></returns>
		public Tab AddTab(Form tabPage,string tabText)
		{
			// If handle not created, force
			IntPtr dummyPtr = tabPage.Handle;
	
			tabPage.Top             = 0;
			tabPage.Left            = 0;
			tabPage.TopLevel        = false;
			tabPage.TopMost         = false;
			tabPage.ControlBox      = false;
			tabPage.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			tabPage.StartPosition   = System.Windows.Forms.FormStartPosition.Manual;

			// Set form size
			tabPage.Size = tabPage.ClientSize;
		//	tabPage.Height = this.Height - 4  - wTabBar1.Height;
		//	tabPage.Width  = this.Width - 4;

			// Set Form's parent
			tabPage.Parent = this.panel1;

			Tab tab = wTabBar1.Tabs.Add(tabText);
			tab.iTag = new Object[]{tabPage};

			return tab;
		}

		#endregion

		#region function SelectFirstTab

		/// <summary>
		/// 
		/// </summary>
		public void SelectFirstTab()
		{
			if(this.wTabBar1.Tabs.Count > 0){
				this.wTabBar1.SelectedTab = this.wTabBar1.Tabs[0];
			}
		}

		#endregion


		#region override OnResize

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if(this.SelectedTab != null){
				Tab t = this.SelectedTab;
				Form aForm = (Form)((object[])(t.iTag))[0];
			
				// Set form size
				aForm.Height = this.Height - 4  - wTabBar1.Height;
				aForm.Width  = this.Width - 4;
			}

		}

		#endregion

		
		#region Properties Implementation

		/// <summary>
		/// 
		/// </summary>
		public Tab SelectedTab
		{
			get{ return wTabBar1.SelectedTab; }

			set{ wTabBar1.SelectedTab = value; }
		}

		/// <summary>
		/// Get access to tab bar control.
		/// </summary>
		public WTabBar WTabBar
		{
			get{ return wTabBar1; }
		}

		#endregion

	}
}
