using System;
using System.Linq;
using GOEddie.Dacpac.References;
using NUnit.Framework;

namespace DacpacHeaderParser.Tests
{
    [TestFixture]
    public class WriteHeaderTests
    {
        [Test]
        public void Can_Add_Reference()
        {
            var parser = new HeaderParser(".\\Test.dacpac");
            string fileName = string.Format("c:\\bloonblah{0}.dacpac", Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", ""));
            const string logicalName = "blooblah.dacpac";
            const string externalParts = "[$(blooblah)]";
            const string suppressMissingDependenciesErrors = "False";

            var newCustomData = new CustomData("Reference", "SqlSchema");
            newCustomData.AddMetadata("FileName", fileName);
            newCustomData.AddMetadata("LogicalName", logicalName);
            newCustomData.AddMetadata("ExternalParts", externalParts);
            newCustomData.AddMetadata("SupressMissingDependenciesErrors", suppressMissingDependenciesErrors);

            var writer = new HeaderWriter(".\\Test.dacpac", new DacHacFactory());
            writer.AddCustomData(newCustomData);
            writer.Close();

            var actualItem = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "Reference" && p.Type == "SqlSchema" &&
                        p.Items.Any(item => item.Name == "FileName" && item.Value == fileName));

            Assert.IsNotNull(actualItem);

        }

        [Test]
        public void Adding_Reference_Also_Creates_SqlCmdVar()
        {
            var parser = new HeaderParser(".\\Test.dacpac");
            string fileName = string.Format("c:\\bloonblah{0}.dacpac", Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", ""));
            const string logicalName = "blooblah.dacpac";
            const string externalParts = "[$(blooblah)]";
            const string suppressMissingDependenciesErrors = "False";

            var newCustomData = new CustomData("Reference", "SqlSchema");
            newCustomData.AddMetadata("FileName", fileName);
            newCustomData.AddMetadata("LogicalName", logicalName);
            newCustomData.AddMetadata("ExternalParts", externalParts);
            newCustomData.AddMetadata("SupressMissingDependenciesErrors", suppressMissingDependenciesErrors);
            newCustomData.RequiredSqlCmdVars.Add("BloopyBloo");

            var writer = new HeaderWriter(".\\Test.dacpac", new DacHacFactory());
            writer.AddCustomData(newCustomData);
            writer.Close();

            var actualItem = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "SqlCmdVariables" && p.Type == "SqlCmdVariable" &&
                        p.Items.Any(item => item.Name == "BloopyBloo"));

            Assert.IsNotNull(actualItem);

        }

        [Test]
        public void Can_Delete_Reference()
        {
            var parser = new HeaderParser(".\\Test.dacpac");
            string fileName = string.Format("c:\\bloonblah{0}.dacpac", Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", ""));
            const string logicalName = "blooblah.dacpac";
            const string externalParts = "[$(blooblah)]";
            const string suppressMissingDependenciesErrors = "False";

            var newCustomData = new CustomData("Reference", "SqlSchema");
            newCustomData.AddMetadata("FileName", fileName);
            newCustomData.AddMetadata("LogicalName", logicalName);
            newCustomData.AddMetadata("ExternalParts", externalParts);
            newCustomData.AddMetadata("SupressMissingDependenciesErrors", suppressMissingDependenciesErrors);

            var writer = new HeaderWriter(".\\Test.dacpac", new DacHacFactory());
            writer.AddCustomData(newCustomData);
            writer.Close();

            var actualItem = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "Reference" && p.Type == "SqlSchema" &&
                        p.Items.Any(item => item.Name == "FileName" && item.Value == fileName));

            writer = new HeaderWriter(".\\Test.dacpac", new DacHacFactory());
            writer.DeleteCustomData(newCustomData);
            writer.Close();

            actualItem = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "Reference" && p.Type == "SqlSchema" &&
                        p.Items.Any(item => item.Name == "FileName" && item.Value == fileName));

            Assert.IsNotNull(actualItem);
        }

    }
}