using System;

namespace LumiSoft.UI.Controls.WTabs
{
	/// <summary>
	/// Summary description for TabChanged_EventArgs.
	/// </summary>
	public class TabChanged_EventArgs
	{
		private Tab m_pNewTab = null;
		private Tab m_pOldTab = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newTab"></param>
		/// <param name="oldTab"></param>
		public TabChanged_EventArgs(Tab newTab,Tab oldTab)
		{
			m_pNewTab = newTab;
			m_pOldTab = oldTab;
		}


		#region Properties Implementation

		/// <summary>
		/// 
		/// </summary>
		public Tab NewTab
		{
			get{ return m_pNewTab; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Tab OldTab
		{
			get{ return m_pOldTab; }
		}

		#endregion

	}
}
