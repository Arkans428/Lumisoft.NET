using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Rich editbox control.
	/// </summary>
	public class WRichEditBox : WFocusedCtrlBase
	{
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem_Bold;
		private System.Windows.Forms.MenuItem menuItem_Italic;
		private System.Windows.Forms.MenuItem menuItem_UnderLine;
		private System.Windows.Forms.MenuItem menuItem_Color;
		private System.Windows.Forms.MenuItem menuItem_Font;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WRichEditBox() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			richTextBox1.LostFocus += new System.EventHandler(this.m_pTextBox_OnLostFocus);
			richTextBox1.GotFocus  += new System.EventHandler(this.m_pTextBox_OnGotFocus);

			// Set control type, needed for ViewStyle coloring.
			m_ControlType = ControlType.Edit;
		}

		#region function Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem_Bold = new System.Windows.Forms.MenuItem();
			this.menuItem_Italic = new System.Windows.Forms.MenuItem();
			this.menuItem_UnderLine = new System.Windows.Forms.MenuItem();
			this.menuItem_Color = new System.Windows.Forms.MenuItem();
			this.menuItem_Font = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.ContextMenu = this.contextMenu1;
			this.richTextBox1.Location = new System.Drawing.Point(1, 1);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(102, 18);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "richTextBox1";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem_Bold,
																						 this.menuItem_Italic,
																						 this.menuItem_UnderLine,
																						 this.menuItem_Color,
																						 this.menuItem_Font});
			// 
			// menuItem_Bold
			// 
			this.menuItem_Bold.Index = 0;
			this.menuItem_Bold.Text = "Bold";
			this.menuItem_Bold.Click += new System.EventHandler(this.menuItem_Bold_Click);
			// 
			// menuItem_Italic
			// 
			this.menuItem_Italic.Index = 1;
			this.menuItem_Italic.Text = "Italic";
			this.menuItem_Italic.Click += new System.EventHandler(this.menuItem_Italic_Click);
			// 
			// menuItem_UnderLine
			// 
			this.menuItem_UnderLine.Index = 2;
			this.menuItem_UnderLine.Text = "UnderLine";
			this.menuItem_UnderLine.Click += new System.EventHandler(this.menuItem_UnderLine_Click);
			// 
			// menuItem_Color
			// 
			this.menuItem_Color.Index = 3;
			this.menuItem_Color.Text = "Color";
			this.menuItem_Color.Click += new System.EventHandler(this.menuItem_Color_Click);
			// 
			// menuItem_Font
			// 
			this.menuItem_Font.Index = 4;
			this.menuItem_Font.Text = "Font";
			this.menuItem_Font.Click += new System.EventHandler(this.menuItem_Font_Click);
			// 
			// WRichEditBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.richTextBox1});
			this.Name = "WRichEditBox";
			this.Size = new System.Drawing.Size(104, 20);
			this.ResumeLayout(false);

		}
		#endregion


		#region Events handling

		#region function m_pTextBox_OnGotFocus

		private void m_pTextBox_OnGotFocus(object sender, System.EventArgs e)
		{
			this.BackColor = m_ViewStyle.EditFocusedColor;			
		}

		#endregion

		#region function m_pTextBox_OnLostFocus

		private void m_pTextBox_OnLostFocus(object sender, System.EventArgs e)
		{
			this.BackColor = m_ViewStyle.GetEditColor(this.ReadOnly,this.Enabled);
		}

		#endregion


		#region function menuItem_Bold_Click

		private void menuItem_Bold_Click(object sender, System.EventArgs e)
		{
			Font f = richTextBox1.SelectionFont;

			FontStyle fStyle = richTextBox1.SelectionFont.Style;			
			if(richTextBox1.SelectionFont.Bold){
				fStyle &= ~FontStyle.Bold;				
			}
			else{
                fStyle |= FontStyle.Bold;				
			}

			f = new Font(richTextBox1.SelectionFont.FontFamily.Name,richTextBox1.SelectionFont.Size,fStyle);

			richTextBox1.SelectionFont = f;
		}

		#endregion

		#region function menuItem_Italic_Click

		private void menuItem_Italic_Click(object sender, System.EventArgs e)
		{
			Font f = richTextBox1.SelectionFont;

			FontStyle fStyle = richTextBox1.SelectionFont.Style;			
			if(richTextBox1.SelectionFont.Italic){
				fStyle &= ~FontStyle.Italic;				
			}
			else{
                fStyle |= FontStyle.Italic;				
			}

			f = new Font(richTextBox1.SelectionFont.FontFamily.Name,richTextBox1.SelectionFont.Size,fStyle);

			richTextBox1.SelectionFont = f;
		}

		#endregion

		#region function menuItem_UnderLine_Click

		private void menuItem_UnderLine_Click(object sender, System.EventArgs e)
		{
			Font f = richTextBox1.SelectionFont;

			FontStyle fStyle = richTextBox1.SelectionFont.Style;			
			if(richTextBox1.SelectionFont.Underline){
				fStyle &= ~FontStyle.Underline;				
			}
			else{
                fStyle |= FontStyle.Underline;				
			}

			f = new Font(richTextBox1.SelectionFont.FontFamily.Name,richTextBox1.SelectionFont.Size,fStyle);

			richTextBox1.SelectionFont = f;
		}

		#endregion

		#region function menuItem_Color_Click

		private void menuItem_Color_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			if(dlg.ShowDialog(this) == DialogResult.OK){
				richTextBox1.SelectionColor = dlg.Color;
			}
		}

		#endregion

		#region function menuItem_Font_Click

		private void menuItem_Font_Click(object sender, System.EventArgs e)
		{
			FontDialog dlg = new FontDialog();
			dlg.Font = richTextBox1.SelectionFont;
			if(dlg.ShowDialog(this) == DialogResult.OK){
				richTextBox1.SelectionFont = dlg.Font;
			}
		}

		#endregion

		#endregion


		#region method Clear

		/// <summary>
		/// Clears all text from text box control.
		/// </summary>
		public void Clear()
		{
			richTextBox1.Clear();
		}

		#endregion


		#region Properties Implementation

		#region Color stuff
		
		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public new Color BackColor
		{
			get{ return base.BackColor; }

			set{
				base.BackColor     = value;
				richTextBox1.BackColor = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Color ForeColor
		{
			get{ return base.ForeColor; }

			set{
				base.ForeColor     = value;
				richTextBox1.ForeColor = value;
			}
		}

		#endregion

		/// <summary>
		/// Gets or sets control text.
		/// </summary>
		public new string Text
		{
			get{ return richTextBox1.Text; }

			set{ richTextBox1.Text = value; }
		}

		/// <summary>
		/// Tries to set text as rtf.If text isn't rtf, sets as normal text.
		/// </summary>
		public string TextEx
		{
			set{
				try{
					richTextBox1.Rtf = value;
				}
				catch{
					richTextBox1.Text = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets rich text.
		/// </summary>
		public string Rtf
		{
			get{ return richTextBox1.Rtf; }

			set{ richTextBox1.Rtf = value; }
		}

		/// <summary>
		/// Gets or sets multiple lines of text.
		/// </summary>
		public string[] Lines
		{
			get{ return richTextBox1.Lines; }

			set{ richTextBox1.Lines = value; }
		}

		/// <summary>
		/// Gets or sets textbox readonly.
		/// </summary>
		public bool ReadOnly
		{
			get{ return richTextBox1.ReadOnly; }

			set{ 
				richTextBox1.ReadOnly = value; 

				this.BackColor = m_ViewStyle.GetEditColor(value,this.Enabled,this.ContainsFocus);
			}
		}

		/// <summary>
		/// Gets text length.
		/// </summary>
		public int TextLength
		{
			get{ return richTextBox1.TextLength; }
		}
		
		#endregion

	}
}
