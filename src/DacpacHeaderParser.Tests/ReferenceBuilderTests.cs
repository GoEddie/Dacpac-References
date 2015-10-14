using System.Linq;
using GOEddie.Dacpac.References;
using NUnit.Framework;

namespace DacpacHeaderParser.Tests
{
    [TestFixture]
    public class ReferenceBuilderTests
    {
        [Test]
        public void ThisDatabase_Reference_Has_FileName_And_LogicalName()
        {
            var builder = new ReferenceBuilder();
            var customData = builder.BuildThisDatabaseReference("filename", "logicalName");
            Assert.AreEqual(2, customData.Items.Count);
            Assert.IsNotNull(customData.Items.FirstOrDefault(p=>p.Name == "FileName" && p.Value =="filename"));
            Assert.IsNotNull(customData.Items.FirstOrDefault(p => p.Name == "LogicalName" && p.Value == "logicalName"));
        }

        [Test]
        public void OtherDatabase_Reference_Has_FileName_And_LogicalName_And_ExternalParts_And_SuppressMissingDependenciesErrors()
        {
            var builder = new ReferenceBuilder();
            const string expected = "bleurgh";
            var customData = builder.BuildOtherDatabaseReference(expected, "filename", "logicalName");
            Assert.AreEqual(4, customData.Items.Count);
            Assert.IsNotNull(customData.Items.FirstOrDefault(p => p.Name == "FileName" && p.Value == "filename"));
            Assert.IsNotNull(customData.Items.FirstOrDefault(p => p.Name == "LogicalName" && p.Value == "logicalName"));
            Assert.IsNotNull(customData.RequiredSqlCmdVars.Any(p=>p==expected));
        }

    }
}