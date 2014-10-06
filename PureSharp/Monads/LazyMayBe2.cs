using System;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad2 {
    public struct LazyMayBe<A>(Lazy<MayBe<A>> value) {
        public readonly Lazy<MayBe<A>> Value = value;
    }
    public static partial class LazyMayBe2Extensions {
        public static LazyMayBe<A> AsLazyMayBe<A>(this A source) {
            return source.Unit();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<MayBe<A>> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this MayBe<A> source) {
            return source.AsLazy().AsLazyMayBe();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<MayBe<A>> source) {
            return source.AsLazy().AsLazyMayBe();
        }
    }
}