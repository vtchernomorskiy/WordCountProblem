using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountUtilities
{
    public static class StringExtensions
    {
        public static IEnumerable<string> ToWordArray(this string statement, bool stripPunctuation = true)
        {
            var validStatement = statement.Validate();

            var sentence = stripPunctuation
                ? new string(validStatement.Where(c => !char.IsPunctuation(c)).ToArray())
                : validStatement;

            return sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
        }

        public static string Validate(this string statement)
        {
            if (string.IsNullOrWhiteSpace(statement))
            {
                throw new InputStatementValidationException("Input string is null, empty or only contains white spaces");
            }

            return statement.Trim();
        }

        public static IEnumerable<string> Validate(this IEnumerable<string> words)
        {
            if (words == null)
            {
                throw new InputStatementValidationException("Input string sequence is null");
            }

            return words;
        }

        public static IEnumerable<WordCount> CountWordOccurences(this IEnumerable<string> words)
        {
            return words
                .Validate()
                .Select(token => new { Word = token.ToUpper(), Count = 0 })
                .GroupBy(word => word.Word)
                .Select(group => new WordCount { Word = group.Key, Count = group.Count() });
        }
    }
}
