using LibCLCC.NET.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LibCLCC.NET.TreeModel
{
	/// <summary>
	/// TreeProvider in xml format/
	/// </summary>
	public class XmlTreeProvider : ITreeProvider
	{
		List<INodeProvider> providers = new List<INodeProvider>();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		public void AddNodeProvider(INodeProvider provider)
		{
			providers.Add(provider);
		}
		/// <summary>
		/// 
		/// </summary>
		public void ClearProviders()
		{
			providers.Clear();
		}
		/// <summary>
		/// Parse to tree.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public OperationResult<GeneralTree> ParseTree(XmlElement element)
		{
			OperationResult<GeneralTree> FinalResult = new OperationResult<GeneralTree>();
			bool Hit = false;
			string Name = element.Name;
			foreach (var provider in providers)
			{
				var newNode = provider.CreateNode(Name);
				if (newNode.HasError())
				{
					continue;
				}
				if (newNode.Result == null)
				{
				}
				else
				{
					FinalResult.Result = newNode.Result;
					Hit = true;
					break;
				}

			}
			if (Hit)
			{
				FinalResult.AddError(new TreeNodeTypeNotDefinedError(Name));
				return FinalResult;
			}
			var node = FinalResult.Result;
			foreach (XmlAttribute attribute in element.Attributes)
			{
				node.SetProperty(attribute.Name , attribute.Value);
			}
			foreach (XmlElement childNode in element.ChildNodes)
			{
				var pr = ParseTree(childNode);
				if (FinalResult.CheckAndInheritErrorAndWarnings(pr))
				{
					return FinalResult;
				}
				else
				{
					var AddResult = node.AddChild(pr.Result);
					if (FinalResult.CheckAndInheritErrorAndWarnings(AddResult))
					{
						return FinalResult;
					}
				}
			}
			return FinalResult;
		}
		/// <summary>
		/// Parse the tree from input string.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public OperationResult<GeneralTree> ParseTree(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			var root = xmlDocument.FirstChild as XmlElement;
			return ParseTree(root);
		}
		/// <summary>
		/// Parse tree from stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public OperationResult<GeneralTree> ParseTree(Stream stream)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(stream);
			var root = xmlDocument.FirstChild as XmlElement;
			return ParseTree(root);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		public void RemoveNodeProvider(INodeProvider provider)
		{
			providers.Remove(provider);
		}

		public OperationResult<string> SerializeTree(GeneralTree tree)
		{
			throw new NotImplementedException();
		}

		public OperationResult<bool> SerializeTree(GeneralTree tree , Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
