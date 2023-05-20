using LibCLCC.NET.TextProcessing;
using System.Diagnostics;

namespace LibCLCC.NET.NUnit
{
    public class Tests
    {
        string _content;
        string __content;
        GeneralPurposeParser parser;
        CommandLineParser CLP;
        [SetUp]
        public void Setup()
        {
            _content = "namespace LibCLCC.NET.NUnit {\r\n    public class Tests {\r\n\tstring _content;\r\n        [SetUp]\r\n //asdas d\r\n   /**asdsa asdasda **/    public void Setup() {\r\n            _content = \"a b\";\r\n        }\r\n\r\n        [Test]\r\n        public void Test1() {\r\n            Assert.Pass();\r\n        }\r\n    }\r\n}";
            __content = "rm /* -rf --no-preserve-root";
            parser = new GeneralPurposeParser();
            CLP = new CommandLineParser();
        }
        [Test]
        public void FloatPointTest()
        {
                CStyleParser cStyleParser = new CStyleParser();
            {

                var float_str_0 = "0.0";
                var o = cStyleParser.Parse(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=0.0";
                var o = cStyleParser.Parse(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=0.0f";
                var o = cStyleParser.Parse(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=.0f";
                var o = cStyleParser.Parse(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "if(1.0==.0f){return 1.1f>=0.0f;}";
                var o = cStyleParser.Parse(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
        }
        [Test]
        public void Test1()
        {
            parser.lineCommentIdentifiers.Add(new LineCommentIdentifier { StartSequence = "//" });
            parser.closableCommentIdentifiers.Add(new ClosableCommentIdentifier {  Start="/*", End="*/"});
            var s = parser.Parse(_content, false, "117");
            Console.WriteLine(s.SequentialToString("->", true, false));
            var _s = CLP.Parse(__content, false);
            Console.WriteLine(_s.SequentialToString(" "), true);
            s.Concatenate(_s);
            Console.WriteLine(s.SequentialToString("->", true, true));
            Assert.Pass();
        }
    }
}