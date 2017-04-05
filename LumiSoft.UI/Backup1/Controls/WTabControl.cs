using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LumiSoft.UI.Controls.WTabs;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    /// This class implements tab control.
    /// </summary>
    public class WTabControl : UserControl
    {
        private WTabBar m_pTab   = null;
        private WPanel  m_pPanel = null;

        private WText              m_pWText     = null;
        private WTabPageCollection m_pTabs      = null;
        private WTabPage           m_pActiveTab = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WTabControl()
        {
            m_pTabs = new WTabPageCollection(this);
            
            InitUI();
        }

        #region method Dispose

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if(m_pWText != null){
                m_pWText.LanguageChanged -= new EventHandler(m_pWText_LanguageChanged);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region method InitUI

        /// <summary>
        /// Creates and initializes UI.
        /// </summary>
        private void InitUI()
        {
            this.ClientSize = new Size(200,200);

            m_pTab = new WTabBar();
            m_pTab.Size = new Size(200,25);
            m_pTab.Location = new Point(0,0);
            m_pTab.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_pTab.TabChanged += new TabChanged_EventHandler(m_pTab_TabChanged);

            m_pPanel = new WPanel();
            m_pPanel.Size = new Size(200,174);
            m_pPanel.Location = new Point(0,26);
            m_pPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_pPanel.BorderStyle = BorderStyle.FixedSingle;
                
            this.Controls.Add(m_pTab);
            this.Controls.Add(m_pPanel);
        }
                
        #endregion


        #region Events handling

        #region method m_pWText_LanguageChanged

        private void m_pWText_LanguageChanged(object sender,EventArgs e)
        {
            foreach(Tab tab in m_pTab.Tabs){
                tab.Caption = string.IsNullOrEmpty(tab.TextID) ? tab.Caption : WText[tab.TextID];
            }
        }

        #endregion


        #region method m_pTab_TabChanged

        /// <summary>
        /// Is called when active tab has changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void m_pTab_TabChanged(object sender,TabChanged_EventArgs e)
        {
            WTabPage tabPage = (WTabPage)e.NewTab.Tag;
            tabPage.Dock = DockStyle.Fill;
            m_pPanel.Controls.Clear();
            m_pPanel.Controls.Add(tabPage);
            m_pActiveTab = tabPage;

            OnActiveTabChanged();
        }

        #endregion

        #endregion


        #region method SelectFirstTab

        /// <summary>
        /// Selects first tab.
        /// </summary>
        public void SelectFirstTab()
        {
            if(m_pTab.Tabs.Count > 0){
                m_pTab.SelectedTab = m_pTab.Tabs[0];
            }
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets or sets WText.
        /// </summary>
        public WText WText
        {
            get{ return m_pWText; }

            set{
                // Release old WText.
                if(m_pWText != null){
                    m_pWText.LanguageChanged -= new EventHandler(m_pWText_LanguageChanged);
                }

                m_pWText = value; 

                // Attach new WText.
                if(m_pWText != null){
                    m_pWText.LanguageChanged += new EventHandler(m_pWText_LanguageChanged);
                }
            }
        }
                
        /// <summary>
        /// Gets tabs collection.
        /// </summary>
        public WTabPageCollection Items
        {
            get{ return m_pTabs; }
        }

        /// <summary>
        /// Gets currently active tab or null if no active tab.
        /// </summary>
        public WTabPage ActiveTab
        {
            get{ return m_pActiveTab; }
        }


        /// <summary>
        /// Gets tab bar.
        /// </summary>
        internal WTabBar TabBar
        {
            get{ return m_pTab; }
        }

        #endregion

        #region Events implementation
        
        /// <summary>
        /// Is called when active tab has changed.
        /// </summary>
        public event EventHandler ActiveTabChanged = null;

        #region method OnActiveTabChanged

        /// <summary>
        /// Raises <b>ActiveTabChanged</b> event.
        /// </summary>
        private void OnActiveTabChanged()
        {
            if(this.ActiveTabChanged != null){
                this.ActiveTabChanged(this,new EventArgs());
            }
        }

        #endregion

        #endregion
    }
}
