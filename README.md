# tartool
A simple windows command line tool 

- to uncompress and untar .tar.gz (.tgz) files 
or
-just untar .tar files

C:\>TarTool.exe
Usage : 
C:\>TarTool.exe sourceFile destinationDirectory

C:\>TarTool.exe D:\sample.tar.gz ./

C:\>TarTool.exe sample.tgz temp

C:\>TarTool.exe -x sample.tar temp

More details are on this post --https://rajasekharan.com/blog/tartool-windows-tar-gzip-tgz-extraction-tool.html

TarTool 2.0 Beta supports bzip2 decompression for files with extensions like tar.bz2 and .bz2.

TarTool -xj sample.tar.bz2 temp
or
TarTool -j sample.bz2

[http://tartool.codeplex.com/releases/view/85391](https://web.archive.org/web/20171011031701/http://tartool.codeplex.com/releases/view/85391)

Note:

TarTool uses SharpZipLib 

See licensing terms of SharpZipLib before reusing this code in one of your projects.
http://www.icsharpcode.net/OpenSource/SharpZipLib/
