@echo off

START /W packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe test\KitchenSink.Tests\bin\Debug\KitchenSink.Tests.dll --noheader --params Browsers=Chrome,Firefox