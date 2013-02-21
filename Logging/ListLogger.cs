using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Qixle.iScanDuo.Controller.Logging
{
    public class ListLogger : Logger
    {
        private readonly ListView list;

        public ListLogger(ListView list)
        {
            this.list = list;
        }

        public override void Log(string message, Logger.Level level)
        {
            if (!ShouldLog(level))
                return;

            DateTime dateTime = DateTime.Now;
            ListViewItem item = new ListViewItem(new string[] { level.ToString(), dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString(), message });
            switch(level)
            {
                case Level.Error:
                    item.ForeColor = Color.Red;
                    break;
                case Level.Warning:
                    item.ForeColor = Color.DarkOrange;
                    break;
                case Level.Info:
                    item.ForeColor = Color.DarkBlue;
                    break;
                case Level.Fine:
                    item.ForeColor = Color.DarkGreen;
                    break;
            }
            if (list.InvokeRequired)
            {
                list.BeginInvoke(new AddListItemDelegate(AddListItem), item);
            }
            else
            {
                AddListItem(item);
            }
        }

        private delegate void AddListItemDelegate(ListViewItem item);
        private void AddListItem(ListViewItem item)
        {
            list.Items.Add(item);
            item.EnsureVisible();
        }
    }
}
