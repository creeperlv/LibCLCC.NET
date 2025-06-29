using LibCLCC.NET.Operations;
using ParserFramework.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.Parser.ActionEngine.Errors
{
	/// <summary>
	/// Target action not found error.
	/// </summary>
	[Serializable]
	public class ActionNotFoundError : ErrorWContent
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		public ActionNotFoundError(string content) : base($"Action Not Found: {content}")
		{
		}
	}
}
