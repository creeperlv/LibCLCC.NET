using LibCLCC.NET.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LibCLCC.NET.TreeModel
{
    /// <summary>
    /// TreeProvider in xml format/
    /// </summary>
    public class XmlTreeProvider : ITreeProvider
    {
        /// <summary>
        /// Treat names that contains a dot as properties or named children.e.g:
		/// &lt;button.child&gt;&lt;TextBlock/&gt;&lt;/button.child&gt;
        /// 
        /// </summary>
        public bool TreatNameContainDotAsPropertyAndNamedChildren=true;
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
        /// Remove all providers in the provider list.
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
            if (!Hit)
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
                if(TreatNameContainDotAsPropertyAndNamedChildren)
                if (childNode.Name.Contains("."))
                {
                    var cname = childNode.Name;
                    var Names = childNode.Name.Split(".");
                    {
                        var key = cname;

                        if (Names [ 0 ] == Name)
                            key = cname [ (Names [ 0 ].Length + 1).. ];
                        if (childNode.ChildNodes.Count > 0)
                        {
                            if (childNode.ChildNodes.Count == 1)
                                if (childNode.ChildNodes [ 0 ] is XmlText text)
                                {
                                    node.SetProperty(key , text.Value.Trim());
                                    continue;
                                }
                            foreach (var CChild in childNode.ChildNodes)
                            {
                                if (CChild is XmlElement ChildElement)
                                {
                                    var ParseResult = ParseTree(ChildElement);
                                    if (FinalResult.CheckAndInheritErrorAndWarnings(ParseResult))
                                    {
                                        return FinalResult;
                                    }
                                    else
                                    {
                                        var AddResult = node.AddNamedChild(key , ParseResult.Result);
                                        if (FinalResult.CheckAndInheritErrorAndWarnings(AddResult))
                                        {
                                            return FinalResult;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var content = childNode.InnerText;
                            node.SetProperty(key , content.Trim());
                        }
                    }
                    continue;
                }
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
            foreach (var item in xmlDocument.ChildNodes)
            {
                if (item is XmlElement xe)
                {
                    return ParseTree(xe);
                }
            }
            {
                OperationResult<GeneralTree> result = new OperationResult<GeneralTree>(null);
                result.AddError<NotAValidXmlContent>();
                return result;
            }
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
        XmlElement ToXmlNode(XmlDocument document , GeneralTree node)
        {
            var element = document.CreateElement(node.GetName());
            var ps = node.GetAllProperties();
            if (ps != null)
                foreach (var item in ps)
                {
                    element.SetAttribute(item.Key , item.Value);
                }
            var cs = node.GetChildren();
            if (cs != null)
                foreach (var item in cs)
                {
                    var child = ToXmlNode(document , item);
                    element.AppendChild(child);
                }
            var ncs = node.GetAllNamedChildren();
            if (ncs != null)
            {

            }
            return element;
        }
        /// <summary>
        /// Serialize to string using XmlDocument.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public OperationResult<string> SerializeTree(GeneralTree tree)
        {
            XmlDocument document = new XmlDocument();
            var root = ToXmlNode(document , tree);
            document.AppendChild(root);
            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter sw = new StringWriter(stringBuilder);
            document.Save(sw);
            return sw.ToString();
        }
        /// <summary>
        /// Serialize to a stream using XmlDocument
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public OperationResult<bool> SerializeTree(GeneralTree tree , Stream stream)
        {
            XmlDocument document = new XmlDocument();
            var root = ToXmlNode(document , tree);
            document.AppendChild(root);
            document.Save(stream);
            return true;
        }
    }
    /// <summary>
    /// The input xml content is not a valid xml content.
    /// </summary>
    public class NotAValidXmlContent : Error { }
}
