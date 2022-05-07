namespace NokautWS;

public class ProductModel
{
    public ProductModel(string name, string price, string href)
    {
        Name = name;
        Price = price;
        Href = href;
    }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Href { get; set; }
}