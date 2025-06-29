using LibCLCC.NET.Lexer;
using LibCLCC.NET.Operations;
using LibCLCC.NET.Parser.ActionEngine;
using LibCLCC.NET.Parser.ActionEngine.Errors;
using ParserFramework.Core.ActionEngine.StdActions;
using ParserFramework.Core.Errors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
#nullable enable
namespace LibCLCC.NET.Parser
{
	/// <summary>
	/// ParserEngine. Example Parser Rule Definition:
	/// <code>
	///NodeId: --auto
	///n_constant
	///n_variable
	///binary_expr
	///unary_expr
	///encapsulated_expr
	///expr
	///Rule:
	///	
	///expr: expr ^bOp expr
	///	=> {
	///		create-node node expr
	///		add-children node LHS $1
	///		set-content node $2
	///		add-children node RHS $3
	///		return node
	///}
	///	| ^uOp expr
	///	=> {
	///		create - node node expr
	///		set - content node $1
	///		add - children node RHS $2
	///		return node
	///	}
	///| ^LP expr ^ RP
	///	=>{
	///	create - node node expr
	///		add - children node real_expr $2
	///		return node
	///	}| constant
	///	=>{
	///	return $1
	///	}
	///	| variable
	///	=>{
	///	return $1
	///	}
	///	;
	///
	///
	///constant: ^number
	///	=> {
	///	create - node node n_constant
	///		set - content node $1.content
	///		return node
	///
	///	}
	///;
	/// </code>
	/// </summary>
	public class ParseEngine
	{
		private AEngine _actionEngine = new AEngine();
		/// <summary>
		/// Create a new instance of the ParseEngine. and registers the standard actions.
		/// </summary>
		public ParseEngine()
		{
			RegisterStandardActions();
		}
		/// <summary>
		/// Parse contents from ILexer using the provided ParseDefinition.
		/// </summary>
		/// <param name="definition"></param>
		/// <param name="lexer"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public OperationResult<List<ASTNode>> Parse(ParseDefinition definition, ILexer lexer, ParseContext? context = null)
		{
			OperationResult<List<ASTNode>> result = new OperationResult<List<ASTNode>>(new List<ASTNode>());
			context ??= new ParseContext(lexer, definition, _actionEngine);
			ASTNode? lastNode = null;
			while (true)
			{
				if (context.IsEnd()) break;
				bool isSuccess = false;
				OperationResult<List<ASTNode>> tresult = new OperationResult<List<ASTNode>>();

				foreach (var item in definition.Rules)
				{
					var rContext = new ParseRuleScopeContext(context, context.currentTokenIndex,
						context.ShowLog, context.AlternativeLogAction);
					rContext.ParentLatest = lastNode;
					var r = ParseRule(item.Value, rContext);
					if (tresult.CheckAndInheritErrorAndWarnings(r))
					{
					}
					else
					{
						if (r.Result)
						{
							if (result.CheckAndInheritErrorAndWarnings(tresult))
							{
								isSuccess = false;
								break;
							}
							isSuccess = true;
							if (rContext.ReturnValue != null)
							{
								if (rContext.ConsumedParentLatest)
								{
									result.Result[^1] = (rContext.ReturnValue);
								}
								else
									result.Result.Add(rContext.ReturnValue);
								lastNode = rContext.ReturnValue;
							}
							continue;
						}
					}
				}
				if (isSuccess == false)
				{
					var seg = context.Peek().Result ?? new LexSegment() { Content = "<null>" };
					Console.WriteLine($"\"{seg.Content}\"({seg.Position}) not matching any rule!");
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Registers the standard built-in actions for the AEngine.
		/// These actions are used to manipulate the AST during parsing.
		/// </summary>
		private void RegisterStandardActions()
		{
			_actionEngine.Actions = new Dictionary<string, AEngineAction>
		{
			{"create-node", new CreateNodeAction()},
			{"set-content", new SetContentAction()},
			{"add-children", new AddChildrenAction()},
			{"return", new ReturnAction()}
		};
		}



		private OperationResult<bool> ParseRule(Rule rule, ParseRuleScopeContext context)
		{
			context.LogLine($"Rule: {rule.RuleName}");
			OperationResult<bool> ruleResult = new OperationResult<bool>();
			bool isSuccess = false;
			int cIndex = context.context.currentTokenIndex;
			for (int i = 0; i < rule.Matches.Count; i++)
			{
				context.context.currentTokenIndex = cIndex;
				if (i == context.LastMatchSelection) continue;
				Match? matchItem = rule.Matches[i];
				List<object?> parameters = new List<object?>();
				bool IsMismatch = false;
				context.LogLine("  Match: " + string.Join(" ", matchItem.Pattern));
				ASTNode? Lastest = null;
				int lastestIndex = -1;
				for (int i1 = 0; i1 < matchItem.Pattern.Count; i1++)
				{
					string? item = matchItem.Pattern[i1];
					context.LogLine("\tCheck: " + item);
					if (item.StartsWith("^"))
					{
						var lex = item[1..];
						LexSegment? lresult = context.context.Peek().Result;
						if (lresult == null)
						{
							ruleResult.AddError(new NotMatchingLexError(item));
							IsMismatch = true;
							break;
						}
						context.LogLine($"\t\t{lex} == {lresult.LexSegmentId}?");
						if (lex == lresult.LexSegmentId)
						{

							context.LogLine($"\t\t{lex} == {lresult.LexSegmentId}: true");
							var cLexSeg = context.context.Consume();
							if (cLexSeg.Result is null)
							{
								ruleResult.AddError(new ErrorWContent("Validated but no usable context."));
								IsMismatch = true;
								break;
							}
							parameters.Add(cLexSeg.Result);
						}
						else
						{
							IsMismatch = true;
							break;
						}
					}
					else
					{
						if (i1 == 0)
						{

							if (context.context.Definition.ASTTypes.TryGetValue(item, out int astType))
							{
								{
									if (context.ParentLatest != null)
									{
										if (context.ParentLatest.Type == astType)
										{
											parameters.Add(context.ParentLatest);
											context.ConsumedParentLatest = true;
											Lastest = context.ParentLatest;
											lastestIndex = i1;
											continue;
										}
									}
								}
							}
						}
						if (context.context.Definition.Rules.TryGetValue(item, out var r))
						{
							var child = context.CreateChild(context.context.currentTokenIndex, i);
							if (lastestIndex == i1 - 1)
								child.ParentLatest = Lastest;
							var cResult = ParseRule(r, child);
							if (ruleResult.CheckAndInheritErrorAndWarnings(cResult))
							{
								IsMismatch = true;
								break;
							}
							else
							{
								if (!cResult.Result)
								{
									ruleResult.AddError(new RuleMatchingExpectedError(item));
									IsMismatch = true;
									break;
								}
								else
								{
									context.LogLine($"Result:{child.ReturnValue}");
									Lastest = child.ReturnValue;
									if (child.ConsumedParentLatest)
									{
										parameters[lastestIndex] = child.ReturnValue;
									}
									else
										parameters.Add(child.ReturnValue);
									lastestIndex = i1;
								}
							}
						}
						else
						{
							ruleResult.AddError(new RuleNotFoundError(item));
							IsMismatch = true;
							break;
						}
					}
				}
				if (IsMismatch)
				{
					continue;
				}
				isSuccess = true;
				if (matchItem.Target is not null)
				{
					context.Parameters = parameters;
					foreach (var item in matchItem.Target.Actions)
					{
						OperationResult<bool> actionResult = context.context.ActionEngine.Execute(item, context);
						if (ruleResult.CheckAndInheritErrorAndWarnings(actionResult))
						{
							isSuccess = true;
							foreach (var err in actionResult.Errors)
							{
								Console.WriteLine($"Action Error: {err} in action:{string.Join(" ", item.Segments)}");
							}
							break;
						}
					}
				}
				break;
			}
			ruleResult.Result = isSuccess;
			if (isSuccess)
			{
				context.LogLine($"\tMatched rule:{rule.RuleName}");
			}
			return ruleResult;
		}
	}
}
