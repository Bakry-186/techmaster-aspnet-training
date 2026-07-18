using Microsoft.AspNetCore.Mvc;

namespace OriginalBadCode.Controllers;

public class Product
{
    public int id;
    public string name;
    public string description;
    public decimal price;
    public int stock;
}

[Route("products")]
public class ProductsController : Controller
{
    public static List<Product> products = new();

    [HttpGet("getall")]
    public IActionResult getall(string page, string size)
    {
        int p = int.Parse(page);
        int s = int.Parse(size);
        return Ok(products.Skip((p - 1) * s).Take(s));
    }

    [HttpGet("getbyid")]
    public IActionResult getbyid(string id)
    {
        var product = products.FirstOrDefault(x => x.id == int.Parse(id));
        if (product == null)
        {
            return Ok(new { message = "not found" });
        }

        return Ok(product);
    }

    [HttpPost("add")]
    public IActionResult add(string name, string description, string price, string stock)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Ok(new { message = "name required" });
        }

        var product = new Product
        {
            id = products.Count + 1,
            name = name,
            description = description,
            price = decimal.Parse(price),
            stock = int.Parse(stock)
        };

        products.Add(product);
        return Ok(product);
    }

    [HttpPost("update")]
    public IActionResult update(string id, string name, string description, string price, string stock)
    {
        var product = products.FirstOrDefault(x => x.id == int.Parse(id));
        if (product == null)
        {
            return Ok(new { message = "not found" });
        }

        product.name = name;
        product.description = description;
        product.price = decimal.Parse(price);
        product.stock = int.Parse(stock);
        return Ok(product);
    }

    [HttpPost("delete")]
    public IActionResult delete(string id)
    {
        var product = products.FirstOrDefault(x => x.id == int.Parse(id));
        if (product == null)
        {
            return Ok(new { message = "not found" });
        }

        products.Remove(product);
        return Ok(new { message = "deleted" });
    }
}
