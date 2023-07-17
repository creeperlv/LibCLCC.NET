using LibCLCC.NET.Operations;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// Provides node with given name.
	/// </summary>
	public interface INodeProvider
	{
		/// <summary>
		/// Create a node.
		/// <br/>
		/// TreeNodeTypeNotDefinedError should be added if a name have no corresponding node defined.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		OperationResult<GeneralTree> CreateNode(string name);
	}
}
