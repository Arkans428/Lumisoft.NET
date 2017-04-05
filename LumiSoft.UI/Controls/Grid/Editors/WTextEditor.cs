using System;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
	/// <summary>
	/// Grid text edit control.
	/// </summary>
	public class WTextEditor : WBaseEditor
	{
		public TextBox m_pTextBox = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WTextEditor()
		{
			m_pTextBox = new TextBox();
			m_pTextBox.Location = new Point(0,0);
            m_pTextBox.Dock = DockStyle.Fill;
			m_pTextBox.BorderStyle = BorderStyle.None;
			m_pTextBox.AutoSize = false;
            m_pTextBox.KeyUp += new KeyEventHandler(m_pTextBox_KeyUp);

			this.Controls.Add(m_pTextBox);
        }
                                

        #region Events handling

        #region method m_pTextBox_KeyUp

        private void m_pTextBox_KeyUp(object sender,KeyEventArgs e)
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
			m_pTextBox.Focus();

			base.StartEdit(view,value);
        }

        #endregion

        #region method SelectAll

        /// <summary>
		/// Selects editor value if editor supports it.
		/// </summary>
		public override void SelectAll()
		{
			m_pTextBox.SelectAll();
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets or sets value what is currently visible in editor.
		/// </summary>
		public override object EditValue
		{
			get{ return m_pTextBox.Text; }

			set{ m_pTextBox.Text = value.ToString(); }
        }

        #endregion

    }
}
