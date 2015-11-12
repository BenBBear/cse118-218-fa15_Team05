using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            /*
             * This tool strip menu item will allow you to choose
             * what mode you want to be in. "Tutorial" or "Capturing"
             */
            item = new ToolStripMenuItem();
            item.Text = "Modes";
            item.Click += new EventHandler(Modes_Click);
            menu.Items.Add(item);

            /*
             * This tool strip menu itme will allow you to 
             */
            item = new ToolStripMenuItem();
            item.Text = "Review Personal Data";
            item.Click += new EventHandler(Data_Review_Click);
            menu.Items.Add(item);

            // separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            /*
             * This tool strip menu item will allow you to log off
             * and exit the program.
             */
            item = new ToolStripMenuItem();
            item.Text = "Log off";
            item.Click += new EventHandler(Log_Off_Click);
            menu.Items.Add(item);
            return menu;
        }

        void Modes_Click(object sender, EventArgs e)
        {
            // do nothing for now
        }

        void Data_Review_Click(object sender, EventArgs e)
        {
            // do nothing for now
        }

        void Log_Off_Click(object sender, EventArgs e)
        {
            // just exit program for now.
            Application.Exit();
        }
    }
}
