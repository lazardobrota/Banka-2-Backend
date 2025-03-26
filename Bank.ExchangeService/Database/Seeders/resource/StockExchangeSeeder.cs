using Bank.ExchangeService.Model;

namespace Bank.ExchangeService.Database.Seeders.Resource;

using StockExchangeModel = StockExchange;

public static class StockExchangeSeeder
{
    public static async Task SeedStockExchanges(this DatabaseContext context)
    {
        if (context.StockExchanges.Any())
            return;

        var exchanges = ReadExchangesFromCsv();
        await context.StockExchanges.AddRangeAsync(exchanges);
        await context.SaveChangesAsync();
    }

    private static List<StockExchangeModel> ReadExchangesFromCsv()
    {
        var exchanges = new List<StockExchangeModel>();
        var now       = DateTime.UtcNow;

        var baseDirectory    = AppContext.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var filePath         = Path.Combine(projectDirectory, "Database", "Seeders", "resource", "exchanges.csv");

        string[] lines = File.ReadAllLines(filePath);

        // Skip the header row
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            // Handle CSV fields properly (especially those with commas inside quotes)
            string[] fields = ParseCsvLine(line);

            if (fields.Length >= 8)
            {
                var exchange = new StockExchangeModel
                               {
                                   Id      = Guid.NewGuid(),
                                   Name    = fields[0],
                                   Acronym = fields[1],
                                   MIC     = fields[2] != "" ? fields[2] : "XXXX",    // Provide a default MIC if not available
                                   Polity  = fields[3] != "" ? fields[3] : "Unknown", // Provide a default Polity
                                   // Set other fields as needed
                                   CreatedAt  = now,
                                   ModifiedAt = now
                               };

                exchanges.Add(exchange);
            }
        }

        return exchanges;
    }

    // Helper method to parse CSV lines correctly, handling quoted fields
    private static string[] ParseCsvLine(string line)
    {
        var result   = new List<string>();
        var inQuotes = false;
        var field    = "";

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(field);
                field = "";
            }
            else
            {
                field += c;
            }
        }

        result.Add(field);

        return result.ToArray();
    }
}
