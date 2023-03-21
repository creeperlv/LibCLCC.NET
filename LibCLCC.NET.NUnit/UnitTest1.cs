using LibCLCC.NET.TextProcessing;
using System.Diagnostics;

namespace LibCLCC.NET.NUnit {
    public class Tests {
        string _content;
        GeneralPurposeParser parser;
        [SetUp]
        public void Setup() {
            _content = "namespace LibCLCC.NET.NUnit {\r\n    public class Tests {\r\n\tstring _content;\r\n        [SetUp]\r\n        public void Setup() {\r\n            _content = \"\";\r\n        }\r\n\r\n        [Test]\r\n        public void Test1() {\r\n            Assert.Pass();\r\n        }\r\n    }\r\n}";
            parser=new GeneralPurposeParser();

        }

        [Test]
        public void Test1() {
            var s=parser.Parse(_content,false);
            Console.WriteLine(s.SequentialToString(" "));
            Assert.Pass();
        }
    }
}