using ProductCatalogLinq.Services;
using ProductCatalogLinq.UI;

var data = new ProductDataService();
var queries = new ProductQueryService(data.GetProducts());
new ConsoleMenu(queries).Run();
