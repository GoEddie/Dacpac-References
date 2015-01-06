using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOEddie.Dacpac.References
{
    public class Reference
    {
        public string Path;
        public string LogicalName;
        public string ExternalParts;
        public bool SupresExternalMissingDependencyErrors;

        public ReferenceType Type;
    }
}
