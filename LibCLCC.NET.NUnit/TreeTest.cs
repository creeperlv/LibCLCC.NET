using LibCLCC.NET.Operations;
using LibCLCC.NET.TreeModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCLCC.NET.NUnit
{
	public class ExampleNode0 : GeneralTree
	{
		public List<GeneralTree> Nodes = new List<GeneralTree>();
		public override OperationResult<bool> AddChild(GeneralTree child)
		{
			Nodes.Add(child);
			return true;
		}
		public override List<GeneralTree> GetChildren()
		{
			return Nodes;
		}
		public Dictionary<string , string> keyValuePairs = new Dictionary<string , string>();
		public override bool SetProperty(string name , string value)
		{
			keyValuePairs.Add(name , value);
			return true;
		}
		public override Dictionary<string , string> GetAllProperties()
		{
			return keyValuePairs;
		}
		public override string GetName() => "Node0";
	}
	public class ExampleNode1 : ExampleNode0
	{
		public override string GetName() => "Node1";
	}
	public class ExampleTreeNodeProvider : INodeProvider
	{
		public OperationResult<GeneralTree> CreateNode(string name)
		{
			OperationResult<GeneralTree> result = new OperationResult<GeneralTree>(new ExampleNode0());
			switch (name)
			{
				case "Node0":
					return new ExampleNode0();
				case "Node1":
					return new ExampleNode1();
				default:
					break;
			}
			return result;
		}
	}
	public class TreeTest
	{
		[Test]
		public void ParseFromXml()
		{
			var content = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Node0><Node1 a=\"ASD\">" +
				"<Node0 B=\"a123\" />" +
				"<Node1><Node1.B>a1234</Node1.B></Node1>" +
				"<Node0 B=\"a1231\" />" +
				"</Node1></Node0>";
			XmlTreeProvider provider = new XmlTreeProvider();
			provider.AddNodeProvider(new ExampleTreeNodeProvider());
			var result = provider.ParseTree(content);
			Assert.That(result.HasError() , Is.False);
			var e0 = result.Result.GetChildren() [ 0 ];
			var e1 = e0.GetChildren() [ 1 ];
			//var e2 = e1.GetChildren() [ 1 ];
			var d = e1.GetAllProperties();
			Assert.That(d [ "B" ] , Is.EqualTo("a1234"));
		}
		[Test]
		public void SerializeToXml()
		{
			ExampleNode0 exampleNode0 = new ExampleNode0();
			ExampleNode0 exampleNode1 = new ExampleNode0();
			ExampleNode0 exampleNode2 = new ExampleNode0();
			ExampleNode1 exampleNode3 = new ExampleNode1();
			exampleNode2.SetProperty("B" , "1B");
			exampleNode3.SetProperty("B" , "1");
			exampleNode0.AddChild(exampleNode1);
			exampleNode1.AddChild(exampleNode2);
			exampleNode1.AddChild(exampleNode3);
			XmlTreeProvider provider = new XmlTreeProvider();
			provider.AddNodeProvider(new ExampleTreeNodeProvider());
			var sr=provider.SerializeTree(exampleNode0);
			Console.WriteLine(sr.Result);
			Assert.That(sr.HasError() , Is.False);
		}
	}
}
