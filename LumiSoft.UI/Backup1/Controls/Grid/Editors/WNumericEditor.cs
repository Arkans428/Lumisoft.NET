using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid numeric editor.
    /// </summary>
    public class WNumericEditor : WBaseEditor
    {
        private WTextBoxBase m_pNumeric = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WNumericEditor()
        {
            m_pNumeric = new WTextBoxBase();
			m_pNumeric.Location = new Point(0,0);
            m_pNumeric.Dock = DockStyle.Fill;
			m_pNumeric.BorderStyle = BorderStyle.None;
			m_pNumeric.AutoSize = false;
            m_pNumeric.Mask = WEditBox_Mask.Numeric;
            m_pNumeric.TextAlign = HorizontalAlignment.Right;
            m_pNumeric.KeyDown += new KeyEventHandler(m_pNumeric_KeyDown);
            m_pNumeric.KeyPress += new KeyPressEventHandler(m_pNumeric_KeyPress);
            m_pNumeric.KeyUp += new KeyEventHandler(m_pNumeric_KeyUp);
            m_pNumeric.TextChanged += new EventHandler(m_pNumeric_TextChanged);

			this.Controls.Add(m_pNumeric);
        }
                                
        #region Events handling

        #region method m_pNumeric_KeyDown

        private void m_pNumeric_KeyDown(object sender,KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        #endregion

        #region method m_pNumeric_KeyPress

        private void m_pNumeric_KeyPress(object sender,KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        #endregion

        #region method m_pNumeric_KeyUp

        private void m_pNumeric_KeyUp(object sender,KeyEventArgs e)
        {
            OnKeyUp(e);

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

        #region method m_pNumeric_TextChanged

        private void m_pNumeric_TextChanged(object sender,EventArgs e)
        {
            OnValueChanged();
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
			m_pNumeric.Focus();

			base.StartEdit(view,value);
        }

        #endregion

        #region method SelectAll

        /// <summary>
		/// Selects editor value if editor supports it.
		/// </summary>
		public override void SelectAll()
		{
            m_pNumeric.SelectAll();
        }

        #endregion


        #region Properties implementation

        /// <summary>
        /// Gets if value is modified.
        /// </summary>
        public override bool IsModified
        {
            get{
                decimal value = 0;
                try{
                    value = Convert.ToDecimal(this.Value);
                }
                catch{
                }
                decimal editValue = 0;
                try{
                    editValue = Convert.ToDecimal(this.EditValue);
                }
                catch{
                }

                if(value != editValue){
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
			get{ return m_pNumeric.DecValue; }

			set{
                try{
                    m_pNumeric.DecValue = Convert.ToDecimal(value);
                }
                catch{
                    m_pNumeric.DecValue = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets number of decimal places.
        /// </summary>
        public int DecimalPlaces
        {
            get{ return m_pNumeric.DecimalPlaces; }

            set{ m_pNumeric.DecimalPlaces = value; }
        }

        /// <summary>
        /// Gets or sets text alignment.
        /// </summary>
        public HorizontalAlignment TextAlign
        {
            get{ return m_pNumeric.TextAlign; }

            set{
                m_pNumeric.TextAlign = value;
            }
        }

        #endregion
    }
}
