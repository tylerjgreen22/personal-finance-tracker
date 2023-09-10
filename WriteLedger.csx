using System.Text.Json;
using System.IO;

// Checking that all three arguments: name, category and price are supplied, and inform user if they are not
if (Args.Count < 3)
{
    Console.WriteLine("Please provide 3 arguments, item name, category and price");
    return;
}

// Set arguments to local variables
string name = Args[0];
string category = Args[1];
string priceString = Args[2];

// If the name is empty, return out and inform user
if (string.IsNullOrEmpty(name))
{
    Console.WriteLine("Please enter an item name");
    return;
}

// If the category is empty, return out and inform user
if (string.IsNullOrEmpty(category))
{
    Console.WriteLine("Please enter an item category");
    return;
}

// Parse the incoming string into a double, and check if the result is greater than 0. If successful, round the value to 2 decimal places, otherwise inform user of invalid price entry
if (double.TryParse(priceString, out double price) || price < 0)
{
    price = Math.Round(price, 2);
}
else
{
    Console.WriteLine("Invalid price entry, please provide a valid price greater than 0 ");
    return;
}

// Confirm with user that the entered transaction is correct
Console.WriteLine($"You entered: Item name: {name}, Item category: {category}, Item price: ${price}, is this correct? (y for yes or any other key for no)");
string userInput = Console.ReadLine();

// If the transaction is correct write to the ledger, otherwise discard and return
if (userInput.ToLower() == "y")
{
    Console.WriteLine("Writing to ledger...");

    // Create a new transaction
    var currTransaction = new Transaction { Name = name, Category = category, Price = price };

    // If the ledger file exists, read from the json from the file, otherwise set json value to be empty
    string json = File.Exists("GeneralLedger.json") ? File.ReadAllText("GeneralLedger.json") : "";

    // List of transactions to be added to
    List<Transaction> transactions = new List<Transaction>();

    // If the json is empty, either because the file did not exist or because the file was empty, add the current transaction to the empty transaction list
    // Otherwise take the json from the file and deserialize it into a list of transactions, and set the local transactions equal to the list from the file,
    // and add the current transaction to this list
    if (string.IsNullOrEmpty(json.Trim()))
    {
        transactions.Add(currTransaction);
    }
    else
    {
        transactions = JsonSerializer.Deserialize<List<Transaction>>(json);
        transactions.Add(currTransaction);

    }

    // Serialize the local list of transactions into json and write the json to the ledger. Creates the ledger if it doesnt exist
    json = JsonSerializer.Serialize(transactions);
    File.WriteAllText("GeneralLedger.json", json);

    Console.WriteLine("Done!");
}
else
{
    Console.WriteLine("Discarding transaction");
}

// Class for representing the transaction to enter to the ledger. Sets a time stamp equal to the current time
private class Transaction
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}