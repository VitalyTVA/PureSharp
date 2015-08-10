using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.MayBeTransformer2;
using PureSharp.MayBeMonad2;
using PureSharp.LazyMonad;

namespace PureSharp.Tests {
    [TestFixture]
    public partial class MayBeTransfromerTests2 {
        [Test]
        public void MayBeTransfromerTest() {
            int? value1 = null;
            int? value2 = null;

            var res = Sum(value1.AsMayBeT(new MayBeTransformer2.LazyMonad()), value2.AsMayBeT(new MayBeTransformer2.LazyMonad()));
            res.RunMayBeT<int?, Lazy<MayBe<int?>>>().Value.Value.IsEqual<int?>(null);

            value1 = 2;
            res = Sum(value1.AsMayBeT(new MayBeTransformer2.LazyMonad()), value2.AsMayBeT(new MayBeTransformer2.LazyMonad()));
            res.RunMayBeT<int?, Lazy<MayBe<int?>>>().Value.Value.IsEqual<int?>(null);

            value2 = 3;
            res = Sum(value1.AsMayBeT(new MayBeTransformer2.LazyMonad()), value2.AsMayBeT(new MayBeTransformer2.LazyMonad()));
            res.RunMayBeT<int?, Lazy<MayBe<int?>>>().Value.Value.IsEqual<int?>(5);

            //value1 = null;
            //res = Sum(f1.AsLazy(), f2.AsLazy());
            //res.Value.Value.IsEqual<int?>(null);
            //value1GetCount.IsEqual(4);
            //value2GetCount.IsEqual(2);

            //SumWhere(f1.AsLazy(), f2.AsLazy()).Value.Value.IsEqual<int?>(null);
            //value1 = 4;
            //SumWhere(f1.AsLazy(), f2.AsLazy()).Value.Value.IsEqual<int?>(7);
            //value1 = 2;
            //SumWhere(f1.AsLazy(), f2.AsLazy()).Value.Value.IsEqual<int?>(null);
        }
        MayBeT<int?> Sum(MayBeT<int?> a, MayBeT<int?> b) {
            return from x in a
                   from y in b
                   select x + y;
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
    public struct MayBeT<A> {
        public object InnerMonadicValue { get; private set; }
        public IMonad Monad { get; private set; }
        public MayBeT(object innerMonadicValue, IMonad monad) {
            InnerMonadicValue = innerMonadicValue;
            Monad = monad;
        }
    }
    public static partial class MayBeTExtensions2 {
        public static TMonad RunMayBeT<A, TMonad>(this MayBeT<A> source) {
            return (TMonad)source.InnerMonadicValue;
        }
        public static MayBeT<A> AsMayBeT<A>(this A source, IMonad monad) {
            return source.Unit(monad);
        }
        static MayBeT<A> Unit<A>(this A source, IMonad monad) {
            return new MayBeT<A>(monad.Unit(source.AsMayBe()), monad);
        }
        static MayBeT<B> SelectMany<A, B>(this MayBeT<A> source, Func<A, MayBeT<B>> f) {
            object result = source.Monad.SelectMany<MayBe<A>, MayBe<B>>(
                source.InnerMonadicValue,
                (MayBe<A> x) => {
                    if(x.Value != null) {
                        return f(x.Value).InnerMonadicValue;
                    }
                    return source.Monad.Unit(default(B).AsMayBe());
                }
            );
            return new MayBeT<B>(result, source.Monad);
        }

        public static MayBeT<C> SelectMany<A, B, C>(this MayBeT<A> source, Func<A, MayBeT<B>> f, Func<A, B, C> resultSelector) {
            return source.SelectMany(
                outer => f(outer).SelectMany(
                inner => resultSelector(outer, inner).Unit<C>(source.Monad)));
        }
        public static MayBeT<B> Select<A, B>(this MayBeT<A> source, Func<A, B> f) {
            return source.SelectMany(x => f(x).Unit<B>(source.Monad));
        }

    }
}