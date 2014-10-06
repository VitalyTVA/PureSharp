using System;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad2 {
    public struct LazyMayBe<A>(Lazy<MayBe<A>> value) {
        public readonly Lazy<MayBe<A>> Value = value;
    }
    public static partial class LazyMayBe2Extensions {
        public static LazyMayBe<A> AsLazyM<A>(this A source) {
            return source.Unit();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<MayBe<A>> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<MayBe<A>> source) {
            return source.AsLazy().AsLazyMayBe();
        }
        static LazyMayBe<A> Unit<A>(this A source) {
            return source.AsMayBe().AsLazy().AsLazyMayBe();
        }
        static LazyMayBe<A> Empty<A>() {
            return MayBe2Extensions.Empty<A>().AsLazy().AsLazyMayBe();
        }
        static LazyMayBe<B> SelectMany<A, B>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f) {
            return LazyMonad.LazyExtensions.SelectMany<MayBe<A>, MayBe<B>>(
                source.Value,
                x => (x.Value != null ? f(x.Value).Value : LazyMonad.LazyExtensions.AsLazy(MayBe2Extensions.Empty<B>()))
            ).AsLazyMayBe();
        }
    }
}