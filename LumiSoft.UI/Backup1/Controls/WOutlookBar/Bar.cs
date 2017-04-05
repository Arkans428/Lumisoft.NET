using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// Holds bar items style.
	/// </summary>
	public enum ItemsStyle
	{
		/// <summary>
		/// Item is fully selected (icon+text).
		/// </summary>
		FullSelect = 1,

		/// <summary>
		/// Icon is selected only.
		/// </summary>
		IconSelect = 2,

		/// <summary>
		/// Small icons.
		/// </summary>
		SmallIcon = 4,

		/// <summary>
		/// Style is taken from ViewStyle.
		/// </summary>
		UseDefault = 7,
	}

	/// <summary>
	/// OutlookBar bar.
	/// </summary>
	public class Bar
	{
		private HorizontalAlignment m_TextAlign      = HorizontalAlignment.Center;
		private HorizontalAlignment m_ItemsTextAlign = HorizontalAlignment.Center;
		private string     m_Caption             = "";
		private string     m_TextID              = "";
		private Color      m_TextColor           = Color.Black;
		private Font       m_Font                = null;
		private object     m_Tag                 = null;
		private Items      m_pItems              = null;
		private Color      m_ItemsTextColor      = Color.Black;
		private Font       m_ItemsFont           = null;
		private ItemsStyle m_ItemsStyle          = ItemsStyle.UseDefault;
		private Color      m_BarBackColor        = Color.Coral;
		private Color      m_ItemsBackColor      = Color.Aqua;
		private Bars       m_pBars               = null;   
		private Rectangle  m_BarRect;
		private Rectangle  m_BarClientRect;
		private int        m_FirstVisibleItem    = 0;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="bars"></param>
		public Bar(Bars bars)
		{
			m_pItems    = new Items(this);
			m_pBars     = bars;
			m_Font      = (Font)bars.WOutlookBar.Font.Clone();
			m_ItemsFont = (Font)bars.WOutlookBar.Font.Clone();
		}


		#region function OnBarNeedsUpdate

		/// <summary>
		/// 
		/// </summary>
		public void OnBarNeedsUpdate()
		{
			m_pBars.WOutlookBar.UpdateAll();
		}

		#endregion

		internal bool ItemFullyVisible(Item item)
		{
			return m_BarClientRect.Contains(item.Bounds);
		}


		#region function IsItemVisible

		/// <summary>
		/// Checks if specified item is visible.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Returns true if item is visible.</returns>
		public bool IsItemVisible(Item item)
		{
			if(m_pBars.WOutlookBar.ActiveBar.Items.Contains(item)){
				if(item.Index >= m_FirstVisibleItem){
					return true;
				}
			}
			
			return false;			
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets bar caption.
		/// </summary>
		public string Caption
		{
			get{ return m_Caption; }

			set{
				m_Caption = value;
				OnBarNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets or sets text ID. This is for multilingual support.
		/// For examle at runtime you can search with this textID right caption for item.
		/// </summary>
		public string TextID
		{
			get{ return m_TextID; }

			set{ m_TextID = value; }
		}

		/// <summary>
		/// Gets or sets bar caption alignment.
		/// </summary>
		public HorizontalAlignment TextAlign
		{
			get{ return m_TextAlign; }

			set{
				m_TextAlign = value;
				OnBarNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets or sets bar tag.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}

		/// <summary>
		/// Gets or sets bar caption font.
		/// </summary>
		public Font Font
		{
			get{ return m_Font; }

			set{
				m_Font = value;
				OnBarNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets bar index.
		/// </summary>
		public int Index
		{
			get{ return m_pBars.IndexOf(this); }
		}

		/// <summary>
		/// Gets bar items collection.
		/// </summary>
		public Items Items
		{
			get{ return m_pItems; }
		}

		/// <summary>
		/// Gets or sets bar items text alignment.
		/// </summary>
		public HorizontalAlignment ItemsTextAlign
		{
			get{ return m_ItemsTextAlign; }

			set{
				m_ItemsTextAlign = value;
				OnBarNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets or sets bar items style.
		/// </summary>
		public ItemsStyle ItemsStyle
		{
			get{ return m_ItemsStyle; }

			set{ m_ItemsStyle = value; }
		}

		/// <summary>
		/// Gets or sets bar items text font.
		/// </summary>
		public Font ItemsFont
		{
			get{ return m_ItemsFont; }

			set{
				m_ItemsFont = value;
				OnBarNeedsUpdate();
			}
		}


		#region internal Properties

		/// <summary>
		/// Gets bar items style. UseDefault is excluded. 
		/// </summary>
		internal ItemsStyle ItemsStyleCurrent
		{
			get{ 
				ItemsStyle itemStyle = this.ItemsStyle;
				//--- First load from ViewStyle
				if(this.ItemsStyle == ItemsStyle.UseDefault){
					itemStyle = this.Bars.WOutlookBar.ViewStyle.BarItemsStyle;				
				}
	
				//--- If ViewStyle retuned UseDefault, set IconSelect as default
				if(itemStyle == ItemsStyle.UseDefault){
					itemStyle = ItemsStyle.IconSelect;
				}
				//-------------------------------------------//
				return itemStyle; 
			}
		}

		internal Rectangle BarRect
		{
			get{ return m_BarRect; }

			set{ m_BarRect = value; }
		}

		internal Rectangle BarClientRect
		{
			get{ return m_BarClientRect; }

			set{ m_BarClientRect = value; }
		}

		internal int FirstVisibleIndex
		{
			get{ return m_FirstVisibleItem; }

			set{ m_FirstVisibleItem = value; }
		}

		internal bool IsLastVisible
		{
			get{
				if(this.Items.Count < 1 || (this.Items[this.Items.Count-1].Bounds.Bottom < this.BarClientRect.Bottom)){
					return true;
				}
				else{
					return false;
				}
			}
		}

		internal Bars Bars
		{
			get{ return m_pBars; }
		}

		internal StringFormat BarStringFormat
		{
			get{
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;

				if(this.ItemsTextAlign == HorizontalAlignment.Center){				
					format.Alignment = StringAlignment.Center;				
				}
				
				if(this.ItemsTextAlign == HorizontalAlignment.Left){
					format.Alignment = StringAlignment.Near;
				}

				if(this.ItemsTextAlign == HorizontalAlignment.Right){
					format.Alignment = StringAlignment.Far;
				}

				return format;
			}
		}

		internal StringFormat ItemsStringFormat
		{
			get{
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Near;

				if(this.ItemsStyleCurrent == ItemsStyle.SmallIcon){
					format.Alignment = StringAlignment.Near;
					return format; 
				}

				if(this.ItemsTextAlign == HorizontalAlignment.Center){				
					format.Alignment = StringAlignment.Center;				
				}
				
				if(this.ItemsTextAlign == HorizontalAlignment.Left){
					format.Alignment = StringAlignment.Near;
				}

				if(this.ItemsTextAlign == HorizontalAlignment.Right){
					format.Alignment = StringAlignment.Far;
				}

				return format;
			}
		}

		#endregion

		#endregion

	}
}
