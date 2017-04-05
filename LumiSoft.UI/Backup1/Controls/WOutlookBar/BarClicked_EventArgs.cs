using System;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// Holds bar clicke EventArgs.
	/// </summary>
	public class BarClicked_EventArgs
	{
		private Bar m_Bar = null;

		/// <summary>
		/// Defaulr constructor.
		/// </summary>
		/// <param name="clickedBar"></param>
		public BarClicked_EventArgs(Bar clickedBar)
		{	
			m_Bar = clickedBar;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets item which was clicked.
		/// </summary>
		public Bar Bar
		{
			get{ return m_Bar; }
		}

		#endregion
	}
}
