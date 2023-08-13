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
		/// Add a child with name, e.g:
		/// &lt;button.child&gt;&lt;TextBlock/&gt;&lt;/button.child&gt;
		/// </summary>
		/// <param name="name"></param>
		/// <param name="Child"></param>
		/// <returns></returns>
		public virtual OperationResult<bool> AddNamedChild(string name,GeneralTree Child)
		{
			return false;
		}
		/// <summary>
		/// Get all of named children. Including every children of every name.
		/// </summary>
		/// <returns></returns>
		public virtual Dictionary<string,List<GeneralTree>> GetAllNamedChildren()
		{
			return new Dictionary<string, List<GeneralTree>>();
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
