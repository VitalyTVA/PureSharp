using System;
using PureSharp.MayBeMonad2;
using PureSharp.ReaderMonad;
using PureSharp.ReaderMonad.ReaderExtensions;

namespace PureSharp.ReaderMayBeMonad2 {
    public static partial class ReaderMayBe2Extensions {
        public static ReaderMayBe<A, A> AskMayBe<A>() {
            return Ask<A>().LiftMayBe().AsReaderMayBe();
        }
        public static ReaderMayBe<E, A> LocalMayBe<E, A>(Func<E, E> f, ReaderMayBe<E, A> reader) {
            Func<E, MayBe<A>> t = x => reader.Value(f(x));
            return t.AsReaderMayBe();
        }
    }
}