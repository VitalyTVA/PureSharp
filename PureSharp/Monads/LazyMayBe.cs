using System;
using PureSharp.MayBeMonad;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad {
    public struct LazyMayBe<A>(Lazy<A> value) {
        public readonly Lazy<A> Value = value;
    }
    public static partial class LazyMayBeExtensions {
        public static LazyMayBe<A> AsLazyM<A>(this A source) {
            return source.Unit();
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Lazy<A> source) {
            return new LazyMayBe<A>(source);
        }
        public static LazyMayBe<A> AsLazyMayBe<A>(this Func<A> source) {
            return source.AsLazy().AsLazyMayBe();
        }

        static LazyMayBe<A> Unit<A>(this A source) {
            return source.AsLazy().AsLazyMayBe();
        }
        static LazyMayBe<A> Empty<A>() {
            return MayBeExtensions.Empty<A>().AsLazyM();
        }
        static LazyMayBe<B> SelectMany<A, B>(this LazyMayBe<A> source, Func<A, LazyMayBe<B>> f) {
            return LazyMonad.LazyExtensions.SelectMany<A, B>(
                source.Value,
                x => (x != null ? f(x).Value : LazyMonad.LazyExtensions.AsLazy(MayBeExtensions.Empty<B>()))
            ).AsLazyMayBe();
        }
    }
}