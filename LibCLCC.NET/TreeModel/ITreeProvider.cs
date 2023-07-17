using LibCLCC.NET.Operations;
using System.IO;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// Provides ability to parse a tree.
	/// </summary>
	public interface ITreeProvider
	{
		/// <summary>
		/// Parse a tree from the content.
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		OperationResult<GeneralTree> ParseTree(string content);
		/// <summary>
		/// Parse a tree from a stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		OperationResult<GeneralTree> ParseTree(Stream stream);
		/// <summary>
		/// Serialize a tree to string.
		/// </summary>
		/// <param name="tree"></param>
		/// <returns></returns>
		public OperationResult<string> SerializeTree(GeneralTree tree);
		/// <summary>
		/// Serialize a tree to stream.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="stream"></param>
		/// <returns></returns>
		public OperationResult<bool> SerializeTree(GeneralTree tree,Stream stream);
		/// <summary>
		/// Add a provider.
		/// </summary>
		/// <param name="provider"></param>
		void AddNodeProvider(INodeProvider provider);
		/// <summary>
		/// Remove a provider
		/// </summary>
		/// <param name="provider"></param>
		void RemoveNodeProvider(INodeProvider provider);
		/// <summary>
		/// Clear internal provider list.
		/// </summary>
		void ClearProviders();
	}
}
