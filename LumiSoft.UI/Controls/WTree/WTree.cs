using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    ///    Summary description for Control1.
    /// </summary>
    public class WTree : System.Windows.Forms.TreeView
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
		
		private TreeNode  m_current_node  = null;
		private int       m_StateImage    = -1;
		private int       m_FolderOpenImg = -1;
		private bool      m_UseNodeImages = false;
		private bool      m_lock          = false;
        
		/// <summary>
		/// Default constructor.
		/// </summary>
        public WTree()
        {
            // This call is required by the WinForms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call
						
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
            components = new System.ComponentModel.Container();
        }

		#endregion


		#region function OnMouseDown

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right){

				TreeNode node = this.GetNodeAt(e.X,e.Y);
				if(node != null){
					this.SelectedNode = node;
				}
			}

			base.OnMouseDown(e);
		}
		
		#endregion

		#region function OnAfterCheck
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterCheck(TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			bool node_checked = node.Checked;

			if(!m_UseNodeImages){
				if(node.IsExpanded) // If node expanded show open folder image or state image
				{
					if(node_checked){
						node.ImageIndex = m_FolderOpenImg;
					}
					else{
						node.ImageIndex = m_StateImage;
					}					
				}
				else
				{
					if(node_checked) // If node collapsed show default image or state image
					{
						node.ImageIndex = ImageIndex;
					}
					else
					{
						node.ImageIndex = m_StateImage;
					}	
				}
			}

			base.OnAfterCheck(e);

			// If looping child or parent nodes , return
			if(m_lock) return;			
			m_lock = true;
												
			// Child nodes stuff -------------------------------------------------------------- //
			if(node.Nodes.Count > 0){				
				// Checks or UnChecks all child nodes
				Check_Childs(node,node_checked);				
			}
			//------------------------------------------------------------------------------------//

            
			// Checks or UnChecks all parent nodes
			if(node.Parent != null){
				Check_Parents(node,node_checked);
			}

			m_lock = false;		
		}

		#endregion


		#region function OnAfterCollapse

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);

			TreeNode node = e.Node;
			if(node.Checked){
				node.ImageIndex = ImageIndex;
			}
			else{
				node.ImageIndex = m_StateImage;
			}			
		}

		#endregion

		#region function OnAfterExpand

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterExpand(System.Windows.Forms.TreeViewEventArgs e)
		{
			base.OnAfterExpand(e);

			TreeNode node = e.Node;
			if(node.Checked){
				node.ImageIndex = m_FolderOpenImg;
			}
			else{
				node.ImageIndex = m_StateImage;
			}			
		}

		#endregion


		#region function Check_Childs
		
		/// <summary>
		/// Checks or UnChecks all Childs of node.
		/// </summary>
		/// <param name="node"> </param>
		/// <param name="node_state"> </param>
		private void Check_Childs(TreeNode node,bool node_state)
		{
			foreach(TreeNode nod in node.Nodes)
			{
				if(nod.Nodes.Count > 0){
					Check_Childs(nod,node_state);
				}
				
				// Set current node state
				nod.Checked = node_state;
			}
		}

		#endregion

		#region function Check_Parents

		/// <summary>
		/// Checks or UnChecks all Parents of node.
		/// </summary>
		/// <param name="node"> </param>
		/// <param name="node_state"> </param>
		private void Check_Parents(TreeNode node,bool node_state)
		{
			TreeNode n = node.Parent;

			// Loop through parent nodes
			while(n != null)
			{
				// If childs checked
				if(node_state){
					n.Checked = true;
					n.ImageIndex = m_FolderOpenImg;
				}
				else
				{
					// Check if all items in node are UnChecked
					if(Is_Last_UnChecked(n)){
						n.Checked = false;
						n.ImageIndex = m_StateImage;
					}					
				}
                
				// Get Next Parent
				n = n.Parent;
			}
		}

		#endregion

		#region function Is_Last_UnChecked

		/// <summary>
		/// Check if all items in node(all childs of node) are UnChecked
		/// </summary>
		/// <param name="node"> </param>
		private bool Is_Last_UnChecked(TreeNode node)
		{		
			// Start with first node
			TreeNode nod = node.FirstNode;
			while(nod != null)
			{	
				if(nod.Checked){
					return false;
				}
				nod = nod.NextNode;
			}

			return true;								
		}

		#endregion

		#region function Loop_Tree

		/// <summary>
		/// Gets Next Node in Tree.
		/// </summary>
		public TreeNode Loop_Tree()
		{
			TreeNode nod = null;

			if(m_current_node == null){
				return m_current_node = this.Nodes[0];
			}
			
			// Try to move child node
			if(m_current_node.Nodes.Count > 0)
			{
				return m_current_node = m_current_node.Nodes[0];
			}
			else
			{
				// Try to move next sibling node
				if(m_current_node.NextNode != null)
				{
					return m_current_node = m_current_node.NextNode;	
				}
				else
				{
					// Try to move parent node and to parent's next sibling node,
					// if fails get next parent,until to end 
					nod = m_current_node;
					while(nod != null){
						if(nod.NextNode != null){	
							return m_current_node = nod.NextNode;	
						}
						nod = nod.Parent;
					}

					m_current_node = null;
				}
			}

			return m_current_node;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start_first"></param>
		/// <returns></returns>
		public TreeNode Loop_Tree(bool start_first) 
		{
			m_current_node = null;
			return Loop_Tree();
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		[Obsolete("dummy I think ???")]
		public void Refresh_StateImages()
		{
			TreeNode nod = Loop_Tree(true);
			while(nod != null)
			{				
				if(!nod.Checked){
					nod.ImageIndex = m_StateImage;
				}
				else{
					nod.ImageIndex = ImageIndex;
				}
				nod = Loop_Tree();
			}
		}

		#region Properties Implementation
		
		/// <summary>
		/// 
		/// </summary>
		[
		Category("Appearance"),
		Description("Sets state image for checked state"),
		]
		public int StateImage
		{
			get{ return m_StateImage; }

			set{
				m_StateImage = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Category("Appearance"),
		Description("Sets Image for open folder"),
		]
		public int FolderOpenImage
		{
			get{ return m_FolderOpenImg; }

			set{
				m_FolderOpenImg = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool UseNodeImages
		{
			get{ return m_UseNodeImages; }

			set{
				m_UseNodeImages = value;
			}
		}

		#endregion

    }
}
