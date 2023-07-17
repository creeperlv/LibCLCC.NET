using LibCLCC.NET.Operations;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// Indicates the node is not defined.
	/// </summary>
	public class TreeNodeTypeNotDefinedError : Error {
		/// <summary>
		/// The name that not defined.
		/// </summary>
		public string TargetTypeName;
		/// <summary>
		/// Indicates the node is not defined.
		/// </summary>
		/// <param name="targetTypeName"></param>
		public TreeNodeTypeNotDefinedError(string targetTypeName)
		{
			TargetTypeName = targetTypeName;
		}
		/// <summary>
		/// The error message.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"Tree Node Type \"{TargetTypeName}\" is not defined.";
		}
	}
}
