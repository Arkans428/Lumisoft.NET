using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LumiSoft.UI.res;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements WGrid numeric plus-minus editor.
    /// </summary>
    public class WNumericPlusMinusEditor : WBaseEditor
    {
        private WSpinEdit m_pEdit = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WNumericPlusMinusEditor()
        {
            m_pEdit = new WSpinEdit();
			m_pEdit.Location = new Point(0,0);
            m_pEdit.Dock = DockStyle.Fill;
			m_pEdit.BorderStyle = BorderStyle.None;
            m_pEdit.KeyUp += new KeyEventHandler(m_pEdit_KeyUp);
            m_pEdit.KeyPress += delegate(object sender,KeyPressEventArgs e){
                if(e.KeyChar == '+'){
                    OnPlusClicked();
                    e.Handled = true;
                }
                else if(e.KeyChar == '-'){
                    OnMinusClicked();
                    e.Handled = true;
                }
            };
            m_pEdit.Mask = WEditBox_Mask.Numeric;
            m_pEdit.TextAlign = HorizontalAlignment.Right;
            m_pEdit.UpButtonPressed += delegate(object sender,EventArgs e){
                OnPlusClicked();
            };
            m_pEdit.DownButtonPressed += delegate(object sender,EventArgs e){
                OnMinusClicked();
            };
            m_pEdit.TextChanged += new EventHandler(m_pEdit_TextChanged);
            m_pEdit.UpIcon = ResManager.GetIcon("plus.ico");
            m_pEdit.DownIcon = ResManager.GetIcon("minus.ico");

            this.Controls.Add(m_pEdit);
        }


        #region Events handling

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
			else if(e.KeyCode == Keys.F2){
				OnPlusClicked();
			}
			else if(e.KeyCode == Keys.F3){
				OnMinusClicked();
			}                  
        }

        #endregion

        #region method m_pEdit_TextChanged

        private void m_pEdit_TextChanged(object sender,EventArgs e)
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
			get{ return m_pEdit.DecValue; }

			set{
                try{
                    m_pEdit.DecValue = Convert.ToDecimal(value);
                }
                catch{
                    m_pEdit.DecValue = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets number of decimal places.
        /// </summary>
        public int DecimalPlaces
        {
            get{ return m_pEdit.DecimalPlaces; }

            set{ m_pEdit.DecimalPlaces = value; }
        }

        /// <summary>
        /// Gets or sets text alignment.
        /// </summary>
        public HorizontalAlignment TextAlign
        {
            get{ return m_pEdit.TextAlign; }

            set{
                m_pEdit.TextAlign = value;
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
