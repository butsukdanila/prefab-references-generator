using System;
using System.Collections.Generic;
using System.Linq;

namespace PrefabRefsGenerator.Utilities
{
	public static class RefTagsParser
	{
		public class Rules
		{
			public char begin;
			public char end;
			public char separator;
		}

		public static readonly Rules Default = new() { begin = '[', end = ']', separator = ',' };

		public static bool TryParse(string source, out string tagless, out string[] tags, Rules rules = null)
		{
			return TryParseInternal(source.AsSpan(), out tagless, out tags, rules ?? Default);
		}

		private static bool TryParseInternal(ReadOnlySpan<char> source, out string tagless, out string[] tags, Rules rules)
		{
			tagless = default;
			tags = default;

			if (!TrySplit(out var taglessPart, ref source, rules.begin)) return false;
			if (!TrySplit(out var tagsPart, ref source, rules.end)) return false;

			tagless = taglessPart.Trim().ToString();
			tags = ParseTags(tagsPart, rules.separator);
			return tags.Length > 0;
		}

		private static string[] ParseTags(ReadOnlySpan<char> tagsPart, char separator)
		{
			var tagsHashSet = new HashSet<string>();
			while (tagsPart.Length > 0)
			{
				TrySplit(out var tagPart, ref tagsPart, separator);
				var tag = tagPart.Trim().ToString();

				if (!string.IsNullOrEmpty(tag))
					tagsHashSet.Add(tag);
			}
			return tagsHashSet.ToArray();
		}

		private static bool TrySplit(out ReadOnlySpan<char> lhs, ref ReadOnlySpan<char> rhs, char separator)
		{
			var position = rhs.IndexOf(separator);
			if (position != -1)
			{
				lhs = rhs[..position];
				rhs = rhs[(position + 1)..];
				return true;
			}

			lhs = rhs[..];
			rhs = rhs[rhs.Length..];
			return false;
		}
	}
}