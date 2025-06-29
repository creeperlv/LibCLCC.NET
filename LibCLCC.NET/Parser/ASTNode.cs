using LibCLCC.NET.Lexer;
using System;
using System.Collections.Generic;

namespace LibCLCC.NET.Parser
{
	/// <summary>
	/// Node of the Abstract Syntax Tree.
	/// </summary>
	[Serializable]
	public class ASTNode
	{
		/// <summary>
		/// Type of the node.
		/// </summary>
		public int Type;
		/// <summary>
		/// Children of the node, key is set by actions.
		/// </summary>
		public Dictionary<string, List<ASTNode>> Children=new ();
		/// <summary>
		/// Properties of the node, key is set by actions.
		/// </summary>
		public Dictionary<string, LexSegment> Properties=new ();
	}
}
