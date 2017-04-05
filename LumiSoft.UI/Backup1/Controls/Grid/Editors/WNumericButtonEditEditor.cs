using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid.Editors
{
    /// <summary>
    /// Implements grid numeric button edit editor.
    /// </summary>
    public class WNumericButtonEditEditor : WBaseEditor
    {
        private WButtonEdit m_pEdit = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WNumericButtonEditEditor()
        {
            m_pEdit = new WButtonEdit();
			m_pEdit.Location = new Point(0,0);
            m_pEdit.Dock = DockStyle.Fill;
			m_pEdit.BorderStyle = BorderStyle.None;
            m_pEdit.Mask = WEditBox_Mask.Numeric;
            m_pEdit.TextAlign = HorizontalAlignment.Right;
            m_pEdit.PlusKeyPressed += new ButtonPressedEventHandler(delegate(object s,EventArgs e){
                OnClicked();
            });
            m_pEdit.KeyUp += new KeyEventHandler(m_pNumeric_KeyUp);
            m_pEdit.ButtonPressed += new ButtonPressedEventHandler(delegate(object s,EventArgs e){
                OnClicked();
            });
            m_pEdit.TextChanged += new EventHandler(m_pEdit_TextChanged);

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
        /// Gets or sets editbox value.
        /// </summary>
        public decimal DecimalValue
        {
            get{ return m_pEdit.DecValue; }

            set{ m_pEdit.DecValue = value; }
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

            set{ m_pEdit.TextAlign = value; }
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
