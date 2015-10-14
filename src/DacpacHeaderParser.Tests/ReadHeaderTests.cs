using System;
using System.Linq;
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
}