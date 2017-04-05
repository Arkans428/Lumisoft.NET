using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls.Grid
{
	/// <summary>
	/// Grid columns collection.
	/// </summary>
	public class WGridColumns : IEnumerable
	{
		private WGridTableView    m_pView    = null;
		private List<WGridColumn> m_pColumns = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="parentView">View that owns these grid columns.</param>
		internal WGridColumns(WGridTableView parentView)
		{
			m_pView = parentView;
			m_pColumns = new List<WGridColumn>();
        }


        #region method AddColumn

        /// <summary>
		/// Adds new column with specified text to columns collection.
		/// </summary>
		/// <param name="text">Column text.</param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text)
		{
			return AddColumn(text,20);
		}

		/// <summary>
		/// Adds new column with specified text and width to columns collection. 
		/// </summary>
		/// <param name="text">Column text.</param>
		/// <param name="width">Column width.</param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,int width)
		{
			return AddColumn(text,width,"");
		}

		/// <summary>
		/// Adds new column with specified text,width and DataColumn mapping name to columns collection.
		/// </summary>
		/// <param name="text">Column text.</param>
		/// <param name="width">Column width.</param>
		/// <param name="mappingName">Column datasource DataColumn mapping name.</param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,int width,String mappingName)
		{
			return AddColumn(text,width,mappingName,HorizontalAlignment.Left);
		}

		/// <summary>
		/// Adds new column with specified text,width and DataColumn mapping name to columns collection.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textID"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,String textID,int width,String mappingName)
		{
			return AddColumn(text,textID,width,mappingName,HorizontalAlignment.Left);
		}

		/// <summary>
		/// Adds new column with specified text,width,DataColumn mapping name and cells text alignment to columns collection.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <param name="cellTextHzAlign"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,int width,String mappingName,HorizontalAlignment cellTextHzAlign)
		{
			return AddColumn(text,"",width,mappingName,cellTextHzAlign,"");
		}

		/// <summary>
		/// Adds new column with specified text,width,DataColumn mapping name and cells text alignment to columns collection.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textID"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <param name="cellTextHzAlign"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,String textID,int width,String mappingName,HorizontalAlignment cellTextHzAlign)
		{
			return AddColumn(text,textID,width,mappingName,cellTextHzAlign,"");
		}

		/// <summary>
		/// Adds new column with specified text,textID,width,DataColumn mapping name,cells text alignment and cell text format to columns collection.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <param name="cellTextHzAlign"></param>
		/// <param name="cellTextFormat"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,int width,String mappingName,HorizontalAlignment cellTextHzAlign,String cellTextFormat)
		{
			return AddColumn(text,"",width,mappingName,cellTextHzAlign,cellTextFormat);
		}

		/// <summary>
		/// Adds new column with specified text,textID,width,DataColumn mapping name,cells text alignment and cell text format to columns collection.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textID"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <param name="cellTextHzAlign"></param>
		/// <param name="cellTextFormat"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String text,String textID,int width,String mappingName,HorizontalAlignment cellTextHzAlign,String cellTextFormat)
		{
			//---- Find column name which doesn't exist ---------//
			String columnName = "Column " + this.Count + 1;
			bool columnExists = false;
			while(columnExists){
				columnExists = false;
			
				foreach(WGridColumn col in m_pColumns){
					if(col.ColumnName.ToLower().Equals(columnName.ToLower())){
						columnExists = true;
					}
				}
			}
			//----------------------------------------------------//
		
			return AddColumn(columnName,text,textID,width,mappingName,cellTextHzAlign,cellTextFormat);
		}

		/// <summary>
		/// Adds new column with specified name,text,textID,width,DataColumn mapping name,cells text alignment and cell text format to columns collection.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="text"></param>
		/// <param name="textID"></param>
		/// <param name="width"></param>
		/// <param name="mappingName"></param>
		/// <param name="cellTextHzAlign"></param>
		/// <param name="cellTextFormat"></param>
		/// <returns></returns>
		public WGridColumn AddColumn(String name,String text,String textID,int width,String mappingName,HorizontalAlignment cellTextHzAlign,String cellTextFormat)
		{
			// Don't allow duplicate column names
			foreach(WGridColumn col in m_pColumns){
				if(col.ColumnName.ToLower().Equals(name.ToLower())){
					throw new Exception("Column '" + name + "' with specified name already exists in columns collection !");
				}
			}
		
			WGridColumn column = new WGridColumn(this,name);
			column.Text = text;
			column.TextID = textID;
			column.Width = width;
			column.MappingName = mappingName;
			column.CellsTextAlign = cellTextHzAlign;
			column.CellTextFormat = cellTextFormat;
		
			m_pColumns.Add(column);
		
			return column;
        }

        #endregion

        #region method IndexOf

        /// <summary>
		/// Gets specified column index in columns collection. Index is zero based.
		/// </summary>
		/// <param name="column"></param>
		/// <returns>Returns specified column index in columns collection.</returns>
		public int IndexOf(WGridColumn column)
		{
			return m_pColumns.IndexOf(column);
        }

        #endregion

        #region method Remove

        /// <summary>
		/// Removes specified column from columns collection.
		/// </summary>
		/// <param name="column"></param>
		public void Remove(WGridColumn column)
		{
			m_pColumns.Remove(column);
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all grid colums.
        /// </summary>
        public void Clear()
        {
            m_pColumns.Clear();
        }

        #endregion

        #region method MoveColumn

        /// <summary>
		/// Moves column to specified index.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="index"></param>
		public void MoveColumn(WGridColumn column,int index)
		{
			m_pColumns.Remove(column);
			m_pColumns.Insert(index,column);
        }

        #endregion


        #region Properties Implementation

        /// <summary>
		/// Gets columns count in columns collection.
		/// </summary>
		public int Count
		{
			get{ return m_pColumns.Count; }
		}

		/// <summary>
		/// Gets column from specified index. Index is zero based.
		/// </summary>
		public WGridColumn this[int index]
		{
			get{ return m_pColumns[index]; }
		}

		/// <summary>
		/// Gets column with specified column name.
		/// </summary>
		public WGridColumn this[string name]
		{
			get{
				foreach(WGridColumn col in m_pColumns){                    
					if(col.MappingName.ToLower() == name.ToLower()){
						return col;
					}
				}
		
				return null;				
			}
		}

		/// <summary>
		/// Returns visible columns (Column.Visible = true).
		/// </summary>
		public WGridColumn[] VisibleColumns
		{
			get{
                List<WGridColumn> visibleColumns = new List<WGridColumn>();
				foreach(WGridColumn col in this){
					if(col.Visible){
						visibleColumns.Add(col);
					}
				}
				
				return visibleColumns.ToArray();
			}
		}


		/// <summary>
		/// Gets columns owner view.
		/// </summary>
		public WGridTableView View
		{
			get{ return m_pView; }
		}


		/// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pColumns.GetEnumerator();
        }

        #endregion

    }
}
