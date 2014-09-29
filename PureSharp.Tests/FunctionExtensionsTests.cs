using System;
using NUnit.Framework;
using PureSharp.Tests.Utils;

namespace PureSharp.Tests {
    [TestFixture]
    public class FunctionExtensionsTest {
        [Test]
        public void PartialTest() {
            Func<int, int, int, int> f = (x, y, z) => x + 2 * y + 3 * z;
            f.Partial(100)(2, 30).IsEqual(194);
            f.Partial(400).Partial(3)(10).IsEqual(436);
        }
        [Test]
        public void CurryTest() {
            Func<int, int, int, int> f = (x, y, z) => x + 2 * y + 3 * z;
            f.Curry()(100)(2)(30).IsEqual(194);
            f.Partial(400).Curry()(3)(10).IsEqual(436);
        }
        [Test]
        public void Identity() {
            F.Indentity<int>()(1).IsEqual(1);
        }
    }
}