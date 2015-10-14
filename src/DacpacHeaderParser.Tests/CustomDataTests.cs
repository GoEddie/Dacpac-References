using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOEddie.Dacpac.References;
using NUnit.Framework;

namespace DacpacHeaderParser.Tests
{
    [TestFixture]
    public class CustomDataTests
    {

        [Test]
        public void existing_metadata_items_are_overwritten()
        {
            const string expected = "111";
            
            var cd = new CustomData("a", "b");
            cd.AddMetadata("d", "e");
            cd.AddMetadata("d", expected);

            Assert.AreEqual(1, cd.Items.Count);
            Assert.AreEqual(expected, cd.Items.First().Value);
        }
    }
}
