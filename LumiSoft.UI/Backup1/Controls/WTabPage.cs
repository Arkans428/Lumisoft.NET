using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using LumiSoft.UI.Controls.WTabs;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    /// This class implements WTabControl tab page.
    /// </summary>
    public class WTabPage : UserControl
    {
        private string m_Key  = "";
        private Tab    m_pTab = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="key">Tab page key.</param>
        /// <param name="tab">Tab bar tab.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>tab</b> is null reference.</exception>
        internal WTabPage(string key,Tab tab)
        {
            if(tab == null){
                throw new ArgumentNullException("tab");
            }

            //SetStyle(ControlStyles.Selectable,false);

            m_Key  = key;
            m_pTab = tab;
        }


        #region Properties implementation

        /// <summary>
        /// Gets tab page key.
        /// </summary>
        public string Key
        {
            get{ return m_Key; }
        }

        /// <summary>
        /// Gets tab caption text ID.
        /// </summary>
        public string TextID
        {
            get{ return m_pTab.TextID; }
        }

        /// <summary>
        /// Gets tab caption text.
        /// </summary>
        public new string Text
        {
            get{ return m_pTab.Caption; }
        }


        /// <summary>
        /// Gets tab bar tab.
        /// </summary>
        internal Tab Tab
        {
            get{ return m_pTab; }
        }

        #endregion

    }
}
