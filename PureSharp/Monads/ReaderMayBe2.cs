using System;
using PureSharp.MayBeMonad2;
using PureSharp.ReaderMonad;
using static PureSharp.F;

namespace PureSharp.ReaderMayBeMonad2 {
    public static partial class ReaderMayBe2Extensions {
        public static ReaderMayBe<A, A> Ask<A>() {
            return ReaderExtensions.Ask<A>().LiftMayBe().AsReaderMayBe();
        }
        public static ReaderMayBe<E, A> Local<E, A>(Func<E, E> f, ReaderMayBe<E, A> reader) {
            return ReaderExtensions.Local(f, reader.Value).AsReaderMayBe();
        }
    }
}