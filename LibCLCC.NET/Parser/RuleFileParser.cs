using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using LibCLCC.NET.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using LibCLCC.NET.Parser.ActionEngine;

namespace LibCLCC.NET.Parser
{
	#nullable enable
	/// <summary>
	/// Parser that parses a string to a Parse Rule Definition.
	/// </summary>
	public static class RuleFileParser
	{
		private static LexerDefinition? lDef;
		static List<string> GeneralIgnorance = new List<string>
		{
			"WS", "NEWLINE", "COMMENT", "*"
		};
		static List<string> GeneralIgnoranceWONewLine = new List<string>
		{
			"WS", "COMMENT", "*"
		};
		private const string lexRule =
@"
Match:
NEWLINE ""[\r\n]""
WS ""\s+""
COMMENT ""//.*""
LEXID_KW ""LexId:""
NODEID_KW ""NodeId:""
RULE_KW ""Rule:""
AUTO_KW ""--auto""
ARROW_BRACE ""=>\s*{""
OR_SEP ""\|""
SEMICOLON ;
LBRACE {
RBRACE }
COLON :
NUMBER \d+
IDENTIFIER ""[\^\$a-zA-Z_][a-zA-Z0-9\-_.]*""

Id:
NEWLINE NEWLINE
WS WS
COMMENT COMMENT
LEXID_KW LEXID_KW
NODEID_KW NODEID_KW
RULE_KW RULE_KW
AUTO_KW AUTO_KW
ARROW_BRACE ARROW_BRACE
OR_SEP OR_SEP
SEMICOLON SEMICOLON
LBRACE LBRACE
RBRACE RBRACE
DOLLAR DOLLAR
DOT DOT
COLON COLON
CARET CARET
NUMBER NUMBER
IDENTIFIER IDENTIFIER";
		static List<string> SectionHeaders = new List<string>
		{
			"LEXID_KW", "NODEID_KW", "RULE_KW"
		};
		static RuleFileParser()
		{
			loadRule();
		}

		private static void loadRule()
		{
			if (!LexerDefinition.TryParse(lexRule, out lDef))
			{
				lDef = null;
			}
			else
			{
				lDef.Substitute();
			}
		}
		private class ParserContext
		{
			public ILexer Lexer;
			public List<LexSegment> TokenBuffer;
			public int CurrentTokenIndex;
			public ParseDefinition ResultDefinition;
			public string SourceId;
			public int CurrentNodeTypeId;

			public ParserContext(ILexer lexer, string sourceId)
			{
				Lexer = lexer;
				TokenBuffer = new List<LexSegment>();
				CurrentTokenIndex = 0;
				ResultDefinition = new ParseDefinition();
				SourceId = sourceId;
				CurrentNodeTypeId = 0; // Initialize auto-incrementing ID
			}
		}
		/// <summary>
		/// Parse ParseDefinition from a string input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static OperationResult<ParseDefinition?> ParseRule(string input, string id = "generic.y")
		{
			StringLexer lexer = new StringLexer();
			if (lDef == null)
			{
				loadRule();
			}
			if (lDef == null)
			{
				Console.WriteLine("Cannot load lexer rules.");
				return new OperationResult<ParseDefinition?>(null);
			}
			lexer.SetDefinition(lDef);
			lexer.Content = input;
			lexer.SourceID = id;
			return ParseRule(lexer);
		}
		/// <summary>
		/// Parse ParseDefinition from a lexer.
		/// </summary>
		/// <param name="inputlexer"></param>
		/// <returns></returns>
		public static OperationResult<ParseDefinition?> ParseRule(ILexer inputlexer)
		{
			OperationResult<ParseDefinition?> parseResult = new OperationResult<ParseDefinition?>();
			parseResult.Result = new ParseDefinition();
			ParseContext context = new ParseContext(inputlexer, parseResult.Result, new AEngine());
			while (true)
			{
				if (context.IsEnd())
				{
					break;
				}
				var current = context.Peek();
				if (parseResult.CheckAndInheritErrorAndWarnings(current))
				{
					break;
				}
				if (current.Result is null)
				{
					break;
				}
				context.Consume();
				switch (current.Result.LexSegmentId)
				{
					case "NODEID_KW":
						ParseNodeIDs(context);
						break;
					case "RULE_KW":
						ParseRules(context);
						break;
					default:
						if (current.Result.LexSegmentId != null && GeneralIgnorance.Contains(current.Result.LexSegmentId))
						{
							continue;
						}
						else
						{
							parseResult.AddError(new ErrorWContent($"Unexpected token '{current.Result.Content}' ({current.Result.LexSegmentId}) in rule file at {current.Result.Position}."));
							return parseResult;
						}
				}
			}

			return parseResult;
		}
		private static OperationResult<bool> ParseNodeIDs(ParseContext context)
		{
			OperationResult<bool> result = new OperationResult<bool>(true);
			bool autoIncrement = false;
			while (true)
			{
				if (context.IsEnd())
				{
					break;
				}
				var current = context.Peek();
				if (result.CheckAndInheritErrorAndWarnings(current))
				{
					break;
				}
				if (current.Result is null || current.Result.LexSegmentId is null
					|| current.Result.Content is null)
				{
					break;
				}
				if (SectionHeaders.IndexOf(current.Result.LexSegmentId) >= 0)
				{
					break;
				}
				context.Consume();
				switch (current.Result.LexSegmentId)
				{
					case "IDENTIFIER":

						if (autoIncrement)
						{
							context.Definition.ASTTypes.Add(current.Result.Content, context.Definition.GetMaxASTTypeID() + 1);
						}
						else
						{
							var nodeID = current.Result.Content;
							current = context.Peek();
							if (result.CheckAndInheritErrorAndWarnings(current))
							{
								break;
							}
							if (current.Result is null || current.Result.LexSegmentId is null
								|| current.Result.Content is null)
							{
								break;
							}
							if (current.Result.LexSegmentId == "NUMBER")
							{
								if (int.TryParse(current.Result.Content, out var value))
								{
									context.Definition.ASTTypes.Add(nodeID, value);
								}
								else
								{
									result.AddError(new TypeParsingError(current.Result, nameof(Int32)));

								}
							}
							else
							{
								result.AddError(new PatternMismatchError(current.Result, "IDENTIFIER NUMBER"));
							}
						}
						break;
					case "AUTO_KW":
						autoIncrement = true;
						break;
					default:
						if (current.Result.LexSegmentId != null && GeneralIgnorance.Contains(current.Result.LexSegmentId))
						{
							continue;
						}
						else
						{
							result.AddError(new UnexpectedTokenError("IDENTIFIER", current.Result));
							return result;
						}
				}
			}
			return result;
		}
		private static OperationResult<bool> ParseRules(ParseContext context)
		{
			OperationResult<bool> result = new OperationResult<bool>(true);
			while (true)
			{
				if (context.IsEnd())
				{
					break;
				}
				var matchingResult = context.PatternMatchingIDs(GeneralIgnorance, "IDENTIFIER", "COLON");
				if (result.CheckAndInheritErrorAndWarnings(matchingResult))
				{
					break;
				}
				if (matchingResult.Result.Item1 == true)
				{
					Rule rule = new Rule();
					rule.RuleName = matchingResult.Result.Item2[0].Content;
					var matchResult = ParseMatch(context);
					if (result.CheckAndInheritErrorAndWarnings(matchResult))
					{
						break;
					}
					rule.Matches.Add(matchResult.Result!);
					while (true)
					{
						var p = context.PatternMatchingIDsFloor(GeneralIgnorance, "OR_SEP", "SEMICOLON");
						if (result.CheckAndInheritErrorAndWarnings(p))
						{
							break;
						}
						if (p.Result.Item1)
						{
							if (p.Result.Item2[0].LexSegmentId == "SEMICOLON")
							{
								break; 
							}
							else
							{

								matchResult = ParseMatch(context);
								if(result.CheckAndInheritErrorAndWarnings(matchResult))
								{
									break;
								}
								rule.Matches.Add(matchResult.Result!);
							}
						}
					}
					if (!result.HasError())
					{
						context.Definition.Rules.Add(rule.RuleName ?? "", rule);
					}
					else
					{
						break;
					}
				}
				else
				{
					var current = context.Peek();
					if (result.CheckAndInheritErrorAndWarnings(current))
					{
						break;
					}
					if (current.Result is null || current.Result.LexSegmentId is null
						|| current.Result.Content is null)
					{
						break;
					}
					context.Consume();
					if (SectionHeaders.IndexOf(current.Result.LexSegmentId) >= 0)
					{
						break;
					}

					result.AddError(new PatternMismatchError(current.Result, "IDENTIFIER COLON"));
					break;
				}
			}
			return result;
		}

		private static OperationResult<Match?> ParseMatch(ParseContext context)
		{
			OperationResult<Match?> result = new OperationResult<Match?>(null);
			var matchPattern = context.PatternMatchingIDsFloor(GeneralIgnorance, "IDENTIFIER");
			if (result.CheckAndInheritErrorAndWarnings(matchPattern))
			{
				return result;
			}
				Match match = new Match();
			if (matchPattern.Result.Item1)
			{
				match.Pattern = matchPattern.Result.Item2.ConvertAll(segment => segment.Content ?? "");
				var startOfTargets = context.PatternMatchingIDs(GeneralIgnorance, "ARROW_BRACE");
				if (result.CheckAndInheritErrorAndWarnings(startOfTargets))
				{
					return result;
				}
				while (true)
				{
					var peek = context.Peek();
					if (result.CheckAndInheritErrorAndWarnings(peek))
					{
						return result;
					}
					if (peek.Result == null) { break; }
					if (peek.Result.LexSegmentId == "RBRACE")
					{
						context.Consume();
						break;
					}
					if (peek.Result.LexSegmentId == "NEWLINE")
						context.Consume();

					var actions = context.PatternMatchingIDsFloor(GeneralIgnoranceWONewLine, "IDENTIFIER");
					if (result.CheckAndInheritErrorAndWarnings(actions))
					{
						break;
					}
					if (actions.Result.Item2.Count > 0)
					{
						Action action = new Action();
						action.Segments = actions.Result.Item2.ConvertAll(segment => segment.Content ?? "");
						match.Target.Actions.Add(action);
					}
				}
			}
			result.Result= match;
			return result;
		}
		
	}

}
