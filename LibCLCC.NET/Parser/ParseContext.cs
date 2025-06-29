using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using LibCLCC.NET.Parser.ActionEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace LibCLCC.NET.Parser
{
	/// <summary>
	/// The context of a parse operation.
	/// </summary>
	public class ParseContext
	{
		/// <summary>
		/// The action engine used to execute actions defined in each target of match.
		/// </summary>
		public AEngine ActionEngine;
		/// <summary>
		/// Target parse definition.
		/// </summary>
		public ParseDefinition Definition;
		/// <summary>
		/// If print the log to Console.
		/// </summary>
		public bool ShowLog = false;
		/// <summary>
		/// Alternative log action to be used instead of Console.WriteLine. (Not controlled by showLog).
		/// </summary>
		public Action<int, string>? AlternativeLogAction = null;
		ILexer _lexer;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lexer"></param>
		/// <param name="definition"></param>
		/// <param name="actionEngine"></param>
		public ParseContext(ILexer lexer, ParseDefinition definition, AEngine actionEngine)
		{
			_lexer = lexer;
			Definition = definition;
			ActionEngine = actionEngine;
		}
		/// <summary>
		/// Buffer of lex segments.
		/// </summary>
		public List<LexSegment> _tokenBuffer = new List<LexSegment>();
		/// <summary>
		/// Index of the current token in the buffer.
		/// </summary>
		public int currentTokenIndex;
		/// <summary>
		/// Check if the current token is the end of the input.
		/// </summary>
		/// <returns></returns>
		public bool IsEnd()
		{
			return Peek().Result == null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ignoredIds"></param>
		/// <returns></returns>
		public OperationResult<LexSegment?> Peek(List<string>? ignoredIds = null)
		{
			if (currentTokenIndex < _tokenBuffer.Count)
			{
				return _tokenBuffer[currentTokenIndex];
			}

			OperationResult<LexSegment?> lexResult;
			while (true)
			{
				lexResult = _lexer.Lex();
				if (lexResult.HasError()) return lexResult;

				if (lexResult.Result != null)
				{
					if (lexResult.Result.LexSegmentId != null)
						if (ignoredIds != null)
						{
							if (ignoredIds.Contains(lexResult.Result.LexSegmentId))
							{
								continue;
							}
						}
					_tokenBuffer.Add(lexResult.Result);
					break;
				}
				else
				{
					break;
				}
			}
			return lexResult;
		}
		/// <summary>
		/// Consume the token. Which will advance the index.
		/// </summary>
		/// <param name="ignoredIds"></param>
		/// <returns></returns>
		public OperationResult<LexSegment?> Consume(List<string>? ignoredIds = null)
		{
			OperationResult<LexSegment?> tokenResult = Peek(ignoredIds);
			if (tokenResult.HasError()) return tokenResult;

			if (tokenResult.Result != null)
			{
				currentTokenIndex++;
				return tokenResult.Result;
			}
			return new OperationResult<LexSegment?>(null);
		}
		/// <summary>
		/// Matching patterns using Peek and Consume.
		/// </summary>
		/// <param name="ignoredIds"></param>
		/// <param name="allowedPatterns"></param>
		/// <returns></returns>
		public OperationResult<(bool, List<LexSegment>)> PatternMatchingIDsFloor(List<string>? ignoredIds = null,
																					params string[] allowedPatterns)
		{

			OperationResult<(bool, List<LexSegment>)> result = new OperationResult<(bool, List<LexSegment>)>();
			result.Result = (true, new List<LexSegment>());

			while (true)
			{
				var current = Peek(ignoredIds);
				if (result.CheckAndInheritErrorAndWarnings(current))
				{
					result.Result.Item1 = false;
					return result;
				}
				if (current.Result is null)
				{
					break;
				}
				if (current.Result.LexSegmentId is null)
				{
					break;
				}
				if (!allowedPatterns.Contains(current.Result.LexSegmentId))
				{
					if (ignoredIds != null)
						if (ignoredIds.Contains(current.Result.LexSegmentId))
						{
							Consume(ignoredIds);
							continue;
						}
					break;
				}
				else
				{
					result.Result.Item2.Add(current.Result);
					Consume(ignoredIds);
				}
			}
			return result;
		}
		/// <summary>
		/// Matching patterns using Peek and Consume.
		/// </summary>
		/// <param name="ignoredIds"></param>
		/// <param name="patterns"></param>
		/// <returns></returns>
		public OperationResult<(bool, List<LexSegment>)> PatternMatchingIDs(List<string>? ignoredIds = null,
			params string[] patterns)
		{
			OperationResult<(bool, List<LexSegment>)> result = new OperationResult<(bool, List<LexSegment>)>();
			List<LexSegment> resultList = new();
			result.Result = (true, resultList);
			for (int i = 0; i < patterns.Length; i++)
			{
				string? item = patterns[i];
				var current = Peek();
				if (result.CheckAndInheritErrorAndWarnings(current))
				{

					result.Result.Item1 = false;
					return result;
				}
				if (current.Result is null)
				{
					result.Result.Item1 = false;
					return result;
				}
				if (current.Result.LexSegmentId is null)
				{
					result.Result.Item1 = false;
					return result;
				}
				if (current.Result.LexSegmentId != item)
				{
					if (ignoredIds != null)
						if (ignoredIds.Contains(current.Result.LexSegmentId))
						{
							Consume();
							i--;
							continue;
						}
					result.Result.Item1 = false;
					return result;
				}
				else
				{
					resultList.Add(current.Result);
					Consume();
				}
			}
			return result;
		}
	}
}
