using System;
using System.Collections.Generic;
using System.Text;
using System.Design;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    /// Language extened tool stip button.
    /// </summary>
    public class WToolStripButton : ToolStripButton
    {
        private WText  m_pWText = null;
        private string m_TextID = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WToolStripButton()
        {
        }
                
        #region Events handling

        #region method m_pWText_LanguageChanged

        private void m_pWText_LanguageChanged(object sender,EventArgs e)
        {
            if(m_pWText != null && !string.IsNullOrEmpty(m_TextID)){
                this.ToolTipText = m_pWText[m_TextID];
            }
            else{
                this.ToolTipText = "";
            }
        }

        #endregion

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

                m_pWText_LanguageChanged(null,null);
            }
        }

        /// <summary>
        /// Gets or stets text ID.
        /// </summary>
        public string TextID
        {
            get{ return m_TextID; }

            set{ 
                m_TextID = value; 

                m_pWText_LanguageChanged(null,null);
            }
        }

        #endregion
    }
}
