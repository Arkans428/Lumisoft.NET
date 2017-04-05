using System;
using System.Drawing;
using System.ComponentModel;

namespace LumiSoft.UI.Controls.WOutlookBar
{
	/// <summary>
	/// OutlookBar Bar item.
	/// </summary>
	public class Item
	{
		private string    m_Caption    = "";
		private string    m_TextID     = "";
		private object    m_Tag        = null;
		private int       m_ImageIndex = -1;
	//	private bool      m_Enabled    = true;
	//	private bool      m_Visible    = true;
		private bool      m_AllowStuck = true;
		private Items     m_pItems     = null;
		private Rectangle m_Bounds;
	
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Item(Items items)
		{
			m_pItems     = items;
			m_AllowStuck = m_pItems.Bar.Bars.WOutlookBar.AllowItemsStuck;
		}


		#region function OnItemNeedsUpdate

		/// <summary>
		/// 
		/// </summary>
		public void OnItemNeedsUpdate()
		{
			m_pItems.Bar.Bars.WOutlookBar.UpdateAll();
		}

		#endregion


		#region Properties Implementation
		
		/// <summary>
		/// Gets or sets item caption.
		/// </summary>
		public string Caption
		{
			get{ return m_Caption; }

			set{ 
				m_Caption = value; 
				OnItemNeedsUpdate();
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
		/// Gets or sets item tag.
		/// </summary>
		public object Tag
		{
			get{ return m_Tag; }

			set{ m_Tag = value; }
		}
		
		/// <summary>
		/// Gets or sets item's image index.
		/// </summary>
		[DefaultValue(-1)]
		public int ImageIndex
		{
			get{ return m_ImageIndex; }

			set{ 
				m_ImageIndex = value; 
				OnItemNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets item index.
		/// </summary>
		public int Index
		{
			get{ return (m_pItems != null ? m_pItems.IndexOf(this) : -1); }
		}

		/// <summary>
		/// Gets or sets if item can stuck.
		/// </summary>
		public bool AllowStuck
		{
			get{ return m_AllowStuck; }

			set{ 
				m_AllowStuck = value; 
				OnItemNeedsUpdate();
			}
		}

		/// <summary>
		/// Gets owner bar of item.
		/// </summary>
		public Bar Bar
		{
			get{ return m_pItems.Bar; }
		}


		#region Internal Properties

		/// <summary>
		/// Gets referance to items collection.
		/// </summary>
		internal Items Items
		{
			get{ return m_pItems; }
		}

		/// <summary>
		/// Gets item image.
		/// </summary>
		internal Image Image
		{
			get{
				if(this.ImageIndex > -1){
					if(this.Bar.ItemsStyleCurrent == ItemsStyle.SmallIcon){
						if(this.WOutlookBar.ImageListSmall != null && this.ImageIndex < this.WOutlookBar.ImageListSmall.Images.Count){
							return this.WOutlookBar.ImageListSmall.Images[this.ImageIndex];
						}
					}
					else{
						if(this.WOutlookBar.ImageList != null && this.ImageIndex < WOutlookBar.ImageList.Images.Count){
							return this.WOutlookBar.ImageList.Images[this.ImageIndex];
						}
					}
				}

				return null; 
			}
		}
	
		/// <summary>
		/// Gets or sets item bounds.
		/// </summary>
		internal Rectangle Bounds
		{
			get{ return m_Bounds; }

			set{ m_Bounds = value; }
		}

		/// <summary>
		/// Gets item image rect.
		/// </summary>
		internal Rectangle ImageRect
		{
			get{ 
				if(this.Bar.ItemsStyleCurrent == ItemsStyle.SmallIcon){
					return new Rectangle(3,this.Bounds.Y+4,16,16);
				}
				else{
					return new Rectangle((this.Bounds.Width-32 + 4)/2,this.Bounds.Y+4,32,32);
				}
			}
		}

		/// <summary>
		/// Gets item text rect.
		/// </summary>
		internal Rectangle TextRect
		{
			get{ 
				if(this.Bar.ItemsStyleCurrent == ItemsStyle.SmallIcon){
					return new Rectangle(this.Bounds.X+20,this.Bounds.Top+5,this.Bounds.Width-15,this.Bounds.Height - 2);
				}
				else{
					return new Rectangle(this.Bounds.X+2,this.ImageRect.Bottom+3,this.Bounds.Width,this.Bounds.Bottom - this.ImageRect.Bottom + 3);
				}
			}
		}

		/// <summary>
		/// Gets referance to OutlookBar control.
		/// </summary>
		internal WOutlookBar WOutlookBar
		{
			get{ return m_pItems.Bar.Bars.WOutlookBar; }
		}

		/// <summary>
		/// Gets referance to OutlookBar control ViewStyle.
		/// </summary>
		internal ViewStyle ViewStyle
		{
			get{ return m_pItems.Bar.Bars.WOutlookBar.ViewStyle; }
		}

		#endregion

		#endregion

	}
}
