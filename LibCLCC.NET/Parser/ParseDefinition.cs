using LibCLCC.NET.TextProcessing;
using System;
using System.Collections.Generic;
#nullable enable
namespace LibCLCC.NET.Parser
{
	/// <summary>
	/// The definition of a parser rule.
	/// </summary>
	[Serializable]
	public class ParseDefinition
	{
		/// <summary>
		/// Known AST types with their IDs.
		/// </summary>
		public Dictionary<string, int> ASTTypes = new();
		/// <summary>
		/// Rules.
		/// </summary>
		public Dictionary<string, Rule> Rules = new();
		/// <summary>
		/// Get the maximum of existing AST type ID.
		/// </summary>
		/// <returns></returns>
		public int GetMaxASTTypeID()
		{
			int maxId = -1;
			foreach (var kvp in ASTTypes)
			{
				if (kvp.Value > maxId)
					maxId = kvp.Value;
			}
			return maxId;
		}
	}
	/// <summary>
	/// Single rule of a parser definition.
	/// </summary>
	[Serializable]
	public class Rule
	{
		/// <summary>
		/// Name of the rule.
		/// </summary>
		public string? RuleName;
		/// <summary>
		/// Match patterns for the rule.
		/// </summary>
		public List<Match> Matches = new();
	}
	/// <summary>
	/// Match pattern for a rule.
	/// </summary>
	[Serializable]
	public class Match
	{
		/// <summary>
		/// Real pattern.
		/// </summary>
		public List<string> Pattern = new List<string>();
		/// <summary>
		/// Target (actions).
		/// </summary>
		public Target Target = new Target();
	}
	/// <summary>
	/// Target of a match.
	/// </summary>
	[Serializable]
	public class Target
	{
		/// <summary>
		/// Actions of the target.
		/// </summary>
		public List<Action> Actions = new();
	}
	/// <summary>
	/// Single Action of a target in a match.
	/// </summary>
	[Serializable]
	public class Action
	{
		/// <summary>
		/// Command and its parameters.
		/// </summary>
		public List<string> Segments = new();

	}
}
