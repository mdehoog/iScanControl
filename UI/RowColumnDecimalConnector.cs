﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Qixle.iScanDuo.Controller.Protocol;
using System.Drawing;

namespace Qixle.iScanDuo.Controller.UI
{
    public class RowColumnDecimalConnector : DecimalConnector, IRowColumnConnector
    {
        private readonly Label columnLabel;

        public RowColumnDecimalConnector(DecimalCommand command, NumericUpDown control, Label rowLabel, Label columnLabel, CommandCategory category)
            : base(command, control, rowLabel, category)
        {
            this.columnLabel = columnLabel;
        }

        public Label RowLabel
        {
            get { return Label; }
        }

        public Label ColumnLabel
        {
            get { return columnLabel; }
        }
    }
}
