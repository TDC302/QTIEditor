using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interfaces
{
    public interface IManifestLinkable
    {
        
        public UniqueIdentifier id { get; }

        string type { get; }

        string href { get; }

        List<string> files { get; }

        List<IManifestLinkable>? dependencies { get; }


        public void WriteToFile(string path);

    }


}
