using System;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.MayBeTransformer {
    public static partial class LazyMayBe2Extensions {
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