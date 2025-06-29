using LibCLCC.NET.Parser.ActionEngine.Errors;
using LibCLCC.NET.Operations;
using System.Collections.Generic;
#nullable enable
namespace LibCLCC.NET.Parser.ActionEngine
{
	/// <summary>
	/// Action Engine that used in target.
	/// </summary>
	public class AEngine
	{
		/// <summary>
		/// Known actions.
		/// </summary>
		public Dictionary<string, AEngineAction>? Actions;
		/// <summary>
		/// Execute target action.
		/// </summary>
		/// <param name="__action"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public OperationResult<bool> Execute(Action __action, ParseRuleScopeContext context)
		{
			OperationResult<bool> result = false;
			if (__action.Segments.Count == 0) return true;
			if (Actions is null)
			{
				result.AddError(new ActionNotFoundError(__action.Segments[0]));
				return result;
			}
			if (Actions.TryGetValue(__action.Segments[0], out var action))
			{
				// The user provided interface had `ParseContext context List<string> args`. Corrected to `ParseContext context, List<string> args`
				return action.Execute(this, context, __action.Segments);
			}
			result.AddError(new ActionNotFoundError(__action.Segments[0]));
			return result;
		}
	}
	/// <summary>
	/// Interface for Action Engine Action.
	/// </summary>
	public interface AEngineAction
	{
		/// <summary>
		/// Entry point for an action.
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="context"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		OperationResult<bool> Execute(AEngine engine, ParseRuleScopeContext context, List<string> args);
	}

}
