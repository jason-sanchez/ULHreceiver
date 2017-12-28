Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("KY1ProdReceiver")> 
<Assembly: AssemblyDescription("20140910-save problem files.Added 10ms delay as testrcvr_2. 1957 changed buffer to 1024 as testRcvr_3.20140913 - resized startup screen size. 20140920 - receiver for CSCITWLISTEN from current production version.'20140924 - log record count on feed bounce. Increased outfile records to 900000. Log receiver start time. 20140925 - change timer to 4 minutes. 20140928 - put location variable in Recieve Messages to find where error is actually occuring. Process the problem file in addition to saving in problems directory. 20140929 - added more location variables 20140930 - code to handle out of bounds error at location 12 and trim the text.'20141001 - Receive Messages - if first character is CR, strip it off.'20151006 - added text showing program start;removed problem file creation;changed ack to wave3_prod vice test. 20151221 - changed ACK to KY1 _Prod. 20160105 - renamed KY1ProdReceiver.")> 
<Assembly: AssemblyCompany("")> 
<Assembly: AssemblyProduct("KY1 Production Receiver")> 
<Assembly: AssemblyCopyright("")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("a30b8219-3c5d-4881-83fb-21cca791c544")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("2016.01.05.0")> 
