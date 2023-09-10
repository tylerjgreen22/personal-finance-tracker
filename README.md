# Personal Finance Tracker Coding Challenge

This is a personal finance tracker created as a submission to a coding challenge provided by Taylor Lamar: https://github.com/lamar-software/coding-challenges/tree/master/challenges/personal-finance-tracker

## How to use this application

This application consists of two scripts, a WriteLedger and a ReadLedger script. A dotnet runtime and script running tool like dotnet scripts is needed to run the scripts.

### WriteLedger

Write ledger can be run by providing three command line arguments to the script, the transaction name, category and price. The name and category are strings and the price is a double.

Example: dotnet script WriteLedger.csx "Chicken Breast" "Groceries" 5.99 <br>

This script will create a GeneralLedger.json file and write the transaction to the ledger, if the file does not already exist

### ReadLedger

Read ledger can be run by providing a flag, either Category (--category or -ca) or Interval (--interval or -in). Both values are strings. The challenge instructions said to use -c or -i but those seem to be reserved flags in dotnet scripts

Examples: 

dotnet script ReadLedger.csx --category "Groceries" <br>
dotnet script ReadLedger.csx --interval 30d <br>

This script will return a summary of the transactions from the ledger, provided it exists and is not empty, matching the supplied flag as well as the total amount spent on these transactions. Three types of values can be supplied for interval, either days, months or years

Examples: 

dotnet script ReadLedger.csx --interval 3y <br>           
Summary of all transactions for the last 3y <br>
Name: Movies, Category: Entertainment Price: 26.79, Date and Time: 9/10/2023 3:20:37 PM <br>
Name: Chicken Breast, Category: Groceries Price: 5.68, Date and Time: 9/10/2023 3:20:49 PM <br>
Name: Milk, Category: Groceries Price: 3.99, Date and Time: 9/10/2023 3:20:59 PM <br>
Name: Oil, Category: Automotive Price: 33.99, Date and Time: 8/10/2023 3:20:59 PM <br>
Name: Tires, Category: Automotive Price: 200.00, Date and Time: 9/10/2021 3:20:59 PM <br>
Total spent for the last 3y: 270.45

dotnet script ReadLedger.csx --category Groceries <br>
Summary of all Groceries transactions: <br>
Name: Chicken Breast, Price: 5.68, Date and Time: 9/10/2023 3:20:49 PM <br>
Name: Milk, Price: 3.99, Date and Time: 9/10/2023 3:20:59 PM <br>
Total spent on Groceries: 9.67 <br>

## Take away

Overall this challenge was quite fun, it was my first time writing scripts in C# and I learned a lot about how to write scripts and how to handle command line arguments with scripts.
I also learned how to work with dates better, as I had not previously worked much dates beyond just as created at timestamps
This challenge took me around 3 hours to complete, however a lot of that time was just figuring out how to run scripts with dotnet and C# using dotnet script and the csx file extension
