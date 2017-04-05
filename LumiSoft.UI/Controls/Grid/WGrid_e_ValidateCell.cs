using System;
using System.Collections.Generic;
using System.Text;

using LumiSoft.UI.Controls.Grid.Editors;

namespace LumiSoft.UI.Controls.Grid
{
    /// <summary>
    /// This method provides data for <b>WGridControl.ValidateCellValue</b> event.
    /// </summary>
    public class WGrid_e_ValidateCell : EventArgs
    {
        private bool        m_IsValidated = true;
        private WBaseEditor m_pEditor     = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="editor">Cell editor.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>editor</b> is null reference.</exception>
        public WGrid_e_ValidateCell(WBaseEditor editor)
        {
            if(editor == null){
                throw new ArgumentNullException("editor");
            }

            m_pEditor = editor;
        }


        #region Properties implementation

        /// <summary>
        /// Gets or sets if cell value is validated.
        /// </summary>
        public bool IsValidated
        {
            get{ return m_IsValidated; }

            set{ m_IsValidated = value; }
        }

        /// <summary>
        /// Gets the cell editor which value is validated.
        /// </summary>
        public WBaseEditor Editor
        {
            get{ return m_pEditor; }
        }

        #endregion
    }
}
