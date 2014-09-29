using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;
using PureSharp.LazyMonad;

namespace PureSharp.Tests {
    [TestFixture]
    public class LazyTests {
        [Test]
        public void LazyTest1() {
            int value1 = 0;
            Func<int> f1 = () => value1;
            int value2 = 0;
            Func<int> f2 = () => value2;

            var t1 = from a in f1.AsLazy()
                    select a * 2;
            value1 = 1;
            t1.Value.IsEqual(2);

            var t2 = from a in f1.AsLazy()
                     from b in f2.AsLazy()
                     select a + b;
            value1 = 2;
            value2 = 3;
            t2.Value.IsEqual(5);

            (from a in 1.AsLazy()
             from b in 2.AsLazy()
             select a + b).IsEqual(x => x.Value, 3);
        }
    }
}