using ProductCatalogLinq.Services;

namespace ProductCatalogLinq.UI;

public class ConsoleMenu
{
    private readonly ProductQueryService _queries;

    public ConsoleMenu(ProductQueryService queries)
    {
        _queries = queries;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n====== Product Catalog LINQ System ======");
            Console.WriteLine("1. View Available Products");
            Console.WriteLine("2. Filter by Category");
            Console.WriteLine("3. Filter by Price Range");
            Console.WriteLine("4. Search by Name");
            Console.WriteLine("5. Sort by Price");
            Console.WriteLine("6. Group by Category");
            Console.WriteLine("7. Stock Value Reports");
            Console.WriteLine("8. Low Stock Products");
            Console.WriteLine("9. Supplier Report");
            Console.WriteLine("10. Pagination Demo");
            Console.WriteLine("11. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1: ProductQueryService.PrintProducts(_queries.GetAvailableProducts()); break;
                    case 2:
                        Console.Write("Category: ");
                        ProductQueryService.PrintProducts(_queries.FilterByCategory(Console.ReadLine() ?? string.Empty));
                        break;
                    case 3:
                        Console.Write("Min: "); decimal.TryParse(Console.ReadLine(), out decimal min);
                        Console.Write("Max: "); decimal.TryParse(Console.ReadLine(), out decimal max);
                        ProductQueryService.PrintProducts(_queries.FilterByPriceRange(min, max));
                        break;
                    case 4:
                        Console.Write("Keyword: ");
                        ProductQueryService.PrintProducts(_queries.SearchByName(Console.ReadLine() ?? string.Empty));
                        break;
                    case 5:
                        Console.WriteLine("1 Asc  2 Desc");
                        int.TryParse(Console.ReadLine(), out int sort);
                        ProductQueryService.PrintProducts(sort == 2 ? _queries.SortByPriceDescending() : _queries.SortByPriceAscending());
                        break;
                    case 6: _queries.PrintGroupedByCategory(); break;
                    case 7:
                        Console.WriteLine($"Total stock value: {_queries.GetTotalStockValue():C}");
                        _queries.PrintStockValuePerCategory();
                        break;
                    case 8: ProductQueryService.PrintProducts(_queries.GetLowStockProducts()); break;
                    case 9:
                        foreach (var r in _queries.GetSupplierReports())
                            Console.WriteLine($"{r.SupplierName} | Count: {r.ProductCount} | Avg: {r.AveragePrice:C} | Value: {r.StockValue:C}");
                        break;
                    case 10:
                        Console.Write("Page: "); int.TryParse(Console.ReadLine(), out int page);
                        Console.Write("Size: "); int.TryParse(Console.ReadLine(), out int size);
                        ProductQueryService.PrintProducts(_queries.GetPage(page, size));
                        break;
                    case 11: return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
