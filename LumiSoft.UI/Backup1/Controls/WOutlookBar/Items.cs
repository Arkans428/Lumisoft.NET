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


		#region method Add

		/// <summary>
		/// Adds new item to the collection.
		/// </summary>
		/// <param name="caption">Item caption text.</param>
		/// <param name="imageIndex">Item image index.</param>
		/// <returns>Returns new added item.</returns>
		public Item Add(string caption,int imageIndex)
		{
			return Add(caption,"",imageIndex,true,null);
		}

        /// <summary>
		/// Adds new item to the collection.
		/// </summary>
		/// <param name="caption">Item caption text.</param>
        /// <param name="textID">Text ID.</param>
		/// <param name="imageIndex">Item image index.</param>
        /// <param name="enabled">Specifies if item is enabled.</param>
        /// <param name="tag">User data.</param>
		/// <returns>Returns new added item.</returns>
		public Item Add(string caption,string textID,int imageIndex,bool enabled,object tag)
		{
            Item item = new Item(this);
			item.Caption = caption;
            item.TextID = textID;
			item.ImageIndex = imageIndex;
            item.Enabled = enabled;
            item.Tag = tag;

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
