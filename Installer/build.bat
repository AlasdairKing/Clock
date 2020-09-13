@title WiX Build - Clock
@echo Opening WiX file and updates.xml for version.
@notepad Clock.wxs
@call notepad C:\users\alasdair\skydrive\personal\aljo\webbie\clock\updates.xml
@echo.
@echo.
@echo Copying files
@copy /Y "..\Clock\bin\Release\Clock.exe" "SourceDir\Clock.exe" 
@copy /Y "..\Clock\bin\Release\Common.Language.xml" "SourceDir\Common.Language.xml" 
@copy /Y "..\Clock\bin\Release\Clock.Help-en.rtf" "SourceDir\Clock.Help-en.rtf"
@copy /Y "..\Clock\bin\Release\Clock.Language.xml" "SourceDir\Clock.Language.xml"
@copy /Y "..\Clock\bin\Release\Clock.ico" "SourceDir\Clock.ico"
@echo Signing executable
@signtool.exe sign /sha1 605A4D6DDF8DBB97FE42475C8600A5E23B6C6230 "SourceDir\Clock.exe"
@echo Candle
@if exist Clock.wixobj del Clock.wixobj
@candle Clock.wxs -nologo -ext WixNetfxExtension -ext WixUtilExtension -ext wixTagExtension -ext WixUiExtension
@echo Light
@if exist Clock.msi del Clock.msi 
@light Clock.wixobj -ext WixUIExtension -spdb -sice:ICE91 -nologo -ext WixNetfxExtension -ext WixUtilExtension -ext wixTagExtension -ext WixUiExtension
@echo Signing MSI
@signtool.exe sign /sha1 605A4D6DDF8DBB97FE42475C8600A5E23B6C6230  "Clock.msi"
@copy Clock.msi "C:\local\installers\WebbIE 4\Clock.msi" /Y
@pause

