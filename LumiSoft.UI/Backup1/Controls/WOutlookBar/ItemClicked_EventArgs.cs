using System;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// Holds ItemClicked_EventArgs.
	/// </summary>
	public class ItemClicked_EventArgs
	{
		private Item m_Item = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="item"></param>
		public ItemClicked_EventArgs(Item item)
		{
			m_Item = item;
		}


		#region Properties Implementation

		/// <summary>
		/// Gets item which was clicked.
		/// </summary>
		public Item Item
		{
			get{ return m_Item; }
		}

		#endregion

	}
}
