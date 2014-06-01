REM move to location of this file
pushd "%~dp0"

mkdir trash
robocopy trash .\scrilla.js.Web\node_modules /s /mir > nul
rmdir .\scrilla.js.Web\node_modules

robocopy trash .\scrilla.js.Web\bower_components /s /mir > nul
rmdir .\scrilla.js.Web\bower_components

robocopy trash .\scrilla.js.Web\lib /s /mir > nul
rmdir .\scrilla.js.Web\lib
rmdir .\trash

REM move back to cwd where this file was executed from
popd