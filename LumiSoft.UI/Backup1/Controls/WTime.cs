using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Holds active time part.
	/// </summary>
	internal enum TimePos
	{
		Hour,
		Minute,
		Second,
	}

	/// <summary>
	/// Hold time value intenally.
	/// </summary>
	internal struct TimeStruct
	{
		internal int hour;
		internal int minute;
		internal int second;

		internal int hourSPos;
		internal int minuteSPos;
		internal int secondSPos;
		internal int hourEPos;
		internal int minuteEPos;
		internal int secondEPos;

		public TimeStruct(bool defaults)
		{
			hour   = 0;
			minute = 0;
			second = 0;

			hourSPos   = 0;
			minuteSPos = 0;
			secondSPos = 0;
			hourEPos   = 0;
			minuteEPos = 0;
			secondEPos = 0;
		}
	}

	/// <summary>
	/// Time editor.
	/// </summary>
	public class WTime : WFocusedCtrlBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TimePos    m_ActiveTimePart;
		private TimeStruct m_TimeVal;		
		private bool       m_ShowSeconds = true;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WTime() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

			m_TimeVal = new TimeStruct(true);
			m_ActiveTimePart = TimePos.Hour;

			// Set control type, needed for ViewStyle coloring.
			m_ControlType = ControlType.Label;
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
			// 
			// WTime
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Name = "WTime";
			this.Size = new System.Drawing.Size(120, 24);

		}
		#endregion


		#region overrides

		#region function override OnPaint

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int yPos = ((this.ClientSize.Height - (int)e.Graphics.MeasureString("9",this.Font).Height) / 2);
			
			int pos = 0;
			// Draw hour				
			e.Graphics.DrawString(m_TimeVal.hour.ToString("d2"),this.Font,new SolidBrush(GetTextColor(TimePos.Hour)),0,yPos);
			m_TimeVal.hourSPos = pos;
			pos += (int)e.Graphics.MeasureString(m_TimeVal.hour.ToString("d2"),this.Font).Width;
			m_TimeVal.hourEPos = pos;

			// Draw :
			e.Graphics.DrawString(":",this.Font,new SolidBrush(Color.Black),pos,yPos);
			pos += (int)e.Graphics.MeasureString(":",this.Font).Width;

			// Draw minute
			e.Graphics.DrawString(m_TimeVal.minute.ToString("d2"),this.Font,new SolidBrush(GetTextColor(TimePos.Minute)),pos,yPos);
			m_TimeVal.minuteSPos = pos;
			pos += (int)e.Graphics.MeasureString(m_TimeVal.minute.ToString("d2"),this.Font).Width;
			m_TimeVal.minuteEPos = pos;

			if(m_ShowSeconds){
				// Draw :
				e.Graphics.DrawString(":",this.Font,new SolidBrush(Color.Black),pos,yPos);
				pos += (int)e.Graphics.MeasureString(":",this.Font).Width;

				// Draw seconds
				e.Graphics.DrawString(m_TimeVal.second.ToString("d2"),this.Font,new SolidBrush(GetTextColor(TimePos.Second)),pos,yPos);
				m_TimeVal.secondSPos = pos;
				m_TimeVal.secondEPos = pos + 100; // dummy
			}
		}

		#endregion


		#region function override OnKeyPress

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			e.Handled = true;

			// Move next time part. Eg. hh->mm.
			if(e.KeyChar == '.' || e.KeyChar == ',' || e.KeyChar == ':'){
				switch(m_ActiveTimePart)
				{
					case TimePos.Hour:
						m_ActiveTimePart = TimePos.Minute;
						break;

					case TimePos.Minute:
						if(m_ShowSeconds){
							m_ActiveTimePart = TimePos.Second;
						}
						else{
							m_ActiveTimePart = TimePos.Hour;
						}
						break;

					case TimePos.Second:
						m_ActiveTimePart = TimePos.Hour;
						break;
				}

				this.Refresh();
			}
			else{
				// We want only number
				if(char.IsDigit(e.KeyChar)){
					switch(m_ActiveTimePart)
					{
						case TimePos.Hour:
							m_TimeVal.hour = GetTimePartValue(23,m_TimeVal.hour,e.KeyChar);
							break;

						case TimePos.Minute:
							m_TimeVal.minute = GetTimePartValue(59,m_TimeVal.minute,e.KeyChar);
							break;

						case TimePos.Second:
							m_TimeVal.second = GetTimePartValue(59,m_TimeVal.second,e.KeyChar);
							break;
					}

					this.OnTextChanged(new EventArgs());
				}

				this.Refresh();
			}
		}

		#endregion


		#region function override OnMouseDown

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			// Hour pos
			if(e.X < m_TimeVal.hourEPos){
				m_ActiveTimePart = TimePos.Hour;
			}
			else{
				// Second pos
				if(m_ShowSeconds && e.X > m_TimeVal.secondSPos){
					m_ActiveTimePart = TimePos.Second;
				}
				else{
					m_ActiveTimePart = TimePos.Minute;
				}
			}

			this.Focus();
			this.Refresh();			
		}

		#endregion

		#endregion


		#region function GetTextColor

		private Color GetTextColor(TimePos val)
		{
			if(this.Focused && val == m_ActiveTimePart){
				return Color.Red;
			}

			return Color.Black;
		}

		#endregion

		#region function GetTimePartValue

		private int GetTimePartValue(int maxVal,int val,char key)
		{			
			if(Convert.ToInt32(val.ToString() + key.ToString()) > maxVal){
				return Convert.ToInt32(key.ToString());
			}
			else{
				return Convert.ToInt32(val.ToString() + key.ToString());
			}
		}

		#endregion


		#region funciton ActivateHours

		/// <summary>
		/// Sets focus to hours edit part.
		/// </summary>
		public void FocusHours()
		{
			m_ActiveTimePart = TimePos.Hour;
		}

		#endregion


		#region Properties Implementaion

		/// <summary>
		/// Gets or sets time value.
		/// </summary>
		public DateTime Value
		{
			get{ return new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,m_TimeVal.hour,m_TimeVal.minute,m_TimeVal.second); }

			set{
				m_TimeVal.hour   = value.Hour;
				m_TimeVal.minute = value.Minute;
				m_TimeVal.second = value.Second;

				this.Refresh();
			}
		}

		/// <summary>
		/// Gets text representation of time.
		/// </summary>
		public override string Text
		{
			get{ return m_TimeVal.hour + ":" + m_TimeVal.minute + ":" + m_TimeVal.second; }

			set{
				DateTime d = Convert.ToDateTime(value);
				this.Value = d;
			}
		}

		/// <summary>
		/// Gets or sets if to show seconds part.
		/// </summary>
		public bool ShowSeconds
		{
			get{ return m_ShowSeconds; }

			set{ 
				m_ShowSeconds = value; 

				this.Refresh();
			}
		}

		#endregion

	}
}
