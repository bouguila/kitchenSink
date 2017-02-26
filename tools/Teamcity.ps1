Param ($checkoutdir = "%teamcity.build.checkoutDir%")

$StarCounterDir = "$checkoutdir\sc"
$StarCounterWorkDirPath = "$StarCounterDir\starcounter-workdir"
$StarCounterRepoPath = "$StarCounterWorkDirPath\personal"
$StarCounterConfigPath = "$StarCounterDir\Configuration"

$KitchenSinkWwwPath = "$checkoutdir\KitchenSink\src\KitchenSink\wwwroot"
$KitchenSinkExePath = "$checkoutdir\KitchenSink\bin\Debug\KitchenSink.exe"
$KitchenSinkTestsPath = "$checkoutdir\KitchenSink\test\KitchenSink.Tests\bin\Debug\KitchenSink.Tests.dll"

#create repo
Function createRepo($wdir, $scpath)
{
	$process = Start-Process -FilePath $scpath\star.exe -ArgumentList "`@`@createrepo $wdir" -PassThru -Wait
	return $process.ExitCode
}

#create xml file (personal.xml) in Configuration folder
Function createXML($repoPath, $configPath)
{
	$fileContent = "<?xml version=`"1.0`" encoding=`"UTF-8`"?>
<service><server-dir>$repoPath</server-dir></service>"
	
	New-Item -Path $configPath -Name personal.xml -ItemType "file" -force -Value $fileContent | Out-Null
	return Test-Path $configPath\personal.xml
}

#start KitchenSink app
Function startKitchenSink($scpath, $wwwPath, $exePath)
{
	$process = Start-Process -FilePath $scpath\star.exe -ArgumentList "--d=kitchensink --resourcedir=$wwwPath $exePath" -PassThru -NoNewWindow
	return $process.Id
}

#run KitchenSink tests
Function runTests($testPath)
{
	$process = Start-Process -FilePath "$checkoutdir\KitchenSink\packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe" -ArgumentList "$testPath --noheader --teamcity --params Browsers=Chrome,Firefox" -PassThru -NoNewWindow -Wait
	return $process.ExitCode
}

#kill StarCounter server
Function killStarcounter($scpath)
{
	$process = Start-Process -FilePath "$scpath/staradmin.exe" -ArgumentList "kill all" -PassThru -Wait
	return $process.ExitCode
}

try 
{
	$createRepoExitCode = createRepo -wdir $StarCounterWorkDirPath -scpath $StarCounterDir
	if ($createRepoExitCode -eq 0)
	{
		$createXMLExitCode = createXML -repoPath $StarCounterRepoPath -configPath $StarCounterConfigPath
		if ($createXMLExitCode)
		{ 
			$id = startKitchenSink -scpath $StarCounterDir -wwwPath $KitchenSinkWwwPath -exePath $KitchenSinkExePath 
			wait-process -id $id
			$testExitCode = runTests -testPath $KitchenSinkTestsPath
			if($testExitCode -ge 0)
			{
				$killingExitCode = killStarcounter -scpath $StarCounterDir
				if($killingExitCode -eq 0) { exit(0) }
				else { exit(1) }
			}
			else { exit(1) }
		}
		else { exit(1) }
	}
	else { exit(1) }
} 
Catch 
{
	$ErrorMessage = $_.Exception.Message
	Write-Output $ErrorMessage
	exit(1)
}