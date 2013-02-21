using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Qixle.iScanDuo.Controller.UI
{
    public interface IRowColumnConnector : IConnector
    {
        Label RowLabel { get; }
        Label ColumnLabel { get; }
    }
}
