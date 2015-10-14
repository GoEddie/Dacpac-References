using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GOEddie.Dacpac.References;
using NUnit.Framework;

namespace DacpacHeaderParser.Tests
{
    [TestFixture]
    public class ModelChecksumWriterTests
    {
        [Test]
        public void Can_Write_Correct_Checksum()
        {
            var dacpac = new DacHacXml(".\\Test.dacpac");
            var stream = dacpac.GetStream("Model.xml");

            var checksum = HashAlgorithm.Create("System.Security.Cryptography.SHA256CryptoServiceProvider").ComputeHash(stream);

            dacpac.Close();

            var writer = new ModelChecksumWriter(".\\Test.dacpac");
            writer.FixChecksum();
            
            //assert wtf??
        }

    }
}
