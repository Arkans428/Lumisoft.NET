using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid date editor.
    /// </summary>
    public class WTimeEditor : WBaseEditor
    {
        private WTime m_pTime = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WTimeEditor()
        {
            m_pTime = new WTime();
			m_pTime.Location = new Point(0,0);
            m_pTime.Dock = DockStyle.Fill;
			m_pTime.BorderStyle = BorderStyle.None;
            m_pTime.KeyUp += new KeyEventHandler(m_pDate_KeyUp);

            this.Controls.Add(m_pTime);
        }


        #region Events handling

        #region method m_pDate_KeyUp

        private void m_pDate_KeyUp(object sender,KeyEventArgs e)
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
			m_pTime.Focus();

			base.StartEdit(view,value);
        }

        #endregion

        #region method SelectAll

        /// <summary>
		/// Selects editor value if editor supports it.
		/// </summary>
		public override void SelectAll()
		{            
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets if value is modified.
        /// </summary>
        public override bool IsModified
        {
            get{
                DateTime value = DateTime.MinValue;
                try{
                    value = Convert.ToDateTime(this.Value);
                }
                catch{
                }

                if(value != (DateTime)this.EditValue){
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
			get{ return m_pTime.Value; }

			set{
                try{
                    m_pTime.Value = Convert.ToDateTime(value);
                }
                catch{
                    m_pTime.Value = DateTime.MinValue;
                }
            }
        }

        #endregion
    }
}
