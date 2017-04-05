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


		#region method Add

		/// <summary>
		/// Adds new bar to the collection.
		/// </summary>
		/// <param name="caption">Caption text.</param>
		/// <returns>Returns new bar what was added.</returns>
		public Bar Add(string caption)
		{
			return Add(caption,"");
		}

        /// <summary>
		/// Adds new bar to the collection.
		/// </summary>
		/// <param name="caption">Caption text.</param>
        /// <param name="textID">Text ID.</param>
		/// <returns>Returns new bar what was added.</returns>
		public Bar Add(string caption,string textID)
		{
            Bar bar = new Bar(this);
			bar.Caption = caption;
            bar.TextID = textID;

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
