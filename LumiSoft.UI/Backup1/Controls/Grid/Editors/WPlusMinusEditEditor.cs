using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LumiSoft.UI.res;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid plus-minus-edit editor.
    /// </summary>
    public class WPlusMinusEditEditor : WBaseEditor
    {
        private WSpinEdit m_pEdit = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WPlusMinusEditEditor()
        {
            m_pEdit = new WSpinEdit();
			m_pEdit.Location = new Point(0,0);
            m_pEdit.Dock = DockStyle.Fill;
			m_pEdit.BorderStyle = BorderStyle.None;
            m_pEdit.KeyDown += new KeyEventHandler(m_pEdit_KeyDown);
            m_pEdit.KeyUp += new KeyEventHandler(m_pEdit_KeyUp);
            m_pEdit.Mask = WEditBox_Mask.Text;
            m_pEdit.UpButtonPressed += delegate(object sender,EventArgs e){
                OnPlusClicked();
            };
            m_pEdit.DownButtonPressed += delegate(object sender,EventArgs e){
                OnMinusClicked();
            };
            m_pEdit.UpIcon = ResManager.GetIcon("plus.ico");
            m_pEdit.DownIcon = ResManager.GetIcon("minus.ico");

            this.Controls.Add(m_pEdit);
        }
                                
        #region Events handling

        #region method m_pEdit_KeyDown

        private void m_pEdit_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2){
                e.Handled = true;
				OnPlusClicked();
			}
			else if(e.KeyCode == Keys.F3){
                e.Handled = true;
				OnMinusClicked();
			}
            else if(e.KeyCode == Keys.Add){
                e.Handled = true;
                e.SuppressKeyPress = true;
                OnPlusClicked();
            }
            else if(e.KeyCode == Keys.Subtract){
                e.Handled = true;
                e.SuppressKeyPress = true;
                OnMinusClicked();
            }
        }

        #endregion

        #region method m_pEdit_KeyUp

        private void m_pEdit_KeyUp(object sender,KeyEventArgs e)
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
			m_pEdit.Focus();

			base.StartEdit(view,value);
        }

        #endregion

        #region method SelectAll

        /// <summary>
		/// Selects editor value if editor supports it.
		/// </summary>
		public override void SelectAll()
		{
            m_pEdit.SelectAll();
        }

        #endregion


        #region Properties implementation
                
        /// <summary>
		/// Gets or sets value what is currently visible in editor.
		/// </summary>
		public override object EditValue
		{
			get{ return m_pEdit.Text; }

			set{
                try{
                    m_pEdit.Text = value.ToString();
                }
                catch{
                    m_pEdit.Text = "";
                }
            }
        }

        #endregion

        #region Events implementation

        /// <summary>
        /// Is raised when plus(+) key has pressed.
        /// </summary>
        public event EventHandler PlusClicked = null;

        #region method OnPlusClicked

        /// <summary>
        /// Raises <b>PlusClicked</b> event.
        /// </summary>
        private void OnPlusClicked()
        {
            if(this.PlusClicked != null){
                this.PlusClicked(this,new EventArgs());
            }
        }

        #endregion

        /// <summary>
        /// Is raised when minus(-) key has pressed.
        /// </summary>
        public event EventHandler MinusClicked = null;

        #region method OnMinusClicked

        /// <summary>
        /// Raises <b>MinusClicked</b> event.
        /// </summary>
        private void OnMinusClicked()
        {
            if(this.MinusClicked != null){
                this.MinusClicked(this,new EventArgs());
            }
        }

        #endregion

        #endregion
    }
}
