using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.UI.Controls.Grid
{
    /// <summary>
    /// Represents grid view collection in grid control.
    /// </summary>
    public class WGridViewCollection
    {
        private List<WGridTableView> m_pList = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal WGridViewCollection()
        {
            m_pList = new List<WGridTableView>();
        }


        #region method Add

        /// <summary>
        /// Adss specified view to the collection.
        /// </summary>
        /// <param name="view">View to add.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>view</b> is null reference.</exception>
        public void Add(WGridTableView view)
        {
            if(view == null){
                throw new ArgumentNullException("view");
            }

            if(Contains(view.Name)){
                throw new ArgumentException("View with the sepcified name '" + view.Name + "' already exists in the collection.");
            }

            m_pList.Add(view);
        }

        #endregion

        #region method Remove

        /// <summary>
        /// Removes with with the specified name from the collection.
        /// </summary>
        /// <param name="name">View name.</param>
        /// <exception cref="ArgumentNullException">Is raised when <b>name</b> is null reference.</exception>
        public void Remove(string name)
        {
            if(name == null){
                throw new ArgumentNullException("name");
            }

            WGridTableView view = this[name];
            if(view != null){
                m_pList.Remove(view);
            }
        }

        #endregion

        #region method Clear

        /// <summary>
        /// Removes all views from the collection.
        /// </summary>
        public void Clear()
        {
            m_pList.Clear();
        }

        #endregion

        #region method Contains

        /// <summary>
        /// Gets if the collection contains view with the specified name.
        /// </summary>
        /// <param name="name">View name.</param>
        /// <returns>Returns true if the collection contains the specified view, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>name</b> is null reference.</exception>
        public bool Contains(string name)
        {   
            if(name == null){
                throw new ArgumentNullException("name");
            }
        
            return this[name] != null;
        }

        #endregion

                
        #region interface IEnumerator

        /// <summary>
		/// Gets enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return m_pList.GetEnumerator();
		}

		#endregion

        #region Properties implementation

        /// <summary>
        /// Gets number of items in the collection.
        /// </summary>
        public int Count
        {
            get{ return m_pList.Count; }
        }

        /// <summary>
        /// Gets view with a specified name.
        /// </summary>
        /// <param name="name">View name.</param>
        /// <returns>Returns specified view or null if no such view.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>name</b> is null reference.</exception>
        public WGridTableView this[string name]
        {
            get{
                if(name == null){
                    throw new ArgumentNullException("name");
                }

                foreach(WGridTableView view in m_pList){
                    if(name.ToLower() == view.Name.ToLower()){
                        return view;
                    }
                }

                return null;
            }
        }

        #endregion

    }
}
