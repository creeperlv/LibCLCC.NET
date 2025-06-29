using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using LibCLCC.NET.Parser.ActionEngine;
using LibCLCC.NET.Parser;
using System.Collections.Generic;

namespace ParserFramework.Core.ActionEngine.StdActions
{
	/// <summary>
	/// Action to add a child ASTNode to another ASTNode.
	/// Syntax: add-children &lt;parent_node_name&gt; &lt;child_key&gt; &lt;child_source_ref&gt;
	/// Example: add-children node_2 LHS $1
	/// </summary>
	public class AddChildrenAction : AEngineAction
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
			if (args.Count < 4)
			{
				result.AddError(new InvalidActionArgumentsError("add-children: Not enough arguments. Expected: add-children <parent_node_name> <child_key> <child_source_ref>"));
				return result;
			}

			string parentNodeName = args[1];
			string childKey = args[2];
			string childSource = args[3]; // e.g., "$1", "$2"

			if (!context.CurrentNodes.TryGetValue(parentNodeName, out ASTNode? parentNode) || parentNode == null)
			{
				result.AddError(new NodeNotCreatedError(parentNodeName));
				return result;
			}

			ASTNode? childNodeToAdd = null;

			if (childSource.StartsWith("$") && int.TryParse(childSource.Substring(1), out int index))
			{
				if (index > 0 && index <= context.Parameters.Count)
				{
					object? sourceItem = context.Parameters[index - 1]; // $1 means index 0
					if (sourceItem is ASTNode astNode)
					{
						childNodeToAdd = astNode;
					}
					else
					{
						result.AddError(new InvalidActionArgumentsError($"add-children: '{childSource}' refers to a LexSegment, but 'add-children' expects an ASTNode reference (result of a sub-rule parse)."));
						return result;
					}
				}
			}

			if (childNodeToAdd == null)
			{
				result.AddError(new InvalidActionArgumentsError($"add-children: Invalid child source '{childSource}'. Expected an ASTNode reference (e.g., $1 referring to a rule)."));
				return result;
			}

			if (!parentNode.Children.ContainsKey(childKey))
			{
				parentNode.Children[childKey] = new List<ASTNode>();
			}
			parentNode.Children[childKey].Add(childNodeToAdd);
			return result;
		}
	}

}
