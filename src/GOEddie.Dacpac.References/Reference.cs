using System;
using System.Collections.Generic;
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

    public enum ReferenceType
    {
        System,
        SameDatabase,
        ExternalDatabase
    }

    public class CustomData
    {
        public readonly string Category;
        public readonly string Type;
        public List<Metadata> Items = new List<Metadata>();

        public CustomData(string category, string type)
        {
            Category = category;
            Type = type;
        }

        public void AddMetadata(string name, string value)
        {
            Items.Add(new Metadata(name, value));
        }
    }

    public class Metadata
    {
        public string Name;
        public string Value;

        public Metadata(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
