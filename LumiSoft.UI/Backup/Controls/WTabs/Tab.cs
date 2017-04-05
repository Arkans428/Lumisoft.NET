using System;
using System.Drawing;

namespace LumiSoft.UI.Controls.WTabs
{
	/// <summary>
	/// WTabBar Tab.
	/// </summary>
	public class Tab
	{
		private string    m_Caption    = "";
		private string    m_TextID     = "";
		private object    m_Tag        = null;
		private object    m_iTag       = null;
		private int       m_ImageIndex = -1;
		private bool      m_Enabled    = true;
	//	private bool      m_Visible    = true;
		private Tabs      m_pTabs      = null;
		private Rectangle m_Bounds;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Tab(Tabs tabs)
		{	
			m_pTabs = tabs;
		}


		#region function OnItemNeedsUpdate

		/// <summary>
		/// 
		/// </summary>
		public void OnItemNeedsUpdate()
		{
			m_pTabs.WTabBar.Invalidate();
		}

		#endregion


		#region Properties Implementation
		
		/// <summary>
		/// Gets or sets item caption.
		/// </summary>
		public string Caption
		{
			get{ return m_Caption; }

			set{ 
				m_Caption = value; 
				OnItemNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets or sets text ID. This is for multilingual support.
		/// For examle at runtime you can search with this textID right caption for item.
		/// </summary>
		public string TextID
		{
			get{ return m_TextID; }

			set{ m_TextID = value; }
		}

		/// <summary>
		/// Gets or sets item tag.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}
		
		/// <summary>
		/// Gets tab index.
		/// </summary>
		public int Index
		{
			get{ return (m_pTabs != null ? m_pTabs.IndexOf(this) : -1); }
		}

		/// <summary>
		/// Gets or sets tab image index.
		/// </summary>
		public int ImageIndex
		{
			get{ return m_ImageIndex; }

			set{ m_ImageIndex = value; }
		}

		/// <summary>
		/// Enables or disables Tab.
		/// </summary>
		public bool Enabled
		{
			get{ return m_Enabled; }

			set{ m_Enabled = value; }
		}
/*
		/// <summary>
		/// Gets owner bar of item.
		/// </summary>
		public Bar Bar
		{
			get{ return m_pItems.Bar; }
		}

*/
		#region Internal Properties
		
		internal object iTag
		{
			get{ return m_iTag; }

			set{ m_iTag = value; }
		}
			
		internal Rectangle Bounds
		{
			get{ return m_Bounds; }

			set{ m_Bounds = value; }
		}

		internal Rectangle ImageRect
		{
			get{ return new Rectangle(this.Bounds.X + 3,this.Bounds.Y + 3,16,16); }
		}

		internal Rectangle TextRect
		{
			get{
				if(this.ImageIndex == -1){
					return this.Bounds;
				}
				else{
					return new Rectangle(this.Bounds.X + 18,this.Bounds.Y,this.Bounds.Width - 18,this.Bounds.Height);
				}
			}
		}
/*
		internal WOutlookBar WOutlookBar
		{
			get{ return m_pItems.Bar.Bars.WOutlookBar; }
		}

		internal ViewStyle ViewStyle
		{
			get{ return m_pItems.Bar.Bars.WOutlookBar.ViewStyle; }
		}
*/
		#endregion

		#endregion

	}
}
