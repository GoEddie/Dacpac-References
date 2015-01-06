using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GOEddie.Dacpac.References
{
    public class HeaderParser
    {
        private readonly string _dacPacPath;

        public HeaderParser(string dacPacPath)
        {
            _dacPacPath = dacPacPath;
        }

        public List<CustomData> GetCustomData()
        {
            var dac = new DacHacXml(_dacPacPath);
            var xml = dac.GetXml("Model.xml");

            var reader = XmlReader.Create(new StringReader(xml));
            reader.MoveToContent();

            var data = new List<CustomData>();
            CustomData currentCustomData = null;

            while (reader.Read())
            {

                if (reader.Name == "CustomData" && reader.NodeType == XmlNodeType.Element)
                {
                    var cat = reader.GetAttribute("Category");
                    var type = reader.GetAttribute("Type");

                    currentCustomData = new CustomData(cat, type);
                    data.Add(currentCustomData);
                }

                if (reader.Name == "Metadata" && reader.NodeType == XmlNodeType.Element)
                {
                    var name = reader.GetAttribute("Name");
                    var value = reader.GetAttribute("Value");

                    currentCustomData.AddMetadata(name, value);
                }

                if (reader.Name == "Header" && reader.NodeType == XmlNodeType.EndElement)
                {
                    break;  //gone too far
                }
            }
            return data;
        }
    }

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

        public void DeleteCustomData(string categoryName)
        {
            var reader = XmlReader.Create(new StringReader(_xml));
            XElement element = XElement.Load(reader);
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("p", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02"); 
            
            
            var node = element.XPathSelectElement(string.Format("//p:Header/p:CustomData[@Category='{0}']", categoryName), namespaceManager);
            node.Remove();

            _xml = element.ToString(SaveOptions.OmitDuplicateNamespaces);

            if(_autoCommitEveryOperation)
                CommitChanges();
        }


        public void DeleteCustomMetadata(string categoryName, string type, string name)
        {
            var reader = XmlReader.Create(new StringReader(_xml));
            XElement element = XElement.Load(reader);
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("p", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");

            if (!string.IsNullOrEmpty(type))
            {
                var node = element.XPathSelectElement(string.Format("//p:Header/p:CustomData[@Category='{0}' and @Type='{1}']/p:Metadata[@Name='{2}']", categoryName, type, name), namespaceManager);
                if (node == null)
                    return;

                node.Remove();
            }
            else
            {
                var node = element.XPathSelectElement(string.Format("//p:Header/p:CustomData[@Category='{0}']/p:Metadata[@Name='{1}']", categoryName, name), namespaceManager);
                if (node == null)
                    return;

                node.Remove();
            }
            
            _xml = element.ToString(SaveOptions.OmitDuplicateNamespaces);

            if (_autoCommitEveryOperation)
                CommitChanges();
        }


        public void AddCustomData(string categoryName, string type)
        {
            var reader = XmlReader.Create(new StringReader(_xml));
            XElement element = XElement.Load(reader);
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("p", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");

            var ns = XNamespace.Get("http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02");

            var header = element.XPathSelectElement("//p:Header", namespaceManager);
            
            var newNode = new XElement(header + "CustomData");
            newNode.SetAttributeValue("Category", categoryName);
            if(!string.IsNullOrEmpty(type))
                newNode.SetAttributeValue("Type", type);

            header.Add(newNode);

            _xml = element.ToString(SaveOptions.None);

            if (_autoCommitEveryOperation)
                CommitChanges();
        }

       
        public void AddCustomMetadata(string categoryName, string type, string name, string value)
        {
            
        }



        public void CommitChanges()
        {
            _dac.SetXml("Model.xml",_xml);
        }
    }

}
//NOW do a header writer, it must add metadata, removemetadata and change (delete and add??)