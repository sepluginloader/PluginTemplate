using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClientPlugin.Settings.Elements
{
    public static class Tools
    {
        private static readonly Regex UpperCaseWordRegex = new Regex(@"[A-Z][a-z]*", RegexOptions.Compiled);

        public static string GetLabelOrDefault(string name, string label = null)
        {
            Debug.Assert(!string.IsNullOrEmpty(name) && name.Trim().Length != 0);

            if (label != null)
                return label;

            var words = UpperCaseWordRegex.Matches(name).Cast<Match>().Select(m => m.Value).ToArray();
            Debug.Assert(words.Length != 0);

            for (var i = 1; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
            }

            return string.Join(" ", words);
        }
    }
}