// Add nuget reference to smartsheet-csharp-sdk (https://www.nuget.org/packages/smartsheet-csharp-sdk/)
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace sdk_csharp_simpleExample
{
    class SimpleExample
    {
        public static void Main(string[] args)
        {
        SmartsheetClient smartsheet = new SmartsheetBuilder()
            .SetAccessToken(Environment.GetEnvironmentVariable("SMARTSHEET_ACCESS_TOKEN") ?? "your_token_here")
            .Build();

        // Si no pasas ningún ID, lista todas tus hojas con su ID
        if (args.Length == 0)
        {
            PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(
                paging: new PaginationParameters(true, null, null));   // true = traer todas, no solo la primera página

            Console.WriteLine($"Tienes {sheets.TotalCount} hojas:");
            foreach (Sheet s in sheets.Data)
                Console.WriteLine($"  {s.Id}  {s.Name}");

            Console.WriteLine("\nPara abrir una:  dotnet run -- <ID>");
            return;
        }

    // Si pasas un ID, abre esa hoja
    long sheetId = long.Parse(args[0]);
    Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId);
    Console.WriteLine($"Hoja: {sheet.Name}  ({sheet.Rows.Count} filas)\n");

    foreach (Row row in sheet.Rows.Take(5))
        dumpRow(row, sheet.Columns);
}

        // Display row contents
        static void dumpRow(Row row, IList<Column> columns)
        {
            Console.WriteLine("Row # " + row.RowNumber + ":");
            foreach (var cell in row.Cells)
            {
                // Find column name by Id in column collection
                var columName = columns.First(column => column.Id == cell.ColumnId).Title;
                Console.WriteLine("    " + columName + ": " + cell.Value);
            }
        }
    }
}
