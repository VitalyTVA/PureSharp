using System;
using PureSharp.LazyExtensions;

namespace PureSharp.LazyMonad {
    public static partial class LazyExtensions {
        public static Lazy<A> AsLazy<A>(this A source) {
            return Unit(source);
        }
        public static Lazy<A> AsLazy<A>(this Func<A> f) {
            return Lazy(f);
        }

        static Lazy<A> Unit<A>(this A source) {
            return Lazy(() => source);
        }
        public static Lazy<B> SelectMany<A, B>(this Lazy<A> source, Func<A, Lazy<B>> f) {
            return Lazy(() => f(source.Value).Value);
        }
    }
}