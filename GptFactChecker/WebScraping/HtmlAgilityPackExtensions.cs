using HtmlAgilityPack;

namespace WebScraping;

internal static class HtmlAgilityPackExtensions
{
    public static bool IsName(this HtmlNode node, string name)
    {
        if (node is null)
            return false;

        return name.Equals(node.Name);
    }
}
