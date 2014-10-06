﻿using System;
using NUnit.Framework;
using PureSharp.LazyMonad;
using PureSharp.Tests.Utils;
using LazyMayBeInt = PureSharp.LazyMayBeMonad.LazyMayBe<int?>;

namespace PureSharp.Tests {
    using LazyMayBeMonad;
    [TestFixture]
    public partial class LazyMayBeTests {
        [Test]
        public void MayBeTransfromerTest() {
            LazyMayBeInt q1 = ((int?)1).AsLazyMayBe();
            q1.Value.Value.IsEqual<int?>(1);
            q1 = ((int?)1).AsLazy().AsLazyMayBe();
            q1.Value.Value.IsEqual<int?>(1);

            int value1GetCount = 0;
            int? value1 = null;
            Func<int?> f1 = () => {
                value1GetCount++;
                return value1;
            };

            int value2GetCount = 0;
            int? value2 = null;
            Func<int?> f2 = () => {
                value2GetCount++;
                return value2;
            };

            var res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(1);
            value2GetCount.IsEqual(0);

            value1 = 2;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(2);
            value2GetCount.IsEqual(1);

            value2 = 3;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.IsEqual<int?>(5);
            value1GetCount.IsEqual(3);
            value2GetCount.IsEqual(2);

            value1 = null;
            res = Sum(f1.AsLazyMayBe(), f2.AsLazyMayBe());
            res.Value.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(4);
            value2GetCount.IsEqual(2);

            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.IsEqual<int?>(null);
            value1 = 4;
            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.IsEqual<int?>(7);
            value1 = 2;
            SumWhere(f1.AsLazyMayBe(), f2.AsLazyMayBe()).Value.Value.IsEqual<int?>(null);
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