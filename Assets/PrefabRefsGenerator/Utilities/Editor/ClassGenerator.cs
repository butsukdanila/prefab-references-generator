using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PrefabRefsGenerator.Utilities.Editor
{
	public abstract class ClassGenerator
	{
		private const string c_new_line = "\r\n";
		private const string c_indent = "\t";

		private readonly StringBuilder m_builder = new();

		private int m_indentCount;

		private string indent => string.Concat(Enumerable.Repeat(c_indent, m_indentCount));

		public void Generate()
		{
			var path = GetGenerationPath();
			if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path can not be null or empty", nameof(path));

			var directory = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(directory)) throw new ArgumentException("Path doesn't contain directory");

			var built = Build();
			if (string.IsNullOrEmpty(built)) throw new Exception("Generation gone wrong. Built string is null or empty");

			Directory.CreateDirectory(directory);
			using var fs = File.Create(path);
			using var sw = new StreamWriter(fs);
			sw.WriteLine(built);
		}

		public string GenerateSha256()
		{
			var path = GetGenerationPath();
			if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path can not be null or empty", nameof(path));

			return string.Empty;
		}

		protected string Build()
		{
			InternalBuild();
			return m_indentCount switch
			{
				> 0 => throw new Exception("Opened more brackets then closed"),
				< 0 => throw new Exception("Closed more brackets then opened"),
				_ => m_builder.ToString(),
			};
		}

		protected abstract void InternalBuild();
		protected abstract string GetGenerationPath();

		protected void Line(string text = "", Options options = Options.All)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			if ((options & Options.Indent) != 0) m_builder.Append(indent);
			m_builder.Append(text);
			if ((options & Options.NewLine) != 0) m_builder.Append(c_new_line);
		}

		protected void OpenBracket()
		{
			Line("{");
			++m_indentCount;
		}

		protected void CloseBracket()
		{
			--m_indentCount;
			Line("}");
		}

		protected enum Options
		{
			None = 0,
			Indent = 1 << 0,
			NewLine = 1 << 1,
			All = Indent | NewLine
		}
	}
}