call "%VS120COMNTOOLS%\VsDevCmd.bat"
msbuild
vstest.console Core.Common.Tests\bin\Debug\Core.Common.Tests.dll