using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.ReaderMonad;
using PureSharp.MayBeMonad2;
using PureSharp.ReaderMayBeMonad2;
using PureSharp.ReaderMonad.ReaderExtensions;
using PureSharp.ReaderMayBeMonad2.ReaderMayBe2Extensions;
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
            //Test()("a").IsEqual("A");

            //string value = string.Empty;
            //var reader = Test(x => value);
            //value = "b";
            //reader("a").IsEqual("AB");
            //value = "c";
            //reader("a").IsEqual("AC");

            //reader = Test(value.AsReader<string, string>());
            //value = "d";
            //reader("a").IsEqual("AC");

            //reader = Local(e => e + e, Test(x => value));
            //reader("a").IsEqual("AAD");

            //reader = Test2(x => value);
            //reader("b").IsEqual("BBBDd");
        }

        static ReaderMayBeString Simple(ReaderMayBeString a) {
            return from x in a
                   select x.ToUpper();
        }

        static ReaderMayBeString AskTest() {
            return from x in AskMayBe<string>()
                   select x.ToUpper();
        }
        static ReaderMayBeString Sum(ReaderMayBeString a) {
            return from x in AskMayBe<string>()
                   from y in a
                   let t = x + y
                   select t.ToUpper();
        }
        //static ReaderMayBeString Sum2(ReaderMayBeString a) {
        //    return from x in Ask<string>()
        //           from y in Local<string, string>(e => e + e, Sum(a)).LiftMayBe().AsReaderMayBe()
        //           from z in a
        //           select x.ToUpper() + y + z;
        //}
    }
}