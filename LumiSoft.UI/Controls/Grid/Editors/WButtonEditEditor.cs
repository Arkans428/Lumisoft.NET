using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid button-edit editor.
    /// </summary>
    public class WButtonEditEditor : WBaseEditor
    {
        private WButtonEdit m_pEdit = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WButtonEditEditor()
        {
            m_pEdit = new WButtonEdit();
			m_pEdit.Location = new Point(0,0);
            m_pEdit.Dock = DockStyle.Fill;
			m_pEdit.BorderStyle = BorderStyle.None;
            m_pEdit.PlusKeyPressed += new ButtonPressedEventHandler(delegate(object s,EventArgs e){
                OnClicked();
            });
            m_pEdit.KeyUp += new KeyEventHandler(m_pNumeric_KeyUp);
            m_pEdit.ButtonPressed += new ButtonPressedEventHandler(delegate(object s,EventArgs e){
                OnClicked();
            });

            this.Controls.Add(m_pEdit);
        }


        #region Events handling

        #region method m_pNumeric_KeyUp

        private void m_pNumeric_KeyUp(object sender,KeyEventArgs e)
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
        /// Is raised when button clicked.
        /// </summary>
        public event EventHandler ButtonPressed = null;

        #region void OnClicked

        /// <summary>
        /// Raises <b>Clicked</b> event.
        /// </summary>
        private void OnClicked()
        {
            if(this.ButtonPressed != null){
                this.ButtonPressed(this,new EventArgs());
            }
        }

        #endregion

        #endregion
    }
}
