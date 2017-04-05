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
			m_pTextBox.BorderStyle = BorderStyle.None;
			m_pTextBox.AutoSize = false;

			this.Controls.Add(m_pTextBox);
        }


        #region method OnResize

        protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			m_pTextBox.Size = new Size(this.Width,this.Height);
        }

        #endregion
        
        #region method OnKeyUp

        protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyDown (e);

			if(e.KeyCode == Keys.Enter){
				m_pGridView.MoveNext();
			}
			else if(e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control){
				m_pGridView.DeleteActiveRow();
			}
			else if(e.KeyCode == Keys.Escape){
				Undo();
			}
        }

        #endregion

        #region method ProcessDialogKey

        protected override bool ProcessDialogKey(Keys keyData)
		{
			if(keyData == Keys.Enter){
				OnKeyUp(new KeyEventArgs(keyData));
				return true;
			}
			else if(keyData == Keys.Escape){
				OnKeyUp(new KeyEventArgs(keyData));
				return true;
			}			

			return false;
        }

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
