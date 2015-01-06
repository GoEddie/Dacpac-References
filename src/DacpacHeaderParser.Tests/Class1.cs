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
    public class ReadHeaderTests
    {
        [Test]
        public void Can_Read_Standard_Headers()
        {
            var parser = new HeaderParser(".\\Test.dacpac");
            var ansiNulls = parser.GetCustomData().FirstOrDefault(p => p.Category == "AnsiNulls");
            Assert.AreEqual("True", ansiNulls.Items.FirstOrDefault(p=>p.Name=="AnsiNulls").Value);
        }

        [Test]
        public void Can_Read_Reference()
        {
            var parser = new HeaderParser(".\\Test.dacpac");
            var firstReference = parser.GetCustomData().FirstOrDefault(p => p.Category == "Reference" && p.Type == "SqlSchema");
            
            var fileName = firstReference.Items.FirstOrDefault(p => p.Name == "FileName").Value;
            var logicalName = firstReference.Items.FirstOrDefault(p => p.Name == "LogicalName").Value;
            var externalParts = firstReference.Items.FirstOrDefault(p => p.Name == "ExternalParts").Value;
            var suppressMissingDependenciesErrors = firstReference.Items.FirstOrDefault(p => p.Name == "SuppressMissingDependenciesErrors").Value;

            Console.WriteLine("DacPac Reference: {0}: {1}: {2}: {3}", fileName, logicalName, externalParts, suppressMissingDependenciesErrors);
        }
    }

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

            var writer = new HeaderWriter(".\\Test.dacpac");
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

            var writer = new HeaderWriter(".\\Test.dacpac");
            writer.AddCustomData(newCustomData);
            writer.Close();

            var actualItem = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "Reference" && p.Type == "SqlSchema" &&
                        p.Items.Any(item => item.Name == "FileName" && item.Value == fileName));

            writer = new HeaderWriter(".\\Test.dacpac");
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
