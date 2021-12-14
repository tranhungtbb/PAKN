# COMMON PATHS
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$outputFolder = "D:\PROJECTS\38.PAKN\New"
$outputFolderBU = "D:\PROJECTS\38.PAKN\Old"
$backupFolder = "D:\PROJECTS\38.PAKN\Backup\Config\"
$slnFolder = Join-Path $buildFolder "../"
$webHostFolder = Join-Path $slnFolder "PAKNAPI"
$ngFolder = Join-Path $slnFolder "PAKNWeb"
$ErrorActionPreference = "Stop"
$APPCMDPath = "C:\windows\system32\inetsrv\appcmd.exe"

# Stio IIS
# IISReset /STOP
& $APPCMDPath stop site /site.name:PAKN_API


## CLEAR ######################################################################
Remove-Item $outputFolderBU -Force -Recurse -ErrorAction Ignore
Copy-Item -Path ($outputFolder) -Destination ($outputFolderBU) -Recurse

Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory -Force
## RESTORE NUGET PACKAGES #####################################################

Set-Location $slnFolder
dotnet restore

## PUBLISH WEB HOST PROJECT ###################################################

Set-Location $webHostFolder
dotnet publish --output (Join-Path $outputFolder "Api")

## PUBLISH ANGULAR UI PROJECT #################################################

Set-Location $ngFolder
& yarn
& node --max_old_space_size=8192 node_modules/@angular/cli/bin/ng  build --prod
New-Item -ItemType Directory -Force -Path (Join-Path $outputFolder "Web")
Copy-Item (Join-Path $ngFolder "wwwroot/*") (Join-Path $outputFolder "Web/") -Recurse

## FINALIZE ###################################################################

Set-Location $outputFolder

Copy-Item -Path (Join-Path $backupFolder "web.config") -Destination (Join-Path $outputFolder "/Web") -Recurse

# Start IIS
# IISReset /START
& $APPCMDPath start site /site.name:PAKN_API


# Delete existing zip files
Set-Location $outputFolder
Del *.zip

# # Zip publish files
# $sourceZipFolder = Join-Path $outputFolder "*"
# $destZipFile = Join-Path $outputFolder "crm-publish.zip"
# Compress-Archive -U -Path $sourceZipFolder -DestinationPath $destZipFile

cd..