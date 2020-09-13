@title Windows Installer using WiX
@echo Opening WiX file for version.
@call notepad Clock.wxs
@echo Copying files
@path=%path%;C:\Program Files (x86)\WiX Toolset v3.7\bin
@if exist clock.wixobj del clock.wixobj
@candle clock.wxs
@if exist clock.msi del clock.msi 
@light clock.wixobj -ext WixUIExtension -spdb 
@signtool.exe sign /sha1 605A4D6DDF8DBB97FE42475C8600A5E23B6C6230  clock.msi
@echo Copying files to the local FTP folder.
@if exist C:\users\alasdair\documents\personal\aljo\webbie\Clock\Clock.msi del C:\users\alasdair\documents\personal\aljo\webbie\Clock\Clock.msi
@echo Copying clock.msi
@copy clock.msi C:\users\alasdair\documents\personal\aljo\webbie\Clock\clock.msi /Y
@if exist clock.msi del clock.msi
@echo All done. Files are in the web folder ready for upload. 
@pause

