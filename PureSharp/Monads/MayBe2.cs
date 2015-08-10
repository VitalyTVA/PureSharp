using System;

namespace PureSharp.MayBeMonad2 {
    public struct MayBe<T> {
        public T Value { get; }
        public MayBe(T value) {
            Value = value;
        }
    }
    public static partial class MayBe2Extensions {
        public static Func<A, MayBe<B>> LiftMayBe<A, B>(this Func<A, B> f) {
            return x => f(x).Unit();
        }
        public static MayBe<A> AsMayBe<A>(this A source) {
            return source.Unit();
        }
        static MayBe<A> Unit<A>(this A source) {
            return new MayBe<A>(source);
        }
        public static MayBe<A> Empty<A>() {
            return default(A).Unit();
        }
        public static MayBe<B> SelectMany<A, B>(this MayBe<A> source, Func<A, MayBe<B>> f) {
            if(source.Value == null)
                return Empty<B>();
            return f(source.Value);
        }
    }
}