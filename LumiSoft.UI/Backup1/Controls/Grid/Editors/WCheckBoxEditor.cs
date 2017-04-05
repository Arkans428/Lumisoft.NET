using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid check box editor.
    /// </summary>
    public class WCheckBoxEditor : WBaseEditor
    {
        private WCheckBox m_pCheckBox = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WCheckBoxEditor()
        {
            m_pCheckBox = new WCheckBox();
			m_pCheckBox.Location = new Point(0,0);
            m_pCheckBox.Dock = DockStyle.Fill;
			m_pCheckBox.BorderStyle = BorderStyle.None;
            m_pCheckBox.BackColor = Color.White;
            m_pCheckBox.HzAlignment = HorizontalAlignment.Center;
            m_pCheckBox.KeyUp += new KeyEventHandler(m_pCheckBox_KeyUp);

			this.Controls.Add(m_pCheckBox);
        }


        #region Events handling

        #region method m_pCheckBox_KeyUp

        private void m_pCheckBox_KeyUp(object sender,KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter){
			}
            else if(e.KeyCode == Keys.Up){
                m_pGridView.Process_keyPressed(e);
            }
            else if(e.KeyCode == Keys.Down){
                m_pGridView.Process_keyPressed(e);
            }
			else if(e.Control && e.KeyCode == Keys.Delete){
				m_pGridView.Process_keyPressed(e);
			}
			else if(e.KeyCode == Keys.Escape){
				Undo();
			}
        }

        #endregion

        #endregion


        #region method StartEdit

        /// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="value"></param>
		internal protected override void StartEdit(WGridTableView view,object value)
		{
			m_pCheckBox.Focus();

			base.StartEdit(view,value);
        }

        #endregion

        #region Properties implementation

        /// <summary>
        /// Gets if value is modified.
        /// </summary>
        public override bool IsModified
        {
            get{
                bool value = false;
                try{
                    value = Convert.ToBoolean(this.Value);
                }
                catch{                    
                }

                if(value != (bool)this.EditValue){
                    return true;
                }
                else{
                    return false;
                }
            }
        }

        /// <summary>
		/// Gets or sets value what is currently visible in editor.
		/// </summary>
		public override object EditValue
		{
			get{ return m_pCheckBox.Checked; }

			set{
                try{
                    m_pCheckBox.Checked = Convert.ToBoolean(value);
                }
                catch{
                    m_pCheckBox.Checked = false;
                }
            }
        }

        #endregion
    }
}
