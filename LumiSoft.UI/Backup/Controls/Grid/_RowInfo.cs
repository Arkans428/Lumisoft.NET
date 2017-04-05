using System;
using System.Data;
using System.Drawing;

namespace LumiSoft.UI.Controls.Grid
{
	/// <summary>
	/// This calss holds grid view row internal info.
	/// </summary>
	internal class _RowInfo
	{
		private WGridTableView m_pOwnerView  = null;
		private int            m_RowHeight   = 0;
		private Rectangle      m_pBounds     = new Rectangle(-1,-1,0,0);
		private DataRowView    m_pRow        = null;
		private WGridTableView m_pDetailView = null;		

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="ownerView">View that owns this row info.</param>
        /// <param name="row">Row what info this row info holds.</param>
		public _RowInfo(WGridTableView ownerView,DataRowView row)
		{
			m_pOwnerView = ownerView;
			m_pRow = row;
        }


        #region method GetValue

        /// <summary>
		/// Gets specified cell value. 
		/// </summary>
		/// <param name="column">Column what value to get.</param>
		/// <returns>Returns specified column value.</returns>
		public object GetValue(WGridColumn column)
		{
			if(column.MappingName.Length > 0){
				if(m_pOwnerView.DataSource.Table.Columns.Contains(column.MappingName)){
					Object value = m_pRow[column.MappingName];
					if(value != null){
						return m_pRow[column.MappingName];
					}
				}
				else{
					throw new Exception("Column '" + column.MappingName + "' doesn't exist !");
				}
			}
		
			return "";
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets row.
		/// </summary>
		public DataRowView RowSource
		{
			get{ return m_pRow; }
		}

		/// <summary>
		/// Gets or sets row height.
		/// </summary>
		public int RowHeight
		{
			get{ return m_RowHeight; }

			set{ m_RowHeight = value; }
		}

		/// <summary>
		/// Gets or sets row bounding rectangle.
		/// </summary>
		public Rectangle Bounds
		{
			get{ return m_pBounds; }

			set{ m_pBounds = value; }
		}

		/// <summary>
		/// Gets row Y position.
		/// </summary>
		public int getY
		{
			get{ return m_pBounds.Y; }
		}

		/// <summary>
		/// Gets or sets row child view.
		/// </summary>
		public WGridTableView DetailView
		{
			get{ return m_pDetailView; }

			set{ m_pDetailView = value; }
        }

        #endregion

    }
}
