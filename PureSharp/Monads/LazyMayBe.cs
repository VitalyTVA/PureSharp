using System;
using PureSharp.MayBeMonad;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad {
    public struct LazyMayBe<A>(Lazy<A> value) {
        public readonly Lazy<A> Value = value;
    }
    public static partial class LazyMayBeExtensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this A source) {
            return source.Unit();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<A> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<A> source) {
            return source.AsLazy().AsLazyMayBe();
        }
    }
}