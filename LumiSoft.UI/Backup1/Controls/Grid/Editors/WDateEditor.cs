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
    public class WDateEditor : WBaseEditor
    {
        private WDatePicker.WDatePicker m_pDate = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WDateEditor()
        {
            m_pDate = new WDatePicker.WDatePicker();
			m_pDate.Location = new Point(0,0);
            m_pDate.Dock = DockStyle.Fill;
			m_pDate.BorderStyle = BorderStyle.None;
            m_pDate.KeyUp += new KeyEventHandler(m_pDate_KeyUp);

            this.Controls.Add(m_pDate);
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
			m_pDate.Focus();

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
			get{ return m_pDate.Value; }

			set{
                try{
                    m_pDate.Value = Convert.ToDateTime(value);
                }
                catch{
                    m_pDate.Value = DateTime.MinValue;
                }
            }
        }

        #endregion
    }
}
