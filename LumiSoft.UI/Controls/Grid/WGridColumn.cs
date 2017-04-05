using System;
using System.Drawing;
using System.Windows.Forms;

using LumiSoft.UI.Controls.Grid.Editors;

namespace LumiSoft.UI.Controls.Grid
{
	/// <summary>
	/// Grid column.
	/// </summary>
	public class WGridColumn
	{
		private WGridColumns        m_pColumns        = null;
		private string              m_ColumnName      = "";
		private string              m_Text            = "";
		private string              m_TextID          = "";	
		private HorizontalAlignment m_TextAlign       = HorizontalAlignment.Right;	
		private Font                m_pFont           = null;
//		private Font                m_pCellsFont      = null;
		private HorizontalAlignment m_CellsTextAlign  = HorizontalAlignment.Left;
		private String              m_CellTextFormat  = "";
		private String              m_MappingName     = "";
		private bool                m_Visible         = true; 
		private bool                m_AllowEdit       = true;
		private bool                m_AllowSort       = true;
		private bool                m_AllowResize     = true;
		private int                 m_Width           = 20;
		private object              m_pTag            = null;		
		private string              m_FooterText      = "";
		private HorizontalAlignment m_FooterTextAlign = HorizontalAlignment.Right;
		private WGridViewFooterType m_FooterType      = WGridViewFooterType.Text;
		private Rectangle           m_pBounds         = new Rectangle(-1,-1,0,0);	
		private WBaseEditor         m_pEditor         = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="columns">Owner colums collection.</param>
		/// <param name="name">Column caption name.</param>
		internal WGridColumn(WGridColumns columns,string name)
		{	
			m_pColumns   = columns;
			m_ColumnName = name;
			m_pFont      = new Font("Microsoft Sans Serif",8F,FontStyle.Regular);
			m_pBounds    = new Rectangle(-1,-1,0,0);
			m_pEditor    = new WTextEditor();
        }


        #region Properties Impelemtation

        /// <summary>
		/// Gets column name.
		/// </summary>
		public string ColumnName
		{
			get{ return m_ColumnName; }
		}

		/// <summary>
		/// Gets or sets column caption.
		/// </summary>
		public string Text
		{
			get{
				if(m_pColumns.View.Grid.WText != null && TextID.Length > 0){                    
					return m_pColumns.View.Grid.WText[TextID];
				}
				else{
					return m_Text;
				}
			}

			set{
				if(m_Text != value){
					m_Text = value;
			
					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}				
			}
		}

		/// <summary>
		/// Gets or sets column text ID.
		/// </summary>
		public string TextID
		{
			get{ return m_TextID; }

			set{
				if(m_TextID != value){
					m_TextID = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column text horiontal alignment.
		/// </summary>
		public HorizontalAlignment TextAlign
		{
			get{ return m_TextAlign; }

			set{
				if(m_TextAlign != value){
					m_TextAlign = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets cells text horizontal alignment.
		/// </summary>
		public HorizontalAlignment CellsTextAlign
		{
			get{ return m_CellsTextAlign; }

			set{
				if(m_CellsTextAlign != value){
					m_CellsTextAlign = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or cells text display format.
		/// </summary>
		public string CellTextFormat
		{
			get{ return m_CellTextFormat; }

			set{
				if(m_CellTextFormat != value){
					m_CellTextFormat = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets datasource DataTable mapping column.
		/// </summary>
		public string MappingName
		{
			get{ return m_MappingName; }

			set{
				if(m_MappingName != value){
					m_MappingName = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column caption text font.
		/// </summary>
		public Font Font
		{
			get{ return m_pFont; }

			set{
				if(m_pFont != value){
					m_pFont = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets if column is visible.
		/// </summary>
		public bool Visible
		{
			get{ return m_Visible; }

			set{
				if(m_Visible != value){
					m_Visible = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets if column is editable.
		/// </summary>
		public bool AllowEdit
		{
			get{ return m_AllowEdit; }

			set{
				if(m_AllowEdit != value){
					m_AllowEdit = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets if column can be sorted.
		/// </summary>
		public bool AllowSort
		{
			get{ return m_AllowSort; }

			set{
				if(m_AllowSort != value){
					m_AllowSort = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets if column can be resized.
		/// </summary>
		public bool AllowResize
		{
			get{ return m_AllowResize; }

			set{
				if(m_AllowResize != value){
					m_AllowResize = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column width.
		/// </summary>
		public int Width
		{
			get{ return m_Width; }

			set{
				if(m_Width != value){
					m_Width = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column tag.
		/// </summary>
		public object Tag
		{
			get{ return m_pTag; }

			set{
				if(m_pTag != value){
					m_pTag = value;
				}
			}
		}

		/// <summary>
		/// Gets column bounding rectangle.
		/// </summary>
		public Rectangle Bounds
		{
			get{ return m_pBounds; }
		}
		internal void setBounds(Rectangle value)
		{
			if(m_pBounds != value){
				m_pBounds = value;
			}
		}

		/// <summary>
		/// Gets or sets column footer text.
		/// </summary>
		public string FooterText
		{
			get{ return m_FooterText; }

			set{
				if(m_FooterText != value){
					m_FooterText = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column footer text horiontal alignment.
		/// </summary>
		public HorizontalAlignment FooterTextAlign
		{
			get{ return m_FooterTextAlign; }

			set{
				if(m_FooterTextAlign != value){
					m_FooterTextAlign = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column column footer type.
		/// </summary>
		public WGridViewFooterType FooterType
		{
			get{ return m_FooterType; }

			set{
				if(m_FooterType != value){
					m_FooterType = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets column editor.
		/// </summary>
		public WBaseEditor Editor
		{
			get{ return m_pEditor; }

			set{
				if(m_pEditor != value){
					m_pEditor = value;

					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets physical column index. Invisible columns are included.
		/// </summary>
		public int Index
		{
			get{ return m_pColumns.IndexOf(this); }

			set{
				if(Index != value){
					m_pColumns.MoveColumn(this,value);
			
					// Notify owner view about Column change.
					m_pColumns.View.OnColumnChanged(this);
				}
			}
        }

        #endregion

    }
}
