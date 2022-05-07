// See https://aka.ms/new-console-template for more information


using Newtonsoft.Json;
using NokautWS;
using NokautWS.Scrappers;

var productScraper = new ProductListScrapper(new NokautConfig("https://www.nokaut.pl"));
var input = Console.ReadLine();
var products = await productScraper.GetProducts(input);
//var products = await productScraper.GetProducts("samsung");
var result = new List<ProductDetails>();

foreach (var product in products)
{
    Console.WriteLine($"Product Name: {product.Name}");
    result.Add(new ProductDetails
    {
        Name= product.Name, 
        Price = product.Price, 
        Href = product.Href
    });
    
}
Console.WriteLine("finished");

var json = JsonConvert.SerializeObject(result);
File.WriteAllText(@"./result.json", json);

