using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOEddie.Dacpac.References;

namespace TheReferencer
{
    class Program
    {
        static void Main(string[] args)
        {
            var editor = new HeaderWriter(args[0]);
            editor.AddCustomData("Edd", null);
            editor.DeleteCustomMetadata("Reference", "SqlSchema", "FileName");
            editor.DeleteCustomData("QuotedIdentifier");
            

            var parser = new HeaderParser(args[0]);

            foreach (var cd in parser.GetCustomData())
            {
                Console.WriteLine("Data: {0} - {1}", cd.Category, cd.Type);
                foreach (var item in cd.Items)
                {
                    Console.WriteLine("Item: {0} - {1}", item.Name, item.Value);
                }                
            }
        }
    }
}
