using System;
using PureSharp.MayBeMonad2;
using PureSharp.ReaderMonad;
using PureSharp.ReaderMonad.ReaderExtensions;

namespace PureSharp.ReaderMayBeMonad2 {
    public static partial class ReaderMayBe2Extensions {
        public static ReaderMayBe<A, A> AskMayBe<A>() {
            return Ask<A>().LiftMayBe().AsReaderMayBe();
        }
    }
}