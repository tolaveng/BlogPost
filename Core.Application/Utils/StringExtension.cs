using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Application.Utils
{
    public static class StringExtension
    {
        public static string RemoveSpecialChars(this String input)
        {
            var regex = new Regex(@"[^0-9a-zA-Z-_]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regex.Replace(input, "-");
        }


        public static string RemoveWhiteSpace(this String input)
        {
            var regex = new Regex(@"[\s]+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regex.Replace(input, "");
        }

        private static string RemoveScriptTags(this String input)
        {
            Regex regex = new Regex(@"<script[^>]*>[\s\S]*?</script>", RegexOptions.IgnoreCase);
            return regex.Replace(input, "");
        }

        public static string StripHTML(this String input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
