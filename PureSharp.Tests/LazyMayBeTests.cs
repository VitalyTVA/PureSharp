using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;
using LazyMayBeInt = System.Lazy<int?>;

namespace PureSharp.Tests {
    using LazyMayBeMonad;
    [TestFixture]
    public partial class LazyMayBeTests {
        Lazy<T> AsLazy<T>(Func<T> lazy) {
            return PureSharp.LazyMonad.LazyExtensions.AsLazy(lazy);
        }
        [Test]
        public void MayBeTransfromerTest() {
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

            var res = Sum(AsLazy(f1), AsLazy(f2));
            res.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(1);
            value2GetCount.IsEqual(0);

            value1 = 2;
            res = Sum(AsLazy(f1), AsLazy(f2));
            res.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(2);
            value2GetCount.IsEqual(1);

            value2 = 3;
            res = Sum(AsLazy(f1), AsLazy(f2));
            res.Value.IsEqual<int?>(5);
            value1GetCount.IsEqual(3);
            value2GetCount.IsEqual(2);

            value1 = null;
            res = Sum(AsLazy(f1), AsLazy(f2));
            res.Value.IsEqual<int?>(null);
            value1GetCount.IsEqual(4);
            value2GetCount.IsEqual(2);

            SumWhere(AsLazy(f1), AsLazy(f2)).Value.IsEqual<int?>(null);
            value1 = 4;
            SumWhere(AsLazy(f1), AsLazy(f2)).Value.IsEqual<int?>(7);
            value1 = 2;
            SumWhere(AsLazy(f1), AsLazy(f2)).Value.IsEqual<int?>(null);
        }
        LazyMayBeInt Sum(LazyMayBeInt a, LazyMayBeInt b) {
            return from x in a
                   from y in b
                   select x + y;
        }
    }
}
namespace PureSharp.Tests {
    using LazyMayBeMonad;
    partial class LazyMayBeTests {
        LazyMayBeInt SumWhere(LazyMayBeInt a, LazyMayBeInt b) {
            return from x in a
                   from y in b
                   where x > y
                   select x + y;
        }
    }
}