namespace NokautWS;

public class NokautConfig
{
    public string BaseUrl { get; }

    public NokautConfig(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
}