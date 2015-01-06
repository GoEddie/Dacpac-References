using System.Collections.Generic;

namespace GOEddie.Dacpac.References
{
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
}