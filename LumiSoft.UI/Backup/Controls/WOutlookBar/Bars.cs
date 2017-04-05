using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// OutlookBar Bars collection.
	/// </summary>	
	public class Bars : ArrayList
	{
		private WOutlookBar m_pOutlookBar = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		public Bars(WOutlookBar parent)
		{
			m_pOutlookBar = parent;
		}


		#region function Add

		/// <summary>
		/// 
		/// </summary>
		/// <param name="caption"></param>
		/// <returns></returns>
		public Bar Add(string caption)
		{
			Bar bar = new Bar(this);
			bar.Caption = caption;

			base.Add(bar);
			this.WOutlookBar.UpdateAll();

			return bar;
		}

		#endregion

		#region override Clear

		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			m_pOutlookBar.ActiveBar = null;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public new Bar this[int nIndex]
		{
			get{ return (Bar)base[nIndex]; }
		}


		#region Properties Implementation

		/// <summary>
		/// 
		/// </summary>
		public WOutlookBar WOutlookBar
		{
			get{ return m_pOutlookBar; }
		}

		#endregion

	}
}
