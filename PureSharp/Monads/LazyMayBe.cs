using System;
using PureSharp.MayBeMonad;
using PureSharp.LazyMonad;

namespace PureSharp.LazyMayBeMonad {
    public static partial class LazyMayBeExtensions {
        public static Lazy<A> AsLazyM<A>(this A source) {
            return source.Unit();
        }
        static Lazy<A> Unit<A>(this A source) {
            return source.AsLazy();
        }
        static Lazy<A> Empty<A>() {
            return MayBeExtensions.Empty<A>().AsLazy();
        }
        static Lazy<B> SelectMany<A, B>(this Lazy<A> source, Func<A, Lazy<B>> f) {
            return LazyMonad.LazyExtensions.SelectMany<A, B>(
                source,
                x => (x != null ? f(x) : LazyMonad.LazyExtensions.AsLazy(MayBeExtensions.Empty<B>()))
            );
        }
    }
}