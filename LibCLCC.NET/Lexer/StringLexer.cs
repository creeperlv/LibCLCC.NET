using LibCLCC.NET.Operations;
using System;
using System.Text.RegularExpressions;

namespace LibCLCC.NET.Lexer
{
	/// <summary>
	/// A Lexer that takes a string as input.
	/// </summary>
	#nullable enable
	public class StringLexer : ILexer
	{
		/// <summary>
		/// Input content
		/// </summary>
		public string? Content;
		/// <summary>
		/// Source Identifier
		/// </summary>
		public string? SourceID;
		int Index = 0;
		uint CurrentLine = 1;
		uint CurrentPos = 0;
		LexerDefinition? Definition = null;
		/// <summary>
		/// Analysis and give next segment.
		/// </summary>
		/// <returns>Null means reach the end.</returns>
		public OperationResult<LexSegment?> Lex()
		{
			OperationResult<LexSegment?> result = new OperationResult<LexSegment?>();
			result.Result = null;
			if (Content is null) return result;
			if (Definition is null) return result;
			if (Index >= Content.Length) return result;
			var c = Content[Index..];
			foreach (var item in Definition.Enclosures)
			{
				var L = Regex.Match(c, item.Value.Item1);
				if (L.Success)
				{
					if (L.Index == 0)
					{

						LexSegment segment = new LexSegment();
						var R = Regex.Match(c, item.Value.Item2);
						segment.Position.LPos = (uint)(CurrentPos + L.Length);
						segment.Position.LLine = CurrentLine;
						if (R.Success)
						{

							Index += R.Index + R.Length;
							segment.Content = c[L.Length..(R.Index - 1)];

							var __content = c[0..(R.Index + R.Length)];
							var lines = Regex.Matches(__content, "\n\r?");
							CurrentLine += (uint)lines.Count;
							segment.Position.RLine = CurrentLine;
							segment.Position.RPos = (uint)(R.Index - lines[^1].Index - lines[^1].Length);
							if (lines.Count > 0)
							{
								CurrentPos = (uint)(R.Index + R.Length - lines[^1].Index - lines[^1].Length);
							}
							segment.Position.RPos = CurrentPos;
						}
						else
						{
							segment.Content = c[L.Length..];
							var lines = Regex.Matches(segment.Content, "\n\r?");
							Index = Content.Length;
							CurrentLine += (uint)lines.Count;
							segment.Position.RLine = CurrentLine;
						}
						segment.SourceID = SourceID;
						if (Definition.LexSegmentIds.TryGetValue(item.Key, out var id))
						{
							segment.LexSegmentId = id;
						}
						else
							segment.LexSegmentId = null;
						result.Result = segment;
						return result;
					}

				}
			}
			uint min = (uint)c.Length;
			string? wildcardID = null;
			foreach (var item in Definition.Matches)
			{
				if (item.Regex == "*")
				{
					wildcardID = item.Item;
					continue;
				}

				var m = Regex.Match(c, item.Regex);
				if (m.Success)
				{
					min = Math.Min(min, (uint)m.Index);
					if (m.Index == 0)
					{
						if (item.Item is null) continue;
						LexSegment segment = new LexSegment();
						segment.Content = m.Value;
						segment.SourceID = SourceID;
						segment.Position.LLine = CurrentLine;
						segment.Position.LPos = CurrentPos;
						if (Definition.LexSegmentIds.TryGetValue(item.Item, out var id))
						{
							segment.LexSegmentId = id;
						}
						else
							segment.LexSegmentId = null;
						var lines = Regex.Matches(segment.Content, "\n\r?");
						CurrentLine += (uint)lines.Count;
						if ((uint)lines.Count > 0)
						{
							CurrentPos = 0;
						}
						CurrentPos += (uint)m.Length;
						segment.Position.RLine = CurrentLine;
						segment.Position.RPos = CurrentPos;
						Index += m.Length;
						result.Result = segment;
						break;
					}
				}
			}
			if (min != 0)
			{
				LexSegment segment = new LexSegment();
				segment.Content = c[0..(int)min];
				segment.SourceID = SourceID;
				segment.Position.LLine = CurrentLine;
				segment.Position.LPos = CurrentPos;
				if (wildcardID != null && Definition.LexSegmentIds.TryGetValue(wildcardID, out var id))
				{
					segment.LexSegmentId = id;
				}
				else
					segment.LexSegmentId = null;
				var lines = Regex.Matches(segment.Content, "\n\r?");
				CurrentLine += (uint)lines.Count;
				if ((uint)lines.Count > 0)
				{
					CurrentPos = 0;
				}
				CurrentPos += (uint)min;
				segment.Position.RLine = CurrentLine;
				segment.Position.RPos = CurrentPos;
				Index += (int)min;
				result.Result = segment;
			}
			return result;
		}

		/// <summary>
		/// The the definition of current StringLexer.
		/// </summary>
		/// <param name="Definition"></param>
		public void SetDefinition(LexerDefinition? Definition)
		{
			this.Definition = Definition;
		}
	}
}
