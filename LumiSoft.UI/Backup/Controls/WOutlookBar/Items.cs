using System;
using System.Collections;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// Bar items collection.
	/// </summary>
	public class Items : ArrayList
	{		
		private Bar m_pBar = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ownerBar"></param>
		public Items(Bar ownerBar) : base()
		{
			m_pBar = ownerBar;
		}


		#region function Add

		/// <summary>
		/// 
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="imageIndex"></param>
		/// <returns></returns>
		public Item Add(string caption,int imageIndex)
		{
			Item item = new Item(this);
			item.Caption = caption;
			item.ImageIndex = imageIndex;

			base.Add(item);
			this.Bar.Bars.WOutlookBar.UpdateAll();

			return item;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public new Item this[int nIndex]
		{
			get{ return (Item)base[nIndex]; }
		}
	

		#region Properties Implementation

		#region Internal Properties

		internal Bar Bar
		{
			get{ return m_pBar; }
		}

		#endregion

		#endregion

	}
}
