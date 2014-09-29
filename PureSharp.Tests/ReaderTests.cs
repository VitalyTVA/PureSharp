using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.ReaderMonad;
using PureSharp.ReaderMonad.ReaderExtensions;
using System;

namespace PureSharp.Tests {
    [TestFixture]
    public class ReaderTests {
        [Test]
        public void ReaderTest() {
            Test()("a").IsEqual("A");

            string value = string.Empty;
            var reader = Test(e => value);
            value = "b";
            reader("a").IsEqual("AB");
            value = "c";
            reader("a").IsEqual("AC");
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
    }
}