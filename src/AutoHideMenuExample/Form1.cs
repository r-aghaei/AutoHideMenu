using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoHideMenuExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool menuIsActive = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Menu is auto-hide. Hover the mouse under the titlebar to see the menu.");
            this.menuStrip1.Visible = false;
        }

        private void hideTimer_Tick(object sender, EventArgs e)
        {
            var p = this.menuStrip1.PointToClient(MousePosition);
            if (menuIsActive)
                return;
            if (menuStrip1.ClientRectangle.Contains(p))
                return;
            foreach (ToolStripMenuItem item in menuStrip1.Items)
                if (item.DropDown.Visible)
                    return;
            this.menuStrip1.Visible = false;
        }

        private void showTimer_Tick(object sender, EventArgs e)
        {
            var p = this.PointToClient(MousePosition);
            if (this.ClientRectangle.Contains(p) && p.Y < 10)
                this.menuStrip1.Visible = true;
        }
        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            menuIsActive = true;
        }
        private void menuStrip1_MenuDeactivate(object sender, EventArgs e)
        {
            menuIsActive = false;
            this.BeginInvoke(new Action(() => { this.menuStrip1.Visible = false; }));
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.Menu))
            {
                if (!this.menuStrip1.Visible)
                {
                    this.menuStrip1.Visible = true;
                    var OnMenuKey = menuStrip1.GetType().GetMethod("OnMenuKey",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);
                    OnMenuKey.Invoke(this.menuStrip1, null);
                }
                else
                {
                    this.menuStrip1.Visible = false;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
