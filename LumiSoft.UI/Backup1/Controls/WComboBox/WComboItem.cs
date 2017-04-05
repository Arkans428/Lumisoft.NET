using System;
using System.Collections;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// ComboBox Item.
	/// </summary>
	public class WComboItem
	{
		private string m_Text = "";
		private object m_Tag  = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">Item text.</param>
		public WComboItem(string text)
		{
			m_Text = text;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text">Item text.</param>
		/// <param name="tag">Item tag.</param>
		public WComboItem(string text,object tag) : this(text)
		{
			m_Tag = tag;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return m_Text;
		}
        
		#region Properties Implementation

		/// <summary>
		/// Gets or sets item text.
		/// </summary>
		public string Text
		{
			get{ return m_Text; }

			set{ m_Text = value; }
		}

		/// <summary>
		/// Gets or sets item tag.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }
			
			set{ m_Tag = value; }
		}

		#endregion

	}

	/// <summary>
	/// Conmbobox item collection.
	/// </summary>
	public class WComboItems : ArrayList
	{
		private WComboBox m_WComboBox = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		public WComboItems(WComboBox parent) : base()
		{
			m_WComboBox = parent;
		}


		/// <summary>
		/// Adds new item to combobox collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(WComboItem item)
		{
			return base.Add(item);
		}

		/// <summary>
		/// Adds new item to combobox collection.
		/// </summary>
		/// <param name="text">Item text.</param>
		/// <returns></returns>
		public int Add(string text)
		{
			return Add(text,null);
		}

		/// <summary>
		/// Adds new item to combobox collection.
		/// </summary>
		/// <param name="text">Item text.</param>
		/// <param name="tag">Item tag.</param>
		/// <returns></returns>
		public int Add(string text,object tag)
		{
			return base.Add(new WComboItem(text,tag));
		}


		/// <summary>
		/// Returns item from specified index.
		/// </summary>
		public new WComboItem this[int nIndex]
		{
			get{ 				
				return (WComboItem)base[nIndex];
			}
		}
		
		/// <summary>
		/// Clears all items.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			m_WComboBox.Text = "";
		}

		/// <summary>
		/// Gives first item index which text is equal.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public int IndexOf(string text)
		{
			int counter = 0;
			foreach(WComboItem item in this){
				if(item.Text == text){
					return counter;
				}

				counter++;
			}

			return -1;
		}

		/// <summary>
		/// Checks if item with specified text exists.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public bool ContainsText(string text)
		{
			foreach(WComboItem item in this){
				if(item.Text == text){
					return true;
				}
			}

			return false;
		}
	}
}
