using HtmlAgilityPack;

namespace NokautWS.Scrappers;

public class ProductListScrapper
{
    private readonly NokautConfig _config;

    public ProductListScrapper(NokautConfig config)
    {
        _config = config;
    }

    // IEnumerable<T> - zwracasz jak operujesz na jakiejś kolekcji np. filtrujesz po nazwie, ale chcesz jeszcze dodać np. inne filtry
    // ICollection<T> - zwracasz jak pobrałaś już jakieś dane i znajdują się już w pamięci
    // IList<T> - Tak samo jak ICollection, ale przewidujesz, że będziesz do tej kolekcji jeszcze coś dodawała
    // IQuerable<T> - Jeśli to zapytanie do bazy danych lub innego zasobu zewnetrznego, który dopiero wykonasz
    public async Task<ICollection<ProductModel>> GetProducts(string queryPhrase,
        CancellationToken cancellationToken = default)
    {
        var document = await FetchPage(queryPhrase, cancellationToken);

        if (string.IsNullOrEmpty(document.Text))
            return ArraySegment<ProductModel>.Empty;
        
        return document.QuerySelectorAll(".product-box")
            .Select(ExtractProduct)
            .ToList();
    }

    private async Task<HtmlDocument> FetchPage(string queryPhrase, CancellationToken cancellationToken = default)
    {
        var url = $"{_config.BaseUrl}/produkt:{queryPhrase}.html";
        var web = new HtmlWeb();
        
        try
        {
            return await web.LoadFromWebAsync(url, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); 
            // w normalnym projekcie wyglądałoby to tak: _logger.Error(e, "Product list fetching failed: {e}", e.Message);
            return new HtmlDocument();
        }
    }

    private ProductModel ExtractProduct(HtmlNode node)
    {
        var name = node
            .QuerySelector(".name")
            .InnerText;

        var price = node
            .QuerySelector(".price > strong")
            .Descendants()
            .Where(x => x is HtmlTextNode)
            .Skip(1)
            .FirstOrDefault()?
            .InnerText?
            .Trim();

        var href = node
            .QuerySelector(".url-box")
            .GetAttributeValue("href", null);

        return new ProductModel(name, price, href);
    }
}