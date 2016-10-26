using System;
using System.Collections.Generic;
using GosuParser;
using static GosuParser.Parser;

namespace Nala
{
    public static class ParserHelpers
    {
        public static Parser<T> S<T>(this Parser<T> value) => value.TakeLeft(Spaces);
        public static Parser<string> S(this string value) => String(value).S();
        public static Parser<char> S(this char value) => Char(value).S();

        public static Parser<T> S1<T>(this Parser<T> value) => value.TakeLeft(Spaces1);
        public static Parser<string> S1(this string value) => String(value).S1();
        public static Parser<char> S1(this char value) => Char(value).S1();

        private static int depth = 0;
        public static Parser<T> Trace<T>(this Parser<T> p, string label) => input =>
        {
            var indent = new string(' ', depth*2);
            System.Diagnostics.Trace.WriteLine($"{indent}Starting {label}");
            depth++;
            var result = p.Run(input);
            depth--;
            var resultText = result.IsSuccess ? "Success" : "Failure";
            System.Diagnostics.Trace.WriteLine($"{indent}Exiting {label}: {resultText}");
            return result;
        };

        public static IEnumerable<string> WithSeperator(this IEnumerable<string> src, string seperator)
        {
            using (var enumerator = src.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                yield return enumerator.Current;

                while (enumerator.MoveNext())
                {
                    yield return seperator + enumerator.Current;
                }
            }
        }
    }
}