using System;
using System.Text;

namespace PrefabRefsGenerator.Utilities
{
	public static class RefNameFormatter
	{
		public class Rules
		{
			public enum Order
			{
				NameFirst,
				TagFirst
			}

			public Order order;
		}

		public static readonly Rules Default = new() { order = Rules.Order.NameFirst };

		public static string Format(string name, string tag, Rules rules = null)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");
			if (string.IsNullOrEmpty(tag)) throw new ArgumentException("Tag cannot be null or empty");

			rules ??= Default;

			var preformat = rules.order switch
			{
				Rules.Order.NameFirst => $"{name}_{tag}",
				Rules.Order.TagFirst => $"{tag}_{name}",
				_ => throw new ArgumentOutOfRangeException(nameof(rules.order)),
			};

			var resultBuilder = new StringBuilder(preformat.Length);
			var makeUpper = false;
			var makeLower = false;
			for (var i = 0; i < preformat.Length; ++i)
			{
				var c = preformat[i];
				if (i == 0)
				{
					resultBuilder.Append(
						char.IsLetter(c) ? char.ToLowerInvariant(c) : '_'
					);
					continue;
				}

				if (char.IsLetterOrDigit(c))
				{
					resultBuilder.Append(
						makeUpper ? char.ToUpperInvariant(c) :
						makeLower ? char.ToLowerInvariant(c) : c
					);

					makeUpper = false;
					makeLower = false;
				}
				else if (char.IsWhiteSpace(c))
				{
					makeUpper = true;
				}
				else if (c.Equals('_'))
				{
					resultBuilder.Append(c);
					makeLower = true;
				}
			}

			return resultBuilder.ToString();
		}
	}
}