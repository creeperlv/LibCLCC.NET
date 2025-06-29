using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using LibCLCC.NET.Parser.ActionEngine;
using LibCLCC.NET.Parser;
using System.Collections.Generic;
using System.Text;

namespace ParserFramework.Core.ActionEngine.StdActions
{

	/// <summary>
	/// create-node action to create a new ASTNode.
	/// </summary>
	public class CreateNodeAction : AEngineAction
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
				result.AddError(new InvalidActionArgumentsError("create-node: Not enough arguments. Expected: create-node <node_name> <ast_type_id>"));
				return result;
			}

			string nodeName = args[1];
			string astTypeString = args[2];

			if (context.context.Definition == null || !context.context.Definition.ASTTypes.TryGetValue(astTypeString, out int astTypeInt))
			{
				result.AddError(new InvalidActionArgumentsError($"create-node: Unknown AST type '{astTypeString}' defined in ResultDefinition."));
				return result;
			}
			ASTNode newNode = new ASTNode { Type = astTypeInt };
			context.CurrentNodes[nodeName] = newNode; // Store the new node in the context
			return result;
		}
	}

}
