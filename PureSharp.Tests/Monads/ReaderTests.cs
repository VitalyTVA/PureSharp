using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.ReaderMonad;
using static PureSharp.ReaderMonad.ReaderExtensions;
using System;

namespace PureSharp.Tests {
    [TestFixture]
    public class ReaderTests {
        [Test]
        public void ReaderTest() {
            Test()("a").IsEqual("A");

            string value = string.Empty;
            var reader = Test(x => value);
            value = "b";
            reader("a").IsEqual("AB");
            value = "c";
            reader("a").IsEqual("AC");

            reader = Test(value.AsReader<string, string>());
            value = "d";
            reader("a").IsEqual("AC");

            reader = Local(e => e + e, Test(x => value));
            reader("a").IsEqual("AAD");

            reader = Test2(x => value);
            reader("b").IsEqual("BBBDd");
        }

        static Func<string, string> Test() {
            return from x in Ask<string>()
                   select x.ToUpper();
        }
        static Func<string, string> Test(Func<string, string> a) {
            return from x in Ask<string>()
                   from y in a
                   let t = x + y
                   select t.ToUpper();
        }
        static Func<string, string> Test2(Func<string, string> a) {
            return from x in Ask<string>()
                   from y in Local(e => e + e, Test(a))
                   from z in a
                   select x.ToUpper() + y + z;
        }
    }
}