using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI
{
    /// <summary>
    /// Implements Form with ViewStyle and WText support.
    /// </summary>
    public class WForm : Form
    {
        private ViewStyle m_pViewStyle = null;
        private WText     m_pWText     = null;
        private string    m_TextID     = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WForm()
        {
        }

        #region ovveride method Dispose

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">True if to release managed and unmanaged resources, false to release unmanaged respurces only.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(m_pViewStyle != null){
                m_pViewStyle.StyleChanged -= new ViewStyleChangedEventHandler(m_pViewStyle_StyleChanged);
                m_pViewStyle = null;
            }
            if(m_pWText != null){
                m_pWText.LanguageChanged += new EventHandler(m_pWText_LanguageChanged);
                m_pWText = null;
            }
        }

        #endregion


        #region Events Handling

        #region method m_pViewStyle_StyleChanged

        private void m_pViewStyle_StyleChanged(object sender,ViewStyle_EventArgs e)
        {
            if(m_pViewStyle != null){
                this.BackColor = m_pViewStyle.ControlBackColor;
            }
        }

        #endregion

        #region method m_pWText_LanguageChanged

        private void m_pWText_LanguageChanged(object sender,EventArgs e)
        {
            if(m_pWText != null && !string.IsNullOrEmpty(m_TextID)){
                this.Text = m_pWText[m_TextID];
            }
        }

        #endregion

        #endregion


        #region override method OnMouseDoubleClick

        /// <summary>
        /// Raises MouseDoubleClieck event.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if((Control.ModifierKeys & Keys.Alt) != 0 && (Control.ModifierKeys & Keys.Control) != 0 && (Control.ModifierKeys & Keys.Shift) != 0){
                Form design = new Form();
                design.ClientSize = new Size(300,600);

                ComboBox controls = new ComboBox();
                controls.Size = new Size(300,20);
                controls.Location = new Point(0,5);
                
                PropertyGrid grid = new PropertyGrid();
                grid.Size = new Size(300,570);
                grid.Location = new Point(0,30);
                foreach(Control c in this.Controls){
                    controls.Items.Add(new LumiSoft.UI.Controls.WComboItem(c.Name + " (" + c.GetType().Name + ")",c));
                }

                design.Controls.Add(controls);
                design.Controls.Add(grid);

                design.Show();

                controls.SelectedIndexChanged += new EventHandler(delegate(object s,EventArgs e1){
                    grid.SelectedObject = ((LumiSoft.UI.Controls.WComboItem)controls.SelectedItem).Tag;
                });
                controls.SelectedIndex = 0;
            }
        }

        #endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets view style.
        /// </summary>
        public ViewStyle ViewStyle
        {
            get{ return m_pViewStyle; }

            set{
                // Release old handler.
                if(m_pViewStyle != null){
                    m_pViewStyle.StyleChanged -= new ViewStyleChangedEventHandler(m_pViewStyle_StyleChanged);
                }

                m_pViewStyle = value;

                if(m_pViewStyle != null){
                    m_pViewStyle.StyleChanged += new ViewStyleChangedEventHandler(m_pViewStyle_StyleChanged);
                    m_pViewStyle_StyleChanged(null,null);
                }
            }
        }
                
        /// <summary>
        /// Gets WText.
        /// </summary>
        public WText WText
        {
            get{ return m_pWText; }

            set{
                // Release old handler.
                if(m_pWText != null){
                    m_pWText.LanguageChanged -= new EventHandler(m_pWText_LanguageChanged);
                }

                m_pWText = value;

                if(m_pWText != null){
                    m_pWText.LanguageChanged += new EventHandler(m_pWText_LanguageChanged);
                    m_pWText_LanguageChanged(null,null);
                }
            }
        }

        /// <summary>
        /// Gets or sets text ID.
        /// </summary>
        public string TextID
        {
            get{ return m_TextID; }

            set{ 
                m_TextID = value; 

                this.Text = "";
                m_pWText_LanguageChanged(null,null);
            }
        }
        
        #endregion

    }
}
