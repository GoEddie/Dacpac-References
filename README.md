#Dacpac-References
=================

##Parse and Update Database References in Dacpac files

References are stored in the model.xml file inside the dacpac which follows the microsoft standard zip format for files.

###The Header Xml looks something like:

DataSchemaModel
	Header
		CustomData
			Metadata
			
###Each Reference is a CustomData with 4 Metadata items:

	FileName, LogicalName, ExternalParts and SuppressMissingDependenciesErrors
	

###I have built the same structure in classes and the top level objects are:

	HeaderParser
	HeaderWriter
	
	parser.GetCustomData() Gets all the CustomData, which you can then either iterate through or use linq to get the reference you want:
	
```
	var fileName = "path\to\dacpac.dacpac";

	var reference = parser.GetCustomData()
                .Where(
                    p =>
                        p.Category == "Reference" && p.Type == "SqlSchema" &&
                        p.Items.Any(item => item.Name == "FileName" && item.Value == fileName));
```
		
	To write or delete a reference you create a CustomData and add the 4 Metadata items to it (this could be pretty easily extended to do this easier) - you then pass the CustomData to the HeaderWriter
	
	If you have a large number of changes to the dacpac then you can pass autoCommitEveryOperation = false to the constructor and call .CommitChanges().
	
	One thing to note is that the HeaderWriter keeps a handle to the dacpac open, so when you are done call .Close - it should obviously implement IDisposable (whoops)
		
		

