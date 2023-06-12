using LibCLCC.NET.TextProcessing;
using LibCLCC.NET.Data;
using System.Diagnostics;

namespace LibCLCC.NET.NUnit
{
    public class Tests
    {
        string? _content;
        string? __content;
        GeneralPurposeScanner? parser;
        CommandLineScanner? CLP;
        [SetUp]
        public void Setup()
        {
            _content = "namespace LibCLCC.NET.NUnit {\r\n    public class Tests {\r\n\tstring _content;\r\n        [SetUp]\r\n //asdas d\r\n   /**asdsa asdasda **/    public void Setup() {\r\n            _content = \"a b\";\r\n        }\r\n\r\n        [Test]\r\n        public void Test1() {\r\n            Assert.Pass();\r\n        }\r\n    }\r\n}";
            __content = "rm /* -rf --no-preserve-root";
            parser = new GeneralPurposeScanner();
            CLP = new CommandLineScanner();
        }
        [Test]
        public void DataTypeTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That("0.1f".TryParse(out float _) , Is.True);
                Assert.That("0.1f".TryParse(out double _) , Is.False);
                Assert.That("0.1d".TryParse(out double _) , Is.True);
                Assert.That("0.1d".TryParse(out float _) , Is.False);
                Assert.That("0.1_f".TryParse(out int _) , Is.False);
                Assert.That("0x112_EDA3".TryParse(out int _) , Is.True);
                Assert.That("0o11_23".TryParse(out int _) , Is.True);
                Assert.That("-0o9asd".TryParse(out int _) , Is.False);
                Assert.That("-0b1110001010_1010".TryParse(out long _) , Is.True);
                Assert.That("-0b1110001010_1010".TryParse(out int _) , Is.True);
                Assert.That("-0b1110001010_1010".TryParse(out ulong _) , Is.False);
                Assert.That("-0b1110001010_1010".TryParse(out uint _) , Is.False);
            });
            {
                Assert.Multiple(() =>
                {
                    Assert.That("123".TryParse(out int d) , Is.True);
                    Assert.That(d , Is.EqualTo(123));
                });
            }
        }

        [Test]
        public void NumberScannerTest()
        {
            var input = "int main(){ float f = 1.023e-3; float f1=123e--3; float f2=1.232; double d=1.232d; double d1=1.232e-3; return 0}";
            CStyleScanner scanner = new CStyleScanner();
            var o = scanner.Scan(input , false , null);
            Console.WriteLine(o.SequentialToString("]>--<["));
        }
        [Test]
        public void FloatPointTest()
        {
            CStyleScanner cStyleParser = new CStyleScanner();
            {

                var float_str_0 = "0.0";
                var o = cStyleParser.Scan(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=0.0";
                var o = cStyleParser.Scan(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=0.0f";
                var o = cStyleParser.Scan(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "=.0f";
                var o = cStyleParser.Scan(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
            {

                var float_str_0 = "if(1.0==.0f){return 1.1f>=0.0f;}";
                var o = cStyleParser.Scan(float_str_0 , false , null);
                Console.WriteLine(o.SequentialToString(" "));
                FloatPointScanner.ScanFloatPoint(ref o);
                Console.WriteLine(o.SequentialToString(" "));
            }
        }
        [Test]
        public void Test1()
        {
            Assert.That(parser , Is.Not.Null);
            Assert.That(CLP , Is.Not.Null);
            if (parser == null) return;
            if (CLP == null) return;
            parser.lineCommentIdentifiers.Add(new LineCommentIdentifier { StartSequence = "//" });
            parser.closableCommentIdentifiers.Add(new ClosableCommentIdentifier { Start = "/*" , End = "*/" });
            var s = parser.Scan(_content , false , "117");
            Console.WriteLine(s.SequentialToString("->" , true , false));
            var _s = CLP.Scan(__content , false);
            Console.WriteLine(_s.SequentialToString(" ") , true);
            s.Concatenate(_s);
            Console.WriteLine(s.SequentialToString("->" , true , true));
            Assert.Pass();
        }
    }
}