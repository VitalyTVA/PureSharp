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