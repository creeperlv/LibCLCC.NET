using LibCLCC.NET.Operations;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// General Purpose Tree.
	/// </summary>
	public class GeneralTree
	{
		/// <summary>
		/// Get the name that a provider can use.
		/// </summary>
		/// <returns></returns>
		public virtual string GetName() => "GeneralTree";
		/// <summary>
		/// Add an child
		/// </summary>
		/// <param name="child"></param>
		/// <returns></returns>
		public virtual OperationResult<bool> AddChild(GeneralTree child)
		{
			return false;
		}
		/// <summary>
		/// Get the children of this node.
		/// </summary>
		/// <returns></returns>
		public virtual List<GeneralTree> GetChildren()
		{
			return null;
		}
		/// <summary>
		/// Set a property
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual bool SetProperty(string name , string value)
		{
			return false;
		}
		/// <summary>
		/// Returns all properties.
		/// </summary>
		/// <returns></returns>
		public virtual Dictionary<string , string> GetAllProperties()
		{
			return new Dictionary<string , string>();
		}
	}
}
