using System;
using PureSharp.MayBeMonad;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad {
    public static partial class LazyMayBeExtensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this A source) {
            return source.Unit();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<A> source) {
            return source.AsLazy().AsLazyMayBe();
        }
    }
}