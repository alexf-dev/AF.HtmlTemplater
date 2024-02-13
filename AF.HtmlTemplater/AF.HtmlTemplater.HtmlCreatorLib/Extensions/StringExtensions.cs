
namespace AF.HtmlTemplater.HtmlCreatorLib.Extensions
{
    public static class StringExtensions
    {
        public static string GetSubstring(this string content, int start, int end)
        {
            return content.Substring(start, end - start);
        }
    }
}
