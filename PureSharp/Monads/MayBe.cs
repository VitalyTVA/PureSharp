using System;

namespace PureSharp.MayBeMonad {
    //public struct LazyMayBe<A>(Lazy<A> value) {
    //    public readonly Lazy<A> Value = value;
    //}
    public static partial class MayBeExtensions {
        static A Unit<A>(this A source) {
            return source;
        }
        public static A Empty<A>() {
            return default(A);
        }
        static B SelectMany<A, B>(this A source, Func<A, B> f) {
            if(source == null)
                return default(B);
            return f(source);
        }
    }
}