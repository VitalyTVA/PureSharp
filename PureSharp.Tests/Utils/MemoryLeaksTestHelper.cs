using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PureSharp.Extensions;

namespace PureSharp.Tests.Utils {
    public static class MemoryLeaksTestHelper {
        public static IList<WeakReference> CollectReference(this IList<WeakReference> references, object value) {
            return references.Cons(new WeakReference(value));
        }
        public static void AssertAllCollected(this IList<WeakReference> references) {
            FullCollect();
            foreach(var reference in references.AsEnumerable()) {
                Assert.IsNull(reference.Target);
            }
        }
        public static void AssertAllAlive(this IList<WeakReference> references) {
            FullCollect();
            foreach(var reference in references.AsEnumerable()) {
                Assert.IsNotNull(reference.Target);
            }
        }
        static void FullCollect() {
            GC.GetTotalMemory(true);
            GC.WaitForPendingFinalizers();
            GC.GetTotalMemory(true);
            GC.WaitForPendingFinalizers();
        }
    }
}
