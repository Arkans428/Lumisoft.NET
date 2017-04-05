using System;
using System.Collections;

namespace LumiSoft.UI.Controls.WTabs
{
	/// <summary>
	/// WTabBar Tabs collection.
	/// </summary>
	public class Tabs : ArrayList
	{		
		private WTabBar m_pTabBar = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="owner"></param>
		public Tabs(WTabBar owner) : base()
		{
			m_pTabBar = owner;
		}


		#region function Add

		/// <summary>
		/// Adds new tab to tabs collection.
		/// </summary>
		/// <param name="caption">Tab caption.</param>
		/// <returns></returns>
		public Tab Add(string caption)
		{
			return Add(caption,null,-1);
		}

		/// <summary>
		/// Adds new tab to tabs collection.
		/// </summary>
		/// <param name="caption">Tab caption.</param>
		/// <param name="tag">Tag for tab.</param>
		/// <returns></returns>
		public Tab Add(string caption,object tag)
		{
			return Add(caption,tag,-1);
		}

		/// <summary>
		/// Adds new tab to tabs collection.
		/// </summary>
		/// <param name="caption">Tab caption.</param>
		/// <param name="tag">Tag for tab.</param>
		/// <param name="imageIndex">Tab's imageindex.</param>
		/// <returns></returns>
		public Tab Add(string caption,object tag,int imageIndex)
		{
			Tab tab = new Tab(this);
			tab.Caption = caption;
			tab.Tag = tag;
			tab.ImageIndex = imageIndex;
			
			base.Add(tab);
			m_pTabBar.Invalidate();

			return tab;
		}

		#endregion

		/// <summary>
		/// Gets specified tab.
		/// </summary>
		public new Tab this[int nIndex]
		{
			get{ return (Tab)base[nIndex]; }
		}
	

		#region Properties Implementation

		#region Internal Properties

		internal WTabBar WTabBar
		{
			get{ return m_pTabBar; }
		}

		#endregion

		#endregion

	}
}
