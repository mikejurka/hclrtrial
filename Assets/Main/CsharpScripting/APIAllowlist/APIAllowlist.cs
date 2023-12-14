using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpatialSys.Client.CSharpScripting
{
    /// APIAllowlist checks if a method is allowlisted, based on a list of rules.
    /// The list of rules is passed in as a string, with each rule separated by a newline.
    /// This list follows a simplified .gitignore syntax, supporting:
    ///   * line comments (prefixed with '#'),
    ///   * negation ('!')
    ///   * wildcard asterisks ('*') at the end of a pattern.
    ///
    /// Example input:
    ///
    /// # This is a comment
    /// * # Catchall to allow all methods by default, except for those that are blocked below
    /// !UnityEngine.Application.Quit
    /// !System.Reflection* # Another comment

    public class APIAllowlist
    {
        // A list of regexes for each line in the allowlist.
        private readonly (Regex pattern, bool allow)[] _rulesReversed;

        public APIAllowlist(string allowlist)
        {
            _rulesReversed = ParseAllowlist(allowlist).Select(CreateRegex).Reverse().ToArray();
        }

        private static IEnumerable<string> ParseAllowlist(string allowlist)
        {
            return allowlist.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Split('#')[0].Trim())
                                .Where(line => line != "");
        }

        // Expects a method's full name, which includes its namespace and class name, but not assembly.
        public bool IsMethodAllowlisted(string methodFullName)
        {
            // Does the method name match any of the allowlist patterns?
            (Regex pattern, bool allow) match =
                _rulesReversed.FirstOrDefault(p => p.pattern.IsMatch(methodFullName));

            // Did we match? (default value of tuple is like null)
            // Was it an allow pattern? (i.e. it did not begin with '!')
            return match != default && match.allow;
        }

        private static (Regex pattern, bool allow) CreateRegex(string rule)
        {
            ValidateRule(rule);
            bool isNegative = rule.StartsWith("!");
            if (isNegative)
            {
                rule = rule.Substring(1);
            }
            bool wildcardEnding = rule.EndsWith("*");
            if (wildcardEnding)
            {
                rule = rule.Substring(0, rule.Length - 1);
            }
            string pattern = Regex.Escape(rule);
            if (wildcardEnding)
            {
                pattern += ".*";
            }
            return (new Regex(pattern, RegexOptions.Compiled), !isNegative);
        }

        private static void ValidateRule(string pattern)
        {
            if (pattern.Contains("**"))
            {
                throw new ArgumentException("Double asterisks (**) are not supported.");
            }
        }
    }
}