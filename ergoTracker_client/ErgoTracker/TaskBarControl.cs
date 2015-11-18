using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    class TaskBarControl : IDisposable
    {
        NotifyIcon _icon;
        MyKinect kinect;

        public TaskBarControl(MyKinect _kinect)
        {
            _icon = new NotifyIcon();
            kinect = _kinect;
        }

        public void Display()
        {
            _icon.MouseClick += new MouseEventHandler(Icon_MouseClick);
            _icon.Icon = new Icon("Icon.ico");
            _icon.Text = "Demo";
            _icon.Visible = true;

            _icon.ContextMenuStrip = new ContextMenus().Create(kinect);
        }

        public void Dispose()
        {
            _icon.Dispose();
        }

        private void Icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _icon.ContextMenuStrip.Show(new Point(Cursor.Position.X - _icon.ContextMenuStrip.Width,
                                                        Cursor.Position.Y - _icon.ContextMenuStrip.Height));
            }
        }
    }
}
