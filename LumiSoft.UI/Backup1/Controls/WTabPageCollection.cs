using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using LumiSoft.UI.Controls.WTabs;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    /// This class implements WTabControl tab pages collection.
    /// </summary>
    public class WTabPageCollection : IEnumerable
    {
        private WTabControl       m_pTabControl = null;
        private OrderedDictionary m_pItems      = null;

        /// <summary>
        /// Default contructor.
        /// </summary>
        /// <param name="tabControl">Owner tab control.</param>
        internal WTabPageCollection(WTabControl tabControl)
        {
            if(tabControl == null){
                throw new ArgumentNullException("tabControl");
            }

            m_pTabControl = tabControl;

            m_pItems = new OrderedDictionary();
        }


        #region method Add

        /// <summary>
        /// Adds new item to the coollection.
        /// </summary>
        /// <param name="key">Item key.</param>
        /// <param name="textID">Item caption text ID.</param>
        /// <param name="text">Item caption text.</param>
        /// <returns>Returns new created tab page.</returns>
        public WTabPage Add(string key,string textID,string text)
        {
            Tab tab = m_pTabControl.TabBar.Tabs.Add(string.IsNullOrEmpty(textID) || m_pTabControl.WText == null  ? text : m_pTabControl.WText[textID]);
            tab.TextID = textID;
            
            WTabPage tapPage = new WTabPage(key,tab);
            tab.Tag = tapPage;
            m_pItems.Add(key,tapPage);

            return tapPage;
        }

        #endregion

        #region method Remove

        /// <summary>
        /// Removes item at the specified index.
        /// </summary>
        /// <param name="index">Zero-based item index.</param>
        public void Remove(int index)
        {
            Remove((WTabPage)m_pItems[index]);
        }

        /// <summary>
        /// Removes specified tab page from the collection.
        /// </summary>
        /// <param name="tabPage">TabPage to remove.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>tabPage</b> is null reference.</exception>
        public void Remove(WTabPage tabPage)
        {
            if(tabPage == null){
                throw new ArgumentNullException("tapPage");
            }
                        
            m_pItems.Remove(tabPage.Key);
            m_pTabControl.TabBar.Tabs.Remove(tabPage.Tab);
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all itmes from the collection.
        /// </summary>
        public void Clear()
        {
            m_pItems.Clear();
        }

        #endregion


        #region interface IEnumerable

        /// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pItems.GetEnumerator();
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets number of the items in the collection.
        /// </summary>
        public int Count
        {
            get{ return m_pItems.Count; }
        }

        /// <summary>
        /// Gets item at the specified index.
        /// </summary>
        /// <param name="index">Zero-based intem index.</param>
        /// <returns>Returns item at the specified index.</returns>
        public WTabPage this[int index]
        {
            get{ return (WTabPage)m_pItems[index]; }
        }

        /// <summary>
        /// Gets item with the specified key.
        /// </summary>
        /// <param name="key">Item key.</param>
        /// <returns>Returns item with the specified key.</returns>
        public WTabPage this[string key]
        {
            get{ return (WTabPage)m_pItems[key]; }
        }
                                
        #endregion
    }
}
