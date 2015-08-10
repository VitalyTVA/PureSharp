using NUnit.Framework;
using PureSharp.Tests.Utils;
using static PureSharp.Monoid;
using PureSharp.WriterMonad;
using static PureSharp.WriterMonad.WriterExtensions;
using System;
using IntStringWriter = PureSharp.WriterMonad.Writer<string, int>;
using LongSringWriter = PureSharp.WriterMonad.Writer<string, long>;

namespace PureSharp.Tests {
    [TestFixture]
    public class WriterTests {
        [Test]
        public void ReaderTest() {
            DoubleSimple(1.AsWriter("Got 1.", StringMonoid))
                .IsEqual(x => x.Value, 2)
                .IsEqual(x => x.Log, "Got 1.");

            Double(2.AsWriter("Got 2.", StringMonoid))
                .IsEqual(x => x.Value, 4)
                .IsEqual(x => x.Log, "Got 2.Doubled 2.");

            Sum(2.AsWriter("Got 2.", StringMonoid), ((long)3).AsWriter("Got 3.", StringMonoid))
                .IsEqual(x => x.Value, (long)5)
                .IsEqual(x => x.Log, "Got 2.Got 3.!Sum 2 + 3.");
        }

        static IntStringWriter DoubleSimple(IntStringWriter a) {
            return from x in a
                   select x * 2;
        }

        static IntStringWriter Double(IntStringWriter a) {
            return from x in a
                   from _ in Tell("Doubled " + x + ".", StringMonoid)
                   select x * 2;
        }
        static LongSringWriter Sum(IntStringWriter a, LongSringWriter b) {
            return from x in a
                   from y in b.Append("!")
                   from _ in Tell("Sum " + x + " + " + y + ".", StringMonoid)
                   select x + y;
        }
    }
}