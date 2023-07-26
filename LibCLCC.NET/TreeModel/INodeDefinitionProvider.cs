using System.Collections.Generic;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// Additional and optional provider interface.
	/// </summary>
	public interface INodeDefinitionProvider
	{
		/// <summary>
		/// Should return known nodes.
		/// </summary>
		/// <returns></returns>
		List<NodeDefinition> GetNodeNames();
	}
}
