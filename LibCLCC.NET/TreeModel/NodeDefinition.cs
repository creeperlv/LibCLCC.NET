using System.Collections.Generic;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// Basic definition of a node.
	/// </summary>
	public class NodeDefinition
	{
		/// <summary>
		/// The name of the node.
		/// </summary>
		public string Name;
		/// <summary>
		/// Max count of children. &lt;0 for infinite children, 0 for no child, and any positive number is a value fot
		/// </summary>
		public int MaxChildrenCount;
		/// <summary>
		/// Properties of the node.
		/// </summary>
		public List<string> Properties;
	}
}
