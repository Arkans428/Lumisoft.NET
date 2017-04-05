using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace LumiSoft.UI
{
    /// <summary>
    /// Wisk UI Text hadling library.
    /// </summary>
    public class WText : IDisposable
    {	        
        private static WText m_pStaticWText = null;

        private DataSet                    m_pDsTexts = null;
		private string                     m_Language = "";
        private Dictionary<string,DataRow> m_pTexts   = null;
				
		/// <summary>
		/// Loads text from specified path.
		/// </summary>
		/// <param name="workingPath">Directory from where to search WText.xml.</param>
		/// <param name="language">Default working language</param>
		public WText(string workingPath,string language)
		{		
			m_pDsTexts = new DataSet();
		    m_pDsTexts.ReadXml(workingPath + "WText.xml");
		
		    ChangeLanguage(language);
		}

		#region method Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{			
		}

		#endregion


		#region method GetTexts

		/// <summary>
		/// Get specified texts from local file.
		/// </summary>
		/// <param name="txtNumbers">Text numbers, separated by ','.</param>
		/// <returns>Returns DataSet containing "Text" table with one DataRow or "Error" table describeing error.</returns>
		/// <remarks>If local file doesn't exist, file will be retrieved from server.</remarks>
		/// <example>
		/// DataSet ds = GetTexts("1,2,45,65","EST");
		/// if(ds.Tables.Contains("Text")){
		///		DataRow dr   = ds.Tables["Text"].Row[0];
		///		string text1 = dr["T1"].ToString(); 
		/// }
		/// </example>
		public DataSet GetTexts(string txtNumbers)
		{			
			DataSet dsTxt = null;
			dsTxt = GetTexts(txtNumbers,m_Language);
						
			return dsTxt;
		}

		/// <summary>
		/// Get specified texts from local file.
		/// </summary>
		/// <param name="txtNumbers">Text numbers, separated by ','.</param>
		/// <param name="language">Language to retrieve.</param>
		/// <returns>Returns DataSet containing "Text" table with one DataRow or "Error" table describeing error.</returns>
		/// <remarks>If local file doesn't exist, file will be retrieved from server.</remarks>
		/// <example>
		/// DataSet ds = GetTexts("1,2,45,65","EST");
		/// if(ds.Tables.Contains("Text")){
		///		DataRow dr   = ds.Tables["Text"].Row[0];
		///		string text1 = dr["T1"].ToString(); 
		/// }
		/// </example>
		public DataSet GetTexts(string txtNumbers,string language)
		{
			DataSet ds = new DataSet();				
			ds = MakeTextDs(txtNumbers,language);
			
			return ds;
		}
		
		#endregion

		#region method Contains

		/// <summary>
		/// Checks if specified text exsists.
		/// </summary>
		/// <param name="txtNr">Text number.</param>
		/// <returns>Returns true if specified text exists.</returns>
		public bool Contains(string txtNr)
		{
			if(txtNr.StartsWith("T")){
				txtNr = txtNr.Substring(1);
			}
			if(m_pTexts.ContainsKey(txtNr)){
				return true;
			}
		
			return false;
		}

		#endregion

		#region method GetLanguages

		/// <summary>
		/// Gets available languages. NOTE: key contains 3 symbol language code eg. EST and value full name eg. Estonian.
		/// </summary>
		/// <returns></returns>
		public Hashtable GetLanguages()
		{
            Hashtable t = new Hashtable();
			foreach(DataColumn dc in m_pDsTexts.Tables["Text"].Columns)
			{
				string languageFullName = dc.ColumnName;
				switch(dc.ColumnName.ToUpper())
				{
					case "EST":
						languageFullName = "Estonian";
						break;

					case "RUS":
						languageFullName = "Russian";
						break;

					case "LIT":
						languageFullName = "Lithuanian";
						break;

					case "LAT":
						languageFullName = "Latvian";
						break;

					case "FIN":
						languageFullName = "Finnsih";
						break;

					case "ENG":
						languageFullName = "English";
						break;

					case "SWE":
						languageFullName = "Sweden";
						break;

					case "ITA":
						languageFullName = "Italian";
						break;

					case "GER":
						languageFullName = "Germanian";
						break;
				}

				if(dc.ColumnName.ToUpper() != "TXTID"){
					t.Add(dc.ColumnName,languageFullName);
				}
			}

			return t;
		}
		
		#endregion

        #region method ChangeLanguage

        /// <summary>
        /// Changes active language.
        /// </summary>
        /// <param name="language">Language code.</param>
        public void ChangeLanguage(String language)
        {
            // If specified language doesn't exist, throw exception
		    if(!m_pDsTexts.Tables["Text"].Columns.Contains(language)){
			    throw new Exception("Language '" + language + "' doesn't exist !");
		    }
    		
		    // Construct Hashtable to specified language. Key = textID and value = DataRow.
		    m_pTexts = new Dictionary<string,DataRow>();						
		    for(int i=0;i<m_pDsTexts.Tables["Text"].Rows.Count;i++){
			    String textID = m_pDsTexts.Tables["Text"].Rows[i]["TxtID"].ToString();			
    					
			    m_pTexts.Add(textID,m_pDsTexts.Tables["Text"].Rows[i]);
		    }
    		
		    m_Language = language;
    		
		    OnLanguageChanged();
        }

        #endregion


        #region method MakeTextDs

        private DataSet MakeTextDs(string textNumbers,string language)
		{
			string[] textNos = textNumbers.Split(new char[]{','});

			DataSet   ds = new DataSet();
			DataTable dt = ds.Tables.Add("Text");		
			DataRow   dr = dt.NewRow();
            
            if(textNumbers == "*"){
                foreach(KeyValuePair<string,DataRow> entry in  m_pTexts){
                    dt.Columns.Add("T" + entry.Key,System.Type.GetType("System.String"));

					dr["T" + entry.Key] = m_pTexts[entry.Key][language];
                }
            }
            else{
                foreach(string textNo in textNos){			
				    if(m_pTexts.ContainsKey(textNo) && !dt.Columns.Contains("T" + textNo) && textNo.Length > 0){
					    dt.Columns.Add("T" + textNo,System.Type.GetType("System.String"));
				
					    DataRow drText = (DataRow)m_pTexts[textNo];
					    dr["T" + textNo] = drText[language];
				    }
			    }
            }			

			dt.Rows.Add(dr);

			return ds;
		} 

		#endregion

		#region function GetTxtFromExpr

		private string GetTxtFromExpr(string expression)
		{
			string retVal = "";
			string[] parts = expression.Split(new char[]{'+'});
			foreach(string part in parts){
				string partT = part.Trim();
				if(partT.StartsWith("'")){
					retVal += partT.Substring(1,partT.Length - 2);
				}
				else{
					retVal += this[partT];
				}
			}

			return retVal;
		}

		#endregion


        #region Properties Implementation

        /// <summary>
        /// Gets or sets static WText. This is global to application domain.
        /// </summary>
        public static WText StaticWText
        {
            get{ return m_pStaticWText; }

            set{ m_pStaticWText = value; }
        }

        /// <summary>
		/// Gets specified text.
		/// </summary>
		public string this[string txtNr]
		{
			get{ return this[txtNr,m_Language];	}
		}

		/// <summary>
		/// Gets specified text.
		/// </summary>
		public string this[string textNo,string language]
		{
			get{
				// If txtNr is expression. eg. 13+'fssf'
				if(textNo.IndexOf("+") > -1){
					return GetTxtFromExpr(textNo);
				}

				if(textNo.StartsWith("T")){
					textNo = textNo.Substring(1);
				}
				if(m_pTexts.ContainsKey(textNo)){
					DataRow drText = (DataRow)m_pTexts[textNo];

					if(drText.Table.Columns.Contains(language)){
						return drText[language].ToString();
					}
					else{
						#if DEBUG 
							throw new Exception("Language '" + language + "' is missing !");					
						#else
							return "*Missing language **";
						#endif
					}					
				}
				else{
					#if DEBUG 
						throw new Exception("Text '" + textNo.ToString() + "' is missing !");					
					#else
						return "*Missing Text **";
					#endif
				}
			}
		}

        /// <summary>
        /// Gets current language.
        /// </summary>
        public string Language
        {
            get{ return m_Language; }
        }
        
        #endregion

        #region Events Handling

        /// <summary>
        /// This event is raised when language has changed.
        /// </summary>
        public event EventHandler LanguageChanged = null;

        /// <summary>
        /// Raises <b>LanguageChanged</b> event.
        /// </summary>
        private void OnLanguageChanged()
        {
            if(this.LanguageChanged != null){
                this.LanguageChanged(this,new EventArgs());
            }
        }

        #endregion

    }
}
