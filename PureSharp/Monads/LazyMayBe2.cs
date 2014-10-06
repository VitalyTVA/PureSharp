using System;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad2 {
    public static partial class LazyMayBe2Extensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<MayBe<A>> source) {
            return source.AsLazy().AsLazyMayBe();
        }
    }
}