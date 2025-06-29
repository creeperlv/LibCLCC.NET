using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using LibCLCC.NET.Parser.ActionEngine;
using LibCLCC.NET.Parser;
using System.Collections.Generic;

namespace ParserFramework.Core.ActionEngine.StdActions
{
	/// <summary>
	/// Action to specify the ASTNode to be returned by the current rule.
	/// Syntax: return &lt;node_name&gt;
	/// Example: return node0
	/// </summary>
	public class ReturnAction : AEngineAction
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
			if (args.Count < 2)
			{
				result.AddError(new InvalidActionArgumentsError("return: Not enough arguments. Expected: return <node_name>"));
				return result;
			}

			string nodeName = args[1];
			if (nodeName.StartsWith("$"))
			{
				if (nodeName.StartsWith("$") && int.TryParse(nodeName.Substring(1), out int index))
				{
					if (index > 0 && index <= context.Parameters.Count)
					{
						object? sourceItem = context.Parameters[index - 1]; // $1 means index 0
						if (sourceItem is ASTNode astNode)
						{
							context.ReturnValue = astNode;
							return result;
						}
						else
						{
							result.AddError(new InvalidActionArgumentsError($"return-node: '{nodeName}' is not a node."));
							return result;
						}
					}
				}
			}
			if (context.CurrentNodes.TryGetValue(nodeName, out ASTNode? nodeToReturn))
			{
				context.ReturnValue = nodeToReturn;
			}
			else
			{
				result.AddError(new NodeNotCreatedError(nodeName));
			}
			return result;
		}
	}

}
