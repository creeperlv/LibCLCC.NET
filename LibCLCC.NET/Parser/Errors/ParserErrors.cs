using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using System;
#nullable enable
namespace ParserFramework.Core.Errors
{
	/// <summary>
	/// 
	/// </summary>
	public class RuleNotFoundError : Error
	{
		/// <summary>
		/// Name of the expected rule.
		/// </summary>
		public string RuleName { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ruleName"></param>
		public RuleNotFoundError(string ruleName) : base() { RuleName = ruleName; }
	}
	/// <summary>
	/// Not matching expected rule.
	/// </summary>

	public class RuleMatchingExpectedError : Error
	{
		/// <summary>
		/// Target rule name.
		/// </summary>
		public string RuleName { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ruleName"></param>
		public RuleMatchingExpectedError(string ruleName) : base() { RuleName = ruleName; }
	}
	/// <summary>
	/// Rule pattern expecting a lex id , but not matching.
	/// </summary>
	public class NotMatchingLexError : Error
	{
		/// <summary>
		/// Target lex id.
		/// </summary>
		public string? LexID { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lexID"></param>
		public NotMatchingLexError(string? lexID) : base() { LexID = lexID; }
	}

	/// <summary>
	/// Arguments of an action are invalid.
	/// </summary>
	public class InvalidActionArgumentsError : Error
	{
		/// <summary>
		/// Target action name.
		/// </summary>
		public string ActionName { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="actionName"></param>
		public InvalidActionArgumentsError(string actionName) : base() { ActionName = actionName; }
	}

	/// <summary>
	/// Expected node not found.
	/// </summary>
	public class NodeNotCreatedError : Error
	{
		/// <summary>
		/// Target node.
		/// </summary>
		public string NodeName { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodeName"></param>
		public NodeNotCreatedError(string nodeName) : base() { NodeName = nodeName; }
	}


	/// <summary>
	/// Generic error with content.
	/// </summary>
	[Serializable]
	public class ErrorWContent : Error
	{
		/// <summary>
		/// The content of the error, usually a message describing the error.
		/// </summary>
		public string content;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>

		public ErrorWContent(string content)
		{
			this.content = content;
		}
		/// <summary>
		/// Returns the content field.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return content;
		}
	}
	/// <summary>
	/// Not expecting a certain token, but got it.
	/// </summary>
	public class UnexpectedTokenError : ErrorWContent
	{
		/// <summary>
		/// 
		/// </summary>
		public string Expected { get; }
		/// <summary>
		/// 
		/// </summary>
		public string ActualContent { get; }
		/// <summary>
		/// 
		/// </summary>
		public string ActualId { get; }
		/// <summary>
		/// 
		/// </summary>
		public LexSegmentPosition Position { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		public UnexpectedTokenError(string expected, LexSegment actual)
			: base($"Unexpected token. Expected '{expected}', but got '{actual.Content}' (ID: {actual.LexSegmentId}) at {actual.Position}.")
		{
			Expected = expected;
			ActualContent = actual.Content ?? "N/A";
			ActualId = actual.LexSegmentId ?? "N/A";
			Position = actual.Position;
		}
	}
	/// <summary>
	/// Pattern mismatch error.
	/// </summary>
	public class PatternMismatchError : ErrorWContent
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="segment"></param>
		/// <param name="targetPattern"></param>
		public PatternMismatchError(LexSegment segment, string targetPattern) :
			base($"{segment} not matching {targetPattern}.")
		{ }
	}
	/// <summary>
	/// Expecting content can be parsed to a certain type, but not.
	/// </summary>

	public class TypeParsingError : ErrorWContent
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="segment"></param>
		/// <param name="targetType"></param>
		public TypeParsingError(LexSegment segment, string targetType) :
			base($"{segment} cannot be parse to {targetType}.")
		{ }
	}


}
