using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Profiler.UI
{
    public interface IRowColumnConnector : IConnector
    {
        Label RowLabel { get; }
        Label ColumnLabel { get; }
    }
}
