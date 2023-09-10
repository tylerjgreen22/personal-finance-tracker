using System.Text.Json;
using System.IO;

// Check if an argument is supplied and inform user if one is not and what to supply
if (Args.Count < 1)
{
    Console.WriteLine("Please provide either a category or an interval using the --category, -ca, --interval, or -in flags followed by the category or interval you want to filter by. Ex: --category Groceries or --interval 30d");
}

// Local variables for the arguments
string category = null;
string interval = null;

// Parse through the arguments and determine if the appropriate flags are present
for (int i = 0; i < Args.Count; i++)
{
    // Args array
    string arg = Args[i];

    // Check if ca or category flag is present. If present, set category if category is in bounds
    if (arg == "-ca" || arg == "--category")
    {
        if (i + 1 < Args.Count)
        {
            category = Args[i + 1];
        }
    }

    // Check if in or interval flag is present. If present, set interval if interval is in bounds
    if (arg == "-in" || arg == "--interval")
    {
        if (i + 1 < Args.Count)
        {
            interval = Args[i + 1];
        }
    }
}

// If neither a category nor an interval was supplied, return and inform user of proper use
if (category == null && interval == null)
{
    System.Console.WriteLine("Please provide either a category or an interval using the --category, -ca, --interval, or -in flags followed by the category or interval you want to filter by. Ex: --category Groceries or --interval 30d");
    return;
}

// If the ledger file exists, read from the json from the file, otherwise set json value to be empty
string json = File.Exists("GeneralLedger.json") ? File.ReadAllText("GeneralLedger.json") : "";

// If the ledger file is not found or is empty, inform user that transactions first need to be written to ledger
if (string.IsNullOrEmpty(json.Trim()))
{
    Console.WriteLine("Ledger is empty! Use WriteLedger.csx to write some transactions to the ledger before reading");
    return;
}

// Deserialize the list of transactions from the ledger and set it to a local list of transactions
// Set up a list of transactions to hold the filtered transactions
// Intialize a total variable to track the total price of filtered transactions
List<Transaction> ledger = JsonSerializer.Deserialize<List<Transaction>>(json);
List<Transaction> transactions = new List<Transaction>();
double total = 0;

// If the category flag is present, filter the ledger based on the supplied category
if (!string.IsNullOrEmpty(category))
{
    // Use LINQ to filter the ledger where the supplied category is equal to the category for each transaction in the ledger
    transactions = ledger.Where(c => c.Category.ToLower() == category.ToLower()).ToList();

    // If transactions were found, print a summary of all found transactions. Otherwise print that no transactions were found
    if (transactions.Count > 0)
    {
        Console.WriteLine($"Summary of all {category} transactions: ");

        // For each transaction found, print the summary of the transaction, with the price formatted to 2 decimal places as well as adding up the total
        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine($"Name: {transaction.Name}, Price: {transaction.Price.ToString("F2")}, Date and Time: {transaction.TimeStamp}");
            total += transaction.Price;
        }

        // Round the total and display the total spent for that category
        total = Math.Round(total, 2);
        Console.WriteLine($"Total spent on {category}: {total}");
    }
    else
    {
        System.Console.WriteLine("No transactions found for that category!");
    }

    return;
}

// If the interval flag is present, filter the ledger based on the supplied interval
if (!string.IsNullOrEmpty(interval))
{
    // Split the supplied interval on the last character to get the amount of time to subtract
    string[] splitInterval = interval.Split(interval[^1]);

    // Get the current date
    DateTime startDate = DateTime.Now;

    // Parse the string of the amount of time to subtract into an int. If the value supplied is negative or is not able to be parsed into an int, return and inform the user
    int amountToSubtract = 0;
    if (!int.TryParse(splitInterval[0], out amountToSubtract) || amountToSubtract < 0)
    {
        System.Console.WriteLine("Invalid interval, interval must be non-negative and a whole integer value");
        return;
    }

    // Check the last char of the supplied interval. If it is d, subtract days, m, subtract months or y, subtract years from the current date
    switch (interval[^1])
    {
        case 'd':
            startDate = DateTime.Now.AddDays(-amountToSubtract);
            break;
        case 'm':
            startDate = DateTime.Now.AddMonths(-amountToSubtract);
            break;
        case 'y':
            startDate = DateTime.Now.AddYears(-amountToSubtract);
            break;
    }

    // Use LINQ to filter the ledger where the dates of the transactions in the ledger are greater than the date of the interval supplied
    // Otherwise, display that no transactions were found for that interval
    transactions = ledger.Where(t => t.TimeStamp >= startDate).ToList();

    // If transactions were found, print a summary of all found transactions. Otherwise print that no transactions were found
    if (transactions.Count > 0)
    {
        Console.WriteLine($"Summary of all transactions for the last {interval}");

        // For each transaction found, print the summary of the transaction, with the price formatted to 2 decimal places as well as adding up the total
        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine($"Name: {transaction.Name}, Category: {transaction.Category} Price: {transaction.Price.ToString("F2")}, Date and Time: {transaction.TimeStamp}");
            total += transaction.Price;
        }

        // Round the total and display the total spent for that interval
        total = Math.Round(total, 2);
        Console.WriteLine($"Total spent for the last {interval}: {total}");
    }
    else
    {
        System.Console.WriteLine("No transactions found for that interval!");
    }

    return;
}

// Class for representing the transaction to read from the ledger
private class Transaction
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
    public DateTime TimeStamp { get; set; }
}