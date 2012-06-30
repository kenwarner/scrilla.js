REM Set the following user name and password
set appName=scrilla.Web
set appPoolName=scrilla.Web
set appDirectory=D:\projects\scrilla\scrilla.Web
set appUrl=http://scrilla:80
REM make sure to add an entry for scrilla to the HOSTS file

REM create directory
mkdir %appDirectory%

REM Clean up any old websites and apppools
%systemroot%\system32\inetsrv\appcmd delete site /site.name:"%appName%"
%systemroot%\system32\inetsrv\appcmd delete apppool /apppool.name:%appPoolName%

REM Create new apppool
%systemroot%\system32\inetsrv\appcmd add apppool /apppool.name:%appPoolName% /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated /enable32BitAppOnWin64:false

REM Create website with apppool
%systemroot%\system32\inetsrv\appcmd add site /site.name:%appName% /physicalPath:%appDirectory% /bindings:%appUrl%
%systemroot%\system32\inetsrv\appcmd set site /site.name:%appName% /[path='/'].applicationPool:%appPoolName%
iisreset 

REM set directory permissions and clear readonly flag
REM icacls %appDirectory% /T /grant "IIS AppPool\%appPoolName%":(OI)(CI)F