using System;

namespace PureSharp.LazyMonad {
    public static partial class LazyExtensions {
        public static Lazy<A> AsLazy<A>(this A source) {
            return Unit(source);
        }
        public static Lazy<A> AsLazy<A>(this Func<A> f) {
            return new Lazy<A>(f);
        }

        static Lazy<A> Unit<A>(this A source) {
            return new Lazy<A>(() => source);
        }
        static Lazy<B> SelectMany<A, B>(this Lazy<A> source, Func<A, Lazy<B>> f) {
            return new Lazy<B>(() => f(source.Value).Value);
        }
    }
}