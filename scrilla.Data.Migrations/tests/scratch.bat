sqlcmd -E -S localhost -i dropDb.sql
sqlcmd -E -S localhost -i createDb.sql
"..\..\packages\FluentMigrator.1.1.1.0\tools\Migrate.exe" /connection "Server=localhost;Database=scrilla.js.tests;Trusted_Connection=true" /provider sqlserver2008 /assembly ..\bin\Debug\scrilla.Data.Migrations.dll /verbose true