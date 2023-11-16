using LibCLCC.NET.Data;

namespace LibCLCC.NET.NUnit
{
    public class RefStringTests
    {
        string ori="/A/B/C//D\\E";
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void DataTypeTest()
        {
            RefString BaseString0 = new RefString(ori , 0 , ori.Length);
            var e=BaseString0.Split('/','\\');
            var comp=ori.Split('/','\\');
            int i=0;
            while(e.MoveNext())
            {
                var c=e.Current.FinalizeString();
                Assert.That(c, Is.EqualTo(comp [i]));
                i++;
            }

        }
    }
}