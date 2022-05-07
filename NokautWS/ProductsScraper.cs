using HtmlAgilityPack;

namespace NokautWS;

public class ProductScraper
{
    private static readonly string BaseUrl = $"https://www.nokaut.pl/produkt:{input}.html";
    private static string input = "samsung";
    //public static string input = Console.ReadLine();


    public IEnumerable<ProductModel> GetProducts()
    {
        var web = new HtmlWeb();
        var doc = web.Load(BaseUrl);
        var productList = doc.QuerySelectorAll("#product-box");
        
        foreach (var item in productList)
        {
           
            var name = item
                .QuerySelector("#product-box > div > div.description > div > h2")
                .InnerText;

            var price = item
            .QuerySelector("#product-box > div > div.description > div > div")
            .InnerText;

            var href = item
           .QuerySelector("#product-box > div > div.thumb > a").Attributes["href"]
                .ToString();

            yield return new ProductModel(name, price, href);
        }
        
        
    }
}