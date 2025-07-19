using LibCLCC.NET.Operations;
using LibCLCC.NET.TextProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LibCLCC.NET.Lexer
{
#nullable enable
	/// <summary>
	/// A Lexer Match Definition.
	/// </summary>
	[Serializable]
	public class LexMatch
	{
		/// <summary>
		/// The regex, * means capture anystring that before closest capture.
		/// </summary>
		public string? Regex = null;
		/// <summary>
		/// The Id of the match item.
		/// </summary>
		public string? Item = null;
	}
	/// <summary>
	/// Lexer Definition
	/// </summary>
	[Serializable]
	public class LexerDefinition
	{
		/// <summary>
		/// Matches.
		/// </summary>
		public List<LexMatch> Matches = new List<LexMatch>();
		/// <summary>
		/// Mapped Id, multiple match item id can map to same segment id, useful for parsers in the future.
		/// </summary>
		public Dictionary<string, string> LexSegmentIds = new Dictionary<string, string>();
		/// <summary>
		/// Enclosure that uses different Left and Right regexes.
		/// </summary>
		public Dictionary<string, ValueTuple<string, string>> Enclosures = new Dictionary<string, (string, string)>();
		static LexDefScanner scanner = new LexDefScanner();
		static bool isKW(Segment seg)
		{
			switch (seg.content)
			{
				case "Match:":
					return true;
				case "Id:":
					return true;
				case "Enclosure:":
					return true;
				default:
					break;
			}
			return false;
		}
		string substitute(string str)
		{
			foreach (var item in Matches)
			{
				str = str.Replace($"{{{item.Item}}}", item.Regex);
			}
			return str;
		}
		void substitute()
		{
			List<string> list = new List<string>();
			Dictionary<int, string> buffer = new Dictionary<int, string>();
			Dictionary<string, ValueTuple<string, string>> buffer2 = new Dictionary<string, (string, string)>();
			for (int i = 0; i < Matches.Count; i++)
			{
				LexMatch? item = Matches[i];
				if (item.Regex is null) continue;
				var l = substitute(item.Regex);
				if (item.Regex != l)
				{
					item.Regex = l;
				}
			}
			//foreach (var item in buffer)
			//{
			//	Matches.Add(item.Key, item.Value);
			//}
			//foreach (var item in list)
			//{
			//	Matches.Remove(item);
			//}
			foreach (var item in Enclosures.Keys)
			{
				var V = Enclosures[item];
				var Lsubbed = substitute(V.Item1);
				var Rsubbed = substitute(V.Item2);
				Enclosures[item] = (Lsubbed, Rsubbed);
			}
		}
		/// <summary>
		/// This will substitute references to existing regexes. Like:
		/// <br/>
		///<code>
		///Number {D}+ 
		///D [0-9]
		///</code>
		///Will become:
		///<code>
		///Number {0-9}+ 
		///D [0-9]
		///</code>
		/// </summary>
		public void Substitute()
		{
			substitute();
			substitute();
		}
		/// <summary>
		/// Try to parse a string to a lexer definition.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="definition"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static bool TryParse(string input, [MaybeNullWhen(false)] out LexerDefinition definition, string? fileName = null)
		{
			var HEAD = scanner.Scan(input, false, fileName);
			var Current = HEAD;
			int Selection = 0;
			LexerDefinition __definition = new LexerDefinition();
			while (true)
			{
				var content = Current.content;
				if (content == "")
					goto AdvanceStep;
				switch (content)
				{
					case "Match:":
						Selection = 0;
						goto AdvanceStep;
					case "Id:":
						Selection = 1;
						goto AdvanceStep;
					case "Enclosure:":
						Selection = 2;
						goto AdvanceStep;
					default:
						break;
				}
				switch (Selection)
				{
					case 0:
						{
							var ID = Current;
							var RegexString = Current.Next;
							if (RegexString == null)
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							if (isKW(RegexString))
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							Current = Current.Next;
							__definition.Matches.Add(new LexMatch { Regex = RegexString.content, Item = ID.content });
						}
						break;
					case 1:
						{
							var ItemID = Current;
							var LexID = Current.Next;
							if (LexID == null)
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							if (isKW(LexID))
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							Current = Current.Next;
							__definition.LexSegmentIds.Add(ItemID.content, LexID.content);
						}
						break;
					case 2:
						{
							var ItemID = Current;
							var LReg = Current.Next;
							if (LReg == null)
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							if (isKW(LReg))
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							var RReg = LReg.Next;
							if (RReg == null)
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							if (isKW(RReg))
							{
								Console.WriteLine(Current.SequentialToString());
								definition = null;
								return false;
							}
							Current = Current.Next;
							Current = Current.Next;
							__definition.Enclosures.Add(ItemID.content, (LReg.content, RReg.content));
						}
						break;
					default:
						break;
				}
			AdvanceStep:
				Current = Current.Next;
				if (Current == null) break;
			}
			definition = __definition;
			return true;
		}
	}
	/// <summary>
	/// The scanner for lexer definition file.
	/// </summary>
	public class LexDefScanner :
			GeneralPurposeScanner
	{
		/// <summary>
		/// Initialize the scanner.
		/// </summary>
		public LexDefScanner()
		{
			this.PredefinedSegmentTemplate.Add("Match:");
			this.PredefinedSegmentTemplate.Add("Id:");
			this.PredefinedSegmentTemplate.Add("Enclosure:");
			this.lineCommentIdentifiers.Add(new LineCommentIdentifier() { StartSequence = "//" });
			this.lineCommentIdentifiers.Add(new LineCommentIdentifier() { StartSequence = "--" });
			this.lineCommentIdentifiers.Add(new LineCommentIdentifier() { StartSequence = "#" });
			this.closableCommentIdentifiers.Add(new ClosableCommentIdentifier() { Start = "/*", End = "*/" });
		}
	}
	/// <summary>
	/// Scanned result.
	/// </summary>
	[Serializable]
	public class LexSegment
	{
		/// <summary>
		/// Result Content.
		/// </summary>
		public string? Content = null;
		/// <summary>
		/// Segment ID (different from match item id).
		/// </summary>
		public string? LexSegmentId = null;
		/// <summary>
		/// Matched Item ID
		/// </summary>
		public string? LexMatchedItemId = null;
		/// <summary>
		/// The identifier of the source. (could be a filename).
		/// </summary>
		public string? SourceID = null;
		/// <summary>
		/// The position in the file.
		/// </summary>
		public LexSegmentPosition Position;
	}
	/// <summary>
	/// Definition of position of a lexer segment.
	/// </summary>
	[Serializable]
	public struct LexSegmentPosition
	{
		/// <summary>
		/// Left Line Number.
		/// </summary>
		public uint LLine;
		/// <summary>
		/// Left Position in its line.
		/// </summary>
		public uint LPos;
		/// <summary>
		/// Right Line Number;
		/// </summary>
		public uint RLine;
		/// <summary>
		/// Right Position in its line.
		/// </summary>
		public uint RPos;
		/// <summary>
		/// Convert to 0:0-1:1 format.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{LLine}:{LPos}-{RLine}:{RPos}";
		}
	}
	/// <summary>
	/// Interface for lexers.
	/// </summary>
	public interface ILexer
	{
		/// <summary>
		/// Set the lexer definition.
		/// </summary>
		/// <param name="Definition"></param>
		public void SetDefinition(LexerDefinition? Definition);
		/// <summary>
		/// Analysis and give next lexer segment.
		/// </summary>
		/// <returns></returns>
		OperationResult<LexSegment?> Lex();
	}
}
