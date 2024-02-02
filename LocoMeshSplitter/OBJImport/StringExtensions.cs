namespace Dummiesman
{
	internal static class StringExtensions
    {
		internal static string Clean(this string str)
        {
            string rstr = str.Replace('\t', ' ');
            while (rstr.Contains("  "))
                rstr = rstr.Replace("  ", " ");
            return rstr.Trim();
        }
    }
}
