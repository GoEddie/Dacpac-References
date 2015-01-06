using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GOEddie.Dacpac.References
{
    public class HeaderWriter
    {
        private readonly string _dacpacPath;
        private readonly bool _autoCommitEveryOperation;
        private DacHacXml _dac;
        private string _xml;
        public HeaderWriter(string dacpacPath, bool autoCommitEveryOperation = true)
        {
            _dacpacPath = dacpacPath;
            _autoCommitEveryOperation = autoCommitEveryOperation;
            _dac = new DacHacXml(_dacpacPath);
            _xml = _dac.GetXml("Model.xml"); //.Replace(" xmlns=", " NOTx=");
        }

        public void AddCustomData(CustomData data)
        {
            AddCustomData(data.Category, data.Type, data.Items);
        }


        private void AddCustomData(string categoryName, string type, IEnumerable<Metadata> metadatas)
        {
            var reader = XmlReader.Create(new StringReader(_xml));
            XElement element = XElement.Load(reader);
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("p", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");

            var ns = XNamespace.Get("http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");

            var header = element.XPathSelectElement("//p:Header", namespaceManager);

            var newNode = new XElement(ns + "CustomData");
            newNode.SetAttributeValue("Category", categoryName);
            if(!string.IsNullOrEmpty(type))
                newNode.SetAttributeValue("Type", type);

            header.Add(newNode);

            foreach (var meta in metadatas)
            {
                var newMetaNode = new XElement(ns + "Metadata");
                newMetaNode.SetAttributeValue("Name", meta.Name);
                newMetaNode.SetAttributeValue("Value", meta.Value);
                newNode.Add(newMetaNode);
            }

            _xml = element.ToString(SaveOptions.None);

            if (_autoCommitEveryOperation)
                CommitChanges();
        }

        public void DeleteCustomData(CustomData data)
        {
            var reader = XmlReader.Create(new StringReader(_xml));
            XElement element = XElement.Load(reader);
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("p", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");
            
            var nodes = element.XPathSelectElements(string.Format("//p:Header/p:CustomData[@Category='{0}' and @Type='{1}']", data.Category, data.Type), namespaceManager);
            XElement nodeToRemove = null;
            foreach (var node in nodes)
            {
                bool isMatch = true;

                foreach (var item in data.Items)
                {
                    var child = node.XPathSelectElement(string.Format("//p:Metadata[@Name='{0}' and @Value='{1}']", item.Name, item.Value ), namespaceManager);
                    if (null == child)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    nodeToRemove = node;
                    break;
                }
            }
            
            if(nodeToRemove != null)
                nodeToRemove.Remove();

            _xml = element.ToString(SaveOptions.OmitDuplicateNamespaces);

            if (_autoCommitEveryOperation)
                CommitChanges();
            
        }

        public void CommitChanges()
        {
            _dac.SetXml("Model.xml",_xml);
        }

        public void Close()
        {
            _dac.Close();
        }
    }
}