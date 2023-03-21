using LibCLCC.NET.TextProcessing;
using System.Diagnostics;

namespace LibCLCC.NET.NUnit {
    public class Tests {
        string _content;
        string __content;
        GeneralPurposeParser parser;
        CommandLineParser CLP;
        [SetUp]
        public void Setup() {
            _content = "namespace LibCLCC.NET.NUnit {\r\n    public class Tests {\r\n\tstring _content;\r\n        [SetUp]\r\n        public void Setup() {\r\n            _content = \"a\";\r\n        }\r\n\r\n        [Test]\r\n        public void Test1() {\r\n            Assert.Pass();\r\n        }\r\n    }\r\n}";
            __content = "rm /* -rf --no-preserve-root";
            parser=new GeneralPurposeParser();
            CLP=new CommandLineParser();
        }

        [Test]
        public void Test1() {
            var s=parser.Parse(_content,false);
            Console.WriteLine(s.SequentialToString(" "));
            s=CLP.Parse(__content,false);
            Console.WriteLine(s.SequentialToString(" "));

            Assert.Pass();
        }
    }
}