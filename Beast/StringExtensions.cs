using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    public static class StringExtensions
    {
        public static readonly char[] Vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
        public static readonly Dictionary<string, string> IrregularPlurals = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public static readonly Dictionary<string, string> IrregularPluralNouns = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        static StringExtensions()
        {
            IrregularPlurals.Add("fe", "ves");
            IrregularPlurals.Add("f", "ves");
            IrregularPlurals.Add("o", "es");
            IrregularPlurals.Add("us", "i");
            IrregularPlurals.Add("is", "es");
            IrregularPlurals.Add("on", "a");
            IrregularPlurals.Add("ix", "ices");
            IrregularPlurals.Add("eua", "euax");
            IrregularPlurals.Add("a", "ae");
            IrregularPlurals.Add("ouse", "ice");
            IrregularPlurals.Add("um", "a");

            IrregularPluralNouns.Add("ale", "ale");
            IrregularPluralNouns.Add("beer", "beer");
            IrregularPluralNouns.Add("bread", "bread");
            IrregularPluralNouns.Add("child", "children");
            IrregularPluralNouns.Add("cod", "cod");
            IrregularPluralNouns.Add("copper", "copper");
            IrregularPluralNouns.Add("deer", "deer");
            IrregularPluralNouns.Add("fish", "fish");
            IrregularPluralNouns.Add("foot", "feet");
            IrregularPluralNouns.Add("gold", "gold");
            IrregularPluralNouns.Add("goose", "geese");
            IrregularPluralNouns.Add("man", "men");
            IrregularPluralNouns.Add("meat", "meat");
            IrregularPluralNouns.Add("moose", "moose");
            IrregularPluralNouns.Add("ogre", "ogre");
            IrregularPluralNouns.Add("ore", "ore");
            IrregularPluralNouns.Add("ox", "oxen");
            IrregularPluralNouns.Add("person", "people");
            IrregularPluralNouns.Add("quiz", "quizzes");
            IrregularPluralNouns.Add("series", "series");
            IrregularPluralNouns.Add("sheep", "sheep");
            IrregularPluralNouns.Add("silver", "silver");
            IrregularPluralNouns.Add("species", "species");
            IrregularPluralNouns.Add("tooth", "teeth");
            IrregularPluralNouns.Add("wine", "wine");
            IrregularPluralNouns.Add("woman", "women");
            IrregularPluralNouns.Add("barracks", "barracks");
            IrregularPluralNouns.Add("gallows", "gallows");
            IrregularPluralNouns.Add("means", "means");
            IrregularPluralNouns.Add("offspring", "offspring");
        }

        #region A
        public static string A(this string value)
        {
            return value.A(false, false, 1);
        }
        public static string A(this string value, int quantity)
        {
            return value.A(false, false, quantity);
        }
        public static string A(this string value, bool isProperNoun)
        {
            return value.A(isProperNoun, false, 1);
        }
        public static string A(this string value, bool isProperNoun, int quantity)
        {
            return value.A(isProperNoun, false, quantity);
        }
        public static string A(this string value, bool isProperNoun, bool isSentenceStart)
        {
            return value.A(isProperNoun, isSentenceStart, 1);
        }
        public static string A(this string value, bool isProperNoun, bool isSentenceStart, int quantity)
        {
            return value.PrefixWord(ArticleType.A, isProperNoun, isSentenceStart, quantity);
        }
        #endregion

        #region The
        public static string The(this string value)
        {
            return value.The(false, false, 1);
        }
        public static string The(this string value, int quantity)
        {
            return value.The(false, false, quantity);
        }
        public static string The(this string value, bool isProperNoun)
        {
            return value.The(isProperNoun, false, 1);
        }
        public static string The(this string value, bool isProperNoun, int quantity)
        {
            return value.The(isProperNoun, false, quantity);
        }
        public static string The(this string value, bool isProperNoun, bool isSentenceStart)
        {
            return value.The(isProperNoun, isSentenceStart, 1);
        }
        public static string The(this string value, bool isProperNoun, bool isSentenceStart, int quantity)
        {
            return value.PrefixWord(ArticleType.The, isProperNoun, isSentenceStart, quantity);
        }
        #endregion

        #region Your
        public static string Your(this string value)
        {
            return value.Your(false, false, 1);
        }
        public static string Your(this string value, int quantity)
        {
            return value.Your(false, false, quantity);
        }
        public static string Your(this string value, bool isProperNoun)
        {
            return value.Your(isProperNoun, false, 1);
        }
        public static string Your(this string value, bool isProperNoun, int quantity)
        {
            return value.Your(isProperNoun, false, quantity);
        }
        public static string Your(this string value, bool isProperNoun, bool isSentenceStart)
        {
            return value.Your(isProperNoun, isSentenceStart, 1);
        }
        public static string Your(this string value, bool isProperNoun, bool isSentenceStart, int quantity)
        {
            return value.PrefixWord(ArticleType.Your, isProperNoun, isSentenceStart, quantity);
        }
        #endregion

        public static string PrefixWord(this string value, ArticleType articleType, bool isProperNoun, bool isSentenceStart, int quantity)
        {
            var loweredValue = value.ToLowerInvariant();
            var startWord = string.Empty;

            // Start Word
            if (articleType == ArticleType.A && !isProperNoun)
            {
                startWord = Vowels.Contains(loweredValue[0]) ? "an" : "a";
            }
            else if (articleType != ArticleType.None && isProperNoun)
            {
                startWord = articleType.ToString().ToLowerInvariant();
            }

            // Sentence Start
            if (isSentenceStart && !string.IsNullOrEmpty(startWord))
            {
                if (startWord.Length > 1)
                    startWord = string.Concat(startWord.Substring(0, 1).ToUpperInvariant(), startWord.Substring(1));
                else
                    startWord = startWord.ToUpperInvariant();
            }

            // Quantity
            if (quantity > 1)
            {
                // Start word should be the quantity
                startWord = quantity.ToString();

                if (IrregularPluralNouns.ContainsKey(value))
                {
                    value = IrregularPluralNouns[value];
                }
                else if (IsIrregularPlural(value))
                {
                    value = value.ToIrregularPlural();
                }
                else
                {
                    value = value.ToRegularPlural();
                }
            }

            return startWord.Length > 0 ? string.Concat(startWord, " ", value) : value;
        }

        private static bool IsIrregularPlural(this string value)
        {
            foreach (var key in IrregularPlurals.Keys)
            {
                if (value.EndsWith(key))
                    return true;
            }
            return false;
        }

        private static string ToRegularPlural(this string value)
        {
            var loweredValue = value.ToUpperInvariant();

            if (loweredValue.EndsWith("s")
                || loweredValue.EndsWith("x")
                || loweredValue.EndsWith("ch")
                || loweredValue.EndsWith("sh")
                || loweredValue.EndsWith("z"))
            {
                return string.Concat(value, "es");
            }

            if (loweredValue.EndsWith("y") && loweredValue.Length >= 2 && !Vowels.Contains(loweredValue[loweredValue.Length - 2]))
                return string.Concat(value.DropLast(1), "ies");

            return string.Concat(value, "s");
        }

        private static string ToIrregularPlural(this string value)
        {
            var loweredValue = value.ToUpperInvariant();
            foreach (var kvp in IrregularPlurals)
            {
                if (loweredValue.EndsWith(kvp.Key))
                    return string.Concat(value.DropLast(kvp.Key.Length), kvp.Value);
            }

            return value;
        }

        private static string DropLast(this string value, int quantity)
        {
            if (value.Length - quantity < value.Length)
                return value;

            return value.Substring(0, value.Length - quantity);
        }
    }

    public enum ArticleType
    {
        None,
        A,
        The,
        Your
    }
}
