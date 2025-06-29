using System;
using System.Collections.Generic;
#nullable enable
namespace LibCLCC.NET.Parser
{
	/// <summary>
	/// The scope context of a parse rule.
	/// </summary>
	public class ParseRuleScopeContext
	{
		internal ParseRuleScopeContext? Parent = null;
		internal ParseContext context;
		internal ASTNode? ReturnValue = null;
		internal int LastMatchSelection = -1;
		internal int initialIndex;
		internal int Depth = 0;
		internal string depthStr = "\t";
		internal ASTNode? ParentLatest = null;
		internal bool ConsumedParentLatest = false;
		internal Dictionary<string, ASTNode> CurrentNodes { get; set; } = new Dictionary<string, ASTNode>();
		internal List<object?> Parameters = new List<object?>();
		internal bool ShowLog = false;
		internal Action<int, string>? AlternativeLogAction = null;
		internal ParseRuleScopeContext(ParseContext context, int initialIndex, bool showLog = false, Action<int, string>? alternativeLogAction = null)
		{
			this.context = context;
			this.initialIndex = initialIndex;
			ShowLog = showLog;
			AlternativeLogAction = alternativeLogAction;
		}
		internal ParseRuleScopeContext CreateChild(int index, int lastMatchSelection)
		{
			return new ParseRuleScopeContext(context, index, this.ShowLog,
				this.AlternativeLogAction)
			{ Depth = this.Depth + 1, LastMatchSelection = lastMatchSelection };
		}
		internal void LogLine(string str)
		{
			if (AlternativeLogAction != null)
			{
				AlternativeLogAction(Depth, str);
				return;
			}
			if (!ShowLog) return;
			for (int i = 0; i < Depth; i++)
			{
				Console.Write(depthStr);
			}
			Console.WriteLine(str);
		}
	}
}
