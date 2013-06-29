
** TODO **
* how do i know what categories the money in the main account is earmarked for
* how do we add fake transactions to represent savings category transfers
* I need a better way to be able to see the bottom line for Primary Checking and also Food budget, blow budget, etc
* put totals at the bottom of /Transactions (maybe a partial view of /Categories)
* Graphs

* Need to write unit tests for all this stuff
* Need to refactor so ALL data is created in the controller

* what does it mean when a bill transaction is paid but the matching account transaction doesn't have the same amount
* includeTransfers not included in /Categories th links

* /Budget?month=2011-11-20 doesn't work
* table headers don't all have same color
* PartialViews /Vendor/25 create links with accountId= instead of just leaving that part off
* rename Transaction -> AccountTransaction
* All service methods should do proper error checking
* All controller actions that call service methods should do error processing
* figure out how to handle difference between http://scrilla.js/Budget and
*  http://scrilla.js/Categories?from=2010-09-01&to=2011-09-30 (hint: /Budget only sums for the primary account)
* change .not() to :not()
