Scrilla.js is a fork of https://github.com/qntmfred/scrilla

Scrilla is a web application I made to solve a problem I had:  
- I love how mint.com pulls down (when it's working) all my financial transaction data  
- But I don't love how mint.com makes me use that data - I have a particular way of maintaining a budget for my household and a particular set of views into my financial life that I like to be able to see.

It was also a good opportunity to explore a few technologies:
- ASP.NET MVC 4
- Javascript MVC frameworks
- FluentMigrator
- Entity Framework Code First
- Dapper

To use locally:
- Add `127.0.0.1  scrilla.js` to your C:\Windows\System32\drivers\etc\hosts file to enable local access from http://scrilla.js/
- Run `\CreateScrillaWebsite.bat` to create the IIS site and app pool
- After building the solution, run `\scrilla.Data.Migrations\dev\scratch.bat` to create the local database along with permissions for IIS AppPool\scrilla.Web
- Log in to your mint.com account and then go to https://wwws.mint.com/transactionDownload.event to initiate a full csv download of your account transactions
- Navigate to http://scrilla.js/ and upload your transaction file
- Do budget stuff!
