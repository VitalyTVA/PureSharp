using System;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.MayBeTransformer {
    public static partial class MayBeTExtensions {
        public static Lazy<MayBe<A>> AsLazyM<A>(this A source) {
            return source.Unit();
        }
        static Lazy<MayBe<A>> Unit<A>(this A source) {
            return source.AsMayBe().AsLazy();
        }
        static Lazy<MayBe<A>> Empty<A>() {
            return MayBe2Extensions.Empty<A>().AsLazy();
        }
        static Lazy<MayBe<B>> SelectMany<A, B>(this Lazy<MayBe<A>> source, Func<A, Lazy<MayBe<B>>> f) {
            return LazyMonad.LazyExtensions.SelectMany<MayBe<A>, MayBe<B>>(
                source,
                x => (x.Value != null ? f(x.Value) : LazyMonad.LazyExtensions.AsLazy(MayBe2Extensions.Empty<B>()))
            );
        }
    }
}

namespace PureSharp.MayBeTransformer2 {
    public interface IMonad {
        object Unit<A>(A value);
        object SelectMany<A, B>(object value, Func<A, object> f);
    }
    public class LazyMonad : IMonad {
        object IMonad.SelectMany<A, B>(object value, Func<A, object> f) {
            return PureSharp.LazyMonad.LazyExtensions.SelectMany<A, B>((Lazy<A>)value, x => (Lazy<B>)f(x));
        }
        object IMonad.Unit<A>(A value) {
            return PureSharp.LazyMonad.LazyExtensions.AsLazy(value);
        }
    }
    public struct MayBeT<A>(object innerMonadicValue, IMonad monad) {
        public object InnerMonadicValue { get; private set; } = innerMonadicValue;
        public IMonad Monad { get; private set; } = monad;
    }
    public static partial class MayBeTExtensions2 {
        //public static MayBeT<TMonad> Lift<TMonad>(TMonad value) {
        //    return new MayBeT<TMonad>(value);
        //}
        static MayBeT<A> Unit<A>(this A source, IMonad monad) {
            return new MayBeT<A>(monad.Unit(source.AsMayBe()), monad);
        }
        static MayBeT<B> SelectMany<A, B>(this MayBeT<A> source, Func<A, MayBeT<B>> f) {
            return (MayBeT<B>)source.Monad.SelectMany<MayBe<A>, MayBe<B>>(
                source.InnerMonadicValue,
                (MayBe<A> x) => {
                    if(x.Value != null) {
                        return f(x.Value);
                    }
                    return Unit<B>(default(B), source.Monad);
                }
            );
        }
    }
}