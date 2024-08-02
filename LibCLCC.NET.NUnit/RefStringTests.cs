using LibCLCC.NET.Data;
using System.Diagnostics;

namespace LibCLCC.NET.NUnit
{
	public class RefStringTests
	{
		string ori = "/A/B/C//D\\E";
		[SetUp]
		public void Setup()
		{
		}
		[Test]
		public void SubStringTest()
		{
			string str = "This is a sentence. A very long sentence.";
			RefString rstr = str;
			var SubString0 = rstr.Substring(0, 4);
			{
				string str2 = "This";
				Assert.That(SubString0 == str2);
			}
			{
				string str2 = "That";
				Assert.That(SubString0 != str2);
			}
			{
				string str2 = "Tha";
				Assert.That(SubString0 != str2);
			}
			{
				string str2 = "This is not equal.";
				Assert.That(SubString0 != str2);
			}
			var SubString1 = rstr.Substring(5);
			{
				Assert.That(SubString1 == str.Substring(5));
			}
			{
				Assert.That(SubString1 == ((RefString)str).Substring(5));
			}
			{
				Assert.That(SubString1.StartsWith("is"));
			}
		}
		[Test]
		public void EndsWithTest()
		{
			RefString rstr = "new RefString()";
			RefString R0 = "()";
			Assert.That(rstr.EndsWith(R0));
			Assert.That(rstr.Substring(2).EndsWith(R0));
			Assert.That(rstr.Substring(0,3).EndsWith("ew"));
		}
		[Test]
		public void ParsingTest()
		{
			string input = "ASD123.001DEF";
			RefString A = input;
			A.Offset = 3;
			A.Length = 3;
			{
				Assert.Multiple(() =>
				{
					Assert.That(A.TryParse(out int v), Is.True);
					Assert.That(v, Is.EqualTo(123));
				});
			}

			A.Length += 4;
			{
				Assert.Multiple(() =>
				{
					Assert.That(A.TryParse(out float v), Is.True);
					Assert.That(v, Is.EqualTo(123.001f));
				});
			}

			A.Length += 1;
			{
				Assert.Multiple(() =>
				{
					Assert.That(A.TryParse(out double v), Is.True);
					Assert.That(v, Is.EqualTo(123.001));
				});
			}

			string Testee_00 = "ASD";
			string Testee_01 = "DEF";
			A.Offset = 0;
			A.Length = input.Length;
			Assert.Multiple(() =>
			{
				Assert.That(A.StartsWith(Testee_00), Is.True);
				Assert.That(A.StartsWith(Testee_01), Is.False);
				Assert.That(A.EndsWith(Testee_00), Is.False);
				Assert.That(A.EndsWith(Testee_01), Is.True);
			});
		}

		[Test]
		public void DataTypeTest()
		{
			RefString BaseString0 = new RefString(ori, 0, ori.Length);
			var e = BaseString0.Split('/', '\\');
			var comp = ori.Split('/', '\\');
			int i = 0;
			while (e.MoveNext())
			{
				var c = e.Current.FinalizeString();
				Assert.That(c, Is.EqualTo(comp[i]));
				i++;
			}

		}
	}
}