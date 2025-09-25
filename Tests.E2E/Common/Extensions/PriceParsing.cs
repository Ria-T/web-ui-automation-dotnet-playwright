using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    /// <summary>
    /// Parse a US-formatted currency from arbitrary label text.
    /// 1) Extract the first currency-like token (e.g., "$1,234.56").
    /// 2) Parse it with NumberStyles.Currency using en-US culture.
    /// </summary>
    public static class PriceParsing
    {
        // en-US understands "$", "," as thousands and "." as decimal
        private static readonly CultureInfo Us = CultureInfo.GetCultureInfo("en-US");

        // Matches tokens like "$1,234.56", "1,234.56", "$59", "59.98" (first occurrence)
        private static readonly Regex CurrencyToken = new(
            pattern: @"\$?\s*\d[\d,]*([.]\d{2})?",
            options: RegexOptions.Compiled);

        /// <summary>
        /// Extracts a decimal from text like "$29.99" or "Item total: $1,239.98".
        /// Throws FormatException if no usable currency token is found.
        /// </summary>
        public static decimal ToMoney(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new FormatException("Empty money text.");

            var m = CurrencyToken.Match(text);
            if (!m.Success)
                throw new FormatException($"No currency token found in: '{text}'");

            var token = m.Value.Trim(); // e.g., "$1,234.56"

            if (decimal.TryParse(token, NumberStyles.Currency, Us, out var value))
                return value;

            throw new FormatException($"Could not parse currency token: '{token}'");
        }

        public static bool TryToMoney(this string text, out decimal value)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(text)) return false;
            var m = CurrencyToken.Match(text);
            if (!m.Success) return false;
            return decimal.TryParse(m.Value.Trim(), NumberStyles.Currency, Us, out value);
        }
    }
}

