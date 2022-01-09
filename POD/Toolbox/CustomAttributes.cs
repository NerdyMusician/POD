using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Toolbox
{
    public class XmlSaveMode : Attribute
    {
        public XmlSaveMode(string mode)
        {
            Mode = mode;
        }

        public string Mode { get; set; }

    }

}
