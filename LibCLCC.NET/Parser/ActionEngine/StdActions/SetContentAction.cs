using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using LibCLCC.NET.Parser.ActionEngine;
using LibCLCC.NET.Parser;
using System;
using System.Collections.Generic;

namespace ParserFramework.Core.ActionEngine.StdActions
{
	/// <summary>
	/// set-content action to set the content of an ASTNode.
	/// </summary>
	public class SetContentAction : AEngineAction
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="context"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public OperationResult<bool> Execute(AEngine engine, ParseRuleScopeContext context, List<string> args)
		{
			OperationResult<bool> result = new OperationResult<bool>(true);
			if (args.Count < 3)
			{
				result.AddError(new InvalidActionArgumentsError("set-content: Not enough arguments. Expected: set-content <node_name> <source_ref>"));
				return result;
			}

			string nodeName = args[1];
			string contentSource = args[2]; // e.g., "$1.content" or "$1"

			if (!context.CurrentNodes.TryGetValue(nodeName, out ASTNode? targetNode) || targetNode == null)
			{
				result.AddError(new NodeNotCreatedError(nodeName));
				return result;
			}

			if (contentSource.StartsWith("$") && int.TryParse(contentSource.Substring(1).Split('.')[0], out int index))
			{
				if (index > 0 && index <= context.Parameters.Count)
				{
					object? sourceItem = context.Parameters[index - 1]; // $1 means index 0

					if (contentSource.Contains(".content")) // Case: set-content node0 $1.content
					{
						if (sourceItem is LexSegment lexSegment)
						{
							targetNode.Properties["content"] = lexSegment;
							return result;
						}
						else
						{
							result.AddError(new InvalidActionArgumentsError($"set-content: '{contentSource}' refers to a non-LexSegment item. '.content' property is only applicable to LexSegment."));
							return result;
						}
					}
					else // Case: set-content node_name $1 (assuming $1 is a LexSegment for content)
					{
						if (sourceItem is LexSegment lexSegment)
						{
							targetNode.Properties["content"] = lexSegment; // Stores the LexSegment directly as a property
							return result;
						}
						else
						{
							result.AddWarning(new Warning());
							Console.WriteLine($"Warning: set-content: '{contentSource}' refers to an ASTNode or unrecognized type. 'content' property typically expects a LexSegment.");
							result.AddError(new InvalidActionArgumentsError($"set-content: '{contentSource}' refers to an ASTNode or unrecognized item type. 'content' property typically expects LexSegment."));
							return result;
						}
					}
				}
			}
			result.AddError(new InvalidActionArgumentsError($"set-content: Invalid content source format or index '{contentSource}'."));
			return result;
		}
	}

}
