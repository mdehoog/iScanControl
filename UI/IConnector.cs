using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Qixle.iScanDuo.Controller.Protocol;

namespace Qixle.iScanDuo.Controller.UI
{
    public enum CommandCategory
    {
        Input,
        AspectRatio,
        Picture,
        Output,
        Configuration,
        CMS,
        Remote,
        Other
    }

    public interface IConnector
    {
        void QueryValue();
        ICommand ICommand { get; }
        Control Control { get; }
        Panel Panel { get; }
        Label Label { get; }
        CommandCategory Category { get; }

        int DelayAfterSet { get; set; }
        void AddDependantConnector(IConnector dependant);
        IList<IConnector> DependantConnectors();
        string CurrentControlStringValue();
        void SetControlStringValue(string value);
    }
}
