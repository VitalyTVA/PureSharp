using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.ReaderMonad;
using PureSharp.MayBeMonad2;
using PureSharp.ReaderMayBeMonad2;
using static PureSharp.ReaderMayBeMonad2.ReaderMayBe2Extensions;
using System;
using ReaderMayBeString = PureSharp.ReaderMayBeMonad2.ReaderMayBe<string, string>;

namespace PureSharp.Tests {
    [TestFixture]
    public class ReaderMayBe2Tests {
        [Test]
        public void ReaderTest() {
            AskTest().Value("x").Value.IsEqual<string>("X");
            Simple("x".AsReaderMayBe<string, string>()).Value("").Value.IsEqual<string>("X");
            Sum("x".AsReaderMayBe<string, string>()).Value("y").Value.IsEqual<string>("YX");
            Sum2("x".AsReaderMayBe<string, string>()).Value("y").Value.IsEqual<string>("YYYXx");
            Sum2("d".AsReaderMayBe<string, string>()).Value("y").Value.IsNull<string>();
            string nullString = null;
            Sum2(nullString.AsReaderMayBe<string, string>()).Value("y").Value.IsNull<string>();
            Sum2("d".AsReaderMayBe<string, string>()).Value(null).Value.IsNull<string>();
        }

        static ReaderMayBeString Simple(ReaderMayBeString a) {
            return from x in a
                   select x.ToUpper();
        }

        static ReaderMayBeString AskTest() {
            return from x in Ask<string>()
                   select x.ToUpper();
        }
        static ReaderMayBeString Sum(ReaderMayBeString a) {
            return from x in Ask<string>()
                   from y in a
                   let t = x + y
                   select t.ToUpper();
        }
        static ReaderMayBeString Sum2(ReaderMayBeString a) {
            return from x in Ask<string>()
                   from y in Local(e => e + e, Sum(a))
                   from z in a
                   where z != "d"
                   select x.ToUpper() + y + z;
        }
    }
}