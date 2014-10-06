using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;
using LazyMayBeInt = PureSharp.LazyMayBeMonad2.LazyMayBe<int?>;

namespace PureSharp.Tests {
    using LazyMayBeMonad2;
    using MayBeMonad2;
    using LazyMonad;
    [TestFixture]
    public partial class LazyMayBe2Tests {
        [Test]
        public void MayBeTransfromerTest() {
            int value1GetCount = 0;
            int? value1 = null;
            Func<MayBe<int?>> f1 = () => {
                value1GetCount++;
                return value1.AsMayBe();
            };

            int value2GetCount = 0;
            int? value2 = null;
            Func<MayBe<int?>> f2 = () => {
                value2GetCount++;
                return value2.AsMayBe();
            };

            var res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(1);
            value2GetCount.IsEqual(0);

            value1 = 2;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(2);
            value2GetCount.IsEqual(1);

            value2 = 3;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.Value.IsEqual<int?>(5);
            value1GetCount.IsEqual(3);
            value2GetCount.IsEqual(2);

            value1 = null;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(4);
            value2GetCount.IsEqual(2);

            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.Value.IsEqual<int?>(null);
            value1 = 4;
            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.Value.IsEqual<int?>(7);
            value1 = 2;
            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.Value.IsEqual<int?>(null);
        }
        LazyMayBeInt Sum(LazyMayBeInt a, LazyMayBeInt b) {
            return from x in a
                   from y in b
                   select x + y;
        }
        LazyMayBeInt SumWhere(LazyMayBeInt a, LazyMayBeInt b) {
            return from x in a
                   from y in b
                   where x > y
                   select x + y;
        }
    }
}