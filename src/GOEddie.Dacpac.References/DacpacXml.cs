using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Packaging;

namespace GOEddie.Dacpac.References
{
    class DacHacXml
    {
        private readonly string _dacPath;
        private Package _package;

        public DacHacXml(string dacPath)
        {
            _dacPath = dacPath;
            _package = Package.Open(dacPath, FileMode.Open, FileAccess.ReadWrite);
        }

        public string GetXml(string fileName)
        {
            var part = _package.GetPart(new Uri(string.Format("/{0}", fileName), UriKind.Relative));
            var stream = part.GetStream();
            
            return new StreamReader(stream).ReadToEnd();
        }

        public void SetXml(string fileName, string xml)
        {
            var part = _package.GetPart(new Uri(string.Format("/{0}", fileName), UriKind.Relative));
            var stream = part.GetStream();
            
            var bytes = ASCIIEncoding.ASCII.GetBytes(xml);
            stream.SetLength(bytes.Length);
            stream.Write(bytes, 0, bytes.Length);

            _package.Close();
            _package = Package.Open(_dacPath, FileMode.Open, FileAccess.ReadWrite);
        }

        public void Close()
        {
            _package.Close();
        }
    }
}
