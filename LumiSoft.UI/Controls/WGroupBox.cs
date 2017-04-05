using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LumiSoft.UI.Controls
{
    /// <summary>
    /// Implements groupbox control
    /// </summary>
    public class WGroupBox : GroupBox
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public WGroupBox()
        {
        }


        #region override method OnMouseDoubleClick

        /// <summary>
        /// Raises MouseDoubleClieck event.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if((Control.ModifierKeys & Keys.Alt) != 0 && (Control.ModifierKeys & Keys.Control) != 0 && (Control.ModifierKeys & Keys.Shift) != 0){
                Form design = new Form();
                design.ClientSize = new Size(300,600);

                ComboBox controls = new ComboBox();
                controls.Size = new Size(300,20);
                controls.Location = new Point(0,5);
                
                PropertyGrid grid = new PropertyGrid();
                grid.Size = new Size(300,570);
                grid.Location = new Point(0,30);
                foreach(Control c in this.Controls){
                    controls.Items.Add(new LumiSoft.UI.Controls.WComboItem(c.Name + " (" + c.GetType().Name + ")",c));
                }

                design.Controls.Add(controls);
                design.Controls.Add(grid);

                design.Show();

                controls.SelectedIndexChanged += new EventHandler(delegate(object s,EventArgs e1){
                    grid.SelectedObject = ((LumiSoft.UI.Controls.WComboItem)controls.SelectedItem).Tag;
                });
                controls.SelectedIndex = 0;
            }
        }

        #endregion
    }
}
