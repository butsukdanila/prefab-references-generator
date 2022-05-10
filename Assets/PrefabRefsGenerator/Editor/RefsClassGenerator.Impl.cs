
using System.Text;

using PrefabRefsGenerator.Utilities.Editor;

using UnityEngine;

namespace PrefabRefsGenerator
{
	public partial class RefsClassGenerator : ClassGenerator
	{
		protected override string GetGenerationPath()
		{
			var sb = new StringBuilder(m_directory);

			var lastchar = m_directory[^1];
			if (lastchar != '/' && lastchar != '\\')
				sb.Append('/');

			sb.Append(m_className).Append(".cs");
			return sb.ToString();
		}

		protected override void InternalBuild()
		{
			Line($"namespace {m_classNamespace}");
			OpenBracket();
			{
				Line($"public class {m_className} : {typeof(PrefabRefs)}");
				OpenBracket();
				{
					foreach (var refInfo in m_refInfos)
					{
						Line($"[{typeof(SerializeField)}] private {refInfo.type} m_{refInfo.name};");
						Line($"public {refInfo.type} {refInfo.name} => m_{refInfo.name};");
						Line("", Options.All ^ Options.Indent);
					}

					Line("#if UNITY_EDITOR", Options.All ^ Options.Indent);
					Line($"private void OnValidate()");
					OpenBracket();
					{
						foreach (var refInfo in m_refInfos)
						{
							Line($"ValidateRef(ref m_{refInfo.name}, \"{refInfo.owner}\");");
						}
					}
					CloseBracket();
					Line("#endif", Options.All ^ Options.Indent);
				}
				CloseBracket();
			}
			CloseBracket();
		}
	}
}